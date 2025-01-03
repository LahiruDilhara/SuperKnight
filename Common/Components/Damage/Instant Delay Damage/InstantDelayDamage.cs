using Components;
using Godot;
using System;
using System.Collections.Generic;

namespace Components
{
	public partial class InstantDelayDamage : InstantDamage
	{
		private Timer timer;

		private readonly Dictionary<Hitbox, Timer> HitBoxes = new Dictionary<Hitbox, Timer>();

		[Export]
		private int DelaySeconds = 10;

		protected override void OnAreaEntered(Area2D area)
		{
			if (area is not Hitbox) return;
			var hitbox = area as Hitbox;

			if (HitBoxes.ContainsKey(hitbox) || !IsDamagable(hitbox)) return;

			Timer timer = new Timer
			{
				OneShot = true,
				WaitTime = DelaySeconds
			};

			// Add the timer as sub node
			AddChild(timer);

			timer.Timeout += () => OnDelayOver(timer, hitbox);

			// Start the timer
			timer.Start();

			// Store the Area2D and Hitbox
			HitBoxes.Add(hitbox, timer);
		}

		private void OnDelayOver(Timer timer, Hitbox hitbox)
		{
			if (!hitbox.IsDead)
			{
				Attack(hitbox);
			}
			HitBoxes.Remove(hitbox);
			timer.QueueFree();
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			foreach (var timer in HitBoxes.Values)
			{
				if (timer != null && IsInstanceValid(timer))
				{
					timer.QueueFree();
				}
			}

			HitBoxes.Clear();
		}
	}

}
