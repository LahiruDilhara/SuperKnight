using Godot;
using Libs;
using System;

namespace Player
{
	public partial class Jump : State
	{
		private Player player;

		[Export]
		private State Idel;

		public override void Enter()
		{
			base.Enter();

			// Cast the Node to Player
			this.player = base.SuperNode as Player;

			// set jump animation
			Animation?.Play("jump");

			// set jump velocity
			player.Velocity += new Vector2(x: 0, y: player.JumpHeight);
			player.MoveAndSlide();
		}
		public override void ProcessUpdate(float delta)
		{
			base.ProcessUpdate(delta);

			var VelocityVector = player.Velocity;

			var direction = Input.GetAxis("left", "right");

			// change to idel if the player hit ground
			if (player.IsOnFloor())
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

			VelocityVector.X = direction * player.JumpProjectionSpeed;

			player.Velocity = VelocityVector;
			player.MoveAndSlide();
		}

		public override void PhysicsUpdate(float delta)
		{
			base.PhysicsUpdate(delta);
			var projectionY = player.Gravity * delta;
			player.Velocity += new Vector2(y: projectionY, x: 0);
		}

		public override void Exit()
		{
			base.Exit();
		}
	}

}
