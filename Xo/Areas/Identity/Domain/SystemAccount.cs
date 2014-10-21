using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xo.Areas.Identity.Domain
{
    /// <summary>
    /// System accounts are useful for tracking the identity of internal, automated
    /// processes, and actions executed by anonymous users. For example, System
    /// and Anonymous identities can be created and used to track such actions.
    /// </summary>
    public class SystemAccount : Identity
    {
        [Obsolete("Runtime use only.", error: true)]
        public SystemAccount() { }

        /// <summary>
        /// Construct a brand new, not yet in the database, system account.
        /// </summary>
        /// <param name="id"></param>
        public SystemAccount(Guid id, string userName)
        {
            this.Id = id;
            this.UserName = userName;
        }
    }
}