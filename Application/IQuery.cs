﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IQuery
    {
        FileManipulationResults GetResults();
    }
}
