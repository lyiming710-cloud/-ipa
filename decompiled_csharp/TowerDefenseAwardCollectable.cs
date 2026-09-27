using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Award/Collectable/TowerDefenseAwardCollectable.cs")]
public class TowerDefenseAwardCollectable : TowerDefenseAwardBase
{
	public new class MethodName : TowerDefenseAwardBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Pressed = "Pressed";
	}

	public new class PropertyName : TowerDefenseAwardBase.PropertyName
	{
		public static readonly StringName sprite = "sprite";

		public new static readonly StringName awardRay = "awardRay";

		public new static readonly StringName awardPickupGlow = "awardPickupGlow";

		public new static readonly StringName downArrow = "downArrow";
	}

	public new class SignalName : TowerDefenseAwardBase.SignalName
	{
	}

	private Sprite2D sprite;

	private AwardRay awardRay;

	private Sprite2D awardPickupGlow;

	private Sprite2D downArrow;

	public override void _Ready()
	{
		base._Ready();
		sprite = GetNodeOrNull<Sprite2D>("%Sprite");
		awardRay = GetNodeOrNull<AwardRay>("%AwardRay");
		awardPickupGlow = GetNodeOrNull<Sprite2D>("%AwardPickupGlow");
		downArrow = GetNodeOrNull<Sprite2D>("%DownArrow");
		GetNode<Button>("%Button").Pressed += Pressed;
	}

	public override void Init(string collectableName)
	{
		CollectableConfig collectable = TowerDefenseManager.GetCollectable(collectableName);
		if (collectable.config is ShovelConfig)
		{
			ShovelConfig shovelConfig = (ShovelConfig)collectable.config;
			sprite.Texture = shovelConfig.texture;
			sprite.Scale = Vector2.One * 80f / sprite.Texture.GetWidth();
			if (GameSaveManager.Instance.GetKeyValue("CurrentShovel").AsString() == "ShovelDefault")
			{
				GameSaveManager.Instance.SetKeyValue("CurrentShovel", collectableName);
			}
		}
		if (collectable.config is AwardSettlementConfig)
		{
			AwardSettlementConfig awardSettlementConfig = (AwardSettlementConfig)collectable.config;
			sprite.Texture = awardSettlementConfig.texture;
		}
	}

	public override async void Pressed()
	{
		if (!press)
		{
			press = true;
			AudioManager.Instance.AudioStopAll();
			AudioManager.Instance.AudioPlay("Win", AudioManagerEnum.TYPE.MUSIC);
			AudioManager.Instance.AudioPlay("AwardLightFill", AudioManagerEnum.TYPE.MUSIC);
			awardPickupGlow.Visible = false;
			downArrow.Visible = false;
			awardRay.Emit();
			Vector2 screenCenterPosition = GetViewport().GetCamera2D().GetScreenCenterPosition();
			Tween tween = CreateTween();
			tween.SetParallel();
			tween.SetEase(Tween.EaseType.Out);
			tween.SetTrans(Tween.TransitionType.Quart);
			tween.TweenProperty(this, "global_position", screenCenterPosition, 5.0);
			tween.TweenProperty(this, "scale", Vector2.One * 2f, 7.0);
			tween.SetParallel(parallel: false);
			tween.TweenProperty(sprite, "modulate:a", 0.0, 2.0);
			await ToSignal(tween, Tween.SignalName.Finished);
			if (Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost)
			{
				MultiPlayerManager.Instance.SendClientReady();
				SceneManager.Instance.ChangeScene("MainMenu");
			}
			else
			{
				SceneManager.Instance.ChangeScene("AwardSettlement");
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "collectableName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Pressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Pressed && args.Count == 0)
		{
			Pressed();
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Pressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.awardRay)
		{
			awardRay = VariantUtils.ConvertTo<AwardRay>(in value);
			return true;
		}
		if (name == PropertyName.awardPickupGlow)
		{
			awardPickupGlow = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.downArrow)
		{
			downArrow = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom(in sprite);
			return true;
		}
		if (name == PropertyName.awardRay)
		{
			value = VariantUtils.CreateFrom(in awardRay);
			return true;
		}
		if (name == PropertyName.awardPickupGlow)
		{
			value = VariantUtils.CreateFrom(in awardPickupGlow);
			return true;
		}
		if (name == PropertyName.downArrow)
		{
			value = VariantUtils.CreateFrom(in downArrow);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.awardRay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.awardPickupGlow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.downArrow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.sprite, Variant.From(in sprite));
		info.AddProperty(PropertyName.awardRay, Variant.From(in awardRay));
		info.AddProperty(PropertyName.awardPickupGlow, Variant.From(in awardPickupGlow));
		info.AddProperty(PropertyName.downArrow, Variant.From(in downArrow));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.sprite, out var value))
		{
			sprite = value.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.awardRay, out var value2))
		{
			awardRay = value2.As<AwardRay>();
		}
		if (info.TryGetProperty(PropertyName.awardPickupGlow, out var value3))
		{
			awardPickupGlow = value3.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.downArrow, out var value4))
		{
			downArrow = value4.As<Sprite2D>();
		}
	}
}
