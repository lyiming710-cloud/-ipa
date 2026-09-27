using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter6/ChomperPot/Scene/TowerDefensePlantChomperPot.cs")]
public class TowerDefensePlantChomperPot : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ChewProcessing = "ChewProcessing";

		public static readonly StringName Teleport = "Teleport";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName chewTime = "chewTime";

		public static readonly StringName _chewTime = "_chewTime";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	public AttackComponent attackComponent;

	public AttackComponent attackComponent2;

	public ChomperComponent chomperComponent;

	private double _chewTime = 30.0;

	[Export(PropertyHint.None, "")]
	public double chewTime
	{
		get
		{
			return _chewTime;
		}
		set
		{
			_chewTime = value;
			if (IsNodeReady() && chomperComponent != null && !chomperComponent.IsReleased)
			{
				chomperComponent.chewTime = (float)value;
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
			chomperComponent = componentManager.GetRuntime<ChomperComponent>();
			attackComponent.SetCheckAreaRectangleWidth(0, TowerDefenseManager.Instance.GetMapGridSize().X * 1.25f);
			AddToGroup("ChomperPot");
			if (chomperComponent != null && !chomperComponent.IsReleased)
			{
				chomperComponent.OnChewProcessing += ChewProcessing;
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (chomperComponent != null && !chomperComponent.IsReleased)
		{
			chomperComponent.OnChewProcessing -= ChewProcessing;
		}
	}

	public virtual void ChewProcessing(double delta)
	{
		Teleport();
	}

	public virtual void Teleport()
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		ChomperComponent chomperComponent = this.chomperComponent;
		if (chomperComponent == null || chomperComponent.IsReleased || !this.chomperComponent.isChew)
		{
			return;
		}
		AttackComponent attackComponent = attackComponent2;
		if (attackComponent == null || attackComponent.IsReleased || !attackComponent2.CanAttackOnContact())
		{
			return;
		}
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>(attackComponent2.GetTargetList());
		if (list.Count == 0)
		{
			return;
		}
		List<TowerDefenseCharacter> list2 = new List<TowerDefenseCharacter>();
		foreach (Node item in GetTree().GetNodesInGroup("ChomperPot"))
		{
			if (item is TowerDefensePlantChomperPot towerDefensePlantChomperPot && towerDefensePlantChomperPot != this && !towerDefensePlantChomperPot.die && !towerDefensePlantChomperPot.isDestroy && !towerDefensePlantChomperPot.IsQueuedForDeletion())
			{
				ChomperComponent chomperComponent2 = towerDefensePlantChomperPot.chomperComponent;
				if (chomperComponent2 != null && !chomperComponent2.IsReleased && towerDefensePlantChomperPot.chomperComponent.isChew)
				{
					list2.Add(towerDefensePlantChomperPot);
				}
			}
		}
		if (list2.Count == 0)
		{
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter = list2[GD.RandRange(0, list2.Count - 1)];
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		Vector2 logicalGlobalPosition2 = towerDefenseCharacter.GetLogicalGlobalPosition();
		foreach (TowerDefenseCharacter item2 in list)
		{
			if (item2 is TowerDefenseZombie && item2.instance.zombiePhysique <= TowerDefenseEnum.ZOMBIE_PHYSIQUE.NORMAL)
			{
				Vector2 logicalGlobalPosition3 = item2.GetLogicalGlobalPosition();
				if (!(logicalGlobalPosition3.X < logicalGlobalPosition.X - 10f))
				{
					double num = (0f - item2.Scale.X) * TowerDefenseManager.Instance.GetMapGridSize().X * 0.5f;
					item2.shadowComponent.saveShadowPosition.Y += logicalGlobalPosition2.Y - logicalGlobalPosition3.Y;
					item2.SetLogicalGlobalPosition(logicalGlobalPosition2 + new Vector2((float)num, 0f));
					item2.gridPos = towerDefenseCharacter.gridPos;
				}
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "chewTime", chewTime } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		chewTime = data.GetValueOrDefault("chewTime", 30.0).AsDouble();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ChewProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Teleport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.ChewProcessing && args.Count == 1)
		{
			ChewProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Teleport && args.Count == 0)
		{
			Teleport();
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
		if (method == MethodName.ChewProcessing)
		{
			return true;
		}
		if (method == MethodName.Teleport)
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
		if (name == PropertyName.chewTime)
		{
			chewTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._chewTime)
		{
			_chewTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.chewTime)
		{
			value = VariantUtils.CreateFrom<double>(chewTime);
			return true;
		}
		if (name == PropertyName._chewTime)
		{
			value = VariantUtils.CreateFrom(in _chewTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName._chewTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.chewTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.chewTime, Variant.From<double>(chewTime));
		info.AddProperty(PropertyName._chewTime, Variant.From(in _chewTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.chewTime, out var value))
		{
			chewTime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._chewTime, out var value2))
		{
			_chewTime = value2.As<double>();
		}
	}
}
