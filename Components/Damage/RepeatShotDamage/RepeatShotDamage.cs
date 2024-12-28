using Components;
using Godot;
using System.Collections.Generic;

namespace Components
{
	public partial class RepeatShotDamage : ShotDamage
	{
		[Export]
		private float IntervalSeconds = 1f;

		[Export]
		private int RepeatCount = 1;

		private Dictionary<Hitbox, HitBoxStatus> Hitboxes = new Dictionary<Hitbox, HitBoxStatus>();

		protected override void OnAreaExited(Area2D area)
		{
			base.OnAreaExited(area);

			if (area is not Hitbox) return;

			var hitbox = area as Hitbox;

			if (Hitboxes.ContainsKey(hitbox) || hitbox.IsDead) return;

			var timer = new Timer
			{
				OneShot = true,
				WaitTime = IntervalSeconds
			};

			AddChild(timer);

			timer.Timeout += () => OnTimeOut(hitbox);

			HitBoxStatus hitBoxStatus = new HitBoxStatus
			{
				counter = RepeatCount,
				timer = timer
			};

			timer.Start();

			Hitboxes.Add(hitbox, hitBoxStatus);
		}

		private void OnTimeOut(Hitbox hitbox)
		{
			if (hitbox == null || !Hitboxes.ContainsKey(hitbox) || !IsInstanceValid(hitbox)) return;

			var hitBoxStatus = Hitboxes[hitbox];

			if (hitBoxStatus.counter <= 0 || hitbox.IsDead)
			{
				// Free the timer and remove the hitbox from the dictionary
				if (IsInstanceValid(hitBoxStatus.timer))
				{
					hitBoxStatus.timer.QueueFree();
				}
				Hitboxes.Remove(hitbox);
				return;
			}

			// Apply damage and decrement the counter
			Attack(hitbox);
			hitBoxStatus.counter--;

			// Restart the timer for the next damage tick
			hitBoxStatus.timer.Start();
		}

		public override void _ExitTree()
		{
			base._ExitTree();

			foreach (var hitBoxState in Hitboxes.Values)
			{
				if (hitBoxState.timer != null && IsInstanceValid(hitBoxState.timer))
				{
					hitBoxState.timer.QueueFree();
				}
			}

			Hitboxes.Clear();
		}

	}

	class HitBoxStatus
	{
		public Timer timer;
		public int counter;
	}
}
