using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter1/Firenut/Scene/TowerDefensePlantBowlingFirenut.cs")]
public class TowerDefensePlantBowlingFirenut : TowerDefensePlantBowlingBase, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefensePlantBowlingBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName ApplyNetworkDamagePoint = "ApplyNetworkDamagePoint";

		public static readonly StringName ApplySpawnDamagePoint = "ApplySpawnDamagePoint";
	}

	public new class PropertyName : TowerDefensePlantBowlingBase.PropertyName
	{
		public static readonly StringName _spawnDamagePoint = "_spawnDamagePoint";
	}

	public new class SignalName : TowerDefensePlantBowlingBase.SignalName
	{
	}

	private string _spawnDamagePoint = "";

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint())
		{
			return;
		}
		if (config.customData != null)
		{
			string text = GameSaveManager.Instance.GetTowerDefensePacketValue("PlantFirenut").GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary()
				.GetValueOrDefault("Custom", "")
				.AsString();
			if (text != "")
			{
				currentCustom = new Array<string> { text };
			}
		}
		ApplySpawnDamagePoint();
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		ApplyNetworkDamagePoint(data);
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		return new Dictionary { ["damagePoint"] = _spawnDamagePoint };
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		ApplyNetworkDamagePoint(data);
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return (!(_spawnDamagePoint == "")) ? 1 : 0;
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { ["spawnDamagePoint"] = _spawnDamagePoint };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		_spawnDamagePoint = data?.GetValueOrDefault("spawnDamagePoint", _spawnDamagePoint).AsString() ?? _spawnDamagePoint;
		if (IsNodeReady())
		{
			ApplySpawnDamagePoint();
		}
	}

	private void ApplyNetworkDamagePoint(Dictionary data)
	{
		_spawnDamagePoint = data?.GetValueOrDefault("damagePoint", "").AsString() ?? "";
		if (IsNodeReady())
		{
			ApplySpawnDamagePoint();
		}
	}

	private void ApplySpawnDamagePoint()
	{
		if (!(_spawnDamagePoint == "") && GodotObject.IsInstanceValid(config?.damagePointData) && GodotObject.IsInstanceValid(sprite))
		{
			config.damagePointData.SetDamagePointFliters(sprite, _spawnDamagePoint);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyNetworkDamagePoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySpawnDamagePoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpecialState());
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState && args.Count == 1)
		{
			ImportNetworkSpecialState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNetworkSpecialStateRevision());
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
		if (method == MethodName.ApplyNetworkDamagePoint && args.Count == 1)
		{
			ApplyNetworkDamagePoint(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySpawnDamagePoint && args.Count == 0)
		{
			ApplySpawnDamagePoint();
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
		if (method == MethodName.ImportNetworkSpawnState)
		{
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision)
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
		if (method == MethodName.ApplyNetworkDamagePoint)
		{
			return true;
		}
		if (method == MethodName.ApplySpawnDamagePoint)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._spawnDamagePoint)
		{
			_spawnDamagePoint = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._spawnDamagePoint)
		{
			value = VariantUtils.CreateFrom(in _spawnDamagePoint);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._spawnDamagePoint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._spawnDamagePoint, Variant.From(in _spawnDamagePoint));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._spawnDamagePoint, out var value))
		{
			_spawnDamagePoint = value.As<string>();
		}
	}
}
