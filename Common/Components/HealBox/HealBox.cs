using Components;
using Godot;
using System;

public partial class HealBox : Area2D
{
	[Signal]
	public delegate void OnHealEventHandler(Heal healComponent);

	[Export]
	public Hitpoint Hitpoint;

	[Export]
	public string Layer;

	public bool IsDamaged => Hitpoint.IsDamaged;

	public bool IsDied => Hitpoint.IsDied;

	public int Heal(int amount)
	{
		if (!IsDamaged) return 0;
		int actualHealAmount = this.Hitpoint.Heal(amount);
		if (actualHealAmount != 0)
		{
			EmitSignal(nameof(this.OnHeal), actualHealAmount);
		}
		return actualHealAmount;
	}

	public int InstantHeal()
	{
		if (!IsDamaged) return 0;
		int healAmount = this.Hitpoint.HealToMax();
		if (healAmount != 0)
		{
			EmitSignal(nameof(this.OnHeal), healAmount);
		}
		return healAmount;
	}
}
