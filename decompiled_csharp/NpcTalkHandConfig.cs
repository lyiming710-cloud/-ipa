using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/Npc/Talk/Config/NpcTalkHandConfig.cs")]
public class NpcTalkHandConfig : NpcTalkBaseConfig
{
	public new class MethodName : NpcTalkBaseConfig.MethodName
	{
		public new static readonly StringName Init = "Init";
	}

	public new class PropertyName : NpcTalkBaseConfig.PropertyName
	{
		public static readonly StringName handScene = "handScene";

		public static readonly StringName shoulderScene = "shoulderScene";

		public static readonly StringName shoulder2Scene = "shoulder2Scene";

		public static readonly StringName headScene = "headScene";
	}

	public new class SignalName : NpcTalkBaseConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public PackedScene handScene;

	[Export(PropertyHint.None, "")]
	public PackedScene shoulderScene;

	[Export(PropertyHint.None, "")]
	public PackedScene shoulder2Scene;

	[Export(PropertyHint.None, "")]
	public PackedScene headScene;

	public override void Init(Dictionary data)
	{
		base.Init(data);
		Array array = data["Arg"].AsGodotArray();
		if (array.Count >= 1)
		{
			handScene = GD.Load<PackedScene>(array[0].AsString());
		}
		if (array.Count >= 2)
		{
			shoulderScene = GD.Load<PackedScene>(array[1].AsString());
		}
		if (array.Count >= 3)
		{
			shoulder2Scene = GD.Load<PackedScene>(array[2].AsString());
		}
		if (array.Count >= 4)
		{
			headScene = GD.Load<PackedScene>(array[3].AsString());
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
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
		if (name == PropertyName.handScene)
		{
			handScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.shoulderScene)
		{
			shoulderScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.shoulder2Scene)
		{
			shoulder2Scene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.headScene)
		{
			headScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.handScene)
		{
			value = VariantUtils.CreateFrom(in handScene);
			return true;
		}
		if (name == PropertyName.shoulderScene)
		{
			value = VariantUtils.CreateFrom(in shoulderScene);
			return true;
		}
		if (name == PropertyName.shoulder2Scene)
		{
			value = VariantUtils.CreateFrom(in shoulder2Scene);
			return true;
		}
		if (name == PropertyName.headScene)
		{
			value = VariantUtils.CreateFrom(in headScene);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.handScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.shoulderScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.shoulder2Scene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.headScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.handScene, Variant.From(in handScene));
		info.AddProperty(PropertyName.shoulderScene, Variant.From(in shoulderScene));
		info.AddProperty(PropertyName.shoulder2Scene, Variant.From(in shoulder2Scene));
		info.AddProperty(PropertyName.headScene, Variant.From(in headScene));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.handScene, out var value))
		{
			handScene = value.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.shoulderScene, out var value2))
		{
			shoulderScene = value2.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.shoulder2Scene, out var value3))
		{
			shoulder2Scene = value3.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.headScene, out var value4))
		{
			headScene = value4.As<PackedScene>();
		}
	}
}
