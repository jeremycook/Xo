using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xo.Areas.Infrastructure.Tasks
{
    public interface IRunOnEachRequest
    {
        void Execute();
    }
}