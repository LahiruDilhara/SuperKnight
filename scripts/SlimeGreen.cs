using Godot;
using System;

public partial class SlimeGreen : Node2D
{
	[Export]
	private int Speed = 400;

	private short direction = 1;

	private RayCast2D RayCastRight;
	private RayCast2D RayCastLeft;

	private AnimatedSprite2D animatedSprite2D;

	public override void _Ready()
	{
		init();
	}

	private void init(){
		this.RayCastRight = GetNode<RayCast2D>("RayCastRight");
		this.RayCastLeft = GetNode<RayCast2D>("RayCastLeft");
		this.animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
    public override void _Process(double delta)
	{
		CalculateDirection();
		var vector = new Vector2(y:0,x: (float) (this.direction * Speed * delta));
		Position += vector;
	}

	private void CalculateDirection(){
		if(this.RayCastRight.IsColliding()){
			direction = -1;
			this.animatedSprite2D.FlipH = true;
		}
		else if(this.RayCastLeft.IsColliding()){
			direction = 1;
			this.animatedSprite2D.FlipH = false;
		}
	}
}
