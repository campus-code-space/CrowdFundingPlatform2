using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EndeKisse2.Data;
using EndeKissie2.Models;

namespace EndeKisse2.Controllers
{
    public class ImageStoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImageStoreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ImageStore
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ImageStore.Include(i => i.Project).Include(i => i.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ImageStore/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageStore = await _context.ImageStore
                .Include(i => i.Project)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageStore == null)
            {
                return NotFound();
            }

            return View(imageStore);
        }

        // GET: ImageStore/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Category");
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            return View();
        }

        // POST: ImageStore/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageUrl,UserId,ProjectId")] ImageStore imageStore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imageStore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Category", imageStore.ProjectId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", imageStore.UserId);
            return View(imageStore);
        }

        // GET: ImageStore/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageStore = await _context.ImageStore.FindAsync(id);
            if (imageStore == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Category", imageStore.ProjectId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", imageStore.UserId);
            return View(imageStore);
        }

        // POST: ImageStore/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageUrl,UserId,ProjectId")] ImageStore imageStore)
        {
            if (id != imageStore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageStore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageStoreExists(imageStore.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Category", imageStore.ProjectId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", imageStore.UserId);
            return View(imageStore);
        }

        // GET: ImageStore/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageStore = await _context.ImageStore
                .Include(i => i.Project)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imageStore == null)
            {
                return NotFound();
            }

            return View(imageStore);
        }

        // POST: ImageStore/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageStore = await _context.ImageStore.FindAsync(id);
            if (imageStore != null)
            {
                _context.ImageStore.Remove(imageStore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageStoreExists(int id)
        {
            return _context.ImageStore.Any(e => e.Id == id);
        }
    }
}
