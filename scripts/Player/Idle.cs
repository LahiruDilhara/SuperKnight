using Godot;
using Libs;
using System;

namespace Player
{
	public partial class Idle : State
	{
		[Export]
		private State Run;

		[Export]
		private State Jump;

		private Player player;
		public override void Enter()
		{
			base.Enter();
			this.player = base.SuperNode as Player;

			// set idel animation
			this.player.animation.Play("idle");

			this.player.Velocity = new Vector2(x: 0, y: 0);
		}
		public override void ProcessUpdate(float delta)
		{
			base.ProcessUpdate(delta);
			if (Input.IsActionPressed("left") || Input.IsActionPressed("right"))
			{
				ChangeState(Run);
				return;
			}
			player.MoveAndSlide();
		}

		public override void PhysicsUpdate(float delta)
		{
			base.PhysicsUpdate(delta);

			var projectionY = player.Gravity * delta;
			player.Velocity += new Vector2(y: projectionY, x: 0);
		}

		public override void InputUpdate(InputEvent @event)
		{
			if ((@event.IsActionPressed("left") || @event.IsActionPressed("right")) && player.IsOnFloor())
			{
				ChangeState(Run);
				return;
			}
			else if (@event.IsActionPressed("jump") && player.IsOnFloor())
			{
				ChangeState(Jump);
				return;
			}
		}

		public override void Exit()
		{
			base.Exit();
		}
	}

}
