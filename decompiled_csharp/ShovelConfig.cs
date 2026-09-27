using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Shovel/Resource/ShovelConfig.cs")]
public class ShovelConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Execute = "Execute";

		public static readonly StringName Unlock = "Unlock";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName texture = "texture";

		public static readonly StringName saveKey = "saveKey";

		public static readonly StringName unlockCheckList = "unlockCheckList";

		public static readonly StringName name = "name";

		public static readonly StringName describe = "describe";

		public static readonly StringName handbookDescribe = "handbookDescribe";

		public static readonly StringName handbookStory = "handbookStory";

		public static readonly StringName eventList = "eventList";

		public static readonly StringName shovelableNames = "shovelableNames";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Texture2D texture;

	[Export(PropertyHint.None, "")]
	public string saveKey;

	[Export(PropertyHint.None, "")]
	public Array<UnlockConditionBaseConfig> unlockCheckList = new Array<UnlockConditionBaseConfig>();

	[Export(PropertyHint.None, "")]
	public string name;

	[Export(PropertyHint.None, "")]
	public string describe;

	[Export(PropertyHint.None, "")]
	public string handbookDescribe;

	[Export(PropertyHint.None, "")]
	public string handbookStory;

	[Export(PropertyHint.None, "")]
	public Array<ShovelEventConfig> eventList = new Array<ShovelEventConfig>();

	[Export(PropertyHint.None, "")]
	public Array<string> shovelableNames = new Array<string>();

	public void Execute(TowerDefenseCharacter character)
	{
		foreach (ShovelEventConfig @event in eventList)
		{
			@event.Execute(character);
		}
	}

	public bool Unlock()
	{
		if (CommandManager.Instance.debugPacketOpenAll)
		{
			return true;
		}
		GlobalFeatureManager instance = GlobalFeatureManager.Instance;
		if (instance == null || !instance.IsRegistered(saveKey))
		{
			return false;
		}
		if (!instance.IsUnlocked(saveKey))
		{
			if (unlockCheckList.Count <= 0)
			{
				return false;
			}
			foreach (UnlockConditionBaseConfig unlockCheck in unlockCheckList)
			{
				if (!unlockCheck.Check())
				{
					return false;
				}
			}
			instance.Unlock(saveKey);
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Unlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Execute && args.Count == 1)
		{
			Execute(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Unlock && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Unlock());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.Unlock)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.texture)
		{
			texture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.saveKey)
		{
			saveKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.unlockCheckList)
		{
			unlockCheckList = VariantUtils.ConvertToArray<UnlockConditionBaseConfig>(in value);
			return true;
		}
		if (name == PropertyName.name)
		{
			this.name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.describe)
		{
			describe = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.handbookDescribe)
		{
			handbookDescribe = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.handbookStory)
		{
			handbookStory = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<ShovelEventConfig>(in value);
			return true;
		}
		if (name == PropertyName.shovelableNames)
		{
			shovelableNames = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.texture)
		{
			value = VariantUtils.CreateFrom(in texture);
			return true;
		}
		if (name == PropertyName.saveKey)
		{
			value = VariantUtils.CreateFrom(in saveKey);
			return true;
		}
		if (name == PropertyName.unlockCheckList)
		{
			value = VariantUtils.CreateFromArray(unlockCheckList);
			return true;
		}
		if (name == PropertyName.name)
		{
			value = VariantUtils.CreateFrom(in this.name);
			return true;
		}
		if (name == PropertyName.describe)
		{
			value = VariantUtils.CreateFrom(in describe);
			return true;
		}
		if (name == PropertyName.handbookDescribe)
		{
			value = VariantUtils.CreateFrom(in handbookDescribe);
			return true;
		}
		if (name == PropertyName.handbookStory)
		{
			value = VariantUtils.CreateFrom(in handbookStory);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		if (name == PropertyName.shovelableNames)
		{
			value = VariantUtils.CreateFromArray(shovelableNames);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.texture, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.saveKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.unlockCheckList, PropertyHint.TypeString, "24/17:UnlockConditionBaseConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.name, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.describe, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.handbookDescribe, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.handbookStory, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:ShovelEventConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.shovelableNames, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.texture, Variant.From(in texture));
		info.AddProperty(PropertyName.saveKey, Variant.From(in saveKey));
		info.AddProperty(PropertyName.unlockCheckList, Variant.CreateFrom(unlockCheckList));
		info.AddProperty(PropertyName.name, Variant.From(in name));
		info.AddProperty(PropertyName.describe, Variant.From(in describe));
		info.AddProperty(PropertyName.handbookDescribe, Variant.From(in handbookDescribe));
		info.AddProperty(PropertyName.handbookStory, Variant.From(in handbookStory));
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.shovelableNames, Variant.CreateFrom(shovelableNames));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.texture, out var value))
		{
			texture = value.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.saveKey, out var value2))
		{
			saveKey = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.unlockCheckList, out var value3))
		{
			unlockCheckList = value3.AsGodotArray<UnlockConditionBaseConfig>();
		}
		if (info.TryGetProperty(PropertyName.name, out var value4))
		{
			name = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.describe, out var value5))
		{
			describe = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.handbookDescribe, out var value6))
		{
			handbookDescribe = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.handbookStory, out var value7))
		{
			handbookStory = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.eventList, out var value8))
		{
			eventList = value8.AsGodotArray<ShovelEventConfig>();
		}
		if (info.TryGetProperty(PropertyName.shovelableNames, out var value9))
		{
			shovelableNames = value9.AsGodotArray<string>();
		}
	}
}
