using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Other/Raker/ZombieRakerWallnutSprite.cs")]
public class ZombieRakerWallnutSprite : AdobeAnimateSpriteBase
{
	public new class MethodName : AdobeAnimateSpriteBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchPhysicsUpdate = "BatchPhysicsUpdate";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName RefreshRakeAttachment = "RefreshRakeAttachment";
	}

	public new class PropertyName : AdobeAnimateSpriteBase.PropertyName
	{
		public static readonly StringName head = "head";

		public static readonly StringName _headReady = "_headReady";

		public static readonly StringName _rakeBackSlot = "_rakeBackSlot";

		public static readonly StringName _rakeBack = "_rakeBack";
	}

	public new class SignalName : AdobeAnimateSpriteBase.SignalName
	{
	}

	public AdobeAnimateSpriteBase head;

	private bool _headReady;

	private AdobeAnimateSlot _rakeBackSlot;

	private AdobeAnimatePart _rakeBack;

	public override void _Ready()
	{
		base._Ready();
		_rakeBackSlot = GetNode<AdobeAnimateSlot>("RakeHeadBackSlot");
		_rakeBack = _rakeBackSlot.GetNode<AdobeAnimatePart>("RakeHeadBack");
		RefreshRakeAttachment();
		if (!AdobeAnimateSprite.IsEditorContext)
		{
			head = GetNode<AdobeAnimateSpriteBase>("%Head");
		}
	}

	public override void BatchPhysicsUpdate(double delta)
	{
		base.BatchPhysicsUpdate(delta);
		RefreshRakeAttachment();
		if (AdobeAnimateSprite.IsEditorContext || !GodotObject.IsInstanceValid(head) || head.GetParent() != this)
		{
			return;
		}
		if (!_headReady)
		{
			if (!head.IsRuntimeReadyCached)
			{
				return;
			}
			_headReady = true;
		}
		SyncRuntimeChildState(head);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (AdobeAnimateSprite.IsEditorContext)
		{
			RefreshRakeAttachment();
		}
	}

	public void RefreshRakeAttachment()
	{
		ZombieRakerSprite.RefreshAttachment(this, _rakeBackSlot, _rakeBack);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchPhysicsUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshRakeAttachment, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.BatchPhysicsUpdate && args.Count == 1)
		{
			BatchPhysicsUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshRakeAttachment && args.Count == 0)
		{
			RefreshRakeAttachment();
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
		if (method == MethodName.BatchPhysicsUpdate)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.RefreshRakeAttachment)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.head)
		{
			head = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._headReady)
		{
			_headReady = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.head)
		{
			value = VariantUtils.CreateFrom(in head);
			return true;
		}
		if (name == PropertyName._headReady)
		{
			value = VariantUtils.CreateFrom(in _headReady);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.head, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._headReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rakeBackSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rakeBack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.head, Variant.From(in head));
		info.AddProperty(PropertyName._headReady, Variant.From(in _headReady));
		info.AddProperty(PropertyName._rakeBackSlot, Variant.From(in _rakeBackSlot));
		info.AddProperty(PropertyName._rakeBack, Variant.From(in _rakeBack));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.head, out var value))
		{
			head = value.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._headReady, out var value2))
		{
			_headReady = value2.As<bool>();
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
