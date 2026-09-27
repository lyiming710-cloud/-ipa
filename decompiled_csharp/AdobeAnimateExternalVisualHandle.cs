public readonly struct AdobeAnimateExternalVisualHandle
{
	internal int Index { get; }

	internal uint Generation { get; }

	public bool IsValid
	{
		get
		{
			if (Index >= 0)
			{
				return Generation != 0;
			}
			return false;
		}
	}

	public static AdobeAnimateExternalVisualHandle Invalid => new AdobeAnimateExternalVisualHandle(-1, 0u);

	internal AdobeAnimateExternalVisualHandle(int index, uint generation)
	{
		Index = index;
		Generation = generation;
	}
}
