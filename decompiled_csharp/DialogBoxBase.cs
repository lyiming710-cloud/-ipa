using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/DialogManager/Base/DialogBoxBase.cs")]
public class DialogBoxBase : Control
{
	public delegate void CloseEventHandler();

	public new class MethodName : Control.MethodName
	{
		public static readonly StringName Init = "Init";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName DragInput = "DragInput";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName ChildClose = "ChildClose";

		public static readonly StringName CloseDialog = "CloseDialog";

		public static readonly StringName DialogCreate = "DialogCreate";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName pasue = "pasue";

		public static readonly StringName aliveProcessMode = "aliveProcessMode";

		public static readonly StringName dragControl = "dragControl";

		public static readonly StringName isRoot = "isRoot";

		public static readonly StringName savePauseState = "savePauseState";

		public static readonly StringName saveTimeScale = "saveTimeScale";

		public static readonly StringName dragStart = "dragStart";

		public static readonly StringName isInDrag = "isInDrag";

		public static readonly StringName savePos = "savePos";

		public static readonly StringName saveMousePos = "saveMousePos";
	}

	public new class SignalName : Control.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool pasue;

	[Export(PropertyHint.None, "")]
	public ProcessModeEnum aliveProcessMode = ProcessModeEnum.Pausable;

	[Export(PropertyHint.None, "")]
	public Control dragControl;

	public bool isRoot;

	public bool savePauseState;

	public double saveTimeScale = 1.0;

	public bool dragStart;

	public bool isInDrag;

	public Vector2 savePos = Vector2.Zero;

	public Vector2 saveMousePos = Vector2.Zero;

	public event CloseEventHandler OnClose;

	public async Task WaitForClose()
	{
		TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
		OnClose += Handler;
		try
		{
			await tcs.Task;
		}
		finally
		{
			OnClose -= Handler;
		}
		void Handler()
		{
			tcs.TrySetResult(result: true);
		}
	}

	public virtual void Init(Dictionary data)
	{
	}

	public override void _Ready()
	{
		saveTimeScale = Engine.TimeScale;
		Engine.TimeScale = 1.0;
		if (pasue)
		{
			savePauseState = GetTree().Paused;
			ProcessMode = ProcessModeEnum.Always;
			GetTree().Paused = true;
		}
		if (dragControl != null)
		{
			dragControl.GuiInput += DragInput;
			dragControl.MouseEntered += () =>
			{
				isInDrag = true;
			};
			dragControl.MouseExited += () =>
			{
				isInDrag = false;
			};
		}
	}

	public void DragInput(InputEvent _event)
	{
		if (isInDrag)
		{
			if (Input.IsActionJustPressed("Press"))
			{
				dragStart = true;
				savePos = dragControl.GlobalPosition;
				saveMousePos = GetGlobalMousePosition();
			}
			if (Input.IsActionJustReleased("Press"))
			{
				dragStart = false;
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (dragStart)
		{
			dragControl.GlobalPosition = savePos + (GetGlobalMousePosition() - saveMousePos);
		}
	}

	public void ChildClose()
	{
		ProcessMode = aliveProcessMode;
	}

	public void CloseDialog()
	{
		Engine.TimeScale = saveTimeScale;
		if (pasue)
		{
			GetTree().Paused = savePauseState;
		}
		OnClose?.Invoke();
		QueueFree();
	}

	public DialogBoxBase DialogCreate(string dialogName, Dictionary data = null)
	{
		if (data == null)
		{
			data = new Dictionary();
		}
		PackedScene dialogScene = DialogManager.GetDialogScene(dialogName);
		if (dialogScene == null)
		{
			return null;
		}
		DialogBoxBase dialogBoxBase = (DialogBoxBase)dialogScene.Instantiate(PackedScene.GenEditState.Disabled);
		dialogBoxBase.OnClose += ChildClose;
		dialogBoxBase.Init(data);
		AddChild(dialogBoxBase, forceReadableName: false, InternalMode.Disabled);
		dialogBoxBase.GlobalPosition = Vector2.Zero;
		return dialogBoxBase;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DragInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChildClose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloseDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DialogCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dialogName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.DragInput && args.Count == 1)
		{
			DragInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChildClose && args.Count == 0)
		{
			ChildClose();
			ret = default;
			return true;
		}
		if (method == MethodName.CloseDialog && args.Count == 0)
		{
			CloseDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.DialogCreate && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<DialogBoxBase>(DialogCreate(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.DragInput)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.ChildClose)
		{
			return true;
		}
		if (method == MethodName.CloseDialog)
		{
			return true;
		}
		if (method == MethodName.DialogCreate)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.pasue)
		{
			pasue = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.aliveProcessMode)
		{
			aliveProcessMode = VariantUtils.ConvertTo<ProcessModeEnum>(in value);
			return true;
		}
		if (name == PropertyName.dragControl)
		{
			dragControl = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.isRoot)
		{
			isRoot = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.savePauseState)
		{
			savePauseState = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.saveTimeScale)
		{
			saveTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dragStart)
		{
			dragStart = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isInDrag)
		{
			isInDrag = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.savePos)
		{
			savePos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.saveMousePos)
		{
			saveMousePos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.pasue)
		{
			value = VariantUtils.CreateFrom(in pasue);
			return true;
		}
		if (name == PropertyName.aliveProcessMode)
		{
			value = VariantUtils.CreateFrom(in aliveProcessMode);
			return true;
		}
		if (name == PropertyName.dragControl)
		{
			value = VariantUtils.CreateFrom(in dragControl);
			return true;
		}
		if (name == PropertyName.isRoot)
		{
			value = VariantUtils.CreateFrom(in isRoot);
			return true;
		}
		if (name == PropertyName.savePauseState)
		{
			value = VariantUtils.CreateFrom(in savePauseState);
			return true;
		}
		if (name == PropertyName.saveTimeScale)
		{
			value = VariantUtils.CreateFrom(in saveTimeScale);
			return true;
		}
		if (name == PropertyName.dragStart)
		{
			value = VariantUtils.CreateFrom(in dragStart);
			return true;
		}
		if (name == PropertyName.isInDrag)
		{
			value = VariantUtils.CreateFrom(in isInDrag);
			return true;
		}
		if (name == PropertyName.savePos)
		{
			value = VariantUtils.CreateFrom(in savePos);
			return true;
		}
		if (name == PropertyName.saveMousePos)
		{
			value = VariantUtils.CreateFrom(in saveMousePos);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.pasue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.aliveProcessMode, PropertyHint.Enum, "Inherit,Pausable,WhenPaused,Always,Disabled", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.dragControl, PropertyHint.NodeType, "Control", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.savePauseState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.saveTimeScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.dragStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isInDrag, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.savePos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.saveMousePos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.pasue, Variant.From(in pasue));
		info.AddProperty(PropertyName.aliveProcessMode, Variant.From(in aliveProcessMode));
		info.AddProperty(PropertyName.dragControl, Variant.From(in dragControl));
		info.AddProperty(PropertyName.isRoot, Variant.From(in isRoot));
		info.AddProperty(PropertyName.savePauseState, Variant.From(in savePauseState));
		info.AddProperty(PropertyName.saveTimeScale, Variant.From(in saveTimeScale));
		info.AddProperty(PropertyName.dragStart, Variant.From(in dragStart));
		info.AddProperty(PropertyName.isInDrag, Variant.From(in isInDrag));
		info.AddProperty(PropertyName.savePos, Variant.From(in savePos));
		info.AddProperty(PropertyName.saveMousePos, Variant.From(in saveMousePos));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.pasue, out var value))
		{
			pasue = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.aliveProcessMode, out var value2))
		{
			aliveProcessMode = value2.As<ProcessModeEnum>();
		}
		if (info.TryGetProperty(PropertyName.dragControl, out var value3))
		{
			dragControl = value3.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.isRoot, out var value4))
		{
			isRoot = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.savePauseState, out var value5))
		{
			savePauseState = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.saveTimeScale, out var value6))
		{
			saveTimeScale = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dragStart, out var value7))
		{
			dragStart = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isInDrag, out var value8))
		{
			isInDrag = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.savePos, out var value9))
		{
			savePos = value9.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.saveMousePos, out var value10))
		{
			saveMousePos = value10.As<Vector2>();
		}
	}
}
