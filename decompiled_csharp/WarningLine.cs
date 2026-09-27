using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Object/WarningLine.cs")]
public class WarningLine : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName UpdateOverlaps = "UpdateOverlaps";

		public static readonly StringName CheckFrontArea = "CheckFrontArea";

		public static readonly StringName CheckBackArea = "CheckBackArea";

		public static readonly StringName CheckCharacter = "CheckCharacter";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName row = "row";

		public static readonly StringName _sprite = "_sprite";

		public static readonly StringName _frontArea = "_frontArea";

		public static readonly StringName _backArea = "_backArea";

		public static readonly StringName feature = "feature";

		public static readonly StringName column = "column";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private Sprite2D _sprite;

	private AabbArea2D _frontArea;

	private AabbArea2D _backArea;

	private readonly HashSet<TowerDefenseCharacter> _frontOverlaps = new HashSet<TowerDefenseCharacter>();

	private readonly HashSet<TowerDefenseCharacter> _backOverlaps = new HashSet<TowerDefenseCharacter>();

	private readonly HashSet<TowerDefenseCharacter> _scratchOverlaps = new HashSet<TowerDefenseCharacter>();

	public TowerDefenseBattleFeatureWarningLine feature;

	public int column;

	public int row
	{
		get
		{
			return column;
		}
		set
		{
			column = value;
		}
	}

	public override void _Ready()
	{
		_sprite = GetNodeOrNull<Sprite2D>("%Sprite");
		_frontArea = GetNode<AabbArea2D>("Sprite/Area2D");
		_backArea = GetNode<AabbArea2D>("Sprite/Area2D2");
		SetPhysicsProcess(enable: false);
	}

	public void UpdateOverlaps()
	{
		ProcessArea(_frontArea, _frontOverlaps, backArea: false);
		ProcessArea(_backArea, _backOverlaps, backArea: true);
	}

	public void CheckFrontArea(AabbArea2D area)
	{
		if (area.GetParent() is TowerDefenseCharacter tdChar)
		{
			CheckCharacter(tdChar, backArea: false);
		}
	}

	public void CheckBackArea(AabbArea2D area)
	{
		if (area.GetParent() is TowerDefenseCharacter tdChar)
		{
			CheckCharacter(tdChar, backArea: true);
		}
	}

	private void ProcessArea(AabbArea2D area, HashSet<TowerDefenseCharacter> currentOverlaps, bool backArea)
	{
		if (!GodotObject.IsInstanceValid(area) || area.ProcessMode == ProcessModeEnum.Disabled)
		{
			currentOverlaps.Clear();
		}
		else
		{
			if (TowerDefenseManager.Instance == null || TowerDefenseManager.Instance.characterRegistry == null)
			{
				return;
			}
			Rect2 checkRect = AabbShapeUtil.ComputeAreaWorldRect(area);
			List<TowerDefenseCharacter> charactersIntersectingRectList = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectList(checkRect);
			_scratchOverlaps.Clear();
			for (int i = 0; i < charactersIntersectingRectList.Count; i++)
			{
				TowerDefenseCharacter towerDefenseCharacter = charactersIntersectingRectList[i];
				if (GodotObject.IsInstanceValid(towerDefenseCharacter))
				{
					_scratchOverlaps.Add(towerDefenseCharacter);
					if (!currentOverlaps.Contains(towerDefenseCharacter))
					{
						CheckCharacter(towerDefenseCharacter, backArea);
					}
				}
			}
			currentOverlaps.RemoveWhere((TowerDefenseCharacter ch) => !GodotObject.IsInstanceValid(ch) || !_scratchOverlaps.Contains(ch));
			foreach (TowerDefenseCharacter scratchOverlap in _scratchOverlaps)
			{
				currentOverlaps.Add(scratchOverlap);
			}
		}
	}

	private void CheckCharacter(TowerDefenseCharacter tdChar, bool backArea)
	{
		if (GodotObject.IsInstanceValid(tdChar) && !tdChar.config.warnningLineFliter && (backArea || !(tdChar.Scale.X < 0f)) && (!backArea || !(tdChar.Scale.X > 0f)) && tdChar.camp != TowerDefenseEnum.CHARACTER_CAMP.PLANT && (tdChar.instance.maskFlags & 0x12) == 0 && tdChar.instance.maskFlags != 0 && !(tdChar is TowerDefensePlant) && !(tdChar is TowerDefenseGravestone) && (!(tdChar is TowerDefenseZombie) || (!tdChar.instance.die && !tdChar.instance.nearDie && (backArea || tdChar.instance.zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS))) && feature != null)
		{
			feature.OnWarningLineTriggered();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateOverlaps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckFrontArea, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "area", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CheckBackArea, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "area", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CheckCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tdChar", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "backArea", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.UpdateOverlaps && args.Count == 0)
		{
			UpdateOverlaps();
			ret = default;
			return true;
		}
		if (method == MethodName.CheckFrontArea && args.Count == 1)
		{
			CheckFrontArea(VariantUtils.ConvertTo<AabbArea2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckBackArea && args.Count == 1)
		{
			CheckBackArea(VariantUtils.ConvertTo<AabbArea2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckCharacter && args.Count == 2)
		{
			CheckCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
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
		if (method == MethodName.UpdateOverlaps)
		{
			return true;
		}
		if (method == MethodName.CheckFrontArea)
		{
			return true;
		}
		if (method == MethodName.CheckBackArea)
		{
			return true;
		}
		if (method == MethodName.CheckCharacter)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.row)
		{
			row = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			_sprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName._frontArea)
		{
			_frontArea = VariantUtils.ConvertTo<AabbArea2D>(in value);
			return true;
		}
		if (name == PropertyName._backArea)
		{
			_backArea = VariantUtils.ConvertTo<AabbArea2D>(in value);
			return true;
		}
		if (name == PropertyName.feature)
		{
			feature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureWarningLine>(in value);
			return true;
		}
		if (name == PropertyName.column)
		{
			column = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.row)
		{
			value = VariantUtils.CreateFrom<int>(row);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			value = VariantUtils.CreateFrom(in _sprite);
			return true;
		}
		if (name == PropertyName._frontArea)
		{
			value = VariantUtils.CreateFrom(in _frontArea);
			return true;
		}
		if (name == PropertyName._backArea)
		{
			value = VariantUtils.CreateFrom(in _backArea);
			return true;
		}
		if (name == PropertyName.feature)
		{
			value = VariantUtils.CreateFrom(in feature);
			return true;
		}
		if (name == PropertyName.column)
		{
			value = VariantUtils.CreateFrom(in column);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._frontArea, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._backArea, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.feature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.column, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.row, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.row, Variant.From<int>(row));
		info.AddProperty(PropertyName._sprite, Variant.From(in _sprite));
		info.AddProperty(PropertyName._frontArea, Variant.From(in _frontArea));
		info.AddProperty(PropertyName._backArea, Variant.From(in _backArea));
		info.AddProperty(PropertyName.feature, Variant.From(in feature));
		info.AddProperty(PropertyName.column, Variant.From(in column));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.row, out var value))
		{
			row = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._sprite, out var value2))
		{
			_sprite = value2.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName._frontArea, out var value3))
		{
			_frontArea = value3.As<AabbArea2D>();
		}
		if (info.TryGetProperty(PropertyName._backArea, out var value4))
		{
			_backArea = value4.As<AabbArea2D>();
		}
		if (info.TryGetProperty(PropertyName.feature, out var value5))
		{
			feature = value5.As<TowerDefenseBattleFeatureWarningLine>();
		}
		if (info.TryGetProperty(PropertyName.column, out var value6))
		{
			column = value6.As<int>();
		}
	}
}
