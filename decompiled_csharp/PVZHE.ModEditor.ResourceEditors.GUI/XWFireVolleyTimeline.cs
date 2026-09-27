using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

internal sealed class XWFireVolleyTimeline : Control
{
	public new class MethodName : Control.MethodName
	{
		public static readonly StringName Bind = "Bind";

		public new static readonly StringName _Draw = "_Draw";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _definition = "_definition";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private FireComponentDefinition _definition;

	public void Bind(FireComponentDefinition definition)
	{
		_definition = definition;
		QueueRedraw();
	}

	public override void _Draw()
	{
		if (GodotObject.IsInstanceValid(_definition))
		{
			Rect2 rect = new Rect2(new Vector2(18f, 34f), new Vector2(Mathf.Max(80f, Size.X - 36f), 54f));
			DrawRect(rect, new Color("091018"));
			DrawRect(rect, new Color("526477"), filled: false, 1.5f);
			int num = Math.Max(1, _definition.fireNum);
			float num2 = Mathf.Max(1f, rect.Size.X - 30f);
			for (int i = 0; i < num; i++)
			{
				float num3 = ((_definition.fireNumAtOnce || num == 1) ? 0.08f : (0.08f + 0.84f * (float)i / ((float)num - 1f)));
				Vector2 position = new Vector2(rect.Position.X + num2 * num3, rect.GetCenter().Y);
				DrawLine(new Vector2(position.X, rect.Position.Y + 8f), new Vector2(position.X, rect.End.Y - 8f), new Color("ffb55a"), 3f);
				DrawCircle(position, (i == 0) ? 7 : 5, new Color("ffe1a8"));
			}
			Font fallbackFont = ThemeDB.FallbackFont;
			DrawString(fallbackFont, new Vector2(18f, 22f), _definition.fireNumAtOnce ? "一个动画事件 · 同时齐射" : "多个动画事件 · 逐次连射", HorizontalAlignment.Left, -1f, 14, new Color("f6ce8c"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			float num4 = Mathf.Abs(_definition.fireIntervalOffset);
			float value = Mathf.Max(0f, _definition.fireInterval - num4 * 2f);
			float value2 = Mathf.Max(0f, _definition.fireInterval - num4);
			DrawString(fallbackFont, new Vector2(18f, 113f), $"基础动画间隔 {_definition.fireIntervalBase:0.##} 秒 · 检测间隔 {value:0.##}～{value2:0.##} 秒", HorizontalAlignment.Left, -1f, 13, new Color("91a6bc"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Bind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Bind && args.Count == 1)
		{
			Bind(VariantUtils.ConvertTo<FireComponentDefinition>(in args[0]));
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
		if (name == PropertyName._definition)
		{
			_definition = VariantUtils.ConvertTo<FireComponentDefinition>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._definition)
		{
			value = VariantUtils.CreateFrom(in _definition);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._definition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._definition, Variant.From(in _definition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._definition, out var value))
		{
			_definition = value.As<FireComponentDefinition>();
		}
	}
}
