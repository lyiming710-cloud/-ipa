using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using Microsoft.Extensions.Logging;
using ZLogger;

[GlobalClass]
[ScriptPath("res://Core/ObjectManager/ObjectManager.cs")]
public class ObjectManager : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName PoolCreate = "PoolCreate";

		public static readonly StringName PoolPush = "PoolPush";

		public static readonly StringName TryPoolPush = "TryPoolPush";

		public static readonly StringName PoolPop = "PoolPop";

		public static readonly StringName GetPoolScene = "GetPoolScene";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName poolList = "poolList";

		public static readonly StringName _lastPoolMaintenanceFrame = "_lastPoolMaintenanceFrame";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private static readonly ILogger _logger = Log.CreateLogger<ObjectManager>();

	private const ulong PoolMaintenanceIntervalFrames = 60uL;

	private ulong _lastPoolMaintenanceFrame;

	public static ObjectManager Instance { get; private set; }

	public Array<PoolConfig> poolList { get; set; } = new Array<PoolConfig>();

	public override void _Ready()
	{
		Instance = this;
		ProjectileUpdateManager node = new ProjectileUpdateManager();
		AddChild(node, forceReadableName: false, InternalMode.Disabled);
		poolList = new Array<PoolConfig>();
		poolList.Resize(1000);
		for (int i = 0; i < 1000; i++)
		{
			poolList[i] = null;
		}
		PoolCreate("uid://bct1gvf1i8ohw", ObjectManagerConfig.OBJECT.damagePart, 64, "Refresh", "Recycle");
		PoolCreate("uid://dk3bkihnh1i0l", ObjectManagerConfig.OBJECT.SUN, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://d161xee5m0kkw", ObjectManagerConfig.OBJECT.SUN_BRAIN, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://da7lvlco511ds", ObjectManagerConfig.OBJECT.SUN_JALAPENO, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://boebeodp5s2g2", ObjectManagerConfig.OBJECT.SUN_QX, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://c4r7bn2wqxnat", ObjectManagerConfig.OBJECT.SUN_MAGIC, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://csynbfevdbiju", ObjectManagerConfig.OBJECT.COIN_SILVER, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://kbif4idtgolo", ObjectManagerConfig.OBJECT.COIN_GOLD, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://6b78y08u52f5", ObjectManagerConfig.OBJECT.COIN_DIAMOND, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://bnrh1k2cgopsn", ObjectManagerConfig.OBJECT.COIN_LUCKY_BAG, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://733w81lrellb", ObjectManagerConfig.OBJECT.COIN_TQ, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://c25ngfvpp0uo3", ObjectManagerConfig.OBJECT.COIN_YB1, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://1twddwaolt4r", ObjectManagerConfig.OBJECT.COIN_YB2, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://dsfn18qda4271", ObjectManagerConfig.OBJECT.COIN_GOLD_SHARD, 32, "Refresh", "Recycle", 8);
		PoolCreate("uid://ueeii5v2dl8q", ObjectManagerConfig.OBJECT.PARTICLES_SPLASH, 128, "Refresh", "Recycle", 24);
		PoolCreate("uid://lcop8me7nwde", ObjectManagerConfig.OBJECT.PARTICLES_RISE_DIRT, 64, "Refresh", "Recycle");
		PoolCreate("uid://b0wigigia32ny", ObjectManagerConfig.OBJECT.PARTICLES_ICE_TRAP, 64, "Refresh", "Recycle");
		PoolCreate("res://Script/Component/TowerDefense/Character/ShowHealthComponent/ShowHealthComponentView.tscn", ObjectManagerConfig.OBJECT.SHOW_HEALTH_VIEW, 256, "", "", 32).idleTrimFrames = 600uL;
	}

	public override void _ExitTree()
	{
		for (int i = 0; i < poolList.Count; i++)
		{
			PoolConfig poolConfig = poolList[i];
			if (GodotObject.IsInstanceValid(poolConfig))
			{
				poolConfig.ReleaseImmediately();
			}
		}
		poolList.Clear();
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public override void _PhysicsProcess(double _delta)
	{
		ulong physicsFrames = Engine.GetPhysicsFrames();
		if (physicsFrames < _lastPoolMaintenanceFrame)
		{
			_lastPoolMaintenanceFrame = physicsFrames;
		}
		if (physicsFrames - _lastPoolMaintenanceFrame >= 60)
		{
			_lastPoolMaintenanceFrame = physicsFrames;
			for (int i = 0; i < poolList.Count; i++)
			{
				poolList[i]?.Maintenance(physicsFrames);
			}
		}
	}

	public void Clear()
	{
		ProjectileUpdateManager.Instance?.Clear();
		BulletField.ClearRuntimeCaches();
		for (int i = 0; i < poolList.Count; i++)
		{
			PoolConfig poolConfig = poolList[i];
			if (GodotObject.IsInstanceValid(poolConfig))
			{
				poolConfig.Clear();
			}
		}
	}

	public PoolConfig PoolCreate(PackedScene scene, ObjectManagerConfig.OBJECT id, int maxNum, string popCallable = "", string pushCallable = "", int idleRetainNum = 16)
	{
		if (id < ObjectManagerConfig.OBJECT.NOONE || id >= ObjectManagerConfig.OBJECT.MAX)
		{
			ILogger logger = _logger;
			ZLoggerErrorInterpolatedStringHandler message = new ZLoggerErrorInterpolatedStringHandler(17, 1, logger, out var enabled);
			if (enabled)
			{
				message.AppendLiteral("Invalid pool ID: ");
				message.AppendFormatted((int)id, 0, null, "(int)id");
			}
			logger.ZLogError(ref message);
			return null;
		}
		if (poolList[(int)id] != null)
		{
			poolList[(int)id].Clear();
		}
		PoolConfig poolConfig = new PoolConfig();
		poolConfig.scene = scene;
		poolConfig.maxNum = maxNum;
		poolConfig.idleRetainNum = idleRetainNum;
		poolConfig.popCallable = popCallable;
		poolConfig.pushCallable = pushCallable;
		poolList[(int)id] = poolConfig;
		return poolConfig;
	}

	public PoolConfig PoolCreate(string scenePath, ObjectManagerConfig.OBJECT id, int maxNum, string popCallable = "", string pushCallable = "", int idleRetainNum = 16)
	{
		if (id < ObjectManagerConfig.OBJECT.NOONE || id >= ObjectManagerConfig.OBJECT.MAX)
		{
			ILogger logger = _logger;
			ZLoggerErrorInterpolatedStringHandler message = new ZLoggerErrorInterpolatedStringHandler(17, 1, logger, out var enabled);
			if (enabled)
			{
				message.AppendLiteral("Invalid pool ID: ");
				message.AppendFormatted((int)id, 0, null, "(int)id");
			}
			logger.ZLogError(ref message);
			return null;
		}
		if (poolList[(int)id] != null)
		{
			poolList[(int)id].Clear();
		}
		PoolConfig poolConfig = new PoolConfig();
		poolConfig.scenePath = scenePath;
		poolConfig.maxNum = maxNum;
		poolConfig.idleRetainNum = idleRetainNum;
		poolConfig.popCallable = popCallable;
		poolConfig.pushCallable = pushCallable;
		poolList[(int)id] = poolConfig;
		return poolConfig;
	}

	public static void PoolPush(ObjectManagerConfig.OBJECT id, Node node)
	{
		if (!GodotObject.IsInstanceValid(node) || TryPoolPush(id, node))
		{
			return;
		}
		ZLoggerErrorInterpolatedStringHandler message;
		bool enabled;
		if (id < ObjectManagerConfig.OBJECT.NOONE || id >= ObjectManagerConfig.OBJECT.MAX)
		{
			ILogger logger = _logger;
			ILogger logger2 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(17, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Invalid pool ID: ");
				message.AppendFormatted((int)id, 0, null, "(int)id");
			}
			logger2.ZLogError(ref message);
		}
		else
		{
			ILogger logger = _logger;
			ILogger logger3 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(29, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Pool not initialized for ID: ");
				message.AppendFormatted((int)id, 0, null, "(int)id");
			}
			logger3.ZLogError(ref message);
		}
	}

	public static bool TryPoolPush(ObjectManagerConfig.OBJECT id, Node node)
	{
		if (!GodotObject.IsInstanceValid(node) || !GodotObject.IsInstanceValid(Instance) || id < ObjectManagerConfig.OBJECT.NOONE || (int)id >= Instance.poolList.Count)
		{
			return false;
		}
		PoolConfig poolConfig = Instance.poolList[(int)id];
		if (!GodotObject.IsInstanceValid(poolConfig))
		{
			return false;
		}
		poolConfig.Push(node);
		return true;
	}

	public static Node PoolPop(ObjectManagerConfig.OBJECT id, Node parent)
	{
		if (!GodotObject.IsInstanceValid(parent))
		{
			return null;
		}
		ZLoggerErrorInterpolatedStringHandler message;
		bool enabled;
		if (id < ObjectManagerConfig.OBJECT.NOONE || id >= ObjectManagerConfig.OBJECT.MAX)
		{
			ILogger logger = _logger;
			ILogger logger2 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(17, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Invalid pool ID: ");
				message.AppendFormatted((int)id, 0, null, "(int)id");
			}
			logger2.ZLogError(ref message);
			return null;
		}
		if (Instance.poolList[(int)id] == null)
		{
			ILogger logger = _logger;
			ILogger logger3 = logger;
			message = new ZLoggerErrorInterpolatedStringHandler(29, 1, logger, out enabled);
			if (enabled)
			{
				message.AppendLiteral("Pool not initialized for ID: ");
				message.AppendFormatted((int)id, 0, null, "(int)id");
			}
			logger3.ZLogError(ref message);
			return null;
		}
		return Instance.poolList[(int)id].Pop(parent);
	}

	public static PackedScene GetPoolScene(ObjectManagerConfig.OBJECT id)
	{
		if (!GodotObject.IsInstanceValid(Instance) || id < ObjectManagerConfig.OBJECT.NOONE || (int)id >= Instance.poolList.Count)
		{
			return null;
		}
		PoolConfig poolConfig = Instance.poolList[(int)id];
		if (!GodotObject.IsInstanceValid(poolConfig))
		{
			return null;
		}
		if (GodotObject.IsInstanceValid(poolConfig.scene))
		{
			return poolConfig.scene;
		}
		if (!string.IsNullOrWhiteSpace(poolConfig.scenePath))
		{
			return GD.Load<PackedScene>(poolConfig.scenePath);
		}
		return null;
	}

	public ObjectManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/ObjectManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PoolCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "maxNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "popCallable", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "pushCallable", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "idleRetainNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PoolPush, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryPoolPush, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.PoolPop, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetPoolScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.PoolCreate && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<PoolConfig>(PoolCreate(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<int>(in args[5])));
			return true;
		}
		if (method == MethodName.PoolPush && args.Count == 2)
		{
			PoolPush(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryPoolPush && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryPoolPush(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.PoolPop && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Node>(PoolPop(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.GetPoolScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetPoolScene(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.PoolPush && args.Count == 2)
		{
			PoolPush(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryPoolPush && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryPoolPush(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.PoolPop && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Node>(PoolPop(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.GetPoolScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetPoolScene(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.PoolCreate)
		{
			return true;
		}
		if (method == MethodName.PoolPush)
		{
			return true;
		}
		if (method == MethodName.TryPoolPush)
		{
			return true;
		}
		if (method == MethodName.PoolPop)
		{
			return true;
		}
		if (method == MethodName.GetPoolScene)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.poolList)
		{
			poolList = VariantUtils.ConvertToArray<PoolConfig>(in value);
			return true;
		}
		if (name == PropertyName._lastPoolMaintenanceFrame)
		{
			_lastPoolMaintenanceFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.poolList)
		{
			value = VariantUtils.CreateFromArray(poolList);
			return true;
		}
		if (name == PropertyName._lastPoolMaintenanceFrame)
		{
			value = VariantUtils.CreateFrom(in _lastPoolMaintenanceFrame);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._lastPoolMaintenanceFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.poolList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.poolList, Variant.CreateFrom(poolList));
		info.AddProperty(PropertyName._lastPoolMaintenanceFrame, Variant.From(in _lastPoolMaintenanceFrame));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.poolList, out var value))
		{
			poolList = value.AsGodotArray<PoolConfig>();
		}
		if (info.TryGetProperty(PropertyName._lastPoolMaintenanceFrame, out var value2))
		{
			_lastPoolMaintenanceFrame = value2.As<ulong>();
		}
	}
}
