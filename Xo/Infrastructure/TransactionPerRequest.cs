using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Xo.Areas.Accounts.Models;
using Xo.Infrastructure.Tasks;

namespace Xo.Infrastructure
{
    public class TransactionPerRequest : IRunOnEachRequest, IRunOnError, IRunAfterEachRequest
    {
        private readonly ApplicationDbContext Db;
        private readonly HttpContextBase HttpContext;
        public TransactionPerRequest(ApplicationDbContext db, HttpContextBase httpContext)
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