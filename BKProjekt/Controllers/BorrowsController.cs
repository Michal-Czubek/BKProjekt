using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BKProjekt.Areas.Identity.Data;
using BKProjekt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace BKProjekt.Controllers
{
    
    public class BorrowsController : Controller
    {
        
        private readonly BKProjektDbContext _context;
        private readonly UserManager<BKProjektUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BorrowsController(BKProjektDbContext context, UserManager<BKProjektUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Borrows
        [Authorize(Roles = "keeper, admin")]
        public async Task<IActionResult> Index()
        {
            var borrows = await _context.Borrows.OrderByDescending(b => b.BorrowDate).ToListAsync();
            return View(borrows);

        }

        // GET: Borrows/Details/5
        [Authorize(Roles = "keeper, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null)
            {
                return NotFound();
            }

            return View(borrow);
        }

        // GET: Borrows/Create
        [Authorize(Roles = "keeper, admin")]
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Author");
            return View();
        }

        // POST: Borrows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,UserId,BorrowDate,ReturnDate,Status")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(borrow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Author", borrow.BookId);
            return View(borrow);
        }

        // GET: Borrows/Edit/5
        [Authorize(Roles = "keeper, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Author", borrow.BookId);
            return View(borrow);
        }

        // POST: Borrows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "keeper, admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,UserId,BorrowDate,ReturnDate,Status")] Borrow borrow)
        {
            if (id != borrow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowExists(borrow.Id))
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
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Author", borrow.BookId);
            return View(borrow);
        }

        // GET: Borrows/Delete/5
        [Authorize(Roles = "keeper, admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null)
            {
                return NotFound();
            }

            return View(borrow);
        }

        // POST: Borrows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "keeper, admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow != null)
            {
                _context.Borrows.Remove(borrow);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowExists(int id)
        {
            return _context.Borrows.Any(e => e.Id == id);
        }


        // GET: Borrows/Allowed/5
        public async Task<IActionResult> Allowed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow == null)
            {
                return NotFound();
            }

            
            borrow.Status = "Wypożyczona";

            // Znalezienie powiązanej książki i zmiana jej statusu na "Allowed"
            var book = await _context.Book.FirstOrDefaultAsync(b => b.Id == borrow.BookId);
            if (book != null)
            {
                book.Status = "Wypożyczona";
            }

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
        // GET: Borrows/Forbid/5
        public async Task<IActionResult> Forbid(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow == null)
            {
                return NotFound();
            }


            borrow.Status = "Niewypożyczono";

            // Znalezienie powiązanej książki i zmiana jej statusu na "Allowed"
            var book = await _context.Book.FirstOrDefaultAsync(b => b.Id == borrow.BookId);
            if (book != null)
            {
                book.Status = "Nie";
            }

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
    }
}
