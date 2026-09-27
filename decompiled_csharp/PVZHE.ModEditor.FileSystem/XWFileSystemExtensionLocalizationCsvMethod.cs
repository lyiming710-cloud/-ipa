using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Localization.GUI;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/Extension/Localization/XWFileSystemExtensionLocalizationCsvMethod.cs")]
public class XWFileSystemExtensionLocalizationCsvMethod : XWFileSystemExtensionMethod
{
	public new class MethodName : XWFileSystemExtensionMethod.MethodName
	{
		public new static readonly StringName GetIcon = "GetIcon";

		public new static readonly StringName Execute = "Execute";
	}

	public new class PropertyName : XWFileSystemExtensionMethod.PropertyName
	{
	}

	public new class SignalName : XWFileSystemExtensionMethod.SignalName
	{
	}

	private const string LocalizationEditorWindowScenePath = "res://addons/ModEditor/Localization/GUI/XWLocalizationCsvEditorWindow.tscn";

	private static PackedScene _localizationEditorWindowScene;

	public override Texture2D GetIcon(XWFileSystemTreeItemData data)
	{
		return XWClassRegistry.Instance.GetUIIcon("Translation") ?? XWClassRegistry.Instance.GetUIIcon("File");
	}

	public override void Execute(XWFileSystemTreeItemData data)
	{
		if (data == null || string.IsNullOrWhiteSpace(data.Path))
		{
			return;
		}
		if (!ResourceLoader.Exists("res://addons/ModEditor/Localization/GUI/XWLocalizationCsvEditorWindow.tscn"))
		{
			XWEditorInterface.Instance?.ShowToast("多语言编辑窗口场景缺失");
			return;
		}
		if (_localizationEditorWindowScene == null)
		{
			_localizationEditorWindowScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Localization/GUI/XWLocalizationCsvEditorWindow.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		Window window = _localizationEditorWindowScene?.Instantiate<Window>(PackedScene.GenEditState.Disabled);
		XWLocalizationEditorPanel xWLocalizationEditorPanel = window?.GetNodeOrNull<XWLocalizationEditorPanel>("LocalizationEditorPanel");
		if (!GodotObject.IsInstanceValid(window) || !GodotObject.IsInstanceValid(xWLocalizationEditorPanel))
		{
			window?.QueueFree();
			XWEditorInterface.Instance?.ShowToast("无法加载多语言编辑窗口");
			return;
		}
		window.Title = "多语言 - " + Path.GetFileName(data.Path);
		window.CloseRequested += window.QueueFree;
		xWLocalizationEditorPanel.SetupCsvFile(data.Path);
		Node node = XWEditorInterface.Instance?.GetEditorPanel();
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			node = (Engine.GetMainLoop() as SceneTree)?.Root;
		}
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			window.QueueFree();
			XWEditorInterface.Instance?.ShowToast("无法打开多语言编辑窗口");
		}
		else
		{
			node.AddChild(window, forceReadableName: false, Node.InternalMode.Disabled);
			window.PopupCenteredClamped(new Vector2I(1100, 700), 0.9f);
			XWEditorInterface.Instance?.ShowToast("已打开多语言表: " + Path.GetFileName(data.Path));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.GetIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetIcon(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0])));
			return true;
		}
		if (method == MethodName.Execute && args.Count == 1)
		{
			Execute(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetIcon)
		{
			return true;
		}
		if (method == MethodName.Execute)
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
