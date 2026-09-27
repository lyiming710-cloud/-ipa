using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/PotQX/Scene/TowerDefensePlantPotQX.cs")]
public class TowerDefensePlantPotQX : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName OpenEntered = "OpenEntered";

		public static readonly StringName OpenProcessing = "OpenProcessing";

		public static readonly StringName OpenExited = "OpenExited";

		public static readonly StringName CloseEntered = "CloseEntered";

		public static readonly StringName CloseProcessing = "CloseProcessing";

		public static readonly StringName CloseExited = "CloseExited";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public new static readonly StringName SyncHologramHypnoses = "SyncHologramHypnoses";

		public static readonly StringName GetHologramPacket = "GetHologramPacket";

		public static readonly StringName CreateHologramCharacter = "CreateHologramCharacter";

		public static readonly StringName ClearHologramCharacter = "ClearHologramCharacter";

		public static readonly StringName HandleOwnerDestroyed = "HandleOwnerDestroyed";

		public static readonly StringName Timeout = "Timeout";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName createCharacterList = "createCharacterList";

		public static readonly StringName open = "open";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private CharacterTimerComponent _timerComponent;

	private StateHandle _openState;

	private StateHandle _closeState;

	private bool _roleStateSignalsConnected;

	public Array<TowerDefenseCharacter> createCharacterList = new Array<TowerDefenseCharacter>();

	public bool open;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_timerComponent = componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
			_timerComponent.OnTimeout += Timeout;
			OnDestroy += HandleOwnerDestroyed;
			_openState = StateMachine?.GetStateById("plant.pot_qx.open");
			_closeState = StateMachine?.GetStateById("plant.pot_qx.close");
			ConnectRoleStateSignals();
		}
	}

	public override void _ExitTree()
	{
		OnDestroy -= HandleOwnerDestroyed;
		DisconnectRoleStateSignals();
		ClearHologramCharacter();
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent != null && !timerComponent.IsReleased)
		{
			_timerComponent.OnTimeout -= Timeout;
		}
		base._ExitTree();
	}

	private void ConnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			return;
		}
		StateHandle openState = _openState;
		if (openState != null && openState.IsValid)
		{
			StateHandle closeState = _closeState;
			if (closeState != null && closeState.IsValid)
			{
				_openState.Entered += OpenEntered;
				_openState.Exited += OpenExited;
				_openState.PhysicsProcessing += OpenProcessing;
				_closeState.Entered += CloseEntered;
				_closeState.Exited += CloseExited;
				_closeState.PhysicsProcessing += CloseProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			_openState.Entered -= OpenEntered;
			_openState.Exited -= OpenExited;
			_openState.PhysicsProcessing -= OpenProcessing;
			_closeState.Entered -= CloseEntered;
			_closeState.Exited -= CloseExited;
			_closeState.PhysicsProcessing -= CloseProcessing;
			_openState = null;
			_closeState = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (inGame && TowerDefenseManager.CurrentControl.isGameRunning && !open)
		{
			SendStateEvent("ToOpen");
			open = true;
		}
	}

	public virtual void OpenEntered()
	{
		sprite.SetAnimation("Open", loop: false);
	}

	public virtual void OpenProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public virtual void OpenExited()
	{
	}

	public virtual void CloseEntered()
	{
		sprite.SetAnimation("Close", loop: false);
	}

	public virtual void CloseProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public virtual void CloseExited()
	{
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!(clip == "Open"))
		{
			if (clip == "Close")
			{
				ClearHologramCharacter();
				Destroy();
			}
		}
		else
		{
			CreateHologramCharacter();
			Idle();
			_timerComponent.Run("Close", 30.0);
		}
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		SyncHologramHypnoses();
	}

	private void SyncHologramHypnoses()
	{
		bool hypnoses = instance.hypnoses;
		foreach (TowerDefenseCharacter createCharacter in createCharacterList)
		{
			if (GodotObject.IsInstanceValid(createCharacter) && !createCharacter.isDestroy)
			{
				createCharacter.SyncHologramHypnoses(hypnoses);
			}
		}
	}

	private TowerDefensePacketConfig GetHologramPacket(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return null;
		}
		if (GodotObject.IsInstanceValid(character.packet))
		{
			return character.packet;
		}
		if (!GodotObject.IsInstanceValid(character.config))
		{
			return null;
		}
		TowerDefensePacketConfig packetConfigReadOnlyByCharacterName = TowerDefenseManager.GetPacketConfigReadOnlyByCharacterName(character.config.name);
		if (GodotObject.IsInstanceValid(packetConfigReadOnlyByCharacterName))
		{
			return packetConfigReadOnlyByCharacterName.Duplicate(deep: true) as TowerDefensePacketConfig;
		}
		return TowerDefenseManager.GetPacketConfig(character.config.name);
	}

	public void CreateHologramCharacter()
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		Array<TowerDefensePacketConfig> array = new Array<TowerDefensePacketConfig>();
		foreach (TowerDefenseCharacter item in cell.GetCharacterListSave())
		{
			if (item != this && item.config.canCopy && item.camp == camp && item.config.plantGridOverrideType == TowerDefenseEnum.PLANTGRIDTYPE.NOONE)
			{
				TowerDefensePacketConfig hologramPacket = GetHologramPacket(item);
				if (GodotObject.IsInstanceValid(hologramPacket))
				{
					array.Add(hologramPacket);
				}
			}
		}
		for (int i = 1; i <= TowerDefenseManager.Instance.GetMapGridNum().Y; i++)
		{
			if (i == gridPos.Y)
			{
				continue;
			}
			Vector2I vector2I = new Vector2I(gridPos.X, i);
			TowerDefenseCellInstance cellGet = TowerDefenseManager.GetMapCell(vector2I);
			foreach (TowerDefensePacketConfig item2 in array)
			{
				if (!cellGet.CanPacketPlant(item2))
				{
					continue;
				}
				TowerDefenseCharacter character = item2.Plant(vector2I);
				if (!GodotObject.IsInstanceValid(character))
				{
					continue;
				}
				createCharacterList.Add(character);
				bool hypnoses = instance.hypnoses;
				Callable.From(() =>
				{
					if (GodotObject.IsInstanceValid(character))
					{
						character.SetSpriteGroupShaderParameter("hologram", true);
						if (hypnoses)
						{
							character.Hypnoses();
						}
						character.instance.hologram = true;
						character.instance.canBeCollection = false;
						cellGet.ReleaseHologramGridOccupancy(character);
					}
				}).CallDeferred();
				if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
				{
					TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
					if (GodotObject.IsInstanceValid(currentControl))
					{
						int nextSyncId = currentControl.GetNextSyncId();
						currentControl.RegisterSyncCharacter(nextSyncId, character);
						MultiPlayerManager.Instance.SendSpawnCharacterAt(item2.saveKey, vector2I.X, vector2I.Y, nextSyncId);
					}
				}
			}
		}
	}

	public void ClearHologramCharacter()
	{
		Array<TowerDefenseCharacter> array = createCharacterList.Duplicate();
		createCharacterList.Clear();
		foreach (TowerDefenseCharacter item in array)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.EmitDestroy();
				TowerDefenseManager.Instance.CharacterUnregister(item);
				item.RemoveFromGroup("Character");
				item.QueueFree();
			}
		}
	}

	private void HandleOwnerDestroyed(TowerDefenseCharacter _)
	{
		ClearHologramCharacter();
	}

	public void Timeout(string timerName)
	{
		if (timerName == "Close")
		{
			SendStateEvent("ToClose");
		}
	}

	public override void DestroySet()
	{
		base.DestroySet();
		ClearHologramCharacter();
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		TowerDefenseCharacter character = TowerDefenseManager.GetPacketConfig("PlantPot").Plant(gridPos);
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl) && GodotObject.IsInstanceValid(character))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, character);
				MultiPlayerManager.Instance.SendSpawnCharacterAt("PlantPot", gridPos.X, gridPos.Y, nextSyncId);
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "open", open } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		open = data.GetValueOrDefault("open", Variant.From<bool>(false)).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(22)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloseEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloseProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncHologramHypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetHologramPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateHologramCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearHologramCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleOwnerDestroyed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Timeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.ConnectRoleStateSignals && args.Count == 0)
		{
			ConnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals && args.Count == 0)
		{
			DisconnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenEntered && args.Count == 0)
		{
			OpenEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenProcessing && args.Count == 1)
		{
			OpenProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenExited && args.Count == 0)
		{
			OpenExited();
			ret = default;
			return true;
		}
		if (method == MethodName.CloseEntered && args.Count == 0)
		{
			CloseEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.CloseProcessing && args.Count == 1)
		{
			CloseProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloseExited && args.Count == 0)
		{
			CloseExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncHologramHypnoses && args.Count == 0)
		{
			SyncHologramHypnoses();
			ret = default;
			return true;
		}
		if (method == MethodName.GetHologramPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(GetHologramPacket(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateHologramCharacter && args.Count == 0)
		{
			CreateHologramCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearHologramCharacter && args.Count == 0)
		{
			ClearHologramCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleOwnerDestroyed && args.Count == 1)
		{
			HandleOwnerDestroyed(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Timeout && args.Count == 1)
		{
			Timeout(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
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
		if (method == MethodName.ConnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.OpenEntered)
		{
			return true;
		}
		if (method == MethodName.OpenProcessing)
		{
			return true;
		}
		if (method == MethodName.OpenExited)
		{
			return true;
		}
		if (method == MethodName.CloseEntered)
		{
			return true;
		}
		if (method == MethodName.CloseProcessing)
		{
			return true;
		}
		if (method == MethodName.CloseExited)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged)
		{
			return true;
		}
		if (method == MethodName.SyncHologramHypnoses)
		{
			return true;
		}
		if (method == MethodName.GetHologramPacket)
		{
			return true;
		}
		if (method == MethodName.CreateHologramCharacter)
		{
			return true;
		}
		if (method == MethodName.ClearHologramCharacter)
		{
			return true;
		}
		if (method == MethodName.HandleOwnerDestroyed)
		{
			return true;
		}
		if (method == MethodName.Timeout)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
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
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.createCharacterList)
		{
			createCharacterList = VariantUtils.ConvertToArray<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.open)
		{
			open = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName.createCharacterList)
		{
			value = VariantUtils.CreateFromArray(createCharacterList);
			return true;
		}
		if (name == PropertyName.open)
		{
			value = VariantUtils.CreateFrom(in open);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.createCharacterList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.open, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName.createCharacterList, Variant.CreateFrom(createCharacterList));
		info.AddProperty(PropertyName.open, Variant.From(in open));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value))
		{
			_roleStateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.createCharacterList, out var value2))
		{
			createCharacterList = value2.AsGodotArray<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.open, out var value3))
		{
			open = value3.As<bool>();
		}
	}
}
