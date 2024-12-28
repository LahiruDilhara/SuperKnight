using Components;
using Godot;
using System;

public partial class InstantHeal : Heal
{
	protected override void Healing(HealBox healBox)
	{
		if (!IsHealable(healBox)) return;
		int healAmount = healBox.InstantHeal();
		SendHealSignal(healAmount);
	}

	protected override void OnAreaEntered(Area2D area)
	{
		if (area is HealBox healBox)
		{
			Healing(healBox);
		}
	}
}
