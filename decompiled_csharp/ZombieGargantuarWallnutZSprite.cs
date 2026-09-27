using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Sprite/WallnutZ/ZombieGargantuarWallnutZSprite.cs")]
public class ZombieGargantuarWallnutZSprite : AdobeAnimateSpriteBase
{
	public new class MethodName : AdobeAnimateSpriteBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchPhysicsUpdate = "BatchPhysicsUpdate";

		public new static readonly StringName ResetAnimation = "ResetAnimation";
	}

	public new class PropertyName : AdobeAnimateSpriteBase.PropertyName
	{
		public static readonly StringName wallnutZHead = "wallnutZHead";

		public static readonly StringName peashooterZImpHead = "peashooterZImpHead";

		public static readonly StringName _wallnutZHead = "_wallnutZHead";

		public static readonly StringName _peashooterZImpHead = "_peashooterZImpHead";
	}

	public new class SignalName : AdobeAnimateSpriteBase.SignalName
	{
	}

	private AdobeAnimateSpriteBase _wallnutZHead;

	private AdobeAnimateSpriteBase _peashooterZImpHead;

	public AdobeAnimateSpriteBase wallnutZHead => _wallnutZHead;

	public AdobeAnimateSpriteBase peashooterZImpHead => _peashooterZImpHead;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_wallnutZHead = GetNode<AdobeAnimateSpriteBase>("%WallnutZHead");
			_peashooterZImpHead = GetNode<AdobeAnimateSpriteBase>("%PeashooterZImpHead");
		}
	}

	public override void BatchPhysicsUpdate(double delta)
	{
		base.BatchPhysicsUpdate(delta);
		if (GodotObject.IsInstanceValid(_wallnutZHead) && _wallnutZHead.IsRuntimeReadyCached)
		{
			SyncRuntimeChildState(_wallnutZHead);
		}
		if (GodotObject.IsInstanceValid(_peashooterZImpHead) && _peashooterZImpHead.IsRuntimeReadyCached)
		{
			SyncRuntimeChildState(_peashooterZImpHead);
		}
	}

	public override void ResetAnimation()
	{
		base.ResetAnimation();
		if (GodotObject.IsInstanceValid(_wallnutZHead))
		{
			_wallnutZHead.ResetAnimation();
		}
		if (GodotObject.IsInstanceValid(_peashooterZImpHead))
		{
			_peashooterZImpHead.ResetAnimation();
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
		if (name == PropertyName._wallnutZHead)
		{
			_wallnutZHead = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._peashooterZImpHead)
		{
			_peashooterZImpHead = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		AdobeAnimateSpriteBase from;
		if (name == PropertyName.wallnutZHead)
		{
			from = wallnutZHead;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.peashooterZImpHead)
		{
			from = peashooterZImpHead;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._wallnutZHead)
		{
			value = VariantUtils.CreateFrom(in _wallnutZHead);
			return true;
		}
		if (name == PropertyName._peashooterZImpHead)
		{
			value = VariantUtils.CreateFrom(in _peashooterZImpHead);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._wallnutZHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._peashooterZImpHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.wallnutZHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.peashooterZImpHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._wallnutZHead, Variant.From(in _wallnutZHead));
		info.AddProperty(PropertyName._peashooterZImpHead, Variant.From(in _peashooterZImpHead));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._wallnutZHead, out var value))
		{
			_wallnutZHead = value.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._peashooterZImpHead, out var value2))
		{
			_peashooterZImpHead = value2.As<AdobeAnimateSpriteBase>();
		}
	}
}
