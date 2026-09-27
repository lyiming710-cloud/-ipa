using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

internal sealed class XWAttackContactTimeline : Control
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

	private AttackComponentDefinition _definition;

	public void Bind(AttackComponentDefinition definition)
	{
		_definition = definition;
		QueueRedraw();
	}

	public override void _Draw()
	{
		if (GodotObject.IsInstanceValid(_definition))
		{
			Rect2 rect = new Rect2(new Vector2(18f, 36f), new Vector2(Mathf.Max(100f, Size.X - 36f), 58f));
			DrawRect(rect, new Color("081018"));
			DrawRect(rect, new Color("566779"), filled: false, 1.5f);
			float[] array = new float[4] { 0.05f, 0.3f, 0.66f, 0.94f };
			Color[] array2 = new Color[4]
			{
				new Color("81b6d9"),
				new Color("d6a85e"),
				new Color("ff6b56"),
				new Color("79d487")
			};
			string[] array3 = new string[4] { "检测", "进入攻击", "命中事件", "动画完成" };
			Font fallbackFont = ThemeDB.FallbackFont;
			for (int i = 0; i < array.Length; i++)
			{
				float num = rect.Position.X + rect.Size.X * array[i];
				DrawLine(new Vector2(num, rect.Position.Y + 6f), new Vector2(num, rect.End.Y - 6f), array2[i], (i == 2) ? 4 : 2);
				DrawCircle(new Vector2(num, rect.GetCenter().Y), (i == 2) ? 7 : 5, array2[i]);
				DrawString(fallbackFont, new Vector2(num - 28f, 116f), array3[i], HorizontalAlignment.Center, 56f, 12, array2[i], TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			}
			string text = (string.IsNullOrWhiteSpace(_definition.attackEventName) ? "无命中事件" : ("“" + _definition.attackEventName + "” → OnAttack / eventList"));
			DrawString(fallbackFont, new Vector2(18f, 24f), text, HorizontalAlignment.Left, -1f, 14, new Color("ffd0a3"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
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
			Bind(VariantUtils.ConvertTo<AttackComponentDefinition>(in args[0]));
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
			_definition = VariantUtils.ConvertTo<AttackComponentDefinition>(in value);
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
			_definition = value.As<AttackComponentDefinition>();
		}
	}
}
