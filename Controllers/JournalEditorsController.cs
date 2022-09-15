using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EDSU_JournalMS.Data;
using EDSU_JournalMS.Models;
using EDSU_JournalMS.Views.Journals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EDSU_JournalMS.Authorization;

namespace EDSU_JournalMS.Controllers
{
    public class JournalEditorsController : DI_BaseViewModel
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public JournalEditorsController(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager, IWebHostEnvironment hostingEnvironment) : base(context, authorizationService, userManager)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: JournalEditors
        public async Task<IActionResult> Index()
        {
              return Context.JournalEditors != null ? 
                          View(await Context.JournalEditors.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.JournalEditors'  is null.");
        }

        // GET: JournalEditors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || Context.JournalEditors == null)
            {
                return NotFound();
            }

            var journalEditor = await Context.JournalEditors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (journalEditor == null)
            {
                return NotFound();
            }

            return View(journalEditor);
        }

        // GET: JournalEditors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JournalEditors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile image, JournalEditor journalEditor)
        {
            var isManager = User.IsInRole(Constants.ArticleReviewerRole);
            var isAdmin = User.IsInRole(Constants.ArticleSuperAdminRole);
            //checking if an applicant has applied before
           // var currentUser = UserManager.GetUserId(User);

            if (isAdmin != true)
            {
                return Forbid();
            }
            var editor = UserManager.GetUserId(User);
            Context.Add(journalEditor);

            //if (ModelState.IsValid)
            //{

            //    Context.Add(Article);
            //    await Context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            try
            {
                
                if (image != null && image.Length > 0)
                {
                    var uploadDir = @"files/EditorImages";
                    var fileName = Path.GetFileNameWithoutExtension(image.FileName);
                    var extension = Path.GetExtension(image.FileName);
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    //fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;

                    fileName = fileName + extension;
                    var path = Path.Combine(webRootPath, uploadDir, fileName);
                    using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                    {
                        image.CopyTo(fs);
                        journalEditor.Image = "/" + uploadDir + "/" + fileName;
                    }

                }
                //  Context.Add(journalEditor);
                await TryUpdateModelAsync<JournalEditor>(journalEditor);               //    c => c.Ssce1, c => c.Ssce2, c => c.BirthCertificate, c => c.DirectEntryUpload, c => c.LGAUpload))

                try
                {
                    await Context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {

                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();

            }
            return RedirectToAction(nameof(Index));

        }

        // GET: JournalEditors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || Context.JournalEditors == null)
            {
                return NotFound();
            }

            var journalEditor = await Context.JournalEditors.FindAsync(id);
            if (journalEditor == null)
            {
                return NotFound();
            }
            return View(journalEditor);
        }

        // POST: JournalEditors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,OtherName,Title,AreaOfSpecialization,Email,Tel,Image,Institution")] JournalEditor journalEditor)
        {
            if (id != journalEditor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Context.Update(journalEditor);
                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JournalEditorExists(journalEditor.Id))
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
            return View(journalEditor);
        }

        // GET: JournalEditors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || Context.JournalEditors == null)
            {
                return NotFound();
            }

            var journalEditor = await Context.JournalEditors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (journalEditor == null)
            {
                return NotFound();
            }

            return View(journalEditor);
        }

        // POST: JournalEditors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Context.JournalEditors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JournalEditors'  is null.");
            }
            var journalEditor = await Context.JournalEditors.FindAsync(id);
            if (journalEditor != null)
            {
                Context.JournalEditors.Remove(journalEditor);
            }
            
            await Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JournalEditorExists(int id)
        {
          return (Context.JournalEditors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
