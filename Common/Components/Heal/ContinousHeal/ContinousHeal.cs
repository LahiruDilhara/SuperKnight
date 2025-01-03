using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ContinousHeal : ShotHeal
{
	[Export]
	private float IntervalSeconds = 1f;

	[Export]
	private int Incremental = 0;

	protected Dictionary<HealBox, int> HealBoxes = new Dictionary<HealBox, int>();

	private Timer GlobalTimer;

	public override void _Ready()
	{
		base._Ready();
		GlobalTimer = new Timer
		{
			WaitTime = IntervalSeconds,
			OneShot = false
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
		base.OnAreaEntered(area);
		if (area is not HealBox) return;

		var healBox = area as HealBox;

		if (!IsHealable(healBox) || HealBoxes.ContainsKey(healBox)) return;

		if (GlobalTimer.IsStopped())
		{
			GlobalTimer.OneShot = false;
			GlobalTimer.Start();
		}

		HealBoxes.Add(healBox, Amount);
	}

	protected void OnTimeOut()
	{
		foreach (var pair in HealBoxes)
		{
			HealBox healBox = pair.Key;
			int amount = pair.Value;

			if (IsHealable(healBox))
			{
				Healing(healBox, amount);
			}
			else
			{
				HealBoxes.Remove(healBox);
			}
		}
	}

	protected override void OnAreaExited(Area2D area)
	{
		base.OnAreaExited(area);
		if (area is not HealBox) return;
		var healBox = area as HealBox;

		if (!IsHealable(healBox)) return;
		if (!HealBoxes.ContainsKey(healBox)) return;

		HealBoxes.Remove(healBox);

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
}
