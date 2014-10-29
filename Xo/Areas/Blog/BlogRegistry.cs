using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Xo.Areas.Blog.Domain;

namespace Xo.Areas.Blog
{
    public class BlogRegistry : Registry
    {
        public BlogRegistry()
        {
            Database.SetInitializer<BlogDbContext>(
                new DropCreateDatabaseIfModelChanges<BlogDbContext>());
        }
    }
}