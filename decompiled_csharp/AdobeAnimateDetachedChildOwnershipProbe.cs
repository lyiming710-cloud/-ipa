using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateDetachedChildOwnershipProbe.cs")]
public class AdobeAnimateDetachedChildOwnershipProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RunDetachedChildCheck = "RunDetachedChildCheck";

		public static readonly StringName Contains = "Contains";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		try
		{
			RunDetachedChildCheck();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[AdobeAnimateDetachedChildOwnershipProbe] Unexpected exception: {value}");
		}
		bool flag = _failures == 0;
		GD.Print($"ADOBE_ANIMATE_DETACHED_CHILD_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private void RunDetachedChildCheck()
	{
		AdobeAnimateSprite adobeAnimateSprite = new AdobeAnimateSprite
		{
			Name = "OldParent"
		};
		AdobeAnimateSprite adobeAnimateSprite2 = new AdobeAnimateSprite
		{
			Name = "DetachedHead"
		};
		Node2D node2D = new Node2D
		{
			Name = "DamagePartDrop"
		};
		adobeAnimateSprite.layerVisible.Add(item: true);
		adobeAnimateSprite.AddChild(adobeAnimateSprite2, forceReadableName: false, InternalMode.Disabled);
		AddChild(adobeAnimateSprite, forceReadableName: false, InternalMode.Disabled);
		AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
		adobeAnimateSprite.InsertSpriteAtLayer(adobeAnimateSprite2, 0);
		Check(Contains(adobeAnimateSprite.GetSpriteChildrenForRender(), adobeAnimateSprite2), "The original parent must initially cache its nested animation.");
		Check(adobeAnimateSprite2.IsRenderedByParentSpriteForRender(), "The nested animation must initially render through its original parent.");
		adobeAnimateSprite.RemoveChild(adobeAnimateSprite2);
		node2D.AddChild(adobeAnimateSprite2, forceReadableName: false, InternalMode.Disabled);
		Check(!Contains(adobeAnimateSprite.GetSpriteChildrenForRender(), adobeAnimateSprite2), "Detaching a damage part must evict it from the original parent child cache.");
		Check(!adobeAnimateSprite2.IsRenderedByParentSpriteForRender(), "A detached damage part must render independently instead of through its original parent.");
		adobeAnimateSprite2.QueueFree();
		adobeAnimateSprite.QueueFree();
		node2D.QueueFree();
	}

	private static bool Contains(AdobeAnimateSprite[] sprites, AdobeAnimateSprite expected)
	{
		for (int i = 0; i < sprites.Length; i++)
		{
			if (sprites[i] == expected)
			{
				return true;
			}
		}
		return false;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[AdobeAnimateDetachedChildOwnershipProbe] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunDetachedChildCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Contains, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "sprites", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "expected", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RunDetachedChildCheck && args.Count == 0)
		{
			RunDetachedChildCheck();
			ret = default;
			return true;
		}
		if (method == MethodName.Contains && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(Contains(VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[1])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Contains && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(Contains(VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[1])));
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
		if (method == MethodName.RunDetachedChildCheck)
		{
			return true;
		}
		if (method == MethodName.Contains)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
