using System.Collections.Generic;
using Godot;
using Types;

namespace Globals
{
    public partial class LevelManager : Node
    {
        public static LevelManager Instance { get; private set; }

        private int CurrentLevelIndex = 0;

        public int CurrentLevel
        {
            get => this.CurrentLevelIndex + 1;
        }

        private List<string> LevelScenes = new List<string> { "res://Levels/Level1/Level1.tscn" };

        public override void _Ready()
        {
            base._Ready();
            if (Instance != null && Instance != this)
            {
                QueueFree();
                return;
            }
            Instance = this;
            GD.Print("LevelManager Initialized");

        }

        public void LoadLevel(int LevelNumber)
        {
            if (LevelNumber > LevelScenes.Count)
            {
                LevelNumber = LevelScenes.Count;
            }
            var LevelIndex = LevelNumber - 1;

            var levelScenePath = LevelScenes[LevelIndex];
            CurrentLevelIndex = LevelIndex;

            SceneManager.Instance.LoadScene(levelScenePath);
        }

        public void ReloadLevel()
        {
            SceneManager.Instance.LoadScene(LevelScenes[CurrentLevelIndex]);
        }

        public void LoadNextLevel()
        {
            var nextLevelIndex = this.CurrentLevelIndex + 1;
            if (nextLevelIndex >= this.LevelScenes.Count)
            {
                MessageBus.Instance.EmitGameFinished();
                return;
            }
            SceneManager.Instance.LoadScene(LevelScenes[CurrentLevelIndex]);
        }
    }
}