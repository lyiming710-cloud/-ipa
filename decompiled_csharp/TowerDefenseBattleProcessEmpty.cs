using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Process/Empty/TowerDefenseBattleProcessEmpty.cs")]
public class TowerDefenseBattleProcessEmpty : TowerDefenseBattleProcess
{
	public new class MethodName : TowerDefenseBattleProcess.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName ResolveDependencies = "ResolveDependencies";

		public new static readonly StringName CheckFinal = "CheckFinal";

		public new static readonly StringName CheckFail = "CheckFail";

		public new static readonly StringName ZombieEnterHouse = "ZombieEnterHouse";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleProcess.PropertyName
	{
		public static readonly StringName seedBankFeature = "seedBankFeature";
	}

	public new class SignalName : TowerDefenseBattleProcess.SignalName
	{
	}

	public TowerDefenseBattleFeatureSeedBank seedBankFeature;

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
	}

	private void ResolveDependencies()
	{
		seedBankFeature = GetFeature<TowerDefenseBattleFeatureSeedBank>("SeedBank");
	}

	public override Task GameInit()
	{
		ResolveDependencies();
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		ResolveDependencies();
		return Task.CompletedTask;
	}

	public override Task GameEntry()
	{
		if (!IsLifetimeActive || !GodotObject.IsInstanceValid(control))
		{
			return Task.CompletedTask;
		}
		if (GodotObject.IsInstanceValid(seedBankFeature?.seedBank))
		{
			seedBankFeature.seedBank.packetSlotContainer.Visible = true;
		}
		if (GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool())
		{
			control.uiTopAnimationPlayer.Play("MobileEnter");
		}
		else
		{
			control.uiTopAnimationPlayer.Play("Enter");
		}
		return Task.CompletedTask;
	}

	public override Task GameReady()
	{
		if (GodotObject.IsInstanceValid(seedBankFeature?.seedBank))
		{
			seedBankFeature.seedBank.ReadyPackets();
		}
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		if (GodotObject.IsInstanceValid(seedBankFeature?.seedBank))
		{
			seedBankFeature.seedBank.StartFromProgress();
		}
		return Task.CompletedTask;
	}

	public override bool CheckFinal()
	{
		return false;
	}

	public override bool CheckFail()
	{
		return false;
	}

	public override void ZombieEnterHouse(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			character.Destroy();
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		seedBankFeature = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveDependencies, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckFinal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckFail, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ZombieEnterHouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveDependencies && args.Count == 0)
		{
			ResolveDependencies();
			ret = default;
			return true;
		}
		if (method == MethodName.CheckFinal && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckFinal());
			return true;
		}
		if (method == MethodName.CheckFail && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckFail());
			return true;
		}
		if (method == MethodName.ZombieEnterHouse && args.Count == 1)
		{
			ZombieEnterHouse(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.ResolveDependencies)
		{
			return true;
		}
		if (method == MethodName.CheckFinal)
		{
			return true;
		}
		if (method == MethodName.CheckFail)
		{
			return true;
		}
		if (method == MethodName.ZombieEnterHouse)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.seedBankFeature)
		{
			seedBankFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureSeedBank>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.seedBankFeature)
		{
			value = VariantUtils.CreateFrom(in seedBankFeature);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.seedBankFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.seedBankFeature, Variant.From(in seedBankFeature));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.seedBankFeature, out var value))
		{
			seedBankFeature = value.As<TowerDefenseBattleFeatureSeedBank>();
		}
	}
}
