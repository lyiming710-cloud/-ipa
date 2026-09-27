using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewPortalProjectileTransportRuntimeTest.cs")]
public class BugOverviewPortalProjectileTransportRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreatePortalHitBox = "CreatePortalHitBox";

		public static readonly StringName CreateProjectile = "CreateProjectile";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ProjectileScenePath = "res://Prefab/TowerDefense/Projectile/TowerDefenseProjectile.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		_ = 1;
		try
		{
			await VerifyPortalTallnutNodeProjectile();
			await VerifyFeaturePortalNodeProjectile();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[PortalProjectileTransport] Unexpected exception: {value}");
		}
		bool flag = _failures == 0 && _checks == 17;
		GD.Print($"PORTAL_PROJECTILE_TRANSPORT_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyPortalTallnutNodeProjectile()
	{
		PortalProjectileTransportPorTallnutStub source = new PortalProjectileTransportPorTallnutStub
		{
			Name = "PortalTallnutSource",
			gridPos = new Vector2I(2, 2),
			Position = new Vector2(200f, 152f),
			camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT
		};
		PortalProjectileTransportPorTallnutStub destination = new PortalProjectileTransportPorTallnutStub
		{
			Name = "PortalTallnutDestination",
			gridPos = new Vector2I(6, 4),
			Position = new Vector2(600f, 304f),
			camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT
		};
		AddChild(source, forceReadableName: false, InternalMode.Disabled);
		AddChild(destination, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		Check(GetTree().GetNodeCountInGroup("PorTallnut") == 2, "The runtime fixture must expose one real portal-Tallnut pair through the production group lookup.");
		TowerDefenseProjectile towerDefenseProjectile = CreateProjectile("PortalTallnutProjectile", new Vector2(205f, 152f), new Vector2I(2, 2), new Vector2(-300f, 0f), TowerDefenseEnum.CHARACTER_CAMP.PLANT);
		source.OnProjectileIntersect(towerDefenseProjectile);
		Check(towerDefenseProjectile.GlobalPosition.IsEqualApprox(destination.GlobalPosition - new Vector2(11f, 0f)), $"Portal Tall Nut must transport a Node projectile to its partner; got {towerDefenseProjectile.GlobalPosition}.");
		Check(towerDefenseProjectile.gridPos == destination.gridPos, $"Portal Tall Nut must update the Node projectile destination cell; got {towerDefenseProjectile.gridPos}.");
		Vector2 other = (towerDefenseProjectile.GlobalPosition = new Vector2(205f, 152f));
		towerDefenseProjectile.gridPos = source.gridPos;
		towerDefenseProjectile.camp = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE;
		source.OnProjectileIntersect(towerDefenseProjectile);
		Check(towerDefenseProjectile.GlobalPosition.IsEqualApprox(other) && towerDefenseProjectile.gridPos == source.gridPos, "Portal Tall Nut must reject a projectile from the opposite camp.");
		towerDefenseProjectile.camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT;
		towerDefenseProjectile.velocity = new Vector2(300f, 0f);
		source.OnProjectileIntersect(towerDefenseProjectile);
		Check(towerDefenseProjectile.GlobalPosition.IsEqualApprox(other) && towerDefenseProjectile.gridPos == source.gridPos, "Portal Tall Nut must reject a projectile travelling away from its entrance.");
	}

	private async Task VerifyFeaturePortalNodeProjectile()
	{
		PortalProjectileTransportFeaturePortalStub portal = new PortalProjectileTransportFeaturePortalStub
		{
			Name = "FeaturePortal",
			gridPos1 = new Vector2I(1, 1),
			gridPos2 = new Vector2I(6, 4),
			gridSize = new Vector2(100f, 76f)
		};
		portal.protalNode1 = new Node2D
		{
			Name = "PortalEndpoint1",
			Position = new Vector2(150f, 76f)
		};
		portal.protalNode2 = new Node2D
		{
			Name = "PortalEndpoint2",
			Position = new Vector2(650f, 304f)
		};
		portal.AddChild(portal.protalNode1, forceReadableName: false, InternalMode.Disabled);
		portal.AddChild(portal.protalNode2, forceReadableName: false, InternalMode.Disabled);
		portal.hitBox1 = CreatePortalHitBox("PortalHitBox1", portal.protalNode1);
		portal.hitBox2 = CreatePortalHitBox("PortalHitBox2", portal.protalNode2);
		AddChild(portal, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		Check(GodotObject.IsInstanceValid(portal.protalNode1) && GodotObject.IsInstanceValid(portal.protalNode2), "The level-feature portal fixture must expose both real transport endpoints.");
		Vector2 position = new Vector2(155f, 86f);
		Vector2 other = new Vector2(150f, 86f);
		Vector2 vector = new Vector2(650f, 314f);
		TowerDefenseProjectile towerDefenseProjectile = CreateProjectile("FeaturePortalProjectile", position, portal.gridPos1, new Vector2(300f, 0f), TowerDefenseEnum.CHARACTER_CAMP.PLANT);
		portal.TeleportProjectile(towerDefenseProjectile, fromPortal1: true);
		Check(towerDefenseProjectile.GlobalPosition.IsEqualApprox(vector), $"The level-feature portal must transport a Node projectile to endpoint 2; got {towerDefenseProjectile.GlobalPosition}.");
		Check(towerDefenseProjectile.gridPos == portal.gridPos2, $"The level-feature portal must update the destination row; got {towerDefenseProjectile.gridPos}.");
		Check(portal.exclude.Contains(towerDefenseProjectile), "The transported Node projectile must enter the anti-bounce exclusion set.");
		portal.TeleportProjectile(towerDefenseProjectile, fromPortal1: false);
		Check(towerDefenseProjectile.GlobalPosition.IsEqualApprox(vector) && towerDefenseProjectile.gridPos == portal.gridPos2, "The destination endpoint must not immediately bounce the projectile back.");
		Check(portal.exclude.Contains(towerDefenseProjectile), "The anti-bounce guard must remain armed while the projectile still overlaps a portal.");
		for (int i = 0; i < 3; i++)
		{
			portal._PhysicsProcess(0.0);
			portal.TeleportProjectile(towerDefenseProjectile, fromPortal1: false);
		}
		Check(towerDefenseProjectile.GlobalPosition.IsEqualApprox(vector) && towerDefenseProjectile.gridPos == portal.gridPos2 && portal.exclude.Contains(towerDefenseProjectile), "A flat projectile overlapping the exit across physics frames must not bounce or stick in the portal pair.");
		towerDefenseProjectile.GlobalPosition = vector + new Vector2(32f, 0f);
		portal._PhysicsProcess(0.0);
		Check(!portal.exclude.Contains(towerDefenseProjectile), "Leaving both endpoints must clear the anti-bounce guard.");
		towerDefenseProjectile.GlobalPosition = vector;
		towerDefenseProjectile.gridPos = portal.gridPos2;
		portal.TeleportProjectile(towerDefenseProjectile, fromPortal1: false);
		Check(towerDefenseProjectile.GlobalPosition.IsEqualApprox(other) && towerDefenseProjectile.gridPos == portal.gridPos1, $"A later endpoint-2 crossing must transport the projectile back to endpoint 1; position={towerDefenseProjectile.GlobalPosition}, grid={towerDefenseProjectile.gridPos}.");
		Check(portal.exclude.Contains(towerDefenseProjectile), "Reverse transport must arm the same anti-bounce guard.");
	}

	private static AabbArea2D CreatePortalHitBox(string name, Node2D endpoint)
	{
		AabbArea2D aabbArea2D = new AabbArea2D
		{
			Name = name
		};
		aabbArea2D.ShapeResources.Add(new AabbShape2DResource
		{
			Geometry = new RectangleShape2D
			{
				Size = new Vector2(12f, 33f)
			}
		});
		endpoint.AddChild(aabbArea2D, forceReadableName: false, InternalMode.Disabled);
		return aabbArea2D;
	}

	private TowerDefenseProjectile CreateProjectile(string name, Vector2 position, Vector2I gridPos, Vector2 velocity, TowerDefenseEnum.CHARACTER_CAMP camp)
	{
		TowerDefenseProjectile towerDefenseProjectile = ResourceLoader.Load<PackedScene>("res://Prefab/TowerDefense/Projectile/TowerDefenseProjectile.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseProjectile>(PackedScene.GenEditState.Disabled);
		Check(GodotObject.IsInstanceValid(towerDefenseProjectile), "The real projectile scene must instantiate for " + name + ".");
		if (!GodotObject.IsInstanceValid(towerDefenseProjectile))
		{
			throw new InvalidOperationException("Could not instantiate res://Prefab/TowerDefense/Projectile/TowerDefenseProjectile.tscn.");
		}
		towerDefenseProjectile.Name = name;
		towerDefenseProjectile.Position = position;
		towerDefenseProjectile.gridPos = gridPos;
		towerDefenseProjectile.velocity = velocity;
		towerDefenseProjectile.camp = camp;
		AddChild(towerDefenseProjectile, forceReadableName: false, InternalMode.Disabled);
		return towerDefenseProjectile;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[PortalProjectileTransport] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreatePortalHitBox, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "endpoint", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreatePortalHitBox && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<AabbArea2D>(CreatePortalHitBox(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Node2D>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateProjectile && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectile>(CreateProjectile(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[4])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreatePortalHitBox && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<AabbArea2D>(CreatePortalHitBox(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Node2D>(in args[1])));
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
		if (method == MethodName.CreatePortalHitBox)
		{
			return true;
		}
		if (method == MethodName.CreateProjectile)
		{
			return true;
		}
		if (method == MethodName.Check)
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
