using Godot;
using System;

namespace Player
{
	public partial class Player : CharacterBody2D
	{
		[Export]
		public float Speed = 150.0f;

		[Export]
		public float JumpHeight = -400.0f;

		[Export]
		public float Gravity = 980f;

		[Export]
		public float JumpProjectionSpeed = 150f;

		public AnimatedSprite2D animation;

		private StateMachine stateMachine;

		public void Init()
		{
			this.animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			this.stateMachine = GetNode<StateMachine>("StateMachine");

			stateMachine.Init(this, this.animation);
		}
		public override void _Ready()
		{
			base._Ready();
			Init();
		}

		public override void _PhysicsProcess(double delta)
		{
			this.stateMachine.PhysicsProcess(delta);
		}

		public override void _Process(double delta)
		{
			this.stateMachine.Process(delta);
		}

		public override void _Input(InputEvent @event)
		{
			this.stateMachine.Input(@event);
		}

	}

}
