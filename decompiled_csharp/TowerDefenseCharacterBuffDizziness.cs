using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Buff/TowerDefenseCharacterBuffDizziness.cs")]
public class TowerDefenseCharacterBuffDizziness : TowerDefenseCharacterBuffConfig
{
	public new class MethodName : TowerDefenseCharacterBuffConfig.MethodName
	{
		public new static readonly StringName _Init = "_Init";

		public new static readonly StringName Enter = "Enter";

		public new static readonly StringName EnterReadOnlyClient = "EnterReadOnlyClient";

		public static readonly StringName ShowDizzinessVisual = "ShowDizzinessVisual";

		public new static readonly StringName Step = "Step";

		public new static readonly StringName StepReadOnlyClient = "StepReadOnlyClient";

		public new static readonly StringName Exit = "Exit";

		public new static readonly StringName ExitReadOnlyClient = "ExitReadOnlyClient";

		public static readonly StringName HideDizzinessVisual = "HideDizzinessVisual";

		public new static readonly StringName Refresh = "Refresh";

		public static readonly StringName IsMagic = "IsMagic";
	}

	public new class PropertyName : TowerDefenseCharacterBuffConfig.PropertyName
	{
		public static readonly StringName time = "time";

		public static readonly StringName magicImmune = "magicImmune";

		public static readonly StringName currentTime = "currentTime";

		public static readonly StringName dizzinessSprite = "dizzinessSprite";
	}

	public new class SignalName : TowerDefenseCharacterBuffConfig.SignalName
	{
	}

	private static PackedScene _STAR;

	[Export(PropertyHint.None, "")]
	public double time = 3.0;

	[Export(PropertyHint.None, "")]
	public bool magicImmune;

	[Export(PropertyHint.None, "")]
	public double currentTime;

	public AdobeAnimateSprite dizzinessSprite;

	private static PackedScene STAR => _STAR ?? (_STAR = GD.Load<PackedScene>("uid://pvjsf1qinxav"));

	public override void _Init()
	{
		key = "Dizziness";
	}

	public override void Enter()
	{
		if (magicImmune && IsMagic())
		{
			character.buff.DeleteBuff("Dizziness");
		}
		else if (character.instance.maskFlags == 0 || (character.instance.maskFlags & 2) != 0)
		{
			character.buff.DeleteBuff("Dizziness");
		}
		else if ((character.instance.unUseBuffFlags & 0x20) != 0 || character.buff.BuffHas("RuneFogDizzyImmune"))
		{
			character.buff.DeleteBuff("Dizziness");
		}
		else
		{
			ShowDizzinessVisual();
		}
	}

	public override void EnterReadOnlyClient()
	{
		ShowDizzinessVisual();
	}

	private void ShowDizzinessVisual()
	{
		if (GodotObject.IsInstanceValid(character.headSlot) && dizzinessSprite == null)
		{
			character.headSlot.Update();
			dizzinessSprite = STAR.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			Transform2D logicalGlobalTransform = character.GetLogicalGlobalTransform(character.spriteGroup);
			logicalGlobalTransform.Origin = character.GetLogicalGlobalPosition(character.headSlot) + new Vector2(-5f, -10f);
			character.AttachAnimatedStatusVisual(dizzinessSprite, character.headSlot, logicalGlobalTransform);
		}
		character.sprite.QueueRedraw();
	}

	public override bool Step(double delta)
	{
		currentTime += delta;
		if (character.nearDie || character.die || currentTime >= time || (magicImmune && IsMagic()))
		{
			return true;
		}
		character.timeScale *= 0.0;
		return false;
	}

	public override void StepReadOnlyClient(double delta)
	{
		Step(delta);
	}

	public override void Exit()
	{
		HideDizzinessVisual();
	}

	public override void ExitReadOnlyClient()
	{
		HideDizzinessVisual();
	}

	private void HideDizzinessVisual()
	{
		if (GodotObject.IsInstanceValid(dizzinessSprite))
		{
			character.DetachAnimatedStatusVisual(dizzinessSprite);
			dizzinessSprite.QueueFree();
		}
		dizzinessSprite = null;
		character.buff?.ResumeAnimationIfNoPlaybackBlockingHardControl();
	}

	public override void Refresh(TowerDefenseCharacterBuffConfig config)
	{
		TowerDefenseCharacterBuffDizziness towerDefenseCharacterBuffDizziness = (TowerDefenseCharacterBuffDizziness)config;
		if (!towerDefenseCharacterBuffDizziness.magicImmune || !IsMagic())
		{
			time = Mathf.Max(time, towerDefenseCharacterBuffDizziness.time);
			magicImmune = magicImmune && towerDefenseCharacterBuffDizziness.magicImmune;
			currentTime = 0.0;
		}
	}

	private bool IsMagic()
	{
		if (!GodotObject.IsInstanceValid(character?.instance))
		{
			return false;
		}
		return (character.instance.physiqueTypeFlags & 0x1000) != 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Enter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnterReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowDizzinessVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.HideDizzinessVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsMagic, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ShowDizzinessVisual && args.Count == 0)
		{
			ShowDizzinessVisual();
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
		if (method == MethodName.HideDizzinessVisual && args.Count == 0)
		{
			HideDizzinessVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 1)
		{
			Refresh(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsMagic && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMagic());
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
		if (method == MethodName.Enter)
		{
			return true;
		}
		if (method == MethodName.EnterReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.ShowDizzinessVisual)
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
		if (method == MethodName.HideDizzinessVisual)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.IsMagic)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.time)
		{
			time = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.magicImmune)
		{
			magicImmune = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			currentTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dizzinessSprite)
		{
			dizzinessSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.time)
		{
			value = VariantUtils.CreateFrom(in time);
			return true;
		}
		if (name == PropertyName.magicImmune)
		{
			value = VariantUtils.CreateFrom(in magicImmune);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			value = VariantUtils.CreateFrom(in currentTime);
			return true;
		}
		if (name == PropertyName.dizzinessSprite)
		{
			value = VariantUtils.CreateFrom(in dizzinessSprite);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.magicImmune, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.dizzinessSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.time, Variant.From(in time));
		info.AddProperty(PropertyName.magicImmune, Variant.From(in magicImmune));
		info.AddProperty(PropertyName.currentTime, Variant.From(in currentTime));
		info.AddProperty(PropertyName.dizzinessSprite, Variant.From(in dizzinessSprite));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.time, out var value))
		{
			time = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.magicImmune, out var value2))
		{
			magicImmune = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.currentTime, out var value3))
		{
			currentTime = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dizzinessSprite, out var value4))
		{
			dizzinessSprite = value4.As<AdobeAnimateSprite>();
		}
	}
}
