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
		// Set the current progress
		int CurrentProgress = CalProgress(currentHitpoints);
		this.progressBar.Value = CurrentProgress;

		// Set the progress bar color
		var styleBox = progressBar.GetThemeStylebox("fill", "ProgressBar") as StyleBoxFlat;
		if (styleBox != null)
		{
			styleBox.BgColor = Color.FromHsv(ProgressHue(CurrentProgress), 1f, 0.8f, 1f);
		}
	}

	private int CalProgress(int currentHitpoints)
	{
		return currentHitpoints * 100 / hitpoint.MaxHitpoints;
	}
	private float ProgressHue(int currentProgress)
	{
		int MaxHue = 130;
		int MaxPercentage = 100;
		int HueValue = MaxHue * currentProgress / MaxPercentage;
		return HueValue / 360.0f;
	}
}
