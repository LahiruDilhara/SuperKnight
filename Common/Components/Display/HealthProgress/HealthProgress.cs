using Components;
using Godot;

public partial class HealthProgress : Control
{
	[Export]
	private Hitpoint hitpoint;

	private ProgressBar progressBar;

	public void Initialize(Hitpoint hitpoint)
	{
		this.hitpoint = hitpoint;
	}
	public override void _Ready()
	{
		this.progressBar = GetNode<ProgressBar>("ProgressBar");

		if (!hitpoint.IsConnected(nameof(hitpoint.HitpointChange), new Callable(this, nameof(this.OnProgressChange))))
		{
			hitpoint.Connect(nameof(hitpoint.HitpointChange), new Callable(this, nameof(this.OnProgressChange)));
		}
	}

	private void OnProgressChange(int currentHitpoints)
	{
		this.progressBar.Value = CalProgress(currentHitpoints);
	}

	private int CalProgress(int currentHitpoints)
	{
		return currentHitpoints * 100 / hitpoint.MaxHitpoints;
	}
}
