using System;
using Godot;

internal readonly struct AdobeAnimateCrowdResourceSignature : IEquatable<AdobeAnimateCrowdResourceSignature>
{
	private readonly Rid _visualAtlasRid;

	private readonly Rid _poseTextureRid;

	private readonly Rid _gpuGraphTextureRid;

	private readonly Rid _dynamicOverrideTextureRid;

	private readonly Rid _rasterCompositeAtlasRid;

	public TextureLayered VisualAtlas { get; }

	public Vector2 VisualAtlasSize { get; }

	public TextureLayered PoseTexture { get; }

	public Vector2I PoseTextureSize { get; }

	public Texture2DArray GpuGraphTexture { get; }

	public Vector2I GpuGraphTextureSize { get; }

	public ImageTexture DynamicOverrideTexture { get; }

	public Vector2I DynamicOverrideTextureSize { get; }

	public Texture2D RasterCompositeAtlas { get; }

	public Vector2I RasterCompositeAtlasSize { get; }

	public Vector2I RasterCompositeTileSize { get; }

	public Vector2 RasterCompositeOrigin { get; }

	public int RasterCompositeColumns { get; }

	internal AdobeAnimateCrowdResourceSignature(TextureLayered visualAtlas, Vector2 visualAtlasSize, TextureLayered poseTexture, Vector2I poseTextureSize, Texture2DArray gpuGraphTexture, Vector2I gpuGraphTextureSize, ImageTexture dynamicOverrideTexture, Vector2I dynamicOverrideTextureSize, AdobeAnimateRasterCompositeData rasterCompositeData = null)
	{
		VisualAtlas = visualAtlas;
		VisualAtlasSize = visualAtlasSize;
		PoseTexture = poseTexture;
		PoseTextureSize = poseTextureSize;
		GpuGraphTexture = gpuGraphTexture;
		GpuGraphTextureSize = gpuGraphTextureSize;
		DynamicOverrideTexture = dynamicOverrideTexture;
		DynamicOverrideTextureSize = dynamicOverrideTextureSize;
		RasterCompositeAtlas = rasterCompositeData?.atlas;
		RasterCompositeAtlasSize = (GodotObject.IsInstanceValid(RasterCompositeAtlas) ? new Vector2I(RasterCompositeAtlas.GetWidth(), RasterCompositeAtlas.GetHeight()) : Vector2I.Zero);
		RasterCompositeTileSize = rasterCompositeData?.tileSize ?? Vector2I.Zero;
		RasterCompositeOrigin = rasterCompositeData?.origin ?? Vector2.Zero;
		RasterCompositeColumns = rasterCompositeData?.columns ?? 0;
		_visualAtlasRid = GetRid(visualAtlas);
		_poseTextureRid = GetRid(poseTexture);
		_gpuGraphTextureRid = GetRid(gpuGraphTexture);
		_dynamicOverrideTextureRid = GetRid(dynamicOverrideTexture);
		_rasterCompositeAtlasRid = GetRid(RasterCompositeAtlas);
	}

	public bool Equals(AdobeAnimateCrowdResourceSignature other)
	{
		if (_visualAtlasRid == other._visualAtlasRid && VisualAtlasSize == other.VisualAtlasSize && _poseTextureRid == other._poseTextureRid && PoseTextureSize == other.PoseTextureSize && _gpuGraphTextureRid == other._gpuGraphTextureRid && GpuGraphTextureSize == other.GpuGraphTextureSize && _dynamicOverrideTextureRid == other._dynamicOverrideTextureRid && DynamicOverrideTextureSize == other.DynamicOverrideTextureSize && _rasterCompositeAtlasRid == other._rasterCompositeAtlasRid && RasterCompositeAtlasSize == other.RasterCompositeAtlasSize && RasterCompositeTileSize == other.RasterCompositeTileSize && RasterCompositeOrigin == other.RasterCompositeOrigin)
		{
			return RasterCompositeColumns == other.RasterCompositeColumns;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is AdobeAnimateCrowdResourceSignature other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add(_visualAtlasRid);
		hashCode.Add(VisualAtlasSize);
		hashCode.Add(_poseTextureRid);
		hashCode.Add(PoseTextureSize);
		hashCode.Add(_gpuGraphTextureRid);
		hashCode.Add(GpuGraphTextureSize);
		hashCode.Add(_dynamicOverrideTextureRid);
		hashCode.Add(DynamicOverrideTextureSize);
		hashCode.Add(_rasterCompositeAtlasRid);
		hashCode.Add(RasterCompositeAtlasSize);
		hashCode.Add(RasterCompositeTileSize);
		hashCode.Add(RasterCompositeOrigin);
		hashCode.Add(RasterCompositeColumns);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(AdobeAnimateCrowdResourceSignature left, AdobeAnimateCrowdResourceSignature right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(AdobeAnimateCrowdResourceSignature left, AdobeAnimateCrowdResourceSignature right)
	{
		return !left.Equals(right);
	}

	private static Rid GetRid(TextureLayered texture)
	{
		if (!GodotObject.IsInstanceValid(texture))
		{
			return default;
		}
		return texture.GetRid();
	}

	private static Rid GetRid(ImageTexture texture)
	{
		if (!GodotObject.IsInstanceValid(texture))
		{
			return default;
		}
		return texture.GetRid();
	}

	private static Rid GetRid(Texture2D texture)
	{
		if (!GodotObject.IsInstanceValid(texture))
		{
			return default;
		}
		return texture.GetRid();
	}
}
