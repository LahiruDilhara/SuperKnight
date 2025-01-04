using Components.Pickables;
using Godot;

public partial class Coin : Node2D
{
	[Export]
	public int Value = 0;

	[Export]
	public int Multiplier = 1;

	public Pickable pickable;
	public override void _Ready()
	{
		// Get references to the nodes
		pickable = GetNode<Pickable>("Pickable");

		// Initialize nodes
		pickable.Initialize(this.Clone());
	}

	public int GetValue()
	{
		return Value * Multiplier;
	}

	// Generate the clone
	private Coin Clone()
	{
		return new Coin
		{
			Value = this.Value,
			Multiplier = this.Multiplier
		};
	}
}
