using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PacketBank/PacketBank/PacketGroup/PacketGroupButton.cs")]
public class PacketGroupButton : NinePatchButtonBase
{
	public delegate void SaveGroupEventHandler(int _id);

	public delegate void LoadGroupEventHandler(int _id);

	public new class MethodName : NinePatchButtonBase.MethodName
	{
		public new static readonly StringName OnReady = "OnReady";

		public static readonly StringName LoadSavedName = "LoadSavedName";

		public static readonly StringName SaveButtonPressed = "SaveButtonPressed";

		public static readonly StringName LoadButtonPressed = "LoadButtonPressed";

		public static readonly StringName OnLoadDelayTimeout = "OnLoadDelayTimeout";

		public static readonly StringName ClearLoadDelay = "ClearLoadDelay";

		public static readonly StringName NameChangeOver = "NameChangeOver";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : NinePatchButtonBase.PropertyName
	{
		public static readonly StringName nameChangeLine = "nameChangeLine";

		public static readonly StringName id = "id";

		public static readonly StringName pressSave = "pressSave";

		public static readonly StringName _loadButton = "_loadButton";

		public static readonly StringName _saveButton = "_saveButton";

		public static readonly StringName _loadDelayTimer = "_loadDelayTimer";
	}

	public new class SignalName : NinePatchButtonBase.SignalName
	{
	}

	public LineEdit nameChangeLine;

	[Export(PropertyHint.None, "")]
	public int id = 1;

	public bool pressSave;

	private TextureButton _loadButton;

	private TextureButton _saveButton;

	private SceneTreeTimer _loadDelayTimer;

	private Action _loadDelayHandler;

	public event SaveGroupEventHandler OnSaveGroup;

	public event LoadGroupEventHandler OnLoadGroup;

	public override void OnReady()
	{
		nameChangeLine = GetNode<LineEdit>("%NameChangeLine");
		_loadButton = GetNode<TextureButton>("%TextureButton");
		_saveButton = GetNode<TextureButton>("%SaveButton");
		_loadButton.Pressed += LoadButtonPressed;
		_saveButton.Pressed += SaveButtonPressed;
		nameChangeLine.EditingToggled += NameChangeOver;
		if (!Engine.IsEditorHint())
		{
			Callable.From(LoadSavedName).CallDeferred();
		}
	}

	private void LoadSavedName()
	{
		if (IsInsideTree() && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			if (TowerDefenseManager.Instance.IsIZMMode() || TowerDefenseManager.Instance.IsIZM2Mode())
			{
				labelText.Text = GameSaveManager.Instance.GetKeyValue($"ZombiePacketGroupName{id}").AsString();
			}
			else
			{
				labelText.Text = GameSaveManager.Instance.GetKeyValue($"PacketGroupName{id}").AsString();
			}
		}
	}

	public void SaveButtonPressed()
	{
		OnSaveGroup?.Invoke(id);
	}

	public void LoadButtonPressed()
	{
		if (!pressSave)
		{
			pressSave = true;
			ClearLoadDelay();
			_loadDelayTimer = GetTree().CreateTimer(0.2, processAlways: false);
			_loadDelayHandler = OnLoadDelayTimeout;
			_loadDelayTimer.Timeout += _loadDelayHandler;
		}
		else
		{
			pressSave = false;
			ClearLoadDelay();
			labelText.Visible = false;
			nameChangeLine.Visible = true;
			nameChangeLine.Text = labelText.Text;
			nameChangeLine.GrabFocus();
		}
	}

	private void OnLoadDelayTimeout()
	{
		ClearLoadDelay();
		if (IsInsideTree() && pressSave)
		{
			OnLoadGroup?.Invoke(id);
			pressSave = false;
		}
	}

	private void ClearLoadDelay()
	{
		if (GodotObject.IsInstanceValid(_loadDelayTimer) && _loadDelayHandler != null)
		{
			_loadDelayTimer.Timeout -= _loadDelayHandler;
		}
		_loadDelayTimer = null;
		_loadDelayHandler = null;
	}

	public void NameChangeOver(bool toggledOn)
	{
		if (!toggledOn)
		{
			labelText.Text = nameChangeLine.Text;
			if (TowerDefenseManager.Instance.IsIZMMode() || TowerDefenseManager.Instance.IsIZM2Mode())
			{
				GameSaveManager.Instance.SetKeyValue($"ZombiePacketGroupName{id}", labelText.Text);
			}
			else
			{
				GameSaveManager.Instance.SetKeyValue($"PacketGroupName{id}", labelText.Text);
			}
			GameSaveManager.Instance.Save();
			labelText.Visible = true;
			nameChangeLine.Visible = false;
		}
	}

	public override void _ExitTree()
	{
		ClearLoadDelay();
		pressSave = false;
		if (GodotObject.IsInstanceValid(_loadButton))
		{
			_loadButton.Pressed -= LoadButtonPressed;
		}
		if (GodotObject.IsInstanceValid(_saveButton))
		{
			_saveButton.Pressed -= SaveButtonPressed;
		}
		if (GodotObject.IsInstanceValid(nameChangeLine))
		{
			nameChangeLine.EditingToggled -= NameChangeOver;
		}
		OnSaveGroup = null;
		OnLoadGroup = null;
		_loadButton = null;
		_saveButton = null;
		nameChangeLine = null;
		base._ExitTree();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadSavedName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnLoadDelayTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearLoadDelay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NameChangeOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.OnReady && args.Count == 0)
		{
			OnReady();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadSavedName && args.Count == 0)
		{
			LoadSavedName();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveButtonPressed && args.Count == 0)
		{
			SaveButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadButtonPressed && args.Count == 0)
		{
			LoadButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnLoadDelayTimeout && args.Count == 0)
		{
			OnLoadDelayTimeout();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearLoadDelay && args.Count == 0)
		{
			ClearLoadDelay();
			ret = default;
			return true;
		}
		if (method == MethodName.NameChangeOver && args.Count == 1)
		{
			NameChangeOver(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.OnReady)
		{
			return true;
		}
		if (method == MethodName.LoadSavedName)
		{
			return true;
		}
		if (method == MethodName.SaveButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LoadButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnLoadDelayTimeout)
		{
			return true;
		}
		if (method == MethodName.ClearLoadDelay)
		{
			return true;
		}
		if (method == MethodName.NameChangeOver)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.nameChangeLine)
		{
			nameChangeLine = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName.id)
		{
			id = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.pressSave)
		{
			pressSave = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._loadButton)
		{
			_loadButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName._saveButton)
		{
			_saveButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName._loadDelayTimer)
		{
			_loadDelayTimer = VariantUtils.ConvertTo<SceneTreeTimer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.nameChangeLine)
		{
			value = VariantUtils.CreateFrom(in nameChangeLine);
			return true;
		}
		if (name == PropertyName.id)
		{
			value = VariantUtils.CreateFrom(in id);
			return true;
		}
		if (name == PropertyName.pressSave)
		{
			value = VariantUtils.CreateFrom(in pressSave);
			return true;
		}
		if (name == PropertyName._loadButton)
		{
			value = VariantUtils.CreateFrom(in _loadButton);
			return true;
		}
		if (name == PropertyName._saveButton)
		{
			value = VariantUtils.CreateFrom(in _saveButton);
			return true;
		}
		if (name == PropertyName._loadDelayTimer)
		{
			value = VariantUtils.CreateFrom(in _loadDelayTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.nameChangeLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.id, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pressSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadDelayTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.nameChangeLine, Variant.From(in nameChangeLine));
		info.AddProperty(PropertyName.id, Variant.From(in id));
		info.AddProperty(PropertyName.pressSave, Variant.From(in pressSave));
		info.AddProperty(PropertyName._loadButton, Variant.From(in _loadButton));
		info.AddProperty(PropertyName._saveButton, Variant.From(in _saveButton));
		info.AddProperty(PropertyName._loadDelayTimer, Variant.From(in _loadDelayTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.nameChangeLine, out var value))
		{
			nameChangeLine = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName.id, out var value2))
		{
			id = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.pressSave, out var value3))
		{
			pressSave = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._loadButton, out var value4))
		{
			_loadButton = value4.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName._saveButton, out var value5))
		{
			_saveButton = value5.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName._loadDelayTimer, out var value6))
		{
			_loadDelayTimer = value6.As<SceneTreeTimer>();
		}
	}
}
