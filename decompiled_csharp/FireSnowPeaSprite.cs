using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter2/FireSnowPea/FireSnowPeaSprite.cs")]
public class FireSnowPeaSprite : AdobeAnimateSpriteBase
{
	public new class MethodName : AdobeAnimateSpriteBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchPhysicsUpdate = "BatchPhysicsUpdate";
	}

	public new class PropertyName : AdobeAnimateSpriteBase.PropertyName
	{
		public static readonly StringName headRight = "headRight";

		public static readonly StringName headLeft = "headLeft";
	}

	public new class SignalName : AdobeAnimateSpriteBase.SignalName
	{
	}

	private AdobeAnimateSpriteBase headRight;

	private AdobeAnimateSpriteBase headLeft;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			headRight = GetNode<AdobeAnimateSpriteBase>("%HeadRight");
			headLeft = GetNode<AdobeAnimateSpriteBase>("%HeadLeft");
		}
	}

	public override void BatchPhysicsUpdate(double delta)
	{
		base.BatchPhysicsUpdate(delta);
		if (Engine.IsEditorHint())
		{
			return;
		}
		if (headRight.IsNodeReady())
		{
			headRight.pause = pause;
			headRight.LightMask = LightMask;
		}
		if (headLeft.IsNodeReady())
		{
			headLeft.pause = pause;
			headLeft.LightMask = LightMask;
		}
		if (headRight.timeScale != 0.0 && headRight.clip == "FlameHeadIdle")
		{
			int num = headRight.clipRange.X + (frameIndex - clipRange.X);
			if (headRight.frameIndex < num - 1)
			{
				headLeft.timeScale = 2.0;
				headRight.timeScale = 2.0;
			}
			else if (headRight.frameIndex > num + 1)
			{
				headLeft.timeScale = 0.5;
				headRight.timeScale = 0.5;
			}
			else
			{
				headLeft.timeScale = 1.0;
				headRight.timeScale = 1.0;
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
		if (name == PropertyName.headRight)
		{
			headRight = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName.headLeft)
		{
			headLeft = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.headRight)
		{
			value = VariantUtils.CreateFrom(in headRight);
			return true;
		}
		if (name == PropertyName.headLeft)
		{
			value = VariantUtils.CreateFrom(in headLeft);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.headRight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.headLeft, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.headRight, Variant.From(in headRight));
		info.AddProperty(PropertyName.headLeft, Variant.From(in headLeft));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.headRight, out var value))
		{
			headRight = value.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName.headLeft, out var value2))
		{
			headLeft = value2.As<AdobeAnimateSpriteBase>();
		}
	}
}
