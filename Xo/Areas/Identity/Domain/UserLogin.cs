using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Xo.Areas.Identity.Domain
{
    public class UserLogin
    {
        [Obsolete("Runtime use only.", error: true)]
        public UserLogin() { }

        public UserLogin(UserId userId, string loginProvider, string providerKey)
        {
            this.UserId = userId;
            this.LoginProvider = loginProvider;
            this.ProviderKey = providerKey;
        }

        [Key, Column(Order = 1)]
        [Required]
        public UserId UserId { get; private set; }

        [Key, Column(Order = 2)]
        [Required]
        public string LoginProvider { get; private set; }

        [Key, Column(Order = 3)]
        [Required]
        public string ProviderKey { get; private set; }
    }
}
