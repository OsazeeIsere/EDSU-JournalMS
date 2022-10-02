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
            var currentUserId = UserManager.GetUserId(User);

            var editorId = (from c in Context.JournalEditors where c.EditorEmail.Equals(currentUser) select c).ToList();
            List<EDSUJournal> article = new List<EDSUJournal>();

            foreach (var editor in editorId)
            {
                  article = (from c in Context.EDSUJournals where c.FirstReviewer.Equals(editor.Id.ToString())|| c.SecondReviewer.Equals(editor.Id.ToString())|| c.ThirdReviewer.Equals(editor.Id.ToString())|| c.FourthReviewer.Equals(editor.Id.ToString())|| c.FifthReviewer.Equals(editor.Id.ToString()) select c).ToList();
                if(article.Count > 0)
                {
                    return View(article);
                }

            }
            var journals = from i in Context.EDSUJournals
                             select i;
            if (journals.Any())
            {
                journals = journals.Where(i => i.UploaderId == currentUserId);
                if (journals.Any())
                {
                    return View(journals);
                }

            }
            var isManager = User.IsInRole(Constants.ArticleReviewerRole);
            var isAdmin = User.IsInRole(Constants.ArticleSuperAdminRole);
            //checking if an applicant has applied before

            if (isAdmin != false)
            {
                return View(await Context.EDSUJournals.Include(x => x.JournalEditors).ToListAsync());

            }

            return Context.EDSUJournals != null ? 
                          View(await Context.EDSUJournals.Include(x=> x.JournalEditors).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Journals'  is null.");
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {

             var article = await Context.EDSUJournals.FindAsync(id);
            var isManager = User.IsInRole(Constants.ArticleReviewerRole);
            var isAdmin = User.IsInRole(Constants.ArticleSuperAdminRole);
            if (isAdmin != false)
                return View(article);
                        
            return View();

          
        }
        
        // Creating a modal form to display abstact
        public async Task<IActionResult> Abstract(int? id)
        {
            var article = await Context.EDSUJournals.FindAsync(id);
            ViewBag.Abstract=article.Abstract;
            return PartialView("_Abstract");
        }

        //Post: Applicant/Detail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int? id, ArticleStatus status)
        {
            var article = await Context.EDSUJournals.FindAsync(id);

            if (article == null)
                return NotFound();


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
                //var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Article, ArticleOperations.Create);//using the same application Aothorizatin handler
                //if (isAuthorized.Succeeded == false)
                //    return Forbid();
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
                        Article.UploaderId = uploader;
                    }

                }
                
                try
                {
                    await Context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

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

        // GET: Articles/Edit/5
        public async Task<IActionResult> Comment(int? id)
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
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(int id, string articleAbstract, string reviewerNo,string commentBody, EDSUJournal Article)
        {
            var currentUser = UserManager.GetUserName(User);

            List<Comment> newComment = new List<Comment>() { new Comment() { ReviewerEmail =currentUser, Title=Article.Title, Abstract= articleAbstract, ReviewerNo= reviewerNo, JournalId = id, CommentBody = commentBody, CommentDate = DateTime.Now }};
            if (newComment != null)
            {
                Context.Comments.UpdateRange(newComment);
            }
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
        // GET: Journals
        public async Task<IActionResult> ViewComment()
        {
            var currentUser = UserManager.GetUserName(User);
            var currentUserId = UserManager.GetUserId(User);

            var journalId = (from c in Context.EDSUJournals where c.UploaderId==(currentUserId) select c).ToList();
            List<Comment> comment = new List<Comment>();
            List<EDSUJournal> article = new List<EDSUJournal>();
            if (journalId.Any()) 
            { 
            
            foreach (var journal in journalId)
            {
               // View(await _context.Courses.Include(x => x.Departments).ToListAsync()) :

                comment = (from c in Context.Comments.Include(c=> c.Journals) where c.JournalId.Equals(journal.Id) select c).ToList();
                if (comment.Count > 0)
                {
                    article=(from c in Context.EDSUJournals where c.Id == journal.Id select c).ToList();
                    if(article.Count > 0)
                    {
                      //  ViewBag.Message1 = article.ToList();
                    }
                    return View(comment);
                }
                //article = (from c in Context.EDSUJournals where c.FirstReviewer.Equals(editor.Id.ToString()) || c.SecondReviewer.Equals(editor.Id.ToString()) || c.ThirdReviewer.Equals(editor.Id.ToString()) || c.FourthReviewer.Equals(editor.Id.ToString()) || c.FifthReviewer.Equals(editor.Id.ToString()) select c).ToList();
                //if (affectedJournal.Count > 0)
                //{
                //    //return View(article);
                //}
            }
            }
            var journals = from i in Context.EDSUJournals
                           select i;
            if (journals.Any())
            {
                journals = journals.Where(i => i.UploaderId == currentUserId);
                if (journals.Any())
                {
                    return View(journals);
                }

            }
            var isManager = User.IsInRole(Constants.ArticleReviewerRole);
            var isAdmin = User.IsInRole(Constants.ArticleSuperAdminRole);
            //checking if an applicant has applied before

            if (isAdmin != false)
            {
                return View(await Context.EDSUJournals.Include(x => x.JournalEditors).ToListAsync());

            }

            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
          return (Context.EDSUJournals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
