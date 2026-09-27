using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter10/HypnoShroomLight/Scene/TowerDefensePlantHypnoShroomLight.cs")]
public class TowerDefensePlantHypnoShroomLight : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName AnnouncePlanted = "AnnouncePlanted";

		public static readonly StringName TryFireFlash = "TryFireFlash";

		public static readonly StringName CanFireFlash = "CanFireFlash";

		public static readonly StringName FireFlashBall = "FireFlashBall";

		public static readonly StringName HideNextCrystal = "HideNextCrystal";

		public static readonly StringName SetCrystalLayerVisible = "SetCrystalLayerVisible";

		public static readonly StringName RefreshCrystalLights = "RefreshCrystalLights";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName RestoreIdleAnime = "RestoreIdleAnime";

		public static readonly StringName FlushPendingFlash = "FlushPendingFlash";

		public new static readonly StringName AttackDeal = "AttackDeal";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName flashProjectileName = "flashProjectileName";

		public static readonly StringName flashSpeed = "flashSpeed";

		public static readonly StringName flashNum = "flashNum";

		public static readonly StringName _crystalOffNum = "_crystalOffNum";

		public static readonly StringName _shotPending = "_shotPending";

		public static readonly StringName _placedAnnounced = "_placedAnnounced";

		public static readonly StringName _eaten = "_eaten";

		public static readonly StringName _light = "_light";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string IdleClip = "Idle";

	private const string ShootingClip = "Shooting";

	private const string AttackEvent = "attack";

	private const string CrystalLayerPrefix = "light";

	public const int MaxFlashNum = 3;

	[Export(PropertyHint.None, "")]
	public string flashProjectileName = "MagicFlashBall";

	[Export(PropertyHint.None, "")]
	public double flashSpeed = 300.0;

	[Export(PropertyHint.None, "")]
	public int flashNum;

	private int _crystalOffNum;

	private bool _shotPending;

	private bool _placedAnnounced;

	private bool _eaten;

	private CharacterAabbAreaComponent _fogArea;

	private PointLight2D _light;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_fogArea = componentManager.GetRuntime<CharacterAabbAreaComponent>("character.aabb_area.fog");
			_light = GetNodeOrNull<PointLight2D>("%Light");
			RefreshCrystalLights();
			if (inGame)
			{
				Callable.From(AnnouncePlanted).CallDeferred();
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		_fogArea = null;
		_light = null;
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && inGame)
		{
			bool flag = !IsSleep() && !die && !isDestroy;
			CharacterAabbAreaComponent fogArea = _fogArea;
			if (fogArea != null && !fogArea.IsReleased)
			{
				_fogArea.SetEnabled(flag);
			}
			if (GodotObject.IsInstanceValid(_light))
			{
				_light.Visible = flag && TowerDefenseManager.GetMapIsNight() && GameSaveManager.Instance.GetConfigValue("MapEffect").AsBool();
			}
		}
	}

	private void AnnouncePlanted()
	{
		if (_placedAnnounced || !GodotObject.IsInstanceValid(this) || isDestroy || !IsInsideTree())
		{
			return;
		}
		_placedAnnounced = true;
		if (!GodotObject.IsInstanceValid(config))
		{
			return;
		}
		TowerDefenseEnum.CHARACTER_CAMP cHARACTER_CAMP = camp;
		Array<Node> nodesInGroup = GetTree().GetNodesInGroup(config.name);
		for (int i = 0; i < nodesInGroup.Count; i++)
		{
			if (nodesInGroup[i] is TowerDefensePlantHypnoShroomLight towerDefensePlantHypnoShroomLight && GodotObject.IsInstanceValid(towerDefensePlantHypnoShroomLight) && !towerDefensePlantHypnoShroomLight.isDestroy && !towerDefensePlantHypnoShroomLight.die && towerDefensePlantHypnoShroomLight.camp == cHARACTER_CAMP)
			{
				towerDefensePlantHypnoShroomLight.TryFireFlash();
			}
		}
	}

	public void TryFireFlash()
	{
		if (CanFireFlash())
		{
			flashNum++;
			if (!(_shotPending = GodotObject.IsInstanceValid(sprite) && sprite.HasClip("Shooting")))
			{
				FireFlashBall();
				HideNextCrystal();
			}
			else
			{
				sprite.SetAnimation("Shooting", loop: false);
			}
		}
	}

	private bool CanFireFlash()
	{
		if (!Engine.IsEditorHint() && inGame && !die && !isDestroy && !IsSleep())
		{
			return flashNum < 3;
		}
		return false;
	}

	private void FireFlashBall()
	{
		Vector2 vector = ((Scale.X < 0f) ? Vector2.Left : Vector2.Right);
		Vector2 pos = GetLogicalGlobalPosition() + new Vector2(vector.X * 20f, -30f);
		FireComponent.CreateProjectilePositionByName(this, null, 0.0, pos, vector * (float)flashSpeed, flashProjectileName, -1, camp);
	}

	private void HideNextCrystal()
	{
		if (_crystalOffNum < 3 && _crystalOffNum < flashNum)
		{
			_crystalOffNum++;
			SetCrystalLayerVisible(_crystalOffNum, visible: false);
		}
	}

	private void SetCrystalLayerVisible(int index, bool visible)
	{
		if (index < 1 || index > 3 || !GodotObject.IsInstanceValid(sprite))
		{
			return;
		}
		string text = "light" + index;
		sprite.Set("Animation/LayerVisible/" + text, visible);
		AdobeAnimateData flashAnimeData = sprite.flashAnimeData;
		if (flashAnimeData?.layerDictionary != null && flashAnimeData.layerDictionary.ContainsKey(text))
		{
			int num = (int)flashAnimeData.layerDictionary[text];
			Array<bool> layerVisibleForInternalRead = sprite.GetLayerVisibleForInternalRead();
			if (layerVisibleForInternalRead != null && num >= 0 && num < layerVisibleForInternalRead.Count && layerVisibleForInternalRead[num] != visible)
			{
				layerVisibleForInternalRead[num] = visible;
			}
		}
		sprite.QueueRedraw();
	}

	private void RefreshCrystalLights()
	{
		for (int i = 1; i <= 3; i++)
		{
			SetCrystalLayerVisible(i, i > _crystalOffNum);
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (!string.IsNullOrEmpty(command) && string.Equals(command, "attack", StringComparison.OrdinalIgnoreCase))
		{
			HideNextCrystal();
			FlushPendingFlash();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (string.IsNullOrEmpty(clip) || !(clip != "Shooting"))
		{
			HideNextCrystal();
			FlushPendingFlash();
			if (clip == "Shooting")
			{
				RestoreIdleAnime();
			}
		}
	}

	private void RestoreIdleAnime()
	{
		if (!Engine.IsEditorHint() && inGame && !die && !isDestroy && !IsSleep() && GodotObject.IsInstanceValid(sprite) && sprite.HasClip("Idle"))
		{
			sprite.timeScale = timeScale;
			sprite.SetAnimation("Idle", loop: true, 0.2);
		}
	}

	private void FlushPendingFlash()
	{
		if (_shotPending)
		{
			_shotPending = false;
			FireFlashBall();
		}
	}

	public override void AttackDeal(TowerDefenseCharacter character, string type, double num)
	{
		base.AttackDeal(character, type, num);
		if (!instance.sleep && !_eaten && GodotObject.IsInstanceValid(character))
		{
			if ((character.instance.unUseBuffFlags & 8) != 0)
			{
				SkipInvincibleHurt(num);
			}
			else if (!(type != "Eat"))
			{
				_eaten = true;
				character.Hypnoses();
				character.BuffAdd(new TowerDefenseCharacterBuffRadiance());
				Destroy();
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["flashNum"] = flashNum,
			["crystalOffNum"] = _crystalOffNum,
			["placedAnnounced"] = _placedAnnounced,
			["eaten"] = _eaten
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		flashNum = (data.ContainsKey("flashNum") ? data["flashNum"].AsInt32() : 0);
		_crystalOffNum = (data.ContainsKey("crystalOffNum") ? data["crystalOffNum"].AsInt32() : 0);
		_placedAnnounced = data.ContainsKey("placedAnnounced") && data["placedAnnounced"].AsBool();
		_eaten = data.ContainsKey("eaten") && data["eaten"].AsBool();
		RefreshCrystalLights();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnnouncePlanted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryFireFlash, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanFireFlash, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FireFlashBall, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideNextCrystal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCrystalLayerVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCrystalLights, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreIdleAnime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlushPendingFlash, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackDeal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnnouncePlanted && args.Count == 0)
		{
			AnnouncePlanted();
			ret = default;
			return true;
		}
		if (method == MethodName.TryFireFlash && args.Count == 0)
		{
			TryFireFlash();
			ret = default;
			return true;
		}
		if (method == MethodName.CanFireFlash && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanFireFlash());
			return true;
		}
		if (method == MethodName.FireFlashBall && args.Count == 0)
		{
			FireFlashBall();
			ret = default;
			return true;
		}
		if (method == MethodName.HideNextCrystal && args.Count == 0)
		{
			HideNextCrystal();
			ret = default;
			return true;
		}
		if (method == MethodName.SetCrystalLayerVisible && args.Count == 2)
		{
			SetCrystalLayerVisible(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCrystalLights && args.Count == 0)
		{
			RefreshCrystalLights();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreIdleAnime && args.Count == 0)
		{
			RestoreIdleAnime();
			ret = default;
			return true;
		}
		if (method == MethodName.FlushPendingFlash && args.Count == 0)
		{
			FlushPendingFlash();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackDeal && args.Count == 3)
		{
			AttackDeal(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.AnnouncePlanted)
		{
			return true;
		}
		if (method == MethodName.TryFireFlash)
		{
			return true;
		}
		if (method == MethodName.CanFireFlash)
		{
			return true;
		}
		if (method == MethodName.FireFlashBall)
		{
			return true;
		}
		if (method == MethodName.HideNextCrystal)
		{
			return true;
		}
		if (method == MethodName.SetCrystalLayerVisible)
		{
			return true;
		}
		if (method == MethodName.RefreshCrystalLights)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.RestoreIdleAnime)
		{
			return true;
		}
		if (method == MethodName.FlushPendingFlash)
		{
			return true;
		}
		if (method == MethodName.AttackDeal)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.flashProjectileName)
		{
			flashProjectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.flashSpeed)
		{
			flashSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.flashNum)
		{
			flashNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._crystalOffNum)
		{
			_crystalOffNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._shotPending)
		{
			_shotPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._placedAnnounced)
		{
			_placedAnnounced = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._eaten)
		{
			_eaten = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._light)
		{
			_light = VariantUtils.ConvertTo<PointLight2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.flashProjectileName)
		{
			value = VariantUtils.CreateFrom(in flashProjectileName);
			return true;
		}
		if (name == PropertyName.flashSpeed)
		{
			value = VariantUtils.CreateFrom(in flashSpeed);
			return true;
		}
		if (name == PropertyName.flashNum)
		{
			value = VariantUtils.CreateFrom(in flashNum);
			return true;
		}
		if (name == PropertyName._crystalOffNum)
		{
			value = VariantUtils.CreateFrom(in _crystalOffNum);
			return true;
		}
		if (name == PropertyName._shotPending)
		{
			value = VariantUtils.CreateFrom(in _shotPending);
			return true;
		}
		if (name == PropertyName._placedAnnounced)
		{
			value = VariantUtils.CreateFrom(in _placedAnnounced);
			return true;
		}
		if (name == PropertyName._eaten)
		{
			value = VariantUtils.CreateFrom(in _eaten);
			return true;
		}
		if (name == PropertyName._light)
		{
			value = VariantUtils.CreateFrom(in _light);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.flashProjectileName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.flashSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.flashNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._crystalOffNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._shotPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._placedAnnounced, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._eaten, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._light, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.flashProjectileName, Variant.From(in flashProjectileName));
		info.AddProperty(PropertyName.flashSpeed, Variant.From(in flashSpeed));
		info.AddProperty(PropertyName.flashNum, Variant.From(in flashNum));
		info.AddProperty(PropertyName._crystalOffNum, Variant.From(in _crystalOffNum));
		info.AddProperty(PropertyName._shotPending, Variant.From(in _shotPending));
		info.AddProperty(PropertyName._placedAnnounced, Variant.From(in _placedAnnounced));
		info.AddProperty(PropertyName._eaten, Variant.From(in _eaten));
		info.AddProperty(PropertyName._light, Variant.From(in _light));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.flashProjectileName, out var value))
		{
			flashProjectileName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.flashSpeed, out var value2))
		{
			flashSpeed = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.flashNum, out var value3))
		{
			flashNum = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._crystalOffNum, out var value4))
		{
			_crystalOffNum = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._shotPending, out var value5))
		{
			_shotPending = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._placedAnnounced, out var value6))
		{
			_placedAnnounced = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._eaten, out var value7))
		{
			_eaten = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._light, out var value8))
		{
			_light = value8.As<PointLight2D>();
		}
	}
}
