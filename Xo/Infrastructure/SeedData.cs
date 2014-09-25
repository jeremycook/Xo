using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xo.Areas.Accounts.Models;
using Xo.Infrastructure.Tasks;

namespace Xo.Infrastructure
{
    public class SeedData : IRunAtStartup
    {
        private readonly ApplicationDbContext Db;

        public SeedData(ApplicationDbContext db)
        {
            Db = db;
        }

        public void Execute()
        {
            // TODO: Add users and such!
        }
    }
}