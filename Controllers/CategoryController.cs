using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;
namespace Expense_Tracker.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (_context.Categories != null)
            {
                return View(await _context.Categories.ToListAsync());
            }
            else
            {
                return Problem("Entity set 'ApplicationDbContext.Categories' is null.");
            }

        }

        [HttpGet]
       

   /*     [HttpPost]
        public IActionResult Create(Categories category)
        {
            if (ModelState.IsValid)
            {
               
                _context.Categories.Add(category);
                _context.SaveChanges();

                
                return RedirectToAction("Index");
            }

            
            return View(category);
        }*/
   [HttpGet]
        public IActionResult AddorEdit(int id = 0)
        {
            if (id == 0)
                return View(new Categories());
            else
                return View(_context.Categories.Find(id));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddorEdit([Bind("CategoryId,Title,Icon,Type")] Categories category)
        {
            if (ModelState.IsValid)
            {
                if (category.CategoryId == 0)
                    _context.Add(category);
                else
                    _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
