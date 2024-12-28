using Components;
using Godot;
using System;
using System.Collections.Generic;

public partial class ContinousDamage : ShotDamage
{
	[Export]
	private float IntervalSeconds = 1f;

	[Export]
	private int Incremental = 0;


	private Dictionary<Hitbox, HitBoxStats> Hitboxes = new Dictionary<Hitbox, HitBoxStats>();

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

		if (Hitboxes.ContainsKey(hitbox) || hitbox.IsDead) return;

		Timer timer = new Timer
		{
			OneShot = true,
			WaitTime = IntervalSeconds
		};

		AddChild(timer);

		timer.Timeout += () => OnTimeOut(hitbox);

		HitBoxStats hitBoxStats = new HitBoxStats
		{
			timer = timer,
			amount = Amount
		};

		Hitboxes.Add(hitbox, hitBoxStats);

		timer.Start();
	}

	protected override void OnAreaExited(Area2D area)
	{
		base.OnAreaExited(area);

		var hitbox = area as Hitbox;
		if (!Hitboxes.ContainsKey(hitbox)) return;

		if (IsInstanceValid(Hitboxes[hitbox].timer))
		{
			Hitboxes[hitbox].timer.QueueFree();
		}

		Hitboxes.Remove(hitbox);
	}

	protected void OnTimeOut(Hitbox hitbox)
	{
		if (hitbox == null || !Hitboxes.ContainsKey(hitbox) || !IsInstanceValid(hitbox)) return;

		// If dead clean the timer and remove the hitbox from the dictionary
		if (hitbox.IsDead)
		{
			if (IsInstanceValid(Hitboxes[hitbox].timer))
			{
				Hitboxes[hitbox].timer.QueueFree();
			}
			Hitboxes.Remove(hitbox);
			return;
		}

		// Attack using the incremented amount
		Attack(hitbox, Hitboxes[hitbox].amount);

		// increment the next damage amount by the increment
		Hitboxes[hitbox].amount += Incremental;

		// Start timer again
		Hitboxes[hitbox].timer.Start();
	}

	public override void _EnterTree()
	{
		base._EnterTree();

		foreach (var hitBoxStats in Hitboxes.Values)
		{
			if (hitBoxStats != null && IsInstanceValid(hitBoxStats.timer))
			{
				hitBoxStats.timer.QueueFree();
			}
		}

		Hitboxes.Clear();
	}
}

class HitBoxStats
{
	public Timer timer;
	public int amount;
}
