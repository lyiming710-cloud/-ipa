using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Other/PotatoSquash/Scene/TowerDefensePlantPotatoSquash.cs")]
public class TowerDefensePlantPotatoSquash : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ReadyRise = "ReadyRise";

		public static readonly StringName OnReadyRise = "OnReadyRise";

		public static readonly StringName OnJumpDownSmash = "OnJumpDownSmash";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName readyTime = "readyTime";

		public static readonly StringName _readyTime = "_readyTime";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private PotatoComponent _potatoComponent;

	private SquashComponent _squashComponent;

	private AttackComponent _attackComponent;

	private double _readyTime = 7.5;

	[Export(PropertyHint.None, "")]
	public double readyTime
	{
		get
		{
			return _readyTime;
		}
		set
		{
			_readyTime = value;
			if (!IsNodeReady())
			{
				return;
			}
			PotatoComponent potatoComponent = _potatoComponent;
			if (potatoComponent != null && !potatoComponent.IsReleased)
			{
				PotatoComponent potatoComponent2 = _potatoComponent;
				if (potatoComponent2 != null && !potatoComponent2.IsReleased)
				{
					_potatoComponent.readyTime = (float)value;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_potatoComponent = componentManager.GetRuntime<PotatoComponent>();
			PotatoComponent potatoComponent = _potatoComponent;
			if (potatoComponent != null && !potatoComponent.IsReleased)
			{
				_potatoComponent.readyTime = (float)readyTime;
			}
			_squashComponent = componentManager.GetRuntime<SquashComponent>();
			_attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			Vector2 vector = TowerDefenseManager.Instance.GetMapGridSize() * new Vector2(5f, 5f);
			_attackComponent?.SetCheckAreaRectangleSize(0, vector);
			_squashComponent?.SetRuntimeCheckRectangleSize(vector);
			_potatoComponent.OnReadyRise += OnReadyRise;
			_squashComponent.OnJumpDownSmash += OnJumpDownSmash;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		SquashComponent squashComponent = _squashComponent;
		if (squashComponent != null && !squashComponent.IsReleased)
		{
			_squashComponent.OnJumpDownSmash -= OnJumpDownSmash;
		}
		PotatoComponent potatoComponent = _potatoComponent;
		if (potatoComponent != null && !potatoComponent.IsReleased)
		{
			_potatoComponent.OnReadyRise -= OnReadyRise;
		}
	}

	public void ReadyRise()
	{
		PotatoComponent potatoComponent = _potatoComponent;
		if (potatoComponent != null && !potatoComponent.IsReleased)
		{
			_potatoComponent.ReadyRise();
		}
	}

	private void OnReadyRise()
	{
		_squashComponent.SetAlive(alive: true);
		instance.biteHurt = 0.0;
		Idle();
		sprite.SetAnimation(_potatoComponent.chargeAnimeClips);
	}

	private void OnJumpDownSmash()
	{
		AudioManager.Instance.AudioPlay("MineExplosion");
		sprite.SetAnimation("Mashed", loop: false);
		if (TowerDefenseManager.HasGameplayAuthority && GodotObject.IsInstanceValid(packet))
		{
			TowerDefenseCharacter towerDefenseCharacter = (HasEconomyOwner ? packet.Plant(EconomyOwnerAccountId, gridPos, playAudio: false) : packet.Plant(gridPos, playAudio: false));
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && instance.hypnoses)
			{
				towerDefenseCharacter.Hypnoses();
			}
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				Dictionary spawnState = new Dictionary { ["plant_play_audio"] = false };
				TowerDefenseManager.PublishSpawnedCharacter(packet.saveKey, towerDefenseCharacter, useCreate: false, 0.0, walkAfterSpawn: false, "", spawnState);
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { ["readyTime"] = readyTime };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		readyTime = (data.ContainsKey("readyTime") ? data["readyTime"].AsDouble() : 7.5);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadyRise, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnReadyRise, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnJumpDownSmash, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadyRise && args.Count == 0)
		{
			ReadyRise();
			ret = default;
			return true;
		}
		if (method == MethodName.OnReadyRise && args.Count == 0)
		{
			OnReadyRise();
			ret = default;
			return true;
		}
		if (method == MethodName.OnJumpDownSmash && args.Count == 0)
		{
			OnJumpDownSmash();
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ReadyRise)
		{
			return true;
		}
		if (method == MethodName.OnReadyRise)
		{
			return true;
		}
		if (method == MethodName.OnJumpDownSmash)
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
		if (name == PropertyName.readyTime)
		{
			readyTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._readyTime)
		{
			_readyTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.readyTime)
		{
			value = VariantUtils.CreateFrom<double>(readyTime);
			return true;
		}
		if (name == PropertyName._readyTime)
		{
			value = VariantUtils.CreateFrom(in _readyTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.readyTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._readyTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.readyTime, Variant.From<double>(readyTime));
		info.AddProperty(PropertyName._readyTime, Variant.From(in _readyTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.readyTime, out var value))
		{
			readyTime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._readyTime, out var value2))
		{
			_readyTime = value2.As<double>();
		}
	}
}
