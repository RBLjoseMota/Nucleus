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
    /// Enumerated values for the vertical set-out of section profiles
    /// </summary>
    public enum VerticalSetOut
    {
        Centroid,
        MidPoint,
        Top,
        Bottom,
        Origin
    }

    /// <summary>
    /// Extension methods for the VerticalSetOut Enum
    /// </summary>
    public static class VerticalSetOutExtensions
    {
        /// <summary>
        /// Get the position of the set-out level as a proportion of the thickness
        /// of a panel.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double FactorFromTop(this VerticalSetOut value)
        {
            if (value == VerticalSetOut.Top) return 0;
            else if (value == VerticalSetOut.Bottom) return 1;
            else return 0.5;
        }

        /// <summary>
        /// Is this set out relative to either the top or bottom edge?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEdge(this VerticalSetOut value)
        {
            return value == VerticalSetOut.Top || value == VerticalSetOut.Bottom;
        }
    }
}
