using System.Threading;
using Godot;

namespace Globals
{
    public partial class GameManager : Node
    {
        // Stores the game manager singleton instance
        public static GameManager Instance { get; private set; }

        // Store the current state of the game
        public GameState CurrentState { get; private set; }

        // Player score
        public int PlayerScore = 0;

        // Player current Hitpoints
        public int PlayerCurrentHitpoints = 2500;

        // Player Max Hitpoints
        public int PlayerMaxHitpoints { get; private set; } = 2500;

        // This will initialize all other managers
        private void InitializeManagers()
        {
            // TODO: This current level number should be get using the save manager.
            LevelManager.Instance.Initialized(1);
            HUDManager.Instance.Initialize(hudScenePath: "res://UI/HUD/hud.tscn");
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

            // Load the Main UI
            SceneManager.Instance.LoadScene("res://UI/MainUi/main_ui.tscn");
            ChangeState(GameState.MainMenu);
        }

        public void ChangeState(GameState newState)
        {
            this.CurrentState = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    MessageBus.Instance.EmitStateChanged("mainMenu");
                    // LoadScene("res://Scenes/MainMenu.tscn");
                    break;
                case GameState.Playing:
                    MessageBus.Instance.EmitStateChanged("playing");
                    // star the game
                    break;
                case GameState.Paused:
                    MessageBus.Instance.EmitStateChanged("paused");
                    GetTree().Paused = true;
                    break;
                case GameState.GameOver:
                    MessageBus.Instance.EmitStateChanged("gameOver");
                    // Handle game over logic
                    break;
            }
        }

        public void SetScore(int amount)
        {
            this.PlayerScore = amount;
            MessageBus.Instance.EmitScoreChanged(this.PlayerScore);
        }

        public void SetCurrentHitpoints(int currentHitpoints)
        {
            this.PlayerCurrentHitpoints = currentHitpoints;
            MessageBus.Instance.EmitHitpointChanged(currentHitpoints, this.PlayerMaxHitpoints);
        }

        public void IncreaseScore(int points)
        {
            PlayerScore += points;
            MessageBus.Instance.EmitScoreChanged(PlayerScore);
        }
        public void ResetGame()
        {
            this.PlayerCurrentHitpoints = 2500;
            this.PlayerMaxHitpoints = 2500;
            this.PlayerScore = 0;
            ChangeState(GameState.Playing);
        }

        public void ExitGame()
        {
            GetTree().Quit();
        }

        public enum GameState
        {
            MainMenu,
            Playing,
            Paused,
            GameOver
        }
    }
}