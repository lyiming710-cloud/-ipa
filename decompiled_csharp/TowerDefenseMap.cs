using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
public class TowerDefenseMap : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName SaveMapBase = "SaveMapBase";

		public static readonly StringName LoadMapBase = "LoadMapBase";

		public static readonly StringName ConfigureEnterRoomRevealNodes = "ConfigureEnterRoomRevealNodes";

		public static readonly StringName ConfigureShaderTimeNodes = "ConfigureShaderTimeNodes";

		public static readonly StringName ResolveConfiguredNodes = "ResolveConfiguredNodes";

		public static readonly StringName ApplyShaderTime = "ApplyShaderTime";

		public static readonly StringName CanExecuteEventFunction = "CanExecuteEventFunction";

		public static readonly StringName FunctionExecute = "FunctionExecute";

		public static readonly StringName WarnRejectedEventFunction = "WarnRejectedEventFunction";

		public static readonly StringName UseStripe = "UseStripe";

		public static readonly StringName ShowShovel = "ShowShovel";

		public static readonly StringName BackShovel = "BackShovel";

		public static readonly StringName ShowGlove = "ShowGlove";

		public static readonly StringName BackGlove = "BackGlove";

		public static readonly StringName CharacterClear = "CharacterClear";

		public static readonly StringName LineUseSet = "LineUseSet";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _shaderTime = "_shaderTime";

		public static readonly StringName stripe = "stripe";

		public static readonly StringName canvasModulateGradient = "canvasModulateGradient";

		public static readonly StringName enterRoomTargetY = "enterRoomTargetY";

		public static readonly StringName enterRoomMoveSpeed = "enterRoomMoveSpeed";

		public static readonly StringName enterRoomShadowOffsetY = "enterRoomShadowOffsetY";

		public static readonly StringName enterRoomCharacterSpeedMultiplier = "enterRoomCharacterSpeedMultiplier";

		public static readonly StringName enterRoomSettleDuration = "enterRoomSettleDuration";

		public static readonly StringName enterRoomRevealNodeNames = "enterRoomRevealNodeNames";

		public static readonly StringName shaderTimeNodeNames = "shaderTimeNodeNames";

		public static readonly StringName shaderTimeParameter = "shaderTimeParameter";

		public static readonly StringName waveSpeed = "waveSpeed";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const string RevealStateKey = "revealNodeVisibility";

	private const string ShaderTimeStateKey = "shaderTime";

	private const int MaxEventFunctionArguments = 10;

	private const int MaxRejectedEventFunctionWarnings = 64;

	private static readonly ConcurrentDictionary<(Type MapType, string FunctionName, int ArgumentCount), byte> RejectedEventFunctionWarnings = new ConcurrentDictionary<(Type, string, int), byte>();

	private IReadOnlyDictionary<string, int> _eventFunctions;

	private readonly List<CanvasItem> _enterRoomRevealNodes = new List<CanvasItem>();

	private readonly List<ShaderMaterial> _shaderTimeMaterials = new List<ShaderMaterial>();

	private double _shaderTime;

	[Export(PropertyHint.None, "")]
	public Node2D stripe;

	[Export(PropertyHint.None, "")]
	public GradientTexture1D canvasModulateGradient;

	[ExportGroup("Enter Room", "")]
	[Export(PropertyHint.None, "")]
	public double enterRoomTargetY = 375.0;

	[Export(PropertyHint.None, "")]
	public double enterRoomMoveSpeed = 200.0;

	[Export(PropertyHint.None, "")]
	public double enterRoomShadowOffsetY = 36.0;

	[Export(PropertyHint.None, "")]
	public double enterRoomCharacterSpeedMultiplier = 2.0;

	[Export(PropertyHint.None, "")]
	public double enterRoomSettleDuration = 3.0;

	[Export(PropertyHint.MultilineText, "")]
	public string enterRoomRevealNodeNames = "";

	[ExportGroup("Shader Time", "")]
	[Export(PropertyHint.MultilineText, "")]
	public string shaderTimeNodeNames = "";

	[Export(PropertyHint.None, "")]
	public StringName shaderTimeParameter = "timer";

	[Export(PropertyHint.None, "")]
	public double waveSpeed = 0.1;

	public override void _Ready()
	{
		ResolveConfiguredNodes();
		ApplyShaderTime();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_shaderTimeMaterials.Count != 0 && double.IsFinite(delta) && double.IsFinite(waveSpeed))
		{
			_shaderTime += delta * waveSpeed;
			if (!double.IsFinite(_shaderTime))
			{
				_shaderTime = 0.0;
			}
			ApplyShaderTime();
		}
	}

	public virtual async Task EnterRoom(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return;
		}
		double num = (double.IsFinite(enterRoomMoveSpeed) ? Math.Max(0.0, enterRoomMoveSpeed) : 0.0);
		Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
		double num2 = (double.IsFinite(enterRoomTargetY) ? enterRoomTargetY : ((double)logicalGlobalPosition.Y));
		double num3 = Math.Abs((double)logicalGlobalPosition.Y - num2);
		double duration = ((num > 0.0) ? (num3 / num) : 0.0);
		Vector2 vector = new Vector2(logicalGlobalPosition.X, (float)num2);
		Tween tween = CreateTween().SetParallel();
		tween.TweenMethod(Callable.From<Vector2>(character.SetLogicalGlobalPosition), logicalGlobalPosition, vector, duration);
		ShadowComponent shadowComponent = character.shadowComponent;
		if (shadowComponent != null && !shadowComponent.IsReleased)
		{
			double num4 = (double.IsFinite(enterRoomShadowOffsetY) ? enterRoomShadowOffsetY : 0.0);
			character.shadowComponent.TweenSaveShadowPositionY(tween, (float)(num2 + num4), duration);
		}
		await ToSignal(tween, Tween.SignalName.Finished);
		if (GodotObject.IsInstanceValid(character) && double.IsFinite(enterRoomCharacterSpeedMultiplier))
		{
			character.timeScaleInit *= enterRoomCharacterSpeedMultiplier;
		}
		foreach (CanvasItem enterRoomRevealNode in _enterRoomRevealNodes)
		{
			if (GodotObject.IsInstanceValid(enterRoomRevealNode))
			{
				enterRoomRevealNode.Visible = true;
			}
		}
		double num5 = (double.IsFinite(enterRoomSettleDuration) ? Math.Max(0.0, enterRoomSettleDuration) : 0.0);
		if (num5 > 0.0 && IsInsideTree())
		{
			await ToSignal(GetTree().CreateTimer(num5, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
	}

	public virtual Dictionary SaveMapBase()
	{
		Dictionary dictionary = new Dictionary();
		if (_enterRoomRevealNodes.Count > 0)
		{
			Dictionary dictionary2 = new Dictionary();
			foreach (CanvasItem enterRoomRevealNode in _enterRoomRevealNodes)
			{
				if (GodotObject.IsInstanceValid(enterRoomRevealNode))
				{
					dictionary2[GetPathTo(enterRoomRevealNode).ToString()] = enterRoomRevealNode.Visible;
				}
			}
			dictionary["revealNodeVisibility"] = dictionary2;
		}
		if (_shaderTimeMaterials.Count > 0)
		{
			dictionary["shaderTime"] = _shaderTime;
		}
		return dictionary;
	}

	public virtual void LoadMapBase(Dictionary data)
	{
		if (data == null)
		{
			return;
		}
		foreach (KeyValuePair<Variant, Variant> item in data.GetValueOrDefault("revealNodeVisibility", new Dictionary()).AsGodotDictionary())
		{
			CanvasItem nodeOrNull = GetNodeOrNull<CanvasItem>(item.Key.AsString());
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				nodeOrNull.Visible = item.Value.AsBool();
			}
		}
		double num = data.GetValueOrDefault("shaderTime", _shaderTime).AsDouble();
		_shaderTime = (double.IsFinite(num) ? num : 0.0);
		ApplyShaderTime();
	}

	public virtual bool CanSaveProgress(out string reason)
	{
		reason = "";
		return true;
	}

	protected void ConfigureEnterRoomRevealNodes(params string[] uniqueNodeNames)
	{
		enterRoomRevealNodeNames = JoinNodeNames(uniqueNodeNames);
	}

	protected void ConfigureShaderTimeNodes(params string[] uniqueNodeNames)
	{
		shaderTimeNodeNames = JoinNodeNames(uniqueNodeNames);
	}

	private void ResolveConfiguredNodes()
	{
		_enterRoomRevealNodes.Clear();
		_shaderTimeMaterials.Clear();
		foreach (string item2 in ParseNodeNames(enterRoomRevealNodeNames))
		{
			CanvasItem nodeOrNull = GetNodeOrNull<CanvasItem>(new NodePath("%" + item2));
			if (GodotObject.IsInstanceValid(nodeOrNull) && !_enterRoomRevealNodes.Contains(nodeOrNull))
			{
				_enterRoomRevealNodes.Add(nodeOrNull);
			}
		}
		foreach (string item3 in ParseNodeNames(shaderTimeNodeNames))
		{
			CanvasItem nodeOrNull2 = GetNodeOrNull<CanvasItem>(new NodePath("%" + item3));
			if (GodotObject.IsInstanceValid(nodeOrNull2) && nodeOrNull2.Material is ShaderMaterial item && !_shaderTimeMaterials.Contains(item))
			{
				_shaderTimeMaterials.Add(item);
			}
		}
	}

	private void ApplyShaderTime()
	{
		foreach (ShaderMaterial shaderTimeMaterial in _shaderTimeMaterials)
		{
			if (GodotObject.IsInstanceValid(shaderTimeMaterial))
			{
				shaderTimeMaterial.SetShaderParameter(shaderTimeParameter, _shaderTime);
			}
		}
	}

	private static IEnumerable<string> ParseNodeNames(string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			string[] array = value.Split(new char[4] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			for (int i = 0; i < array.Length; i++)
			{
				yield return array[i];
			}
		}
	}

	private static string JoinNodeNames(IEnumerable<string> names)
	{
		if (names != null)
		{
			return string.Join(",", names);
		}
		return "";
	}

	public bool CanExecuteEventFunction(string functionName, int argumentCount)
	{
		if (string.IsNullOrEmpty(functionName) || argumentCount < 0 || argumentCount > 10)
		{
			return false;
		}
		if (GetEventFunctions().TryGetValue(functionName, out var value))
		{
			return (value & (1 << argumentCount)) != 0;
		}
		return false;
	}

	public Variant FunctionExecute(string functionName, Godot.Collections.Array variable)
	{
		if (variable == null)
		{
			variable = new Godot.Collections.Array();
		}
		if (!CanExecuteEventFunction(functionName, variable.Count))
		{
			WarnRejectedEventFunction(functionName, variable.Count);
			return default;
		}
		return variable.Count switch
		{
			0 => Call(functionName), 
			1 => Call(functionName, variable[0]), 
			2 => Call(functionName, variable[0], variable[1]), 
			3 => Call(functionName, variable[0], variable[1], variable[2]), 
			4 => Call(functionName, variable[0], variable[1], variable[2], variable[3]), 
			5 => Call(functionName, variable[0], variable[1], variable[2], variable[3], variable[4]), 
			6 => Call(functionName, variable[0], variable[1], variable[2], variable[3], variable[4], variable[5]), 
			7 => Call(functionName, variable[0], variable[1], variable[2], variable[3], variable[4], variable[5], variable[6]), 
			8 => Call(functionName, variable[0], variable[1], variable[2], variable[3], variable[4], variable[5], variable[6], variable[7]), 
			9 => Call(functionName, variable[0], variable[1], variable[2], variable[3], variable[4], variable[5], variable[6], variable[7], variable[8]), 
			10 => Call(functionName, variable[0], variable[1], variable[2], variable[3], variable[4], variable[5], variable[6], variable[7], variable[8], variable[9]), 
			_ => default, 
		};
	}

	private IReadOnlyDictionary<string, int> GetEventFunctions()
	{
		if (_eventFunctions != null)
		{
			return _eventFunctions;
		}
		System.Collections.Generic.Dictionary<string, int> dictionary = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
		RegisterEventFunctions(dictionary);
		_eventFunctions = dictionary;
		return _eventFunctions;
	}

	protected virtual void RegisterEventFunctions(IDictionary<string, int> functions)
	{
		RegisterEventFunction(functions, "UseStripe", 1);
		RegisterEventFunction(functions, "ShowShovel", 0);
		RegisterEventFunction(functions, "BackShovel", 0);
		RegisterEventFunction(functions, "ShowGlove", 0);
		RegisterEventFunction(functions, "BackGlove", 0);
		RegisterEventFunction(functions, "CharacterClear", 0);
		RegisterEventFunction(functions, "LineUseSet", 2);
	}

	protected static void RegisterEventFunction(IDictionary<string, int> functions, string functionName, int minimumArgumentCount, int maximumArgumentCount = -1)
	{
		if (functions == null || string.IsNullOrEmpty(functionName))
		{
			throw new ArgumentException("Map event function registration requires a target and a name.");
		}
		if (maximumArgumentCount < 0)
		{
			maximumArgumentCount = minimumArgumentCount;
		}
		if (minimumArgumentCount < 0 || maximumArgumentCount < minimumArgumentCount || maximumArgumentCount > 10)
		{
			throw new ArgumentOutOfRangeException("minimumArgumentCount", $"Map event functions support 0..{10} arguments.");
		}
		functions.TryGetValue(functionName, out var value);
		for (int i = minimumArgumentCount; i <= maximumArgumentCount; i++)
		{
			value |= 1 << i;
		}
		functions[functionName] = value;
	}

	private void WarnRejectedEventFunction(string functionName, int argumentCount)
	{
		if (RejectedEventFunctionWarnings.Count < 64)
		{
			(Type, string, int) key = (GetType(), functionName ?? string.Empty, argumentCount);
			if (RejectedEventFunctionWarnings.TryAdd(key, 0))
			{
				GD.PushWarning($"[Map] Rejected unregistered event function '{functionName}' with {argumentCount} argument(s) on {GetType().Name}.");
			}
		}
	}

	public void UseStripe(int row)
	{
		stripe.Visible = true;
		TowerDefenseManager.GetMapFeature().stripeRow = row;
		stripe.GlobalPosition = new Vector2(TowerDefenseManager.Instance.GetMapCellPos(new Vector2I(row + 1, 1)).X - 10f, stripe.GlobalPosition.Y);
	}

	public void ShowShovel()
	{
		GlobalFeatureManager.Instance?.Unlock("Shovel");
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.shovelManager))
		{
			mapFeature.shovelManager.GlobalPosition = new Vector2(mapFeature.shovelManager.GlobalPosition.X, 0f);
			mapFeature.shovelManager.shovelShow = true;
		}
	}

	public void BackShovel()
	{
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.shovelManager))
		{
			mapFeature.shovelManager.shovelShow = false;
			mapFeature.shovelManager.Position = new Vector2(mapFeature.shovelManager.Position.X, 0f);
		}
	}

	public void ShowGlove()
	{
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.gloveManager))
		{
			mapFeature.gloveManager.GlobalPosition = new Vector2(mapFeature.gloveManager.GlobalPosition.X, 0f);
			mapFeature.gloveManager.gloveShow = true;
		}
	}

	public void BackGlove()
	{
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.gloveManager))
		{
			mapFeature.gloveManager.Position = new Vector2(mapFeature.gloveManager.Position.X, 0f);
		}
	}

	public void CharacterClear()
	{
		HashSet<ulong> destroyedInstanceIds = new HashSet<ulong>();
		foreach (Variant item in TowerDefenseManager.Instance.GetCharacter())
		{
			DestroyCharacterOnce(item.AsGodotObject() as TowerDefenseCharacter, destroyedInstanceIds);
		}
		SceneTree tree = TowerDefenseManager.Instance.GetTree();
		if (!GodotObject.IsInstanceValid(tree))
		{
			return;
		}
		foreach (Node item2 in tree.GetNodesInGroup("Gravestone"))
		{
			DestroyCharacterOnce(item2 as TowerDefenseGravestone, destroyedInstanceIds);
		}
	}

	private static void DestroyCharacterOnce(TowerDefenseCharacter character, HashSet<ulong> destroyedInstanceIds)
	{
		if (GodotObject.IsInstanceValid(character) && !character.isDestroy && destroyedInstanceIds.Add(character.GetInstanceId()))
		{
			character.ClearFromMap();
		}
	}

	public void LineUseSet(int line, bool open)
	{
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			mapFeature.SetLineUse(line, open);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveMapBase, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadMapBase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureEnterRoomRevealNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedStringArray, "uniqueNodeNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureShaderTimeNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedStringArray, "uniqueNodeNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveConfiguredNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyShaderTime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanExecuteEventFunction, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "functionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "argumentCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FunctionExecute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "functionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "variable", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WarnRejectedEventFunction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "functionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "argumentCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UseStripe, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "row", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowShovel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BackShovel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowGlove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BackGlove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CharacterClear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LineUseSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveMapBase && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveMapBase());
			return true;
		}
		if (method == MethodName.LoadMapBase && args.Count == 1)
		{
			LoadMapBase(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureEnterRoomRevealNodes && args.Count == 1)
		{
			ConfigureEnterRoomRevealNodes(VariantUtils.ConvertTo<string[]>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureShaderTimeNodes && args.Count == 1)
		{
			ConfigureShaderTimeNodes(VariantUtils.ConvertTo<string[]>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveConfiguredNodes && args.Count == 0)
		{
			ResolveConfiguredNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyShaderTime && args.Count == 0)
		{
			ApplyShaderTime();
			ret = default;
			return true;
		}
		if (method == MethodName.CanExecuteEventFunction && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanExecuteEventFunction(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.FunctionExecute && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(FunctionExecute(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[1])));
			return true;
		}
		if (method == MethodName.WarnRejectedEventFunction && args.Count == 2)
		{
			WarnRejectedEventFunction(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UseStripe && args.Count == 1)
		{
			UseStripe(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowShovel && args.Count == 0)
		{
			ShowShovel();
			ret = default;
			return true;
		}
		if (method == MethodName.BackShovel && args.Count == 0)
		{
			BackShovel();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowGlove && args.Count == 0)
		{
			ShowGlove();
			ret = default;
			return true;
		}
		if (method == MethodName.BackGlove && args.Count == 0)
		{
			BackGlove();
			ret = default;
			return true;
		}
		if (method == MethodName.CharacterClear && args.Count == 0)
		{
			CharacterClear();
			ret = default;
			return true;
		}
		if (method == MethodName.LineUseSet && args.Count == 2)
		{
			LineUseSet(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.SaveMapBase)
		{
			return true;
		}
		if (method == MethodName.LoadMapBase)
		{
			return true;
		}
		if (method == MethodName.ConfigureEnterRoomRevealNodes)
		{
			return true;
		}
		if (method == MethodName.ConfigureShaderTimeNodes)
		{
			return true;
		}
		if (method == MethodName.ResolveConfiguredNodes)
		{
			return true;
		}
		if (method == MethodName.ApplyShaderTime)
		{
			return true;
		}
		if (method == MethodName.CanExecuteEventFunction)
		{
			return true;
		}
		if (method == MethodName.FunctionExecute)
		{
			return true;
		}
		if (method == MethodName.WarnRejectedEventFunction)
		{
			return true;
		}
		if (method == MethodName.UseStripe)
		{
			return true;
		}
		if (method == MethodName.ShowShovel)
		{
			return true;
		}
		if (method == MethodName.BackShovel)
		{
			return true;
		}
		if (method == MethodName.ShowGlove)
		{
			return true;
		}
		if (method == MethodName.BackGlove)
		{
			return true;
		}
		if (method == MethodName.CharacterClear)
		{
			return true;
		}
		if (method == MethodName.LineUseSet)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._shaderTime)
		{
			_shaderTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.stripe)
		{
			stripe = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.canvasModulateGradient)
		{
			canvasModulateGradient = VariantUtils.ConvertTo<GradientTexture1D>(in value);
			return true;
		}
		if (name == PropertyName.enterRoomTargetY)
		{
			enterRoomTargetY = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.enterRoomMoveSpeed)
		{
			enterRoomMoveSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.enterRoomShadowOffsetY)
		{
			enterRoomShadowOffsetY = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.enterRoomCharacterSpeedMultiplier)
		{
			enterRoomCharacterSpeedMultiplier = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.enterRoomSettleDuration)
		{
			enterRoomSettleDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.enterRoomRevealNodeNames)
		{
			enterRoomRevealNodeNames = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.shaderTimeNodeNames)
		{
			shaderTimeNodeNames = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.shaderTimeParameter)
		{
			shaderTimeParameter = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.waveSpeed)
		{
			waveSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._shaderTime)
		{
			value = VariantUtils.CreateFrom(in _shaderTime);
			return true;
		}
		if (name == PropertyName.stripe)
		{
			value = VariantUtils.CreateFrom(in stripe);
			return true;
		}
		if (name == PropertyName.canvasModulateGradient)
		{
			value = VariantUtils.CreateFrom(in canvasModulateGradient);
			return true;
		}
		if (name == PropertyName.enterRoomTargetY)
		{
			value = VariantUtils.CreateFrom(in enterRoomTargetY);
			return true;
		}
		if (name == PropertyName.enterRoomMoveSpeed)
		{
			value = VariantUtils.CreateFrom(in enterRoomMoveSpeed);
			return true;
		}
		if (name == PropertyName.enterRoomShadowOffsetY)
		{
			value = VariantUtils.CreateFrom(in enterRoomShadowOffsetY);
			return true;
		}
		if (name == PropertyName.enterRoomCharacterSpeedMultiplier)
		{
			value = VariantUtils.CreateFrom(in enterRoomCharacterSpeedMultiplier);
			return true;
		}
		if (name == PropertyName.enterRoomSettleDuration)
		{
			value = VariantUtils.CreateFrom(in enterRoomSettleDuration);
			return true;
		}
		if (name == PropertyName.enterRoomRevealNodeNames)
		{
			value = VariantUtils.CreateFrom(in enterRoomRevealNodeNames);
			return true;
		}
		if (name == PropertyName.shaderTimeNodeNames)
		{
			value = VariantUtils.CreateFrom(in shaderTimeNodeNames);
			return true;
		}
		if (name == PropertyName.shaderTimeParameter)
		{
			value = VariantUtils.CreateFrom(in shaderTimeParameter);
			return true;
		}
		if (name == PropertyName.waveSpeed)
		{
			value = VariantUtils.CreateFrom(in waveSpeed);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName._shaderTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.stripe, PropertyHint.NodeType, "Node2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.canvasModulateGradient, PropertyHint.ResourceType, "GradientTexture1D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Enter Room", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.enterRoomTargetY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.enterRoomMoveSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.enterRoomShadowOffsetY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.enterRoomCharacterSpeedMultiplier, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.enterRoomSettleDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.enterRoomRevealNodeNames, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Shader Time", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.shaderTimeNodeNames, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.shaderTimeParameter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.waveSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._shaderTime, Variant.From(in _shaderTime));
		info.AddProperty(PropertyName.stripe, Variant.From(in stripe));
		info.AddProperty(PropertyName.canvasModulateGradient, Variant.From(in canvasModulateGradient));
		info.AddProperty(PropertyName.enterRoomTargetY, Variant.From(in enterRoomTargetY));
		info.AddProperty(PropertyName.enterRoomMoveSpeed, Variant.From(in enterRoomMoveSpeed));
		info.AddProperty(PropertyName.enterRoomShadowOffsetY, Variant.From(in enterRoomShadowOffsetY));
		info.AddProperty(PropertyName.enterRoomCharacterSpeedMultiplier, Variant.From(in enterRoomCharacterSpeedMultiplier));
		info.AddProperty(PropertyName.enterRoomSettleDuration, Variant.From(in enterRoomSettleDuration));
		info.AddProperty(PropertyName.enterRoomRevealNodeNames, Variant.From(in enterRoomRevealNodeNames));
		info.AddProperty(PropertyName.shaderTimeNodeNames, Variant.From(in shaderTimeNodeNames));
		info.AddProperty(PropertyName.shaderTimeParameter, Variant.From(in shaderTimeParameter));
		info.AddProperty(PropertyName.waveSpeed, Variant.From(in waveSpeed));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._shaderTime, out var value))
		{
			_shaderTime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.stripe, out var value2))
		{
			stripe = value2.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.canvasModulateGradient, out var value3))
		{
			canvasModulateGradient = value3.As<GradientTexture1D>();
		}
		if (info.TryGetProperty(PropertyName.enterRoomTargetY, out var value4))
		{
			enterRoomTargetY = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.enterRoomMoveSpeed, out var value5))
		{
			enterRoomMoveSpeed = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.enterRoomShadowOffsetY, out var value6))
		{
			enterRoomShadowOffsetY = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.enterRoomCharacterSpeedMultiplier, out var value7))
		{
			enterRoomCharacterSpeedMultiplier = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.enterRoomSettleDuration, out var value8))
		{
			enterRoomSettleDuration = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.enterRoomRevealNodeNames, out var value9))
		{
			enterRoomRevealNodeNames = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.shaderTimeNodeNames, out var value10))
		{
			shaderTimeNodeNames = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.shaderTimeParameter, out var value11))
		{
			shaderTimeParameter = value11.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.waveSpeed, out var value12))
		{
			waveSpeed = value12.As<double>();
		}
	}
}
