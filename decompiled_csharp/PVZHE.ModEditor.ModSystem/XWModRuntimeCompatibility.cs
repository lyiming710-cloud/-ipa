using System;

namespace PVZHE.ModEditor.ModSystem;

public static class XWModRuntimeCompatibility
{
	public static bool HasManagedCode(XWModManifest manifest)
	{
		if (manifest != null)
		{
			if (string.IsNullOrWhiteSpace(manifest.RuntimeAssembly))
			{
				return !string.IsNullOrWhiteSpace(manifest.RuntimeEntryType);
			}
			return true;
		}
		return false;
	}

	public static bool ValidatePackage(XWModManifest manifest, out string diagnostic)
	{
		diagnostic = "";
		if (!HasManagedCode(manifest))
		{
			return true;
		}
		string text;
		if (manifest.SchemaVersion != 2)
		{
			text = $"schemaVersion={manifest.SchemaVersion}, expected={2}";
		}
		else if (manifest.RuntimeApiVersion != 1)
		{
			text = $"runtimeApiVersion={manifest.RuntimeApiVersion}, expected={1}";
		}
		else if (string.IsNullOrWhiteSpace(manifest.RuntimeAssembly))
		{
			text = "runtimeEntryType requires runtimeAssembly";
		}
		else
		{
			text = ((!string.Equals(manifest.RuntimeAssemblyPolicy?.Trim(), "required", StringComparison.OrdinalIgnoreCase) && !string.Equals(manifest.RuntimeAssemblyPolicy?.Trim(), "optional", StringComparison.OrdinalIgnoreCase)) ? ("runtimeAssemblyPolicy='" + manifest.RuntimeAssemblyPolicy + "', expected=required|optional") : "");
		}
		if (text.Length == 0)
		{
			return true;
		}
		diagnostic = $"Mod '{manifest.Id}' 托管兼容声明无效：{text}；请使用当前工具链重新编译并重新导出。";
		return false;
	}
}
