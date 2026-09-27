using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/General/Character/Armor/CharacterArmorData.cs")]
public class CharacterArmorData : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName _LoadTypeDataFromJSON = "_LoadTypeDataFromJSON";

		public static readonly StringName Init = "Init";

		public static readonly StringName SplitFliter = "SplitFliter";

		public static readonly StringName ClearArmorFlitersAll = "ClearArmorFlitersAll";

		public static readonly StringName ClearArmorFliters = "ClearArmorFliters";

		public static readonly StringName OpenArmorFlitersAll = "OpenArmorFlitersAll";

		public static readonly StringName OpenArmorFliters = "OpenArmorFliters";

		public static readonly StringName SetArmorReplace = "SetArmorReplace";

		public static readonly StringName CreateArmorPartNode = "CreateArmorPartNode";

		public static readonly StringName GetSlotConfig = "GetSlotConfig";

		public static readonly StringName GetOrCreateSlotConfig = "GetOrCreateSlotConfig";

		public static readonly StringName GetTypeData = "GetTypeData";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName armorList = "armorList";

		public static readonly StringName _armorList = "_armorList";

		public static readonly StringName armorDictionary = "armorDictionary";

		public static readonly StringName fliterAllDictionary = "fliterAllDictionary";

		public static readonly StringName fliterOpenDictionary = "fliterOpenDictionary";

		public static readonly StringName fliterCloseDictionary = "fliterCloseDictionary";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private static Json _registryJson;

	private Array<ArmorSlotConfig> _armorList = new Array<ArmorSlotConfig>();

	private Action _initAction;

	[Export(PropertyHint.None, "")]
	public Dictionary armorDictionary = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary fliterAllDictionary = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary fliterOpenDictionary = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary fliterCloseDictionary = new Dictionary();

	private static Json REGISTRY_JSON => _registryJson ?? (_registryJson = GD.Load<Json>("res://Registry/Armor/ArmorRegistry.json"));

	[Export(PropertyHint.None, "")]
	public Array<ArmorSlotConfig> armorList
	{
		get
		{
			return _armorList;
		}
		set
		{
			if (_initAction == null)
			{
				_initAction = Init;
			}
			foreach (ArmorSlotConfig armor in _armorList)
			{
				if (armor != null && armor.IsConnected(Resource.SignalName.Changed, Callable.From(_initAction)))
				{
					armor.Changed -= _initAction;
				}
			}
			_armorList = value;
			foreach (ArmorSlotConfig armor2 in _armorList)
			{
				if (armor2 != null && !armor2.IsConnected(Resource.SignalName.Changed, Callable.From(_initAction)))
				{
					armor2.Changed += _initAction;
				}
			}
			Init();
		}
	}

	public static TowerDefenseArmorTypeData _LoadTypeDataFromJSON(string armorName)
	{
		Dictionary dictionary = REGISTRY_JSON.Data.AsGodotDictionary().GetValueOrDefault("Armors", new Dictionary()).AsGodotDictionary();
		if (dictionary.ContainsKey(armorName))
		{
			return GD.Load<TowerDefenseArmorTypeData>(dictionary[armorName].AsString());
		}
		return null;
	}

	public void Init()
	{
		fliterAllDictionary.Clear();
		armorDictionary.Clear();
		fliterOpenDictionary.Clear();
		fliterCloseDictionary.Clear();
		foreach (ArmorSlotConfig armor in armorList)
		{
			if (armor != null)
			{
				TowerDefenseArmorTypeData towerDefenseArmorTypeData = _LoadTypeDataFromJSON(armor.armorName);
				armorDictionary[armor.armorName] = new Dictionary
				{
					["slotConfig"] = armor,
					["typeData"] = towerDefenseArmorTypeData
				};
				Array<string> array = new Array<string>();
				fliterAllDictionary[armor.armorName] = array.Duplicate();
				Array<string> array2 = SplitFliter(armor.destroyFliter);
				if (array2.Count > 0)
				{
					((Godot.Collections.Array)fliterAllDictionary[armor.armorName]).AddRange(array2);
				}
				fliterOpenDictionary[armor.armorName] = array.Duplicate();
				Array<string> array3 = SplitFliter(armor.openFliter);
				if (array3.Count > 0)
				{
					((Godot.Collections.Array)fliterOpenDictionary[armor.armorName]).AddRange(array3);
				}
				fliterCloseDictionary[armor.armorName] = array.Duplicate();
				Array<string> array4 = SplitFliter(armor.closeFliter);
				if (array4.Count > 0)
				{
					((Godot.Collections.Array)fliterCloseDictionary[armor.armorName]).AddRange(array4);
				}
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
		string[] array2 = fliter.Split('&', StringSplitOptions.RemoveEmptyEntries);
		foreach (string text in array2)
		{
			array.Add(text.Trim());
		}
		return array;
	}

	public void ClearArmorFlitersAll(AdobeAnimateSprite sprite)
	{
		foreach (Variant key in fliterAllDictionary.Keys)
		{
			string armorName = (string)key;
			ClearArmorFliters(sprite, armorName);
		}
	}

	public void ClearArmorFliters(AdobeAnimateSprite sprite, string armorName)
	{
		sprite.SetFliters((Godot.Collections.Array)fliterAllDictionary[armorName], open: false);
		sprite.SetFliters((Godot.Collections.Array)fliterCloseDictionary[armorName], open: true);
		sprite.SetFliters((Godot.Collections.Array)fliterOpenDictionary[armorName], open: false);
	}

	public void OpenArmorFlitersAll(AdobeAnimateSprite sprite)
	{
		foreach (Variant key in fliterAllDictionary.Keys)
		{
			string armorName = (string)key;
			OpenArmorFliters(sprite, armorName);
		}
	}

	public void OpenArmorFliters(AdobeAnimateSprite sprite, string armorName)
	{
		sprite.SetFliters((Godot.Collections.Array)fliterAllDictionary[armorName], open: true);
		sprite.SetFliters((Godot.Collections.Array)fliterCloseDictionary[armorName], open: false);
		sprite.SetFliters((Godot.Collections.Array)fliterOpenDictionary[armorName], open: true);
	}

	public void SetArmorReplace(AdobeAnimateSprite sprite, string armorName, int stage)
	{
		ArmorSlotConfig armorSlotConfig = (ArmorSlotConfig)(GodotObject)((Dictionary)armorDictionary[armorName])["slotConfig"];
		if (armorSlotConfig.replaceMethod != "Media" || armorSlotConfig.replaceMediaName == null || string.IsNullOrEmpty(armorSlotConfig.replaceMediaName.ToString()))
		{
			return;
		}
		TowerDefenseArmorTypeData towerDefenseArmorTypeData = (TowerDefenseArmorTypeData)(GodotObject)((Dictionary)armorDictionary[armorName])["typeData"];
		if (towerDefenseArmorTypeData != null)
		{
			Array<string> stageAnimeTexturePaths = towerDefenseArmorTypeData.stageAnimeTexturePaths;
			if (stageAnimeTexturePaths.Count > 0)
			{
				int index = Mathf.Clamp(stage, 0, stageAnimeTexturePaths.Count - 1);
				sprite.SetAtlasReplace(armorSlotConfig.replaceMediaName, stageAnimeTexturePaths[index]);
			}
		}
	}

	public static AdobeAnimatePart CreateArmorPartNode(AdobeAnimateSprite sprite, ArmorSlotConfig slotConfig, TowerDefenseArmorTypeData typeData, int stage = 0)
	{
		if (!GodotObject.IsInstanceValid(sprite) || slotConfig == null || typeData == null || typeData.stageAnimeTexturePaths == null || typeData.stageAnimeTexturePaths.Count == 0)
		{
			return null;
		}
		AdobeAnimateSlot node = sprite.GetNode<AdobeAnimateSlot>(slotConfig.slotPath);
		AdobeAnimatePart adobeAnimatePart = AdobeAnimatePart.CreateAtlasTexturePart(typeData.stageAnimeTexturePaths[Mathf.Clamp(stage, 0, typeData.stageAnimeTexturePaths.Count - 1)]);
		if (!GodotObject.IsInstanceValid(adobeAnimatePart))
		{
			return null;
		}
		adobeAnimatePart.Position = slotConfig.offset;
		adobeAnimatePart.Rotation = (float)slotConfig.rotation;
		adobeAnimatePart.Scale = slotConfig.scale;
		adobeAnimatePart.Modulate = new Color(1f, 1f, 1f, slotConfig.alphaMultiplier);
		node.AddChild(adobeAnimatePart, forceReadableName: false, Node.InternalMode.Disabled);
		return adobeAnimatePart;
	}

	public ArmorSlotConfig GetSlotConfig(string armorName)
	{
		if (armorDictionary.ContainsKey(armorName))
		{
			return (ArmorSlotConfig)(GodotObject)((Dictionary)armorDictionary[armorName])["slotConfig"];
		}
		return null;
	}

	public ArmorSlotConfig GetOrCreateSlotConfig(string armorName)
	{
		if (armorDictionary.ContainsKey(armorName))
		{
			return (ArmorSlotConfig)(GodotObject)((Dictionary)armorDictionary[armorName])["slotConfig"];
		}
		TowerDefenseArmorTypeData towerDefenseArmorTypeData = _LoadTypeDataFromJSON(armorName);
		if (towerDefenseArmorTypeData == null)
		{
			return null;
		}
		ArmorSlotConfig armorSlotConfig = new ArmorSlotConfig();
		armorSlotConfig.armorName = armorName;
		armorSlotConfig.replaceMethod = "Sprite";
		armorSlotConfig.slotPath = new NodePath("HeadSlot");
		armorSlotConfig.offset = Vector2.Zero;
		armorSlotConfig.rotation = 0.0;
		armorSlotConfig.scale = Vector2.One;
		armorSlotConfig.damagePoint = -1.0;
		armorSlotConfig.openFliter = "";
		armorSlotConfig.closeFliter = "";
		armorSlotConfig.destroyFliter = "";
		armorDictionary[armorName] = new Dictionary
		{
			["slotConfig"] = armorSlotConfig,
			["typeData"] = towerDefenseArmorTypeData
		};
		Array<string> array = new Array<string>();
		fliterAllDictionary[armorName] = array.Duplicate();
		Array<string> array2 = SplitFliter(armorSlotConfig.destroyFliter);
		if (array2.Count > 0)
		{
			((Godot.Collections.Array)fliterAllDictionary[armorName]).AddRange(array2);
		}
		fliterOpenDictionary[armorName] = array.Duplicate();
		Array<string> array3 = SplitFliter(armorSlotConfig.openFliter);
		if (array3.Count > 0)
		{
			((Godot.Collections.Array)fliterOpenDictionary[armorName]).AddRange(array3);
		}
		fliterCloseDictionary[armorName] = array.Duplicate();
		Array<string> array4 = SplitFliter(armorSlotConfig.closeFliter);
		if (array4.Count > 0)
		{
			((Godot.Collections.Array)fliterCloseDictionary[armorName]).AddRange(array4);
		}
		return armorSlotConfig;
	}

	public TowerDefenseArmorTypeData GetTypeData(string armorName)
	{
		if (armorDictionary.ContainsKey(armorName))
		{
			return (TowerDefenseArmorTypeData)(GodotObject)((Dictionary)armorDictionary[armorName])["typeData"];
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._LoadTypeDataFromJSON, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SplitFliter, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearArmorFlitersAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearArmorFliters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenArmorFlitersAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenArmorFliters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetArmorReplace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateArmorPartNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "slotConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "typeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSlotConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetOrCreateSlotConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTypeData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._LoadTypeDataFromJSON && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseArmorTypeData>(_LoadTypeDataFromJSON(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
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
		if (method == MethodName.ClearArmorFlitersAll && args.Count == 1)
		{
			ClearArmorFlitersAll(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearArmorFliters && args.Count == 2)
		{
			ClearArmorFliters(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenArmorFlitersAll && args.Count == 1)
		{
			OpenArmorFlitersAll(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenArmorFliters && args.Count == 2)
		{
			OpenArmorFliters(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetArmorReplace && args.Count == 3)
		{
			SetArmorReplace(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateArmorPartNode && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimatePart>(CreateArmorPartNode(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<ArmorSlotConfig>(in args[1]), VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		if (method == MethodName.GetSlotConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ArmorSlotConfig>(GetSlotConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetOrCreateSlotConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ArmorSlotConfig>(GetOrCreateSlotConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseArmorTypeData>(GetTypeData(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._LoadTypeDataFromJSON && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseArmorTypeData>(_LoadTypeDataFromJSON(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateArmorPartNode && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimatePart>(CreateArmorPartNode(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<ArmorSlotConfig>(in args[1]), VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._LoadTypeDataFromJSON)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.SplitFliter)
		{
			return true;
		}
		if (method == MethodName.ClearArmorFlitersAll)
		{
			return true;
		}
		if (method == MethodName.ClearArmorFliters)
		{
			return true;
		}
		if (method == MethodName.OpenArmorFlitersAll)
		{
			return true;
		}
		if (method == MethodName.OpenArmorFliters)
		{
			return true;
		}
		if (method == MethodName.SetArmorReplace)
		{
			return true;
		}
		if (method == MethodName.CreateArmorPartNode)
		{
			return true;
		}
		if (method == MethodName.GetSlotConfig)
		{
			return true;
		}
		if (method == MethodName.GetOrCreateSlotConfig)
		{
			return true;
		}
		if (method == MethodName.GetTypeData)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.armorList)
		{
			armorList = VariantUtils.ConvertToArray<ArmorSlotConfig>(in value);
			return true;
		}
		if (name == PropertyName._armorList)
		{
			_armorList = VariantUtils.ConvertToArray<ArmorSlotConfig>(in value);
			return true;
		}
		if (name == PropertyName.armorDictionary)
		{
			armorDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.fliterAllDictionary)
		{
			fliterAllDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.fliterOpenDictionary)
		{
			fliterOpenDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.fliterCloseDictionary)
		{
			fliterCloseDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.armorList)
		{
			value = VariantUtils.CreateFromArray(armorList);
			return true;
		}
		if (name == PropertyName._armorList)
		{
			value = VariantUtils.CreateFromArray(_armorList);
			return true;
		}
		if (name == PropertyName.armorDictionary)
		{
			value = VariantUtils.CreateFrom(in armorDictionary);
			return true;
		}
		if (name == PropertyName.fliterAllDictionary)
		{
			value = VariantUtils.CreateFrom(in fliterAllDictionary);
			return true;
		}
		if (name == PropertyName.fliterOpenDictionary)
		{
			value = VariantUtils.CreateFrom(in fliterOpenDictionary);
			return true;
		}
		if (name == PropertyName.fliterCloseDictionary)
		{
			value = VariantUtils.CreateFrom(in fliterCloseDictionary);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._armorList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.armorList, PropertyHint.TypeString, "24/17:ArmorSlotConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.armorDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.fliterAllDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.fliterOpenDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.fliterCloseDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.armorList, Variant.CreateFrom(armorList));
		info.AddProperty(PropertyName._armorList, Variant.CreateFrom(_armorList));
		info.AddProperty(PropertyName.armorDictionary, Variant.From(in armorDictionary));
		info.AddProperty(PropertyName.fliterAllDictionary, Variant.From(in fliterAllDictionary));
		info.AddProperty(PropertyName.fliterOpenDictionary, Variant.From(in fliterOpenDictionary));
		info.AddProperty(PropertyName.fliterCloseDictionary, Variant.From(in fliterCloseDictionary));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.armorList, out var value))
		{
			armorList = value.AsGodotArray<ArmorSlotConfig>();
		}
		if (info.TryGetProperty(PropertyName._armorList, out var value2))
		{
			_armorList = value2.AsGodotArray<ArmorSlotConfig>();
		}
		if (info.TryGetProperty(PropertyName.armorDictionary, out var value3))
		{
			armorDictionary = value3.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.fliterAllDictionary, out var value4))
		{
			fliterAllDictionary = value4.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.fliterOpenDictionary, out var value5))
		{
			fliterOpenDictionary = value5.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.fliterCloseDictionary, out var value6))
		{
			fliterCloseDictionary = value6.As<Dictionary>();
		}
	}
}
