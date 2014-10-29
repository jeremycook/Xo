using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Xo.Areas.Users.Domain;
using Xo.Areas.Infrastructure.Tasks;

namespace Xo.Areas.Infrastructure
{
    /// <summary>
    /// Implement the transaction per HTTP request pattern for your <typeparamref name="TDbContext"/> of choice.
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class TransactionPerRequest<TDbContext> : IRunOnEachRequest, IRunOnError, IRunAfterEachRequest
        where TDbContext : DbContext
    {
        private readonly TDbContext Db;
        private readonly HttpContextBase HttpContext;
        public TransactionPerRequest(TDbContext db, HttpContextBase httpContext)
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