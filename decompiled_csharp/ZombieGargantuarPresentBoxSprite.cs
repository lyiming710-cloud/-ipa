using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Sprite/PresentBox/ZombieGargantuarPresentBoxSprite.cs")]
public class ZombieGargantuarPresentBoxSprite : AdobeAnimateSpriteBase
{
	public new class MethodName : AdobeAnimateSpriteBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchPhysicsUpdate = "BatchPhysicsUpdate";

		public new static readonly StringName ResetAnimation = "ResetAnimation";
	}

	public new class PropertyName : AdobeAnimateSpriteBase.PropertyName
	{
		public static readonly StringName presentBoxHead = "presentBoxHead";

		public static readonly StringName presentBoxImpHead = "presentBoxImpHead";

		public static readonly StringName _presentBoxHead = "_presentBoxHead";

		public static readonly StringName _presentBoxImpHead = "_presentBoxImpHead";
	}

	public new class SignalName : AdobeAnimateSpriteBase.SignalName
	{
	}

	private AdobeAnimateSpriteBase _presentBoxHead;

	private AdobeAnimateSpriteBase _presentBoxImpHead;

	public AdobeAnimateSpriteBase presentBoxHead => _presentBoxHead;

	public AdobeAnimateSpriteBase presentBoxImpHead => _presentBoxImpHead;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_presentBoxHead = GetNode<AdobeAnimateSpriteBase>("%PresentBoxHead");
			_presentBoxImpHead = GetNode<AdobeAnimateSpriteBase>("%PresentBoxImpHead");
		}
	}

	public override void BatchPhysicsUpdate(double delta)
	{
		base.BatchPhysicsUpdate(delta);
		if (GodotObject.IsInstanceValid(_presentBoxHead) && _presentBoxHead.IsRuntimeReadyCached)
		{
			SyncRuntimeChildState(_presentBoxHead);
		}
		if (GodotObject.IsInstanceValid(_presentBoxImpHead) && _presentBoxImpHead.IsRuntimeReadyCached)
		{
			SyncRuntimeChildState(_presentBoxImpHead);
		}
	}

	public override void ResetAnimation()
	{
		base.ResetAnimation();
		if (GodotObject.IsInstanceValid(_presentBoxHead))
		{
			_presentBoxHead.ResetAnimation();
		}
		if (GodotObject.IsInstanceValid(_presentBoxImpHead))
		{
			_presentBoxImpHead.ResetAnimation();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchPhysicsUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ResetAnimation && args.Count == 0)
		{
			ResetAnimation();
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
		if (method == MethodName.ResetAnimation)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._presentBoxHead)
		{
			_presentBoxHead = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._presentBoxImpHead)
		{
			_presentBoxImpHead = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		AdobeAnimateSpriteBase from;
		if (name == PropertyName.presentBoxHead)
		{
			from = presentBoxHead;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.presentBoxImpHead)
		{
			from = presentBoxImpHead;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._presentBoxHead)
		{
			value = VariantUtils.CreateFrom(in _presentBoxHead);
			return true;
		}
		if (name == PropertyName._presentBoxImpHead)
		{
			value = VariantUtils.CreateFrom(in _presentBoxImpHead);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._presentBoxHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._presentBoxImpHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.presentBoxHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.presentBoxImpHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._presentBoxHead, Variant.From(in _presentBoxHead));
		info.AddProperty(PropertyName._presentBoxImpHead, Variant.From(in _presentBoxImpHead));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._presentBoxHead, out var value))
		{
			_presentBoxHead = value.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._presentBoxImpHead, out var value2))
		{
			_presentBoxImpHead = value2.As<AdobeAnimateSpriteBase>();
		}
	}
}
