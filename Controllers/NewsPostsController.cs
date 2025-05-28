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
        public async Task<IActionResult> Create([Bind("Id,Title,Content, PublishedAt")] NewsPost newsPost , IFormFile MainImage, IFormFile[] AdditionalImages, IFormFile[] AttachmentFiles)

        {
            // Logica per la data di pubblicazione:
            // Se l'utente non ha specificato una data (PublishedAt è il valore di default DateTime.MinValue),
            // allora impostiamo la data di pubblicazione a oggi (solo la data, senza l'ora).
            // Altrimenti, se l'utente ha specificato una data, usiamo quella, assicurandoci
            // di prendere solo la parte della data.
            if (newsPost.PublishedAt == DateTime.MinValue)
            {
                newsPost.PublishedAt = DateTime.Today; // Imposta alla data odierna (00:00:00)
            }
            else
            {
                // Assicura che venga salvata solo la parte della data, rimuovendo l'eventuale componente oraria
                newsPost.PublishedAt = (newsPost.PublishedAt ?? DateTime.Now).Date;
            }

            // Rimuoviamo esplicitamente l'errore di validazione per l'ID se presente,
            // poiché per la creazione l'ID non è fornito dall'utente ma generato dal DB.
            // Questo è utile se il modello ha qualche validazione sull'ID che non si applica qui.
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                // Non c'è bisogno di impostare nuovamente newsPost.PublishedAt qui,
                // è già stato gestito sopra.

                // Salva l'immagine principale
                if (MainImage != null && MainImage.Length > 0)
                {
                    // Valida la dimensione e il tipo del file se necessario
                    // Esempio: if (MainImage.Length > 5 * 1024 * 1024) { ModelState.AddModelError("MainImage", "L'immagine principale supera i 5MB."); }
                    // Esempio: if (!new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(Path.GetExtension(MainImage.FileName).ToLowerInvariant())) { ModelState.AddModelError("MainImage", "Formato immagine non supportato."); }
                    if (ModelState.IsValid) // Ricontrolla dopo validazioni aggiuntive sui file
                    {
                        newsPost.MainImagePath = await SaveFileAsync(MainImage, "images");
                    }
                }

                // Salva immagini aggiuntive
                if (AdditionalImages != null && AdditionalImages.Length > 0)
                {
                    var imagePaths = new List<string>();
                    foreach (var image in AdditionalImages)
                    {
                        if (image.Length > 0)
                        {
                            // Aggiungere validazioni simili a MainImage se necessario
                            var imagePath = await SaveFileAsync(image, "images");
                            imagePaths.Add(imagePath);
                        }
                    }
                    if (imagePaths.Any()) // Solo se sono state aggiunte immagini valide
                    {
                        newsPost.AdditionalImagePaths = string.Join(";", imagePaths);
                    }
                }

                // Salva file allegati
                if (AttachmentFiles != null && AttachmentFiles.Length > 0)
                {
                    var attachmentPaths = new List<string>();
                    foreach (var file in AttachmentFiles)
                    {
                        if (file.Length > 0)
                        {
                            // Aggiungere validazioni simili a MainImage se necessario (es. dimensione max 10MB)
                            var filePath = await SaveFileAsync(file, "attachments");
                            attachmentPaths.Add($"{file.FileName}|{filePath}"); // Salva nome originale e percorso
                        }
                    }
                    if (attachmentPaths.Any()) // Solo se sono stati aggiunti allegati validi
                    {
                        newsPost.AttachmentPaths = string.Join(";", attachmentPaths);
                    }
                }

                // Ricontrolla ModelState.IsValid nel caso in cui le validazioni dei file abbiano aggiunto errori
                if (!ModelState.IsValid)
                {
                    return View(newsPost); // Ritorna alla view con i messaggi di errore
                }

                _context.Add(newsPost);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "News creata con successo!"; // Messaggio di successo opzionale
                return RedirectToAction(nameof(Index));
            }

            // Se il ModelState non è valido (es. campi obbligatori mancanti per Title o Content),
            // ritorna alla view con i dati inseriti e i messaggi di errore.
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