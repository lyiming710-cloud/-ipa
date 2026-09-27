using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter6/Starleaf/Scene/TowerDefensePlantStarleaf.cs")]
public class TowerDefensePlantStarleaf : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName BlockCharacter = "BlockCharacter";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _isBlocking = "_isBlocking";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private BlockComponent _blockComponent;

	private bool _isBlocking;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_blockComponent = componentManager.GetRuntime<BlockComponent>();
			if (_blockComponent != null)
			{
				_blockComponent.OnBlock += BlockCharacter;
				_blockComponent.SetCheckRectangleSize(TowerDefenseManager.Instance.GetMapGridSize() * 4f);
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_blockComponent != null)
		{
			_blockComponent.OnBlock -= BlockCharacter;
		}
		_blockComponent = null;
	}

	public async void BlockCharacter()
	{
		if (!_isBlocking)
		{
			_isBlocking = true;
			itemLayer = TowerDefenseEnum.LAYER_GROUNDITEM.EFFECT;
			sprite.SetAnimation("Block", loop: false, 0.1);
			sprite.AddAnimation("Idle", 0.0);
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		TowerDefenseCharacter characterTargetNearest = TowerDefenseManager.Instance.GetCharacterTargetNearest(this, TowerDefenseEnum.TARGET_NEAR_METHOD.POSITION);
		if (characterTargetNearest != null)
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(characterTargetNearest.gridPos);
			double num = 0.0;
			if (GodotObject.IsInstanceValid(mapCell))
			{
				num = 0.0 - mapCell.GetGroundHeight() + 30.0;
			}
			TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = new TowerDefenseProjectileCreateData("MeteorStar");
			towerDefenseProjectileCreateData.baseDamage = 100.0;
			towerDefenseProjectileCreateData.damageFlags = 2;
			if ((double)GD.Randf() < 0.2)
			{
				towerDefenseProjectileCreateData.projectileName = "MeteorStarS";
				towerDefenseProjectileCreateData.baseDamage = 200.0;
				towerDefenseProjectileCreateData.damageFlags = 2;
				Health(100.0);
			}
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				useFall = true,
				gridYOverride = characterTargetNearest.gridPos.Y,
				zOverride = 600.0,
				ySpeedOverride = 800.0
			};
			FireComponent.CreateProjectilePositionByData(null, null, (float)num, characterTargetNearest.GetLogicalGlobalPosition() - new Vector2(150f, 0f), new Vector2(GD.RandRange(50, 150), 0f), towerDefenseProjectileCreateData, -1, camp, default, overrides);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Block")
		{
			_isBlocking = false;
			itemLayer = TowerDefenseEnum.LAYER_GROUNDITEM.PLANT;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlockCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BlockCharacter && args.Count == 0)
		{
			BlockCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.BlockCharacter)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._isBlocking)
		{
			_isBlocking = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._isBlocking)
		{
			value = VariantUtils.CreateFrom(in _isBlocking);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._isBlocking, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._isBlocking, Variant.From(in _isBlocking));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._isBlocking, out var value))
		{
			_isBlocking = value.As<bool>();
		}
	}
}
