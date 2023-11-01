﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artin.BringAuto.Shared.Enums
{
    public enum CarStatus
    {
        Waiting,
        AcceptionOrder,
        Driving,
        WaitForLoad,
        OutOfService,
        Charging,
        StoppedByPhone
    }
}
