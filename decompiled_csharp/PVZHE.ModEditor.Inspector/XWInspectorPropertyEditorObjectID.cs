using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/ObjectID/XWInspectorPropertyEditorObjectID.cs")]
public class XWInspectorPropertyEditorObjectID : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName OnIdTextChanged = "OnIdTextChanged";

		public static readonly StringName ApplyEditedId = "ApplyEditedId";

		public static readonly StringName RestoreCanonicalTextIfInvalid = "RestoreCanonicalTextIfInvalid";

		public static readonly StringName UpdateObjectState = "UpdateObjectState";

		public static readonly StringName SetStatus = "SetStatus";

		public static readonly StringName OnJumpPressed = "OnJumpPressed";

		public static readonly StringName GetSafeObjectDbCapacity = "GetSafeObjectDbCapacity";

		public static readonly StringName GetObjectSlot = "GetObjectSlot";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _idInput = "_idInput";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _jumpButton = "_jumpButton";

		public static readonly StringName _displayedId = "_displayedId";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/ObjectID/XWInspectorPropertyEditorObjectID.tscn";

	private const ulong ObjectDbSlotMask = 16777215uL;

	private static readonly Color EmptyColor = new Color("8B96A8");

	private static readonly Color ValidColor = new Color("63D69D");

	private static readonly Color InvalidColor = new Color("FF6B70");

	private static readonly Color InputErrorColor = new Color("FFB84D");

	private LineEdit _idInput;

	private Label _statusLabel;

	private Button _jumpButton;

	private long _displayedId;

	public static XWInspectorPropertyEditorObjectID Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/ObjectID/XWInspectorPropertyEditorObjectID.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorObjectID>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_idInput = GetNode<LineEdit>("%IdInput");
		_statusLabel = GetNode<Label>("%StatusLabel");
		_jumpButton = GetNode<Button>("%JumpButton");
		_idInput.TextChanged += OnIdTextChanged;
		_idInput.TextSubmitted += (string _) =>
		{
			RestoreCanonicalTextIfInvalid();
		};
		_jumpButton.Pressed += OnJumpPressed;
	}

	public override void UpdateValue()
	{
		Variant propertyValue = GetPropertyValue();
		long id = (_displayedId = ((propertyValue.VariantType == Variant.Type.Nil) ? 0 : propertyValue.AsInt64()));
		XWInspectorPropertyEditorBase.SetLineEditTextPreservingCaret(_idInput, id.ToString(CultureInfo.InvariantCulture));
		UpdateObjectState(id);
	}

	public override Variant GetValue()
	{
		return _displayedId;
	}

	private void OnIdTextChanged(string text)
	{
		long result;
		if (string.IsNullOrWhiteSpace(text))
		{
			ApplyEditedId(0L);
		}
		else if (!long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
		{
			SetStatus("▲ 请输入整数 ID", InputErrorColor, "ObjectId 必须是 -9223372036854775808 到 9223372036854775807 之间的整数。");
			_jumpButton.Disabled = true;
			_jumpButton.TooltipText = "输入有效整数后才能定位对象";
		}
		else
		{
			ApplyEditedId(result);
		}
	}

	private void ApplyEditedId(long id)
	{
		_displayedId = id;
		UpdateObjectState(id);
		ValueChange(id);
	}

	private void RestoreCanonicalTextIfInvalid()
	{
		if (!long.TryParse(_idInput.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var _))
		{
			XWInspectorPropertyEditorBase.SetLineEditTextPreservingCaret(_idInput, _displayedId.ToString(CultureInfo.InvariantCulture));
			UpdateObjectState(_displayedId);
		}
	}

	private void UpdateObjectState(long id)
	{
		GodotObject result;
		bool outsideSafeCapacity;
		if (id == 0L)
		{
			SetStatus("○ 未指定", EmptyColor, "0 表示没有绑定对象。");
			_jumpButton.Disabled = true;
			_jumpButton.TooltipText = "当前没有可定位的对象";
		}
		else if (!TryResolveObject(id, out result, out outsideSafeCapacity))
		{
			if (outsideSafeCapacity)
			{
				SetStatus("△ 暂无法确认", InputErrorColor, $"实例 ID {id} 超出当前编辑器可安全查询的 ObjectDB 范围。\n" + "为避免 Godot 原生错误，当前不会执行不安全查询或定位。");
				_jumpButton.Disabled = true;
				_jumpButton.TooltipText = "超出安全查询范围，暂时无法定位";
			}
			else
			{
				SetStatus("× 对象已失效", InvalidColor, $"找不到实例 ID {id}，对象可能已释放。");
				_jumpButton.Disabled = true;
				_jumpButton.TooltipText = "对象已失效，无法定位";
			}
		}
		else
		{
			string text = ((result is Node node && node.Name != (StringName)"") ? node.Name.ToString() : result.GetClass());
			SetStatus("● " + text, ValidColor, $"有效对象\n类型：{result.GetClass()}\n实例 ID：{id}");
			_jumpButton.Disabled = false;
			_jumpButton.TooltipText = "在检查器中定位 " + text;
		}
	}

	private void SetStatus(string text, Color color, string tooltip)
	{
		_statusLabel.Text = text;
		_statusLabel.TooltipText = tooltip;
		_statusLabel.AddThemeColorOverride("font_color", color);
	}

	private void OnJumpPressed()
	{
		if (_displayedId == 0L)
		{
			UpdateObjectState(_displayedId);
			return;
		}
		if (!TryResolveObject(_displayedId, out var result, out var _))
		{
			UpdateObjectState(_displayedId);
			return;
		}
		XWInspector inspector = GetInspector();
		if (GodotObject.IsInstanceValid(inspector))
		{
			inspector.SetObject(result);
		}
	}

	private bool TryResolveObject(long id, out GodotObject result, out bool outsideSafeCapacity)
	{
		result = null;
		outsideSafeCapacity = false;
		if (id == 0L)
		{
			return false;
		}
		long num = id & 0xFFFFFF;
		ulong safeObjectDbCapacity = GetSafeObjectDbCapacity();
		if ((ulong)num >= safeObjectDbCapacity)
		{
			outsideSafeCapacity = true;
			return false;
		}
		result = GodotObject.InstanceFromId((ulong)id);
		return GodotObject.IsInstanceValid(result);
	}

	private ulong GetSafeObjectDbCapacity()
	{
		ulong val = 1uL;
		double monitor = Performance.GetMonitor(Performance.Monitor.ObjectCount);
		if (monitor > 1.0)
		{
			val = (ulong)Math.Ceiling(monitor);
		}
		val = Math.Max(val, GetObjectSlot(this) + 1);
		if (GodotObject.IsInstanceValid(Property?.Object))
		{
			val = Math.Max(val, GetObjectSlot(Property.Object) + 1);
		}
		ulong num = 1uL;
		while (num < val && num <= 8388608)
		{
			num <<= 1;
		}
		return num;
	}

	private static ulong GetObjectSlot(GodotObject obj)
	{
		if (!GodotObject.IsInstanceValid(obj))
		{
			return 0uL;
		}
		return obj.GetInstanceId() & 0xFFFFFF;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnIdTextChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyEditedId, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreCanonicalTextIfInvalid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateObjectState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnJumpPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSafeObjectDbCapacity, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetObjectSlot, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorObjectID>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateValue && args.Count == 0)
		{
			UpdateValue();
			ret = default;
			return true;
		}
		if (method == MethodName.GetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetValue());
			return true;
		}
		if (method == MethodName.OnIdTextChanged && args.Count == 1)
		{
			OnIdTextChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyEditedId && args.Count == 1)
		{
			ApplyEditedId(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreCanonicalTextIfInvalid && args.Count == 0)
		{
			RestoreCanonicalTextIfInvalid();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateObjectState && args.Count == 1)
		{
			UpdateObjectState(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetStatus && args.Count == 3)
		{
			SetStatus(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnJumpPressed && args.Count == 0)
		{
			OnJumpPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.GetSafeObjectDbCapacity && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetSafeObjectDbCapacity());
			return true;
		}
		if (method == MethodName.GetObjectSlot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetObjectSlot(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorObjectID>(Create());
			return true;
		}
		if (method == MethodName.GetObjectSlot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetObjectSlot(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.UpdateValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.OnIdTextChanged)
		{
			return true;
		}
		if (method == MethodName.ApplyEditedId)
		{
			return true;
		}
		if (method == MethodName.RestoreCanonicalTextIfInvalid)
		{
			return true;
		}
		if (method == MethodName.UpdateObjectState)
		{
			return true;
		}
		if (method == MethodName.SetStatus)
		{
			return true;
		}
		if (method == MethodName.OnJumpPressed)
		{
			return true;
		}
		if (method == MethodName.GetSafeObjectDbCapacity)
		{
			return true;
		}
		if (method == MethodName.GetObjectSlot)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._idInput)
		{
			_idInput = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._jumpButton)
		{
			_jumpButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._displayedId)
		{
			_displayedId = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._idInput)
		{
			value = VariantUtils.CreateFrom(in _idInput);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._jumpButton)
		{
			value = VariantUtils.CreateFrom(in _jumpButton);
			return true;
		}
		if (name == PropertyName._displayedId)
		{
			value = VariantUtils.CreateFrom(in _displayedId);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._idInput, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._jumpButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._displayedId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._idInput, Variant.From(in _idInput));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._jumpButton, Variant.From(in _jumpButton));
		info.AddProperty(PropertyName._displayedId, Variant.From(in _displayedId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._idInput, out var value))
		{
			_idInput = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value2))
		{
			_statusLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._jumpButton, out var value3))
		{
			_jumpButton = value3.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._displayedId, out var value4))
		{
			_displayedId = value4.As<long>();
		}
	}
}
