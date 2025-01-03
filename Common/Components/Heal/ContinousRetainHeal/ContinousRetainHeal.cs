using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ContinousRetainHeal : ContinousHeal
{
	[Export]
	private bool UseIncrementedHeal = false;

	[Export]
	private float RetainInterval = 1f;

	[Export]
	private int RepeatCount = 1;

	private Dictionary<HealBox, HealBoxStats> RetainHealBoxes = new Dictionary<HealBox, HealBoxStats>();

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
		if (area is not HealBox) return;
		var healBox = area as HealBox;

		// Used the current settted amount as the Damage amount
		int healAmount = Amount;

		// if specified to use the incremented heal, then change the healAmount to previously incremented heal.
		if (HealBoxes.ContainsKey(healBox) && UseIncrementedHeal)
		{
			healAmount = HealBoxes[healBox];
		}

		// call the base class to clear the heal
		base.OnAreaExited(area);

		// After all damages, if it is not damagable then return
		if (!IsHealable(healBox)) return;

		HealBoxStats hitBoxStats = new HealBoxStats
		{
			amount = healAmount,
			count = RepeatCount,
		};

		RetainHealBoxes.Add(healBox, hitBoxStats);

		// If the global timer was stopeed, then start it
		if (GlobalTimer.IsStopped())
		{
			GlobalTimer.Start();
			GlobalTimer.OneShot = false;
		}

	}

	private void OnTimeOut()
	{
		foreach (var pair in RetainHealBoxes)
		{
			HealBox hitbox = pair.Key;

			if (IsHealable(hitbox) && pair.Value.count > 0)
			{
				Healing(hitbox, pair.Value.amount);
				pair.Value.count -= 1;
			}
			else
			{
				RetainHealBoxes.Remove(hitbox);
			}
		}
		// If the dictionary is empty then stop the timer.
		if (!RetainHealBoxes.Any())
		{
			GlobalTimer.Stop();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		RetainHealBoxes.Clear();
		GlobalTimer.QueueFree();
	}
}


public class HealBoxStats
{
	public int count;

	public int amount;
}