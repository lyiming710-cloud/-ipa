using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://addons/AdobeAnimateEditor/Runtime/AdobeAnimateAtlasProfile.cs")]
public class AdobeAnimateAtlasProfile : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName GetStableCacheKey = "GetStableCacheKey";

		public static readonly StringName LoadManifest = "LoadManifest";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName ProfileId = "ProfileId";

		public static readonly StringName ManifestPath = "ManifestPath";

		public static readonly StringName StartupOnly = "StartupOnly";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string ProfileId { get; set; } = "";

	[Export(PropertyHint.File, "*.tres")]
	public string ManifestPath { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public bool StartupOnly { get; set; }

	public string GetStableCacheKey()
	{
		if (!string.IsNullOrWhiteSpace(ProfileId))
		{
			return ProfileId.Trim();
		}
		if (!string.IsNullOrWhiteSpace(ResourcePath))
		{
			return ResourcePath;
		}
		return ManifestPath ?? string.Empty;
	}

	public AdobeAnimateGlobalAtlasManifest LoadManifest()
	{
		if (string.IsNullOrWhiteSpace(ManifestPath))
		{
			return null;
		}
		return ResourceLoader.Load<AdobeAnimateGlobalAtlasManifest>(ManifestPath, null, ResourceLoader.CacheMode.Reuse);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.GetStableCacheKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadManifest, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetStableCacheKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetStableCacheKey());
			return true;
		}
		if (method == MethodName.LoadManifest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateGlobalAtlasManifest>(LoadManifest());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetStableCacheKey)
		{
			return true;
		}
		if (method == MethodName.LoadManifest)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ProfileId)
		{
			ProfileId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ManifestPath)
		{
			ManifestPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.StartupOnly)
		{
			StartupOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.ProfileId)
		{
			from = ProfileId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ManifestPath)
		{
			from = ManifestPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.StartupOnly)
		{
			value = VariantUtils.CreateFrom<bool>(StartupOnly);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ProfileId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ManifestPath, PropertyHint.File, "*.tres", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.StartupOnly, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ProfileId, Variant.From<string>(ProfileId));
		info.AddProperty(PropertyName.ManifestPath, Variant.From<string>(ManifestPath));
		info.AddProperty(PropertyName.StartupOnly, Variant.From<bool>(StartupOnly));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ProfileId, out var value))
		{
			ProfileId = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ManifestPath, out var value2))
		{
			ManifestPath = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.StartupOnly, out var value3))
		{
			StartupOnly = value3.As<bool>();
		}
	}
}
