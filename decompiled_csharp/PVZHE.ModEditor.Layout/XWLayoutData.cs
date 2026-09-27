using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Layout;

[ScriptPath("res://addons/ModEditor/Layout/XWLayoutData.cs")]
public class XWLayoutData : RefCounted
{
	public enum DockPosition
	{
		Left,
		Right,
		Center,
		Bottom,
		LeftTop,
		LeftBottom,
		LeftTopRight,
		LeftBottomRight,
		RightTop,
		RightBottom,
		RightTopRight,
		RightBottomRight
	}

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName ToJson = "ToJson";

		public static readonly StringName FromJson = "FromJson";

		public static readonly StringName ResolveDockPosition = "ResolveDockPosition";

		public static readonly StringName IsLeftDock = "IsLeftDock";

		public static readonly StringName IsRightDock = "IsRightDock";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName LayoutName = "LayoutName";

		public static readonly StringName HSplitOffsets = "HSplitOffsets";

		public static readonly StringName CenterVSplitOffset = "CenterVSplitOffset";

		public static readonly StringName LeftDockVisible = "LeftDockVisible";

		public static readonly StringName LeftDockStretchRatio = "LeftDockStretchRatio";

		public static readonly StringName RightDockVisible = "RightDockVisible";

		public static readonly StringName RightDockStretchRatio = "RightDockStretchRatio";

		public static readonly StringName CenterDockVisible = "CenterDockVisible";

		public static readonly StringName BottomDockVisible = "BottomDockVisible";

		public static readonly StringName BottomDockStretchRatio = "BottomDockStretchRatio";

		public static readonly StringName LeftVSplitOffset = "LeftVSplitOffset";

		public static readonly StringName LeftHSplitOffset = "LeftHSplitOffset";

		public static readonly StringName RightVSplitOffset = "RightVSplitOffset";

		public static readonly StringName RightHSplitOffset = "RightHSplitOffset";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public string LayoutName = "默认";

	public int[] HSplitOffsets = System.Array.Empty<int>();

	public int CenterVSplitOffset;

	public bool LeftDockVisible = true;

	public float LeftDockStretchRatio = 0.2f;

	public bool RightDockVisible = true;

	public float RightDockStretchRatio = 0.2f;

	public bool CenterDockVisible = true;

	public bool BottomDockVisible = true;

	public float BottomDockStretchRatio = 0.3f;

	public int LeftVSplitOffset;

	public int LeftHSplitOffset;

	public int RightVSplitOffset;

	public int RightHSplitOffset;

	public System.Collections.Generic.Dictionary<string, int> PanelDocks = new System.Collections.Generic.Dictionary<string, int>();

	public System.Collections.Generic.Dictionary<string, string> PanelTabTitles = new System.Collections.Generic.Dictionary<string, string>();

	public Dictionary ToJson()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		int[] hSplitOffsets = HSplitOffsets;
		foreach (int num in hSplitOffsets)
		{
			array.Add(num);
		}
		Dictionary dictionary = new Dictionary();
		foreach (KeyValuePair<string, int> panelDock in PanelDocks)
		{
			dictionary[panelDock.Key] = panelDock.Value;
		}
		Dictionary dictionary2 = new Dictionary();
		foreach (KeyValuePair<string, string> panelTabTitle in PanelTabTitles)
		{
			dictionary2[panelTabTitle.Key] = panelTabTitle.Value;
		}
		return new Dictionary
		{
			{ "layout_name", LayoutName },
			{ "h_split_offsets", array },
			{ "center_vsplit_offset", CenterVSplitOffset },
			{ "left_dock_visible", LeftDockVisible },
			{ "left_dock_stretch_ratio", LeftDockStretchRatio },
			{ "right_dock_visible", RightDockVisible },
			{ "right_dock_stretch_ratio", RightDockStretchRatio },
			{ "center_dock_visible", CenterDockVisible },
			{ "bottom_dock_visible", BottomDockVisible },
			{ "bottom_dock_stretch_ratio", BottomDockStretchRatio },
			{ "left_vsplit_offset", LeftVSplitOffset },
			{ "left_hsplit_offset", LeftHSplitOffset },
			{ "right_vsplit_offset", RightVSplitOffset },
			{ "right_hsplit_offset", RightHSplitOffset },
			{ "panel_docks", dictionary },
			{ "panel_tab_titles", dictionary2 }
		};
	}

	public void FromJson(Dictionary data)
	{
		if (data.ContainsKey("layout_name"))
		{
			LayoutName = (string)data["layout_name"];
		}
		if (data.ContainsKey("h_split_offsets"))
		{
			Godot.Collections.Array array = (Godot.Collections.Array)data["h_split_offsets"];
			List<int> list = new List<int>();
			foreach (Variant item in array)
			{
				list.Add((int)item);
			}
			HSplitOffsets = list.ToArray();
		}
		if (data.ContainsKey("center_vsplit_offset"))
		{
			CenterVSplitOffset = (int)data["center_vsplit_offset"];
		}
		if (data.ContainsKey("left_dock_visible"))
		{
			LeftDockVisible = (bool)data["left_dock_visible"];
		}
		if (data.ContainsKey("left_dock_stretch_ratio"))
		{
			LeftDockStretchRatio = (float)data["left_dock_stretch_ratio"];
		}
		if (data.ContainsKey("right_dock_visible"))
		{
			RightDockVisible = (bool)data["right_dock_visible"];
		}
		if (data.ContainsKey("right_dock_stretch_ratio"))
		{
			RightDockStretchRatio = (float)data["right_dock_stretch_ratio"];
		}
		if (data.ContainsKey("center_dock_visible"))
		{
			CenterDockVisible = (bool)data["center_dock_visible"];
		}
		if (data.ContainsKey("bottom_dock_visible"))
		{
			BottomDockVisible = (bool)data["bottom_dock_visible"];
		}
		if (data.ContainsKey("bottom_dock_stretch_ratio"))
		{
			BottomDockStretchRatio = (float)data["bottom_dock_stretch_ratio"];
		}
		if (data.ContainsKey("left_vsplit_offset"))
		{
			LeftVSplitOffset = (int)data["left_vsplit_offset"];
		}
		if (data.ContainsKey("left_hsplit_offset"))
		{
			LeftHSplitOffset = (int)data["left_hsplit_offset"];
		}
		if (data.ContainsKey("right_vsplit_offset"))
		{
			RightVSplitOffset = (int)data["right_vsplit_offset"];
		}
		if (data.ContainsKey("right_hsplit_offset"))
		{
			RightHSplitOffset = (int)data["right_hsplit_offset"];
		}
		if (data.ContainsKey("panel_docks"))
		{
			PanelDocks.Clear();
			Dictionary dictionary = (Dictionary)data["panel_docks"];
			foreach (Variant key in dictionary.Keys)
			{
				PanelDocks[(string)key] = (int)dictionary[key];
			}
		}
		if (!data.ContainsKey("panel_tab_titles"))
		{
			return;
		}
		PanelTabTitles.Clear();
		Dictionary dictionary2 = (Dictionary)data["panel_tab_titles"];
		foreach (Variant key2 in dictionary2.Keys)
		{
			PanelTabTitles[(string)key2] = (string)dictionary2[key2];
		}
	}

	public static int ResolveDockPosition(int dockPos)
	{
		return dockPos switch
		{
			0 => 4, 
			1 => 8, 
			_ => dockPos, 
		};
	}

	public static bool IsLeftDock(int dockPos)
	{
		if (dockPos != 0 && dockPos != 4 && dockPos != 5 && dockPos != 6)
		{
			return dockPos == 7;
		}
		return true;
	}

	public static bool IsRightDock(int dockPos)
	{
		if (dockPos != 1 && dockPos != 8 && dockPos != 9 && dockPos != 10)
		{
			return dockPos == 11;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.ToJson, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FromJson, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveDockPosition, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "dockPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsLeftDock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "dockPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRightDock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "dockPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ToJson && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ToJson());
			return true;
		}
		if (method == MethodName.FromJson && args.Count == 1)
		{
			FromJson(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveDockPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveDockPosition(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsLeftDock && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLeftDock(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsRightDock && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRightDock(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResolveDockPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveDockPosition(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsLeftDock && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLeftDock(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsRightDock && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRightDock(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ToJson)
		{
			return true;
		}
		if (method == MethodName.FromJson)
		{
			return true;
		}
		if (method == MethodName.ResolveDockPosition)
		{
			return true;
		}
		if (method == MethodName.IsLeftDock)
		{
			return true;
		}
		if (method == MethodName.IsRightDock)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.LayoutName)
		{
			LayoutName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.HSplitOffsets)
		{
			HSplitOffsets = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName.CenterVSplitOffset)
		{
			CenterVSplitOffset = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.LeftDockVisible)
		{
			LeftDockVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.LeftDockStretchRatio)
		{
			LeftDockStretchRatio = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.RightDockVisible)
		{
			RightDockVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.RightDockStretchRatio)
		{
			RightDockStretchRatio = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.CenterDockVisible)
		{
			CenterDockVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.BottomDockVisible)
		{
			BottomDockVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.BottomDockStretchRatio)
		{
			BottomDockStretchRatio = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.LeftVSplitOffset)
		{
			LeftVSplitOffset = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.LeftHSplitOffset)
		{
			LeftHSplitOffset = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.RightVSplitOffset)
		{
			RightVSplitOffset = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.RightHSplitOffset)
		{
			RightHSplitOffset = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.LayoutName)
		{
			value = VariantUtils.CreateFrom(in LayoutName);
			return true;
		}
		if (name == PropertyName.HSplitOffsets)
		{
			value = VariantUtils.CreateFrom(in HSplitOffsets);
			return true;
		}
		if (name == PropertyName.CenterVSplitOffset)
		{
			value = VariantUtils.CreateFrom(in CenterVSplitOffset);
			return true;
		}
		if (name == PropertyName.LeftDockVisible)
		{
			value = VariantUtils.CreateFrom(in LeftDockVisible);
			return true;
		}
		if (name == PropertyName.LeftDockStretchRatio)
		{
			value = VariantUtils.CreateFrom(in LeftDockStretchRatio);
			return true;
		}
		if (name == PropertyName.RightDockVisible)
		{
			value = VariantUtils.CreateFrom(in RightDockVisible);
			return true;
		}
		if (name == PropertyName.RightDockStretchRatio)
		{
			value = VariantUtils.CreateFrom(in RightDockStretchRatio);
			return true;
		}
		if (name == PropertyName.CenterDockVisible)
		{
			value = VariantUtils.CreateFrom(in CenterDockVisible);
			return true;
		}
		if (name == PropertyName.BottomDockVisible)
		{
			value = VariantUtils.CreateFrom(in BottomDockVisible);
			return true;
		}
		if (name == PropertyName.BottomDockStretchRatio)
		{
			value = VariantUtils.CreateFrom(in BottomDockStretchRatio);
			return true;
		}
		if (name == PropertyName.LeftVSplitOffset)
		{
			value = VariantUtils.CreateFrom(in LeftVSplitOffset);
			return true;
		}
		if (name == PropertyName.LeftHSplitOffset)
		{
			value = VariantUtils.CreateFrom(in LeftHSplitOffset);
			return true;
		}
		if (name == PropertyName.RightVSplitOffset)
		{
			value = VariantUtils.CreateFrom(in RightVSplitOffset);
			return true;
		}
		if (name == PropertyName.RightHSplitOffset)
		{
			value = VariantUtils.CreateFrom(in RightHSplitOffset);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.LayoutName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.HSplitOffsets, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CenterVSplitOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.LeftDockVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.LeftDockStretchRatio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RightDockVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.RightDockStretchRatio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CenterDockVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.BottomDockVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.BottomDockStretchRatio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LeftVSplitOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LeftHSplitOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RightVSplitOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RightHSplitOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.LayoutName, Variant.From(in LayoutName));
		info.AddProperty(PropertyName.HSplitOffsets, Variant.From(in HSplitOffsets));
		info.AddProperty(PropertyName.CenterVSplitOffset, Variant.From(in CenterVSplitOffset));
		info.AddProperty(PropertyName.LeftDockVisible, Variant.From(in LeftDockVisible));
		info.AddProperty(PropertyName.LeftDockStretchRatio, Variant.From(in LeftDockStretchRatio));
		info.AddProperty(PropertyName.RightDockVisible, Variant.From(in RightDockVisible));
		info.AddProperty(PropertyName.RightDockStretchRatio, Variant.From(in RightDockStretchRatio));
		info.AddProperty(PropertyName.CenterDockVisible, Variant.From(in CenterDockVisible));
		info.AddProperty(PropertyName.BottomDockVisible, Variant.From(in BottomDockVisible));
		info.AddProperty(PropertyName.BottomDockStretchRatio, Variant.From(in BottomDockStretchRatio));
		info.AddProperty(PropertyName.LeftVSplitOffset, Variant.From(in LeftVSplitOffset));
		info.AddProperty(PropertyName.LeftHSplitOffset, Variant.From(in LeftHSplitOffset));
		info.AddProperty(PropertyName.RightVSplitOffset, Variant.From(in RightVSplitOffset));
		info.AddProperty(PropertyName.RightHSplitOffset, Variant.From(in RightHSplitOffset));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.LayoutName, out var value))
		{
			LayoutName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.HSplitOffsets, out var value2))
		{
			HSplitOffsets = value2.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName.CenterVSplitOffset, out var value3))
		{
			CenterVSplitOffset = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.LeftDockVisible, out var value4))
		{
			LeftDockVisible = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.LeftDockStretchRatio, out var value5))
		{
			LeftDockStretchRatio = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.RightDockVisible, out var value6))
		{
			RightDockVisible = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.RightDockStretchRatio, out var value7))
		{
			RightDockStretchRatio = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.CenterDockVisible, out var value8))
		{
			CenterDockVisible = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.BottomDockVisible, out var value9))
		{
			BottomDockVisible = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.BottomDockStretchRatio, out var value10))
		{
			BottomDockStretchRatio = value10.As<float>();
		}
		if (info.TryGetProperty(PropertyName.LeftVSplitOffset, out var value11))
		{
			LeftVSplitOffset = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName.LeftHSplitOffset, out var value12))
		{
			LeftHSplitOffset = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName.RightVSplitOffset, out var value13))
		{
			RightVSplitOffset = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName.RightHSplitOffset, out var value14))
		{
			RightHSplitOffset = value14.As<int>();
		}
	}
}
