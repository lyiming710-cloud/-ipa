using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter7/PiratePeater/PiratePeaterSprite.cs")]
public class PiratePeaterSprite : AdobeAnimateSpriteBase
{
	public new class MethodName : AdobeAnimateSpriteBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchPhysicsUpdate = "BatchPhysicsUpdate";
	}

	public new class PropertyName : AdobeAnimateSpriteBase.PropertyName
	{
		public static readonly StringName _head1 = "_head1";

		public static readonly StringName _head2 = "_head2";

		public static readonly StringName _head3 = "_head3";
	}

	public new class SignalName : AdobeAnimateSpriteBase.SignalName
	{
	}

	private AdobeAnimateSpriteBase _head1;

	private AdobeAnimateSpriteBase _head2;

	private AdobeAnimateSpriteBase _head3;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_head1 = GetNode<AdobeAnimateSpriteBase>("%Head1");
			_head2 = GetNode<AdobeAnimateSpriteBase>("%Head2");
			_head3 = GetNode<AdobeAnimateSpriteBase>("%Head3");
		}
	}

	public override void BatchPhysicsUpdate(double delta)
	{
		base.BatchPhysicsUpdate(delta);
		if (!Engine.IsEditorHint())
		{
			if (GodotObject.IsInstanceValid(_head1) && _head1.IsNodeReady())
			{
				_head1.pause = pause;
				_head1.LightMask = LightMask;
			}
			if (GodotObject.IsInstanceValid(_head2) && _head2.IsNodeReady())
			{
				_head2.pause = pause;
				_head2.LightMask = LightMask;
			}
			if (GodotObject.IsInstanceValid(_head3) && _head3.IsNodeReady())
			{
				_head3.pause = pause;
				_head3.LightMask = LightMask;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchPhysicsUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BatchPhysicsUpdate && args.Count == 1)
		{
			BatchPhysicsUpdate(VariantUtils.ConvertTo<double>(in args[0]));
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._head1)
		{
			_head1 = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._head2)
		{
			_head2 = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._head3)
		{
			_head3 = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._head1)
		{
			value = VariantUtils.CreateFrom(in _head1);
			return true;
		}
		if (name == PropertyName._head2)
		{
			value = VariantUtils.CreateFrom(in _head2);
			return true;
		}
		if (name == PropertyName._head3)
		{
			value = VariantUtils.CreateFrom(in _head3);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._head1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._head2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._head3, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._head1, Variant.From(in _head1));
		info.AddProperty(PropertyName._head2, Variant.From(in _head2));
		info.AddProperty(PropertyName._head3, Variant.From(in _head3));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._head1, out var value))
		{
			_head1 = value.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._head2, out var value2))
		{
			_head2 = value2.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._head3, out var value3))
		{
			_head3 = value3.As<AdobeAnimateSpriteBase>();
		}
	}
}
