using Components;
using Godot;
using System;
using System.Collections.Generic;

public partial class InstantDelayDamage : InstantDamage
{
	private Timer timer;

	private readonly Dictionary<Hitbox, Timer> HitBoxes = new Dictionary<Hitbox, Timer>();

	[Export]
	private int DelaySeconds = 10;

	protected override void OnAreaEntered(Area2D area)
	{
		if (area is not Hitbox) return;
		var hitbox = area as Hitbox;

		if (HitBoxes.ContainsKey(hitbox)) return;

		Timer timer = new Timer
		{
			OneShot = true,
			WaitTime = DelaySeconds
		};

		// Add the timer as sub node
		AddChild(timer);

		timer.Timeout += () => OnDelayOver(timer, hitbox);

		// Start the timer
		timer.Start();

		// Store the Area2D and Hitbox
		HitBoxes.Add(hitbox, timer);
	}

	private void OnDelayOver(Timer timer, Hitbox hitbox)
	{
		Attack(hitbox);
		HitBoxes.Remove(hitbox);
		timer.QueueFree();
	}
}
