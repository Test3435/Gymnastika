﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Services
{
    public interface ISportsPlanProvider : IProvider<SportsPlan>
    {
        //IEnumerable<SportsPlan> Fetch(Func<SportsPlan, bool> predicate);
    }
}
