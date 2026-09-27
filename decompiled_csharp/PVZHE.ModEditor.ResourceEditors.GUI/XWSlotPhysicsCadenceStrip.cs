using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

internal sealed class XWSlotPhysicsCadenceStrip : Control
{
	public new class MethodName : Control.MethodName
	{
		public static readonly StringName Bind = "Bind";

		public new static readonly StringName _Draw = "_Draw";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _interval = "_interval";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private int _interval = 3;

	public void Bind(int interval)
	{
		_interval = Math.Clamp(interval, 1, 30);
		QueueRedraw();
	}

	public override void _Draw()
	{
		int num = Math.Min(12, _interval);
		float num2 = 5f;
		float num3 = Math.Max(14f, (Size.X - num2 * (float)(num - 1)) / (float)num);
		Font fallbackFont = ThemeDB.FallbackFont;
		for (int i = 0; i < num; i++)
		{
			Rect2 rect = new Rect2(new Vector2((float)i * (num3 + num2), 8f), new Vector2(num3, 25f));
			bool flag = i == num - 1;
			DrawRect(rect, flag ? new Color("55c7ed") : new Color("294351"));
			DrawString(fallbackFont, rect.Position + new Vector2(5f, 18f), flag ? "扫描" : $"{i + 1}", HorizontalAlignment.Left, -1f, 12, flag ? new Color("08232f") : new Color("a6bdc9"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}
		if (_interval > num)
		{
			DrawString(fallbackFont, new Vector2(0f, 54f), $"… 共 {_interval} 个物理帧后重新查询占用者", HorizontalAlignment.Left, -1f, 12, new Color("88a9ba"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Bind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "interval", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Bind && args.Count == 1)
		{
			Bind(VariantUtils.ConvertTo<int>(in args[0]));
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
		if (name == PropertyName._interval)
		{
			_interval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._interval)
		{
			value = VariantUtils.CreateFrom(in _interval);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._interval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._interval, Variant.From(in _interval));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._interval, out var value))
		{
			_interval = value.As<int>();
		}
	}
}
