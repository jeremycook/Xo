using Humanizer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Xo.Areas.Blog.Domain;
using Xo.Areas.Users.Models;
using Xo.Areas.Infrastructure.Alerts;

namespace Xo.Areas.Blog.Controllers
{
    [Authorize]
    [RouteArea("Blog", AreaPrefix = "Blog")]
    [RoutePrefix("Posts")]
    [Route("{Action}/{Id:int}")]
    public class PostsController : Controller
    {
        private readonly BlogDbContext Db;
        private readonly CurrentUser CurrentUser;

        public PostsController(BlogDbContext db, CurrentUser currentUser)
        {
            this.Db = db;
            this.CurrentUser = currentUser;
        }


        [Route("")]
        public async Task<ActionResult> Index(string terms = null)
        {
            var posts = Db.Posts.Include(p => p.Author)
                .Where(o => terms == null || o.Title.Contains(terms));
            return View(await posts.ToListAsync());
        }


        [Route("Create")]
        public ActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Post model)
        {
            var post = new Post(DateTime.UtcNow, CurrentUser.User.Id);
            post.Title = model.Title;
            post.Body = model.Body;
            post.PublicationDate = model.PublicationDate;
            post.AuthorId = model.AuthorId;
            post.Status = model.Status;

            ModelState.Clear();
            if (TryValidateModel(post))
            {
                Db.Posts.Add(post);
                await Db.SaveChangesAsync();

                return RedirectToAction("Index")
                    .WithSuccess("Created {0}.".FormatWith(post));
            }

            return View(post);
        }


        public async Task<ActionResult> Details(int id)
        {
            Post model = await Db.Posts.SingleOrDefaultAsync(o => o.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }


        public async Task<ActionResult> Edit(int id)
        {
            Post model = await Db.Posts.SingleOrDefaultAsync(o => o.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Post model)
        {
            Post post = await Db.Posts.SingleOrDefaultAsync(o => o.Id == id);
            post.Title = model.Title;
            post.Body = model.Body;
            post.PublicationDate = model.PublicationDate;
            post.AuthorId = model.AuthorId;
            post.Status = model.Status;

            ModelState.Clear();
            if (TryValidateModel(post))
            {
                await Db.SaveChangesAsync();
                return RedirectToAction("Index")
                    .WithSuccess("Saved changes to {0}.".FormatWith(post));
            }

            return View(model);
        }


        public async Task<ActionResult> Delete(int id)
        {
            Post model = await Db.Posts.SingleOrDefaultAsync(o => o.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, FormCollection form)
        {
            Post post = await Db.Posts.SingleOrDefaultAsync(o => o.Id == id);
            Db.Posts.Remove(post);
            await Db.SaveChangesAsync();

            return RedirectToAction("Index")
                .WithSuccess("Deleted {0}.".FormatWith(post));
        }
    }
}
