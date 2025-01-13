using Godot;
using Player;

namespace Level
{
    public partial class Level : Node
    {
        public int PlayerMaxHitpoints { get; protected set; }

        protected Player.Player player = null;

        public override void _Ready()
        {
            // Every level should have a player node inside it, this will get reference to it to initialize it.
            this.player = GetNode<Player.Player>("Player");

            // Initialize
            // Initialize the player
            this.player.Initialize(this.PlayerMaxHitpoints);
        }
    }
}