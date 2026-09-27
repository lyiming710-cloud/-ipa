using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter9/DiceBomb/Scene/TowerDefensePlantDiceBomb.cs")]
public class TowerDefensePlantDiceBomb : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName OnExplodeHandler = "OnExplodeHandler";

		public static readonly StringName ScheduleNextExplode = "ScheduleNextExplode";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _rollResult = "_rollResult";

		public static readonly StringName _explodeCount = "_explodeCount";

		public static readonly StringName _destroyPending = "_destroyPending";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private ExplodeComponent _explodeComponent;

	private int _rollResult;

	private int _explodeCount;

	private bool _destroyPending;

	private const double ExplodeInterval = 0.35;

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint() || editorPreviewMode)
		{
			return;
		}
		_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
		if (_explodeComponent != null)
		{
			_explodeComponent.OnExplode += OnExplodeHandler;
			_rollResult = GD.RandRange(1, 6);
			string text = $"Explode{_rollResult}";
			if (sprite != null && sprite.HasClip(text))
			{
				_explodeComponent.explodeAnimeClips = text;
			}
		}
	}

	public override void _ExitTree()
	{
		ExplodeComponent explodeComponent = _explodeComponent;
		if (explodeComponent != null && !explodeComponent.IsReleased)
		{
			_explodeComponent.OnExplode -= OnExplodeHandler;
		}
		base._ExitTree();
	}

	private void OnExplodeHandler()
	{
		_explodeCount++;
		if (_explodeCount >= _rollResult)
		{
			if (!_destroyPending)
			{
				_destroyPending = true;
				Destroy();
			}
		}
		else
		{
			ScheduleNextExplode();
		}
	}

	private void ScheduleNextExplode()
	{
		SceneTree tree = GetTree();
		if (tree == null)
		{
			return;
		}
		tree.CreateTimer(0.35, processAlways: false).Timeout += () =>
		{
			ExplodeComponent explodeComponent = _explodeComponent;
			if (explodeComponent != null && !explodeComponent.IsReleased && !die)
			{
				_explodeComponent.Explode();
			}
		};
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["rollResult"] = _rollResult,
			["explodeCount"] = _explodeCount
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		_rollResult = data.GetValueOrDefault("rollResult", 1).AsInt32();
		if (_rollResult < 1)
		{
			_rollResult = 1;
		}
		_explodeCount = data.GetValueOrDefault("explodeCount", 0).AsInt32();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnExplodeHandler, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleNextExplode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.OnExplodeHandler && args.Count == 0)
		{
			OnExplodeHandler();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleNextExplode && args.Count == 0)
		{
			ScheduleNextExplode();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName.OnExplodeHandler)
		{
			return true;
		}
		if (method == MethodName.ScheduleNextExplode)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._rollResult)
		{
			_rollResult = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._explodeCount)
		{
			_explodeCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._destroyPending)
		{
			_destroyPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._rollResult)
		{
			value = VariantUtils.CreateFrom(in _rollResult);
			return true;
		}
		if (name == PropertyName._explodeCount)
		{
			value = VariantUtils.CreateFrom(in _explodeCount);
			return true;
		}
		if (name == PropertyName._destroyPending)
		{
			value = VariantUtils.CreateFrom(in _destroyPending);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._rollResult, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._explodeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._destroyPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._rollResult, Variant.From(in _rollResult));
		info.AddProperty(PropertyName._explodeCount, Variant.From(in _explodeCount));
		info.AddProperty(PropertyName._destroyPending, Variant.From(in _destroyPending));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._rollResult, out var value))
		{
			_rollResult = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._explodeCount, out var value2))
		{
			_explodeCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._destroyPending, out var value3))
		{
			_destroyPending = value3.As<bool>();
		}
	}
}
