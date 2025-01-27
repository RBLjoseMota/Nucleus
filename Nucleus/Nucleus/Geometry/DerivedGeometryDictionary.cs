﻿using Nucleus.Base;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// A dictionary to store derived element geometry
    /// </summary>
    [Serializable]
    public class DerivedGeometryDictionary : Dictionary<string, VertexGeometry>, IOwned<Element>
    {
        #region Properties

        /// <summary>
        /// Private backing field for Owner property
        /// </summary>
        private Element _Owner = null;

        /// <summary>
        /// The owning element of the derived geometry
        /// </summary>
        public Element Owner { get { return _Owner; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise a DerivedGeometryDictionary owned by the specified element
        /// </summary>
        /// <param name="owner"></param>
        public DerivedGeometryDictionary(Element owner)
        {
            _Owner = owner;
        }

        #endregion
    }
}
