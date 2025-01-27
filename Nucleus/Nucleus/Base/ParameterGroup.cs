﻿using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Base
{
    /// <summary>
    /// A named grouping of parameters.
    /// Parameters can be tagged with a group in order
    /// to define their 
    /// </summary>
    [Serializable]
    public class ParameterGroup : Named, IComparable, IComparable<ParameterGroup>
    {
        #region Properties

        private double _Order = 0;

        /// <summary>
        /// The order weighting for this parameter group.  Those with a lower order
        /// weighting will be displayed first.
        /// </summary>
        public double Order
        {
            get { return _Order; }
            set { ChangeProperty(ref _Order, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor, creates a blank empty parameter group
        /// with no name.  Only use this constructor if you know what 
        /// you're doing.
        /// </summary>
        internal ParameterGroup()
        {

        }

        /// <summary>
        /// Creates a new ParameterGroup with the specified name
        /// </summary>
        /// <param name="name">The name of the group</param>
        public ParameterGroup(string name)
        {
            _Name = name;
        }

        /// <summary>
        /// Creates a new ParameterGroup with the specified name and 
        /// ordering weighting
        /// </summary>
        /// <param name="name"></param>
        /// <param name="order"></param>
        public ParameterGroup(string name, double order) : this(name)
        {
            _Order = order;
        }

        #endregion

        #region Methods

        /// <summary>
        /// IComparable implementation to aid sorting groups
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ParameterGroup other)
        {
            int result = Order.CompareTo(other.Order);
            if (result == 0) return GUID.CompareTo(other.GUID);
            return result;
        }

        /// <summary>
        /// IComparable implementation to aid sorting groups
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is ParameterGroup)
                return CompareTo((ParameterGroup)obj);
            else return 0;
        }

        #endregion
    }
}
