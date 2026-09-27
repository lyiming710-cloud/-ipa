using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/BroadCastManager/BroadCastManager.cs")]
public class BroadCastManager : Node
{
	public delegate void BroadCastOverEventHandler();

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadFontAfterFirstDraw = "LoadFontAfterFirstDraw";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName BroadCastAdd = "BroadCastAdd";

		public static readonly StringName BroadCastTimerTimeOut = "BroadCastTimerTimeOut";

		public static readonly StringName Next = "Next";

		public static readonly StringName BraodCastClear = "BraodCastClear";

		public static readonly StringName BroadCastFloatCreate = "BroadCastFloatCreate";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName editorPreviewMode = "editorPreviewMode";

		public static readonly StringName broad = "broad";

		public static readonly StringName broadCastLabel = "broadCastLabel";

		public static readonly StringName broadCastTimer = "broadCastTimer";

		public static readonly StringName floatBroaderNode = "floatBroaderNode";

		public static readonly StringName stay = "stay";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string BroadcastFontPath = "res://Asset/Font/fzkt.ttf";

	[Export(PropertyHint.None, "")]
	public bool editorPreviewMode;

	private static PackedScene _broadCastFloatScene;

	public Control broad;

	public Label broadCastLabel;

	public Timer broadCastTimer;

	public Control floatBroaderNode;

	public List<BroadCastConfig> broadCastList = new List<BroadCastConfig>();

	public bool stay;

	public static BroadCastManager Instance { get; private set; }

	private static PackedScene BroadCastFloatScene => _broadCastFloatScene ?? (_broadCastFloatScene = GD.Load<PackedScene>("uid://c43r0mavkmqxl"));

	public event BroadCastOverEventHandler OnBroadCastOver;

	public override void _Ready()
	{
		if (!editorPreviewMode)
		{
			Instance = this;
		}
		broad = GetNode<Control>("%Broad");
		broadCastLabel = GetNode<Label>("%BroadCastLabel");
		broadCastTimer = GetNode<Timer>("%BroadCastTimer");
		floatBroaderNode = GetNode<Control>("%FloatBroaderNode");
		SetPhysicsProcess(enable: false);
		if (!editorPreviewMode && GodotObject.IsInstanceValid(SceneManager.Instance))
		{
			SceneManager.Instance.OnSceneChange += (string sceneName) =>
			{
				BraodCastClear();
			};
		}
		GetNode<Timer>("%BroadCastTimer").Timeout += BroadCastTimerTimeOut;
		LoadFontAfterFirstDraw();
	}

	private async void LoadFontAfterFirstDraw()
	{
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		if (IsInsideTree() && GodotObject.IsInstanceValid(broadCastLabel))
		{
			StartupLoadDiagnostics.Mark("deferred.broadcast_font.begin");
			FontFile fontFile = GD.Load<FontFile>("res://Asset/Font/fzkt.ttf");
			if (GodotObject.IsInstanceValid(fontFile))
			{
				broadCastLabel.AddThemeFontOverride("font", fontFile);
			}
			StartupLoadDiagnostics.Mark("deferred.broadcast_font.end");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (broadCastList.Count > 0)
		{
			if (!stay && broadCastTimer.IsStopped())
			{
				broadCastLabel.Text = broadCastList[0].broadCastString;
				if (broadCastList[0].broadCastTime != -1.0)
				{
					broadCastTimer.Start(broadCastList[0].broadCastTime);
				}
				else
				{
					stay = true;
				}
				broad.Visible = broadCastList.Count > 0;
				broadCastList.RemoveAt(0);
			}
		}
		else
		{
			SetPhysicsProcess(enable: false);
		}
	}

	public void BroadCastAdd(BroadCastConfig config)
	{
		if (!broadCastList.Contains(config))
		{
			broadCastList.Add(config);
			SetPhysicsProcess(enable: true);
		}
	}

	public void BroadCastTimerTimeOut()
	{
		stay = false;
		OnBroadCastOver?.Invoke();
		if (broadCastList.Count == 0)
		{
			broadCastLabel.Text = "";
			broad.Visible = false;
		}
	}

	public void Next()
	{
		stay = false;
		broadCastTimer.Stop();
		if (broadCastList.Count > 0)
		{
			broadCastList.RemoveAt(0);
		}
	}

	public void BraodCastClear()
	{
		stay = false;
		broadCastLabel.Text = "";
		broadCastTimer.Stop();
		broadCastList.Clear();
		broad.Visible = false;
	}

	public void BroadCastFloatCreate(string text, Color color = default(Color))
	{
		if (color == default(Color))
		{
			color = Colors.White;
		}
		BroadCastFloat broadCastFloat = BroadCastFloatScene.Instantiate<BroadCastFloat>(PackedScene.GenEditState.Disabled);
		floatBroaderNode.AddChild(broadCastFloat, forceReadableName: false, InternalMode.Disabled);
		broadCastFloat.Init(text, color);
	}

	public BroadCastManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/BroadCastManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFontAfterFirstDraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BroadCastAdd, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BroadCastTimerTimeOut, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Next, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BraodCastClear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BroadCastFloatCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
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
		if (method == MethodName.LoadFontAfterFirstDraw && args.Count == 0)
		{
			LoadFontAfterFirstDraw();
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BroadCastAdd && args.Count == 1)
		{
			BroadCastAdd(VariantUtils.ConvertTo<BroadCastConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BroadCastTimerTimeOut && args.Count == 0)
		{
			BroadCastTimerTimeOut();
			ret = default;
			return true;
		}
		if (method == MethodName.Next && args.Count == 0)
		{
			Next();
			ret = default;
			return true;
		}
		if (method == MethodName.BraodCastClear && args.Count == 0)
		{
			BraodCastClear();
			ret = default;
			return true;
		}
		if (method == MethodName.BroadCastFloatCreate && args.Count == 2)
		{
			BroadCastFloatCreate(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]));
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
		if (method == MethodName.LoadFontAfterFirstDraw)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.BroadCastAdd)
		{
			return true;
		}
		if (method == MethodName.BroadCastTimerTimeOut)
		{
			return true;
		}
		if (method == MethodName.Next)
		{
			return true;
		}
		if (method == MethodName.BraodCastClear)
		{
			return true;
		}
		if (method == MethodName.BroadCastFloatCreate)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.editorPreviewMode)
		{
			editorPreviewMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.broad)
		{
			broad = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.broadCastLabel)
		{
			broadCastLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.broadCastTimer)
		{
			broadCastTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName.floatBroaderNode)
		{
			floatBroaderNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.stay)
		{
			stay = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.editorPreviewMode)
		{
			value = VariantUtils.CreateFrom(in editorPreviewMode);
			return true;
		}
		if (name == PropertyName.broad)
		{
			value = VariantUtils.CreateFrom(in broad);
			return true;
		}
		if (name == PropertyName.broadCastLabel)
		{
			value = VariantUtils.CreateFrom(in broadCastLabel);
			return true;
		}
		if (name == PropertyName.broadCastTimer)
		{
			value = VariantUtils.CreateFrom(in broadCastTimer);
			return true;
		}
		if (name == PropertyName.floatBroaderNode)
		{
			value = VariantUtils.CreateFrom(in floatBroaderNode);
			return true;
		}
		if (name == PropertyName.stay)
		{
			value = VariantUtils.CreateFrom(in stay);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.editorPreviewMode, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.broad, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.broadCastLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.broadCastTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.floatBroaderNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.stay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.editorPreviewMode, Variant.From(in editorPreviewMode));
		info.AddProperty(PropertyName.broad, Variant.From(in broad));
		info.AddProperty(PropertyName.broadCastLabel, Variant.From(in broadCastLabel));
		info.AddProperty(PropertyName.broadCastTimer, Variant.From(in broadCastTimer));
		info.AddProperty(PropertyName.floatBroaderNode, Variant.From(in floatBroaderNode));
		info.AddProperty(PropertyName.stay, Variant.From(in stay));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.editorPreviewMode, out var value))
		{
			editorPreviewMode = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.broad, out var value2))
		{
			broad = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.broadCastLabel, out var value3))
		{
			broadCastLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.broadCastTimer, out var value4))
		{
			broadCastTimer = value4.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName.floatBroaderNode, out var value5))
		{
			floatBroaderNode = value5.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.stay, out var value6))
		{
			stay = value6.As<bool>();
		}
	}
}
