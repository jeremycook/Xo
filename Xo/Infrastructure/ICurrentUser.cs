﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xo.Areas.Accounts.Models;

namespace Xo.Infrastructure
{
    public interface ICurrentUser
    {
        ApplicationUser User { get; }
    }
}
