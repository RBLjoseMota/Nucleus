﻿using Nucleus.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// An enumerated value representing different possible
    /// user input functions.
    /// </summary>
    /// <remarks>Values whose integer equivalent is a multiple
    /// of 1000 are 'top level' and taken for certain purposes
    /// as being equivalent to those within the range of the
    /// next 999 values.</remarks>
    public enum InputFunction
    {
        /// <summary>
        /// The input function is not defined
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Movement (in any direction)
        /// </summary>
        Move = 1000,

        /// <summary>
        /// Upwards movement
        /// </summary>
        Up = 1100,

        /// <summary>
        /// Rightwards movement
        /// </summary>
        Right = 1200,

        /// <summary>
        /// Downwards movement
        /// </summary>
        Down = 1300,

        /// <summary>
        /// Leftwards movement
        /// </summary>
        Left = 1400,

        /// <summary>
        /// A resting action
        /// </summary>
        Wait = 2000
        
    }

    /// <summary>
    /// Extension methods for the InputFunction enum
    /// </summary>
    public static class InputFunctionExtensions
    {
        /// <summary>
        /// Get the vector which indicates the direction associated with this input
        /// (if any)
        /// </summary>
        /// <param name="iF"></param>
        /// <returns></returns>
        public static Vector DirectionVector(this InputFunction iF)
        {
            switch (iF)
            {
                case InputFunction.Up:
                    return new Vector(0, 1);
                case InputFunction.Down:
                    return new Vector(0, -1);
                case InputFunction.Left:
                    return new Vector(-1, 0);
                case InputFunction.Right:
                    return new Vector(1, 0);
                default:
                    return Vector.Unset;
            }
        }

        /// <summary>
        /// Convert this input to a 'top level' input that represents a
        /// category of different inputs
        /// </summary>
        /// <param name="iF"></param>
        /// <returns></returns>
        public static InputFunction ToTopLevel(this InputFunction iF)
        {
            int i = (int)iF;
            return (InputFunction)((i/1000)*1000);
        }

        /// <summary>
        /// Is this a 'top level' input that represents a category of
        /// different sub-inputs?
        /// </summary>
        /// <param name="iF"></param>
        /// <returns></returns>
        public static bool IsTopLevel(this InputFunction iF)
        {
            return ((int)iF) % 1000 == 0;
        }
    }
}
