using Components;
using Globals;
using Godot;
using System;
using Types;

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
		private PicableCollector picableCollector;

		public override void _Ready()
		{
			base._Ready();
			this.Animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			this.stateMachine = GetNode<StateMachine>("StateMachine");
			this.hitpoint = GetNode<Hitpoint>("Hitpoint");
			this.healBox = GetNode<HealBox>("HealBox");
			this.hitbox = GetNode<Hitbox>("Hitbox");
			this.picableCollector = GetNode<PicableCollector>("PicableCollector");

			// Connect with signals
			// Connect with hitpoint component hitpoint change signal
			if (!hitpoint.IsConnected(nameof(hitpoint.HitpointChange), new Callable(this, nameof(this.OnHitpointChange))))
			{
				hitpoint.Connect(nameof(hitpoint.HitpointChange), new Callable(this, nameof(this.OnHitpointChange)));
			}
			// Connect with hitpoint component died signal
			if (!hitpoint.IsConnected(nameof(hitpoint.Died), new Callable(this, nameof(this.OnDead))))
			{
				hitpoint.Connect(nameof(hitpoint.Died), new Callable(this, nameof(this.OnDead)));
			}
			// Connect with pickable collector component picked signal
			if (!picableCollector.IsConnected(nameof(picableCollector.Picked), new Callable(this, nameof(this.OnPick))))
			{
				picableCollector.Connect(nameof(picableCollector.Picked), new Callable(this, nameof(this.OnPick)));
			}

			stateMachine.Initialize(character: this, Animation: Animation);
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

		// Signal Handlers
		private void OnHitpointChange(int currentHitpoint)
		{
			MessageBus.Instance.EmitHitpointChanged(currentHitpoint, this.hitpoint.MaxHitpoints);
		}
		private void OnDead()
		{

		}
		private void OnPick(Node pickedNode)
		{
			if (pickedNode is Coin coin)
			{
				// If it is a coin, then increase the score
				StateManager.Instance.IncreaseScore(coin.GetValue());
			}
		}
	}
}