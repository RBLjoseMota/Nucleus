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

namespace Nucleus.Units
{
    /// <summary>
    /// Enum to represent different physical dimensions
    /// </summary>
    [Serializable]
    public enum DimensionType
    {
        /// <summary>
        /// A quantity to which no physical dimension is applicable.
        /// </summary>
        Dimensionless = 0,

        /// <summary>
        /// A distance, or length, measurement.
        /// Default unit: m
        /// </summary>
        Distance = 1,

        /// <summary>
        /// An angle or rotation measurement.
        /// Default unit: Radians
        /// </summary>
        Angle = 2,

        /// <summary>
        /// A measurement of force.
        /// Default unit: N
        /// </summary>
        Force = 100,

        /// <summary>
        /// A measurement of moments
        /// Default unit: Nm
        /// </summary>
        Moments = 101,
       
        //TODO: Add more dimension types as and when needed
    }

}
