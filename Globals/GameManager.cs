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
        }

        public void ChangeState(GameState newState)
        {
            this.CurrentState = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    // LoadScene("res://Scenes/MainMenu.tscn");
                    break;
                case GameState.Playing:
                    // star the game
                    break;
                case GameState.Paused:
                    GetTree().Paused = true;
                    break;
                case GameState.GameOver:
                    // Handle game over logic
                    break;
            }
        }

        public void SetScore(int amount)
        {
            this.PlayerScore = amount;
            MessageBus.Instance.EmitScoreChanged(this.PlayerScore);
        }

        public void SetHitpoints(int currentHitpoints)
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

        private void LoadScene(string scenePath)
        {
            var tree = GetTree();
            tree.ChangeSceneToFile(scenePath);
        }

        // create a level manager or scene manager. which replace the current scene from the root sceen node.

        public enum GameState
        {
            MainMenu,
            Playing,
            Paused,
            GameOver
        }
    }
}