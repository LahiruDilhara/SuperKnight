using Godot;

namespace Globals
{
    public partial class UIManager : Node
    {
        public static UIManager Instance { get; private set; }

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
    }
}