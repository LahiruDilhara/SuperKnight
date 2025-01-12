using Godot;
using Types;

namespace Globals
{
    public partial class StateManager : Node
    {
        public static StateManager Instance { get; private set; }

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

            MessageBus.Instance.ConnectSignal(nameof(MessageBus.Instance.ChangeState), new Callable(this, nameof(this.ChangeState)));
        }

        public async void ChangeState(string newState)
        {

            switch (newState)
            {
                case GameState.MainMenu:
                    await UIManager.Instance.ShowMainUI();
                    break;
                case GameState.GamePlay:
                    break;
                case GameState.Paused:
                    break;
                case GameState.GameOver:
                    break;
                default:
                    return;
            }

            this.CurrentState = newState;
            MessageBus.Instance.EmitStateChanged(CurrentState);
        }
    }
}