using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Registry/Battle/Feature/Fog/Object/TowerDefenseFogBatch.cs")]
public sealed class TowerDefenseFogBatch : Node
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Register = "Register";

		public static readonly StringName RequestVisibilityUpdate = "RequestVisibilityUpdate";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName ActiveCount = "ActiveCount";

		public static readonly StringName NeedsUpdateWhenNoLights = "NeedsUpdateWhenNoLights";

		public static readonly StringName _needsEmptyLightUpdate = "_needsEmptyLightUpdate";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const uint LightCollisionLayerMask = 8u;

	private readonly List<TowerDefenseFog> _active = new List<TowerDefenseFog>();

	private bool _needsEmptyLightUpdate = true;

	public int ActiveCount => _active.Count;

	internal bool NeedsUpdateWhenNoLights => _needsEmptyLightUpdate;

	public void Register(TowerDefenseFog fog)
	{
		if (GodotObject.IsInstanceValid(fog) && !_active.Contains(fog))
		{
			_active.Add(fog);
			_needsEmptyLightUpdate = true;
			fog.SetBatchManaged(managed: true);
			fog.BindBatch(this);
			SetPhysicsProcess(enable: true);
		}
	}

	public void RequestVisibilityUpdate()
	{
		if (_active.Count != 0)
		{
			_needsEmptyLightUpdate = true;
			SetPhysicsProcess(enable: true);
		}
	}

	public override void _Ready()
	{
		SetPhysicsProcess(_active.Count > 0);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_active.Count == 0)
		{
			SetPhysicsProcess(enable: false);
			return;
		}
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		IReadOnlyList<AabbAreaLayerRegistry.WorldRectSnapshot> worldRectSnapshotsForCollisionLayer = AabbAreaLayerRegistry.GetWorldRectSnapshotsForCollisionLayer(TowerDefenseManager.GetCharacterNode(), 8u);
		if (worldRectSnapshotsForCollisionLayer.Count == 0 && !_needsEmptyLightUpdate)
		{
			TowerDefensePerfProfiler.End("fog.batch", startTicks, _active.Count);
			return;
		}
		bool flag = true;
		for (int num = _active.Count - 1; num >= 0; num--)
		{
			TowerDefenseFog towerDefenseFog = _active[num];
			if (!GodotObject.IsInstanceValid(towerDefenseFog) || !towerDefenseFog.IsInsideTree())
			{
				_active.RemoveAt(num);
			}
			else
			{
				flag &= towerDefenseFog.BatchPhysicsUpdate(delta, worldRectSnapshotsForCollisionLayer);
			}
		}
		if (worldRectSnapshotsForCollisionLayer.Count == 0)
		{
			_needsEmptyLightUpdate = !flag;
		}
		else
		{
			_needsEmptyLightUpdate = true;
		}
		TowerDefensePerfProfiler.End("fog.batch", startTicks, _active.Count);
		if (_active.Count == 0)
		{
			SetPhysicsProcess(enable: false);
		}
	}

	public override void _ExitTree()
	{
		for (int i = 0; i < _active.Count; i++)
		{
			if (GodotObject.IsInstanceValid(_active[i]))
			{
				_active[i].SetBatchManaged(managed: false);
			}
		}
		_active.Clear();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.Register, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fog", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RequestVisibilityUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Register && args.Count == 1)
		{
			Register(VariantUtils.ConvertTo<TowerDefenseFog>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RequestVisibilityUpdate && args.Count == 0)
		{
			RequestVisibilityUpdate();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Register)
		{
			return true;
		}
		if (method == MethodName.RequestVisibilityUpdate)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._needsEmptyLightUpdate)
		{
			_needsEmptyLightUpdate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ActiveCount)
		{
			value = VariantUtils.CreateFrom<int>(ActiveCount);
			return true;
		}
		if (name == PropertyName.NeedsUpdateWhenNoLights)
		{
			value = VariantUtils.CreateFrom<bool>(NeedsUpdateWhenNoLights);
			return true;
		}
		if (name == PropertyName._needsEmptyLightUpdate)
		{
			value = VariantUtils.CreateFrom(in _needsEmptyLightUpdate);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._needsEmptyLightUpdate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ActiveCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.NeedsUpdateWhenNoLights, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._needsEmptyLightUpdate, Variant.From(in _needsEmptyLightUpdate));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._needsEmptyLightUpdate, out var value))
		{
			_needsEmptyLightUpdate = value.As<bool>();
		}
	}
}
