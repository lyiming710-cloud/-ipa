using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventProjectileSplit.cs")]
public class TowerDefenseCharacterEventProjectileSplit : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public static readonly StringName Run = "Run";

		public static readonly StringName SpawnSplitEffect = "SpawnSplitEffect";

		public static readonly StringName ResolveEffectZIndex = "ResolveEffectZIndex";

		public static readonly StringName IsEnemyTarget = "IsEnemyTarget";

		public static readonly StringName ResolveDirSign = "ResolveDirSign";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName projectileData = "projectileData";

		public static readonly StringName lineOffsets = "lineOffsets";

		public static readonly StringName speed = "speed";

		public static readonly StringName forwardOffset = "forwardOffset";

		public static readonly StringName useTargetGridY = "useTargetGridY";

		public static readonly StringName enemyOnly = "enemyOnly";

		public static readonly StringName edgeCompensate = "edgeCompensate";

		public static readonly StringName effectScene = "effectScene";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseProjectileCreateData projectileData;

	[Export(PropertyHint.None, "")]
	public Array<int> lineOffsets = new Array<int> { -1, 0, 1 };

	[Export(PropertyHint.None, "")]
	public double speed = 300.0;

	[Export(PropertyHint.None, "")]
	public double forwardOffset = 30.0;

	[Export(PropertyHint.None, "")]
	public bool useTargetGridY = true;

	[Export(PropertyHint.None, "")]
	public bool enemyOnly = true;

	[Export(PropertyHint.None, "")]
	public bool edgeCompensate = true;

	[Export(PropertyHint.None, "")]
	public PackedScene effectScene;

	public string projectileName
	{
		get
		{
			if (projectileData == null)
			{
				return "";
			}
			return projectileData.projectileName.ToString();
		}
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(projectile, target, projectileData, lineOffsets, speed, forwardOffset, useTargetGridY, enemyOnly, edgeCompensate, effectScene);
	}

	public static void Run(TowerDefenseProjectile projectile, TowerDefenseCharacter target, TowerDefenseProjectileCreateData _projectileData, Array<int> _lineOffsets, double _speed, double _forwardOffset, bool _useTargetGridY, bool _enemyOnly = true, bool _edgeCompensate = true, PackedScene _effectScene = null)
	{
		if (_projectileData == null || _lineOffsets == null || _lineOffsets.Count == 0)
		{
			return;
		}
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance) || !GodotObject.IsInstanceValid(target) || (_enemyOnly && !IsEnemyTarget(projectile, target)))
		{
			return;
		}
		float num = ResolveDirSign(projectile, target);
		Vector2 vector;
		int x;
		int y;
		double height;
		int collisionFlags;
		TowerDefenseEnum.CHARACTER_CAMP camp;
		if (GodotObject.IsInstanceValid(projectile))
		{
			vector = projectile.GlobalPosition;
			x = projectile.gridPos.X;
			y = projectile.gridPos.Y;
			height = projectile.height;
			collisionFlags = projectile.collisionFlags;
			camp = projectile.camp;
		}
		else
		{
			vector = target.GetLogicalGlobalPosition();
			x = target.gridPos.X;
			y = target.gridPos.Y;
			height = 0.0;
			collisionFlags = ((target.instance == null) ? 1 : target.instance.collisionFlags);
			camp = ((target.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT) ? TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE : TowerDefenseEnum.CHARACTER_CAMP.PLANT);
		}
		if (_useTargetGridY)
		{
			x = target.gridPos.X;
			y = target.gridPos.Y;
		}
		float x2 = vector.X + (float)(_forwardOffset * (double)num);
		Vector2 velocity = new Vector2((float)(_speed * (double)num), 0f);
		float value = ((num > 0f) ? 0f : ((float)Math.PI));
		if (_effectScene != null)
		{
			Vector2 position = (GodotObject.IsInstanceValid(target) ? target.GetLogicalGlobalPosition() : vector);
			SpawnSplitEffect(_effectScene, position, new Vector2I(x, y), height, collisionFlags, camp);
		}
		int max = (_edgeCompensate ? instance.GetMapGridNum().Y : 0);
		for (int i = 0; i < _lineOffsets.Count; i++)
		{
			int num2 = y + _lineOffsets[i];
			if (_edgeCompensate)
			{
				num2 = Mathf.Clamp(num2, 1, max);
			}
			Vector2 mapCellPosCenter = instance.GetMapCellPosCenter(new Vector2I(x, num2));
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				gridYOverride = num2,
				checkAllOverride = false,
				initialRotationOverride = value
			};
			FireComponent.CreateProjectilePosition(null, null, height, new Vector2(x2, mapCellPosCenter.Y), velocity, _projectileData, collisionFlags, camp, default, overrides);
		}
	}

	private static void SpawnSplitEffect(PackedScene scene, Vector2 position, Vector2I gridPosition, double height, int collisionFlags, TowerDefenseEnum.CHARACTER_CAMP camp)
	{
		if (scene == null || !position.IsFinite())
		{
			return;
		}
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		if (GodotObject.IsInstanceValid(characterNode))
		{
			Node2D node2D = scene.Instantiate<Node2D>(PackedScene.GenEditState.Disabled);
			if (node2D is TowerDefenseProjectileEffectBase towerDefenseProjectileEffectBase)
			{
				towerDefenseProjectileEffectBase.Init(gridPosition, camp, collisionFlags, null, height);
			}
			node2D.GlobalPosition = position;
			node2D.ZIndex = ResolveEffectZIndex(gridPosition.Y);
			characterNode.AddChild(node2D, forceReadableName: false, Node.InternalMode.Disabled);
		}
	}

	internal static int ResolveEffectZIndex(int gridY)
	{
		return (int)Math.Clamp((long)Mathf.Max(1, gridY) * 15L + 10, -4096L, 4096L);
	}

	private static bool IsEnemyTarget(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		TowerDefenseEnum.CHARACTER_CAMP cHARACTER_CAMP = (GodotObject.IsInstanceValid(projectile) ? projectile.camp : TowerDefenseEnum.CHARACTER_CAMP.NOONE);
		if (cHARACTER_CAMP != TowerDefenseEnum.CHARACTER_CAMP.PLANT && cHARACTER_CAMP != TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
		{
			return true;
		}
		return target.camp != cHARACTER_CAMP;
	}

	private static float ResolveDirSign(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		if (target.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT)
		{
			return -1f;
		}
		if (target.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
		{
			return 1f;
		}
		if (GodotObject.IsInstanceValid(projectile) && projectile.velocity.X < 0f)
		{
			return -1f;
		}
		return 1f;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.ExecuteProject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "_projectileData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Array, "_lineOffsets", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_speed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_forwardOffset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_useTargetGridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_enemyOnly", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_edgeCompensate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_effectScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnSplitEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveEffectZIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "gridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsEnemyTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveDirSign, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ExecuteProject && args.Count == 2)
		{
			ExecuteProject(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Run && args.Count == 10)
		{
			Run(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in args[2]), VariantUtils.ConvertToArray<int>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]), VariantUtils.ConvertTo<bool>(in args[8]), VariantUtils.ConvertTo<PackedScene>(in args[9]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnSplitEffect && args.Count == 6)
		{
			SpawnSplitEffect(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveEffectZIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveEffectZIndex(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsEnemyTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEnemyTarget(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolveDirSign && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(ResolveDirSign(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Run && args.Count == 10)
		{
			Run(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in args[2]), VariantUtils.ConvertToArray<int>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]), VariantUtils.ConvertTo<bool>(in args[8]), VariantUtils.ConvertTo<PackedScene>(in args[9]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnSplitEffect && args.Count == 6)
		{
			SpawnSplitEffect(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveEffectZIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveEffectZIndex(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsEnemyTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEnemyTarget(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolveDirSign && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(ResolveDirSign(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ExecuteProject)
		{
			return true;
		}
		if (method == MethodName.Run)
		{
			return true;
		}
		if (method == MethodName.SpawnSplitEffect)
		{
			return true;
		}
		if (method == MethodName.ResolveEffectZIndex)
		{
			return true;
		}
		if (method == MethodName.IsEnemyTarget)
		{
			return true;
		}
		if (method == MethodName.ResolveDirSign)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.projectileData)
		{
			projectileData = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		if (name == PropertyName.lineOffsets)
		{
			lineOffsets = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.speed)
		{
			speed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.forwardOffset)
		{
			forwardOffset = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.useTargetGridY)
		{
			useTargetGridY = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.enemyOnly)
		{
			enemyOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.edgeCompensate)
		{
			edgeCompensate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.effectScene)
		{
			effectScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom<string>(projectileName);
			return true;
		}
		if (name == PropertyName.projectileData)
		{
			value = VariantUtils.CreateFrom(in projectileData);
			return true;
		}
		if (name == PropertyName.lineOffsets)
		{
			value = VariantUtils.CreateFromArray(lineOffsets);
			return true;
		}
		if (name == PropertyName.speed)
		{
			value = VariantUtils.CreateFrom(in speed);
			return true;
		}
		if (name == PropertyName.forwardOffset)
		{
			value = VariantUtils.CreateFrom(in forwardOffset);
			return true;
		}
		if (name == PropertyName.useTargetGridY)
		{
			value = VariantUtils.CreateFrom(in useTargetGridY);
			return true;
		}
		if (name == PropertyName.enemyOnly)
		{
			value = VariantUtils.CreateFrom(in enemyOnly);
			return true;
		}
		if (name == PropertyName.edgeCompensate)
		{
			value = VariantUtils.CreateFrom(in edgeCompensate);
			return true;
		}
		if (name == PropertyName.effectScene)
		{
			value = VariantUtils.CreateFrom(in effectScene);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.projectileData, PropertyHint.ResourceType, "TowerDefenseProjectileCreateData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.lineOffsets, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.speed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.forwardOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useTargetGridY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enemyOnly, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.edgeCompensate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.effectScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.projectileData, Variant.From(in projectileData));
		info.AddProperty(PropertyName.lineOffsets, Variant.CreateFrom(lineOffsets));
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName.forwardOffset, Variant.From(in forwardOffset));
		info.AddProperty(PropertyName.useTargetGridY, Variant.From(in useTargetGridY));
		info.AddProperty(PropertyName.enemyOnly, Variant.From(in enemyOnly));
		info.AddProperty(PropertyName.edgeCompensate, Variant.From(in edgeCompensate));
		info.AddProperty(PropertyName.effectScene, Variant.From(in effectScene));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.projectileData, out var value))
		{
			projectileData = value.As<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName.lineOffsets, out var value2))
		{
			lineOffsets = value2.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.speed, out var value3))
		{
			speed = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.forwardOffset, out var value4))
		{
			forwardOffset = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.useTargetGridY, out var value5))
		{
			useTargetGridY = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.enemyOnly, out var value6))
		{
			enemyOnly = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.edgeCompensate, out var value7))
		{
			edgeCompensate = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.effectScene, out var value8))
		{
			effectScene = value8.As<PackedScene>();
		}
	}
}
