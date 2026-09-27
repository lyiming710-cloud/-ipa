using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

internal sealed class XWProductionDropTimeline : Control
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

	private ProduceComponentDefinition _definition;

	public void Bind(ProduceComponentDefinition definition)
	{
		_definition = definition;
		QueueRedraw();
	}

	public override void _Draw()
	{
		if (GodotObject.IsInstanceValid(_definition))
		{
			Font fallbackFont = ThemeDB.FallbackFont;
			float num = 18f;
			float num2 = Mathf.Max(120f, Size.X - 36f);
			float num3 = 58f;
			DrawLine(new Vector2(num, num3), new Vector2(num + num2, num3), new Color("748676"), 3f);
			Vector2 initialDelayRange = _definition.initialDelayRange;
			float num4 = Mathf.Max(0.001f, _definition.produceInterval);
			float num5 = Mathf.Max(Mathf.Max(initialDelayRange.X, initialDelayRange.Y), num4);
			float num6 = num2 / Mathf.Max(1f, num5 + num4 * 0.25f);
			float num7 = num + Mathf.Max(0f, Mathf.Min(initialDelayRange.X, initialDelayRange.Y)) * num6;
			float num8 = num + Mathf.Max(0f, Mathf.Max(initialDelayRange.X, initialDelayRange.Y)) * num6;
			float num9 = num + num5 * num6;
			float num10 = Mathf.Max(num, num9 - Mathf.Max(0f, _definition.glowLeadTime) * num6);
			DrawRect(new Rect2(new Vector2(num7, num3 - 14f), new Vector2(Mathf.Max(2f, num8 - num7), 28f)), new Color("365b77"));
			DrawCircle(new Vector2(num10, num3), 8f, new Color("fff07a"));
			DrawCircle(new Vector2(num9, num3), 10f, new Color("77d48d"));
			DrawString(fallbackFont, new Vector2(num, 22f), "初始延迟范围", HorizontalAlignment.Left, -1f, 13, new Color("8fb6d2"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			DrawString(fallbackFont, new Vector2(Mathf.Max(num, num10 - 28f), 102f), "发光", HorizontalAlignment.Left, -1f, 13, new Color("fff07a"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			DrawString(fallbackFont, new Vector2(Mathf.Max(num, num9 - 32f), 125f), "生产", HorizontalAlignment.Left, -1f, 13, new Color("77d48d"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			DrawString(fallbackFont, new Vector2(num, 145f), $"追赶上限 {Math.Max(1, _definition.maxCatchUpProductions)} 次/帧 · 亮起 {_definition.glowFadeInTime:0.##}s · 熄灭 {_definition.glowFadeOutTime:0.##}s", HorizontalAlignment.Left, -1f, 12, new Color("9bad9b"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
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
			Bind(VariantUtils.ConvertTo<ProduceComponentDefinition>(in args[0]));
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
			_definition = VariantUtils.ConvertTo<ProduceComponentDefinition>(in value);
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
			_definition = value.As<ProduceComponentDefinition>();
		}
	}
}
