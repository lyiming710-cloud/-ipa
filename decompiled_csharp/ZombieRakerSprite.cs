using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Other/Raker/ZombieRakerSprite.cs")]
public class ZombieRakerSprite : AdobeAnimateSpriteBase
{
	public new class MethodName : AdobeAnimateSpriteBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName BatchPhysicsUpdate = "BatchPhysicsUpdate";

		public static readonly StringName RefreshAttachments = "RefreshAttachments";

		public static readonly StringName RefreshAttachment = "RefreshAttachment";

		public new static readonly StringName OnPartSnapshotCreated = "OnPartSnapshotCreated";

		public static readonly StringName AppendAttachment = "AppendAttachment";
	}

	public new class PropertyName : AdobeAnimateSpriteBase.PropertyName
	{
		public static readonly StringName _mustacheSlot = "_mustacheSlot";

		public static readonly StringName _mustache = "_mustache";

		public static readonly StringName _rakeBackSlot = "_rakeBackSlot";

		public static readonly StringName _rakeBack = "_rakeBack";
	}

	public new class SignalName : AdobeAnimateSpriteBase.SignalName
	{
	}

	private AdobeAnimateSlot _mustacheSlot;

	private AdobeAnimatePart _mustache;

	private AdobeAnimateSlot _rakeBackSlot;

	private AdobeAnimatePart _rakeBack;

	public override void _Ready()
	{
		base._Ready();
		_mustacheSlot = GetNode<AdobeAnimateSlot>("MustacheSlot");
		_mustache = _mustacheSlot.GetNode<AdobeAnimatePart>("Mustache");
		_rakeBackSlot = GetNode<AdobeAnimateSlot>("RakeHeadBackSlot");
		_rakeBack = _rakeBackSlot.GetNode<AdobeAnimatePart>("RakeHeadBack");
		RefreshAttachments();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (AdobeAnimateSprite.IsEditorContext)
		{
			RefreshAttachments();
		}
	}

	public override void BatchPhysicsUpdate(double delta)
	{
		base.BatchPhysicsUpdate(delta);
		RefreshAttachments();
	}

	public void RefreshAttachments()
	{
		RefreshAttachment(this, _mustacheSlot, _mustache);
		RefreshAttachment(this, _rakeBackSlot, _rakeBack);
	}

	internal static void RefreshAttachment(AdobeAnimateSprite owner, AdobeAnimateSlot slot, AdobeAnimatePart visual)
	{
		if (GodotObject.IsInstanceValid(slot) && GodotObject.IsInstanceValid(visual))
		{
			Array<bool> layerVisibleForInternalRead = owner.GetLayerVisibleForInternalRead();
			int num = slot.followSlotId - 1;
			bool flag = num >= 0 && num < layerVisibleForInternalRead.Count && layerVisibleForInternalRead[num];
			if (AdobeAnimateManagedSprite2D.GetLogicalVisible(visual) != flag)
			{
				AdobeAnimateManagedSprite2D.SetLogicalVisible(visual, flag);
				owner.MarkManagedSlotVisualStateChanged();
			}
		}
	}

	internal override void OnPartSnapshotCreated(AdobeAnimateSlot slot, Array<StringName> layers, AdobeAnimatePart part)
	{
		RefreshAttachments();
		if (layers.Contains("anim_head1"))
		{
			AppendAttachment(this, slot, part, _mustacheSlot, _mustache);
		}
	}

	internal static void AppendAttachment(AdobeAnimateSprite owner, AdobeAnimateSlot anchor, AdobeAnimatePart part, AdobeAnimateSlot sourceSlot, AdobeAnimatePart source)
	{
		if (GodotObject.IsInstanceValid(source) && AdobeAnimateManagedSprite2D.GetLogicalVisible(source) && owner.TryGetManagedSlotTransformForRender(sourceSlot, out var transform))
		{
			AdobeAnimatePart adobeAnimatePart = AdobeAnimatePart.CreateAtlasTexturePart(source.externalAtlasTexturePath, source.externalAtlasCentered);
			adobeAnimatePart.Name = source.Name;
			adobeAnimatePart.Transform = transform.Translated(-anchor.Position) * source.Transform;
			adobeAnimatePart.Modulate = sourceSlot.Modulate * source.Modulate;
			adobeAnimatePart.SelfModulate = source.SelfModulate;
			part.AddChild(adobeAnimatePart, forceReadableName: false, InternalMode.Disabled);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchPhysicsUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAttachments, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshAttachment, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "visual", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnPartSnapshotCreated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Array, "layers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "part", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AppendAttachment, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "anchor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "part", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "sourceSlot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchPhysicsUpdate && args.Count == 1)
		{
			BatchPhysicsUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAttachments && args.Count == 0)
		{
			RefreshAttachments();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAttachment && args.Count == 3)
		{
			RefreshAttachment(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[1]), VariantUtils.ConvertTo<AdobeAnimatePart>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPartSnapshotCreated && args.Count == 3)
		{
			OnPartSnapshotCreated(VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[0]), VariantUtils.ConvertToArray<StringName>(in args[1]), VariantUtils.ConvertTo<AdobeAnimatePart>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AppendAttachment && args.Count == 5)
		{
			AppendAttachment(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[1]), VariantUtils.ConvertTo<AdobeAnimatePart>(in args[2]), VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[3]), VariantUtils.ConvertTo<AdobeAnimatePart>(in args[4]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RefreshAttachment && args.Count == 3)
		{
			RefreshAttachment(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[1]), VariantUtils.ConvertTo<AdobeAnimatePart>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AppendAttachment && args.Count == 5)
		{
			AppendAttachment(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[1]), VariantUtils.ConvertTo<AdobeAnimatePart>(in args[2]), VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[3]), VariantUtils.ConvertTo<AdobeAnimatePart>(in args[4]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.BatchPhysicsUpdate)
		{
			return true;
		}
		if (method == MethodName.RefreshAttachments)
		{
			return true;
		}
		if (method == MethodName.RefreshAttachment)
		{
			return true;
		}
		if (method == MethodName.OnPartSnapshotCreated)
		{
			return true;
		}
		if (method == MethodName.AppendAttachment)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._mustacheSlot)
		{
			_mustacheSlot = VariantUtils.ConvertTo<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._mustache)
		{
			_mustache = VariantUtils.ConvertTo<AdobeAnimatePart>(in value);
			return true;
		}
		if (name == PropertyName._rakeBackSlot)
		{
			_rakeBackSlot = VariantUtils.ConvertTo<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._rakeBack)
		{
			_rakeBack = VariantUtils.ConvertTo<AdobeAnimatePart>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._mustacheSlot)
		{
			value = VariantUtils.CreateFrom(in _mustacheSlot);
			return true;
		}
		if (name == PropertyName._mustache)
		{
			value = VariantUtils.CreateFrom(in _mustache);
			return true;
		}
		if (name == PropertyName._rakeBackSlot)
		{
			value = VariantUtils.CreateFrom(in _rakeBackSlot);
			return true;
		}
		if (name == PropertyName._rakeBack)
		{
			value = VariantUtils.CreateFrom(in _rakeBack);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._mustacheSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mustache, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rakeBackSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rakeBack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._mustacheSlot, Variant.From(in _mustacheSlot));
		info.AddProperty(PropertyName._mustache, Variant.From(in _mustache));
		info.AddProperty(PropertyName._rakeBackSlot, Variant.From(in _rakeBackSlot));
		info.AddProperty(PropertyName._rakeBack, Variant.From(in _rakeBack));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._mustacheSlot, out var value))
		{
			_mustacheSlot = value.As<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._mustache, out var value2))
		{
			_mustache = value2.As<AdobeAnimatePart>();
		}
		if (info.TryGetProperty(PropertyName._rakeBackSlot, out var value3))
		{
			_rakeBackSlot = value3.As<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._rakeBack, out var value4))
		{
			_rakeBack = value4.As<AdobeAnimatePart>();
		}
	}
}
