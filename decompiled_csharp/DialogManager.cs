using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/DialogManager/DialogManager.cs")]
public class DialogManager : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName DialogCreate = "DialogCreate";

		public static readonly StringName GetDialogScene = "GetDialogScene";

		public static readonly StringName Clear = "Clear";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _dialogLayer = "_dialogLayer";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private CanvasLayer _dialogLayer;

	private static readonly System.Collections.Generic.Dictionary<string, PackedScene> _dialogSceneCache = new System.Collections.Generic.Dictionary<string, PackedScene>();

	public static readonly Dictionary DIALOGS = new Dictionary
	{
		{ "NewVersion", "uid://byl6sdrqwtna6" },
		{ "BattlePause", "uid://dfo6k1agbut6f" },
		{ "MainMenuOption", "uid://b0npiop54wclo" },
		{ "ReStart", "uid://cv6y0meevfp1m" },
		{ "BattleOption", "uid://dtstbivduflfa" },
		{ "BattleFail", "uid://vjoytf5ho72h" },
		{ "Pause", "uid://bqcytklobbjc5" },
		{ "DeleteUser", "uid://b7ugvdt5744hb" },
		{ "ExitGame", "uid://2ruypbgb1o60" },
		{ "NewUser", "uid://fluy240kgqxt" },
		{ "RenameUser", "uid://cinxg1baricor" },
		{ "User", "uid://bl4axhkknocdw" },
		{ "DifficultWarning", "uid://b3ihu8d0tlfdj" },
		{ "ShopSale", "uid://7uohxkgjtfq1" },
		{ "ShopCantSale", "uid://dh52k7mi2sqxe" },
		{ "Help", "uid://bb57xq2pya6qc" },
		{ "Almanac", "uid://54lyi8ygt8gb" },
		{ "Shop", "uid://dj1q7rgc2viko" },
		{ "TryLevel", "uid://bqgqsw4x8hpsc" },
		{ "StarExchange", "uid://wipun8o16gvw" },
		{ "DailyChallenge", "uid://rpjdq2sskhcc" },
		{ "DailyChallengeLevelChoose", "uid://kh5l2bpv5cij" },
		{ "DailyChallengeLevelAward", "uid://vsl60p6vfwq8" },
		{ "LevelEditorTips", "uid://bq3p568k734sc" },
		{ "LevelEditorNewLevel", "uid://dla7fmvvo2eyt" },
		{ "MyLevelDelete", "uid://dlbvaubvob4fa" },
		{ "DiyLevelChange", "uid://dpqscpnd01xpe" },
		{ "OnlineLevelExchange", "uid://dne6cdlf3sqv0" },
		{ "OnlineLevelPreview", "uid://7s2pv78sxbll" },
		{ "DialogBoxTips", "uid://b5okjyouryjct" },
		{ "DialogBoxChoose", "uid://cux0wbsftgq37" },
		{ "DialogBoxInput", "uid://bm70be8qeyqip" },
		{ "MultiplayerLobby", "res://Prefab/GUI/DialogBox/DialogBoxGeneral/DialogBoxMultiplayerLobby/DialogBoxMultiplayerLobby.tscn" }
	};

	public static DialogManager Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
		_dialogLayer = GetNode<CanvasLayer>("%DialogLayer");
		SceneManager.Instance.OnSceneChange += Clear;
	}

	public DialogBoxBase DialogCreate(string dialogName, Dictionary data = null)
	{
		if (data == null)
		{
			data = new Dictionary();
		}
		PackedScene dialogScene = GetDialogScene(dialogName);
		if (dialogScene == null)
		{
			return null;
		}
		DialogBoxBase dialogBoxBase = (DialogBoxBase)dialogScene.Instantiate(PackedScene.GenEditState.Disabled);
		dialogBoxBase.Init(data);
		_dialogLayer.AddChild(dialogBoxBase, forceReadableName: false, InternalMode.Disabled);
		dialogBoxBase.GlobalPosition = Vector2.Zero;
		return dialogBoxBase;
	}

	public static PackedScene GetDialogScene(string dialogName)
	{
		if (string.IsNullOrEmpty(dialogName))
		{
			return null;
		}
		if (_dialogSceneCache.TryGetValue(dialogName, out var value))
		{
			return value;
		}
		if (!DIALOGS.ContainsKey(dialogName))
		{
			GD.PushError("Dialog scene not registered: " + dialogName);
			return null;
		}
		string text = ProjectResourceUidCache.ResolveResourcePath(DIALOGS[dialogName].AsString());
		PackedScene packedScene = GD.Load<PackedScene>(text);
		if (packedScene == null)
		{
			GD.PushError("Dialog scene load failed: " + dialogName + " -> " + text);
			return null;
		}
		_dialogSceneCache[dialogName] = packedScene;
		return packedScene;
	}

	public void Clear(string sceneName)
	{
		foreach (Node child in _dialogLayer.GetChildren())
		{
			child.QueueFree();
		}
		GetTree().Paused = false;
	}

	public DialogManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/DialogManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DialogCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dialogName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDialogScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dialogName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sceneName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.DialogCreate && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<DialogBoxBase>(DialogCreate(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1])));
			return true;
		}
		if (method == MethodName.GetDialogScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetDialogScene(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Clear && args.Count == 1)
		{
			Clear(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetDialogScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetDialogScene(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.DialogCreate)
		{
			return true;
		}
		if (method == MethodName.GetDialogScene)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._dialogLayer)
		{
			_dialogLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._dialogLayer)
		{
			value = VariantUtils.CreateFrom(in _dialogLayer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._dialogLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._dialogLayer, Variant.From(in _dialogLayer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._dialogLayer, out var value))
		{
			_dialogLayer = value.As<CanvasLayer>();
		}
	}
}
