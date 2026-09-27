using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Effect/Once/TowerDefenseEffectParticlesOnce.cs")]
public class TowerDefenseEffectParticlesOnce : TowerDefenseEffectBase
{
	public new class MethodName : TowerDefenseEffectBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName Recycle = "Recycle";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitScene = "InitScene";

		public static readonly StringName Init = "Init";

		public static readonly StringName Finish = "Finish";

		public static readonly StringName ResumeRetained = "ResumeRetained";

		public static readonly StringName RequestParticleRestart = "RequestParticleRestart";

		public static readonly StringName FlushParticleRestart = "FlushParticleRestart";

		public static readonly StringName StopParticleTree = "StopParticleTree";

		public static readonly StringName IsSimpleBuiltInParticleScene = "IsSimpleBuiltInParticleScene";

		public static readonly StringName HasOnlyBuiltInRootScript = "HasOnlyBuiltInRootScript";
	}

	public new class PropertyName : TowerDefenseEffectBase.PropertyName
	{
		public static readonly StringName _particleRestartRequested = "_particleRestartRequested";

		public static readonly StringName _particleRestartQueued = "_particleRestartQueued";

		public static readonly StringName _retainedScene = "_retainedScene";

		public static readonly StringName _retainedIdle = "_retainedIdle";

		public static readonly StringName objectId = "objectId";

		public static readonly StringName particles = "particles";
	}

	public new class SignalName : TowerDefenseEffectBase.SignalName
	{
	}

	private const int RetainedPerSceneLimit = 16;

	private const string BuiltInParticleRoot = "res://Prefab/Particles/";

	private const string BuiltInParticleScriptPath = "res://Extends/Particles/GPUParticles2DOnece.cs";

	private static readonly Dictionary<PackedScene, Stack<TowerDefenseEffectParticlesOnce>> RetainedByScene = new Dictionary<PackedScene, Stack<TowerDefenseEffectParticlesOnce>>();

	private static readonly Dictionary<PackedScene, bool> SimpleParticleSceneCache = new Dictionary<PackedScene, bool>();

	private static Script _builtInParticleScript;

	private bool _particleRestartRequested;

	private bool _particleRestartQueued;

	private static PackedScene _towerDefenseEffectParticlesOnce;

	private PackedScene _retainedScene;

	private bool _retainedIdle;

	[Export(PropertyHint.None, "")]
	public ObjectManagerConfig.OBJECT objectId;

	[Export(PropertyHint.None, "")]
	public GPUParticles2DOnece particles;

	private static PackedScene TOWER_DEFENSE_EFFECT_PARTICLES_ONCE => _towerDefenseEffectParticlesOnce ?? (_towerDefenseEffectParticlesOnce = GD.Load<PackedScene>("uid://dbyd0mqkya1j3"));

	public static TowerDefenseEffectParticlesOnce Create()
	{
		return TOWER_DEFENSE_EFFECT_PARTICLES_ONCE.Instantiate<TowerDefenseEffectParticlesOnce>(PackedScene.GenEditState.Disabled);
	}

	public static bool TrySpawnRetained(PackedScene particlesScene, Node2D parent, Vector2I gridPosition, Vector2 globalPosition, out TowerDefenseEffectParticlesOnce effect)
	{
		effect = null;
		if (particlesScene == null || !GodotObject.IsInstanceValid(parent) || !parent.IsInsideTree() || !globalPosition.IsFinite() || !IsSimpleBuiltInParticleScene(particlesScene))
		{
			return false;
		}
		if (RetainedByScene.TryGetValue(particlesScene, out var value))
		{
			while (value.Count > 0)
			{
				TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = value.Pop();
				if (GodotObject.IsInstanceValid(towerDefenseEffectParticlesOnce) && towerDefenseEffectParticlesOnce.IsInsideTree() && towerDefenseEffectParticlesOnce._retainedIdle)
				{
					effect = towerDefenseEffectParticlesOnce;
					break;
				}
			}
		}
		if (!GodotObject.IsInstanceValid(effect))
		{
			Node node = particlesScene.Instantiate(PackedScene.GenEditState.Disabled);
			if (!(node is GPUParticles2DOnece scene))
			{
				node?.Free();
				return false;
			}
			effect = Create();
			effect._retainedScene = particlesScene;
			effect.InitScene(scene);
			effect.gridPos = gridPosition;
			parent.AddChild(effect, forceReadableName: false, InternalMode.Disabled);
		}
		else
		{
			if (effect.GetParent() != parent)
			{
				effect.Reparent(parent);
			}
			effect.gridPos = gridPosition;
			effect.GlobalPosition = globalPosition;
			effect.ResumeRetained();
		}
		effect.GlobalPosition = globalPosition;
		return true;
	}

	public void Refresh()
	{
		AddToGroup("Effect");
		_retainedIdle = false;
		Visible = true;
		SetPhysicsProcess(enable: true);
		if (GodotObject.IsInstanceValid(particles))
		{
			particles.Visible = true;
			RequestParticleRestart();
		}
	}

	public void Recycle()
	{
		RemoveFromGroup("Effect");
		Visible = false;
		SetPhysicsProcess(enable: false);
		_particleRestartRequested = false;
		StopParticleTree(particles);
	}

	public override void _Ready()
	{
		base._Ready();
		if (particles != null)
		{
			if (!particles.IsConnected(GpuParticles2D.SignalName.Finished, Callable.From(Finish)))
			{
				particles.Finished += Finish;
			}
			RequestParticleRestart();
		}
	}

	public void InitScene(GPUParticles2DOnece scene)
	{
		AddToGroup("Effect");
		_retainedIdle = false;
		particles = scene;
		StopParticleTree(particles);
		AddChild(particles, forceReadableName: false, InternalMode.Disabled);
		if (!particles.IsConnected(GpuParticles2D.SignalName.Finished, Callable.From(Finish)))
		{
			particles.Finished += Finish;
		}
		if (IsInsideTree())
		{
			RequestParticleRestart();
		}
	}

	public void Init(PackedScene particlesScene)
	{
		InitScene(particlesScene.Instantiate<GPUParticles2DOnece>(PackedScene.GenEditState.Disabled));
	}

	public void Finish()
	{
		if (_retainedIdle)
		{
			return;
		}
		if (_retainedScene != null && IsInsideTree())
		{
			if (!RetainedByScene.TryGetValue(_retainedScene, out var value))
			{
				value = new Stack<TowerDefenseEffectParticlesOnce>(16);
				RetainedByScene.Add(_retainedScene, value);
			}
			if (value.Count < 16)
			{
				_retainedIdle = true;
				StopParticleTree(particles);
				Recycle();
				value.Push(this);
				return;
			}
		}
		if (objectId == ObjectManagerConfig.OBJECT.NOONE || !ObjectManager.TryPoolPush(objectId, this))
		{
			QueueFree();
		}
	}

	private void ResumeRetained()
	{
		_retainedIdle = false;
		Visible = true;
		AddToGroup("Effect");
		SetPhysicsProcess(enable: true);
		if (GodotObject.IsInstanceValid(particles))
		{
			particles.Visible = true;
			RequestParticleRestart();
		}
	}

	private void RequestParticleRestart()
	{
		_particleRestartRequested = true;
		if (!_particleRestartQueued)
		{
			_particleRestartQueued = true;
			Callable.From(FlushParticleRestart).CallDeferred();
		}
	}

	private void FlushParticleRestart()
	{
		_particleRestartQueued = false;
		if (_particleRestartRequested && !_retainedIdle && IsInsideTree() && Visible && GodotObject.IsInstanceValid(particles))
		{
			_particleRestartRequested = false;
			particles.Visible = true;
			particles.Init();
		}
	}

	private static void StopParticleTree(GpuParticles2D root)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return;
		}
		root.Emitting = false;
		foreach (Node child in root.GetChildren())
		{
			if (child is GpuParticles2D root2)
			{
				StopParticleTree(root2);
			}
		}
	}

	private static bool IsSimpleBuiltInParticleScene(PackedScene scene)
	{
		if (SimpleParticleSceneCache.TryGetValue(scene, out var value))
		{
			return value;
		}
		bool flag = false;
		string text = scene.ResourcePath ?? string.Empty;
		if (text.StartsWith("res://Prefab/Particles/", StringComparison.Ordinal) && text.EndsWith(".tscn", StringComparison.OrdinalIgnoreCase) && ResourceLoader.Load<PackedScene>(text, "", ResourceLoader.CacheMode.Reuse) == scene)
		{
			SceneState state = scene.GetState();
			flag = GodotObject.IsInstanceValid(state) && state.GetNodeCount() > 0 && string.Equals(state.GetNodeType(0).ToString(), "GPUParticles2D", StringComparison.Ordinal) && HasOnlyBuiltInRootScript(state);
		}
		SimpleParticleSceneCache[scene] = flag;
		return flag;
	}

	private static bool HasOnlyBuiltInRootScript(SceneState state)
	{
		if (!GodotObject.IsInstanceValid(_builtInParticleScript))
		{
			_builtInParticleScript = ResourceLoader.Load<Script>("res://Extends/Particles/GPUParticles2DOnece.cs", "", ResourceLoader.CacheMode.Reuse);
		}
		if (!GodotObject.IsInstanceValid(_builtInParticleScript))
		{
			return false;
		}
		for (int i = 0; i < state.GetNodeCount(); i++)
		{
			Script script = null;
			for (int j = 0; j < state.GetNodePropertyCount(i); j++)
			{
				if (string.Equals(state.GetNodePropertyName(i, j).ToString(), "script", StringComparison.Ordinal))
				{
					script = state.GetNodePropertyValue(i, j).AsGodotObject() as Script;
					break;
				}
			}
			if (i == 0)
			{
				if (script != _builtInParticleScript)
				{
					return false;
				}
			}
			else if (GodotObject.IsInstanceValid(script))
			{
				return false;
			}
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Recycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GPUParticles2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "particlesScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResumeRetained, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestParticleRestart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlushParticleRestart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopParticleTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GPUParticles2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsSimpleBuiltInParticleScene, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasOnlyBuiltInRootScript, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "state", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SceneState"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectParticlesOnce>(Create());
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.Recycle && args.Count == 0)
		{
			Recycle();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.InitScene && args.Count == 1)
		{
			InitScene(VariantUtils.ConvertTo<GPUParticles2DOnece>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<PackedScene>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		if (method == MethodName.ResumeRetained && args.Count == 0)
		{
			ResumeRetained();
			ret = default;
			return true;
		}
		if (method == MethodName.RequestParticleRestart && args.Count == 0)
		{
			RequestParticleRestart();
			ret = default;
			return true;
		}
		if (method == MethodName.FlushParticleRestart && args.Count == 0)
		{
			FlushParticleRestart();
			ret = default;
			return true;
		}
		if (method == MethodName.StopParticleTree && args.Count == 1)
		{
			StopParticleTree(VariantUtils.ConvertTo<GpuParticles2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSimpleBuiltInParticleScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSimpleBuiltInParticleScene(VariantUtils.ConvertTo<PackedScene>(in args[0])));
			return true;
		}
		if (method == MethodName.HasOnlyBuiltInRootScript && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasOnlyBuiltInRootScript(VariantUtils.ConvertTo<SceneState>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectParticlesOnce>(Create());
			return true;
		}
		if (method == MethodName.StopParticleTree && args.Count == 1)
		{
			StopParticleTree(VariantUtils.ConvertTo<GpuParticles2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSimpleBuiltInParticleScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSimpleBuiltInParticleScene(VariantUtils.ConvertTo<PackedScene>(in args[0])));
			return true;
		}
		if (method == MethodName.HasOnlyBuiltInRootScript && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasOnlyBuiltInRootScript(VariantUtils.ConvertTo<SceneState>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.Recycle)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.InitScene)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		if (method == MethodName.ResumeRetained)
		{
			return true;
		}
		if (method == MethodName.RequestParticleRestart)
		{
			return true;
		}
		if (method == MethodName.FlushParticleRestart)
		{
			return true;
		}
		if (method == MethodName.StopParticleTree)
		{
			return true;
		}
		if (method == MethodName.IsSimpleBuiltInParticleScene)
		{
			return true;
		}
		if (method == MethodName.HasOnlyBuiltInRootScript)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._particleRestartRequested)
		{
			_particleRestartRequested = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._particleRestartQueued)
		{
			_particleRestartQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._retainedScene)
		{
			_retainedScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName._retainedIdle)
		{
			_retainedIdle = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.objectId)
		{
			objectId = VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in value);
			return true;
		}
		if (name == PropertyName.particles)
		{
			particles = VariantUtils.ConvertTo<GPUParticles2DOnece>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._particleRestartRequested)
		{
			value = VariantUtils.CreateFrom(in _particleRestartRequested);
			return true;
		}
		if (name == PropertyName._particleRestartQueued)
		{
			value = VariantUtils.CreateFrom(in _particleRestartQueued);
			return true;
		}
		if (name == PropertyName._retainedScene)
		{
			value = VariantUtils.CreateFrom(in _retainedScene);
			return true;
		}
		if (name == PropertyName._retainedIdle)
		{
			value = VariantUtils.CreateFrom(in _retainedIdle);
			return true;
		}
		if (name == PropertyName.objectId)
		{
			value = VariantUtils.CreateFrom(in objectId);
			return true;
		}
		if (name == PropertyName.particles)
		{
			value = VariantUtils.CreateFrom(in particles);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._particleRestartRequested, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._particleRestartQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._retainedScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._retainedIdle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.objectId, PropertyHint.Enum, "NOONE:0,PROJECTILE:1,damagePart:2,GAMECOLLECT:10,SUN:11,SUN_BRAIN:12,SUN_JALAPENO:13,SUN_QX:14,SUN_MAGIC:15,COIN:20,COIN_SILVER:21,COIN_GOLD:22,COIN_DIAMOND:23,COIN_LUCKY_BAG:24,COIN_TQ:25,COIN_YB1:26,COIN_YB2:27,COIN_GOLD_SHARD:28,PARTICLES_SPLASH:201,PARTICLES_RISE_DIRT:202,PARTICLES_ICE_TRAP:203,SHOW_HEALTH_VIEW:301,MAX:1000", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.particles, PropertyHint.NodeType, "GPUParticles2DOnece", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._particleRestartRequested, Variant.From(in _particleRestartRequested));
		info.AddProperty(PropertyName._particleRestartQueued, Variant.From(in _particleRestartQueued));
		info.AddProperty(PropertyName._retainedScene, Variant.From(in _retainedScene));
		info.AddProperty(PropertyName._retainedIdle, Variant.From(in _retainedIdle));
		info.AddProperty(PropertyName.objectId, Variant.From(in objectId));
		info.AddProperty(PropertyName.particles, Variant.From(in particles));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._particleRestartRequested, out var value))
		{
			_particleRestartRequested = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._particleRestartQueued, out var value2))
		{
			_particleRestartQueued = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._retainedScene, out var value3))
		{
			_retainedScene = value3.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName._retainedIdle, out var value4))
		{
			_retainedIdle = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.objectId, out var value5))
		{
			objectId = value5.As<ObjectManagerConfig.OBJECT>();
		}
		if (info.TryGetProperty(PropertyName.particles, out var value6))
		{
			particles = value6.As<GPUParticles2DOnece>();
		}
	}
}
