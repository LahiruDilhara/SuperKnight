using Components;
using Godot;
using System;

public partial class Coin : Node2D
{
	public override void _Ready()
	{
		var node = GetNode<Pickable>("Pickable");
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

	private void CoinCollectCallable(Node2D body)
	{
		GD.Print("Call the callable in coin");
	}
}
