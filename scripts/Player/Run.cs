using Godot;
using Libs;
using System;

namespace Player
{
	public partial class Run : State
	{

		private Player player;

		[Export]
		private State Idel;

		[Export]
		private State Jump;
		public override void Enter()
		{
			base.Enter();

			// Cast the Node to Player
			this.player = base.SuperNode as Player;

			this.player.animation.Play("run");
		}
		public override void ProcessUpdate(float delta)
		{
			var VelocityVector = player.Velocity;

			var direction = Input.GetAxis("left", "right");
			if (direction == 0f)
			{
				ChangeState(Idel);
				return;
			}

			if (direction == -1)
			{
				player.animation.FlipH = true;
			}
			else if (direction == 1)
			{
				player.animation.FlipH = false;
			}

			VelocityVector.X = direction * player.Speed;
			player.Velocity = VelocityVector;
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
			if (@event.IsActionPressed("jump"))
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
