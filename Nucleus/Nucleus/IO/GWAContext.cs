﻿using Nucleus.Base;
using Nucleus.Conversion;
using Nucleus.Extensions;
using Nucleus.Geometry;
using Nucleus.Model;
using Nucleus.Model.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.IO
{
    /// <summary>
    /// Contextual operations involved with writing out a GWA-format file
    /// </summary>
    [Serializable]
    public class GWAContext : StringConversionContextBase
    {
        // TODO: SubComponentIndex is a single parameter which will potentially be used
        // for sub-objects on properties as well.  This can lead to unwanted ID increments.
        // TO BE FIXED!

        #region Fields

        /// <summary>
        /// The dictionary of IDs for various types
        /// </summary>
        public Dictionary<Type, int> _NextID = new Dictionary<Type, int>();

        #endregion

        #region Properties

        private IDMappingTable<Guid, IList<int>> _IDMap = new IDMappingTable<Guid, IList<int>>("Nucleus", "GSA");

        /// <summary>
        /// The ID mapping table
        /// </summary>
        /// <remarks>
        /// Currently, this does not categorise the items, meaning it's not usable to map back again...
        /// </remarks>
        public IDMappingTable<Guid, IList<int>> IDMap
        {
            get { return _IDMap; }
        }

        #endregion

        public GWAContext()
        {
            //Initialise ID starters
            _NextID.Add(typeof(Node), 1);
            _NextID.Add(typeof(Element), 1);
            _NextID.Add(typeof(SectionFamily), 1);
            _NextID.Add(typeof(BuildUpFamily), 1);
            _NextID.Add(typeof(ModelObjectSetBase), 1);
            _NextID.Add(typeof(LoadCase), 1);
        }

        /// <summary>
        /// Get the next available GSA ID for the specified object.
        /// Will increment the stored next available ID for the relevant type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetNextIDFor(object obj)
        {
            Type type = obj.GetType();
            type = _NextID.Keys.ClosestAncestor(type);
            int result = _NextID[type];
            _NextID[type] = result + 1;
            return result;
        }

        public override bool HasSubComponentsToWrite(object source)
        {
            if (source is PanelElement)
            {
                PanelElement pEl = (PanelElement)source;
                if (pEl.Geometry is Mesh)
                {
                    Mesh mesh = (Mesh)pEl.Geometry;
                    return (mesh.Faces.Count > this.SubComponentIndex);
                }
            }
            return base.HasSubComponentsToWrite(source);
        }

        /// <summary>
        /// Test whether a PanelElement source object has a representation
        /// that can be written out as a mesh
        /// </summary>
        /// <returns></returns>
        public bool HasMeshRepresentation()
        {
            if (SourceObject is PanelElement)
            {
                PanelElement pEl = (PanelElement)SourceObject;
                if (pEl.Geometry is Mesh)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Convert to integer
        /// </summary>
        /// <returns></returns>
        public string ToInt()
        {
            if (SourceObject is bool && ((bool)SourceObject) == true) return "1";
            else return "0";
        }

        /// <summary>
        /// Get the element type of the current element object
        /// </summary>
        /// <returns></returns>
        public string ElementType()
        {
            if (SourceObject is LinearElement)
            {
                return "BEAM"; //TODO
            }
            else
            {
                MeshFace face = CurrentPanelFace();
                if (face.IsTri) return "TRI3";
                else return "QUAD4";
            }
            //TODO
        }

        /// <summary>
        /// Get a node topology description for the current sub-element
        /// </summary>
        /// <returns></returns>
        public string ElementTopo()
        {
            StringBuilder sb = new StringBuilder();
            if (SourceObject is PanelElement)
            {
                MeshFace face = CurrentPanelFace();
                for (int i = 0; i < Math.Min(4, face.Count);i++)
                {
                    if (i > 0) sb.Append("\t");
                    string id = GetID(face[i].Node);
                    sb.Append(id);
                }
            }
            return sb.ToString();
            //TODO : Multi-span linear elements?
        }

        /// <summary>
        /// Convert a Bool6D to a GWA release description
        /// </summary>
        /// <returns></returns>
        public string ToReleaseString()
        {
            if (SourceObject is Bool6D)
            {
                Bool6D b6D = (Bool6D)SourceObject;
                return b6D.ToString("R", "F");
            }
            return "FFFFFF";
        }

        /// <summary>
        /// Get a point load position in GWA form (-ve if relative)
        /// </summary>
        /// <returns></returns>
        public double PointLoadPosition()
        {
            if (SourceObject is LinearElementPointLoad)
            {
                var lEPL = (LinearElementPointLoad)SourceObject;
                if (lEPL.Relative) return -lEPL.Position;
                else return lEPL.Position;
            }
            return 0;
        }

        /// <summary>
        /// Get the current sub-component mesh face of the current panel element
        /// source object
        /// </summary>
        /// <returns></returns>
        public MeshFace CurrentPanelFace()
        {
            if (SourceObject is PanelElement)
            {
                PanelElement el = (PanelElement)SourceObject;
                if (el.Geometry is Mesh)
                {
                    Mesh mesh = (Mesh)el.Geometry;
                    return mesh.Faces[SubComponentIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// Get the group number of the current element object
        /// </summary>
        /// <returns></returns>
        public string ElementGroup()
        {
            return "1";
            //TODO
        }

        /// <summary>
        /// Get the description of the material of the current family object
        /// </summary>
        /// <returns></returns>
        public string FamilyMaterial()
        {
            if (SourceObject is Family)
            {
                Family family = (Family)SourceObject;
                Material material = family.GetPrimaryMaterial();
                if (material != null) return material.Name;
            }
            return "STEEL";
            //TODO
        }

        /// <summary>
        /// Convert a set to a GWA list definition
        /// </summary>
        /// <returns></returns>
        public string ListDefinition()
        {
            if (SourceObject is ModelObjectSetBase)
            {
                var set = (ModelObjectSetBase)SourceObject;
                List<int> ids = set.GetItemIDs<int>(IDMap);
                ids.Sort();
                return ids.ToCompressedString();
            }
            return "";
        }

        /// <summary>
        /// Get a GSA section description of the current object
        /// </summary>
        /// <returns></returns>
        public string SectionDescription()
        {
            if (SourceObject is SectionFamily)
            {
                SectionFamily section = (SectionFamily)SourceObject;
                // TODO: Create section description
                if (section.Profile != null)
                {
                    //if (section.Profile.CatalogueName != null)
                    //   return "CAT " + section.Profile.CatalogueName;
                    if (section.Profile is SymmetricIProfile)
                    {
                        var profile = (SymmetricIProfile)section.Profile;
                        return string.Format("STD I({0}) {1} {2} {3} {4}", "m", profile.Depth, profile.Width, profile.WebThickness, profile.FlangeThickness);
                    }
                    else if (section.Profile is RectangularHollowProfile)
                    {
                        var profile = (RectangularHollowProfile)section.Profile;
                        return string.Format("STD RHS({0}) {1} {2} {3} {4}", "m", profile.Depth, profile.Width, profile.WebThickness, profile.FlangeThickness);
                    }
                    else if (section.Profile is RectangularProfile)
                    {
                        var profile = (RectangularProfile)section.Profile;
                        return string.Format("STD R({0}) {1} {2}", "m", profile.Depth, profile.Width);
                    }
                    else if (section.Profile is CircularHollowProfile)
                    {
                        var chsSection = (CircularHollowProfile)section.Profile;
                        return string.Format("STD CHS({0}) {1} {2}", "m", chsSection.Diameter, chsSection.WallThickness);
                    }
                    else if (section.Profile is CircularProfile)
                    {
                        var profile = (CircularProfile)section.Profile;
                        return string.Format("STD C({0}) {1}", "m", profile.Diameter);
                    }
                    else if (section.Profile is TProfile)
                    {
                        var profile = (TProfile)section.Profile;
                        return string.Format("STD T({0}) {1} {2} {3} {4}", "m", profile.Depth, profile.Width, profile.WebThickness, profile.FlangeThickness);
                    }
                    else if (section.Profile is AngleProfile)
                    {
                        var profile = (AngleProfile)section.Profile;
                        return string.Format("STD A({0}) {1} {2} {3} {4}", "m", profile.Depth, profile.Width, profile.WebThickness, profile.FlangeThickness);
                    }
                    else if (section.Profile is ChannelProfile)
                    {
                        var profile = (ChannelProfile)section.Profile;
                        //?:
                        return string.Format("STD CH({0}) {1} {2} {3} {4}", "m", profile.Depth, profile.Width, profile.WebThickness, profile.FlangeThickness);
                    }
                    // TODO: Other types
                }
            }
            return "EXP";
        }

        /// <summary>
        /// Get the GSA ID of the specified object
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            return GetID(SourceObject, SubComponentIndex);
        }

        /// <summary>
        /// Get the GSA ID of the specified object
        /// </summary>
        /// <returns></returns>
        public string GetID(object obj, int subComponentIndex = 0)
        {
            if (obj != null && obj is ModelObject)
            {
                if (obj is GlobalCoordinateSystemReference) return "GLOBAL";
                else if (obj is LocalCoordinateSystemReference) return "LOCAL";

                //if (!HasSubComponentsToWrite(obj)) subComponentIndex = 0; //Now unnecessary?

                ModelObject mObj = (ModelObject)obj;
                if (IDMap.HasSecondID(mObj.GUID))
                {
                    IList<int> IDs = IDMap.GetSecondID(mObj.GUID);
                    if (IDs.Count > subComponentIndex) return IDs[subComponentIndex].ToString();
                    else
                    {
                        int ID = GetNextIDFor(obj);
                        IDs.Add(ID); //This depends on this being accessed in order...
                        return ID.ToString();
                    }
                }
                else
                {
                    IList<int> IDs = new List<int>();
                    int ID = GetNextIDFor(obj);
                    IDs.Add(ID);
                    IDMap.Add(mObj.GUID, IDs);
                    return ID.ToString();
                }
                //return mObj.NumericID.ToString();
            }
            else return "";
        }
    }
}
