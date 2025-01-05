using Godot;

namespace Globals
{
    public partial class LevelManager : Node
    {
        public static LevelManager Instance { get; private set; }

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
    }
}