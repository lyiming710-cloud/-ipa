using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Sprite/Gloompult/ZombieGargantuarGloompultSprite.cs")]
public class ZombieGargantuarGloompultSprite : AdobeAnimateSpriteBase
{
	public new class MethodName : AdobeAnimateSpriteBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchPhysicsUpdate = "BatchPhysicsUpdate";

		public new static readonly StringName ResetAnimation = "ResetAnimation";
	}

	public new class PropertyName : AdobeAnimateSpriteBase.PropertyName
	{
		public static readonly StringName gloompultHead = "gloompultHead";

		public static readonly StringName puffShroomImpHead = "puffShroomImpHead";

		public static readonly StringName _gloompultHead = "_gloompultHead";

		public static readonly StringName _puffShroomImpHead = "_puffShroomImpHead";
	}

	public new class SignalName : AdobeAnimateSpriteBase.SignalName
	{
	}

	private AdobeAnimateSpriteBase _gloompultHead;

	private AdobeAnimateSpriteBase _puffShroomImpHead;

	public AdobeAnimateSpriteBase gloompultHead => _gloompultHead;

	public AdobeAnimateSpriteBase puffShroomImpHead => _puffShroomImpHead;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_gloompultHead = GetNode<AdobeAnimateSpriteBase>("%GloompultHead");
			_puffShroomImpHead = GetNode<AdobeAnimateSpriteBase>("%PuffShroomImpHead");
		}
	}

	public override void BatchPhysicsUpdate(double delta)
	{
		base.BatchPhysicsUpdate(delta);
		if (GodotObject.IsInstanceValid(_gloompultHead) && _gloompultHead.IsRuntimeReadyCached)
		{
			SyncRuntimeChildState(_gloompultHead);
		}
		if (GodotObject.IsInstanceValid(_puffShroomImpHead) && _puffShroomImpHead.IsRuntimeReadyCached)
		{
			SyncRuntimeChildState(_puffShroomImpHead);
		}
	}

	public override void ResetAnimation()
	{
		base.ResetAnimation();
		if (GodotObject.IsInstanceValid(_gloompultHead))
		{
			_gloompultHead.ResetAnimation();
		}
		if (GodotObject.IsInstanceValid(_puffShroomImpHead))
		{
			_puffShroomImpHead.ResetAnimation();
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
		if (name == PropertyName._gloompultHead)
		{
			_gloompultHead = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._puffShroomImpHead)
		{
			_puffShroomImpHead = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		AdobeAnimateSpriteBase from;
		if (name == PropertyName.gloompultHead)
		{
			from = gloompultHead;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.puffShroomImpHead)
		{
			from = puffShroomImpHead;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._gloompultHead)
		{
			value = VariantUtils.CreateFrom(in _gloompultHead);
			return true;
		}
		if (name == PropertyName._puffShroomImpHead)
		{
			value = VariantUtils.CreateFrom(in _puffShroomImpHead);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._gloompultHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._puffShroomImpHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.gloompultHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.puffShroomImpHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._gloompultHead, Variant.From(in _gloompultHead));
		info.AddProperty(PropertyName._puffShroomImpHead, Variant.From(in _puffShroomImpHead));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._gloompultHead, out var value))
		{
			_gloompultHead = value.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._puffShroomImpHead, out var value2))
		{
			_puffShroomImpHead = value2.As<AdobeAnimateSpriteBase>();
		}
	}
}
