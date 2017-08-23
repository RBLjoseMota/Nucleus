﻿using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Results
{
    /// <summary>
    /// An abstract base class for dictionaries of results for an individual model object,
    /// keyed by results case and then by type
    /// </summary>
    public abstract class ModelObjectResults<TResults> 
        : ResultsDictionary<ResultsCase, TResults>, IModelObjectResults
        where TResults : ICaseResults
    {
    }
}
