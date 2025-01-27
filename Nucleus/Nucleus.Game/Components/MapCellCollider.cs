﻿using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A data component which may be attached to elements and map cells
    /// which will register a collision with solid objects in the same map cell
    /// </summary>
    [Serializable]
    public class MapCellCollider : Unique, IMapCellDataComponent, IElementDataComponent
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Solid property
        /// </summary>
        private bool _Solid = true;

        /// <summary>
        /// Is this collider solid?  Solid objects will register collisions with other solid objects.
        /// </summary>
        public bool Solid
        {
            get { return _Solid; }
            set
            {
                _Solid = value;
                NotifyPropertyChanged("Solid");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Can the owner of this collider enter the specified cell?
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool CanEnter(MapCell cell)
        {
            return Blocker(cell) == null;
        }

        /// <summary>
        /// Get the element which is blocking the owner of this collider
        /// from entering the specified cell.  If this returns null, there
        /// is no blocking element in the cell and the owner may enter it.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public Element Blocker(MapCell cell)
        {
            if (!Solid) return null; //Can pass through!
            foreach (Element el in cell.Contents)
            {
                MapCellCollider other = el.GetData<MapCellCollider>();
                if (other != null && other.Solid) return el; // Blockage!
            }
            return null; // No blockage!
        }

        #endregion

    }
}
