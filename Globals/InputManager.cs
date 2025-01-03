using Godot;
using Player;
using System;
using System.Collections.Generic;


namespace Globals
{
    public partial class InputManager : Node
    {
        // expose the global singleton instance access
        public static InputManager Instance;

        // store all the actions
        private Dictionary<string, bool> Actions = new Dictionary<string, bool>();

        private bool InputEnable = true;

        public override void _Ready()
        {
            base._Ready();
            Instance = this;
            InitilizeDefaultActions();
        }

        /// <summary>
        /// Initialize the default actions. currently this initialize the jump, left, right actions
        /// </summary>
        private void InitilizeDefaultActions()
        {
            AddAction("jump");
            AddAction("left");
            AddAction("right");
        }

        /// <summary>
        /// This method should add the action to the dictionary and map it in the InputMap.
        /// </summary>
        /// <param name="action"></param>
        private void AddAction(string action)
        {
            Actions.Add(action, false);
        }

        public bool IsActionPressed(string action)
        {
            if (!InputEnable) return false;
            return Input.IsActionPressed(action);
        }

        public bool IsActionWasPressed(string action)
        {
            if (!InputEnable) return false;
            return Input.IsActionJustPressed(action);
        }

        public bool IsActionWasReleased(string action)
        {
            if (!InputEnable) return false;
            return Input.IsActionJustReleased(action);
        }
    }
}