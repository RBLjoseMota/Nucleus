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

using FreeBuild.Base;
using FreeBuild.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// A collection of elements
    /// </summary>
    [Serializable]
    public class ElementCollection : ModelObjectCollection<Element>
    {
        #region Properties

        /// <summary>
        /// Extract a collection of all the linear elements in this collection.
        /// A new collection will be generated each time this is called.
        /// </summary>
        public ElementCollection LinearElements
        {
            get
            {
                var result = new ElementCollection();
                foreach (Element element in this)
                {
                    if (element is LinearElement) result.Add(element);
                }
                return result;
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Default constructor.  Initialises a new empty ElementCollection
        /// </summary>
        public ElementCollection() : base() { }

        /// <summary>
        /// Owner constructor.  Initialises an empty ElementCollection with the specified owner
        /// </summary>
        /// <param name="model"></param>
        protected ElementCollection(Model model) : base(model) { }

        #endregion

        #region Methods

        /// <summary>
        /// Find and return the subset of elements in this collection that contain a reference to the
        /// specified node.
        /// </summary>
        /// <param name="node">The node to search for</param>
        /// <returns></returns>
        public ElementCollection AllWith(Node node)
        {
            var result = new ElementCollection();
            if (node != null)
            {
                foreach (Element el in this)
                {
                    if (el.ContainsNode(node)) result.Add(el);
                }
            }
            return result;
        }

        /// <summary>
        /// Find and return the subset of elements in this collection that contain a reference to the
        /// specified property.
        /// </summary>
        /// <param name="property">The property to search for</param>
        /// <returns></returns>
        public ElementCollection AllWith(VolumetricProperty property)
        {
            var result = new ElementCollection();
            foreach (Element el in this)
            {
                if (el.GetProperty() == property) result.Add(el);
            }
            return result;
        }

        #endregion
    }
}