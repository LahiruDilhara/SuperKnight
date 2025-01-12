using System.Collections.Generic;
using Godot;

namespace Globals
{
    public partial class UIManager : Node
    {
        public static UIManager Instance { get; private set; }

        private Dictionary<string, string> UIScenePaths = new Dictionary<string, string> {
            {"mainUI","res://UI/MainUi/main_ui.tscn"}
        };

        public override void _Ready()
        {
            base._Ready();

            // check if an instance already exists
            if (Instance != null && Instance != this)
            {
                QueueFree();
                return;
            }

            Instance = this;
            GD.Print("UIManager Initialized");
        }

        public async void ShowMainUI()
        {
            if (UIScenePaths.ContainsKey("mainUI"))
            {
                await SceneManager.Instance.LoadScene(UIScenePaths["mainUI"]);
            }
        }
    }
}