﻿// Copyright (c) 2016 Paul Jeffries
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Geometry
{
    /// <summary>
    /// Abstract base class for surfaces - infinitely thin membrane geometries
    /// </summary>
    [Serializable]
    public abstract class Surface : VertexGeometry
    {
        #region Methods

        /// <summary>
        /// Calculate the surface area (and centroid) of this surface
        /// </summary>
        /// <param name="centroid"></param>
        /// <returns></returns>
        public abstract double CalculateArea(out Vector centroid);

        /// <summary>
        /// Calculate the surface area of this surface
        /// </summary>
        /// <returns></returns>
        public double CalculateArea()
        {
            Vector centroid;
            return CalculateArea(out centroid);
        }

        public override string ToString()
        {
            return "Surface";
        }

        #endregion
    }
}