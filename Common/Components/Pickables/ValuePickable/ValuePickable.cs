using Godot;

namespace Components.Pickables
{
	public partial class ValuePickable : Pickable
	{
		[Export]
		public int Value = 0;

		[Export]
		public int Multiplier = 1;

		public int GetValue()
		{
			return Value * this.Multiplier;
		}
	}

}
