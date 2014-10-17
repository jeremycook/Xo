using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Xo.Areas.Identity.Domain;
using Xo.Infrastructure.Tasks;

namespace Xo.Infrastructure
{
    public class TransactionPerRequest : IRunOnEachRequest, IRunOnError, IRunAfterEachRequest
    {
        private readonly IdentityDbContext Db;
        private readonly HttpContextBase HttpContext;
        public TransactionPerRequest(IdentityDbContext db, HttpContextBase httpContext)
        {
            Db = db;
            HttpContext = httpContext;
        }

        void IRunOnEachRequest.Execute()
        {
            HttpContext.Items["TransactionPerRequest:Transaction"] =
                Db.Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        void IRunOnError.Execute()
        {
            HttpContext.Items["TransactionPerRequest:Error"] = true;
        }

        void IRunAfterEachRequest.Execute()
        {
            var transaction = (DbContextTransaction)HttpContext.Items["TransactionPerRequest:Transaction"];

            if (HttpContext.Items["TransactionPerRequest:Error"] != null)
            {
                transaction.Rollback();
            }
            else
            {
                transaction.Commit();
            }
        }
    }
}