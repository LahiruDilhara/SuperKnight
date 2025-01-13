using Globals;
using Godot;

namespace Types
{
    public partial class Level : Node
    {
        public int PlayerMaxHitpoints { get; protected set; }

        public override void _Ready()
        {
            base._Ready();
        }
    }
}