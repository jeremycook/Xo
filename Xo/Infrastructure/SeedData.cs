using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xo.Areas.Identity.Models;
using Xo.Infrastructure.Tasks;

namespace Xo.Infrastructure
{
    public class SeedData : IRunAtStartup
    {
        private readonly IdentityDbContext Db;

        public SeedData(IdentityDbContext db)
        {
            Db = db;
        }

        public void Execute()
        {
            // TODO: Add users and such!
        }
    }
}