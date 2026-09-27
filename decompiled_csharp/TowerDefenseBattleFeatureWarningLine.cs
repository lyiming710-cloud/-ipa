using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/WarningLine/TowerDefenseBattleFeatureWarningLine.cs")]
public class TowerDefenseBattleFeatureWarningLine : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public static readonly StringName AddWarningLine = "AddWarningLine";

		public static readonly StringName AddWarningColumn = "AddWarningColumn";

		public new static readonly StringName Process = "Process";

		public static readonly StringName OnWarningLineTriggered = "OnWarningLineTriggered";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public static readonly StringName SerializeState = "SerializeState";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public static readonly StringName ApplyState = "ApplyState";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName ClearWarningLines = "ClearWarningLines";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName _triggered = "_triggered";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _warningLineScene;

	private readonly List<WarningLine> _warningLines = new List<WarningLine>();

	private bool _triggered;

	private static PackedScene WarningLineScene => _warningLineScene ?? (_warningLineScene = GD.Load<PackedScene>("uid://7e2ylijvno4n"));

	public void AddWarningLine(int row)
	{
		AddWarningColumn(row);
	}

	public void AddWarningColumn(int column)
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(instance) || !GodotObject.IsInstanceValid(characterNode) || !GodotObject.IsInstanceValid(WarningLineScene))
		{
			return;
		}
		int x = instance.GetMapGridNum().X;
		if (column < 0 || column >= x)
		{
			GD.PushWarning($"[WarningLine] Column {column} is outside the map range [0, {x}).");
			return;
		}
		foreach (WarningLine warningLine2 in _warningLines)
		{
			if (GodotObject.IsInstanceValid(warningLine2) && warningLine2.column == column)
			{
				return;
			}
		}
		WarningLine warningLine = WarningLineScene.Instantiate<WarningLine>(PackedScene.GenEditState.Disabled);
		characterNode.AddChild(warningLine, forceReadableName: false, Node.InternalMode.Disabled);
		warningLine.GlobalPosition = instance.GetMapCellPos(new Vector2I(column + 1, 1)) + new Vector2(-10f, 0f);
		Sprite2D node = warningLine.GetNode<Sprite2D>("%Sprite");
		Vector2 scale = node.Scale;
		scale.Y = (instance.GetMapGridSize().X * (float)instance.GetMapGridNum().X - 20f) / 502f;
		node.Scale = scale;
		warningLine.feature = this;
		warningLine.column = column;
		_warningLines.Add(warningLine);
	}

	public override void Process(double delta)
	{
		if (_triggered || (Global.IsMultiplayerMode && (MultiPlayerManager.Instance == null || !MultiPlayerManager.Instance.isHost)))
		{
			return;
		}
		for (int num = _warningLines.Count - 1; num >= 0; num--)
		{
			WarningLine warningLine = _warningLines[num];
			if (!GodotObject.IsInstanceValid(warningLine))
			{
				_warningLines.RemoveAt(num);
			}
			else
			{
				warningLine.UpdateOverlaps();
			}
		}
	}

	public void OnWarningLineTriggered()
	{
		if (_triggered)
		{
			return;
		}
		_triggered = true;
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(instance?.characterRegistry))
		{
			foreach (TowerDefenseCharacter cleanCharacters in instance.characterRegistry.GetCleanCharactersList())
			{
				cleanCharacters.ProcessMode = Node.ProcessModeEnum.Disabled;
			}
		}
		control.GameFail(null);
	}

	public override Dictionary SaveFeature()
	{
		return SerializeState();
	}

	public override Dictionary SyncSerialize()
	{
		return SerializeState();
	}

	private Dictionary SerializeState()
	{
		Array array = new Array();
		foreach (WarningLine warningLine in _warningLines)
		{
			if (GodotObject.IsInstanceValid(warningLine))
			{
				array.Add(warningLine.column);
			}
		}
		return new Dictionary
		{
			{ "columns", array },
			{ "triggered", _triggered }
		};
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		ApplyState(_data);
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		ApplyState(_data);
	}

	private void ApplyState(Dictionary state)
	{
		bool flag = state.GetValueOrDefault("triggered", false).AsBool();
		HashSet<int> hashSet = new HashSet<int>();
		if (!flag)
		{
			foreach (Variant item in state.GetValueOrDefault("columns", state.GetValueOrDefault("rows", new Array())).AsGodotArray())
			{
				hashSet.Add(item.AsInt32());
			}
		}
		for (int num = _warningLines.Count - 1; num >= 0; num--)
		{
			WarningLine warningLine = _warningLines[num];
			if (!GodotObject.IsInstanceValid(warningLine) || !hashSet.Contains(warningLine.column))
			{
				_warningLines.RemoveAt(num);
				if (GodotObject.IsInstanceValid(warningLine))
				{
					warningLine.feature = null;
					warningLine.QueueFree();
				}
			}
		}
		_triggered = flag;
		if (_triggered)
		{
			return;
		}
		foreach (int item2 in hashSet)
		{
			AddWarningColumn(item2);
		}
	}

	public override void Destroy()
	{
		ClearWarningLines();
		_triggered = false;
		base.Destroy();
	}

	private void ClearWarningLines()
	{
		foreach (WarningLine warningLine in _warningLines)
		{
			if (GodotObject.IsInstanceValid(warningLine))
			{
				warningLine.feature = null;
				warningLine.QueueFree();
			}
		}
		_warningLines.Clear();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.AddWarningLine, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "row", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddWarningColumn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnWarningLineTriggered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SerializeState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearWarningLines, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AddWarningLine && args.Count == 1)
		{
			AddWarningLine(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddWarningColumn && args.Count == 1)
		{
			AddWarningColumn(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnWarningLineTriggered && args.Count == 0)
		{
			OnWarningLineTriggered();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveFeature());
			return true;
		}
		if (method == MethodName.SyncSerialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SyncSerialize());
			return true;
		}
		if (method == MethodName.SerializeState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SerializeState());
			return true;
		}
		if (method == MethodName.LoadFeature && args.Count == 2)
		{
			LoadFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncDeserialize && args.Count == 1)
		{
			SyncDeserialize(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyState && args.Count == 1)
		{
			ApplyState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearWarningLines && args.Count == 0)
		{
			ClearWarningLines();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.AddWarningLine)
		{
			return true;
		}
		if (method == MethodName.AddWarningColumn)
		{
			return true;
		}
		if (method == MethodName.Process)
		{
			return true;
		}
		if (method == MethodName.OnWarningLineTriggered)
		{
			return true;
		}
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.SyncSerialize)
		{
			return true;
		}
		if (method == MethodName.SerializeState)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		if (method == MethodName.SyncDeserialize)
		{
			return true;
		}
		if (method == MethodName.ApplyState)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.ClearWarningLines)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._triggered)
		{
			_triggered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._triggered)
		{
			value = VariantUtils.CreateFrom(in _triggered);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._triggered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._triggered, Variant.From(in _triggered));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._triggered, out var value))
		{
			_triggered = value.As<bool>();
		}
	}
}
