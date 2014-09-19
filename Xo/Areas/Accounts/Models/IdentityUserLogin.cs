using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Xo.Areas.Accounts.Models
{
    public class IdentityUserLogin
    {
        [Obsolete("Runtime use only.", error: true)]
        public IdentityUserLogin()
        {
        }

        public IdentityUserLogin(Guid userId, string loginProvider, string providerKey)
        {
            this.UserId = userId;
            this.LoginProvider = loginProvider;
            this.ProviderKey = providerKey;
        }

        [Key, Column(Order = 1)]
        public Guid UserId { get; private set; }

        [Key, Column(Order = 2)]
        public string LoginProvider { get; private set; }

        [Key, Column(Order = 3)]
        public string ProviderKey { get; private set; }
    }
}
