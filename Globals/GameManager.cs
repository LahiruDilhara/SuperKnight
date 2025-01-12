using System.Threading;
using Godot;
using Types;

namespace Globals
{
    public partial class GameManager : Node
    {
        // Stores the game manager singleton instance
        public static GameManager Instance { get; private set; }

        // Player score
        public int PlayerScore = 0;

        // This will initialize all other managers
        private void InitializeManagers()
        {
            // TODO: This current level number should be get using the save manager.
            LevelManager.Instance.Initialized(1);
            SceneManager.Instance.Initialize(loadingScenePath: "res://UI/LoadingScenes/BlackedLoadingScene/blackLoadingScene.tscn");
        }

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
            GD.Print("GameManager Initialized");

            InitializeManagers();

            // Change State to Main UI
            StateManager.Instance.ChangeState(GameState.MainMenu);
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

        public void ExitGame()
        {
            GetTree().Quit();
        }
    }
}