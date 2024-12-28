using Components;
using Godot;
using System;

public partial class ShotHeal : Heal
{
	[Export]
	protected int Amount = 0;

	[Export]
	protected bool OnEnter = true;

	[Export]
	protected bool OnExit = false;

	protected override void Healing(HealBox healBox)
	{
		if (!IsHealable(healBox)) return;
		int healAmount = healBox.Heal(Amount);
		SendHealSignal(healAmount);
	}

	protected override void OnAreaEntered(Area2D area)
	{
		if (area is HealBox healBox && OnEnter)
		{
			Healing(healBox);
		}
	}

	protected override void OnAreaExited(Area2D area)
	{
		if (area is HealBox healBox && OnExit)
		{
			Healing(healBox);
		}
	}
}
