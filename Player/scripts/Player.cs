using Components;
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

		public AnimatedSprite2D Animation;

		private StateMachine stateMachine;

		private Hitpoint hitpoint;

		public void Init()
		{
			this.Animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			this.stateMachine = GetNode<StateMachine>("StateMachine");
			this.hitpoint = GetNode<Hitpoint>("Hitpoint");

		}
		public override void _Ready()
		{
			base._Ready();
			Init();
			stateMachine.Init(character: this, Animation: Animation);
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
