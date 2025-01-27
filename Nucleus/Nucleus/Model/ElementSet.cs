﻿using Nucleus.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Model
{
    /// <summary>
    /// A parametrically-defined set of elements.  
    /// Allows element collections to be defined via a base collection and a set of logical 
    /// filters which act upon that collection.
    /// </summary>
    [Serializable]
    public class ElementSet : ModelObjectSet<Element, ElementCollection,
        ISetFilter<Element>, SetFilterCollection<ISetFilter<Element>, Element>,
        ElementSet, ModelObjectSetCollection<ElementSet>>
    {
        #region Constructor

        /// <summary>
        /// Create a new blank element set
        /// </summary>
        public ElementSet() : base() { }

        /// <summary>
        /// Create a new blank element set with the specified name
        /// </summary>
        /// <param name="name"></param>
        public ElementSet(string name) : base(name) { }

        /// <summary>
        /// Create a new 'all elements' set
        /// </summary>
        /// <param name="all"></param>
        public ElementSet(bool all) : base(all) { }

        /// <summary>
        /// Initialise this set to contain a single item
        /// </summary>
        /// <param name="item"></param>
        public ElementSet(Element item) : base(item) { }

        /// <summary>
        /// Initialise this set to contain the specified base collection of items
        /// </summary>
        /// <param name="collection"></param>
        public ElementSet(ElementCollection collection) : base(collection) { }

        /// <summary>
        /// Initialise this set to contain the specified base collection of items
        /// </summary>
        /// <param name="collection"></param>
        public ElementSet(LinearElementCollection collection) 
            : this(collection.ToElementCollection()) { }

        /// <summary>
        /// Initialise this set to contain the specified base collection of items
        /// </summary>
        /// <param name="collection"></param>
        public ElementSet(PanelElementCollection collection) 
            : this(collection.ToElementCollection()) { }

        /// <summary>
        /// Initialise a set to contain the items and filters from another set
        /// </summary>
        /// <param name="other"></param>
        public ElementSet(ElementSet other) : base()
        {
            All = other.All;
            Add(other.BaseCollection);
            Add(other.SubSets);
            Add(other.Filters);
        }

        /// <summary>
        /// Initialise a set to contain the items and filters from another set
        /// </summary>
        /// <param name="other"></param>
        public ElementSet(LinearElementSet other) : this((ElementSet)other) { }

        /// <summary>
        /// Initialise a set to contain the items and filters from another set
        /// </summary>
        /// <param name="other"></param>
        public ElementSet(PanelElementSet other) : this((ElementSet)other) { }

        #endregion

        #region Methods

        protected override ElementCollection GetItemsInModel()
        {
            return Model?.Elements;
        }

        #endregion
    }

    /// <summary>
    /// Base class for ElementSet subtypes that automatically filter
    /// out items that are not of the specified subtype
    /// </summary>
    /// <typeparam name="TSubType"></typeparam>
    [Serializable]
    public abstract class ElementSet<TSubType> : ElementSet
        where TSubType : Element
    {
        #region Constructors

        /// <summary>
        /// Create a new blank element set
        /// </summary>
        public ElementSet() : base() { }

        /// <summary>
        /// Create a new 'all elements' set
        /// </summary>
        /// <param name="all"></param>
        public ElementSet(bool all) : base(all) { }

        /// <summary>
        /// Initialise this set to contain a single item
        /// </summary>
        /// <param name="item"></param>
        public ElementSet(Element item) : base(item) { }

        /// <summary>
        /// Initialise this set to contain the specified base collection of items
        /// </summary>
        /// <param name="collection"></param>
        public ElementSet(ElementCollection collection) : base(collection) { }

        /// <summary>
        /// Initialise this set to contain all elements in the model to which
        /// it belongs which are part of the specified element family
        /// </summary>
        /// <param name="family"></param>
        public ElementSet(Family family) : base(true)
        {
            Filters.Add(new FamilyFilter(family));
        }

        #endregion

        #region Methods

        protected override bool PassInternalFilter(Element item)
        {
            return item is TSubType;
        }

        #endregion
    }
}
