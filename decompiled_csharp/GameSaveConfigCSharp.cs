using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/GameSaveManager/Resource/GameSaveConfigCSharp.cs")]
public class GameSaveConfigCSharp : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName _Init = "_Init";

		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public static readonly StringName InitUser = "InitUser";

		public static readonly StringName RenameUser = "RenameUser";

		public static readonly StringName DeleteUser = "DeleteUser";

		public static readonly StringName TowerDefensePacketDictionaryInit = "TowerDefensePacketDictionaryInit";

		public static readonly StringName TowerDefensePacketDictionaryInitData = "TowerDefensePacketDictionaryInitData";

		public static readonly StringName FeatureDictionaryInit = "FeatureDictionaryInit";

		public static readonly StringName TutorialDictionaryInit = "TutorialDictionaryInit";

		public static readonly StringName LevelDictionaryInit = "LevelDictionaryInit";

		public static readonly StringName KeyDictionaryInit = "KeyDictionaryInit";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName userCurrent = "userCurrent";

		public static readonly StringName userList = "userList";

		public static readonly StringName saveDictionary = "saveDictionary";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string userCurrent = "";

	[Export(PropertyHint.None, "")]
	public Array<string> userList = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Dictionary saveDictionary = new Dictionary();

	public void _Init()
	{
		userCurrent = "";
		userList = new Array<string>();
		saveDictionary = new Dictionary();
	}

	public void Init(Dictionary data)
	{
		userCurrent = data["Current"].AsString();
		Variant variant = Json.ParseString(data["List"].AsString());
		userList = new Array<string>();
		foreach (Variant item in variant.AsGodotArray())
		{
			userList.Add(item.AsString());
		}
		saveDictionary = Json.ParseString(data["Dictionary"].AsString()).AsGodotDictionary();
	}

	public Dictionary Export()
	{
		return new Dictionary
		{
			["Current"] = userCurrent,
			["List"] = Json.Stringify(userList),
			["Dictionary"] = Json.Stringify(saveDictionary)
		};
	}

	public void InitUser(string user)
	{
		Dictionary dictionary = new Dictionary();
		TowerDefensePacketDictionaryInit(dictionary);
		FeatureDictionaryInit(dictionary);
		TutorialDictionaryInit(dictionary);
		LevelDictionaryInit(dictionary);
		KeyDictionaryInit(dictionary);
		userList.Add(user);
		saveDictionary[user] = dictionary;
		string path = "user://Csharp/Progress/" + user;
		if (!DirAccess.DirExistsAbsolute(path))
		{
			DirAccess.MakeDirRecursiveAbsolute(path);
		}
	}

	public void RenameUser(string user, string newName)
	{
		saveDictionary[newName] = saveDictionary[user];
		saveDictionary.Remove(user);
		userList.Remove(user);
		userList.Add(newName);
		string text = "user://Csharp/Progress/" + user + "/";
		string to = "user://Csharp/Progress/" + newName + "/";
		if (DirAccess.DirExistsAbsolute(text))
		{
			DirAccess.RenameAbsolute(text, to);
		}
	}

	public void DeleteUser(string user)
	{
		userList.Remove(user);
		saveDictionary.Remove(user);
		string text = "user://Csharp/Progress/" + user + "/";
		if (DirAccess.DirExistsAbsolute(text))
		{
			string[] filesAt = DirAccess.GetFilesAt(text);
			foreach (string text2 in filesAt)
			{
				DirAccess.RemoveAbsolute(text + text2);
			}
			DirAccess.RemoveAbsolute(text);
		}
	}

	public void TowerDefensePacketDictionaryInit(Dictionary dictionary)
	{
		dictionary["TowerDefensePacket"] = new Dictionary();
		Dictionary dictionary2 = GameSaveManager.TOWER_DEFENSE_PACKET_INIT.Data.AsGodotDictionary();
		foreach (Variant key in dictionary2.Keys)
		{
			dictionary["TowerDefensePacket"].AsGodotDictionary()[key] = TowerDefensePacketDictionaryInitData(dictionary2[key].AsBool());
		}
	}

	public Dictionary TowerDefensePacketDictionaryInitData(bool unlock)
	{
		return new Dictionary
		{
			["Unlock"] = unlock,
			["Love"] = false,
			["Key"] = new Dictionary { ["Custom"] = "" }
		};
	}

	public void FeatureDictionaryInit(Dictionary dictionary)
	{
		dictionary["Feature"] = new Dictionary();
		Dictionary dictionary2 = GameSaveManager.FEATURE_INIT.Data.AsGodotDictionary();
		foreach (Variant key in dictionary2.Keys)
		{
			dictionary["Feature"].AsGodotDictionary()[key] = dictionary2[key];
		}
	}

	public void TutorialDictionaryInit(Dictionary dictionary)
	{
		dictionary["Tutorial"] = new Dictionary();
		Dictionary dictionary2 = GameSaveManager.TUTORIAL_INIT.Data.AsGodotDictionary();
		foreach (Variant key in dictionary2.Keys)
		{
			dictionary["Tutorial"].AsGodotDictionary()[key] = dictionary2[key];
		}
	}

	public void LevelDictionaryInit(Dictionary dictionary)
	{
		dictionary["Level"] = new Dictionary();
		foreach (Variant key9 in GameSaveManager.LEVEL_INIT.Data.AsGodotDictionary().Keys)
		{
			dictionary["Level"].AsGodotDictionary()[key9] = new Dictionary
			{
				["Normal"] = false,
				["Difficult"] = false,
				["Ultimate"] = false,
				["Mower"] = false,
				["Key"] = new Dictionary
				{
					["Like"] = -1,
					["Played"] = 0,
					["Finish"] = 0
				}
			};
		}
	}

	public void KeyDictionaryInit(Dictionary dictionary)
	{
		dictionary["Key"] = new Dictionary();
		Dictionary dictionary2 = GameSaveManager.KEY_INIT.Data.AsGodotDictionary();
		foreach (Variant key in dictionary2.Keys)
		{
			dictionary["Key"].AsGodotDictionary()[key] = dictionary2[key];
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitUser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenameUser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "newName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DeleteUser, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TowerDefensePacketDictionaryInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TowerDefensePacketDictionaryInitData, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "unlock", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FeatureDictionaryInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TutorialDictionaryInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelDictionaryInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.KeyDictionaryInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Init && args.Count == 0)
		{
			_Init();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.InitUser && args.Count == 1)
		{
			InitUser(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenameUser && args.Count == 2)
		{
			RenameUser(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteUser && args.Count == 1)
		{
			DeleteUser(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TowerDefensePacketDictionaryInit && args.Count == 1)
		{
			TowerDefensePacketDictionaryInit(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TowerDefensePacketDictionaryInitData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(TowerDefensePacketDictionaryInitData(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.FeatureDictionaryInit && args.Count == 1)
		{
			FeatureDictionaryInit(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TutorialDictionaryInit && args.Count == 1)
		{
			TutorialDictionaryInit(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelDictionaryInit && args.Count == 1)
		{
			LevelDictionaryInit(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.KeyDictionaryInit && args.Count == 1)
		{
			KeyDictionaryInit(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Init)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.InitUser)
		{
			return true;
		}
		if (method == MethodName.RenameUser)
		{
			return true;
		}
		if (method == MethodName.DeleteUser)
		{
			return true;
		}
		if (method == MethodName.TowerDefensePacketDictionaryInit)
		{
			return true;
		}
		if (method == MethodName.TowerDefensePacketDictionaryInitData)
		{
			return true;
		}
		if (method == MethodName.FeatureDictionaryInit)
		{
			return true;
		}
		if (method == MethodName.TutorialDictionaryInit)
		{
			return true;
		}
		if (method == MethodName.LevelDictionaryInit)
		{
			return true;
		}
		if (method == MethodName.KeyDictionaryInit)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.userCurrent)
		{
			userCurrent = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.userList)
		{
			userList = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.saveDictionary)
		{
			saveDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.userCurrent)
		{
			value = VariantUtils.CreateFrom(in userCurrent);
			return true;
		}
		if (name == PropertyName.userList)
		{
			value = VariantUtils.CreateFromArray(userList);
			return true;
		}
		if (name == PropertyName.saveDictionary)
		{
			value = VariantUtils.CreateFrom(in saveDictionary);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.userCurrent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.userList, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.saveDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.userCurrent, Variant.From(in userCurrent));
		info.AddProperty(PropertyName.userList, Variant.CreateFrom(userList));
		info.AddProperty(PropertyName.saveDictionary, Variant.From(in saveDictionary));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.userCurrent, out var value))
		{
			userCurrent = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.userList, out var value2))
		{
			userList = value2.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.saveDictionary, out var value3))
		{
			saveDictionary = value3.As<Dictionary>();
		}
	}
}
