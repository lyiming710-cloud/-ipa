using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Award/Packet/TowerDefenseAwardPacket.cs")]
public class TowerDefenseAwardPacket : TowerDefenseAwardBase
{
	public new class MethodName : TowerDefenseAwardBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Pressed = "Pressed";
	}

	public new class PropertyName : TowerDefenseAwardBase.PropertyName
	{
		public static readonly StringName packet = "packet";

		public new static readonly StringName awardRay = "awardRay";

		public new static readonly StringName awardPickupGlow = "awardPickupGlow";

		public new static readonly StringName downArrow = "downArrow";
	}

	public new class SignalName : TowerDefenseAwardBase.SignalName
	{
	}

	private TowerDefenseInGamePacketShow packet;

	private AwardRay awardRay;

	private Sprite2D awardPickupGlow;

	private Sprite2D downArrow;

	public override void _Ready()
	{
		base._Ready();
		packet = GetNodeOrNull<TowerDefenseInGamePacketShow>("%Packet");
		awardRay = GetNodeOrNull<AwardRay>("%AwardRay");
		awardPickupGlow = GetNodeOrNull<Sprite2D>("%AwardPickupGlow");
		downArrow = GetNodeOrNull<Sprite2D>("%DownArrow");
		GetNode<Button>("%Button").Pressed += Pressed;
	}

	public override void Init(string packetName)
	{
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
		packet.Init(packetConfig, skipGlobalChangeCost: true);
		packet.Reset();
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
			await ToSignal(tween, Tween.SignalName.Finished);
			tween = CreateTween();
			tween.TweenProperty(packet, "modulate:a", 0.0, 2.0);
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
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (name == PropertyName.packet)
		{
			packet = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
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
		if (name == PropertyName.packet)
		{
			value = VariantUtils.CreateFrom(in packet);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.packet, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.awardRay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.awardPickupGlow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.downArrow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packet, Variant.From(in packet));
		info.AddProperty(PropertyName.awardRay, Variant.From(in awardRay));
		info.AddProperty(PropertyName.awardPickupGlow, Variant.From(in awardPickupGlow));
		info.AddProperty(PropertyName.downArrow, Variant.From(in downArrow));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packet, out var value))
		{
			packet = value.As<TowerDefenseInGamePacketShow>();
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
