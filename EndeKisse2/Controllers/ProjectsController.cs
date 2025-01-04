using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EndeKisse2.Data;
using EndeKissie2.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EndeKisse2.Services;
using Microsoft.CodeAnalysis;

namespace EndeKisse2.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = from projects in _context.Project
                                       select projects;
            applicationDbContext = applicationDbContext.Where(p => p.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            return View(await applicationDbContext.Include(p => p.User).ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }
             ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EndeKissie2.Models.Project project, Progress progress, ProjectStatus projectStatus)
        {
            if (ModelState.IsValid)
            {

                if (project.ImageFile1 != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + project.ImageFile1.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await project.ImageFile1.CopyToAsync(fileStream);
                    }
                    project.ImageUrl1 = $"~/images/{uniqueFileName}";
                }


                _context.Add(project);   // do the Add() funtion set the db ?? 
                await _context.SaveChangesAsync();

                progress.ProjectId = project.Id;
                progress.ProgressLevel = 0;
                progress.Description = "Project Intiated";

                projectStatus.ProjectId = project.Id;
                projectStatus.Banned = false;   
                projectStatus.Funded = false;

                

                _context.Add(progress);
                _context.Add(projectStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", project.UserId);
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", project.UserId);
            return View(project);
        }

        public void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) {}
            else
            {
                // Convert the URL to a physical file path
                string webRootPath = _webHostEnvironment.WebRootPath;
                string fullPath = Path.Combine(webRootPath, imageUrl.TrimStart('/'));

                // Check if the file exists
                if (System.IO.File.Exists(fullPath))
                {
                    // Delete the file
                    System.IO.File.Delete(fullPath);
                }
                else { }
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
            }
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EndeKissie2.Models.Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string? prevImageUrl = await _context.Project
                                            .Where(i => i.Id == id && !string.IsNullOrEmpty(i.ImageUrl1))
                                            .OrderByDescending(i => i.Id) // Assuming Id is an auto-incrementing primary key
                                            .Select(i => i.ImageUrl1)
                                            .FirstOrDefaultAsync();
                project.ImageUrl1 = prevImageUrl;
                try
                {
                    if (project.ImageFile1 != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + project.ImageFile1.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await project.ImageFile1.CopyToAsync(fileStream);
                        }
                        project.ImageUrl1 = $"~/images/{uniqueFileName}";
                    }
                   
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", project.UserId);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project != null)
            {
                _context.Project.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.Id == id);
        }
    }
}
