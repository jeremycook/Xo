using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xo.Areas.Infrastructure.Alerts
{
    public class Alert
    {
        public Alert(string alertClass, string message)
        {
            this.AlertClass = alertClass;
            this.Message = message;
        }

        public string AlertClass { get; set; }
        public string Message { get; set; }
    }
}
