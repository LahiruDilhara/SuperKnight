using Controllers;
using Godot;
using System;

namespace Player
{
	public partial class State : Node
	{
		[Signal]
		public delegate void StateChangeEventHandler(State state);

		public CharacterBody2D Character;

		public AnimatedSprite2D Animation;

		public IController controller;

		public virtual void Enter()
		{
		}

		public virtual void ProcessUpdate(float delta) { }

		public virtual void InputUpdate(InputEvent @event) { }

		public virtual void PhysicsUpdate(float delta) { }

		public virtual void Exit() { }

		protected void ChangeState(State state)
		{
			EmitSignal(nameof(this.StateChange), state);
		}
	}
}