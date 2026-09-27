using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/Tutorial/Condition/TutorialConditionCheckCharaterNum.cs")]
public class TutorialConditionCheckCharaterNum : TutorialConditionConfig
{
	public new class MethodName : TutorialConditionConfig.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName Step = "Step";
	}

	public new class PropertyName : TutorialConditionConfig.PropertyName
	{
		public static readonly StringName characterName = "characterName";

		public static readonly StringName method = "method";

		public static readonly StringName num = "num";
	}

	public new class SignalName : TutorialConditionConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string characterName = "";

	[Export(PropertyHint.Enum, ">,>=,==,<,<=")]
	public string method = ">";

	[Export(PropertyHint.None, "")]
	public int num = 1;

	public override void Init(Dictionary data)
	{
		base.Init(data);
		characterName = data.GetValueOrDefault("CharacterName", "").AsString();
		method = data.GetValueOrDefault("Method", ">").AsString();
		num = data.GetValueOrDefault("Num", 1).AsInt32();
	}

	public override bool Step()
	{
		Array characterFromName = TowerDefenseManager.Instance.GetCharacterFromName(characterName);
		return method switch
		{
			">" => characterFromName.Count > num, 
			">=" => characterFromName.Count >= num, 
			"==" => characterFromName.Count == num, 
			"<" => characterFromName.Count < num, 
			"<=" => characterFromName.Count <= num, 
			_ => false, 
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Step, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Step && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Step());
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
		if (method == MethodName.Step)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.characterName)
		{
			characterName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.method)
		{
			method = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.characterName)
		{
			value = VariantUtils.CreateFrom(in characterName);
			return true;
		}
		if (name == PropertyName.method)
		{
			value = VariantUtils.CreateFrom(in method);
			return true;
		}
		if (name == PropertyName.num)
		{
			value = VariantUtils.CreateFrom(in num);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.characterName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.method, PropertyHint.Enum, ">,>=,==,<,<=", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.characterName, Variant.From(in characterName));
		info.AddProperty(PropertyName.method, Variant.From(in method));
		info.AddProperty(PropertyName.num, Variant.From(in num));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.characterName, out var value))
		{
			characterName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.method, out var value2))
		{
			method = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.num, out var value3))
		{
			num = value3.As<int>();
		}
	}
}
