using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Scene/TowerDefesne/TowerDefenseNew/TowerDefenseCardScroll.cs")]
public class TowerDefenseCardScroll : ScrollContainer
{
	public new class MethodName : ScrollContainer.MethodName
	{
		public static readonly StringName ConfigureScrolling = "ConfigureScrolling";

		public static readonly StringName CardsOverflow = "CardsOverflow";

		public new static readonly StringName _Input = "_Input";
	}

	public new class PropertyName : ScrollContainer.PropertyName
	{
		public static readonly StringName ScrollingEnabled = "ScrollingEnabled";

		public static readonly StringName Vertical = "Vertical";

		public static readonly StringName _touchIndex = "_touchIndex";

		public static readonly StringName _touchStart = "_touchStart";

		public static readonly StringName _touchScroll = "_touchScroll";

		public static readonly StringName _touchDragging = "_touchDragging";
	}

	public new class SignalName : ScrollContainer.SignalName
	{
	}

	private int _touchIndex = -1;

	private Vector2 _touchStart;

	private float _touchScroll;

	private bool _touchDragging;

	public bool ScrollingEnabled { get; private set; }

	public bool Vertical { get; private set; }

	public void ConfigureScrolling(bool enabled, bool vertical)
	{
		if (Vertical != vertical || !enabled)
		{
			int scrollHorizontal = (ScrollVertical = 0);
			ScrollHorizontal = scrollHorizontal;
			_touchIndex = -1;
			_touchDragging = false;
		}
		ScrollingEnabled = enabled;
		Vertical = vertical;
		HorizontalScrollMode = (ScrollMode)((enabled && !vertical) ? 3 : 0);
		VerticalScrollMode = (ScrollMode)((enabled & vertical) ? 3 : 0);
	}

	private bool CardsOverflow()
	{
		Control childOrNull = GetChildOrNull<Control>(0);
		if (childOrNull == null)
		{
			return false;
		}
		if (!Vertical)
		{
			return childOrNull.Size.X > Size.X;
		}
		return childOrNull.Size.Y > Size.Y;
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!ScrollingEnabled || !CardsOverflow() || !IsVisibleInTree())
		{
			return;
		}
		if (inputEvent is InputEventScreenTouch inputEventScreenTouch)
		{
			if (inputEventScreenTouch.Pressed && _touchIndex < 0 && GetGlobalRect().HasPoint(inputEventScreenTouch.Position))
			{
				_touchIndex = inputEventScreenTouch.Index;
				_touchStart = inputEventScreenTouch.Position;
				_touchScroll = (Vertical ? ScrollVertical : ScrollHorizontal);
				_touchDragging = false;
			}
			else if (!inputEventScreenTouch.Pressed && inputEventScreenTouch.Index == _touchIndex)
			{
				if (_touchDragging)
				{
					TowerDefenseManager.Instance.GetPacketPickControl()?.Release();
				}
				_touchIndex = -1;
				_touchDragging = false;
			}
		}
		else if (inputEvent is InputEventScreenDrag inputEventScreenDrag && inputEventScreenDrag.Index == _touchIndex)
		{
			Vector2 vector = inputEventScreenDrag.Position - _touchStart;
			if (_touchDragging || !(vector.Length() < 16f))
			{
				_touchDragging = true;
				TowerDefenseManager.Instance.GetPacketPickControl()?.Release();
				if (Vertical)
				{
					ScrollVertical = (int)(_touchScroll - vector.Y);
				}
				else
				{
					ScrollHorizontal = (int)(_touchScroll - vector.X);
				}
				GetViewport().SetInputAsHandled();
			}
		}
		else
		{
			if (!(inputEvent is InputEventMouseButton { Pressed: not false } inputEventMouseButton) || !GetGlobalRect().HasPoint(inputEventMouseButton.Position))
			{
				return;
			}
			MouseButton buttonIndex = inputEventMouseButton.ButtonIndex;
			int num;
			if ((buttonIndex == MouseButton.WheelDown || buttonIndex == MouseButton.WheelRight) ? true : false)
			{
				num = 1;
			}
			else
			{
				MouseButton buttonIndex2 = inputEventMouseButton.ButtonIndex;
				bool flag = ((buttonIndex2 == MouseButton.WheelUp || buttonIndex2 == MouseButton.WheelLeft) ? true : false);
				num = (flag ? (-1) : 0);
			}
			int num2 = num;
			if (num2 != 0)
			{
				int num3 = (int)((float)(Vertical ? 62 : 51) * Math.Max(1f, Math.Abs(inputEventMouseButton.Factor)));
				if (Vertical)
				{
					ScrollVertical += num2 * num3;
				}
				else
				{
					ScrollHorizontal += num2 * num3;
				}
				GetViewport().SetInputAsHandled();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.ConfigureScrolling, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "vertical", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CardsOverflow, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ConfigureScrolling && args.Count == 2)
		{
			ConfigureScrolling(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CardsOverflow && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CardsOverflow());
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ConfigureScrolling)
		{
			return true;
		}
		if (method == MethodName.CardsOverflow)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ScrollingEnabled)
		{
			ScrollingEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Vertical)
		{
			Vertical = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._touchIndex)
		{
			_touchIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._touchStart)
		{
			_touchStart = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._touchScroll)
		{
			_touchScroll = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._touchDragging)
		{
			_touchDragging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.ScrollingEnabled)
		{
			from = ScrollingEnabled;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Vertical)
		{
			from = Vertical;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._touchIndex)
		{
			value = VariantUtils.CreateFrom(in _touchIndex);
			return true;
		}
		if (name == PropertyName._touchStart)
		{
			value = VariantUtils.CreateFrom(in _touchStart);
			return true;
		}
		if (name == PropertyName._touchScroll)
		{
			value = VariantUtils.CreateFrom(in _touchScroll);
			return true;
		}
		if (name == PropertyName._touchDragging)
		{
			value = VariantUtils.CreateFrom(in _touchDragging);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.ScrollingEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Vertical, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._touchIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._touchStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._touchScroll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._touchDragging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ScrollingEnabled, Variant.From<bool>(ScrollingEnabled));
		info.AddProperty(PropertyName.Vertical, Variant.From<bool>(Vertical));
		info.AddProperty(PropertyName._touchIndex, Variant.From(in _touchIndex));
		info.AddProperty(PropertyName._touchStart, Variant.From(in _touchStart));
		info.AddProperty(PropertyName._touchScroll, Variant.From(in _touchScroll));
		info.AddProperty(PropertyName._touchDragging, Variant.From(in _touchDragging));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ScrollingEnabled, out var value))
		{
			ScrollingEnabled = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Vertical, out var value2))
		{
			Vertical = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._touchIndex, out var value3))
		{
			_touchIndex = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._touchStart, out var value4))
		{
			_touchStart = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._touchScroll, out var value5))
		{
			_touchScroll = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName._touchDragging, out var value6))
		{
			_touchDragging = value6.As<bool>();
		}
	}
}
