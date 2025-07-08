using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bookstore.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using bookstore.Models;



public class AuthorsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AuthorsController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var authors = await _context.Author.ToListAsync();
        var model = _mapper.Map<List<AuthorViewModel>>(authors);
        return View(model);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var author = await _context.Author.FirstOrDefaultAsync(m => m.AuthorId == id);
        if (author == null) return NotFound();

        var model = _mapper.Map<AuthorViewModel>(author);
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Authors/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AuthorViewModel model)
    {
        if (ModelState.IsValid)
        {
            var author = _mapper.Map<Author>(model);
            _context.Add(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var author = await _context.Author.FindAsync(id);
        if (author == null) return NotFound();

        var model = _mapper.Map<AuthorViewModel>(author);
        return View(model);
    }

    // POST: Authors/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AuthorViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var author = await _context.Author.FindAsync(id);
        if (author == null) return NotFound();
        {
            _mapper.Map(model, author);
        }
        try
        {
            _context.Update(author);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthorExists(id)) return NotFound();
            else throw;
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: Authors/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        {
            var author = await _context.Author.FirstOrDefaultAsync(m => m.AuthorId == id);

            if (author == null) return NotFound();
            {
                var model = _mapper.Map<AuthorViewModel>(author);


                return View(model);
            }

        }
    }

            [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var author = await _context.Author.FindAsync(id);
        if (author != null)
        {
            _context.Author.Remove(author);
            await _context.SaveChangesAsync();
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AuthorExists(int id)
    {
        return _context.Author.Any(e => e.AuthorId == id);
    }
}
