using Godot;

internal readonly struct AdobeAnimateGpuHitFlashState
{
	public float StartTime { get; }

	public float Strength { get; }

	public float Duration { get; }

	public bool Enabled { get; }

	public AdobeAnimateGpuHitFlashState(float startTime, float strength, float duration)
	{
		StartTime = Mathf.Max(0f, startTime);
		Strength = Mathf.Max(0f, strength);
		Duration = Mathf.Max(1E-06f, duration);
		Enabled = Strength > 0f;
	}
}
