using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Map/Resource/TowerDefenseMapConfig.cs")]
public class TowerDefenseMapConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName HasSpecialRule = "HasSpecialRule";

		public static readonly StringName SpecialRulesPreventSleep = "SpecialRulesPreventSleep";

		public static readonly StringName ApplyPacketCooldownRules = "ApplyPacketCooldownRules";

		public static readonly StringName IgnoresDynamicPacketCostGrowth = "IgnoresDynamicPacketCostGrowth";

		public static readonly StringName GetAttackDpsLifestealRatio = "GetAttackDpsLifestealRatio";

		public static readonly StringName RefreshSpecialRuleRuntimeCache = "RefreshSpecialRuleRuntimeCache";

		public static readonly StringName InvalidateSpecialRuleRuntimeCache = "InvalidateSpecialRuleRuntimeCache";

		public static readonly StringName GetZombieColumnSpeedMultiplier = "GetZombieColumnSpeedMultiplier";

		public static readonly StringName ApplyCharacterRules = "ApplyCharacterRules";

		public static readonly StringName GetEffectiveCellConfig = "GetEffectiveCellConfig";

		public static readonly StringName AreGridTypesEquivalent = "AreGridTypesEquivalent";

		public static readonly StringName AreGroundCurvesEquivalent = "AreGroundCurvesEquivalent";

		public static readonly StringName IsFinite = "IsFinite";

		public static readonly StringName IsFinitePositive = "IsFinitePositive";

		public static readonly StringName GetMapTexture = "GetMapTexture";

		public static readonly StringName GetMapScene = "GetMapScene";

		public static readonly StringName GetMapThumbnail = "GetMapThumbnail";

		public static readonly StringName ClearLoadedMapResources = "ClearLoadedMapResources";

		public static readonly StringName ApplyMapPreviewTexture = "ApplyMapPreviewTexture";

		public static readonly StringName GetResourcePath = "GetResourcePath";

		public static readonly StringName CreateMapThumbnail = "CreateMapThumbnail";

		public static readonly StringName PrepareMapThumbnailSourceImage = "PrepareMapThumbnailSourceImage";

		public static readonly StringName LoadMapSourceImage = "LoadMapSourceImage";

		public static readonly StringName GetMapThumbnailCacheKey = "GetMapThumbnailCacheKey";

		public static readonly StringName GetSourceModifiedStamp = "GetSourceModifiedStamp";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName mapTexture = "mapTexture";

		public static readonly StringName mapScene = "mapScene";

		public static readonly StringName translate = "translate";

		public static readonly StringName dayNightSwitching = "dayNightSwitching";

		public static readonly StringName mapTexturePath = "mapTexturePath";

		public static readonly StringName mapScenePath = "mapScenePath";

		public static readonly StringName _mapTexture = "_mapTexture";

		public static readonly StringName _mapThumbnail = "_mapThumbnail";

		public static readonly StringName _mapThumbnailKey = "_mapThumbnailKey";

		public static readonly StringName _mapScene = "_mapScene";

		public static readonly StringName _specialRuleRuntimeCacheValid = "_specialRuleRuntimeCacheValid";

		public static readonly StringName _cachedAttackDpsLifestealRatio = "_cachedAttackDpsLifestealRatio";

		public static readonly StringName _cachedZombieColumnSpeedMultipliers = "_cachedZombieColumnSpeedMultipliers";

		public static readonly StringName mapSize = "mapSize";

		public static readonly StringName mapOffset = "mapOffset";

		public static readonly StringName plantOffset = "plantOffset";

		public static readonly StringName gridNum = "gridNum";

		public static readonly StringName gridBeginPos = "gridBeginPos";

		public static readonly StringName gridSize = "gridSize";

		public static readonly StringName edge = "edge";

		public static readonly StringName cellConfig = "cellConfig";

		public static readonly StringName lineUse = "lineUse";

		public static readonly StringName isNight = "isNight";

		public static readonly StringName useSunFall = "useSunFall";

		public static readonly StringName enableBattleZoom = "enableBattleZoom";

		public static readonly StringName maximumFps = "maximumFps";

		public static readonly StringName specialRules = "specialRules";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public const int MaxGridDimension = 50;

	[Export(PropertyHint.None, "")]
	public string translate = "";

	[Export(PropertyHint.None, "")]
	public string dayNightSwitching = "";

	[Export(PropertyHint.None, "")]
	public string mapTexturePath = "";

	[Export(PropertyHint.None, "")]
	public string mapScenePath = "";

	private Texture2D _mapTexture;

	private Texture2D _mapThumbnail;

	private string _mapThumbnailKey = "";

	private PackedScene _mapScene;

	private bool _specialRuleRuntimeCacheValid;

	private double _cachedAttackDpsLifestealRatio;

	private readonly double[] _cachedZombieColumnSpeedMultipliers = new double[51];

	[Export(PropertyHint.None, "")]
	public Vector2 mapSize = new Vector2(1400f, 600f);

	[Export(PropertyHint.None, "")]
	public Vector2 mapOffset = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public double plantOffset = 50.0;

	[Export(PropertyHint.None, "")]
	public Vector2I gridNum = new Vector2I(9, 5);

	[Export(PropertyHint.None, "")]
	public Vector2 gridBeginPos = new Vector2(256f, 45f);

	[Export(PropertyHint.None, "")]
	public Vector2 gridSize = new Vector2(80f, 98f);

	[Export(PropertyHint.None, "")]
	public Vector4 edge = new Vector4(200f, 0f, 1100f, 576f);

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCellConfig> cellConfig = new Array<TowerDefenseCellConfig>();

	[Export(PropertyHint.None, "")]
	public Array<int> lineUse = new Array<int>();

	[Export(PropertyHint.None, "")]
	public bool isNight;

	[Export(PropertyHint.None, "")]
	public bool useSunFall = true;

	[Export(PropertyHint.None, "")]
	public bool enableBattleZoom;

	[Export(PropertyHint.None, "")]
	public int maximumFps = -1;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseMapRuleConfig> specialRules = new Array<TowerDefenseMapRuleConfig>();

	public Texture2D mapTexture
	{
		get
		{
			return GetMapTexture();
		}
		set
		{
			_mapTexture = value;
			mapTexturePath = GetResourcePath(value);
		}
	}

	public PackedScene mapScene
	{
		get
		{
			return GetMapScene();
		}
		set
		{
			_mapScene = value;
			mapScenePath = GetResourcePath(value);
		}
	}

	public bool HasSpecialRule(StringName ruleId)
	{
		if (ruleId.IsEmpty || specialRules == null)
		{
			return false;
		}
		foreach (TowerDefenseMapRuleConfig specialRule in specialRules)
		{
			if (GodotObject.IsInstanceValid(specialRule) && specialRule.id == ruleId)
			{
				return true;
			}
		}
		return false;
	}

	public bool SpecialRulesPreventSleep()
	{
		if (specialRules == null)
		{
			return false;
		}
		foreach (TowerDefenseMapRuleConfig specialRule in specialRules)
		{
			if (GodotObject.IsInstanceValid(specialRule) && specialRule.preventsSleep)
			{
				return true;
			}
		}
		return false;
	}

	public double ApplyPacketCooldownRules(TowerDefenseEnum.PACKET_TYPE packetType, double multiplier)
	{
		if (!double.IsFinite(multiplier))
		{
			multiplier = 1.0;
		}
		if (specialRules == null)
		{
			return multiplier;
		}
		foreach (TowerDefenseMapRuleConfig specialRule in specialRules)
		{
			if (!GodotObject.IsInstanceValid(specialRule) || specialRule.packetRules == null)
			{
				continue;
			}
			foreach (TowerDefenseMapPacketRuleConfig packetRule in specialRule.packetRules)
			{
				if (GodotObject.IsInstanceValid(packetRule) && packetRule.Matches(packetType))
				{
					multiplier *= packetRule.cooldownMultiplier;
				}
			}
		}
		if (!double.IsFinite(multiplier))
		{
			return 0.0;
		}
		return Math.Max(0.0, multiplier);
	}

	public bool IgnoresDynamicPacketCostGrowth(TowerDefenseEnum.PACKET_TYPE packetType)
	{
		if (specialRules == null)
		{
			return false;
		}
		foreach (TowerDefenseMapRuleConfig specialRule in specialRules)
		{
			if (!GodotObject.IsInstanceValid(specialRule) || specialRule.packetRules == null)
			{
				continue;
			}
			foreach (TowerDefenseMapPacketRuleConfig packetRule in specialRule.packetRules)
			{
				if (GodotObject.IsInstanceValid(packetRule) && packetRule.Matches(packetType) && packetRule.ignoreDynamicCostGrowth)
				{
					return true;
				}
			}
		}
		return false;
	}

	public double GetAttackDpsLifestealRatio()
	{
		if (_specialRuleRuntimeCacheValid)
		{
			return _cachedAttackDpsLifestealRatio;
		}
		return RefreshSpecialRuleRuntimeCache();
	}

	public double RefreshSpecialRuleRuntimeCache()
	{
		double num = 0.0;
		System.Array.Fill(_cachedZombieColumnSpeedMultipliers, 1.0);
		int num2 = specialRules?.Count ?? 0;
		for (int i = 0; i < num2; i++)
		{
			TowerDefenseMapRuleConfig towerDefenseMapRuleConfig = specialRules[i];
			if (!GodotObject.IsInstanceValid(towerDefenseMapRuleConfig))
			{
				continue;
			}
			if (double.IsFinite(towerDefenseMapRuleConfig.attackDpsLifestealRatio))
			{
				num += Math.Max(0.0, towerDefenseMapRuleConfig.attackDpsLifestealRatio);
			}
			if (towerDefenseMapRuleConfig.zombieColumnSpeedMinCol > 0 && towerDefenseMapRuleConfig.zombieColumnSpeedMaxCol > 0 && towerDefenseMapRuleConfig.zombieColumnSpeedMinCol <= towerDefenseMapRuleConfig.zombieColumnSpeedMaxCol && double.IsFinite(towerDefenseMapRuleConfig.zombieColumnSpeedMultiplier) && !(towerDefenseMapRuleConfig.zombieColumnSpeedMultiplier < 0.0))
			{
				int num3 = Math.Max(1, towerDefenseMapRuleConfig.zombieColumnSpeedMinCol);
				int num4 = Math.Min(50, towerDefenseMapRuleConfig.zombieColumnSpeedMaxCol);
				for (int j = num3; j <= num4; j++)
				{
					_cachedZombieColumnSpeedMultipliers[j] *= towerDefenseMapRuleConfig.zombieColumnSpeedMultiplier;
				}
			}
		}
		for (int k = 1; k <= 50; k++)
		{
			if (!double.IsFinite(_cachedZombieColumnSpeedMultipliers[k]))
			{
				_cachedZombieColumnSpeedMultipliers[k] = 1.0;
			}
		}
		_cachedAttackDpsLifestealRatio = (double.IsFinite(num) ? num : 0.0);
		_specialRuleRuntimeCacheValid = true;
		return _cachedAttackDpsLifestealRatio;
	}

	public void InvalidateSpecialRuleRuntimeCache()
	{
		_specialRuleRuntimeCacheValid = false;
	}

	public double GetZombieColumnSpeedMultiplier(int gridX)
	{
		if (gridX <= 0 || gridX > 50)
		{
			return 1.0;
		}
		if (!_specialRuleRuntimeCacheValid)
		{
			RefreshSpecialRuleRuntimeCache();
		}
		return _cachedZombieColumnSpeedMultipliers[gridX];
	}

	public void ApplyCharacterRules(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character) || specialRules == null)
		{
			return;
		}
		foreach (TowerDefenseMapRuleConfig specialRule in specialRules)
		{
			if (!GodotObject.IsInstanceValid(specialRule) || specialRule.characterRules == null)
			{
				continue;
			}
			foreach (TowerDefenseMapCharacterRuleConfig characterRule in specialRule.characterRules)
			{
				if (GodotObject.IsInstanceValid(characterRule))
				{
					characterRule.Apply(character);
				}
			}
		}
	}

	public bool TryValidateRuntime(out string reason)
	{
		if (gridNum.X < 1 || gridNum.Y < 1)
		{
			reason = $"grid dimensions must be positive, got {gridNum.X}x{gridNum.Y}";
			return false;
		}
		if (gridNum.X > 50 || gridNum.Y > 50)
		{
			reason = $"grid dimensions exceed the supported {50}x{50} limit: {gridNum.X}x{gridNum.Y}";
			return false;
		}
		if (maximumFps == 0 || maximumFps < -1)
		{
			reason = $"maximum FPS must be -1 or positive, got {maximumFps}";
			return false;
		}
		if (!IsFinitePositive(gridSize))
		{
			reason = $"grid cell size must be finite and positive, got {gridSize}";
			return false;
		}
		if (!IsFinitePositive(mapSize))
		{
			reason = $"map size must be finite and positive, got {mapSize}";
			return false;
		}
		if (!IsFinite(gridBeginPos) || !IsFinite(mapOffset) || !double.IsFinite(plantOffset))
		{
			reason = "grid origin, map offset, and plant offset must be finite";
			return false;
		}
		if (!IsFinite(edge) || edge.Z <= edge.X || edge.W <= edge.Y)
		{
			reason = $"map edge must be finite with right > left and bottom > top, got {edge}";
			return false;
		}
		if (lineUse != null)
		{
			foreach (int item in lineUse)
			{
				if (item < 1 || item > gridNum.Y)
				{
					reason = $"default enabled line {item} is outside 1..{gridNum.Y}";
					return false;
				}
			}
		}
		if (specialRules != null)
		{
			HashSet<StringName> hashSet = new HashSet<StringName>();
			foreach (TowerDefenseMapRuleConfig specialRule in specialRules)
			{
				if (!GodotObject.IsInstanceValid(specialRule))
				{
					reason = "special rule list contains a null or freed rule";
					return false;
				}
				if (!specialRule.TryValidateRuntime(out reason))
				{
					return false;
				}
				if (!hashSet.Add(specialRule.id))
				{
					reason = $"special rule id '{specialRule.id}' is duplicated";
					return false;
				}
			}
		}
		if (cellConfig != null)
		{
			foreach (TowerDefenseCellConfig item2 in cellConfig)
			{
				if (!GodotObject.IsInstanceValid(item2))
				{
					reason = "cell topology contains a null or freed config";
					return false;
				}
				if (item2.pos.X < 1 || item2.pos.Y < 1 || item2.pos.Z < item2.pos.X || item2.pos.W < item2.pos.Y || item2.pos.Z > gridNum.X || item2.pos.W > gridNum.Y)
				{
					reason = $"cell range {item2.pos} is outside the {gridNum.X}x{gridNum.Y} grid";
					return false;
				}
				if (item2.gridType == null)
				{
					reason = $"cell range {item2.pos} has no grid-type array";
					return false;
				}
				if (GodotObject.IsInstanceValid(item2.groundHeightCurve) && !GodotObject.IsInstanceValid(item2.groundHeightCurve.Curve))
				{
					reason = $"cell range {item2.pos} has an invalid ground-height curve";
					return false;
				}
			}
		}
		reason = "";
		return true;
	}

	public bool IsRuntimeTopologyCompatibleWith(TowerDefenseMapConfig other, out string reason)
	{
		if (!GodotObject.IsInstanceValid(other))
		{
			reason = "target map config is null or freed";
			return false;
		}
		if (!TryValidateRuntime(out reason))
		{
			return false;
		}
		if (!other.TryValidateRuntime(out reason))
		{
			return false;
		}
		if (this == other)
		{
			reason = "";
			return true;
		}
		if (gridNum != other.gridNum)
		{
			reason = $"grid dimensions differ: {gridNum} versus {other.gridNum}";
			return false;
		}
		if (gridBeginPos != other.gridBeginPos || gridSize != other.gridSize || plantOffset != other.plantOffset || mapSize != other.mapSize || edge != other.edge)
		{
			reason = "grid origin, cell size, plant offset, map size, or edge differs";
			return false;
		}
		for (int i = 1; i <= gridNum.X; i++)
		{
			for (int j = 1; j <= gridNum.Y; j++)
			{
				TowerDefenseCellConfig effectiveCellConfig = GetEffectiveCellConfig(i, j);
				TowerDefenseCellConfig effectiveCellConfig2 = other.GetEffectiveCellConfig(i, j);
				if (!AreGridTypesEquivalent(effectiveCellConfig, effectiveCellConfig2) || !AreGroundCurvesEquivalent(effectiveCellConfig?.groundHeightCurve, effectiveCellConfig2?.groundHeightCurve))
				{
					reason = $"structural cell topology differs at ({i}, {j})";
					return false;
				}
			}
		}
		reason = "";
		return true;
	}

	internal TowerDefenseCellConfig GetEffectiveCellConfig(int x, int y)
	{
		TowerDefenseCellConfig result = null;
		if (cellConfig == null)
		{
			return result;
		}
		foreach (TowerDefenseCellConfig item in cellConfig)
		{
			if (GodotObject.IsInstanceValid(item) && x >= item.pos.X && x <= item.pos.Z && y >= item.pos.Y && y <= item.pos.W)
			{
				result = item;
			}
		}
		return result;
	}

	private static bool AreGridTypesEquivalent(TowerDefenseCellConfig left, TowerDefenseCellConfig right)
	{
		int num = (GodotObject.IsInstanceValid(left) ? left.gridType.Count : 2);
		int num2 = (GodotObject.IsInstanceValid(right) ? right.gridType.Count : 2);
		if (num != num2)
		{
			return false;
		}
		for (int i = 0; i < num; i++)
		{
			int num3;
			if (GodotObject.IsInstanceValid(left))
			{
				num3 = (int)left.gridType[i];
			}
			else
			{
				num3 = ((i == 0) ? 2 : 4);
			}
			TowerDefenseEnum.PLANTGRIDTYPE pLANTGRIDTYPE;
			if (GodotObject.IsInstanceValid(right))
			{
				pLANTGRIDTYPE = right.gridType[i];
			}
			else
			{
				pLANTGRIDTYPE = ((i == 0) ? TowerDefenseEnum.PLANTGRIDTYPE.GROUND : TowerDefenseEnum.PLANTGRIDTYPE.AIR);
			}
			if (num3 != (int)pLANTGRIDTYPE)
			{
				return false;
			}
		}
		return true;
	}

	private static bool AreGroundCurvesEquivalent(CurveTexture leftTexture, CurveTexture rightTexture)
	{
		Curve curve = (GodotObject.IsInstanceValid(leftTexture) ? leftTexture.Curve : null);
		Curve curve2 = (GodotObject.IsInstanceValid(rightTexture) ? rightTexture.Curve : null);
		if (!GodotObject.IsInstanceValid(curve) || !GodotObject.IsInstanceValid(curve2))
		{
			if (!GodotObject.IsInstanceValid(curve))
			{
				return !GodotObject.IsInstanceValid(curve2);
			}
			return false;
		}
		for (int i = 0; i <= 128; i++)
		{
			float offset = (float)i / 128f;
			if (!Mathf.IsEqualApprox(curve.Sample(offset), curve2.Sample(offset)))
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsFinite(Vector2 value)
	{
		if (float.IsFinite(value.X))
		{
			return float.IsFinite(value.Y);
		}
		return false;
	}

	private static bool IsFinitePositive(Vector2 value)
	{
		if (IsFinite(value) && value.X > 0f)
		{
			return value.Y > 0f;
		}
		return false;
	}

	private static bool IsFinite(Vector4 value)
	{
		if (float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z))
		{
			return float.IsFinite(value.W);
		}
		return false;
	}

	public Texture2D GetMapTexture(bool cache = true)
	{
		if (cache && GodotObject.IsInstanceValid(_mapTexture))
		{
			return _mapTexture;
		}
		if (string.IsNullOrWhiteSpace(mapTexturePath))
		{
			return null;
		}
		Texture2D result = ResourceLoader.Load<Texture2D>(mapTexturePath, "", (ResourceLoader.CacheMode)(cache ? 1 : 0));
		if (cache)
		{
			_mapTexture = result;
		}
		return result;
	}

	public PackedScene GetMapScene(bool cache = true)
	{
		if (cache && GodotObject.IsInstanceValid(_mapScene))
		{
			return _mapScene;
		}
		if (string.IsNullOrWhiteSpace(mapScenePath))
		{
			return null;
		}
		PackedScene result = ResourceLoader.Load<PackedScene>(mapScenePath, "", (ResourceLoader.CacheMode)(cache ? 1 : 0));
		if (cache)
		{
			_mapScene = result;
		}
		return result;
	}

	public Texture2D GetMapThumbnail(int maxDimension = 384)
	{
		maxDimension = Math.Max(16, maxDimension);
		string mapThumbnailCacheKey = GetMapThumbnailCacheKey(maxDimension);
		if (GodotObject.IsInstanceValid(_mapThumbnail) && _mapThumbnailKey == mapThumbnailCacheKey)
		{
			return _mapThumbnail;
		}
		Texture2D texture2D = ThumbnailBinaryResourceCache.Load("map-thumbnail", mapThumbnailCacheKey);
		if (GodotObject.IsInstanceValid(texture2D))
		{
			_mapThumbnail = texture2D;
			_mapThumbnailKey = mapThumbnailCacheKey;
			return _mapThumbnail;
		}
		Texture2D texture2D2 = CreateMapThumbnail(maxDimension);
		if (GodotObject.IsInstanceValid(texture2D2))
		{
			ThumbnailBinaryResourceCache.Save("map-thumbnail", mapThumbnailCacheKey, texture2D2);
			_mapThumbnail = texture2D2;
			_mapThumbnailKey = mapThumbnailCacheKey;
		}
		return _mapThumbnail;
	}

	public void ClearLoadedMapResources(bool clearThumbnail = false)
	{
		_mapTexture = null;
		_mapScene = null;
		if (clearThumbnail)
		{
			_mapThumbnail = null;
			_mapThumbnailKey = "";
		}
	}

	public static void ApplyMapPreviewTexture(TextureRect textureRect, Texture2D texture)
	{
		if (GodotObject.IsInstanceValid(textureRect))
		{
			textureRect.Texture = texture;
			textureRect.Scale = Vector2.One;
			textureRect.Rotation = 0f;
			textureRect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
			textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;
		}
	}

	private static string GetResourcePath(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "";
		}
		return resource.ResourcePath ?? "";
	}

	private Texture2D CreateMapThumbnail(int maxDimension)
	{
		Image image = LoadMapSourceImage();
		if (!GodotObject.IsInstanceValid(image) || image.IsEmpty())
		{
			return null;
		}
		if (!PrepareMapThumbnailSourceImage(image))
		{
			return null;
		}
		int width = image.GetWidth();
		int height = image.GetHeight();
		int num = Math.Max(width, height);
		if (num > maxDimension)
		{
			float num2 = (float)maxDimension / (float)num;
			int width2 = Math.Max(1, Mathf.RoundToInt((float)width * num2));
			int height2 = Math.Max(1, Mathf.RoundToInt((float)height * num2));
			image.Resize(width2, height2, Image.Interpolation.Bilinear);
		}
		return ImageTexture.CreateFromImage(image);
	}

	private static bool PrepareMapThumbnailSourceImage(Image image)
	{
		if (!GodotObject.IsInstanceValid(image) || image.IsEmpty())
		{
			return false;
		}
		if (!image.IsCompressed())
		{
			return true;
		}
		return image.Decompress() == Error.Ok;
	}

	private Image LoadMapSourceImage()
	{
		if (!string.IsNullOrWhiteSpace(mapTexturePath))
		{
			Texture2D texture2D = GetMapTexture(cache: false);
			Image image = (GodotObject.IsInstanceValid(texture2D) ? texture2D.GetImage() : null);
			if (GodotObject.IsInstanceValid(image) && !image.IsEmpty())
			{
				return image;
			}
			string text = ProjectSettings.GlobalizePath(mapTexturePath);
			if (!string.IsNullOrWhiteSpace(text))
			{
				Image image2 = Image.LoadFromFile(text);
				if (GodotObject.IsInstanceValid(image2) && !image2.IsEmpty())
				{
					return image2;
				}
			}
		}
		return null;
	}

	private string GetMapThumbnailCacheKey(int maxDimension)
	{
		string text = (string.IsNullOrWhiteSpace(mapTexturePath) ? ResourcePath : mapTexturePath);
		return $"{text}|{maxDimension}|{GetSourceModifiedStamp(text)}";
	}

	private static long GetSourceModifiedStamp(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return 0L;
		}
		try
		{
			string text = ProjectSettings.GlobalizePath(path);
			if (!string.IsNullOrWhiteSpace(text) && File.Exists(text))
			{
				return File.GetLastWriteTimeUtc(text).Ticks;
			}
		}
		catch
		{
		}
		try
		{
			if (Godot.FileAccess.FileExists(path))
			{
				return (long)Godot.FileAccess.GetModifiedTime(path);
			}
		}
		catch
		{
		}
		return 0L;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(25)
		{
			new MethodInfo(MethodName.HasSpecialRule, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "ruleId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpecialRulesPreventSleep, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyPacketCooldownRules, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "multiplier", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IgnoresDynamicPacketCostGrowth, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetAttackDpsLifestealRatio, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshSpecialRuleRuntimeCache, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateSpecialRuleRuntimeCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetZombieColumnSpeedMultiplier, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "gridX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCharacterRules, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetEffectiveCellConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "x", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "y", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AreGridTypesEquivalent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "left", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "right", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AreGroundCurvesEquivalent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "leftTexture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CurveTexture"), exported: false),
				new PropertyInfo(Variant.Type.Object, "rightTexture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CurveTexture"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsFinite, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsFinitePositive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "cache", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "cache", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapThumbnail, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maxDimension", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearLoadedMapResources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "clearThumbnail", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMapPreviewTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "textureRect", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureRect"), exported: false),
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateMapThumbnail, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maxDimension", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareMapThumbnailSourceImage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadMapSourceImage, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapThumbnailCacheKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maxDimension", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSourceModifiedStamp, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasSpecialRule && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSpecialRule(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.SpecialRulesPreventSleep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(SpecialRulesPreventSleep());
			return true;
		}
		if (method == MethodName.ApplyPacketCooldownRules && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyPacketCooldownRules(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.IgnoresDynamicPacketCostGrowth && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IgnoresDynamicPacketCostGrowth(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAttackDpsLifestealRatio && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetAttackDpsLifestealRatio());
			return true;
		}
		if (method == MethodName.RefreshSpecialRuleRuntimeCache && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(RefreshSpecialRuleRuntimeCache());
			return true;
		}
		if (method == MethodName.InvalidateSpecialRuleRuntimeCache && args.Count == 0)
		{
			InvalidateSpecialRuleRuntimeCache();
			ret = default;
			return true;
		}
		if (method == MethodName.GetZombieColumnSpeedMultiplier && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetZombieColumnSpeedMultiplier(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyCharacterRules && args.Count == 1)
		{
			ApplyCharacterRules(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetEffectiveCellConfig && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCellConfig>(GetEffectiveCellConfig(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.AreGridTypesEquivalent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AreGridTypesEquivalent(VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.AreGroundCurvesEquivalent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AreGroundCurvesEquivalent(VariantUtils.ConvertTo<CurveTexture>(in args[0]), VariantUtils.ConvertTo<CurveTexture>(in args[1])));
			return true;
		}
		if (method == MethodName.IsFinite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFinite(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.IsFinitePositive && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFinitePositive(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetMapTexture(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetMapScene(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapThumbnail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetMapThumbnail(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearLoadedMapResources && args.Count == 1)
		{
			ClearLoadedMapResources(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMapPreviewTexture && args.Count == 2)
		{
			ApplyMapPreviewTexture(VariantUtils.ConvertTo<TextureRect>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourcePath(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateMapThumbnail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(CreateMapThumbnail(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.PrepareMapThumbnailSourceImage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(PrepareMapThumbnailSourceImage(VariantUtils.ConvertTo<Image>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadMapSourceImage && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Image>(LoadMapSourceImage());
			return true;
		}
		if (method == MethodName.GetMapThumbnailCacheKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetMapThumbnailCacheKey(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSourceModifiedStamp && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<long>(GetSourceModifiedStamp(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AreGridTypesEquivalent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AreGridTypesEquivalent(VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.AreGroundCurvesEquivalent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AreGroundCurvesEquivalent(VariantUtils.ConvertTo<CurveTexture>(in args[0]), VariantUtils.ConvertTo<CurveTexture>(in args[1])));
			return true;
		}
		if (method == MethodName.IsFinite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFinite(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.IsFinitePositive && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFinitePositive(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyMapPreviewTexture && args.Count == 2)
		{
			ApplyMapPreviewTexture(VariantUtils.ConvertTo<TextureRect>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourcePath(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.PrepareMapThumbnailSourceImage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(PrepareMapThumbnailSourceImage(VariantUtils.ConvertTo<Image>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSourceModifiedStamp && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<long>(GetSourceModifiedStamp(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.HasSpecialRule)
		{
			return true;
		}
		if (method == MethodName.SpecialRulesPreventSleep)
		{
			return true;
		}
		if (method == MethodName.ApplyPacketCooldownRules)
		{
			return true;
		}
		if (method == MethodName.IgnoresDynamicPacketCostGrowth)
		{
			return true;
		}
		if (method == MethodName.GetAttackDpsLifestealRatio)
		{
			return true;
		}
		if (method == MethodName.RefreshSpecialRuleRuntimeCache)
		{
			return true;
		}
		if (method == MethodName.InvalidateSpecialRuleRuntimeCache)
		{
			return true;
		}
		if (method == MethodName.GetZombieColumnSpeedMultiplier)
		{
			return true;
		}
		if (method == MethodName.ApplyCharacterRules)
		{
			return true;
		}
		if (method == MethodName.GetEffectiveCellConfig)
		{
			return true;
		}
		if (method == MethodName.AreGridTypesEquivalent)
		{
			return true;
		}
		if (method == MethodName.AreGroundCurvesEquivalent)
		{
			return true;
		}
		if (method == MethodName.IsFinite)
		{
			return true;
		}
		if (method == MethodName.IsFinitePositive)
		{
			return true;
		}
		if (method == MethodName.GetMapTexture)
		{
			return true;
		}
		if (method == MethodName.GetMapScene)
		{
			return true;
		}
		if (method == MethodName.GetMapThumbnail)
		{
			return true;
		}
		if (method == MethodName.ClearLoadedMapResources)
		{
			return true;
		}
		if (method == MethodName.ApplyMapPreviewTexture)
		{
			return true;
		}
		if (method == MethodName.GetResourcePath)
		{
			return true;
		}
		if (method == MethodName.CreateMapThumbnail)
		{
			return true;
		}
		if (method == MethodName.PrepareMapThumbnailSourceImage)
		{
			return true;
		}
		if (method == MethodName.LoadMapSourceImage)
		{
			return true;
		}
		if (method == MethodName.GetMapThumbnailCacheKey)
		{
			return true;
		}
		if (method == MethodName.GetSourceModifiedStamp)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.mapTexture)
		{
			mapTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.mapScene)
		{
			mapScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.translate)
		{
			translate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.dayNightSwitching)
		{
			dayNightSwitching = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.mapTexturePath)
		{
			mapTexturePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.mapScenePath)
		{
			mapScenePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._mapTexture)
		{
			_mapTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._mapThumbnail)
		{
			_mapThumbnail = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._mapThumbnailKey)
		{
			_mapThumbnailKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._mapScene)
		{
			_mapScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName._specialRuleRuntimeCacheValid)
		{
			_specialRuleRuntimeCacheValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedAttackDpsLifestealRatio)
		{
			_cachedAttackDpsLifestealRatio = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.mapSize)
		{
			mapSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.mapOffset)
		{
			mapOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.plantOffset)
		{
			plantOffset = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.gridNum)
		{
			gridNum = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.gridBeginPos)
		{
			gridBeginPos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.gridSize)
		{
			gridSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.edge)
		{
			edge = VariantUtils.ConvertTo<Vector4>(in value);
			return true;
		}
		if (name == PropertyName.cellConfig)
		{
			cellConfig = VariantUtils.ConvertToArray<TowerDefenseCellConfig>(in value);
			return true;
		}
		if (name == PropertyName.lineUse)
		{
			lineUse = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.isNight)
		{
			isNight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useSunFall)
		{
			useSunFall = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.enableBattleZoom)
		{
			enableBattleZoom = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.maximumFps)
		{
			maximumFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.specialRules)
		{
			specialRules = VariantUtils.ConvertToArray<TowerDefenseMapRuleConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.mapTexture)
		{
			value = VariantUtils.CreateFrom<Texture2D>(mapTexture);
			return true;
		}
		if (name == PropertyName.mapScene)
		{
			value = VariantUtils.CreateFrom<PackedScene>(mapScene);
			return true;
		}
		if (name == PropertyName.translate)
		{
			value = VariantUtils.CreateFrom(in translate);
			return true;
		}
		if (name == PropertyName.dayNightSwitching)
		{
			value = VariantUtils.CreateFrom(in dayNightSwitching);
			return true;
		}
		if (name == PropertyName.mapTexturePath)
		{
			value = VariantUtils.CreateFrom(in mapTexturePath);
			return true;
		}
		if (name == PropertyName.mapScenePath)
		{
			value = VariantUtils.CreateFrom(in mapScenePath);
			return true;
		}
		if (name == PropertyName._mapTexture)
		{
			value = VariantUtils.CreateFrom(in _mapTexture);
			return true;
		}
		if (name == PropertyName._mapThumbnail)
		{
			value = VariantUtils.CreateFrom(in _mapThumbnail);
			return true;
		}
		if (name == PropertyName._mapThumbnailKey)
		{
			value = VariantUtils.CreateFrom(in _mapThumbnailKey);
			return true;
		}
		if (name == PropertyName._mapScene)
		{
			value = VariantUtils.CreateFrom(in _mapScene);
			return true;
		}
		if (name == PropertyName._specialRuleRuntimeCacheValid)
		{
			value = VariantUtils.CreateFrom(in _specialRuleRuntimeCacheValid);
			return true;
		}
		if (name == PropertyName._cachedAttackDpsLifestealRatio)
		{
			value = VariantUtils.CreateFrom(in _cachedAttackDpsLifestealRatio);
			return true;
		}
		if (name == PropertyName._cachedZombieColumnSpeedMultipliers)
		{
			value = VariantUtils.CreateFrom(in _cachedZombieColumnSpeedMultipliers);
			return true;
		}
		if (name == PropertyName.mapSize)
		{
			value = VariantUtils.CreateFrom(in mapSize);
			return true;
		}
		if (name == PropertyName.mapOffset)
		{
			value = VariantUtils.CreateFrom(in mapOffset);
			return true;
		}
		if (name == PropertyName.plantOffset)
		{
			value = VariantUtils.CreateFrom(in plantOffset);
			return true;
		}
		if (name == PropertyName.gridNum)
		{
			value = VariantUtils.CreateFrom(in gridNum);
			return true;
		}
		if (name == PropertyName.gridBeginPos)
		{
			value = VariantUtils.CreateFrom(in gridBeginPos);
			return true;
		}
		if (name == PropertyName.gridSize)
		{
			value = VariantUtils.CreateFrom(in gridSize);
			return true;
		}
		if (name == PropertyName.edge)
		{
			value = VariantUtils.CreateFrom(in edge);
			return true;
		}
		if (name == PropertyName.cellConfig)
		{
			value = VariantUtils.CreateFromArray(cellConfig);
			return true;
		}
		if (name == PropertyName.lineUse)
		{
			value = VariantUtils.CreateFromArray(lineUse);
			return true;
		}
		if (name == PropertyName.isNight)
		{
			value = VariantUtils.CreateFrom(in isNight);
			return true;
		}
		if (name == PropertyName.useSunFall)
		{
			value = VariantUtils.CreateFrom(in useSunFall);
			return true;
		}
		if (name == PropertyName.enableBattleZoom)
		{
			value = VariantUtils.CreateFrom(in enableBattleZoom);
			return true;
		}
		if (name == PropertyName.maximumFps)
		{
			value = VariantUtils.CreateFrom(in maximumFps);
			return true;
		}
		if (name == PropertyName.specialRules)
		{
			value = VariantUtils.CreateFromArray(specialRules);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.translate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.dayNightSwitching, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.mapTexturePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.mapScenePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapThumbnail, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._mapThumbnailKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._specialRuleRuntimeCacheValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cachedAttackDpsLifestealRatio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedFloat64Array, PropertyName._cachedZombieColumnSpeedMultipliers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.mapSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.mapOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.plantOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.gridBeginPos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.gridSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector4, PropertyName.edge, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.cellConfig, PropertyHint.TypeString, "24/17:TowerDefenseCellConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.lineUse, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isNight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useSunFall, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enableBattleZoom, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maximumFps, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.specialRules, PropertyHint.TypeString, "24/17:TowerDefenseMapRuleConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mapTexture, Variant.From<Texture2D>(mapTexture));
		info.AddProperty(PropertyName.mapScene, Variant.From<PackedScene>(mapScene));
		info.AddProperty(PropertyName.translate, Variant.From(in translate));
		info.AddProperty(PropertyName.dayNightSwitching, Variant.From(in dayNightSwitching));
		info.AddProperty(PropertyName.mapTexturePath, Variant.From(in mapTexturePath));
		info.AddProperty(PropertyName.mapScenePath, Variant.From(in mapScenePath));
		info.AddProperty(PropertyName._mapTexture, Variant.From(in _mapTexture));
		info.AddProperty(PropertyName._mapThumbnail, Variant.From(in _mapThumbnail));
		info.AddProperty(PropertyName._mapThumbnailKey, Variant.From(in _mapThumbnailKey));
		info.AddProperty(PropertyName._mapScene, Variant.From(in _mapScene));
		info.AddProperty(PropertyName._specialRuleRuntimeCacheValid, Variant.From(in _specialRuleRuntimeCacheValid));
		info.AddProperty(PropertyName._cachedAttackDpsLifestealRatio, Variant.From(in _cachedAttackDpsLifestealRatio));
		info.AddProperty(PropertyName.mapSize, Variant.From(in mapSize));
		info.AddProperty(PropertyName.mapOffset, Variant.From(in mapOffset));
		info.AddProperty(PropertyName.plantOffset, Variant.From(in plantOffset));
		info.AddProperty(PropertyName.gridNum, Variant.From(in gridNum));
		info.AddProperty(PropertyName.gridBeginPos, Variant.From(in gridBeginPos));
		info.AddProperty(PropertyName.gridSize, Variant.From(in gridSize));
		info.AddProperty(PropertyName.edge, Variant.From(in edge));
		info.AddProperty(PropertyName.cellConfig, Variant.CreateFrom(cellConfig));
		info.AddProperty(PropertyName.lineUse, Variant.CreateFrom(lineUse));
		info.AddProperty(PropertyName.isNight, Variant.From(in isNight));
		info.AddProperty(PropertyName.useSunFall, Variant.From(in useSunFall));
		info.AddProperty(PropertyName.enableBattleZoom, Variant.From(in enableBattleZoom));
		info.AddProperty(PropertyName.maximumFps, Variant.From(in maximumFps));
		info.AddProperty(PropertyName.specialRules, Variant.CreateFrom(specialRules));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mapTexture, out var value))
		{
			mapTexture = value.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.mapScene, out var value2))
		{
			mapScene = value2.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.translate, out var value3))
		{
			translate = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.dayNightSwitching, out var value4))
		{
			dayNightSwitching = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.mapTexturePath, out var value5))
		{
			mapTexturePath = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.mapScenePath, out var value6))
		{
			mapScenePath = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName._mapTexture, out var value7))
		{
			_mapTexture = value7.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._mapThumbnail, out var value8))
		{
			_mapThumbnail = value8.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._mapThumbnailKey, out var value9))
		{
			_mapThumbnailKey = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName._mapScene, out var value10))
		{
			_mapScene = value10.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName._specialRuleRuntimeCacheValid, out var value11))
		{
			_specialRuleRuntimeCacheValid = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedAttackDpsLifestealRatio, out var value12))
		{
			_cachedAttackDpsLifestealRatio = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.mapSize, out var value13))
		{
			mapSize = value13.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.mapOffset, out var value14))
		{
			mapOffset = value14.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.plantOffset, out var value15))
		{
			plantOffset = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName.gridNum, out var value16))
		{
			gridNum = value16.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.gridBeginPos, out var value17))
		{
			gridBeginPos = value17.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.gridSize, out var value18))
		{
			gridSize = value18.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.edge, out var value19))
		{
			edge = value19.As<Vector4>();
		}
		if (info.TryGetProperty(PropertyName.cellConfig, out var value20))
		{
			cellConfig = value20.AsGodotArray<TowerDefenseCellConfig>();
		}
		if (info.TryGetProperty(PropertyName.lineUse, out var value21))
		{
			lineUse = value21.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.isNight, out var value22))
		{
			isNight = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useSunFall, out var value23))
		{
			useSunFall = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.enableBattleZoom, out var value24))
		{
			enableBattleZoom = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.maximumFps, out var value25))
		{
			maximumFps = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName.specialRules, out var value26))
		{
			specialRules = value26.AsGodotArray<TowerDefenseMapRuleConfig>();
		}
	}
}
