using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Projectile/TowerDefenseProjectileRegistry.cs")]
public class TowerDefenseProjectileRegistry : RefCounted
{
	internal readonly record struct ProjectileRegistrationSnapshot(bool HasData, TowerDefenseProjectileData Data, bool HasSkin, TowerDefenseProjectileSkinData Skin);

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName RegisterInit = "RegisterInit";

		public static readonly StringName RegisterProjectile = "RegisterProjectile";

		public static readonly StringName RegisterProjectileOverlay = "RegisterProjectileOverlay";

		public static readonly StringName NotifyProjectileConfigChanged = "NotifyProjectileConfigChanged";

		public static readonly StringName RemoveProjectileRegistration = "RemoveProjectileRegistration";

		public static readonly StringName RegisterProjectileSkin = "RegisterProjectileSkin";

		public static readonly StringName RegisterProjectileChange = "RegisterProjectileChange";

		public static readonly StringName GetProjectile = "GetProjectile";

		public static readonly StringName HasProjectileSkin = "HasProjectileSkin";

		public static readonly StringName GetProjectileSkinProjectileScene = "GetProjectileSkinProjectileScene";

		public static readonly StringName GetProjectileSkinSplatScene = "GetProjectileSkinSplatScene";

		public static readonly StringName GetProjectileChange = "GetProjectileChange";

		public static readonly StringName HasProjectileChange = "HasProjectileChange";

		public static readonly StringName IsStarProjectile = "IsStarProjectile";

		public static readonly StringName HasChangeTarget = "HasChangeTarget";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private static Json _registryJson;

	public static bool IsInit = false;

	public static System.Collections.Generic.Dictionary<StringName, TowerDefenseProjectileData> ProjectileDictionary = new System.Collections.Generic.Dictionary<StringName, TowerDefenseProjectileData>();

	public static System.Collections.Generic.Dictionary<StringName, TowerDefenseProjectileSkinData> ProjectileSkinDictionary = new System.Collections.Generic.Dictionary<StringName, TowerDefenseProjectileSkinData>();

	public static System.Collections.Generic.Dictionary<StringName, TowerDefenseProjectileChangeData> ProjectileChangeDataDictionary = new System.Collections.Generic.Dictionary<StringName, TowerDefenseProjectileChangeData>();

	private static long _registrationRevision;

	public static Json REGISTRY_JSON => _registryJson ?? (_registryJson = GD.Load<Json>("res://Registry/Projectile/ProjectileRegistry.json"));

	internal static long RegistrationRevision => Interlocked.Read(in _registrationRevision);

	public static void Init()
	{
		if (!IsInit)
		{
			IsInit = true;
			RegisterInit();
		}
	}

	public static void RegisterInit()
	{
		Dictionary dictionary = (Dictionary)REGISTRY_JSON.Data;
		Dictionary dictionary2 = (Dictionary)dictionary.GetValueOrDefault("Projectiles", new Dictionary());
		foreach (Variant key in dictionary2.Keys)
		{
			string text = (string)key;
			TowerDefenseProjectileData towerDefenseProjectileData = SafeLoad<TowerDefenseProjectileData>((string)dictionary2[text], "Projectile:" + text);
			if (towerDefenseProjectileData != null)
			{
				RegisterProjectile(text, towerDefenseProjectileData);
			}
		}
		Dictionary dictionary3 = (Dictionary)dictionary.GetValueOrDefault("Skins", new Dictionary());
		foreach (Variant key2 in dictionary3.Keys)
		{
			string text2 = (string)key2;
			Dictionary dictionary4 = (Dictionary)dictionary3[text2];
			foreach (Variant key3 in dictionary4.Keys)
			{
				string text3 = (string)key3;
				Dictionary dictionary5 = (Dictionary)dictionary4[text3];
				PackedScene skinProjectileScene = null;
				PackedScene skinSplatScene = null;
				if (dictionary5.ContainsKey("SplatScene"))
				{
					skinSplatScene = SafeLoad<PackedScene>((string)dictionary5["SplatScene"], "Splat:" + text2);
				}
				if (dictionary5.ContainsKey("ProjectileScene"))
				{
					skinProjectileScene = SafeLoad<PackedScene>((string)dictionary5["ProjectileScene"], "Skin:" + text2 + "/" + text3);
				}
				RegisterProjectileSkin(text2, text3, skinProjectileScene, skinSplatScene);
			}
		}
		Dictionary dictionary6 = (Dictionary)dictionary.GetValueOrDefault("Changes", new Dictionary());
		foreach (Variant key4 in dictionary6.Keys)
		{
			string text4 = (string)key4;
			Dictionary dictionary7 = (Dictionary)dictionary6[text4];
			foreach (Variant key5 in dictionary7.Keys)
			{
				string text5 = (string)key5;
				RegisterProjectileChange(text4, text5, (string)dictionary7[text5]);
			}
		}
	}

	public static void RegisterProjectile(StringName projectileName, TowerDefenseProjectileData projectileData)
	{
		ProjectileDictionary[projectileName] = projectileData;
		ProjectileSkinDictionary[projectileName] = new TowerDefenseProjectileSkinData();
		RegisterProjectileSkin(projectileName, "Default", projectileData.projectileScene, projectileData.splatScene);
	}

	internal static void RegisterProjectileOverlay(StringName projectileName, TowerDefenseProjectileData projectileData)
	{
		ProjectileSkinDictionary.TryGetValue(projectileName, out var value);
		RegisterProjectile(projectileName, projectileData);
		if (value == null)
		{
			return;
		}
		foreach (StringName skin in value.SkinList)
		{
			if (!(skin.ToString() == "Default") && !HasProjectileSkin(projectileName, skin))
			{
				RegisterProjectileSkin(projectileName, skin, value.GetSkinProjectileScene(skin), value.GetSkinSplatScene(skin));
			}
		}
	}

	internal static void NotifyProjectileConfigChanged()
	{
		Interlocked.Increment(ref _registrationRevision);
	}

	internal static ProjectileRegistrationSnapshot CaptureProjectileRegistration(StringName projectileName)
	{
		TowerDefenseProjectileData value;
		TowerDefenseProjectileSkinData value2;
		return new ProjectileRegistrationSnapshot(ProjectileDictionary.TryGetValue(projectileName, out value), HasSkin: ProjectileSkinDictionary.TryGetValue(projectileName, out value2), Data: value, Skin: value2);
	}

	internal static void RestoreProjectileRegistration(StringName projectileName, ProjectileRegistrationSnapshot snapshot)
	{
		if (snapshot.HasData)
		{
			ProjectileDictionary[projectileName] = snapshot.Data;
		}
		else
		{
			ProjectileDictionary.Remove(projectileName);
		}
		if (snapshot.HasSkin)
		{
			ProjectileSkinDictionary[projectileName] = snapshot.Skin;
		}
		else
		{
			ProjectileSkinDictionary.Remove(projectileName);
		}
		Interlocked.Increment(ref _registrationRevision);
	}

	internal static bool RemoveProjectileRegistration(StringName projectileName)
	{
		bool num = ProjectileDictionary.Remove(projectileName) | ProjectileSkinDictionary.Remove(projectileName);
		if (num)
		{
			Interlocked.Increment(ref _registrationRevision);
		}
		return num;
	}

	public static void RegisterProjectileSkin(StringName projectileName, StringName skinName, PackedScene skinProjectileScene, PackedScene skinSplatScene)
	{
		if (ProjectileSkinDictionary.ContainsKey(projectileName))
		{
			ProjectileSkinDictionary[projectileName].AddSkin(skinName, skinProjectileScene, skinSplatScene);
			Interlocked.Increment(ref _registrationRevision);
		}
	}

	public static void RegisterProjectileChange(StringName changeName, StringName projectileName, StringName toProjectileName)
	{
		if (!ProjectileChangeDataDictionary.ContainsKey(changeName))
		{
			ProjectileChangeDataDictionary[changeName] = new TowerDefenseProjectileChangeData();
		}
		ProjectileChangeDataDictionary[changeName].AddChange(projectileName, toProjectileName);
	}

	private static T SafeLoad<T>(string path, string entry) where T : Resource
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return null;
		}
		string text = path;
		if (path.StartsWith("uid://", StringComparison.OrdinalIgnoreCase))
		{
			long num = ResourceUid.TextToId(path);
			if (num == -1 || !ResourceUid.HasId(num))
			{
				GD.PushWarning($"Projectile registry skipped unresolved UID {path} ({entry}).");
				return null;
			}
			text = ResourceUid.GetIdPath(num);
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
		}
		try
		{
			return ResourceLoader.Load(text, "", ResourceLoader.CacheMode.Reuse) as T;
		}
		catch (Exception ex)
		{
			GD.PushWarning($"Projectile registry skipped {text} ({entry}): {ex.Message}");
			return null;
		}
	}

	public static TowerDefenseProjectileData GetProjectile(StringName projectileName)
	{
		if (projectileName == null || !ProjectileDictionary.ContainsKey(projectileName))
		{
			return null;
		}
		return (TowerDefenseProjectileData)ProjectileDictionary[projectileName].Duplicate();
	}

	public static bool HasProjectileSkin(StringName projectileName, StringName skinName)
	{
		if (!ProjectileSkinDictionary.ContainsKey(projectileName))
		{
			return false;
		}
		return ProjectileSkinDictionary[projectileName].HasSkin(skinName);
	}

	public static PackedScene GetProjectileSkinProjectileScene(StringName projectileName, StringName skinName)
	{
		return ProjectileSkinDictionary[projectileName].GetSkinProjectileScene(skinName);
	}

	public static PackedScene GetProjectileSkinSplatScene(StringName projectileName, StringName skinName)
	{
		return ProjectileSkinDictionary[projectileName].GetSkinSplatScene(skinName);
	}

	public static StringName GetProjectileChange(StringName projectileName, StringName changeName)
	{
		if (!ProjectileChangeDataDictionary.ContainsKey(changeName))
		{
			return null;
		}
		return ProjectileChangeDataDictionary[changeName].GetChange(projectileName);
	}

	public static bool HasProjectileChange(StringName changeName)
	{
		return ProjectileChangeDataDictionary.ContainsKey(changeName);
	}

	public static bool IsStarProjectile(StringName projectileName, TowerDefenseProjectileConfig runtimeConfig = null)
	{
		if (runtimeConfig == null || !runtimeConfig.isStar)
		{
			if (projectileName != null && ProjectileDictionary.TryGetValue(projectileName, out var value) && value != null)
			{
				return value.isStar;
			}
			return false;
		}
		return true;
	}

	public static bool HasChangeTarget(StringName projectileName, StringName changeName)
	{
		if (!ProjectileChangeDataDictionary.ContainsKey(changeName))
		{
			return false;
		}
		return ProjectileChangeDataDictionary[changeName].HasChangeTarget(projectileName);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterProjectile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterProjectileOverlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyProjectileConfigChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RemoveProjectileRegistration, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterProjectileSkin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "skinName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "skinProjectileScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Object, "skinSplatScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterProjectileChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "changeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "toProjectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasProjectileSkin, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "skinName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectileSkinProjectileScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "skinName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectileSkinSplatScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "skinName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectileChange, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "changeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasProjectileChange, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "changeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsStarProjectile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "runtimeConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasChangeTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "changeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterProjectile && args.Count == 2)
		{
			RegisterProjectile(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterProjectileOverlay && args.Count == 2)
		{
			RegisterProjectileOverlay(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyProjectileConfigChanged && args.Count == 0)
		{
			NotifyProjectileConfigChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveProjectileRegistration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveProjectileRegistration(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterProjectileSkin && args.Count == 4)
		{
			RegisterProjectileSkin(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<PackedScene>(in args[2]), VariantUtils.ConvertTo<PackedScene>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterProjectileChange && args.Count == 3)
		{
			RegisterProjectileChange(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileData>(GetProjectile(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.HasProjectileSkin && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProjectileSkin(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.GetProjectileSkinProjectileScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetProjectileSkinProjectileScene(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.GetProjectileSkinSplatScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetProjectileSkinSplatScene(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.GetProjectileChange && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StringName>(GetProjectileChange(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.HasProjectileChange && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProjectileChange(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.IsStarProjectile && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsStarProjectile(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.HasChangeTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasChangeTarget(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterProjectile && args.Count == 2)
		{
			RegisterProjectile(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterProjectileOverlay && args.Count == 2)
		{
			RegisterProjectileOverlay(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyProjectileConfigChanged && args.Count == 0)
		{
			NotifyProjectileConfigChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveProjectileRegistration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveProjectileRegistration(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterProjectileSkin && args.Count == 4)
		{
			RegisterProjectileSkin(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<PackedScene>(in args[2]), VariantUtils.ConvertTo<PackedScene>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterProjectileChange && args.Count == 3)
		{
			RegisterProjectileChange(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileData>(GetProjectile(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.HasProjectileSkin && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProjectileSkin(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.GetProjectileSkinProjectileScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetProjectileSkinProjectileScene(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.GetProjectileSkinSplatScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetProjectileSkinSplatScene(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.GetProjectileChange && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StringName>(GetProjectileChange(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.HasProjectileChange && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProjectileChange(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.IsStarProjectile && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsStarProjectile(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.HasChangeTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasChangeTarget(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.RegisterInit)
		{
			return true;
		}
		if (method == MethodName.RegisterProjectile)
		{
			return true;
		}
		if (method == MethodName.RegisterProjectileOverlay)
		{
			return true;
		}
		if (method == MethodName.NotifyProjectileConfigChanged)
		{
			return true;
		}
		if (method == MethodName.RemoveProjectileRegistration)
		{
			return true;
		}
		if (method == MethodName.RegisterProjectileSkin)
		{
			return true;
		}
		if (method == MethodName.RegisterProjectileChange)
		{
			return true;
		}
		if (method == MethodName.GetProjectile)
		{
			return true;
		}
		if (method == MethodName.HasProjectileSkin)
		{
			return true;
		}
		if (method == MethodName.GetProjectileSkinProjectileScene)
		{
			return true;
		}
		if (method == MethodName.GetProjectileSkinSplatScene)
		{
			return true;
		}
		if (method == MethodName.GetProjectileChange)
		{
			return true;
		}
		if (method == MethodName.HasProjectileChange)
		{
			return true;
		}
		if (method == MethodName.IsStarProjectile)
		{
			return true;
		}
		if (method == MethodName.HasChangeTarget)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
