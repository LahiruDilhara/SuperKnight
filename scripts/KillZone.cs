using Godot;
using System;

public partial class KillZone : Area2D
{

	private Timer RestartTimer;
	public override void _Ready()
	{
		init();
	}

	private void init(){
		this.RestartTimer = GetNode<Timer>("RestartTimer");

		Connect("body_entered",new Callable(this,nameof(this.OnBodyEnteredHandler)));
		RestartTimer.Connect("timeout", new Callable(this, nameof(this.OnRestartTimerTimeOut)));
	}

	public override void _Process(double delta)
	{
	}

	private void OnBodyEnteredHandler(Node2D body){
		// body.QueueFree();
		// if not add this check, this will called when timeMap is colied because body_entered signal called when PhysicsBody2D or TimeMap entered
		if(body is PhysicsBody2D){


			// slow motion the engine when collide
			Engine.TimeScale = 0.5;
			// get the player collition object and remove it
			body.GetNode<CollisionPolygon2D>("CollisionPolygon2D").QueueFree();
			RestartTimer.Start();
		}
	}

	private void OnRestartTimerTimeOut(){
		Engine.TimeScale = 1;
		GetTree().ReloadCurrentScene();
	}
}
