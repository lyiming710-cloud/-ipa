using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/General/Character/Costom/CharacterCustomData.cs")]
public class CharacterCustomData : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName SplitFliter = "SplitFliter";

		public static readonly StringName ClearCustomFliters = "ClearCustomFliters";

		public static readonly StringName SetCustomFliters = "SetCustomFliters";

		public static readonly StringName SetDamagePoint = "SetDamagePoint";

		public static readonly StringName ClearDamagePoint = "ClearDamagePoint";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName customList = "customList";

		public static readonly StringName _customList = "_customList";

		public static readonly StringName customDictionary = "customDictionary";

		public static readonly StringName fliterOpenAll = "fliterOpenAll";

		public static readonly StringName fliterCloseAll = "fliterCloseAll";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private Array<CharacterCustomConfig> _customList = new Array<CharacterCustomConfig>();

	private Action _initAction;

	[Export(PropertyHint.None, "")]
	public Dictionary customDictionary = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Array<string> fliterOpenAll = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<string> fliterCloseAll = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<CharacterCustomConfig> customList
	{
		get
		{
			return _customList;
		}
		set
		{
			if (_initAction == null)
			{
				_initAction = Init;
			}
			foreach (CharacterCustomConfig custom in _customList)
			{
				if (custom != null && custom.IsConnected(Resource.SignalName.Changed, Callable.From(_initAction)))
				{
					custom.Changed -= _initAction;
				}
			}
			_customList = value;
			foreach (CharacterCustomConfig custom2 in _customList)
			{
				if (custom2 != null && !custom2.IsConnected(Resource.SignalName.Changed, Callable.From(_initAction)))
				{
					custom2.Changed += _initAction;
				}
			}
			Init();
			NotifyPropertyListChanged();
		}
	}

	public void Init()
	{
		fliterOpenAll.Clear();
		fliterCloseAll.Clear();
		customDictionary.Clear();
		foreach (CharacterCustomConfig custom in customList)
		{
			if (custom == null)
			{
				continue;
			}
			Array<string> array = SplitFliter(custom.animeFliterOpen);
			Array<string> array2 = SplitFliter(custom.animeFliterClose);
			customDictionary[custom.customName] = new Dictionary
			{
				["Config"] = custom,
				["Open"] = array,
				["Close"] = array2
			};
			if (array.Count > 0)
			{
				foreach (string item in array)
				{
					fliterOpenAll.Add(item);
				}
			}
			if (array2.Count <= 0)
			{
				continue;
			}
			foreach (string item2 in array2)
			{
				fliterCloseAll.Add(item2);
			}
		}
	}

	private Array<string> SplitFliter(string fliter)
	{
		Array<string> array = new Array<string>();
		if (string.IsNullOrEmpty(fliter))
		{
			return array;
		}
		string[] array2 = fliter.Split("&", StringSplitOptions.RemoveEmptyEntries);
		foreach (string item in array2)
		{
			array.Add(item);
		}
		return array;
	}

	public void ClearCustomFliters(AdobeAnimateSprite sprite)
	{
		sprite.SetFlitersRecursive((Godot.Collections.Array?)fliterOpenAll, open: false);
		sprite.SetFlitersRecursive((Godot.Collections.Array?)fliterCloseAll, open: true);
	}

	public void SetCustomFliters(AdobeAnimateSprite sprite, string customName)
	{
		if (customDictionary.ContainsKey(customName))
		{
			Dictionary dictionary = (Dictionary)customDictionary[customName];
			if (dictionary.ContainsKey("Open") && dictionary.ContainsKey("Close"))
			{
				Array<string> array = (Array<string>)dictionary["Open"];
				Array<string> array2 = (Array<string>)dictionary["Close"];
				sprite.SetFlitersRecursive((Godot.Collections.Array?)array, open: true);
				sprite.SetFlitersRecursive((Godot.Collections.Array?)array2, open: false);
			}
		}
	}

	public void SetDamagePoint(AdobeAnimateSprite sprite, string customName, int index)
	{
		if (!customDictionary.ContainsKey(customName))
		{
			return;
		}
		Dictionary dictionary = (Dictionary)customDictionary[customName];
		if (dictionary.ContainsKey("Config"))
		{
			CharacterCustomConfig characterCustomConfig = (CharacterCustomConfig)(GodotObject)dictionary["Config"];
			if (index >= 0 && index < characterCustomConfig.damagePointChangeMediaTexturePaths.Count)
			{
				sprite.SetAtlasReplace(characterCustomConfig.damagePointChangeMediaName, characterCustomConfig.damagePointChangeMediaTexturePaths[index]);
			}
		}
	}

	public void ClearDamagePoint(AdobeAnimateSprite sprite, string customName)
	{
		if (!customDictionary.ContainsKey(customName))
		{
			return;
		}
		Dictionary dictionary = (Dictionary)customDictionary[customName];
		if (dictionary.ContainsKey("Config"))
		{
			CharacterCustomConfig characterCustomConfig = (CharacterCustomConfig)(GodotObject)dictionary["Config"];
			if (characterCustomConfig.damagePointChangeMediaName != "")
			{
				sprite.SetAtlasReplace(characterCustomConfig.damagePointChangeMediaName, string.Empty);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SplitFliter, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearCustomFliters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetCustomFliters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "customName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetDamagePoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "customName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearDamagePoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "customName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.SplitFliter && args.Count == 1)
		{
			Array<string> array = SplitFliter(VariantUtils.ConvertTo<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.ClearCustomFliters && args.Count == 1)
		{
			ClearCustomFliters(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCustomFliters && args.Count == 2)
		{
			SetCustomFliters(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDamagePoint && args.Count == 3)
		{
			SetDamagePoint(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearDamagePoint && args.Count == 2)
		{
			ClearDamagePoint(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.SplitFliter)
		{
			return true;
		}
		if (method == MethodName.ClearCustomFliters)
		{
			return true;
		}
		if (method == MethodName.SetCustomFliters)
		{
			return true;
		}
		if (method == MethodName.SetDamagePoint)
		{
			return true;
		}
		if (method == MethodName.ClearDamagePoint)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.customList)
		{
			customList = VariantUtils.ConvertToArray<CharacterCustomConfig>(in value);
			return true;
		}
		if (name == PropertyName._customList)
		{
			_customList = VariantUtils.ConvertToArray<CharacterCustomConfig>(in value);
			return true;
		}
		if (name == PropertyName.customDictionary)
		{
			customDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.fliterOpenAll)
		{
			fliterOpenAll = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.fliterCloseAll)
		{
			fliterCloseAll = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.customList)
		{
			value = VariantUtils.CreateFromArray(customList);
			return true;
		}
		if (name == PropertyName._customList)
		{
			value = VariantUtils.CreateFromArray(_customList);
			return true;
		}
		if (name == PropertyName.customDictionary)
		{
			value = VariantUtils.CreateFrom(in customDictionary);
			return true;
		}
		if (name == PropertyName.fliterOpenAll)
		{
			value = VariantUtils.CreateFromArray(fliterOpenAll);
			return true;
		}
		if (name == PropertyName.fliterCloseAll)
		{
			value = VariantUtils.CreateFromArray(fliterCloseAll);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._customList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.customList, PropertyHint.TypeString, "24/17:CharacterCustomConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.customDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.fliterOpenAll, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.fliterCloseAll, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.customList, Variant.CreateFrom(customList));
		info.AddProperty(PropertyName._customList, Variant.CreateFrom(_customList));
		info.AddProperty(PropertyName.customDictionary, Variant.From(in customDictionary));
		info.AddProperty(PropertyName.fliterOpenAll, Variant.CreateFrom(fliterOpenAll));
		info.AddProperty(PropertyName.fliterCloseAll, Variant.CreateFrom(fliterCloseAll));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.customList, out var value))
		{
			customList = value.AsGodotArray<CharacterCustomConfig>();
		}
		if (info.TryGetProperty(PropertyName._customList, out var value2))
		{
			_customList = value2.AsGodotArray<CharacterCustomConfig>();
		}
		if (info.TryGetProperty(PropertyName.customDictionary, out var value3))
		{
			customDictionary = value3.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.fliterOpenAll, out var value4))
		{
			fliterOpenAll = value4.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.fliterCloseAll, out var value5))
		{
			fliterCloseAll = value5.AsGodotArray<string>();
		}
	}
}
