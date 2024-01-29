using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Twillio
{
    public class TwillioConfig
    {
        public string FromNumber { get; set; }
        public String SID { get; set; }
        public String Token { get; set; }
        public int CallRetryCount { get; set; }
        public int CallStatusQueryIntervalMS { get; set; }
        public int CallStatusQueryTimeoutCount { get; set; }
    }
}
