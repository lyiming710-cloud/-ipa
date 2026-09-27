using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/CharacterMainHitBoxRuntimeTest.cs")]
public class CharacterMainHitBoxRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RunWorldBoundsCheck = "RunWorldBoundsCheck";

		public static readonly StringName RunHitFacingScaleSnapshotCheck = "RunHitFacingScaleSnapshotCheck";

		public static readonly StringName RunSuppressionCheck = "RunSuppressionCheck";

		public static readonly StringName RunSharedDefinitionIsolationCheck = "RunSharedDefinitionIsolationCheck";

		public static readonly StringName RunProfileResourceChecks = "RunProfileResourceChecks";

		public static readonly StringName RunRepresentativeSceneChecks = "RunRepresentativeSceneChecks";

		public static readonly StringName CreateCharacter = "CreateCharacter";

		public static readonly StringName Check = "Check";

		public static readonly StringName RectApprox = "RectApprox";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private int _checks;

	private int _failures;

	private readonly List<Node> _instances = new List<Node>();

	public override void _Ready()
	{
		RunWorldBoundsCheck();
		RunHitFacingScaleSnapshotCheck();
		RunSuppressionCheck();
		RunSharedDefinitionIsolationCheck();
		RunProfileResourceChecks();
		RunRepresentativeSceneChecks();
		foreach (Node instance in _instances)
		{
			instance.Free();
		}
		_instances.Clear();
		bool flag = _failures == 0;
		GD.Print($"CHARACTER_MAIN_HITBOX_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void RunWorldBoundsCheck()
	{
		CharacterHitBoxDefinition characterHitBoxDefinition = new CharacterHitBoxDefinition
		{
			Size = new Vector2(122f, 32f),
			LocalTransform = new Transform2D(0f, new Vector2(43f, -5f))
		};
		TowerDefenseCharacter towerDefenseCharacter = new TowerDefenseCharacter
		{
			Position = new Vector2(314f, -87f),
			Rotation = 0.31f,
			Scale = new Vector2(-1.25f, 0.8f),
			HitBoxDefinition = characterHitBoxDefinition
		};
		Rect2 right = AabbShapeUtil.ComputeRectangleWorldRect(towerDefenseCharacter.GlobalTransform * characterHitBoxDefinition.LocalTransform, characterHitBoxDefinition.Size);
		Check(RectApprox(towerDefenseCharacter.WorldHitRect, right), "WorldHitRect lost local/world transforms");
		Check(RectApprox(towerDefenseCharacter.WorldBroadphaseRect, right), "WorldBroadphaseRect differs from hit bounds");
		Check(towerDefenseCharacter.TryGetActiveWorldHitRect(out var rect) && RectApprox(rect, right), "Active HitBox query rejected valid geometry");
		towerDefenseCharacter.Free();
		characterHitBoxDefinition.Dispose();
	}

	private void RunHitFacingScaleSnapshotCheck()
	{
		HitScaleSnapshotCharacter hitScaleSnapshotCharacter = new HitScaleSnapshotCharacter
		{
			HitBoxDefinition = new CharacterHitBoxDefinition
			{
				Size = new Vector2(44f, 70f)
			}
		};
		AddChild(hitScaleSnapshotCharacter, forceReadableName: false, InternalMode.Disabled);
		hitScaleSnapshotCharacter.Scale = new Vector2(1.25f, 0.8f);
		hitScaleSnapshotCharacter.ForceUpdateTransform();
		_ = hitScaleSnapshotCharacter.WorldHitRect;
		Check(hitScaleSnapshotCharacter.TryGetCachedHitTransform(out var originX, out var scaleX) && Mathf.IsEqualApprox(scaleX, 1.25f), "Initial hit-facing scale snapshot differs from the character scale");
		hitScaleSnapshotCharacter.Scale = new Vector2(-1.5f, 0.8f);
		hitScaleSnapshotCharacter.ForceUpdateTransform();
		_ = hitScaleSnapshotCharacter.WorldHitRect;
		Check(hitScaleSnapshotCharacter.TryGetCachedHitTransform(out originX, out var scaleX2) && Mathf.IsEqualApprox(scaleX2, -1.5f), "Local X flip did not refresh projectile hit-facing data");
		hitScaleSnapshotCharacter.SetGlobalPositionForPhysicsFrame(new Vector2(180f, -24f), Engine.GetPhysicsFrames());
		_ = hitScaleSnapshotCharacter.WorldHitRect;
		Check(hitScaleSnapshotCharacter.TryGetCachedHitTransform(out var originX2, out var scaleX3) && Mathf.IsEqualApprox(originX2, 180f) && Mathf.IsEqualApprox(scaleX3, -1.5f), "Position-only movement changed or invalidated the cached hit-facing scale");
		hitScaleSnapshotCharacter.Free();
	}

	private void RunSuppressionCheck()
	{
		TowerDefenseCharacter towerDefenseCharacter = CreateCharacter();
		towerDefenseCharacter.SetHitBoxSuppressed(TowerDefenseCharacter.HitBoxSuppressionReason.Paused, suppressed: true);
		towerDefenseCharacter.SetHitBoxSuppressed(TowerDefenseCharacter.HitBoxSuppressionReason.Carried, suppressed: true);
		Check(!towerDefenseCharacter.IsHitBoxEnabled, "Combined enabled suppression was ignored");
		towerDefenseCharacter.SetHitBoxSuppressed(TowerDefenseCharacter.HitBoxSuppressionReason.Paused, suppressed: false);
		Check(!towerDefenseCharacter.IsHitBoxEnabled, "Clearing one enabled reason cleared another");
		towerDefenseCharacter.SetHitBoxSuppressed(TowerDefenseCharacter.HitBoxSuppressionReason.Carried, suppressed: false);
		Check(towerDefenseCharacter.IsHitBoxEnabled, "Enabled suppression did not restore after all reasons cleared");
		towerDefenseCharacter.SetHitBoxMonitorSuppressed(TowerDefenseCharacter.HitBoxSuppressionReason.Tanglekelp, suppressed: true);
		towerDefenseCharacter.SetHitBoxMonitorSuppressed(TowerDefenseCharacter.HitBoxSuppressionReason.PlanternTanglekelp, suppressed: true);
		Check(!towerDefenseCharacter.IsHitBoxMonitorable, "Combined monitor suppression was ignored");
		towerDefenseCharacter.SetHitBoxMonitorSuppressed(TowerDefenseCharacter.HitBoxSuppressionReason.Tanglekelp, suppressed: false);
		Check(!towerDefenseCharacter.IsHitBoxMonitorable, "Clearing one monitor reason cleared another");
		towerDefenseCharacter.SetHitBoxMonitorSuppressed(TowerDefenseCharacter.HitBoxSuppressionReason.PlanternTanglekelp, suppressed: false);
		Check(towerDefenseCharacter.IsHitBoxMonitorable, "Monitor suppression did not restore after all reasons cleared");
		towerDefenseCharacter.DestroyHitBoxRuntime();
		towerDefenseCharacter.SetHitBoxEnabled(enabled: true);
		towerDefenseCharacter.SetHitBoxMonitorable(monitorable: true);
		Check(!towerDefenseCharacter.HasHitBox && !towerDefenseCharacter.IsHitBoxEnabled && !towerDefenseCharacter.IsHitBoxMonitorable, "Destroyed HitBox was resurrected by runtime setters");
		towerDefenseCharacter.Free();
	}

	private void RunSharedDefinitionIsolationCheck()
	{
		CharacterHitBoxDefinition characterHitBoxDefinition = new CharacterHitBoxDefinition
		{
			Size = new Vector2(44f, 70f)
		};
		TowerDefenseCharacter towerDefenseCharacter = new TowerDefenseCharacter
		{
			HitBoxDefinition = characterHitBoxDefinition
		};
		TowerDefenseCharacter towerDefenseCharacter2 = new TowerDefenseCharacter
		{
			HitBoxDefinition = characterHitBoxDefinition
		};
		towerDefenseCharacter.SetHitBoxSuppressed(TowerDefenseCharacter.HitBoxSuppressionReason.Carried, suppressed: true);
		towerDefenseCharacter.SetHitBoxMonitorSuppressed(TowerDefenseCharacter.HitBoxSuppressionReason.Tanglekelp, suppressed: true);
		Check(!towerDefenseCharacter.IsHitBoxEnabled && towerDefenseCharacter2.IsHitBoxEnabled, "Per-character enabled state mutated a shared definition");
		Check(!towerDefenseCharacter.IsHitBoxMonitorable && towerDefenseCharacter2.IsHitBoxMonitorable, "Per-character monitor state mutated a shared definition");
		Check(characterHitBoxDefinition.DefaultEnabled && characterHitBoxDefinition.DefaultMonitorable, "Runtime suppression mutated immutable profile defaults");
		towerDefenseCharacter.Free();
		towerDefenseCharacter2.Free();
		characterHitBoxDefinition.Dispose();
	}

	private void RunProfileResourceChecks()
	{
		int num = 0;
		string[] filesAt = DirAccess.GetFilesAt("res://Resource/TowerDefense/Collision/CharacterHitBoxes");
		foreach (string text in filesAt)
		{
			if (text.EndsWith(".tres", StringComparison.OrdinalIgnoreCase))
			{
				num++;
				string text2 = "res://Resource/TowerDefense/Collision/CharacterHitBoxes/" + text;
				CharacterHitBoxDefinition characterHitBoxDefinition = ResourceLoader.Load<CharacterHitBoxDefinition>(text2, null, ResourceLoader.CacheMode.Reuse);
				Check(GodotObject.IsInstanceValid(characterHitBoxDefinition), "HitBox profile failed to load: " + text2);
				Check(characterHitBoxDefinition?.HasValidGeometry ?? false, "HitBox profile has invalid geometry: " + text2);
				if (text.Contains("NotMonitorable", StringComparison.Ordinal))
				{
					Check(characterHitBoxDefinition != null && !characterHitBoxDefinition.DefaultMonitorable, "HitBox profile lost monitorability: " + text2);
				}
			}
		}
		Check(num == 35, $"Expected 35 shared HitBox profiles, found {num}");
	}

	private void RunRepresentativeSceneChecks()
	{
		string[] array = new string[8] { "res://Prefab/TowerDefense/Character/TowerDefenseCharacter.tscn", "res://Prefab/TowerDefense/Character/TowerDefenseZombieImpBase.tscn", "res://Prefab/TowerDefense/Character/TowerDefenseZombieGargantuarBase.tscn", "res://Prefab/TowerDefense/Character/TowerDefenseMower.tscn", "res://Asset/Anime/Character/Plant/Star/Robot/Scene/TowerDefensePlantRobot.tscn", "res://Asset/Anime/Character/Zombie/Boss/Boss/Scene/TowerDefenseZombieBoss.tscn", "res://Asset/Anime/Character/Zombie/Chapter1/Bed/Scene/TowerDefenseZombieBed.tscn", "res://Asset/Anime/Character/Vase/Normal/Scene/TowerDefenseVaseNormal.tscn" };
		foreach (string text in array)
		{
			Node node = ResourceLoader.Load<PackedScene>(text, null, ResourceLoader.CacheMode.Reuse)?.Instantiate(PackedScene.GenEditState.Disabled);
			TowerDefenseCharacter towerDefenseCharacter = node as TowerDefenseCharacter;
			Check(GodotObject.IsInstanceValid(towerDefenseCharacter), "Representative scene failed to instantiate: " + text);
			Check(towerDefenseCharacter?.GetNodeOrNull("HitBox") == null, "Representative scene still has a HitBox node: " + text);
			Check(GodotObject.IsInstanceValid(towerDefenseCharacter?.HitBoxDefinition) && towerDefenseCharacter.HitBoxDefinition.HasValidGeometry, $"Representative scene has invalid HitBox definition: {text}; definitionValid={GodotObject.IsInstanceValid(towerDefenseCharacter?.HitBoxDefinition)}; size={towerDefenseCharacter?.HitBoxDefinition?.Size}");
			if (GodotObject.IsInstanceValid(node))
			{
				_instances.Add(node);
			}
		}
	}

	private static TowerDefenseCharacter CreateCharacter()
	{
		return new TowerDefenseCharacter
		{
			HitBoxDefinition = new CharacterHitBoxDefinition
			{
				Size = new Vector2(44f, 70f)
			}
		};
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[CharacterMainHitBoxTest] " + message);
		}
	}

	private static bool RectApprox(Rect2 left, Rect2 right)
	{
		if (left.Position.IsEqualApprox(right.Position))
		{
			return left.Size.IsEqualApprox(right.Size);
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunWorldBoundsCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunHitFacingScaleSnapshotCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunSuppressionCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunSharedDefinitionIsolationCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunProfileResourceChecks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunRepresentativeSceneChecks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RectApprox, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RunWorldBoundsCheck && args.Count == 0)
		{
			RunWorldBoundsCheck();
			ret = default;
			return true;
		}
		if (method == MethodName.RunHitFacingScaleSnapshotCheck && args.Count == 0)
		{
			RunHitFacingScaleSnapshotCheck();
			ret = default;
			return true;
		}
		if (method == MethodName.RunSuppressionCheck && args.Count == 0)
		{
			RunSuppressionCheck();
			ret = default;
			return true;
		}
		if (method == MethodName.RunSharedDefinitionIsolationCheck && args.Count == 0)
		{
			RunSharedDefinitionIsolationCheck();
			ret = default;
			return true;
		}
		if (method == MethodName.RunProfileResourceChecks && args.Count == 0)
		{
			RunProfileResourceChecks();
			ret = default;
			return true;
		}
		if (method == MethodName.RunRepresentativeSceneChecks && args.Count == 0)
		{
			RunRepresentativeSceneChecks();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCharacter && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(CreateCharacter());
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RectApprox && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(RectApprox(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateCharacter && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(CreateCharacter());
			return true;
		}
		if (method == MethodName.RectApprox && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(RectApprox(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.RunWorldBoundsCheck)
		{
			return true;
		}
		if (method == MethodName.RunHitFacingScaleSnapshotCheck)
		{
			return true;
		}
		if (method == MethodName.RunSuppressionCheck)
		{
			return true;
		}
		if (method == MethodName.RunSharedDefinitionIsolationCheck)
		{
			return true;
		}
		if (method == MethodName.RunProfileResourceChecks)
		{
			return true;
		}
		if (method == MethodName.RunRepresentativeSceneChecks)
		{
			return true;
		}
		if (method == MethodName.CreateCharacter)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.RectApprox)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
