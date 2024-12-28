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

	// public int Heal(int amount){

	// }
}
