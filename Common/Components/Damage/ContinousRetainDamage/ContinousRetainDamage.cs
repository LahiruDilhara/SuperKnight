using System;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Components
{
	public partial class ContinousRetainDamage : ContinousDamage
	{
		[Export]
		private bool UseIncrementedDamage = false;

		[Export]
		private float RetainInterval = 1f;

		[Export]
		private int RepeatCount = 1;

		private Dictionary<Hitbox, HitBoxStats> RetainHitBoxes = new Dictionary<Hitbox, HitBoxStats>();

		private Timer GlobalTimer;

		public override void _Ready()
		{
			base._Ready();

			GlobalTimer = new Timer
			{
				WaitTime = RetainInterval,
				OneShot = false,
			};

			AddChild(GlobalTimer);
			GlobalTimer.Timeout += OnTimeOut;
		}

		protected override void OnAreaExited(Area2D area)
		{
			if (area is not Hitbox) return;
			var hitBox = area as Hitbox;

			// Used the current settted amount as the Damage amount
			int damageAmount = Amount;

			// if specified to use the incremented damage, then change the damageAmount to previously incremented damage.
			if (Hitboxes.ContainsKey(hitBox) && UseIncrementedDamage)
			{
				damageAmount = Hitboxes[hitBox];
			}

			// call the base class to clear the damage
			base.OnAreaExited(area);

			// After all damages, if it is not damagable then return
			if (!IsDamagable(hitBox)) return;

			HitBoxStats hitBoxStats = new HitBoxStats
			{
				amount = damageAmount,
				count = RepeatCount,
			};

			RetainHitBoxes.Add(hitBox, hitBoxStats);

			// If the global timer was stopeed, then start it
			if (GlobalTimer.IsStopped())
			{
				GlobalTimer.Start();
				GlobalTimer.OneShot = false;
			}

		}

		private void OnTimeOut()
		{
			foreach (var pair in RetainHitBoxes)
			{
				Hitbox hitbox = pair.Key;

				if (IsDamagable(hitbox) && pair.Value.count > 0)
				{
					Attack(hitbox, pair.Value.amount);
					pair.Value.count -= 1;
				}
				else
				{
					RetainHitBoxes.Remove(hitbox);
				}
			}
			// If the dictionary is empty then stop the timer.
			if (!RetainHitBoxes.Any())
			{
				GlobalTimer.Stop();
			}
		}

		public override void _EnterTree()
		{
			base._EnterTree();
			RetainHitBoxes.Clear();
			GlobalTimer.QueueFree();
		}
	}

	public class HitBoxStats
	{
		public int count;

		public int amount;
	}

}
