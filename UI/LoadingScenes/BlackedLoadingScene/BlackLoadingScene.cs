using Godot;
using System;
using System.Threading.Tasks;
using Types;
public partial class BlackLoadingScene : LoadingScene
{
	private AnimationPlayer animationPlayer;
	private TaskCompletionSource<bool> _currentAnimationTask;
	public override void _Ready()
	{
		this.animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.AnimationFinished += OnAnimationFinished;
	}

	public override async Task PlayAnimation(string animationName)
	{
		if (_currentAnimationTask != null && !_currentAnimationTask.Task.IsCompleted)
		{
			GD.PrintErr("Previous animation task is still running.");
			return;
		}
		_currentAnimationTask = new TaskCompletionSource<bool>();
		this.animationPlayer.Play(animationName);

		await _currentAnimationTask.Task;
	}

	private void OnAnimationFinished(StringName finishedAnimationName)
	{
		if (_currentAnimationTask != null && !_currentAnimationTask.Task.IsCompleted)
		{
			_currentAnimationTask.SetResult(true);
		}
	}
}
