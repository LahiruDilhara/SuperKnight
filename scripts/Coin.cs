using Godot;
using System;

public partial class Coin : Area2D
{
	public override void _Ready()
	{
		init();
	}

	private void init(){
		Connect("body_entered", new Callable(this,nameof(this.CoinCollectCallable)));
	}

	public override void _Process(double delta)
	{
	}

	private void CoinCollectCallable(Node2D body){
		Global.increaseScore();
		this.QueueFree();
	}
}
