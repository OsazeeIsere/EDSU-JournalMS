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
using static EDSU_JournalMS.Models.Enum;

namespace EDSU_JournalMS.Controllers
{
    public class JournalsController : DI_BaseViewModel
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public JournalsController(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager, IWebHostEnvironment hostingEnvironment) : base(context, authorizationService, userManager)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public FileResult DownloadFile(string FileName)
        {
            string path = Path.Combine(this._hostingEnvironment.WebRootPath, "files/Articles/") + FileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", FileName);
        }
        // GET: Journals
        public async Task<IActionResult> Index()
        {
            var currentUser = UserManager.GetUserName(User);
            var editors = from i in Context.JournalEditors
                             select i;
            editors = editors.Where(i => i.EditorEmail == currentUser);
            var editorId = from c in editors select c.Id;
            if (editors.Any())
            {

                List<EDSUJournal> article = new List<EDSUJournal>();
                article = (from c in Context.EDSUJournals select c).ToList();
                foreach(EDSUJournal journal in article)
                    if ((journal.FirstReviewer != null) && journal.FirstReviewer == editorId.ToString())

                        //if ((journal.FirstReviewer!=null)&&journal.FirstReviewer == editorId.ToString()|| (journal.SecondReviewer!=null)&& journal.SecondReviewer == editorId.ToString()                       
                        //                        || (journal.ThirdReviewer!=null)&& journal.ThirdReviewer == editorId.ToString()|| (journal.FourthReviewer!=null)&& journal.FourthReviewer == editorId.ToString()||(journal.FifthReviewer!=null)&& journal.fi == editorId.ToString())
                    {
                        article.Add(journal);
                        return View(article);

                    }
                //var articles = from i in Context.EDSUJournals
                //               select i;
                //articles = articles.Where(i => i.FirstReviewer. == currentUser);

                //if (article(currentUser)) { }
                //var isManager = User.IsInRole(Constants.ArticleReviewerRole);
                //var isAdmin = User.IsInRole(Constants.ArticleSuperAdminRole);
            }

            //checking if an applicant has applied before

            //if (isManager == true && isAdmin == false)
            //{
            //  //  article = article.Where(i => i. == currentUser);
            //}

            return Context.EDSUJournals != null ? 
                          View(await Context.EDSUJournals.Include(x=> x.JournalEditors).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Journals'  is null.");
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id, ArticleStatus status)
        {

            var articleOperation = (status.ToString() == ArticleOperations.Accepted.ToString())
                ? ArticleOperations.Accepted
                : ArticleOperations.Rejected;
                        var article = await Context.EDSUJournals.FindAsync(id);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, article, articleOperation);

            if (isAuthorized.Succeeded == false)
                return Forbid();

            var JournalToUpdate = await Context.EDSUJournals
                .FirstOrDefaultAsync(c => c.Id == id);

            if (await TryUpdateModelAsync<EDSUJournal>(JournalToUpdate, "", c => c.FirstReviewer, c => c.SecondReviewer,
                c => c.ThirdReviewer, c => c.FourthReviewer, c => c.FifthReviewer,c => c.Status))

            {
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
                return RedirectToAction(nameof(Index));
            }
            return View();

            //if (id == null || Context.Articles == null)
            //{
            //    return NotFound();
            //}

            //var Article = await Context.Articles
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (Article == null)
            //{
            //    return NotFound();
            //}

            //return View(Article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            List<JournalEditor> reviewer = new List<JournalEditor>();
            reviewer = (from c in Context.JournalEditors select c).ToList();
            reviewer.Insert(0, new JournalEditor
            {
                Id = 0,
                FirstName = "--Select Editor--"
            });
            ViewBag.message = reviewer;
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile FullArticle, EDSUJournal Article)
        {
            var uploader = UserManager.GetUserId(User);
            Context.Add(Article);

            //if (ModelState.IsValid)
            //{

            //    Context.Add(Article);
            //    await Context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            try
            {
                var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Article, ArticleOperations.Create);//using the same application Aothorizatin handler
                if (isAuthorized.Succeeded == false)
                    return Forbid();
                if (FullArticle != null && FullArticle.Length > 0)
                {
                    var uploadDir = @"files/Articles";
                    var fileName = Path.GetFileNameWithoutExtension(FullArticle.FileName);
                    var extension = Path.GetExtension(FullArticle.FileName);
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    //fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;

                    fileName = fileName + extension;
                    var path = Path.Combine(webRootPath, uploadDir, fileName);
                    using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                    {
                        FullArticle.CopyTo(fs);
                        Article.Upload =fileName;
                    }

                }
                //  Context.Add(Article);
                await TryUpdateModelAsync<EDSUJournal>(Article);               //    c => c.Ssce1, c => c.Ssce2, c => c.BirthCertificate, c => c.DirectEntryUpload, c => c.LGAUpload))

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

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || Context.EDSUJournals == null)
            {
                return NotFound();
            }
            var article = await Context.EDSUJournals.FindAsync(id);
            if (article == null)
                return NotFound();
            List<JournalEditor> reviewer = new List<JournalEditor>();
            reviewer = (from c in Context.JournalEditors select c).ToList();
            reviewer.Insert(0, new JournalEditor
            {
                Id = 0,
                FirstName = "--Select Editor--"
            });
            ViewBag.message = reviewer;

           if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EDSUJournal Article)
        {
            var article = await Context.EDSUJournals.FindAsync(id);

            if (id != Article.Id)
            {
                return NotFound();
            }

            var JournalToUpdate = await Context.EDSUJournals
             .FirstOrDefaultAsync(c => c.Id == id);

            if (await TryUpdateModelAsync<EDSUJournal>(JournalToUpdate, "", c => c.FirstReviewer, c => c.SecondReviewer,
                c => c.ThirdReviewer, c => c.FourthReviewer, c => c.FifthReviewer, c => c.Status))

            {
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
                return RedirectToAction(nameof(Index));
            }
              return View();
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || Context.EDSUJournals == null)
            {
                return NotFound();
            }

            var Article = await Context.EDSUJournals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Article == null)
            {
                return NotFound();
            }

            return View(Article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Context.EDSUJournals == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Articles'  is null.");
            }
            var Article = await Context.EDSUJournals.FindAsync(id);
            if (Article != null)
            {
                Context.EDSUJournals.Remove(Article);
            }
            
            await Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
          return (Context.EDSUJournals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
