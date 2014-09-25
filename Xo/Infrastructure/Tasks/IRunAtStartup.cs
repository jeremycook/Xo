using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xo.Infrastructure.Tasks
{
    public interface IRunAtStartup
    {
        void Execute();
    }
}