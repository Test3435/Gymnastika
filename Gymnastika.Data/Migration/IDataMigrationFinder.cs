﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Data.Migration
{
    public interface IDataMigrationFinder
    {
        IEnumerable<IDataMigration> Find();
    }
}
