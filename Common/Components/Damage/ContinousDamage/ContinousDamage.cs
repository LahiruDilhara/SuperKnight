using Components;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ContinousDamage : ShotDamage
{
	[Export]
	private float IntervalSeconds = 1f;

	[Export]
	private int Incremental = 0;

	protected Dictionary<Hitbox, int> Hitboxes = new Dictionary<Hitbox, int>();

	private Timer GlobalTimer;

	public override void _Ready()
	{
		base._Ready();
		GlobalTimer = new Timer
		{
			WaitTime = IntervalSeconds,
			OneShot = false,
		};

		AddChild(GlobalTimer);
		GlobalTimer.Timeout += OnTimeOut;
	}

	protected override void Attack(Hitbox hitbox, int amount)
	{
		if (!IsDamagable(hitbox)) return;
		int damageAmount = hitbox.Damage(amount);
		EmitSignal(nameof(OnAreaDamage), damageAmount);
	}

	protected override void OnAreaEntered(Area2D area)
	{
		base.OnAreaEntered(area);
		if (area is not Hitbox) return;

		var hitbox = area as Hitbox;

		if (!IsDamagable(hitbox) || Hitboxes.ContainsKey(hitbox)) return;

		if (GlobalTimer.IsStopped())
		{
			GlobalTimer.OneShot = false;
			GlobalTimer.Start();
		}

		Hitboxes.Add(hitbox, Amount);
	}

	protected override void OnAreaExited(Area2D area)
	{
		base.OnAreaExited(area);

		var hitbox = area as Hitbox;

		if (!IsDamagable(hitbox)) return;

		if (!Hitboxes.ContainsKey(hitbox)) return;

		Hitboxes.Remove(hitbox);

		if (!Hitboxes.Any())
		{
			GlobalTimer.Stop();
		}
	}
	private void OnTimeOut()
	{
		foreach (var pair in Hitboxes)
		{
			Hitbox hitBox = pair.Key;
			int amount = pair.Value;

			if (IsDamagable(hitBox))
			{
				Attack(hitBox, amount);
				Hitboxes[hitBox] = amount + Incremental;
			}
			else
			{
				Hitboxes.Remove(hitBox);
			}
		}
	}

	public override void _EnterTree()
	{
		base._EnterTree();

		Hitboxes.Clear();
	}
}