using Components;
using Godot;
using System;

public partial class HealthProgress : Control
{
	[Export]
	private Hitpoint hitpoint;

	public void Initialize(Hitpoint hitpoint)
	{
		this.hitpoint = hitpoint;
	}
	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
	}
}
