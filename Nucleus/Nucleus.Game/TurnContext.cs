﻿using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// Object to contain context data about a turn in a turn-based game
    /// </summary>
    [Serializable]
    public class TurnContext
    {
        #region Properties

        /// <summary>
        /// The current state
        /// </summary>
        public GameState State { get; set; }

        /// <summary>
        /// The current stage
        /// </summary>
        public MapStage Stage { get; set; }

        /// <summary>
        /// The element whose turn has completed
        /// (and whose components are currently being activated)
        /// </summary>
        public Element Element { get; set; }

        #endregion

        #region Constructors

        public TurnContext() { }

        public TurnContext(GameState state, MapStage stage, Element element)
        {
            State = state;
            Stage = stage;
            Element = element;
        }

        #endregion
    }
}