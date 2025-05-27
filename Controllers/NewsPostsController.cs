using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApexVolley.Data;
using ApexVolley.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace ApexVolley.Controllers
{
    public class NewsPostsController : Controller
    {
        private readonly ApexVolleyContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NewsPostsController(ApexVolleyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: NewsPosts
        public async Task<IActionResult> Index()
        {
            return View(await _context.NewsPost.ToListAsync());
        }

        // GET: NewsPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsPost = await _context.NewsPost
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsPost == null)
            {
                return NotFound();
            }

            return View(newsPost);
        }

        // GET: NewsPosts/Create
        [Authorize(Roles = "Staff,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Content")] NewsPost newsPost,
            IFormFile MainImage,
            IFormFile[] AdditionalImages,
            IFormFile[] AttachmentFiles)
        {
            if (ModelState.IsValid)
                
            {
                newsPost.PublishedAt = DateTime.Now;
                // Salva l'immagine principale
                if (MainImage != null && MainImage.Length > 0)
                {
                    newsPost.MainImagePath = await SaveFileAsync(MainImage, "images");
                }

                // Salva immagini aggiuntive
                if (AdditionalImages != null && AdditionalImages.Length > 0)
                {
                    var imagePaths = new List<string>();
                    foreach (var image in AdditionalImages)
                    {
                        if (image.Length > 0)
                        {
                            var imagePath = await SaveFileAsync(image, "images");
                            imagePaths.Add(imagePath);
                        }
                    }
                    newsPost.AdditionalImagePaths = string.Join(";", imagePaths);
                }

                // Salva file allegati
                if (AttachmentFiles != null && AttachmentFiles.Length > 0)
                {
                    var attachmentPaths = new List<string>();
                    foreach (var file in AttachmentFiles)
                    {
                        if (file.Length > 0)
                        {
                            var filePath = await SaveFileAsync(file, "attachments");
                            attachmentPaths.Add($"{file.FileName}|{filePath}");
                        }
                    }
                    newsPost.AttachmentPaths = string.Join(";", attachmentPaths);
                }

                _context.Add(newsPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newsPost);
        }

        // GET: NewsPosts/Edit/5
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsPost = await _context.NewsPost.FindAsync(id);
            if (newsPost == null)
            {
                return NotFound();
            }
            return View(newsPost);
        }

        // POST: NewsPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,PublishedAt,MainImagePath,AdditionalImagePaths,AttachmentPaths")] NewsPost newsPost,
            IFormFile MainImage,
            IFormFile[] AdditionalImages,
            IFormFile[] AttachmentFiles)
        {
            if (id != newsPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Recupera il record esistente per mantenere i file attuali
                    var existingNewsPost = await _context.NewsPost.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);

                    // Aggiorna l'immagine principale solo se viene caricata una nuova
                    if (MainImage != null && MainImage.Length > 0)
                    {
                        // Elimina l'immagine precedente se esiste
                        if (!string.IsNullOrEmpty(existingNewsPost.MainImagePath))
                        {
                            DeleteFile(existingNewsPost.MainImagePath);
                        }
                        newsPost.MainImagePath = await SaveFileAsync(MainImage, "images");
                    }
                    else
                    {
                        // Mantieni l'immagine esistente
                        newsPost.MainImagePath = existingNewsPost.MainImagePath;
                    }

                    // Aggiungi nuove immagini aggiuntive (mantieni quelle esistenti)
                    var existingAdditionalImages = existingNewsPost.AdditionalImagePaths ?? "";
                    if (AdditionalImages != null && AdditionalImages.Length > 0)
                    {
                        var newImagePaths = new List<string>();
                        foreach (var image in AdditionalImages)
                        {
                            if (image.Length > 0)
                            {
                                var imagePath = await SaveFileAsync(image, "images");
                                newImagePaths.Add(imagePath);
                            }
                        }

                        var allImagePaths = new List<string>();
                        if (!string.IsNullOrEmpty(existingAdditionalImages))
                        {
                            allImagePaths.AddRange(existingAdditionalImages.Split(';'));
                        }
                        allImagePaths.AddRange(newImagePaths);

                        newsPost.AdditionalImagePaths = string.Join(";", allImagePaths);
                    }
                    else
                    {
                        newsPost.AdditionalImagePaths = existingAdditionalImages;
                    }

                    // Aggiungi nuovi file allegati (mantieni quelli esistenti)
                    var existingAttachments = existingNewsPost.AttachmentPaths ?? "";
                    if (AttachmentFiles != null && AttachmentFiles.Length > 0)
                    {
                        var newAttachmentPaths = new List<string>();
                        foreach (var file in AttachmentFiles)
                        {
                            if (file.Length > 0)
                            {
                                var filePath = await SaveFileAsync(file, "attachments");
                                newAttachmentPaths.Add($"{file.FileName}|{filePath}");
                            }
                        }

                        var allAttachmentPaths = new List<string>();
                        if (!string.IsNullOrEmpty(existingAttachments))
                        {
                            allAttachmentPaths.AddRange(existingAttachments.Split(';'));
                        }
                        allAttachmentPaths.AddRange(newAttachmentPaths);

                        newsPost.AttachmentPaths = string.Join(";", allAttachmentPaths);
                    }
                    else
                    {
                        newsPost.AttachmentPaths = existingAttachments;
                    }

                    _context.Update(newsPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsPostExists(newsPost.Id))
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
            return View(newsPost);
        }

        // GET: NewsPosts/Delete/5
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsPost = await _context.NewsPost
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsPost == null)
            {
                return NotFound();
            }

            return View(newsPost);
        }

        // POST: NewsPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsPost = await _context.NewsPost.FindAsync(id);
            if (newsPost != null)
            {
                // Elimina tutti i file associati
                if (!string.IsNullOrEmpty(newsPost.MainImagePath))
                {
                    DeleteFile(newsPost.MainImagePath);
                }

                if (!string.IsNullOrEmpty(newsPost.AdditionalImagePaths))
                {
                    var imagePaths = newsPost.AdditionalImagePaths.Split(';');
                    foreach (var imagePath in imagePaths)
                    {
                        DeleteFile(imagePath);
                    }
                }

                if (!string.IsNullOrEmpty(newsPost.AttachmentPaths))
                {
                    var attachmentPaths = newsPost.AttachmentPaths.Split(';');
                    foreach (var attachmentPath in attachmentPaths)
                    {
                        var parts = attachmentPath.Split('|');
                        if (parts.Length > 1)
                        {
                            DeleteFile(parts[1]); // parts[1] è il percorso del file
                        }
                    }
                }

                _context.NewsPost.Remove(newsPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsPostExists(int id)
        {
            return _context.NewsPost.Any(e => e.Id == id);
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folderName);

            // Crea la cartella se non esiste
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Genera un nome file univoco
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Salva il file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Ritorna il percorso relativo per il salvataggio nel database
            return Path.Combine("uploads", folderName, fileName).Replace("\\", "/");
        }

        private void DeleteFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }
    }
}