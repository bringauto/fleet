using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Helpers
{
    public static class RelativeMapHelper
    {
        public static Double RelativePosition(double position, double min, double max, double scale)
           => (position - min) / (max - min) * scale;
    }
}
