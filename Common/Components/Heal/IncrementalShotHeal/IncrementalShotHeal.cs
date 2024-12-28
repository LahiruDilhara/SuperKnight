using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public partial class IncrementalShotHeal : ShotHeal
{
	[Export]
	protected float IntervalSeconds = 1f;

	[Export]
	private int Incremental = 1;

	private Timer GlobalTimer;

	private Dictionary<HealBox, HealingStats> HealBoxes = new Dictionary<HealBox, HealingStats>();

	public void SetIncremental(int value)
	{
		if (value > Amount)
		{
			Incremental = Amount;
		}
		else
		{
			Incremental = value;
		}
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		SetIncremental(Incremental);
	}

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

	protected override void Healing(HealBox healBox, int amount)
	{
		if (!IsHealable(healBox)) return;
		int healAmount = healBox.Heal(amount);
		SendHealSignal(healAmount);
	}

	protected override void OnAreaEntered(Area2D area)
	{
		if (area is HealBox healBox && OnEnter)
		{
			SetHealing(healBox);
		}
	}


	protected override void OnAreaExited(Area2D area)
	{
		if (area is HealBox healBox && OnExit)
		{
			SetHealing(healBox);
		}
	}

	private void SetHealing(HealBox healBox)
	{
		if (HealBoxes.ContainsKey(healBox) || !IsHealable(healBox)) return;

		HealingStats healingStats = new HealingStats
		{
			CurrentAmount = 0,
			TargetAmount = Amount,
			Incremental = Incremental
		};

		HealBoxes.Add(healBox, healingStats);

		if (GlobalTimer.IsStopped())
		{
			GlobalTimer.OneShot = false;
			GlobalTimer.Start();
		}
	}


	private void OnTimeOut()
	{
		foreach (var pair in HealBoxes)
		{
			HealBox healBox = pair.Key;

			if (IsHealable(healBox) && pair.Value.CurrentAmount < pair.Value.TargetAmount)
			{
				var remainingToHeal = pair.Value.TargetAmount - pair.Value.CurrentAmount;
				var healingAmount = remainingToHeal > pair.Value.Incremental ? pair.Value.Incremental : remainingToHeal;
				Healing(healBox, healingAmount);
				HealBoxes[healBox].CurrentAmount += healingAmount;
			}
			else
			{
				HealBoxes.Remove(healBox);
			}
		}

		if (!HealBoxes.Any())
		{
			GlobalTimer.Stop();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		HealBoxes.Clear();
		GlobalTimer.QueueFree();
	}
	private class HealingStats
	{
		public int TargetAmount;
		public int CurrentAmount;

		public int Incremental;
	}

}

