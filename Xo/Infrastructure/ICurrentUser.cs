﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xo.Areas.Identity.Domain;

namespace Xo.Infrastructure
{
    public interface ICurrentUser
    {
        User User { get; }
    }
}
