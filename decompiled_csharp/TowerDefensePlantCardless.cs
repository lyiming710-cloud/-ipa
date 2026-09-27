using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Diamond/Cardless/Scene/TowerDefensePlantCardless.cs")]
public class TowerDefensePlantCardless : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName PreSpawn = "PreSpawn";

		public new static readonly StringName SleepEntered = "SleepEntered";

		public new static readonly StringName SleepExited = "SleepExited";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public static readonly StringName EnsureChangeCostRegistered = "EnsureChangeCostRegistered";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName changeCost = "changeCost";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefensePacketChangeCost changeCost;

	public override async void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			instance.invincible = true;
			instance.canBeCollection = false;
			changeCost = changeCost.Duplicate() as TowerDefensePacketChangeCost;
			OnHypnosisStateChanged();
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			while (GodotObject.IsInstanceValid(this) && IsInsideTree() && GodotObject.IsInstanceValid(TowerDefenseManager.Instance) && !TowerDefenseManager.Instance.IsGameRunning())
			{
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			}
			if (GodotObject.IsInstanceValid(this) && IsInsideTree())
			{
				EnsureChangeCostRegistered();
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (!Engine.IsEditorHint())
		{
			TowerDefenseManager.Instance.ChangeCostRemove(changeCost);
		}
	}

	public override void PreSpawn()
	{
		base.PreSpawn();
		EnsureChangeCostRegistered();
	}

	public override void SleepEntered()
	{
		base.SleepEntered();
		instance.invincible = false;
		instance.canBeCollection = true;
	}

	public override void SleepExited()
	{
		base.SleepExited();
		instance.invincible = true;
		instance.canBeCollection = false;
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		bool flag = TowerDefenseManager.Instance.IsIZMMode() || TowerDefenseManager.Instance.IsIZM2Mode();
		changeCost.method = ((instance.hypnoses != flag) ? "Increase" : "Decrease");
		instance.invincible = true;
		instance.canBeCollection = false;
	}

	private void EnsureChangeCostRegistered()
	{
		if (GodotObject.IsInstanceValid(changeCost) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance.currentControl) && !TowerDefenseManager.GetChangeCostList().Contains(changeCost))
		{
			TowerDefenseManager.Instance.ChangeCostAdd(changeCost);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SleepEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SleepExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureChangeCostRegistered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.PreSpawn && args.Count == 0)
		{
			PreSpawn();
			ret = default;
			return true;
		}
		if (method == MethodName.SleepEntered && args.Count == 0)
		{
			SleepEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.SleepExited && args.Count == 0)
		{
			SleepExited();
			ret = default;
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureChangeCostRegistered && args.Count == 0)
		{
			EnsureChangeCostRegistered();
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
		if (method == MethodName.PreSpawn)
		{
			return true;
		}
		if (method == MethodName.SleepEntered)
		{
			return true;
		}
		if (method == MethodName.SleepExited)
		{
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged)
		{
			return true;
		}
		if (method == MethodName.EnsureChangeCostRegistered)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.changeCost)
		{
			changeCost = VariantUtils.ConvertTo<TowerDefensePacketChangeCost>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.changeCost)
		{
			value = VariantUtils.CreateFrom(in changeCost);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.changeCost, PropertyHint.ResourceType, "TowerDefensePacketChangeCost", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.changeCost, Variant.From(in changeCost));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.changeCost, out var value))
		{
			changeCost = value.As<TowerDefensePacketChangeCost>();
		}
	}
}
