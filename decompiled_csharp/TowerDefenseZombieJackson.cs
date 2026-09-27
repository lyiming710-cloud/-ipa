using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter2/Jackson/Scene/TowerDefenseZombieJackson.cs")]
public class TowerDefenseZombieJackson : TowerDefenseZombie, IJackson, INetworkDancerOwner
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName RemoveDancer = "RemoveDancer";

		public static readonly StringName IsSameDancer = "IsSameDancer";

		public static readonly StringName SetNetworkDancer = "SetNetworkDancer";

		public static readonly StringName ApplyPendingNetworkDancers = "ApplyPendingNetworkDancers";

		public static readonly StringName ReleaseDancerOwnership = "ReleaseDancerOwnership";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName Hypnoses = "Hypnoses";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName HasDancingRuntime = "HasDancingRuntime";

		public static readonly StringName HasDancingComponent = "HasDancingComponent";

		public static readonly StringName dancerPacketName = "dancerPacketName";

		public static readonly StringName _pendingNetworkDancers = "_pendingNetworkDancers";

		public static readonly StringName _dancerPacketName = "_dancerPacketName";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	public DancingComponent dancingComponent;

	private readonly TowerDefenseCharacter[] _pendingNetworkDancers = new TowerDefenseCharacter[4];

	private string _dancerPacketName = "";

	private bool HasDancingRuntime
	{
		get
		{
			DancingComponent dancingComponent = this.dancingComponent;
			if (dancingComponent == null)
			{
				return false;
			}
			return !dancingComponent.IsReleased;
		}
	}

	private bool HasDancingComponent
	{
		get
		{
			if (HasDancingRuntime)
			{
				return dancingComponent.Lifecycle == ComponentRuntimeLifecycle.Active;
			}
			return false;
		}
	}

	[Export(PropertyHint.None, "")]
	public string dancerPacketName
	{
		get
		{
			return _dancerPacketName;
		}
		set
		{
			_dancerPacketName = value ?? "";
			if (!string.IsNullOrEmpty(_dancerPacketName) && HasDancingComponent)
			{
				dancingComponent.dancerPacketName = _dancerPacketName;
			}
		}
	}

	public void RemoveDancer(TowerDefenseCharacter dancer)
	{
		if (HasDancingRuntime)
		{
			dancingComponent.RemoveDancer(dancer);
		}
		for (int i = 0; i < 4; i++)
		{
			if (IsSameDancer(_pendingNetworkDancers[i], dancer))
			{
				_pendingNetworkDancers[i] = null;
			}
		}
	}

	private static bool IsSameDancer(TowerDefenseCharacter candidate, TowerDefenseCharacter dancer)
	{
		if (candidate != dancer)
		{
			if (GodotObject.IsInstanceValid(candidate) && GodotObject.IsInstanceValid(dancer) && candidate.syncId >= 0)
			{
				return candidate.syncId == dancer.syncId;
			}
			return false;
		}
		return true;
	}

	public void SetNetworkDancer(int slot, TowerDefenseCharacter dancer)
	{
		if (slot >= 0 && slot < 4)
		{
			if (!HasDancingRuntime)
			{
				_pendingNetworkDancers[slot] = dancer;
				return;
			}
			dancingComponent.SetNetworkDancer(slot, dancer);
			_pendingNetworkDancers[slot] = null;
		}
	}

	private void ApplyPendingNetworkDancers()
	{
		if (!HasDancingRuntime)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			TowerDefenseCharacter dancer = _pendingNetworkDancers[i];
			if (GodotObject.IsInstanceValid(dancer))
			{
				SetNetworkDancer(i, dancer);
			}
			else
			{
				_pendingNetworkDancers[i] = null;
			}
		}
	}

	private void ReleaseDancerOwnership()
	{
		if (HasDancingRuntime)
		{
			for (int i = 0; i < 4; i++)
			{
				dancingComponent.SetNetworkDancer(i, null);
			}
		}
		Array.Fill(_pendingNetworkDancers, null);
	}

	public override void _ExitTree()
	{
		if (IsQueuedForDeletion())
		{
			ReleaseDancerOwnership();
		}
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint())
		{
			return;
		}
		dancingComponent = componentManager.GetRuntime<DancingComponent>();
		if (HasDancingComponent)
		{
			if (!string.IsNullOrEmpty(dancerPacketName))
			{
				dancingComponent.dancerPacketName = dancerPacketName;
			}
			else
			{
				foreach (TowerDefenseArmorInstance armor in instance.armorList)
				{
					if (armor.slotConfig.armorName == "BlackHelmet" || armor.slotConfig.armorName == "SpecialHelmet")
					{
						dancingComponent.dancerPacketName = "ZombieDancerCone";
						break;
					}
				}
			}
		}
		ApplyPendingNetworkDancers();
	}

	public override void WalkEntered()
	{
		base.WalkEntered();
		if (HasDancingComponent)
		{
			dancingComponent.OnWalkEntered();
		}
	}

	public override void WalkProcessing(double delta)
	{
		if (HasDancingComponent)
		{
			groundMoveComponent.SetAlive(dancingComponent.CanWalk());
		}
		base.WalkProcessing(delta);
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		if (HasDancingComponent)
		{
			dancingComponent.OnAttackProcessing(delta);
		}
	}

	public override void Walk()
	{
		if (die)
		{
			SendStateEvent("ToDie");
		}
		else if (!HasDancingComponent || !dancingComponent.OnWalk())
		{
			SendStateEvent("ToWalk");
		}
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		if (HasDancingComponent)
		{
			dancingComponent.OnDieProcessing();
		}
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		ReleaseDancerOwnership();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName.RemoveDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsSameDancer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "candidate", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetNetworkDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPendingNetworkDancers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseDancerOwnership, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RemoveDancer && args.Count == 1)
		{
			RemoveDancer(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSameDancer && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSameDancer(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		if (method == MethodName.SetNetworkDancer && args.Count == 2)
		{
			SetNetworkDancer(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingNetworkDancers && args.Count == 0)
		{
			ApplyPendingNetworkDancers();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseDancerOwnership && args.Count == 0)
		{
			ReleaseDancerOwnership();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkEntered && args.Count == 0)
		{
			WalkEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsSameDancer && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSameDancer(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.RemoveDancer)
		{
			return true;
		}
		if (method == MethodName.IsSameDancer)
		{
			return true;
		}
		if (method == MethodName.SetNetworkDancer)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingNetworkDancers)
		{
			return true;
		}
		if (method == MethodName.ReleaseDancerOwnership)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.WalkEntered)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.Hypnoses)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.dancerPacketName)
		{
			dancerPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._dancerPacketName)
		{
			_dancerPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.HasDancingRuntime)
		{
			from = HasDancingRuntime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasDancingComponent)
		{
			from = HasDancingComponent;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.dancerPacketName)
		{
			value = VariantUtils.CreateFrom<string>(dancerPacketName);
			return true;
		}
		if (name == PropertyName._pendingNetworkDancers)
		{
			GodotObject[] pendingNetworkDancers = _pendingNetworkDancers;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(pendingNetworkDancers);
			return true;
		}
		if (name == PropertyName._dancerPacketName)
		{
			value = VariantUtils.CreateFrom(in _dancerPacketName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._pendingNetworkDancers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasDancingRuntime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasDancingComponent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._dancerPacketName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.dancerPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dancerPacketName, Variant.From<string>(dancerPacketName));
		info.AddProperty(PropertyName._dancerPacketName, Variant.From(in _dancerPacketName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dancerPacketName, out var value))
		{
			dancerPacketName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._dancerPacketName, out var value2))
		{
			_dancerPacketName = value2.As<string>();
		}
	}
}
