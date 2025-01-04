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
		private HealBox healBox;
		private Hitbox hitbox;

		public override void _Ready()
		{
			base._Ready();
			this.Animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			this.stateMachine = GetNode<StateMachine>("StateMachine");
			this.hitpoint = GetNode<Hitpoint>("Hitpoint");
			this.healBox = GetNode<HealBox>("HealBox");
			this.hitbox = GetNode<Hitbox>("Hitbox");



			this.hitpoint.HitpointChange += HitpointChange;
			hitpoint.Died += Dead;

			stateMachine.Initialize(character: this, Animation: Animation);
		}
		public void Initialize()
		{

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

		public void HitpointChange(int hitpoints)
		{
			GD.Print($"The hitpoints are {hitpoints}");
		}

		public void Dead()
		{
			GD.Print($"The player over");
		}
	}

}
