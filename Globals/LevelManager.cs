using System.Collections.Generic;
using Godot;
using Types;

namespace Globals
{
    public partial class LevelManager : Node
    {
        public static LevelManager Instance { get; private set; }

        private int CurrentLevelIndex = 0;

        private List<string> LevelScenes = new List<string> { "res://Levels/Level1/Level1.tscn" };

        private Level CurrentLevel = null;

        public int CurrentLevelNumber
        {
            get => this.CurrentLevelIndex + 1;
        }

        public void Initialized(int CurrentLevel)
        {
            this.CurrentLevelIndex = CurrentLevel - 1;
        }

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

        public void LoadCurrentLevel()
        {
            LoadLevel(this.CurrentLevelIndex + 1);
        }

        /// <summary>
        /// This method is responsible for loading specified level. If the level number is greater than the available levels, it loads the last level. If it is less than 1, it loads the first level.
        /// </summary>
        /// <param name="LevelNumber"></param>
        public async void LoadLevel(int LevelNumber)
        {
            if (LevelNumber > LevelScenes.Count)
            {
                GD.PrintErr($"There is no level number {LevelNumber}");
                LevelNumber = LevelScenes.Count;
            }
            if (LevelNumber < 1)
            {
                GD.PrintErr($"There is no level number {LevelNumber}");
                LevelNumber = 0;
            }
            var LevelIndex = LevelNumber - 1;

            var levelScenePath = LevelScenes[LevelIndex];
            CurrentLevelIndex = LevelIndex;

            CurrentLevel = await SceneManager.Instance.LoadMainScene<Level>(levelScenePath);
            MessageBus.Instance.EmitLevelChanged(CurrentLevelIndex + 1, CurrentLevel);
        }

        /// <summary>
        /// This method is responsible for reloading the current Level
        /// </summary>
        public async void ReloadLevel()
        {
            await SceneManager.Instance.LoadMainScene<Level>(LevelScenes[CurrentLevelIndex]);
            MessageBus.Instance.EmitLevelReloaded(CurrentLevelIndex + 1, CurrentLevel);
        }

        /// <summary>
        /// This method is used for loading the next level. If there is no level remaining, it will sends the GameFinished signal
        /// </summary>
        public async void LoadNextLevel()
        {
            var nextLevelIndex = this.CurrentLevelIndex + 1;
            if (nextLevelIndex >= this.LevelScenes.Count)
            {
                MessageBus.Instance.EmitGameFinished();
                return;
            }
            this.CurrentLevelIndex = nextLevelIndex;
            this.CurrentLevel = await SceneManager.Instance.LoadMainScene<Level>(LevelScenes[CurrentLevelIndex]);
            MessageBus.Instance.EmitLevelChanged(CurrentLevelIndex + 1, CurrentLevel);
        }

        /// <summary>
        /// This method is used to load the previous level.
        /// </summary>
        public async void LoadPreviousLevel()
        {
            var previousLevelIndex = this.CurrentLevelIndex - 1;
            if (previousLevelIndex < 0 || previousLevelIndex >= this.LevelScenes.Count)
            {
                GD.PrintErr("There is no previous level remaining");
                return;
            }
            this.CurrentLevelIndex = previousLevelIndex;
            this.CurrentLevel = await SceneManager.Instance.LoadMainScene<Level>(LevelScenes[CurrentLevelIndex]);
            MessageBus.Instance.EmitLevelChanged(CurrentLevelIndex + 1, CurrentLevel);
        }

        public void PauseCurrentLevel()
        {
            if (CurrentLevel != null)
            {
                this.CurrentLevel.ProcessMode = ProcessModeEnum.Disabled;
            }
        }

        public void ResumeCurrentLevel()
        {
            if (CurrentLevel != null)
            {
                this.CurrentLevel.ProcessMode = ProcessModeEnum.Inherit;
            }
        }
    }
}