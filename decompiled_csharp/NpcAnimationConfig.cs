using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/Npc/NpcAnimationConfig.cs")]
public class NpcAnimationConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName ResolveAnimation = "ResolveAnimation";

		public static readonly StringName IsLeaveAnimation = "IsLeaveAnimation";

		public static readonly StringName GetLoop = "GetLoop";

		public static readonly StringName GetBlendTime = "GetBlendTime";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName enterAnimation = "enterAnimation";

		public static readonly StringName idleAnimation = "idleAnimation";

		public static readonly StringName leaveAnimation = "leaveAnimation";

		public static readonly StringName aliases = "aliases";

		public static readonly StringName nextAnimations = "nextAnimations";

		public static readonly StringName loops = "loops";

		public static readonly StringName blendTimes = "blendTimes";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string enterAnimation = "";

	[Export(PropertyHint.None, "")]
	public string idleAnimation = "";

	[Export(PropertyHint.None, "")]
	public string leaveAnimation = "";

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, string> aliases = new Godot.Collections.Dictionary<string, string>();

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, string> nextAnimations = new Godot.Collections.Dictionary<string, string>();

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, bool> loops = new Godot.Collections.Dictionary<string, bool>();

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, double> blendTimes = new Godot.Collections.Dictionary<string, double>();

	public string ResolveAnimation(string animation)
	{
		if (string.IsNullOrEmpty(animation))
		{
			return "";
		}
		if (!aliases.TryGetValue(animation, out var value))
		{
			return animation;
		}
		return value;
	}

	public bool IsLeaveAnimation(string animation)
	{
		string text = ResolveAnimation(leaveAnimation);
		if (!string.IsNullOrEmpty(text))
		{
			return ResolveAnimation(animation) == text;
		}
		return false;
	}

	public bool GetLoop(string clip)
	{
		bool value;
		return !loops.TryGetValue(clip, out value) | value;
	}

	public double GetBlendTime(string clip)
	{
		if (!blendTimes.TryGetValue(clip, out var value))
		{
			return 0.0;
		}
		return value;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.ResolveAnimation, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsLeaveAnimation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLoop, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetBlendTime, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResolveAnimation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveAnimation(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsLeaveAnimation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLeaveAnimation(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLoop && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(GetLoop(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetBlendTime && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetBlendTime(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ResolveAnimation)
		{
			return true;
		}
		if (method == MethodName.IsLeaveAnimation)
		{
			return true;
		}
		if (method == MethodName.GetLoop)
		{
			return true;
		}
		if (method == MethodName.GetBlendTime)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.enterAnimation)
		{
			enterAnimation = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.idleAnimation)
		{
			idleAnimation = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.leaveAnimation)
		{
			leaveAnimation = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.aliases)
		{
			aliases = VariantUtils.ConvertToDictionary<string, string>(in value);
			return true;
		}
		if (name == PropertyName.nextAnimations)
		{
			nextAnimations = VariantUtils.ConvertToDictionary<string, string>(in value);
			return true;
		}
		if (name == PropertyName.loops)
		{
			loops = VariantUtils.ConvertToDictionary<string, bool>(in value);
			return true;
		}
		if (name == PropertyName.blendTimes)
		{
			blendTimes = VariantUtils.ConvertToDictionary<string, double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.enterAnimation)
		{
			value = VariantUtils.CreateFrom(in enterAnimation);
			return true;
		}
		if (name == PropertyName.idleAnimation)
		{
			value = VariantUtils.CreateFrom(in idleAnimation);
			return true;
		}
		if (name == PropertyName.leaveAnimation)
		{
			value = VariantUtils.CreateFrom(in leaveAnimation);
			return true;
		}
		if (name == PropertyName.aliases)
		{
			value = VariantUtils.CreateFromDictionary(aliases);
			return true;
		}
		if (name == PropertyName.nextAnimations)
		{
			value = VariantUtils.CreateFromDictionary(nextAnimations);
			return true;
		}
		if (name == PropertyName.loops)
		{
			value = VariantUtils.CreateFromDictionary(loops);
			return true;
		}
		if (name == PropertyName.blendTimes)
		{
			value = VariantUtils.CreateFromDictionary(blendTimes);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.enterAnimation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.idleAnimation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.leaveAnimation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.aliases, PropertyHint.TypeString, "4/0:;4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.nextAnimations, PropertyHint.TypeString, "4/0:;4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.loops, PropertyHint.TypeString, "4/0:;1/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.blendTimes, PropertyHint.TypeString, "4/0:;3/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.enterAnimation, Variant.From(in enterAnimation));
		info.AddProperty(PropertyName.idleAnimation, Variant.From(in idleAnimation));
		info.AddProperty(PropertyName.leaveAnimation, Variant.From(in leaveAnimation));
		info.AddProperty(PropertyName.aliases, Variant.CreateFrom(aliases));
		info.AddProperty(PropertyName.nextAnimations, Variant.CreateFrom(nextAnimations));
		info.AddProperty(PropertyName.loops, Variant.CreateFrom(loops));
		info.AddProperty(PropertyName.blendTimes, Variant.CreateFrom(blendTimes));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.enterAnimation, out var value))
		{
			enterAnimation = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.idleAnimation, out var value2))
		{
			idleAnimation = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.leaveAnimation, out var value3))
		{
			leaveAnimation = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.aliases, out var value4))
		{
			aliases = value4.AsGodotDictionary<string, string>();
		}
		if (info.TryGetProperty(PropertyName.nextAnimations, out var value5))
		{
			nextAnimations = value5.AsGodotDictionary<string, string>();
		}
		if (info.TryGetProperty(PropertyName.loops, out var value6))
		{
			loops = value6.AsGodotDictionary<string, bool>();
		}
		if (info.TryGetProperty(PropertyName.blendTimes, out var value7))
		{
			blendTimes = value7.AsGodotDictionary<string, double>();
		}
	}
}
