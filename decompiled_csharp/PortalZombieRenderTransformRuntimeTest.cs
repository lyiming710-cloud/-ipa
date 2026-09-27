using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/PortalZombieRenderTransformRuntimeTest.cs")]
public class PortalZombieRenderTransformRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreatePortal = "CreatePortal";

		public static readonly StringName CreatePortalTallnut = "CreatePortalTallnut";

		public static readonly StringName InvokeCharacterTeleport = "InvokeCharacterTeleport";

		public static readonly StringName InvokePortalTallnutCharacterTeleport = "InvokePortalTallnutCharacterTeleport";

		public static readonly StringName ReadRenderTransformDirty = "ReadRenderTransformDirty";

		public static readonly StringName ReadRenderedTransform = "ReadRenderedTransform";

		public static readonly StringName Check = "Check";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string NormalZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private readonly List<string> _failures = new List<string>();

	private int _checks;

	public override async void _Ready()
	{
		AdobeAnimateRenderBackend previousBackend = Global.Instance.adobeAnimateRenderBackend;
		TowerDefenseZombieNormal zombie = null;
		PortalZombieRenderTransformPortalStub portal = null;
		PortalZombieRenderTransformTallnutStub portalTallnutSource = null;
		PortalZombieRenderTransformTallnutStub portalTallnutDestination = null;
		try
		{
			try
			{
				Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
				zombie = Instantiate<TowerDefenseZombieNormal>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
				Check(GodotObject.IsInstanceValid(zombie), "测试必须实例化真实普通僵尸场景。");
				if (!GodotObject.IsInstanceValid(zombie))
				{
					goto end_IL_005c;
				}
				zombie.inGame = false;
				zombie.editorPreviewMode = false;
				zombie.gridPos = new Vector2I(2, 1);
				zombie.GlobalPosition = new Vector2(250f, 152f);
				AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				portal = CreatePortal();
				AddChild(portal, forceReadableName: false, InternalMode.Disabled);
				await WaitProcessFrames(4);
				Check(zombie.config?.name == "ZombieNormal", "测试对象必须保留普通僵尸正式配置。");
				Check(GodotObject.IsInstanceValid(zombie.sprite), "真实普通僵尸必须提供 Adobe Animate 根贴图。");
				if (!GodotObject.IsInstanceValid(zombie.sprite))
				{
					goto end_IL_005c;
				}
				zombie.sprite.ClearRetainedRootMotionForStoppedMovement();
				Check(!ReadRenderTransformDirty(zombie.sprite), "传送前必须先建立干净的渲染变换缓存。");
				InvokeCharacterTeleport(portal, zombie);
				Check(zombie.GetLogicalGlobalPosition().IsEqualApprox(portal.protalNode2.GlobalPosition), "传送门必须把普通僵尸的逻辑位置送到出口。");
				Check(zombie.gridPos == portal.gridPos2, "传送门必须同步普通僵尸的出口格子。");
				Check(!ReadRenderTransformDirty(zombie.sprite) && ReadRenderedTransform(zombie.sprite).IsEqualApprox(zombie.sprite.GlobalTransform), "传送后必须在同一次坐标写入中推进普通僵尸身体，不能等待后续脏标记。");
				AdobeAnimateRenderManager.RenderActive(new AdobeAnimateSprite[1] { zombie.sprite }, activeListAlreadyValidated: true, renderRootsAlreadyFiltered: true);
				Check(ReadRenderedTransform(zombie.sprite).IsEqualApprox(zombie.sprite.GlobalTransform), "渲染事务必须消费出口位置，不能继续复用入口处贴图变换。");
				portalTallnutSource = CreatePortalTallnut("PortalTallnutSource", new Vector2(250f, 152f), new Vector2I(2, 1));
				portalTallnutDestination = CreatePortalTallnut("PortalTallnutDestination", new Vector2(750f, 304f), new Vector2I(7, 3));
				AddChild(portalTallnutSource, forceReadableName: false, InternalMode.Disabled);
				AddChild(portalTallnutDestination, forceReadableName: false, InternalMode.Disabled);
				Check(GetTree().GetNodeCountInGroup("PorTallnut") == 2, "测试必须提供唯一一对传送门高坚果。");
				zombie.GlobalPosition = portalTallnutSource.GlobalPosition;
				zombie.gridPos = portalTallnutSource.gridPos;
				zombie.sprite.ClearRetainedRootMotionForStoppedMovement();
				Check(!ReadRenderTransformDirty(zombie.sprite), "进入传送门高坚果前必须先建立干净的身体渲染变换缓存。");
				InvokePortalTallnutCharacterTeleport(portalTallnutSource, zombie);
				Vector2 other = portalTallnutDestination.GlobalPosition - new Vector2(11f, 0f);
				Check(zombie.GetLogicalGlobalPosition().IsEqualApprox(other), "传送门高坚果必须把普通僵尸的逻辑位置送到出口。");
				Check(zombie.gridPos == portalTallnutDestination.gridPos, "传送门高坚果必须同步普通僵尸的出口格子。");
				Check(!ReadRenderTransformDirty(zombie.sprite) && ReadRenderedTransform(zombie.sprite).IsEqualApprox(zombie.sprite.GlobalTransform), "传送门高坚果传送后必须在同一次坐标写入中推进普通僵尸身体。");
				AdobeAnimateRenderManager.RenderActive(new AdobeAnimateSprite[1] { zombie.sprite }, activeListAlreadyValidated: true, renderRootsAlreadyFiltered: true);
				Check(ReadRenderedTransform(zombie.sprite).IsEqualApprox(zombie.sprite.GlobalTransform), "传送门高坚果的渲染事务必须使用出口身体变换，不能保留入口身体。");
				zombie.GlobalPosition = new Vector2(250f, 152f);
				zombie.gridPos = new Vector2I(2, 1);
				zombie.sprite.ClearRetainedRootMotionForStoppedMovement();
				Check(!ReadRenderTransformDirty(zombie.sprite), "远端传送同步前必须先建立干净的身体渲染变换缓存。");
				PortalZombieRenderTransformNetworkContextStub portalZombieRenderTransformNetworkContextStub = new PortalZombieRenderTransformNetworkContextStub();
				portalZombieRenderTransformNetworkContextStub.ZombieSyncVelocities[1] = new Vector2(-20f, 0f);
				ZombieStateReplicator replicator = new ZombieStateReplicator(portalZombieRenderTransformNetworkContextStub);
				Dictionary zombieData = new Dictionary
				{
					["x"] = 750f,
					["y"] = 304f,
					["g"] = 3,
					["tp"] = true
				};
				InvokeRemoteZombieTeleport(replicator, zombie, zombieData);
				Check(zombie.GetLogicalGlobalPosition().IsEqualApprox(new Vector2(750f, 304f)), "远端同步必须把普通僵尸的逻辑位置直接送到传送出口。");
				Check(zombie.gridPos.Y == 3, "远端同步必须应用传送出口行。");
				Check(!ReadRenderTransformDirty(zombie.sprite) && ReadRenderedTransform(zombie.sprite).IsEqualApprox(zombie.sprite.GlobalTransform), "远端传送同步必须在同一次坐标写入中推进普通僵尸身体。");
				Check(!portalZombieRenderTransformNetworkContextStub.ZombieSyncVelocities.ContainsKey(1), "远端传送同步必须清除入口处的旧估算速度。");
				AdobeAnimateRenderManager.RenderActive(new AdobeAnimateSprite[1] { zombie.sprite }, activeListAlreadyValidated: true, renderRootsAlreadyFiltered: true);
				Check(ReadRenderedTransform(zombie.sprite).IsEqualApprox(zombie.sprite.GlobalTransform), "远端传送后的渲染事务必须使用出口身体变换，不能保留入口身体。");
				goto end_IL_0053;
				end_IL_005c:;
			}
			catch (Exception value)
			{
				_failures.Add($"运行测试出现异常：{value}");
				goto end_IL_0053;
			}
			end_IL_0053:;
		}
		finally
		{
			Global.Instance.adobeAnimateRenderBackend = previousBackend;
			if (GodotObject.IsInstanceValid(portal))
			{
				portal.QueueFree();
			}
			if (GodotObject.IsInstanceValid(portalTallnutSource))
			{
				portalTallnutSource.QueueFree();
			}
			if (GodotObject.IsInstanceValid(portalTallnutDestination))
			{
				portalTallnutDestination.QueueFree();
			}
			if (GodotObject.IsInstanceValid(zombie))
			{
				zombie.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			Finish();
		}
	}

	private static PortalZombieRenderTransformPortalStub CreatePortal()
	{
		PortalZombieRenderTransformPortalStub portalZombieRenderTransformPortalStub = new PortalZombieRenderTransformPortalStub
		{
			Name = "PortalZombieRenderTransformPortal",
			gridPos1 = new Vector2I(2, 1),
			gridPos2 = new Vector2I(7, 3),
			gridSize = new Vector2(100f, 76f),
			protalNode1 = new Node2D
			{
				Name = "PortalEndpoint1",
				Position = new Vector2(250f, 152f)
			},
			protalNode2 = new Node2D
			{
				Name = "PortalEndpoint2",
				Position = new Vector2(750f, 304f)
			}
		};
		portalZombieRenderTransformPortalStub.AddChild(portalZombieRenderTransformPortalStub.protalNode1, forceReadableName: false, InternalMode.Disabled);
		portalZombieRenderTransformPortalStub.AddChild(portalZombieRenderTransformPortalStub.protalNode2, forceReadableName: false, InternalMode.Disabled);
		return portalZombieRenderTransformPortalStub;
	}

	private static PortalZombieRenderTransformTallnutStub CreatePortalTallnut(string name, Vector2 position, Vector2I gridPosition)
	{
		return new PortalZombieRenderTransformTallnutStub
		{
			Name = name,
			Position = position,
			gridPos = gridPosition
		};
	}

	private static void InvokeCharacterTeleport(TowerDefensePortal portal, TowerDefenseCharacter zombie)
	{
		System.Reflection.MethodInfo? method = typeof(TowerDefensePortal).GetMethod("TryTeleport", BindingFlags.Instance | BindingFlags.NonPublic);
		if (method == null)
		{
			throw new MissingMethodException(typeof(TowerDefensePortal).FullName, "TryTeleport");
		}
		method.Invoke(portal, new object[2] { zombie, true });
	}

	private static void InvokePortalTallnutCharacterTeleport(TowerDefensePlantPorTallnut portalTallnut, TowerDefenseCharacter zombie)
	{
		System.Reflection.MethodInfo? method = typeof(TowerDefensePlantPorTallnut).GetMethod("HandleOverlap", BindingFlags.Instance | BindingFlags.NonPublic);
		if (method == null)
		{
			throw new MissingMethodException(typeof(TowerDefensePlantPorTallnut).FullName, "HandleOverlap");
		}
		method.Invoke(portalTallnut, new object[1] { zombie });
	}

	private static void InvokeRemoteZombieTeleport(ZombieStateReplicator replicator, TowerDefenseZombie zombie, Dictionary zombieData)
	{
		System.Reflection.MethodInfo? method = typeof(ZombieStateReplicator).GetMethod("ApplyZombieDelta", BindingFlags.Instance | BindingFlags.NonPublic);
		if (method == null)
		{
			throw new MissingMethodException(typeof(ZombieStateReplicator).FullName, "ApplyZombieDelta");
		}
		method.Invoke(replicator, new object[3] { 1, zombie, zombieData });
	}

	private static bool ReadRenderTransformDirty(AdobeAnimateSprite sprite)
	{
		return ReadPrivateField<bool>(sprite, "_renderGlobalTransformDirty");
	}

	private static Transform2D ReadRenderedTransform(AdobeAnimateSprite sprite)
	{
		return ReadPrivateField<Transform2D>(sprite, "_renderGlobalTransform");
	}

	private static T ReadPrivateField<T>(object owner, string fieldName)
	{
		FieldInfo? field = typeof(AdobeAnimateSprite).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
		if (field == null)
		{
			throw new MissingFieldException(typeof(AdobeAnimateSprite).FullName, fieldName);
		}
		return (T)field.GetValue(owner);
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures.Add(message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PushError("[PortalZombieRenderTransform] " + failure);
		}
		bool flag = _failures.Count == 0 && _checks == 20;
		GD.Print($"PORTAL_ZOMBIE_RENDER_TRANSFORM_RESULT passed={flag} checks={_checks} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static T Instantiate<T>(string path) where T : Node
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(9)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CreatePortal, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CreatePortalTallnut, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.InvokeCharacterTeleport, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "portal", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.InvokePortalTallnutCharacterTeleport, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "portalTallnut", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ReadRenderTransformDirty, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ReadRenderedTransform, new Godot.Bridge.PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Finish, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.CreatePortal && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PortalZombieRenderTransformPortalStub>(CreatePortal());
			return true;
		}
		if (method == MethodName.CreatePortalTallnut && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<PortalZombieRenderTransformTallnutStub>(CreatePortalTallnut(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2])));
			return true;
		}
		if (method == MethodName.InvokeCharacterTeleport && args.Count == 2)
		{
			InvokeCharacterTeleport(VariantUtils.ConvertTo<TowerDefensePortal>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvokePortalTallnutCharacterTeleport && args.Count == 2)
		{
			InvokePortalTallnutCharacterTeleport(VariantUtils.ConvertTo<TowerDefensePlantPorTallnut>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadRenderTransformDirty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadRenderTransformDirty(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadRenderedTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ReadRenderedTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreatePortal && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PortalZombieRenderTransformPortalStub>(CreatePortal());
			return true;
		}
		if (method == MethodName.CreatePortalTallnut && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<PortalZombieRenderTransformTallnutStub>(CreatePortalTallnut(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2])));
			return true;
		}
		if (method == MethodName.InvokeCharacterTeleport && args.Count == 2)
		{
			InvokeCharacterTeleport(VariantUtils.ConvertTo<TowerDefensePortal>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvokePortalTallnutCharacterTeleport && args.Count == 2)
		{
			InvokePortalTallnutCharacterTeleport(VariantUtils.ConvertTo<TowerDefensePlantPorTallnut>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadRenderTransformDirty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadRenderTransformDirty(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadRenderedTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ReadRenderedTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
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
		if (method == MethodName.CreatePortal)
		{
			return true;
		}
		if (method == MethodName.CreatePortalTallnut)
		{
			return true;
		}
		if (method == MethodName.InvokeCharacterTeleport)
		{
			return true;
		}
		if (method == MethodName.InvokePortalTallnutCharacterTeleport)
		{
			return true;
		}
		if (method == MethodName.ReadRenderTransformDirty)
		{
			return true;
		}
		if (method == MethodName.ReadRenderedTransform)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Finish)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
	}
}
