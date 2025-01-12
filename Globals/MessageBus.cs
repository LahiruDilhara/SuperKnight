using Godot;
using System;

namespace Globals
{
    public partial class MessageBus : Node
    {
        // Stores the game manager singleton instance
        public static MessageBus Instance { get; private set; }

        // Signals for global events
        /// <summary>
        /// There are states like gamePlay, paused, gameOver
        /// </summary>
        /// <param name="stateName"></param>
        [Signal]
        public delegate void StateChangedEventHandler(string stateName);

        [Signal]
        public delegate void HitpointChangedEventHandler(int currentHitpoints, int MaxHitpoints);

        [Signal]
        public delegate void ScoreChangedEventHandler(int score);

        [Signal]
        public delegate void GameFinishedEventHandler();

        [Signal]
        public delegate void LevelChangedEventHandler(int levelNumber);

        [Signal]
        public delegate void LevelReloadedEventHandler(int levelNumber);

        public override void _Ready()
        {
            base._Ready();
            // check if an instance already exists
            if (Instance != null && Instance != this)
            {
                // remove current instance if there is already one exists
                QueueFree();
                return;
            }
            Instance = this;
            GD.Print("MessageBus Initialized");
        }

        public void EmitStateChanged(string stateName)
        {
            EmitSignal(nameof(this.StateChanged), stateName);
        }

        public void EmitHitpointChanged(int currentHitpoints, int MaxHitpoints)
        {
            EmitSignal(nameof(this.HitpointChanged), currentHitpoints, MaxHitpoints);
        }

        public void EmitScoreChanged(int score)
        {
            EmitSignal(nameof(this.ScoreChanged), score);
        }

        public void EmitGameFinished()
        {
            EmitSignal(nameof(this.GameFinished));
        }
        public void EmitLevelChanged(int levelNumber)
        {
            EmitSignal(nameof(this.LevelChanged), levelNumber);
        }
        public void EmitLevelReloaded(int levelNumber)
        {
            EmitSignal(nameof(this.LevelReloaded), levelNumber);
        }

        public void ConnectSignal(string signalName, Callable callable)
        {
            if (!this.IsConnected(signalName, callable))
            {
                this.Connect(signalName, callable);
            }
        }
        public void DisconnectSignal(string signalName, Callable callable)
        {
            if (this.IsConnected(signalName, callable))
            {
                this.Disconnect(signalName, callable);
            }
        }
    }
}
