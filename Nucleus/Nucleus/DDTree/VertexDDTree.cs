﻿using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.DDTree
{
    /// <summary>
    /// A divided dimension tree to store geometry vertices
    /// </summary>
    [Serializable]
    public class VertexDDTree : PositionDDTree<Vertex>
    {
        #region Constructors

        public VertexDDTree(IList<Vertex> items, int maxDivisions = 10, double minCellSize = 1) : base(items, maxDivisions, minCellSize)
        {
        }

        #endregion
    }
}
