using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/Npc/Talk/Base/NpcTalkBaseConfig.cs")]
public class NpcTalkBaseConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName npc = "npc";

		public static readonly StringName text = "text";

		public static readonly StringName anime = "anime";

		public static readonly StringName audio = "audio";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string npc = "";

	[Export(PropertyHint.None, "")]
	public string text = "";

	[Export(PropertyHint.None, "")]
	public string anime = "";

	[Export(PropertyHint.None, "")]
	public string audio = "";

	public virtual void Init(Dictionary data)
	{
		npc = data["Npc"].AsString();
		text = data["Text"].AsString();
		anime = data["Anime"].AsString();
		audio = data["Audio"].AsString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.npc)
		{
			npc = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.text)
		{
			text = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.anime)
		{
			anime = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.audio)
		{
			audio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.npc)
		{
			value = VariantUtils.CreateFrom(in npc);
			return true;
		}
		if (name == PropertyName.text)
		{
			value = VariantUtils.CreateFrom(in text);
			return true;
		}
		if (name == PropertyName.anime)
		{
			value = VariantUtils.CreateFrom(in anime);
			return true;
		}
		if (name == PropertyName.audio)
		{
			value = VariantUtils.CreateFrom(in audio);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.npc, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.text, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.anime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.audio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.npc, Variant.From(in npc));
		info.AddProperty(PropertyName.text, Variant.From(in text));
		info.AddProperty(PropertyName.anime, Variant.From(in anime));
		info.AddProperty(PropertyName.audio, Variant.From(in audio));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.npc, out var value))
		{
			npc = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.text, out var value2))
		{
			text = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.anime, out var value3))
		{
			anime = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.audio, out var value4))
		{
			audio = value4.As<string>();
		}
	}
}
