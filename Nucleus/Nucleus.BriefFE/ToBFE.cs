﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFE = BriefFiniteElementNet;
using Nucleus.Model;
using Nucleus.Geometry;
using Nucleus.Base;

namespace Nucleus.BriefFE
{
    /// <summary>
    /// Helper class to convert Nucleus object types to equivalent
    /// Brief FE types
    /// </summary>
    public static class ToBFE
    {
        /// <summary>
        /// Convert a Nucleus position vector to a Brief FE point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static BFE.Point Convert(Vector point)
        {
            return new BFE.Point(point.X, point.Y, point.Z);
        }

        /// <summary>
        /// Convert a Nucleus point to a Brief FE PointYZ.
        /// NOTE: The X and Y coordinates of the input point
        /// will map to the Y and Z coordinates of the output
        /// point, respectively.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static BFE.PointYZ ConvertYZ(Vector point)
        {
            return new BFE.PointYZ(point.X, point.Y);
        }

        /// <summary>
        /// Convert a list of Nucleus points to Brief FE PointYZs.
        /// NOTE: The X and Y coordinates of the input point
        /// will map to the Y and Z coordinates of the output
        /// point, respectively.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static BFE.PointYZ[] ConvertYZ(IList<Vector> points)
        {
            var result = new BFE.PointYZ[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                result[i] = ConvertYZ(points[i]);
            }
            return result;
        }

        /// <summary>
        /// Convert a boolean to a BFE Constraint enum
        /// </summary>
        /// <param name="boolean"></param>
        /// <returns></returns>
        private static BFE.DofConstraint ToConstraint(bool boolean)
        {
            if (boolean) return BFE.DofConstraint.Fixed;
            else return BFE.DofConstraint.Released;
        }

        /// <summary>
        /// Convert a fixity Bool6D to a Brief FE Constraint struct
        /// </summary>
        /// <param name="fixity"></param>
        /// <returns></returns>
        public static BFE.Constraint Convert(Bool6D fixity)
        {
            return new BFE.Constraint(
                ToConstraint(fixity.X),
                ToConstraint(fixity.Y),
                ToConstraint(fixity.Z),
                ToConstraint(fixity.XX),
                ToConstraint(fixity.YY),
                ToConstraint(fixity.ZZ));
        }

        /// <summary>
        /// Convert a Nucleus node to a BFE one
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static BFE.Node Convert(Node node)
        {
            var result = new BFE.Node(Convert(node.Position))
            {
                Label = node.GUID.ToString(),
            };
            if (node.HasData<NodeSupport>())
            {
                var nS = node.GetData<NodeSupport>();
                result.Constraints = Convert(nS.Fixity);
            }
            return result;
        }

        /// <summary>
        /// Convert a Nucleus curve on the XY plane to a Brief FE PolygonYz
        /// (on the YZ plane)
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static BFE.PolygonYz Convert(Curve curve, Angle tolerance)
        {
            return new BFE.PolygonYz(ConvertYZ(curve.Facet(tolerance)));
        }

        /// <summary>
        /// Convert a Nucleus curve on the XY plane to a Brief FE PolygonYz
        /// (on the YZ plane)
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static BFE.PolygonYz Convert(Curve curve)
        {
            return Convert(curve, Tolerance.Angle);
        }

        /// <summary>
        /// Convert a Nucleus material to a BFE one
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public static BFE.Materials.UniformIsotropicMaterial Convert(IsoMaterial material)
        {
            var result = new BFE.Materials.UniformIsotropicMaterial();
            result.YoungModulus = material.E;
            result.PoissonRatio = material.PoissonsRatio;
            return result;
        }

        /// <summary>
        /// Convert a Nucleus material to a BFE one
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public static BFE.Materials.BaseMaterial Convert(Material material)
        {
            if (material is IsoMaterial) return Convert((IsoMaterial)material);
            else throw new NotImplementedException("Conversion of " + material.GetType().Name + " not supported.");
        }

        /// <summary>
        /// Convert a Nucleus Direction to a BFE LoadDirection
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static BFE.LoadDirection Convert(Direction direction)
        {
            if (direction == Direction.X) return BFE.LoadDirection.X;
            if (direction == Direction.Y) return BFE.LoadDirection.Y;
            if (direction == Direction.Z) return BFE.LoadDirection.Z;
            else throw new NotImplementedException(); //TODO: Figure out how to do moments!
        }

        /// <summary>
        /// Convert a Nucleus CoordinateSystemReference to a BFE CoordinationSystem
        /// </summary>
        /// <param name="cSystem"></param>
        /// <returns></returns>
        public static BFE.CoordinationSystem Convert(CoordinateSystemReference cSystem)
        {
            if (cSystem.IsLocal) return BFE.CoordinationSystem.Local;
            else return BFE.CoordinationSystem.Global; // TODO: Throw error?
        }
    }
}
