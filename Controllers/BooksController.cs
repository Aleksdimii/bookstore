using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bookstore.Data;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using bookstore.Models;

namespace bookstore.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Allow anyone to see the list of books
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var books = await _context.Book.ToListAsync();
            var model = _mapper.Map<List<BookViewModel>>(books);
            return View(model);
        }

        // Allow anyone to see book details
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await _context.Book.FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
                return NotFound();

            var model = _mapper.Map<BookViewModel>(book);
            return View(model);
        }

        // Only Admins can access create actions
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var book = _mapper.Map<Book>(model);
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Only Admins can access edit actions
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await _context.Book.FindAsync(id);
            if (book == null)
                return NotFound();

            var model = _mapper.Map<BookViewModel>(book);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookViewModel model)
        {
            if (id != model.BookId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var book = _mapper.Map<Book>(model);
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(model.BookId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Only Admins can access delete actions
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await _context.Book.FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
                return NotFound();

            var model = _mapper.Map<BookViewModel>(book);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
                _context.Book.Remove(book);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }

    }
}
