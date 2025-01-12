using Godot;
using Types;

namespace Globals
{
    public partial class StateManager : Node
    {
        public StateManager Instance { get; private set; }

        public string CurrentState { get; private set; }

        public override void _Ready()
        {
            base._Ready();

            if (Instance != null && Instance != this)
            {
                QueueFree();
                return;
            }
            Instance = this;
            GD.Print("StateManager Initialized");
        }

        public async void ChangeState(string newState)
        {
            this.CurrentState = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.GamePlay:
                    break;
                case GameState.Paused:
                    break;
                case GameState.GameOver:
                    break;
            }
        }
    }
}