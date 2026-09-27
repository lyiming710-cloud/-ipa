using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/AttackCollisionRuntimeTest.cs")]
public class AttackCollisionRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RunRectangleIsolationAndLifecycle = "RunRectangleIsolationAndLifecycle";

		public static readonly StringName RunSegmentIsolation = "RunSegmentIsolation";

		public static readonly StringName RunCompoundUnionAndShapeDisable = "RunCompoundUnionAndShapeDisable";

		public static readonly StringName RunHitBoxSourceFallbackAndRebind = "RunHitBoxSourceFallbackAndRebind";

		public static readonly StringName RunHitBoxBoundsRevisionInvalidation = "RunHitBoxBoundsRevisionInvalidation";

		public static readonly StringName CreateDefinition = "CreateDefinition";

		public static readonly StringName CreateOwner = "CreateOwner";

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

	public override void _Ready()
	{
		try
		{
			RunRectangleIsolationAndLifecycle();
			RunSegmentIsolation();
			RunCompoundUnionAndShapeDisable();
			RunHitBoxSourceFallbackAndRebind();
			RunHitBoxBoundsRevisionInvalidation();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[AttackCollisionRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failures == 0;
		GD.Print($"ATTACK_COLLISION_RUNTIME_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private void RunRectangleIsolationAndLifecycle()
	{
		RectangleShape2D rectangleShape2D = new RectangleShape2D
		{
			Size = new Vector2(20f, 10f)
		};
		AabbShape2DResource aabbShape2DResource = new AabbShape2DResource
		{
			Geometry = rectangleShape2D,
			LocalTransform = new Transform2D(0f, new Vector2(5f, 0f)),
			DebugDraw = false
		};
		AttackComponentDefinition definition = CreateDefinition("attack.runtime.rectangle", aabbShape2DResource);
		TowerDefenseCharacter towerDefenseCharacter = CreateOwner(new Vector2(100f, 200f));
		TowerDefenseCharacter towerDefenseCharacter2 = CreateOwner(new Vector2(-30f, 40f));
		ComponentManager manager = new ComponentManager();
		ComponentManager manager2 = new ComponentManager();
		AttackComponent attackComponent = Bind(definition, manager, towerDefenseCharacter);
		AttackComponent attackComponent2 = Bind(definition, manager2, towerDefenseCharacter2);
		Check(attackComponent.CheckAreaShapeCount == 1 && attackComponent2.CheckAreaShapeCount == 1, "Both runtimes must see the shared Definition shape.");
		Check(attackComponent.TryGetCheckAreaRectangleSize(0, out var size) && size == rectangleShape2D.Size, "Runtime A must read the configured rectangle size.");
		Check(attackComponent2.TryGetCheckAreaRectangleSize(0, out var size2) && size2 == rectangleShape2D.Size, "Runtime B must read the configured rectangle size.");
		Vector2 vector = new Vector2(40f, 30f);
		Check(attackComponent.SetCheckAreaRectangleSize(0, vector), "Runtime A must accept a rectangle override.");
		Check(attackComponent.SetCheckAreaShapeLocalOrigin(0, new Vector2(10f, 5f)), "Runtime A must accept a local-origin override.");
		Check(attackComponent.TryGetCheckAreaRectangleSize(0, out size) && size == vector, "Runtime A must expose its effective rectangle override.");
		Check(attackComponent2.TryGetCheckAreaRectangleSize(0, out size2) && size2 == new Vector2(20f, 10f), "Runtime A must not change Runtime B.");
		Check(rectangleShape2D.Size == new Vector2(20f, 10f) && aabbShape2DResource.LocalTransform.Origin == new Vector2(5f, 0f), "Runtime overrides must not mutate the shared Definition Resource.");
		Check(attackComponent.TryGetCheckAreaWorldRect(out var worldRect) && RectApprox(worldRect, new Rect2(90f, 190f, 40f, 30f)), "Rectangle world bounds must include owner and local transforms.");
		Check(attackComponent.SetCheckAreaShapeWorldOrigin(0, new Vector2(150f, 250f)), "Runtime A must accept a world-origin override.");
		Check(attackComponent.TryGetCheckAreaShapeLocalTransform(0, out var localTransform) && localTransform.Origin.IsEqualApprox(new Vector2(50f, 50f)), "World origin must be converted into owner-local coordinates.");
		Check(attackComponent.TryGetCheckAreaWorldRect(out worldRect) && RectApprox(worldRect, new Rect2(130f, 235f, 40f, 30f)), "World-origin override must update effective bounds.");
		Check(attackComponent.DrawCheckAreaCollisionPreview(towerDefenseCharacter), "Runtime preview must accept effective Attack geometry.");
		attackComponent.Detach(ComponentDetachReason.TemporaryTreeExit);
		attackComponent.Bind(manager, towerDefenseCharacter, definition);
		Check(attackComponent.TryGetCheckAreaRectangleSize(0, out size) && size == vector, "Temporary tree exit must retain per-character geometry overrides.");
		Check(attackComponent.SetCheckAreaEnabled(enabled: false) && !attackComponent.TryGetCheckAreaWorldRect(out var worldRect2), "Disabling an Attack area must prevent stale bounds from being returned.");
		Check(attackComponent2.TryGetCheckAreaWorldRect(out worldRect2), "Disabling Runtime A must not disable Runtime B.");
		Dictionary data = attackComponent.ExportComponentSave();
		TowerDefenseCharacter towerDefenseCharacter3 = CreateOwner(towerDefenseCharacter.Position);
		ComponentManager manager3 = new ComponentManager();
		AttackComponent attackComponent3 = Bind(definition, manager3, towerDefenseCharacter3);
		attackComponent3.ImportComponentSave(data, null);
		Check(!attackComponent3.TryGetCheckAreaWorldRect(out worldRect2) && attackComponent3.TryGetCheckAreaRectangleSize(0, out var size3) && size3 == vector, "Save import must restore area enabled state and geometry overrides.");
		attackComponent2.SyncDeserialize(attackComponent.SyncSerialize());
		Check(!attackComponent2.TryGetCheckAreaWorldRect(out worldRect2) && attackComponent2.TryGetCheckAreaRectangleSize(0, out var size4) && size4 == vector, "Network sync must restore area enabled state and geometry overrides.");
		attackComponent.Release();
		attackComponent2.Release();
		attackComponent3.Release();
		towerDefenseCharacter.Free();
		towerDefenseCharacter2.Free();
		towerDefenseCharacter3.Free();
	}

	private void RunSegmentIsolation()
	{
		SegmentShape2D segmentShape2D = new SegmentShape2D
		{
			A = new Vector2(0f, -2f),
			B = new Vector2(-10f, 3f)
		};
		AabbShape2DResource aabbShape2DResource = new AabbShape2DResource
		{
			Geometry = segmentShape2D,
			LocalTransform = Transform2D.Identity,
			DebugDraw = false
		};
		AttackComponentDefinition definition = CreateDefinition("attack.runtime.segment", aabbShape2DResource);
		TowerDefenseCharacter towerDefenseCharacter = CreateOwner(Vector2.Zero);
		TowerDefenseCharacter towerDefenseCharacter2 = CreateOwner(Vector2.Zero);
		ComponentManager manager = new ComponentManager();
		ComponentManager manager2 = new ComponentManager();
		AttackComponent attackComponent = Bind(definition, manager, towerDefenseCharacter);
		AttackComponent attackComponent2 = Bind(definition, manager2, towerDefenseCharacter2);
		Check(attackComponent.SetCheckAreaSegmentLengthX(0, 35f), "Segment length override must be accepted.");
		Check(attackComponent.TryGetCheckAreaSegmentEnd(0, out var endpoint) && endpoint.IsEqualApprox(new Vector2(-35f, 3f)), "Segment override must preserve configured direction and Y endpoint.");
		Check(attackComponent2.TryGetCheckAreaSegmentEnd(0, out var endpoint2) && endpoint2 == segmentShape2D.B, "Segment override must remain isolated per runtime.");
		Check(segmentShape2D.B == new Vector2(-10f, 3f), "Segment override must not mutate shared geometry.");
		attackComponent.Release();
		attackComponent2.Release();
		towerDefenseCharacter.Free();
		towerDefenseCharacter2.Free();
	}

	private void RunCompoundUnionAndShapeDisable()
	{
		AabbShape2DResource aabbShape2DResource = new AabbShape2DResource
		{
			Geometry = new RectangleShape2D
			{
				Size = new Vector2(10f, 10f)
			},
			LocalTransform = new Transform2D(0f, new Vector2(-10f, 0f))
		};
		AabbShape2DResource aabbShape2DResource2 = new AabbShape2DResource
		{
			Geometry = new RectangleShape2D
			{
				Size = new Vector2(10f, 20f)
			},
			LocalTransform = new Transform2D(0f, new Vector2(15f, 0f))
		};
		AttackComponentDefinition definition = CreateDefinition("attack.runtime.compound", aabbShape2DResource, aabbShape2DResource2);
		TowerDefenseCharacter towerDefenseCharacter = CreateOwner(Vector2.Zero);
		ComponentManager manager = new ComponentManager();
		AttackComponent attackComponent = Bind(definition, manager, towerDefenseCharacter);
		Check(attackComponent.TryGetCheckAreaWorldRect(out var worldRect) && RectApprox(worldRect, new Rect2(-15f, -10f, 35f, 20f)), "Multiple Definition shapes must union in order.");
		Check(attackComponent.SetCheckAreaShapeEnabled(1, enabled: false) && attackComponent.TryGetCheckAreaWorldRect(out var worldRect2) && RectApprox(worldRect2, new Rect2(-15f, -5f, 10f, 10f)), "Per-shape disable must remove that shape from effective bounds.");
		attackComponent.Release();
		towerDefenseCharacter.Free();
	}

	private void RunHitBoxSourceFallbackAndRebind()
	{
		AabbShape2DResource aabbShape2DResource = new AabbShape2DResource
		{
			Geometry = new RectangleShape2D
			{
				Size = new Vector2(10f, 10f)
			},
			DebugDraw = false
		};
		AttackComponentDefinition definition = CreateDefinition("attack.runtime.hitbox-source", aabbShape2DResource);
		TowerDefenseCharacter towerDefenseCharacter = CreateOwner(Vector2.Zero);
		TowerDefenseCharacter towerDefenseCharacter2 = CreateOwner(new Vector2(100f, 100f));
		towerDefenseCharacter2.HitBoxDefinition = new CharacterHitBoxDefinition
		{
			Size = new Vector2(30f, 40f)
		};
		ComponentManager manager = new ComponentManager();
		AttackComponent attackComponent = Bind(definition, manager, towerDefenseCharacter);
		attackComponent.SetCheckHitBoxSource(towerDefenseCharacter2);
		Check(attackComponent.TryGetCheckAreaWorldRect(out var worldRect) && RectApprox(worldRect, new Rect2(85f, 80f, 30f, 40f)), "A valid runtime HitBox source must replace Definition geometry.");
		attackComponent.Detach(ComponentDetachReason.TemporaryTreeExit);
		attackComponent.Bind(manager, towerDefenseCharacter, definition);
		Check(attackComponent.TryGetCheckAreaWorldRect(out worldRect) && RectApprox(worldRect, new Rect2(85f, 80f, 30f, 40f)), "Temporary tree exit must preserve the runtime HitBox source.");
		towerDefenseCharacter2.HitBoxDefinition = null;
		Check(attackComponent.TryGetCheckAreaWorldRect(out var worldRect2) && RectApprox(worldRect2, new Rect2(-5f, -5f, 10f, 10f)), "An invalid HitBox source must fall back to Definition geometry without a stale cache.");
		Check(attackComponent.SetCheckAreaEnabled(enabled: false) && !attackComponent.TryGetCheckAreaWorldRect(out var _), "Global area disable must also disable a runtime HitBox source.");
		attackComponent.Release();
		towerDefenseCharacter.Free();
		towerDefenseCharacter2.Free();
	}

	private void RunHitBoxBoundsRevisionInvalidation()
	{
		TowerDefenseCharacter towerDefenseCharacter = CreateOwner(Vector2.Zero);
		ulong hitBoxBoundsRevision = towerDefenseCharacter.HitBoxBoundsRevision;
		towerDefenseCharacter._Notification(2000);
		Check(towerDefenseCharacter.HitBoxBoundsRevision != hitBoxBoundsRevision, "Transform changes must invalidate cached Attack hit-box bounds.");
		ulong hitBoxBoundsRevision2 = towerDefenseCharacter.HitBoxBoundsRevision;
		towerDefenseCharacter.HitBoxDefinition = new CharacterHitBoxDefinition
		{
			Size = new Vector2(20f, 30f)
		};
		Check(towerDefenseCharacter.HitBoxBoundsRevision != hitBoxBoundsRevision2, "Replacing the hit-box definition must invalidate cached Attack bounds.");
		ulong hitBoxBoundsRevision3 = towerDefenseCharacter.HitBoxBoundsRevision;
		towerDefenseCharacter.SetHitBoxEnabled(enabled: false);
		Check(towerDefenseCharacter.HitBoxBoundsRevision != hitBoxBoundsRevision3, "Changing hit-box availability must invalidate cached Attack bounds.");
		ulong hitBoxBoundsRevision4 = towerDefenseCharacter.HitBoxBoundsRevision;
		_ = towerDefenseCharacter.WorldHitRect;
		Check(towerDefenseCharacter.HitBoxBoundsRevision == hitBoxBoundsRevision4, "Reading stable hit-box bounds must not invalidate the Attack range cache.");
		towerDefenseCharacter.Free();
	}

	private static AttackComponentDefinition CreateDefinition(string instanceId, params AabbShape2DResource[] shapes)
	{
		AttackComponentDefinition attackComponentDefinition = new AttackComponentDefinition
		{
			ComponentTypeId = "AttackComponent",
			DefinitionId = instanceId,
			InstanceId = instanceId,
			WireIndex = 0
		};
		foreach (AabbShape2DResource item in shapes)
		{
			attackComponentDefinition.checkShapeResources.Add(item);
		}
		return attackComponentDefinition;
	}

	private static TowerDefenseCharacter CreateOwner(Vector2 position)
	{
		return new TowerDefenseCharacter
		{
			Position = position
		};
	}

	private static AttackComponent Bind(AttackComponentDefinition definition, ComponentManager manager, TowerDefenseCharacter owner)
	{
		AttackComponent attackComponent = new AttackComponent();
		attackComponent.Bind(manager, owner, definition);
		return attackComponent;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[AttackCollisionRuntimeTest] " + message);
		}
	}

	private static bool RectApprox(Rect2 actual, Rect2 expected)
	{
		if (actual.Position.IsEqualApprox(expected.Position))
		{
			return actual.Size.IsEqualApprox(expected.Size);
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunRectangleIsolationAndLifecycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunSegmentIsolation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunCompoundUnionAndShapeDisable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunHitBoxSourceFallbackAndRebind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunHitBoxBoundsRevisionInvalidation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "instanceId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "shapes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateOwner, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RectApprox, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "actual", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "expected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RunRectangleIsolationAndLifecycle && args.Count == 0)
		{
			RunRectangleIsolationAndLifecycle();
			ret = default;
			return true;
		}
		if (method == MethodName.RunSegmentIsolation && args.Count == 0)
		{
			RunSegmentIsolation();
			ret = default;
			return true;
		}
		if (method == MethodName.RunCompoundUnionAndShapeDisable && args.Count == 0)
		{
			RunCompoundUnionAndShapeDisable();
			ret = default;
			return true;
		}
		if (method == MethodName.RunHitBoxSourceFallbackAndRebind && args.Count == 0)
		{
			RunHitBoxSourceFallbackAndRebind();
			ret = default;
			return true;
		}
		if (method == MethodName.RunHitBoxBoundsRevisionInvalidation && args.Count == 0)
		{
			RunHitBoxBoundsRevisionInvalidation();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateDefinition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<AttackComponentDefinition>(CreateDefinition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertToSystemArrayOfGodotObject<AabbShape2DResource>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateOwner && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(CreateOwner(VariantUtils.ConvertTo<Vector2>(in args[0])));
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
		if (method == MethodName.CreateDefinition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<AttackComponentDefinition>(CreateDefinition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertToSystemArrayOfGodotObject<AabbShape2DResource>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateOwner && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(CreateOwner(VariantUtils.ConvertTo<Vector2>(in args[0])));
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
		if (method == MethodName.RunRectangleIsolationAndLifecycle)
		{
			return true;
		}
		if (method == MethodName.RunSegmentIsolation)
		{
			return true;
		}
		if (method == MethodName.RunCompoundUnionAndShapeDisable)
		{
			return true;
		}
		if (method == MethodName.RunHitBoxSourceFallbackAndRebind)
		{
			return true;
		}
		if (method == MethodName.RunHitBoxBoundsRevisionInvalidation)
		{
			return true;
		}
		if (method == MethodName.CreateDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateOwner)
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
