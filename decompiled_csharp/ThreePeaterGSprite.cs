using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter5/ThreePeaterG/ThreePeaterGSprite.cs")]
public class ThreePeaterGSprite : AdobeAnimateSpriteBase
{
	public new class MethodName : AdobeAnimateSpriteBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchPhysicsUpdate = "BatchPhysicsUpdate";
	}

	public new class PropertyName : AdobeAnimateSpriteBase.PropertyName
	{
		public static readonly StringName head1 = "head1";

		public static readonly StringName head2 = "head2";

		public static readonly StringName head3 = "head3";
	}

	public new class SignalName : AdobeAnimateSpriteBase.SignalName
	{
	}

	public AdobeAnimateSpriteBase head1;

	public AdobeAnimateSpriteBase head2;

	public AdobeAnimateSpriteBase head3;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			head1 = GetNode<AdobeAnimateSpriteBase>("%Head1");
			head2 = GetNode<AdobeAnimateSpriteBase>("%Head2");
			head3 = GetNode<AdobeAnimateSpriteBase>("%Head3");
		}
	}

	public override void BatchPhysicsUpdate(double delta)
	{
		base.BatchPhysicsUpdate(delta);
		if (!Engine.IsEditorHint())
		{
			if (head1.IsNodeReady())
			{
				head1.pause = pause;
				head1.LightMask = LightMask;
			}
			if (head2.IsNodeReady())
			{
				head2.pause = pause;
				head2.LightMask = LightMask;
			}
			if (head3.IsNodeReady())
			{
				head3.pause = pause;
				head3.LightMask = LightMask;
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
		if (name == PropertyName.head1)
		{
			head1 = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName.head2)
		{
			head2 = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName.head3)
		{
			head3 = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.head1)
		{
			value = VariantUtils.CreateFrom(in head1);
			return true;
		}
		if (name == PropertyName.head2)
		{
			value = VariantUtils.CreateFrom(in head2);
			return true;
		}
		if (name == PropertyName.head3)
		{
			value = VariantUtils.CreateFrom(in head3);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.head1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.head2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.head3, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.head1, Variant.From(in head1));
		info.AddProperty(PropertyName.head2, Variant.From(in head2));
		info.AddProperty(PropertyName.head3, Variant.From(in head3));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.head1, out var value))
		{
			head1 = value.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName.head2, out var value2))
		{
			head2 = value2.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName.head3, out var value3))
		{
			head3 = value3.As<AdobeAnimateSpriteBase>();
		}
	}
}
