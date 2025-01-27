﻿using Nucleus.Base;
using Nucleus.Geometry;
using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game
{
    /// <summary>
    /// A game state for Roguelikes
    /// </summary>
    [Serializable]
    public class RLState : MapState
    {
        #region Properties

        /// <summary>
        /// Private backing member variable for the Active property
        /// </summary>
        private Element _Active = null;

        /// <summary>
        /// The element who is currently activated for a turn.
        /// </summary>
        public Element Active
        {
            get { return _Active; }
            set
            {
                _Active = value;
                NotifyPropertyChanged("Active");
            }
        }

        public override ElementCollection Elements
        {
            get { return Stage.Elements; }
        }

        /// <summary>
        /// Private backing member variable for the Log property
        /// </summary>
        private IActionLog _Log = null;

        /// <summary>
        /// The logger which stores and displays messages to the user.  May be null, in which case no messages will be displayed.
        /// </summary>
        public IActionLog Log
        {
            get { return _Log; }
            set
            {
                _Log = value;
                NotifyPropertyChanged("Log");
            }
        }

        /// <summary>
        /// The random number generator used to provide randomisation
        /// </summary>
        public Random RNG { get; set; } = new Random();

        /// <summary>
        /// The time delay to be used between the end of the player turn and the start of AI movement
        /// </summary>
        private double _AITurnDelay = 0.15;

        /// <summary>
        /// Time remaining before AI turns can begin
        /// </summary>
        private double _AITurnCountDown = 0;

        #endregion

        #region Constructor

        public RLState()
        {
           

        }

        #endregion

        #region Methods

        public override void StartUp()
        {
            base.StartUp();
            EndTurnOf(Controlled);
        }

        public override void Update(UpdateInfo info)
        {
            base.Update(info);
            if (_AITurnCountDown > 0)
            {
                _AITurnCountDown -= info.TimeStep;
                if (_AITurnCountDown <= 0)
                {
                    _AITurnCountDown = 0;
                    NextTurn();
                }
            }
        }

        /// <summary>
        /// Called when the user releases a key or button
        /// </summary>
        /// <param name="input">The input function pressed</param>
        /// <param name="direction">The direction associated with the input,
        /// if applicable.</param>
        public override void InputRelease(InputFunction input, Vector direction)
        {
            base.InputRelease(input, direction);
            Element controlled = Controlled;
            if (controlled != null && controlled == Active)
            {
                var aA = controlled.GetData<AvailableActions>();

                GameAction action = null;

                if (direction.IsValidNonZero())
                {
                    //Find targeted cell:
                    MapData mD = controlled.GetData<MapData>();
                    if (mD != null && mD.MapCell != null)
                    {
                        MapCell newCell = Stage.Map.AdjacentCell(mD.MapCell.Index, direction);
                        action = aA.ActionForInput(input, newCell.Index);
                    }
                }

                // Haven't found a targeted action; fallback to:
                if (action == null) action = aA.ActionForInput(input);
                if (action != null)
                {
                    action.Enact(Log, new EffectContext(controlled, this, direction));
                    EndTurnOf(controlled);
                }

            }
        }

        /// <summary>
        /// Process the start of the turn
        /// </summary>
        /// <param name="element"></param>
        public void StartTurnOf(Element element)
        {
            var context = new TurnContext(this, Stage, element, RNG);
            Active = element;
            for (int i = 0; i < element.Data.Count; i++)
            {
                IElementDataComponent dC = element.Data[i];
                if (dC is IStartOfTurn)
                {
                    ((IStartOfTurn)dC).StartOfTurn(context);
                }
                if (dC is IDeletable && ((IDeletable)dC).IsDeleted)
                {
                    element.Data.RemoveAt(i);
                    i--;
                }
            }

            if (element != Controlled)
            {
                var ai = new ActionSelectionAI(); //TEMP?
                var actions = element.GetData<AvailableActions>();
                GameAction tAction = ai.SelectAction(context, actions.Actions);

                tAction.Enact(Log, new EffectContext(element, this));
                EndTurnOf(element);
            }
            else
            {
                var actions = element.GetData<AvailableActions>();
                if (actions == null || actions.Actions.Count == 0)
                {
                    EndTurnOf(element);
                }
            }
        }

        /// <summary>
        /// Process the end of the turn
        /// </summary>
        public void EndTurnOf(Element element)
        {
            var context = new TurnContext(this, Stage, element, RNG);
            for (int i = 0; i < element.Data.Count; i++)
            {
                IElementDataComponent dC = element.Data[i];
                if (dC is IEndOfTurn)
                {
                    ((IEndOfTurn)dC).EndOfTurn(context);
                }
                if (dC is IDeletable && ((IDeletable)dC).IsDeleted)
                {
                    element.Data.RemoveAt(i);
                    i--;
                }
            }

            // Clean up to end the turn
            CleanUpDeleted();

            if (!Controlled.IsDeleted && Controlled != element) //Pause after player's turn
                NextTurn();
            else
                DelayAITurn(); //Reset the countdown
        }

        /// <summary>
        /// Reset the AI turn delay countdown to provide a slight pause before
        /// the AI will take its turn.  Allows for player moves to be distingishable
        /// in the user interface.
        /// </summary>
        public void DelayAITurn()
        {
            _AITurnCountDown = _AITurnDelay;
        }

        /// <summary>
        /// Progress to the next turn
        /// </summary>
        public void NextTurn()
        {

            // Find element with next turn:
            Element next = null;
            int lowest = 0;
            foreach (Element element in Elements)
            {
                var tC = element.GetData<TurnCounter>();
                if (tC != null && (next == null || lowest > tC.CountDown))
                {
                    next = element;
                    lowest = tC.CountDown;
                }
            }
            // Adjust turn counters for everything:
            foreach (Element element in Elements)
            {
                var tC = element.GetData<TurnCounter>();
                if (tC != null)
                {
                    tC.CountDown -= lowest;
                }
            }
            // TODO: Switch to more efficient system?

            if (next != null)
            {
                next.GetData<TurnCounter>().CountDown += 1000;
                StartTurnOf(next);
            }
        }

        /// <summary>
        /// Clean up after any elements which have been deleted in this turn
        /// </summary>
        public void CleanUpDeleted()
        {
            for (int i = Elements.Count - 1; i >= 0; i--)
            {
                var element = Elements[i];
                if (element.IsDeleted)
                {
                    // Remove from active elements:
                    Elements.RemoveAt(i);

                    // Remove from map:
                    MapData mD = element.GetData<MapData>();
                    mD?.MapCell?.RemoveFromCell(element);

                    // TODO: Cleanup any other components that need it?
                }
            }
        }

        /// <summary>
        /// Save this state to the specified filepath
        /// </summary>
        /// <param name="filePath"></param>
        public bool Save(FilePath filePath)
        {
            var saveFile = new RLStateSaveFile();
            saveFile.State = this;
            return saveFile.SaveAs(filePath);
        }

        #endregion
    }
}
