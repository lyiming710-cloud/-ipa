using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Projectile/Data/TowerDefenseProjectileSkinData.cs")]
public class TowerDefenseProjectileSkinData : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName AddSkin = "AddSkin";

		public static readonly StringName GetSkinProjectileScene = "GetSkinProjectileScene";

		public static readonly StringName GetSkinSplatScene = "GetSkinSplatScene";

		public static readonly StringName HasSkin = "HasSkin";

		public static readonly StringName IsEmpty = "IsEmpty";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public List<StringName> SkinList = new List<StringName>();

	public Dictionary<StringName, PackedScene> SkinProjectileSceneDictionary = new Dictionary<StringName, PackedScene>();

	public Dictionary<StringName, PackedScene> SkinSplatSceneDictionary = new Dictionary<StringName, PackedScene>();

	public void AddSkin(StringName skinName, PackedScene projectileScene, PackedScene splatScene)
	{
		SkinProjectileSceneDictionary[skinName] = projectileScene;
		SkinSplatSceneDictionary[skinName] = splatScene;
		SkinList.Add(skinName);
	}

	public PackedScene GetSkinProjectileScene(StringName skinName)
	{
		if (!HasSkin(skinName))
		{
			return null;
		}
		return SkinProjectileSceneDictionary[skinName];
	}

	public PackedScene GetSkinSplatScene(StringName skinName)
	{
		if (!HasSkin(skinName))
		{
			return null;
		}
		return SkinSplatSceneDictionary[skinName];
	}

	public bool HasSkin(StringName skinName)
	{
		return SkinList.Contains(skinName);
	}

	public bool IsEmpty()
	{
		return SkinList.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.AddSkin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "skinName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Object, "splatScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetSkinProjectileScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "skinName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSkinSplatScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "skinName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasSkin, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "skinName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AddSkin && args.Count == 3)
		{
			AddSkin(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1]), VariantUtils.ConvertTo<PackedScene>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSkinProjectileScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetSkinProjectileScene(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSkinSplatScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetSkinSplatScene(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.HasSkin && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSkin(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.IsEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEmpty());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.AddSkin)
		{
			return true;
		}
		if (method == MethodName.GetSkinProjectileScene)
		{
			return true;
		}
		if (method == MethodName.GetSkinSplatScene)
		{
			return true;
		}
		if (method == MethodName.HasSkin)
		{
			return true;
		}
		if (method == MethodName.IsEmpty)
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
