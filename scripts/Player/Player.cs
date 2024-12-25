using Godot;
using System;

namespace Player
{
	public partial class Player : CharacterBody2D
	{
		[Export]
		public int RunSpeed = 150;

		[Export]
		public int JumpHeight = -400;

		[Export]
		public int Gravity = 980;

		[Export]
		public int JumpProjectionSpeed = 150;

		public AnimatedSprite2D Animation;

		private StateMachine stateMachine;

		public void Init()
		{
			this.Animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			this.stateMachine = GetNode<StateMachine>("StateMachine");

			stateMachine.Init(character: this, Gravity: Gravity, JumpProjectionSpeed: JumpProjectionSpeed, JumpHeight: JumpHeight, RunSpeed: RunSpeed, Animation: Animation);
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
