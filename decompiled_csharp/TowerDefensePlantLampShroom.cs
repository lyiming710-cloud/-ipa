using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter8/LampShroom/Scene/TowerDefensePlantLampShroom.cs")]
public class TowerDefensePlantLampShroom : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName Explode = "Explode";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName spawnPacketList = "spawnPacketList";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const float PacketSpawnOffsetX = 52f;

	private const float PacketLaunchSpeedX = 65f;

	private const float PacketLaunchSpeedY = -300f;

	private ExplodeComponent _explodeComponent;

	public Array<Array> spawnPacketList = new Array<Array>
	{
		new Array { "PlantSunFlagbean", "PlantIceSunShroom" },
		new Array { "PlantJalaTorch", "PlantCherryPea" },
		new Array { "PlantHypnoBean", "ZombieNormalDoomShroomBlackHelmet" },
		new Array { "PlantKabbageTail", "PlantTabooBean" },
		new Array { "PlantGarlicChomper", "PlantGarlicChomper" }
	};

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
			_explodeComponent.OnExplode += Explode;
			AudioManager.Instance.AudioPlay("Plantern");
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		ExplodeComponent explodeComponent = _explodeComponent;
		if (explodeComponent != null && !explodeComponent.IsReleased)
		{
			_explodeComponent.OnExplode -= Explode;
		}
	}

	public override void BatchUpdate(double delta)
	{
		if (!Engine.IsEditorHint())
		{
			base.BatchUpdate(delta);
		}
	}

	public void Explode()
	{
		AudioManager.Instance.AudioPlay("Diamond");
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		Array array = spawnPacketList.PickRandom();
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(array[0].AsString());
		if (instance.hypnoses)
		{
			packetConfig.overrideHypnoses = true;
		}
		SpawnPacket(packetConfig, logicalGlobalPosition - new Vector2(52f, 0f), 15.0, isFall: false, useCost: false, useRandf: false, new Vector2(-65f, -300f));
		TowerDefensePacketConfig packetConfig2 = TowerDefenseManager.GetPacketConfig(array[1].AsString());
		if (instance.hypnoses)
		{
			packetConfig2.overrideHypnoses = true;
		}
		SpawnPacket(packetConfig2, logicalGlobalPosition + new Vector2(52f, 0f), 15.0, isFall: false, useCost: false, useRandf: false, new Vector2(65f, -300f));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Explode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Explode && args.Count == 0)
		{
			Explode();
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.Explode)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.spawnPacketList)
		{
			spawnPacketList = VariantUtils.ConvertToArray<Array>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.spawnPacketList)
		{
			value = VariantUtils.CreateFromArray(spawnPacketList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.spawnPacketList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.spawnPacketList, Variant.CreateFrom(spawnPacketList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.spawnPacketList, out var value))
		{
			spawnPacketList = value.AsGodotArray<Array>();
		}
	}
}
