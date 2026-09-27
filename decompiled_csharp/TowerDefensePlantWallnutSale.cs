using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Other/WallnutSale/Scene/TowerDefensePlantWallnutSale.cs")]
public class TowerDefensePlantWallnutSale : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName _DeferredReady = "_DeferredReady";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName canCopy = "canCopy";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	public bool canCopy = true;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && GodotObject.IsInstanceValid(packet) && packet.GetCost() == 0)
		{
			CallDeferred("_DeferredReady");
		}
	}

	private async void _DeferredReady()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (instance.hologram)
		{
			canCopy = false;
		}
		if (canCopy)
		{
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(config.name);
			TowerDefenseCharacterOverride towerDefenseCharacterOverride = new TowerDefenseCharacterOverride();
			TowerDefenseCharacterPropertyChangeConfig towerDefenseCharacterPropertyChangeConfig = new TowerDefenseCharacterPropertyChangeConfig();
			towerDefenseCharacterPropertyChangeConfig.propertyName = "canCopy";
			towerDefenseCharacterPropertyChangeConfig.value = false;
			towerDefenseCharacterOverride.propertyChange = new Array<TowerDefenseCharacterPropertyChangeConfig> { towerDefenseCharacterPropertyChangeConfig };
			TowerDefensePacketOverride towerDefensePacketOverride = new TowerDefensePacketOverride();
			towerDefensePacketOverride.characterOverride = towerDefenseCharacterOverride;
			packetConfig._override = towerDefensePacketOverride;
			SpawnPacket(packetConfig, GetLogicalGlobalPosition(), 15.0, isFall: false);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "canCopy", canCopy } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		canCopy = data.GetValueOrDefault("canCopy", Variant.From<bool>(true)).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._DeferredReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._DeferredReady && args.Count == 0)
		{
			_DeferredReady();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName._DeferredReady)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.canCopy)
		{
			canCopy = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.canCopy)
		{
			value = VariantUtils.CreateFrom(in canCopy);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.canCopy, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.canCopy, Variant.From(in canCopy));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.canCopy, out var value))
		{
			canCopy = value.As<bool>();
		}
	}
}
