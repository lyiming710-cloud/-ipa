using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Buff/TowerDefenseCharacterBuffFrozen.cs")]
public class TowerDefenseCharacterBuffFrozen : TowerDefenseCharacterBuffConfig
{
	public new class MethodName : TowerDefenseCharacterBuffConfig.MethodName
	{
		public new static readonly StringName _Init = "_Init";

		public new static readonly StringName Enter = "Enter";

		public new static readonly StringName EnterReadOnlyClient = "EnterReadOnlyClient";

		public new static readonly StringName Step = "Step";

		public new static readonly StringName StepReadOnlyClient = "StepReadOnlyClient";

		public new static readonly StringName Exit = "Exit";

		public static readonly StringName RejectFrozenAndApplyIceSpeedDown = "RejectFrozenAndApplyIceSpeedDown";

		public static readonly StringName ApplyIceSpeedDown = "ApplyIceSpeedDown";

		public new static readonly StringName Remove = "Remove";

		public new static readonly StringName ExitReadOnlyClient = "ExitReadOnlyClient";

		public new static readonly StringName Refresh = "Refresh";

		public static readonly StringName ShowIceTrapVisual = "ShowIceTrapVisual";

		public static readonly StringName CreateIceTrapSprite = "CreateIceTrapSprite";

		public static readonly StringName ConfigureIceTrapSprite = "ConfigureIceTrapSprite";
	}

	public new class PropertyName : TowerDefenseCharacterBuffConfig.PropertyName
	{
		public new static readonly StringName FrameMeshColorMultiplier = "FrameMeshColorMultiplier";

		public static readonly StringName time = "time";

		public static readonly StringName iceSpeedDownTime = "iceSpeedDownTime";

		public static readonly StringName currentTime = "currentTime";
	}

	public new class SignalName : TowerDefenseCharacterBuffConfig.SignalName
	{
	}

	public const string VisualKey = "Frozen";

	private static readonly Color ICE_SPEED_DOWN_COLOR = new Color(0.2f, 0.35f, 1f);

	[Export(PropertyHint.None, "")]
	public double time = 8.0;

	[Export(PropertyHint.None, "")]
	public double iceSpeedDownTime = 15.0;

	[Export(PropertyHint.None, "")]
	public double currentTime;

	public override Color FrameMeshColorMultiplier => ICE_SPEED_DOWN_COLOR;

	public override void _Init()
	{
		key = "Frozen";
	}

	public override void Enter()
	{
		if (character.instance.maskFlags == 0 || (character.instance.maskFlags & 2) != 0)
		{
			RejectFrozenAndApplyIceSpeedDown();
			return;
		}
		if ((character.instance.unUseBuffFlags & 2) != 0)
		{
			RejectFrozenAndApplyIceSpeedDown();
			return;
		}
		character.buff.DeleteBuff("Burn");
		ShowIceTrapVisual();
	}

	public override void EnterReadOnlyClient()
	{
		ShowIceTrapVisual();
	}

	public override bool Step(double delta)
	{
		currentTime += delta;
		if (character.nearDie || character.die || currentTime >= time)
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
		character.buff?.SetBuffVisualVisible("Frozen", visible: false);
		character.CreateIceTrap();
		ApplyIceSpeedDown();
	}

	private void RejectFrozenAndApplyIceSpeedDown()
	{
		character.buff.DeleteBuff("Frozen");
		ApplyIceSpeedDown();
	}

	private void ApplyIceSpeedDown()
	{
		if (iceSpeedDownTime != 0.0)
		{
			TowerDefenseCharacterBuffIceSpeedDown towerDefenseCharacterBuffIceSpeedDown = new TowerDefenseCharacterBuffIceSpeedDown();
			towerDefenseCharacterBuffIceSpeedDown.time = iceSpeedDownTime;
			character.buff.AddBuff(towerDefenseCharacterBuffIceSpeedDown);
		}
	}

	public override void Remove()
	{
		ExitReadOnlyClient();
	}

	public override void ExitReadOnlyClient()
	{
		character.buff?.SetBuffVisualVisible("Frozen", visible: false);
	}

	public override void Refresh(TowerDefenseCharacterBuffConfig config)
	{
		TowerDefenseCharacterBuffFrozen towerDefenseCharacterBuffFrozen = config as TowerDefenseCharacterBuffFrozen;
		time = Mathf.Max(time, towerDefenseCharacterBuffFrozen.time + GD.RandRange(-0.2, 0.2));
		iceSpeedDownTime = towerDefenseCharacterBuffFrozen.iceSpeedDownTime;
		currentTime = 0.0;
	}

	private void ShowIceTrapVisual()
	{
		BuffComponent buffComponent = character?.buff;
		if (buffComponent == null || buffComponent.IsReleased)
		{
			return;
		}
		Sprite2D cachedBuffVisual = buffComponent.GetCachedBuffVisual("Frozen");
		if (!GodotObject.IsInstanceValid(cachedBuffVisual))
		{
			if (!buffComponent.TryGetVisualDefinition("Frozen", out var visualDefinition) || visualDefinition == null || !visualDefinition.enabled || !GodotObject.IsInstanceValid(character.spriteGroup))
			{
				return;
			}
			cachedBuffVisual = CreateIceTrapSprite(visualDefinition);
			if (!GodotObject.IsInstanceValid(cachedBuffVisual))
			{
				return;
			}
			ConfigureIceTrapSprite(cachedBuffVisual, visualDefinition);
			character.spriteGroup.AddChild(cachedBuffVisual, forceReadableName: false, Node.InternalMode.Disabled);
			buffComponent.RegisterBuffVisual("Frozen", cachedBuffVisual, new AdobeAnimateExternalVisualDescriptor(AdobeAnimateExternalVisualAttachmentMode.Root, visualDefinition.drawBand));
		}
		buffComponent.SetBuffVisualVisible("Frozen", visible: true);
	}

	private static Sprite2D CreateIceTrapSprite(BuffVisualDefinition definition)
	{
		if (GodotObject.IsInstanceValid(definition.scene))
		{
			Node node = definition.scene.Instantiate(PackedScene.GenEditState.Disabled);
			if (node is Sprite2D result)
			{
				return result;
			}
			if (GodotObject.IsInstanceValid(node))
			{
				node.Free();
			}
		}
		return new Sprite2D();
	}

	private static void ConfigureIceTrapSprite(Sprite2D sprite, BuffVisualDefinition definition)
	{
		if (GodotObject.IsInstanceValid(definition.texture))
		{
			sprite.Texture = definition.texture;
		}
		sprite.Name = (definition.nodeName.IsEmpty ? new StringName("IcetrapSprite") : definition.nodeName);
		sprite.Position = definition.position;
		sprite.Scale = definition.scale;
		sprite.Rotation = definition.rotation;
		sprite.ZIndex = definition.zIndex;
		sprite.ZAsRelative = definition.zAsRelative;
		sprite.Centered = definition.centered;
		sprite.Offset = definition.offset;
		sprite.Visible = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Enter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnterReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Step, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StepReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Exit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RejectFrozenAndApplyIceSpeedDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyIceSpeedDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Remove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExitReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowIceTrapVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateIceTrapSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Sprite2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureIceTrapSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Sprite2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.RejectFrozenAndApplyIceSpeedDown && args.Count == 0)
		{
			RejectFrozenAndApplyIceSpeedDown();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyIceSpeedDown && args.Count == 0)
		{
			ApplyIceSpeedDown();
			ret = default;
			return true;
		}
		if (method == MethodName.Remove && args.Count == 0)
		{
			Remove();
			ret = default;
			return true;
		}
		if (method == MethodName.ExitReadOnlyClient && args.Count == 0)
		{
			ExitReadOnlyClient();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 1)
		{
			Refresh(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowIceTrapVisual && args.Count == 0)
		{
			ShowIceTrapVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateIceTrapSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Sprite2D>(CreateIceTrapSprite(VariantUtils.ConvertTo<BuffVisualDefinition>(in args[0])));
			return true;
		}
		if (method == MethodName.ConfigureIceTrapSprite && args.Count == 2)
		{
			ConfigureIceTrapSprite(VariantUtils.ConvertTo<Sprite2D>(in args[0]), VariantUtils.ConvertTo<BuffVisualDefinition>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateIceTrapSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Sprite2D>(CreateIceTrapSprite(VariantUtils.ConvertTo<BuffVisualDefinition>(in args[0])));
			return true;
		}
		if (method == MethodName.ConfigureIceTrapSprite && args.Count == 2)
		{
			ConfigureIceTrapSprite(VariantUtils.ConvertTo<Sprite2D>(in args[0]), VariantUtils.ConvertTo<BuffVisualDefinition>(in args[1]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
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
		if (method == MethodName.RejectFrozenAndApplyIceSpeedDown)
		{
			return true;
		}
		if (method == MethodName.ApplyIceSpeedDown)
		{
			return true;
		}
		if (method == MethodName.Remove)
		{
			return true;
		}
		if (method == MethodName.ExitReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.ShowIceTrapVisual)
		{
			return true;
		}
		if (method == MethodName.CreateIceTrapSprite)
		{
			return true;
		}
		if (method == MethodName.ConfigureIceTrapSprite)
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
		if (name == PropertyName.iceSpeedDownTime)
		{
			iceSpeedDownTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			currentTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.FrameMeshColorMultiplier)
		{
			value = VariantUtils.CreateFrom<Color>(FrameMeshColorMultiplier);
			return true;
		}
		if (name == PropertyName.time)
		{
			value = VariantUtils.CreateFrom(in time);
			return true;
		}
		if (name == PropertyName.iceSpeedDownTime)
		{
			value = VariantUtils.CreateFrom(in iceSpeedDownTime);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			value = VariantUtils.CreateFrom(in currentTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Color, PropertyName.FrameMeshColorMultiplier, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.iceSpeedDownTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.time, Variant.From(in time));
		info.AddProperty(PropertyName.iceSpeedDownTime, Variant.From(in iceSpeedDownTime));
		info.AddProperty(PropertyName.currentTime, Variant.From(in currentTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.time, out var value))
		{
			time = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.iceSpeedDownTime, out var value2))
		{
			iceSpeedDownTime = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.currentTime, out var value3))
		{
			currentTime = value3.As<double>();
		}
	}
}
