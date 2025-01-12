using Godot;
using Types;

namespace Globals
{
    public partial class StateManager : Node
    {
        public static StateManager Instance { get; private set; }

        public string CurrentState { get; private set; }

        // Player score
        public int PlayerScore = 0;

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
                    LevelManager.Instance.LoadCurrentLevel();
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

        public void SetScore(int amount)
        {
            this.PlayerScore = amount;
            MessageBus.Instance.EmitScoreChanged(this.PlayerScore);
        }

        public void IncreaseScore(int points)
        {
            PlayerScore += points;
            MessageBus.Instance.EmitScoreChanged(PlayerScore);
        }
        public void ResetGame()
        {
            this.PlayerScore = 0;
        }
    }
}