using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseApproval.Data;
using ExpenseApproval.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ExpenseApproval
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public ExpensesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
             _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Expenses
        [Authorize(Roles = "Admin,NormalUser")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            if (!role.Contains("Admin"))
            {
                return View(await _context.Expense.Where(a=>a.ExpenseRequesterID==user.Id).ToListAsync());
            }
            else
            {
                return View(await _context.Expense.ToListAsync());

            }

        }

        // GET: Expenses/Details/5
        [Authorize(Roles = "Admin,NormalUser")]
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
           var role= await _userManager.GetRolesAsync(user);
            string currentUserId = user.Id;
            if (id == null)
            {
                return NotFound();
            }
            Expense expense;
            if (!role.Contains("Admin"))
            {
                expense = await _context.Expense
                    .FirstOrDefaultAsync(m => m.ExpenseId == id && m.ExpenseRequesterID == user.Id);
            }
            expense = await _context.Expense
                    .FirstOrDefaultAsync(m => m.ExpenseId == id);
            
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expenses/Create
        [Authorize(Roles = "Admin,NormalUser")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,NormalUser")]
        public async Task<IActionResult> Create([Bind("ExpenseId,Description,Amount,FileName,Date")] Expense expense)
        {
            var user = await _userManager.GetUserAsync(User);
            expense.ExpenseRequesterID= user.Id;
            expense.UpdatedBy = user.Id;
            expense.Status = ExpenseStatus.Submitted;
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        // GET: Expenses/Edit/5


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            Expense expense;
            //if (!role.Contains("Admin"))
            //{
            //    expense = await _context.Expense
            //        .FirstOrDefaultAsync(m => m.ExpenseId == id && m.ExpenseRequesterID == user.Id);
            //}
             expense = await _context.Expense.FindAsync(id);
            
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseId,Description,Amount,FileName,Date,Status")] Expense expense)
        {
            if (id != expense.ExpenseId)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            expense.UpdatedBy = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                   //if (!role.Contains("Admin")&& user.Id== expense.ExpenseRequesterID)
                    {
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.ExpenseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        // GET: Expenses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            Expense expense;
            //if (!role.Contains("Admin"))
            //{
            //    expense = await _context.Expense
            //       .FirstOrDefaultAsync(m => m.ExpenseId == id&& m.ExpenseRequesterID==user.Id);
            //}
           expense = await _context.Expense
                .FirstOrDefaultAsync(m => m.ExpenseId == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);
            Expense expense;
            //if (!role.Contains("Admin"))
            //{
            //    expense = await _context.Expense.FirstOrDefaultAsync(m => m.ExpenseId == id && m.ExpenseRequesterID == user.Id);
            //}
             expense = await _context.Expense.FindAsync(id);

           
                _context.Expense.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expense.Any(e => e.ExpenseId == id);
        }
    }
}
