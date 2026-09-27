using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Map/Control/TowerDefenseMapControl.cs")]
public class TowerDefenseMapControl : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName SetCharacterCanvasModulate = "SetCharacterCanvasModulate";

		public static readonly StringName SubscribeGameConfig = "SubscribeGameConfig";

		public static readonly StringName OnConfigValueChanged = "OnConfigValueChanged";

		public static readonly StringName RefreshMapEffectVisibility = "RefreshMapEffectVisibility";

		public static readonly StringName ApplyMapEffectVisibility = "ApplyMapEffectVisibility";

		public static readonly StringName UpdateCanvasModulate = "UpdateCanvasModulate";

		public static readonly StringName ConvertMapWorldToSpriteNodePosition = "ConvertMapWorldToSpriteNodePosition";

		public static readonly StringName PositionMapOverlayAtWorldPoint = "PositionMapOverlayAtWorldPoint";

		public static readonly StringName IsConfirmInput = "IsConfirmInput";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName canvasModulate = "canvasModulate";

		public static readonly StringName spriteNode = "spriteNode";

		public static readonly StringName mapNode = "mapNode";

		public static readonly StringName mapIceCap = "mapIceCap";

		public static readonly StringName changeMapLayer = "changeMapLayer";

		public static readonly StringName canvasModulateCharacter = "canvasModulateCharacter";

		public static readonly StringName editorSprite = "editorSprite";

		public static readonly StringName mapFeature = "mapFeature";

		public static readonly StringName _gameSaveManager = "_gameSaveManager";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	public CanvasModulate canvasModulate;

	public Node2D spriteNode;

	public Node2D mapNode;

	public Node2D mapIceCap;

	public CanvasLayer changeMapLayer;

	[Export(PropertyHint.None, "")]
	public CanvasModulate canvasModulateCharacter;

	public Sprite2D editorSprite;

	public TowerDefenseBattleFeatureMap mapFeature;

	private GameSaveManager _gameSaveManager;

	public override void _Ready()
	{
		canvasModulate = GetNode<CanvasModulate>("%CanvasModulate");
		spriteNode = GetNode<Node2D>("%SpriteNode");
		mapNode = GetNode<Node2D>("%MapNode");
		mapIceCap = GetNode<Node2D>("%MapIceCap");
		changeMapLayer = GetNode<CanvasLayer>("%ChangeMapLayer");
		SubscribeGameConfig();
		RefreshMapEffectVisibility();
		if (Global.Instance.isEditor && SceneManager.Instance.currentScene == "LevelEditorStage")
		{
			spriteNode.Scale = Vector2.One * 0.85f;
		}
		foreach (Node child in mapNode.GetChildren())
		{
			child.QueueFree();
		}
	}

	public override void _PhysicsProcess(double _delta)
	{
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance.currentControl) && !TowerDefenseManager.Instance.currentControl.isGameRunning && !TowerDefenseManager.Instance.currentControl.isGameFail)
		{
			mapFeature.ProcessInput();
		}
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(_gameSaveManager))
		{
			_gameSaveManager.ConfigValueChanged -= OnConfigValueChanged;
		}
		_gameSaveManager = null;
	}

	public void SetCharacterCanvasModulate(CanvasModulate value)
	{
		canvasModulateCharacter = value;
		SubscribeGameConfig();
		RefreshMapEffectVisibility();
	}

	private void SubscribeGameConfig()
	{
		GameSaveManager instance = GameSaveManager.Instance;
		if (_gameSaveManager != instance)
		{
			if (GodotObject.IsInstanceValid(_gameSaveManager))
			{
				_gameSaveManager.ConfigValueChanged -= OnConfigValueChanged;
			}
			_gameSaveManager = instance;
			if (GodotObject.IsInstanceValid(_gameSaveManager))
			{
				_gameSaveManager.ConfigValueChanged += OnConfigValueChanged;
			}
		}
	}

	private void OnConfigValueChanged(string key, Variant value)
	{
		if (string.Equals(key, "MapEffect", StringComparison.Ordinal))
		{
			ApplyMapEffectVisibility(value.AsBool());
		}
	}

	private void RefreshMapEffectVisibility()
	{
		bool visible = !GodotObject.IsInstanceValid(_gameSaveManager) || _gameSaveManager.GetConfigValue("MapEffect").AsBool();
		ApplyMapEffectVisibility(visible);
	}

	private void ApplyMapEffectVisibility(bool visible)
	{
		if (GodotObject.IsInstanceValid(canvasModulate))
		{
			canvasModulate.Visible = visible;
		}
		if (GodotObject.IsInstanceValid(canvasModulateCharacter))
		{
			canvasModulateCharacter.Visible = visible;
		}
	}

	public void UpdateCanvasModulate(double _delta)
	{
		if (GodotObject.IsInstanceValid(canvasModulate) && GodotObject.IsInstanceValid(mapFeature) && mapFeature.isChange && GodotObject.IsInstanceValid(mapFeature.currentGradient) && GodotObject.IsInstanceValid(mapFeature.currentGradient.Gradient))
		{
			canvasModulate.Color = mapFeature.currentGradient.Gradient.Sample((float)mapFeature.currentGradientPos);
			if (GodotObject.IsInstanceValid(canvasModulateCharacter))
			{
				canvasModulateCharacter.Color = canvasModulate.Color;
			}
		}
	}

	public Vector2 ConvertMapWorldToSpriteNodePosition(Vector2 mapWorldPosition, Vector2 screenOffset)
	{
		if (!GodotObject.IsInstanceValid(spriteNode))
		{
			return Vector2.Zero;
		}
		Vector2 vector = GetCanvasTransform() * mapWorldPosition + screenOffset;
		return spriteNode.GetGlobalTransformWithCanvas().AffineInverse() * vector;
	}

	public void PositionMapOverlayAtWorldPoint(Node2D overlay, Vector2 mapWorldPosition, Vector2 screenOffset)
	{
		if (GodotObject.IsInstanceValid(overlay))
		{
			overlay.Position = ConvertMapWorldToSpriteNodePosition(mapWorldPosition, screenOffset);
		}
	}

	public bool IsConfirmInput()
	{
		if (Global.Instance.isEditor && SceneManager.CurrentScene == "LevelEditorStage" && GodotObject.IsInstanceValid(LevelEditorMapEditor.instance) && LevelEditorMapEditor.instance.ShouldSuppressPlacementConfirm)
		{
			return false;
		}
		if (Global.Instance.isMobile)
		{
			return Input.IsActionJustReleased("Press");
		}
		return Input.IsActionJustPressed("Press");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCharacterCanvasModulate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "value", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CanvasModulate"), exported: false)
			}, null),
			new MethodInfo(MethodName.SubscribeGameConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnConfigValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshMapEffectVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyMapEffectVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCanvasModulate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConvertMapWorldToSpriteNodePosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "mapWorldPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "screenOffset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PositionMapOverlayAtWorldPoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "overlay", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mapWorldPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "screenOffset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsConfirmInput, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.SetCharacterCanvasModulate && args.Count == 1)
		{
			SetCharacterCanvasModulate(VariantUtils.ConvertTo<CanvasModulate>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SubscribeGameConfig && args.Count == 0)
		{
			SubscribeGameConfig();
			ret = default;
			return true;
		}
		if (method == MethodName.OnConfigValueChanged && args.Count == 2)
		{
			OnConfigValueChanged(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshMapEffectVisibility && args.Count == 0)
		{
			RefreshMapEffectVisibility();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMapEffectVisibility && args.Count == 1)
		{
			ApplyMapEffectVisibility(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCanvasModulate && args.Count == 1)
		{
			UpdateCanvasModulate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConvertMapWorldToSpriteNodePosition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ConvertMapWorldToSpriteNodePosition(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.PositionMapOverlayAtWorldPoint && args.Count == 3)
		{
			PositionMapOverlayAtWorldPoint(VariantUtils.ConvertTo<Node2D>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsConfirmInput && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsConfirmInput());
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.SetCharacterCanvasModulate)
		{
			return true;
		}
		if (method == MethodName.SubscribeGameConfig)
		{
			return true;
		}
		if (method == MethodName.OnConfigValueChanged)
		{
			return true;
		}
		if (method == MethodName.RefreshMapEffectVisibility)
		{
			return true;
		}
		if (method == MethodName.ApplyMapEffectVisibility)
		{
			return true;
		}
		if (method == MethodName.UpdateCanvasModulate)
		{
			return true;
		}
		if (method == MethodName.ConvertMapWorldToSpriteNodePosition)
		{
			return true;
		}
		if (method == MethodName.PositionMapOverlayAtWorldPoint)
		{
			return true;
		}
		if (method == MethodName.IsConfirmInput)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.canvasModulate)
		{
			canvasModulate = VariantUtils.ConvertTo<CanvasModulate>(in value);
			return true;
		}
		if (name == PropertyName.spriteNode)
		{
			spriteNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.mapNode)
		{
			mapNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.mapIceCap)
		{
			mapIceCap = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.changeMapLayer)
		{
			changeMapLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.canvasModulateCharacter)
		{
			canvasModulateCharacter = VariantUtils.ConvertTo<CanvasModulate>(in value);
			return true;
		}
		if (name == PropertyName.editorSprite)
		{
			editorSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName._gameSaveManager)
		{
			_gameSaveManager = VariantUtils.ConvertTo<GameSaveManager>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.canvasModulate)
		{
			value = VariantUtils.CreateFrom(in canvasModulate);
			return true;
		}
		if (name == PropertyName.spriteNode)
		{
			value = VariantUtils.CreateFrom(in spriteNode);
			return true;
		}
		if (name == PropertyName.mapNode)
		{
			value = VariantUtils.CreateFrom(in mapNode);
			return true;
		}
		if (name == PropertyName.mapIceCap)
		{
			value = VariantUtils.CreateFrom(in mapIceCap);
			return true;
		}
		if (name == PropertyName.changeMapLayer)
		{
			value = VariantUtils.CreateFrom(in changeMapLayer);
			return true;
		}
		if (name == PropertyName.canvasModulateCharacter)
		{
			value = VariantUtils.CreateFrom(in canvasModulateCharacter);
			return true;
		}
		if (name == PropertyName.editorSprite)
		{
			value = VariantUtils.CreateFrom(in editorSprite);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			value = VariantUtils.CreateFrom(in mapFeature);
			return true;
		}
		if (name == PropertyName._gameSaveManager)
		{
			value = VariantUtils.CreateFrom(in _gameSaveManager);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.canvasModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.spriteNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapIceCap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.changeMapLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.canvasModulateCharacter, PropertyHint.NodeType, "CanvasModulate", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.editorSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gameSaveManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.canvasModulate, Variant.From(in canvasModulate));
		info.AddProperty(PropertyName.spriteNode, Variant.From(in spriteNode));
		info.AddProperty(PropertyName.mapNode, Variant.From(in mapNode));
		info.AddProperty(PropertyName.mapIceCap, Variant.From(in mapIceCap));
		info.AddProperty(PropertyName.changeMapLayer, Variant.From(in changeMapLayer));
		info.AddProperty(PropertyName.canvasModulateCharacter, Variant.From(in canvasModulateCharacter));
		info.AddProperty(PropertyName.editorSprite, Variant.From(in editorSprite));
		info.AddProperty(PropertyName.mapFeature, Variant.From(in mapFeature));
		info.AddProperty(PropertyName._gameSaveManager, Variant.From(in _gameSaveManager));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.canvasModulate, out var value))
		{
			canvasModulate = value.As<CanvasModulate>();
		}
		if (info.TryGetProperty(PropertyName.spriteNode, out var value2))
		{
			spriteNode = value2.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.mapNode, out var value3))
		{
			mapNode = value3.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.mapIceCap, out var value4))
		{
			mapIceCap = value4.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.changeMapLayer, out var value5))
		{
			changeMapLayer = value5.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.canvasModulateCharacter, out var value6))
		{
			canvasModulateCharacter = value6.As<CanvasModulate>();
		}
		if (info.TryGetProperty(PropertyName.editorSprite, out var value7))
		{
			editorSprite = value7.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.mapFeature, out var value8))
		{
			mapFeature = value8.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName._gameSaveManager, out var value9))
		{
			_gameSaveManager = value9.As<GameSaveManager>();
		}
	}
}
