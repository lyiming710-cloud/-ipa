using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

internal sealed class XWProductionHealthRuler : Control
{
	public new class MethodName : Control.MethodName
	{
		public static readonly StringName Bind = "Bind";

		public new static readonly StringName _Draw = "_Draw";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _segments = "_segments";

		public static readonly StringName _catchUp = "_catchUp";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private int _segments = 6;

	private int _catchUp = 1;

	public void Bind(int segments, int catchUp)
	{
		_segments = Math.Clamp(segments, 1, 1000);
		_catchUp = Math.Clamp(catchUp, 1, 120);
		QueueRedraw();
	}

	public override void _Draw()
	{
		Font fallbackFont = ThemeDB.FallbackFont;
		Rect2 rect = new Rect2(new Vector2(18f, 28f), new Vector2(Mathf.Max(100f, Size.X - 36f), 24f));
		DrawRect(rect, new Color("2c5637"));
		DrawRect(rect, new Color("7bd78c"), filled: false, 2f);
		int num = Math.Min(_segments, 40);
		for (int i = 1; i < num; i++)
		{
			float x = rect.Position.X + rect.Size.X * (float)i / (float)num;
			DrawLine(new Vector2(x, rect.Position.Y), new Vector2(x, rect.End.Y), new Color("d7f0d8"), 1f);
		}
		DrawString(fallbackFont, new Vector2(18f, 78f), $"100% → 0% 共 {_segments} 个阈值；单帧最多补产 {_catchUp} 次", HorizontalAlignment.Left, -1f, 13, new Color("b8d9ba"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Bind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "segments", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "catchUp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Bind && args.Count == 2)
		{
			Bind(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Bind)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._segments)
		{
			_segments = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._catchUp)
		{
			_catchUp = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._segments)
		{
			value = VariantUtils.CreateFrom(in _segments);
			return true;
		}
		if (name == PropertyName._catchUp)
		{
			value = VariantUtils.CreateFrom(in _catchUp);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._segments, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._catchUp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._segments, Variant.From(in _segments));
		info.AddProperty(PropertyName._catchUp, Variant.From(in _catchUp));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._segments, out var value))
		{
			_segments = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._catchUp, out var value2))
		{
			_catchUp = value2.As<int>();
		}
	}
}
