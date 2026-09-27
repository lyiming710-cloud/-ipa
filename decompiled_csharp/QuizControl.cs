using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/GUI/InGame/QuizManager/QuizControl.cs")]
public class QuizControl : Control
{
	public delegate void RunButtonPressedEventHandler();

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName RunButtonPressed = "RunButtonPressed";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName startGUINode = "startGUINode";

		public static readonly StringName changePresentBoxButtonNode = "changePresentBoxButtonNode";

		public static readonly StringName betPanelBack = "betPanelBack";

		public static readonly StringName betPanelNode = "betPanelNode";

		public static readonly StringName coinLabel = "coinLabel";

		public static readonly StringName _runButton = "_runButton";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public Control startGUINode;

	public Control changePresentBoxButtonNode;

	public Panel betPanelBack;

	public Control betPanelNode;

	public Label coinLabel;

	private TextureButton _runButton;

	public event RunButtonPressedEventHandler OnRunButtonPressed;

	public override void _Ready()
	{
		startGUINode = GetNodeOrNull<Control>("%StartGUINode");
		changePresentBoxButtonNode = GetNodeOrNull<Control>("%ChangePresentBoxButtonNode");
		betPanelBack = GetNodeOrNull<Panel>("%BetPanelBack");
		betPanelNode = GetNodeOrNull<Control>("%BetPanelNode");
		coinLabel = GetNodeOrNull<Label>("%CoinLabel");
		_runButton = GetNode<TextureButton>("%StartGUINode/RunButton");
		_runButton.Pressed += RunButtonPressed;
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(_runButton))
		{
			_runButton.Pressed -= RunButtonPressed;
		}
		OnRunButtonPressed = null;
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance.coinBank))
		{
			TowerDefenseManager.Instance.coinBank.StartHide();
		}
		if (GodotObject.IsInstanceValid(BroadCastManager.Instance))
		{
			BroadCastManager.Instance.BraodCastClear();
		}
		_runButton = null;
		base._ExitTree();
	}

	public void RunButtonPressed()
	{
		OnRunButtonPressed?.Invoke();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.RunButtonPressed && args.Count == 0)
		{
			RunButtonPressed();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.RunButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.startGUINode)
		{
			startGUINode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.changePresentBoxButtonNode)
		{
			changePresentBoxButtonNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.betPanelBack)
		{
			betPanelBack = VariantUtils.ConvertTo<Panel>(in value);
			return true;
		}
		if (name == PropertyName.betPanelNode)
		{
			betPanelNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.coinLabel)
		{
			coinLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._runButton)
		{
			_runButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.startGUINode)
		{
			value = VariantUtils.CreateFrom(in startGUINode);
			return true;
		}
		if (name == PropertyName.changePresentBoxButtonNode)
		{
			value = VariantUtils.CreateFrom(in changePresentBoxButtonNode);
			return true;
		}
		if (name == PropertyName.betPanelBack)
		{
			value = VariantUtils.CreateFrom(in betPanelBack);
			return true;
		}
		if (name == PropertyName.betPanelNode)
		{
			value = VariantUtils.CreateFrom(in betPanelNode);
			return true;
		}
		if (name == PropertyName.coinLabel)
		{
			value = VariantUtils.CreateFrom(in coinLabel);
			return true;
		}
		if (name == PropertyName._runButton)
		{
			value = VariantUtils.CreateFrom(in _runButton);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.startGUINode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.changePresentBoxButtonNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.betPanelBack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.betPanelNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.coinLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.startGUINode, Variant.From(in startGUINode));
		info.AddProperty(PropertyName.changePresentBoxButtonNode, Variant.From(in changePresentBoxButtonNode));
		info.AddProperty(PropertyName.betPanelBack, Variant.From(in betPanelBack));
		info.AddProperty(PropertyName.betPanelNode, Variant.From(in betPanelNode));
		info.AddProperty(PropertyName.coinLabel, Variant.From(in coinLabel));
		info.AddProperty(PropertyName._runButton, Variant.From(in _runButton));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.startGUINode, out var value))
		{
			startGUINode = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.changePresentBoxButtonNode, out var value2))
		{
			changePresentBoxButtonNode = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.betPanelBack, out var value3))
		{
			betPanelBack = value3.As<Panel>();
		}
		if (info.TryGetProperty(PropertyName.betPanelNode, out var value4))
		{
			betPanelNode = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.coinLabel, out var value5))
		{
			coinLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._runButton, out var value6))
		{
			_runButton = value6.As<TextureButton>();
		}
	}
}
