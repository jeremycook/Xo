using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Xo.Areas.Blog.Domain
{
    public class BlogDbContext : DbContext
    {
        public IDbSet<Post> Posts { get; set; }

        //// Foreigners:

        public IDbSet<Xo.Areas.Users.Domain.User> Users { get; set; }
    }
}