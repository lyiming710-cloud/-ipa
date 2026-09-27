using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Save/TowerDefenseLevelSaveConfigCSharp.cs")]
public class TowerDefenseLevelSaveConfigCSharp : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Save = "Save";

		public static readonly StringName Load = "Load";

		public static readonly StringName ReadFinite = "ReadFinite";

		public static readonly StringName ReadBool = "ReadBool";

		public static readonly StringName DiscardRestoredCharacter = "DiscardRestoredCharacter";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName HasProcessSaveData = "HasProcessSaveData";

		public static readonly StringName modSnapshot = "modSnapshot";

		public static readonly StringName characterList = "characterList";

		public static readonly StringName projectileList = "projectileList";

		public static readonly StringName bulletFieldList = "bulletFieldList";

		public static readonly StringName dropItemList = "dropItemList";

		public static readonly StringName featureSave = "featureSave";

		public static readonly StringName processSave = "processSave";

		public static readonly StringName gameStateSave = "gameStateSave";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Dictionary modSnapshot;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterSaveConfigCSharp> characterList = new Array<TowerDefenseCharacterSaveConfigCSharp>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseProjectileSaveConfigCSharp> projectileList = new Array<TowerDefenseProjectileSaveConfigCSharp>();

	[Export(PropertyHint.None, "")]
	public Array<Dictionary> bulletFieldList = new Array<Dictionary>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseDropItemSaveConfigCSharp> dropItemList = new Array<TowerDefenseDropItemSaveConfigCSharp>();

	[Export(PropertyHint.None, "")]
	public Dictionary featureSave = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary processSave = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary gameStateSave = new Dictionary();

	public System.Collections.Generic.Dictionary<StringName, TowerDefenseCharacter> charcterDicionary = new System.Collections.Generic.Dictionary<StringName, TowerDefenseCharacter>();

	public ProgressRestoreReport RestoreReport { get; private set; } = new ProgressRestoreReport();

	internal bool HasProcessSaveData
	{
		get
		{
			Dictionary data;
			return TryGetSection(processSave, "main", out data);
		}
	}

	public bool CanLoad(out string reason)
	{
		reason = "";
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance?.currentControl))
		{
			reason = "当前战斗控制器不可用。";
			return false;
		}
		return true;
	}

	public void Save()
	{
		XWModEnvironmentStatus status = XWModEnvironmentService.GetStatus();
		modSnapshot = ((status.Ready && status.Snapshot != null) ? XWModEnvironmentService.WriteSnapshot(status.Snapshot) : null);
		characterList.Clear();
		foreach (Node item in Global.Instance.GetTree().GetNodesInGroup("Character"))
		{
			if (item is TowerDefenseCharacter { isShow: false, inGame: not false, die: false } towerDefenseCharacter && !towerDefenseCharacter.instance.die)
			{
				TowerDefenseCharacterSaveConfigCSharp towerDefenseCharacterSaveConfigCSharp = new TowerDefenseCharacterSaveConfigCSharp();
				towerDefenseCharacterSaveConfigCSharp.SaveCharacter(towerDefenseCharacter);
				characterList.Add(towerDefenseCharacterSaveConfigCSharp);
			}
		}
		if (projectileList == null)
		{
			projectileList = new Array<TowerDefenseProjectileSaveConfigCSharp>();
		}
		projectileList.Clear();
		if (bulletFieldList == null)
		{
			bulletFieldList = new Array<Dictionary>();
		}
		bulletFieldList.Clear();
		if (GodotObject.IsInstanceValid(BulletField.Instance))
		{
			bulletFieldList = BulletField.Instance.ExportBulletFieldSave();
		}
		dropItemList.Clear();
		foreach (Node item2 in Global.Instance.GetTree().GetNodesInGroup("SunDropItem"))
		{
			if (item2 is TowerDefenseSunBase { die: false, isCollect: false, AccountId: { IsLocal: not false } } towerDefenseSunBase)
			{
				TowerDefenseDropItemSaveConfigCSharp towerDefenseDropItemSaveConfigCSharp = new TowerDefenseDropItemSaveConfigCSharp();
				towerDefenseDropItemSaveConfigCSharp.SaveDropItem(towerDefenseSunBase);
				dropItemList.Add(towerDefenseDropItemSaveConfigCSharp);
			}
		}
		featureSave.Clear();
		processSave.Clear();
		TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
		if (GodotObject.IsInstanceValid(currentControl))
		{
			foreach (StringName key5 in currentControl.featureDictionary.Keys)
			{
				Dictionary dictionary = currentControl.featureDictionary[key5].SaveFeature();
				if (dictionary.Count > 0)
				{
					featureSave[key5] = dictionary;
				}
			}
			if (GodotObject.IsInstanceValid(currentControl.process))
			{
				Dictionary dictionary2 = currentControl.process.SaveProcess();
				if (dictionary2.Count > 0)
				{
					processSave["main"] = dictionary2;
				}
			}
		}
		gameStateSave = new Dictionary
		{
			["runGameTime"] = TowerDefenseManager.Instance.runGameTime,
			["timeScale"] = Global.Instance.timeScale,
			["pausePacket"] = TowerDefenseManager.Instance.pausePacket,
			["pauseZombie"] = TowerDefenseManager.Instance.pauseZombie
		};
	}

	public bool Load(bool resetReport = true)
	{
		if (resetReport)
		{
			RestoreReport = new ProgressRestoreReport();
		}
		if (!CanLoad(out var reason))
		{
			GD.PushWarning("[ProgressLoad] " + reason);
			return false;
		}
		TowerDefenseControlNew control = TowerDefenseManager.Instance.currentControl;
		control.featureDictionary.TryGetValue("Map", out var value);
		TowerDefenseBattleFeatureMap mapFeature = value as TowerDefenseBattleFeatureMap;
		if (GodotObject.IsInstanceValid(mapFeature) && !GodotObject.IsInstanceValid(mapFeature.currentMap))
		{
			GD.PushWarning("[ProgressLoad] 当前关卡地图未能建立。");
			return false;
		}
		Dictionary mapSaveData = null;
		if (TryGetFeatureSaveData("Map", out mapSaveData) && GodotObject.IsInstanceValid(mapFeature))
		{
			RestoreReport.Try("Map", () =>
			{
				if (!mapFeature.TryPrepareProgressLoad(mapSaveData, out var reason2))
				{
					RestoreReport.Record("Map", reason2);
				}
			});
		}
		if (characterList == null)
		{
			characterList = new Array<TowerDefenseCharacterSaveConfigCSharp>();
		}
		charcterDicionary.Clear();
		List<(TowerDefenseCharacterSaveConfigCSharp, TowerDefenseCharacter)> list = new List<(TowerDefenseCharacterSaveConfigCSharp, TowerDefenseCharacter)>();
		foreach (TowerDefenseCharacterSaveConfigCSharp saved in characterList)
		{
			TowerDefenseCharacter character = null;
			if (!RestoreReport.Try("Character", () =>
			{
				if (!GodotObject.IsInstanceValid(saved))
				{
					throw new InvalidOperationException("Empty character record.");
				}
				if (string.IsNullOrWhiteSpace(saved.nodeName) || charcterDicionary.ContainsKey(saved.nodeName))
				{
					throw new InvalidOperationException("Empty or duplicate character identity.");
				}
				saved.owner = this;
				character = saved.InstantiateCharacterForRestore();
				if (!GodotObject.IsInstanceValid(character))
				{
					throw new InvalidOperationException("Cannot create character: " + saved.packetName + ".");
				}
			}))
			{
				DiscardRestoredCharacter(character);
				continue;
			}
			charcterDicionary[saved.nodeName] = character;
			list.Add((saved, character));
		}
		foreach (var item in list)
		{
			if (!RestoreReport.Try("Character", () =>
			{
				item.Item1.RestoreCharacter(item.Item2);
			}))
			{
				charcterDicionary.Remove(item.Item1.nodeName);
				DiscardRestoredCharacter(item.Item2);
			}
		}
		if (bulletFieldList == null)
		{
			bulletFieldList = new Array<Dictionary>();
		}
		if (bulletFieldList.Count > 0 || GodotObject.IsInstanceValid(BulletField.Instance))
		{
			RestoreReport.Try("Projectile", () =>
			{
				BulletField bulletField = BulletField.EnsureMountedOnCharacterNode();
				if (GodotObject.IsInstanceValid(bulletField))
				{
					bulletField.ImportBulletFieldSave(bulletFieldList, this);
				}
				else
				{
					RestoreReport.Record("Projectile", "Bullet field unavailable.", bulletFieldList.Count);
				}
			});
		}
		RestoreReport.Record("Projectile", "Skipped unsupported Node projectile records.", projectileList?.Count ?? 0);
		if (dropItemList == null)
		{
			dropItemList = new Array<TowerDefenseDropItemSaveConfigCSharp>();
		}
		foreach (TowerDefenseDropItemSaveConfigCSharp drop in dropItemList)
		{
			RestoreReport.Try("DropItem", () =>
			{
				if (!GodotObject.IsInstanceValid(drop) || !drop.LoadDropItem(this))
				{
					RestoreReport.Record("DropItem", "Skipped unavailable drop item.");
				}
			});
		}
		if (mapSaveData != null && GodotObject.IsInstanceValid(mapFeature))
		{
			RestoreReport.Try("Map", () =>
			{
				mapFeature.LoadFeature(mapSaveData, this);
			});
		}
		if (featureSave == null)
		{
			featureSave = new Dictionary();
		}
		foreach (KeyValuePair<Variant, Variant> item2 in featureSave)
		{
			Variant.Type variantType = item2.Key.VariantType;
			if ((variantType != Variant.Type.String && variantType != Variant.Type.StringName) || 1 == 0)
			{
				RestoreReport.Record("Feature", "Invalid Feature name.");
				continue;
			}
			string name = item2.Key.AsString();
			if (name == "Map")
			{
				continue;
			}
			RestoreReport.Try("Feature", () =>
			{
				if (!TryGetFeatureSaveData(name, out var data2))
				{
					throw new InvalidOperationException("Invalid Feature record: " + name + ".");
				}
				if (!control.featureDictionary.TryGetValue(name, out var value2) || !GodotObject.IsInstanceValid(value2))
				{
					RestoreReport.Record("Feature", "Unavailable Feature: " + name + ".");
				}
				else
				{
					value2.LoadFeature(data2, this);
				}
			});
		}
		foreach (KeyValuePair<StringName, TowerDefenseBattleFeature> pair in control.featureDictionary)
		{
			if (!GodotObject.IsInstanceValid(pair.Value) || TryGetFeatureSaveData(pair.Key.ToString(), out var _))
			{
				continue;
			}
			RestoreReport.Try("Feature", () =>
			{
				if (pair.Value.CanLoadProgress())
				{
					Dictionary dictionary2 = pair.Value.SaveFeature();
					if (dictionary2 != null && dictionary2.Count > 0)
					{
						RestoreReport.Record("Feature", $"Using initial state for {pair.Key}.");
					}
				}
			});
		}
		if (GodotObject.IsInstanceValid(control.process))
		{
			if (TryGetSection(processSave, "main", out var processData))
			{
				RestoreReport.Try("Process", () =>
				{
					control.process.LoadProcess(processData, this);
				});
			}
			else
			{
				RestoreReport.Try("Process", () =>
				{
					if (control.process.CanLoadProgress())
					{
						Dictionary dictionary2 = control.process.SaveProcess();
						if (dictionary2 != null && dictionary2.Count > 0)
						{
							RestoreReport.Record("Process", "Using initial Process state.");
						}
					}
				});
			}
		}
		else
		{
			Dictionary dictionary = processSave;
			if (dictionary != null && dictionary.Count > 0)
			{
				RestoreReport.Record("Process", "Saved Process is unavailable.");
			}
		}
		TowerDefenseManager.Instance.runGameTime = ReadFinite(gameStateSave, "runGameTime", 0.0);
		Global.Instance.timeScale = ReadFinite(gameStateSave, "timeScale", 1.0);
		TowerDefenseManager.Instance.pausePacket = ReadBool(gameStateSave, "pausePacket", fallback: false);
		TowerDefenseManager.Instance.pauseZombie = ReadBool(gameStateSave, "pauseZombie", fallback: false);
		return true;
	}

	internal bool TryGetFeatureSaveData(string name, out Dictionary data)
	{
		return TryGetSection(featureSave, name, out data);
	}

	internal static bool TryGetSection(Dictionary container, string name, out Dictionary data)
	{
		data = null;
		if (container == null)
		{
			return false;
		}
		bool flag;
		foreach (KeyValuePair<Variant, Variant> item in container)
		{
			Variant.Type variantType = item.Key.VariantType;
			flag = ((variantType == Variant.Type.String || variantType == Variant.Type.StringName) ? true : false);
			if (!flag || item.Key.AsString() != name)
			{
				continue;
			}
			if (item.Value.VariantType != Variant.Type.Dictionary)
			{
				flag = false;
			}
			else
			{
				data = item.Value.AsGodotDictionary();
				flag = data != null;
			}
			goto IL_009e;
		}
		return false;
		IL_009e:
		return flag;
	}

	internal double ReadFinite(Dictionary data, string key, double fallback, string category = "State")
	{
		if (data == null || !data.TryGetValue(key, out var value))
		{
			return fallback;
		}
		Variant.Type variantType = value.VariantType;
		if (((ulong)(variantType - 2) <= 1uL) ? true : false)
		{
			double num = value.AsDouble();
			if (double.IsFinite(num))
			{
				return num;
			}
		}
		RestoreReport.Record(category, "Invalid numeric field: " + key + "; using default.");
		return fallback;
	}

	internal bool ReadBool(Dictionary data, string key, bool fallback, string category = "State")
	{
		if (data == null || !data.TryGetValue(key, out var value))
		{
			return fallback;
		}
		if (value.VariantType == Variant.Type.Bool)
		{
			return value.AsBool();
		}
		RestoreReport.Record(category, "Invalid boolean field: " + key + "; using default.");
		return fallback;
	}

	internal static void DiscardRestoredCharacter(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			character.ProcessMode = Node.ProcessModeEnum.Disabled;
			character.GetParent()?.RemoveChild(character);
			character.QueueFree();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.Save, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Load, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "resetReport", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadFinite, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadBool, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DiscardRestoredCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Save && args.Count == 0)
		{
			Save();
			ret = default;
			return true;
		}
		if (method == MethodName.Load && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Load(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadFinite && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ReadFinite(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.ReadBool && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadBool(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.DiscardRestoredCharacter && args.Count == 1)
		{
			DiscardRestoredCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.DiscardRestoredCharacter && args.Count == 1)
		{
			DiscardRestoredCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Save)
		{
			return true;
		}
		if (method == MethodName.Load)
		{
			return true;
		}
		if (method == MethodName.ReadFinite)
		{
			return true;
		}
		if (method == MethodName.ReadBool)
		{
			return true;
		}
		if (method == MethodName.DiscardRestoredCharacter)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.modSnapshot)
		{
			modSnapshot = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.characterList)
		{
			characterList = VariantUtils.ConvertToArray<TowerDefenseCharacterSaveConfigCSharp>(in value);
			return true;
		}
		if (name == PropertyName.projectileList)
		{
			projectileList = VariantUtils.ConvertToArray<TowerDefenseProjectileSaveConfigCSharp>(in value);
			return true;
		}
		if (name == PropertyName.bulletFieldList)
		{
			bulletFieldList = VariantUtils.ConvertToArray<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.dropItemList)
		{
			dropItemList = VariantUtils.ConvertToArray<TowerDefenseDropItemSaveConfigCSharp>(in value);
			return true;
		}
		if (name == PropertyName.featureSave)
		{
			featureSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.processSave)
		{
			processSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.gameStateSave)
		{
			gameStateSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.HasProcessSaveData)
		{
			value = VariantUtils.CreateFrom<bool>(HasProcessSaveData);
			return true;
		}
		if (name == PropertyName.modSnapshot)
		{
			value = VariantUtils.CreateFrom(in modSnapshot);
			return true;
		}
		if (name == PropertyName.characterList)
		{
			value = VariantUtils.CreateFromArray(characterList);
			return true;
		}
		if (name == PropertyName.projectileList)
		{
			value = VariantUtils.CreateFromArray(projectileList);
			return true;
		}
		if (name == PropertyName.bulletFieldList)
		{
			value = VariantUtils.CreateFromArray(bulletFieldList);
			return true;
		}
		if (name == PropertyName.dropItemList)
		{
			value = VariantUtils.CreateFromArray(dropItemList);
			return true;
		}
		if (name == PropertyName.featureSave)
		{
			value = VariantUtils.CreateFrom(in featureSave);
			return true;
		}
		if (name == PropertyName.processSave)
		{
			value = VariantUtils.CreateFrom(in processSave);
			return true;
		}
		if (name == PropertyName.gameStateSave)
		{
			value = VariantUtils.CreateFrom(in gameStateSave);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.modSnapshot, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.characterList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterSaveConfigCSharp", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.projectileList, PropertyHint.TypeString, "24/17:TowerDefenseProjectileSaveConfigCSharp", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.bulletFieldList, PropertyHint.TypeString, "27/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.dropItemList, PropertyHint.TypeString, "24/17:TowerDefenseDropItemSaveConfigCSharp", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.featureSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.processSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.gameStateSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasProcessSaveData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.modSnapshot, Variant.From(in modSnapshot));
		info.AddProperty(PropertyName.characterList, Variant.CreateFrom(characterList));
		info.AddProperty(PropertyName.projectileList, Variant.CreateFrom(projectileList));
		info.AddProperty(PropertyName.bulletFieldList, Variant.CreateFrom(bulletFieldList));
		info.AddProperty(PropertyName.dropItemList, Variant.CreateFrom(dropItemList));
		info.AddProperty(PropertyName.featureSave, Variant.From(in featureSave));
		info.AddProperty(PropertyName.processSave, Variant.From(in processSave));
		info.AddProperty(PropertyName.gameStateSave, Variant.From(in gameStateSave));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.modSnapshot, out var value))
		{
			modSnapshot = value.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.characterList, out var value2))
		{
			characterList = value2.AsGodotArray<TowerDefenseCharacterSaveConfigCSharp>();
		}
		if (info.TryGetProperty(PropertyName.projectileList, out var value3))
		{
			projectileList = value3.AsGodotArray<TowerDefenseProjectileSaveConfigCSharp>();
		}
		if (info.TryGetProperty(PropertyName.bulletFieldList, out var value4))
		{
			bulletFieldList = value4.AsGodotArray<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.dropItemList, out var value5))
		{
			dropItemList = value5.AsGodotArray<TowerDefenseDropItemSaveConfigCSharp>();
		}
		if (info.TryGetProperty(PropertyName.featureSave, out var value6))
		{
			featureSave = value6.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.processSave, out var value7))
		{
			processSave = value7.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.gameStateSave, out var value8))
		{
			gameStateSave = value8.As<Dictionary>();
		}
	}
}
