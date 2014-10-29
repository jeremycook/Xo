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
        public System.Data.Entity.IDbSet<Xo.Areas.Identity.Domain.Identity> Identities { get; set; }
    }
}