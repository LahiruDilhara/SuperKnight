using Godot;

namespace Globals
{
    public partial class HUDManager : Node
    {
        public static HUDManager Instance { get; private set; }

        private string HudScenePath;

        public void Initialize(string hudScenePath = "res://UI/HUD/hud.tscn")
        {
            this.HudScenePath = hudScenePath;
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
            GD.Print("HUDManager Initialized");
        }
    }
}