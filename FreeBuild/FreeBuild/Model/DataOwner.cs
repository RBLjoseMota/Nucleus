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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Model
{
    /// <summary>
    /// Abstract generic base class for model objects which own data components.
    /// These objects contain a data store which can be used to hold data of various kinds
    /// attached to this object in an easily extensible way.
    /// </summary>
    [Serializable]
    public abstract class DataOwner<TDataStore, TData> : Named
        where TDataStore : DataStore<TData>, new()
        where TData : class
    {
        #region Properties

        /// <summary>
        /// Private backing field for Data property
        /// </summary>
        private TDataStore _Data;

        /// <summary>
        /// The store of data objects attached to this model object.
        /// This container can be used to add and retrieve data objects of specific
        /// types.
        /// </summary>
        public TDataStore Data
        {
            get
            {
                if (_Data == null) _Data = new TDataStore();
                return _Data;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected DataOwner() : base() { }

        /// <summary>
        /// Duplication constructor
        /// </summary>
        /// <param name="other"></param>
        protected DataOwner(DataOwner<TDataStore,TData> other) : base(other) { }

        #endregion

        #region Methods

        /// <summary>
        /// Check whether this object has any attached data components
        /// </summary>
        public bool HasData()
        {
                return _Data != null && _Data.Count > 0;
        }

        /// <summary>
        /// Check whether this object has any attached data components of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasData<T>() where T : class, TData
        {
            return _Data != null && Data.HasData<T>();
        }

        /// <summary>
        /// Get the data component of the specified type attached to this object (if one exists)
        /// </summary>
        /// <typeparam name="T">The type of the attached data component to be retrieved</typeparam>
        /// <returns></returns>
        public T GetData<T>()where T : class, TData
        {
            return _Data?.GetData<T>();
        }

        /// <summary>
        /// Get data of the specified generic type (or the closest available sub-type) attached to
        /// this object.  If no data component of the specified type is found then optionally a
        /// new one will be created.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <param name="create">If true, a new data component of the specified type will
        /// be created and returned should one not already exist.</param>
        /// <returns></returns>
        public T GetData<T>(bool create) where T : class, TData, new()
        {
            return Data.GetData<T>(create);
        }

        /// <summary>
        /// Get all data within this store that is of the specified generic type or which
        /// is assignable to that type.
        /// </summary>
        /// <typeparam name="T">The type of data component to be retrieved.</typeparam>
        /// <returns></returns>
        public IList<T> GetAllData<T>() where T : TData
        {
            return Data.GetAllData<T>();
        }

        #endregion
    }

    /// <summary>
    /// Abstract base class for model objects which own data components.
    /// These objects contain a data store which can be used to hold data of various kinds
    /// attached to this object in an easily extensible way.
    /// </summary>
    [Serializable]
    public abstract class DataOwner : DataOwner<DataStore, object>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected DataOwner() : base() { }

        /// <summary>
        /// Duplication constructor
        /// </summary>
        /// <param name="other"></param>
        protected DataOwner(DataOwner other) : base(other) { }

        #endregion
    }
}
