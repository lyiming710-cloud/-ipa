using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Mower/Manager/TowerDefenseMowerManager.cs")]
public class TowerDefenseMowerManager : Control
{
	public new class MethodName : Control.MethodName
	{
		public static readonly StringName GetMowerPreviewPosition = "GetMowerPreviewPosition";

		public static readonly StringName NeedsInputProcessing = "NeedsInputProcessing";

		public static readonly StringName ProcessMowerInput = "ProcessMowerInput";

		public static readonly StringName MovePreviewTo = "MovePreviewTo";

		public static readonly StringName DisposeBattleState = "DisposeBattleState";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName mowerFeature = "mowerFeature";

		public static readonly StringName config = "config";

		public static readonly StringName _isDraggingPreview = "_isDraggingPreview";

		public static readonly StringName _previewSprite = "_previewSprite";

		public static readonly StringName _previewGridPosition = "_previewGridPosition";

		public static readonly StringName _previewTween = "_previewTween";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public TowerDefenseBattleFeatureMower mowerFeature;

	public TowerDefenseBattleFeatureMowerConfig config;

	private bool _isDraggingPreview;

	private AdobeAnimateSprite _previewSprite;

	private Vector2I _previewGridPosition;

	private Tween _previewTween;

	private Vector2 GetMowerPreviewPosition(Vector2I gridPos)
	{
		double num = 0.0;
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
		if (GodotObject.IsInstanceValid(mapCell))
		{
			num = mapCell.GetGroundHeight(0.0);
		}
		double num2 = config?.previewGroundOffset ?? 10.0;
		return TowerDefenseManager.GetMapCellPlantPos(gridPos) - new Vector2(0f, (float)(num + num2));
	}

	public bool NeedsInputProcessing()
	{
		if (_isDraggingPreview || Input.IsActionJustPressed("Press") || Input.IsActionJustReleased("Press"))
		{
			return true;
		}
		if (!GodotObject.IsInstanceValid(_previewSprite))
		{
			return false;
		}
		if (!Input.IsActionJustPressed("P1Up") && !Input.IsActionJustPressed("P1Down") && !Input.IsActionJustPressed("P1Left"))
		{
			return Input.IsActionJustPressed("P1Right");
		}
		return true;
	}

	public void ProcessMowerInput(TowerDefenseCellInstance cell, Vector2I gridPos)
	{
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (!GodotObject.IsInstanceValid(mapFeature))
		{
			return;
		}
		if (Input.IsActionJustPressed("Press") && GodotObject.IsInstanceValid(cell) && cell.CanMowerMove())
		{
			if (!GodotObject.IsInstanceValid(_previewSprite))
			{
				_previewSprite = TowerDefenseManager.GetCharacterSprite(config?.previewSpriteName ?? "MowerDefault");
				if (!GodotObject.IsInstanceValid(_previewSprite))
				{
					return;
				}
				_previewSprite.GlobalPosition = GetMowerPreviewPosition(gridPos);
				_previewSprite.Scale = Vector2.One * (float)(config?.previewScale ?? 1.0);
				Node2D characterNode = TowerDefenseManager.GetCharacterNode();
				if (!GodotObject.IsInstanceValid(characterNode))
				{
					_previewSprite.QueueFree();
					_previewSprite = null;
					return;
				}
				characterNode.AddChild(_previewSprite, forceReadableName: false, InternalMode.Disabled);
			}
			MovePreviewTo(gridPos);
			_isDraggingPreview = true;
		}
		if (Input.IsActionJustReleased("Press"))
		{
			_isDraggingPreview = false;
		}
		if (_isDraggingPreview && GodotObject.IsInstanceValid(cell) && gridPos - _previewGridPosition != Vector2I.Zero)
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(_previewGridPosition);
			if (GodotObject.IsInstanceValid(mapCell) && mapCell.CanMoveToCell(cell))
			{
				MovePreviewTo(gridPos);
				mapCell.MoveToCell(cell);
			}
		}
		if (!GodotObject.IsInstanceValid(_previewSprite))
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		bool flag = false;
		if (_previewGridPosition.Y - 1 >= 1 && Input.IsActionJustPressed("P1Up"))
		{
			num2--;
			flag = true;
		}
		if (_previewGridPosition.Y + 1 <= mapFeature.config.gridNum.Y && Input.IsActionJustPressed("P1Down"))
		{
			num2++;
			flag = true;
		}
		if (_previewGridPosition.X - 1 >= 1 && Input.IsActionJustPressed("P1Left"))
		{
			num--;
			flag = true;
		}
		if (_previewGridPosition.X + 1 <= mapFeature.config.gridNum.X && Input.IsActionJustPressed("P1Right"))
		{
			num++;
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		Vector2I vector2I = new Vector2I(num, num2);
		if (vector2I != Vector2I.Zero)
		{
			TowerDefenseCellInstance mapCell2 = TowerDefenseManager.GetMapCell(_previewGridPosition);
			TowerDefenseCellInstance mapCell3 = TowerDefenseManager.GetMapCell(_previewGridPosition + vector2I);
			if (GodotObject.IsInstanceValid(mapCell2) && GodotObject.IsInstanceValid(mapCell3) && mapCell2.CanMoveToCell(mapCell3))
			{
				MovePreviewTo(_previewGridPosition + vector2I);
				mapCell2.MoveToCell(mapCell3);
			}
		}
	}

	private void MovePreviewTo(Vector2I gridPosition)
	{
		if (GodotObject.IsInstanceValid(_previewSprite))
		{
			_previewTween?.Kill();
			_previewTween = CreateTween();
			_previewTween.SetEase(Tween.EaseType.Out);
			_previewTween.SetTrans(Tween.TransitionType.Quart);
			_previewTween.TweenProperty(_previewSprite, "global_position", GetMowerPreviewPosition(gridPosition), config?.previewTweenDuration ?? 0.5);
			_previewGridPosition = gridPosition;
			_previewSprite.ZIndex = gridPosition.Y * 15 + 1;
		}
	}

	public void DisposeBattleState()
	{
		_previewTween?.Kill();
		_previewTween = null;
		if (GodotObject.IsInstanceValid(_previewSprite))
		{
			_previewSprite.QueueFree();
		}
		_previewSprite = null;
		_previewGridPosition = Vector2I.Zero;
		_isDraggingPreview = false;
		mowerFeature = null;
		config = null;
	}

	public override void _ExitTree()
	{
		DisposeBattleState();
		base._ExitTree();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.GetMowerPreviewPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NeedsInputProcessing, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessMowerInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MovePreviewTo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeBattleState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetMowerPreviewPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetMowerPreviewPosition(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.NeedsInputProcessing && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(NeedsInputProcessing());
			return true;
		}
		if (method == MethodName.ProcessMowerInput && args.Count == 2)
		{
			ProcessMowerInput(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MovePreviewTo && args.Count == 1)
		{
			MovePreviewTo(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeBattleState && args.Count == 0)
		{
			DisposeBattleState();
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
		if (method == MethodName.GetMowerPreviewPosition)
		{
			return true;
		}
		if (method == MethodName.NeedsInputProcessing)
		{
			return true;
		}
		if (method == MethodName.ProcessMowerInput)
		{
			return true;
		}
		if (method == MethodName.MovePreviewTo)
		{
			return true;
		}
		if (method == MethodName.DisposeBattleState)
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
		if (name == PropertyName.mowerFeature)
		{
			mowerFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMower>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMowerConfig>(in value);
			return true;
		}
		if (name == PropertyName._isDraggingPreview)
		{
			_isDraggingPreview = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._previewSprite)
		{
			_previewSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._previewGridPosition)
		{
			_previewGridPosition = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._previewTween)
		{
			_previewTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.mowerFeature)
		{
			value = VariantUtils.CreateFrom(in mowerFeature);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName._isDraggingPreview)
		{
			value = VariantUtils.CreateFrom(in _isDraggingPreview);
			return true;
		}
		if (name == PropertyName._previewSprite)
		{
			value = VariantUtils.CreateFrom(in _previewSprite);
			return true;
		}
		if (name == PropertyName._previewGridPosition)
		{
			value = VariantUtils.CreateFrom(in _previewGridPosition);
			return true;
		}
		if (name == PropertyName._previewTween)
		{
			value = VariantUtils.CreateFrom(in _previewTween);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.mowerFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isDraggingPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._previewGridPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mowerFeature, Variant.From(in mowerFeature));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName._isDraggingPreview, Variant.From(in _isDraggingPreview));
		info.AddProperty(PropertyName._previewSprite, Variant.From(in _previewSprite));
		info.AddProperty(PropertyName._previewGridPosition, Variant.From(in _previewGridPosition));
		info.AddProperty(PropertyName._previewTween, Variant.From(in _previewTween));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mowerFeature, out var value))
		{
			mowerFeature = value.As<TowerDefenseBattleFeatureMower>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseBattleFeatureMowerConfig>();
		}
		if (info.TryGetProperty(PropertyName._isDraggingPreview, out var value3))
		{
			_isDraggingPreview = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._previewSprite, out var value4))
		{
			_previewSprite = value4.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._previewGridPosition, out var value5))
		{
			_previewGridPosition = value5.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._previewTween, out var value6))
		{
			_previewTween = value6.As<Tween>();
		}
	}
}
