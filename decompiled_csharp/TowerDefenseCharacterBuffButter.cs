using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Buff/TowerDefenseCharacterBuffButter.cs")]
public class TowerDefenseCharacterBuffButter : TowerDefenseCharacterBuffConfig
{
	public new class MethodName : TowerDefenseCharacterBuffConfig.MethodName
	{
		public new static readonly StringName _Init = "_Init";

		public static readonly StringName IsFreezeOwner = "IsFreezeOwner";

		public new static readonly StringName Enter = "Enter";

		public new static readonly StringName EnterReadOnlyClient = "EnterReadOnlyClient";

		public static readonly StringName ShowButterVisual = "ShowButterVisual";

		public new static readonly StringName Step = "Step";

		public new static readonly StringName StepReadOnlyClient = "StepReadOnlyClient";

		public new static readonly StringName Exit = "Exit";

		public new static readonly StringName ExitReadOnlyClient = "ExitReadOnlyClient";

		public static readonly StringName HideButterVisual = "HideButterVisual";

		public new static readonly StringName Refresh = "Refresh";
	}

	public new class PropertyName : TowerDefenseCharacterBuffConfig.PropertyName
	{
		public static readonly StringName freezeOrder = "freezeOrder";

		public static readonly StringName time = "time";

		public static readonly StringName currentTime = "currentTime";

		public static readonly StringName splatTexture = "splatTexture";

		public static readonly StringName butterSprite = "butterSprite";
	}

	public new class SignalName : TowerDefenseCharacterBuffConfig.SignalName
	{
	}

	private static Texture2D _BUTTER_SPLAT;

	internal static int NextFreezeOrder;

	internal int freezeOrder;

	[Export(PropertyHint.None, "")]
	public double time = 8.0;

	[Export(PropertyHint.None, "")]
	public double currentTime;

	[Export(PropertyHint.None, "")]
	public Texture2D splatTexture;

	public Sprite2D butterSprite;

	private AdobeAnimateExternalVisualHandle _butterVisualHandle = AdobeAnimateExternalVisualHandle.Invalid;

	private static Texture2D BUTTER_SPLAT => _BUTTER_SPLAT ?? (_BUTTER_SPLAT = GD.Load<Texture2D>("uid://cf2k7klghh2nl"));

	public override void _Init()
	{
		key = "Butter";
	}

	private bool IsFreezeOwner()
	{
		if (character?.buff == null)
		{
			return true;
		}
		if (character.buff.BuffGet("ButterGene") is TowerDefenseCharacterBuffButterGene towerDefenseCharacterBuffButterGene && towerDefenseCharacterBuffButterGene.freezeOrder > freezeOrder)
		{
			return false;
		}
		return true;
	}

	public override void Enter()
	{
		if (character.instance.maskFlags == 0 || (character.instance.maskFlags & 2) != 0)
		{
			character.buff.DeleteBuff("Butter");
			return;
		}
		if ((character.instance.unUseBuffFlags & 4) != 0)
		{
			character.buff.DeleteBuff("Butter");
			return;
		}
		freezeOrder = ++NextFreezeOrder;
		ShowButterVisual();
	}

	public override void EnterReadOnlyClient()
	{
		freezeOrder = ++NextFreezeOrder;
		ShowButterVisual();
	}

	private void ShowButterVisual()
	{
		if (GodotObject.IsInstanceValid(butterSprite))
		{
			return;
		}
		butterSprite = new Sprite2D();
		butterSprite.Texture = ((splatTexture != null) ? splatTexture : BUTTER_SPLAT);
		butterSprite.Rotation = -0.4f;
		butterSprite.Scale = new Vector2(character.Scale.X * character.sprite.Scale.X, butterSprite.Scale.Y);
		character.spriteGroup.AddChild(butterSprite, forceReadableName: false, Node.InternalMode.Disabled);
		if (GodotObject.IsInstanceValid(character.headSlot))
		{
			character.headSlot.Update();
			butterSprite.Position = character.GetLogicalGlobalTransform(character.spriteGroup).AffineInverse() * (character.GetLogicalGlobalPosition(character.headSlot) + new Vector2(-5f, -10f));
		}
		else
		{
			if (character is TowerDefenseZombie towerDefenseZombie)
			{
				butterSprite.Position = character.GetLogicalGlobalTransform(character.spriteGroup).AffineInverse() * (character.GetLogicalGlobalPosition(towerDefenseZombie.sprite) + new Vector2(-25f, -30f));
			}
			if (character is TowerDefensePlant towerDefensePlant)
			{
				butterSprite.Position = character.GetLogicalGlobalTransform(character.spriteGroup).AffineInverse() * (character.GetLogicalGlobalPosition(towerDefensePlant.sprite) + new Vector2(25f, -25f));
				butterSprite.Scale = new Vector2(butterSprite.Scale.X * -1f, butterSprite.Scale.Y);
			}
		}
		_butterVisualHandle = character.RegisterCharacterExternalVisual(butterSprite, GodotObject.IsInstanceValid(character.headSlot) ? new AdobeAnimateExternalVisualDescriptor(AdobeAnimateExternalVisualAttachmentMode.Slot, AdobeAnimateExternalVisualDrawBand.InFrontOfAnimation, character.headSlot, 1) : new AdobeAnimateExternalVisualDescriptor(AdobeAnimateExternalVisualAttachmentMode.Root, AdobeAnimateExternalVisualDrawBand.InFrontOfAnimation));
	}

	public override bool Step(double delta)
	{
		currentTime += delta;
		if (character.nearDie || character.die || currentTime >= time)
		{
			return true;
		}
		if (IsFreezeOwner())
		{
			character.timeScale *= 0.0;
		}
		return false;
	}

	public override void StepReadOnlyClient(double delta)
	{
		Step(delta);
	}

	public override void Exit()
	{
		HideButterVisual();
	}

	public override void ExitReadOnlyClient()
	{
		HideButterVisual();
	}

	private void HideButterVisual()
	{
		if (GodotObject.IsInstanceValid(butterSprite))
		{
			character.UnregisterCharacterExternalVisual(_butterVisualHandle);
			_butterVisualHandle = AdobeAnimateExternalVisualHandle.Invalid;
			butterSprite.QueueFree();
			butterSprite = null;
		}
	}

	public override void Refresh(TowerDefenseCharacterBuffConfig config)
	{
		time = Mathf.Max(time, ((TowerDefenseCharacterBuffButter)config).time);
		currentTime = 0.0;
		TowerDefenseCharacterBuffButter towerDefenseCharacterBuffButter = (TowerDefenseCharacterBuffButter)config;
		if (towerDefenseCharacterBuffButter.splatTexture != null)
		{
			splatTexture = towerDefenseCharacterBuffButter.splatTexture;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsFreezeOwner, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Enter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnterReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowButterVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Step, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StepReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Exit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExitReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideButterVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Init && args.Count == 0)
		{
			_Init();
			ret = default;
			return true;
		}
		if (method == MethodName.IsFreezeOwner && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFreezeOwner());
			return true;
		}
		if (method == MethodName.Enter && args.Count == 0)
		{
			Enter();
			ret = default;
			return true;
		}
		if (method == MethodName.EnterReadOnlyClient && args.Count == 0)
		{
			EnterReadOnlyClient();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowButterVisual && args.Count == 0)
		{
			ShowButterVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.Step && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Step(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.StepReadOnlyClient && args.Count == 1)
		{
			StepReadOnlyClient(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Exit && args.Count == 0)
		{
			Exit();
			ret = default;
			return true;
		}
		if (method == MethodName.ExitReadOnlyClient && args.Count == 0)
		{
			ExitReadOnlyClient();
			ret = default;
			return true;
		}
		if (method == MethodName.HideButterVisual && args.Count == 0)
		{
			HideButterVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 1)
		{
			Refresh(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Init)
		{
			return true;
		}
		if (method == MethodName.IsFreezeOwner)
		{
			return true;
		}
		if (method == MethodName.Enter)
		{
			return true;
		}
		if (method == MethodName.EnterReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.ShowButterVisual)
		{
			return true;
		}
		if (method == MethodName.Step)
		{
			return true;
		}
		if (method == MethodName.StepReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.Exit)
		{
			return true;
		}
		if (method == MethodName.ExitReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.HideButterVisual)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.freezeOrder)
		{
			freezeOrder = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.time)
		{
			time = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			currentTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.splatTexture)
		{
			splatTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.butterSprite)
		{
			butterSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.freezeOrder)
		{
			value = VariantUtils.CreateFrom(in freezeOrder);
			return true;
		}
		if (name == PropertyName.time)
		{
			value = VariantUtils.CreateFrom(in time);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			value = VariantUtils.CreateFrom(in currentTime);
			return true;
		}
		if (name == PropertyName.splatTexture)
		{
			value = VariantUtils.CreateFrom(in splatTexture);
			return true;
		}
		if (name == PropertyName.butterSprite)
		{
			value = VariantUtils.CreateFrom(in butterSprite);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.freezeOrder, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.splatTexture, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.butterSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.freezeOrder, Variant.From(in freezeOrder));
		info.AddProperty(PropertyName.time, Variant.From(in time));
		info.AddProperty(PropertyName.currentTime, Variant.From(in currentTime));
		info.AddProperty(PropertyName.splatTexture, Variant.From(in splatTexture));
		info.AddProperty(PropertyName.butterSprite, Variant.From(in butterSprite));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.freezeOrder, out var value))
		{
			freezeOrder = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.time, out var value2))
		{
			time = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.currentTime, out var value3))
		{
			currentTime = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.splatTexture, out var value4))
		{
			splatTexture = value4.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.butterSprite, out var value5))
		{
			butterSprite = value5.As<Sprite2D>();
		}
	}
}
