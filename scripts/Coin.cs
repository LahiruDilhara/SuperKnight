using Components.Pickables;
using Godot;

public partial class Coin : Node2D
{
	public override void _Ready()
	{
		var node = GetNode<Pickable>("ValuePickable");
		node.NodeType = typeof(Player.Player);
		node.Pick += CoinCollectCallable;
	}

	private void init()
	{
		// Connect("body_entered", new Callable(this, nameof(this.CoinCollectCallable)));
	}

	public override void _Process(double delta)
	{
	}

	private void CoinCollectCallable()
	{
		GD.Print("Call the callable in coin");
	}
}
