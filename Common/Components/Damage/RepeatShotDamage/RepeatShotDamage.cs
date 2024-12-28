using Components;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Components
{
	public partial class RepeatShotDamage : ShotDamage
	{
		[Export]
		private float IntervalSeconds = 1f;

		[Export]
		private int RepeatCount = 1;

		private Timer GlobalTimer;

		private Dictionary<Hitbox, int> Hitboxes = new Dictionary<Hitbox, int>();

		public override void _Ready()
		{
			base._Ready();

			GlobalTimer = new Timer
			{
				OneShot = false,
				WaitTime = IntervalSeconds
			};

			AddChild(GlobalTimer);
			GlobalTimer.Timeout += OnTimeOut;
		}

		protected override void OnAreaExited(Area2D area)
		{
			base.OnAreaExited(area);

			if (area is not Hitbox) return;

			var hitbox = area as Hitbox;

			if (Hitboxes.ContainsKey(hitbox) || !IsDamagable(hitbox)) return;

			Hitboxes.Add(hitbox, RepeatCount);

			if (GlobalTimer.IsStopped())
			{
				GlobalTimer.OneShot = false;
				GlobalTimer.Start();
			}
		}

		private void OnTimeOut()
		{
			foreach (var pair in Hitboxes)
			{
				Hitbox hitbox = pair.Key;

				if (IsDamagable(hitbox) && pair.Value > 0)
				{
					Attack(hitbox);
					Hitboxes[hitbox] = pair.Value - 1;
				}
				else
				{
					Hitboxes.Remove(hitbox);
				}
			}

			if (!Hitboxes.Any())
			{
				GlobalTimer.Stop();
			}
		}

		public override void _ExitTree()
		{
			base._ExitTree();

			Hitboxes.Clear();
		}

	}
}
