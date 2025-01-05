using Godot;
using System;
using System.Threading.Tasks;

namespace UI
{
	public partial class LoadingScene : CanvasLayer
	{
		private AnimationPlayer animationPlayer;
		public override void _Ready()
		{
			this.animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		}

		public async Task PlayAnimation(string animationName)
		{
			this.animationPlayer.Play(animationName);
			var tcs = new TaskCompletionSource<bool>();

			animationPlayer.AnimationFinished += (animName) =>
			{
				if (animName == animationName)
				{
					tcs.SetResult(true);
				}
			};
			await tcs.Task;
		}
	}
}
