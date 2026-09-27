using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/InsaniKelp/Scene/TowerDefensePlantInsaniKelp.cs")]
public class TowerDefensePlantInsaniKelp : TowerDefensePlant
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

		public static readonly StringName OpenIdleEntered = "OpenIdleEntered";

		public static readonly StringName OpenIdleProcessing = "OpenIdleProcessing";

		public static readonly StringName OpenIdleExited = "OpenIdleExited";

		public static readonly StringName CloseEntered = "CloseEntered";

		public static readonly StringName CloseProcessing = "CloseProcessing";

		public static readonly StringName CloseExited = "CloseExited";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName Timeout = "Timeout";

		public static readonly StringName Pressed = "Pressed";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName isTanglekelp = "isTanglekelp";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private CharacterTimerComponent _timerComponent;

	private MousePressComponent _mousePressComponent;

	private StateHandle _openState;

	private StateHandle _openIdleState;

	private StateHandle _closeState;

	private bool _roleStateSignalsConnected;

	public bool isTanglekelp;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_timerComponent = componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
			_mousePressComponent = componentManager.GetRuntime<MousePressComponent>();
			_timerComponent.OnTimeout += Timeout;
			_mousePressComponent.OnPressed += Pressed;
			_openState = StateMachine?.GetStateById("plant.insani_kelp.open");
			_openIdleState = StateMachine?.GetStateById("plant.insani_kelp.open_idle");
			_closeState = StateMachine?.GetStateById("plant.insani_kelp.close");
			ConnectRoleStateSignals();
		}
	}

	public override void _ExitTree()
	{
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent != null && !timerComponent.IsReleased)
		{
			_timerComponent.OnTimeout -= Timeout;
		}
		if (_mousePressComponent != null)
		{
			_mousePressComponent.OnPressed -= Pressed;
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
		if (openState == null || !openState.IsValid)
		{
			return;
		}
		StateHandle openIdleState = _openIdleState;
		if (openIdleState != null && openIdleState.IsValid)
		{
			StateHandle closeState = _closeState;
			if (closeState != null && closeState.IsValid)
			{
				_openState.Entered += OpenEntered;
				_openState.Exited += OpenExited;
				_openState.PhysicsProcessing += OpenProcessing;
				_openIdleState.Entered += OpenIdleEntered;
				_openIdleState.Exited += OpenIdleExited;
				_openIdleState.PhysicsProcessing += OpenIdleProcessing;
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
			_openIdleState.Entered -= OpenIdleEntered;
			_openIdleState.Exited -= OpenIdleExited;
			_openIdleState.PhysicsProcessing -= OpenIdleProcessing;
			_closeState.Entered -= CloseEntered;
			_closeState.Exited -= CloseExited;
			_closeState.PhysicsProcessing -= CloseProcessing;
			_openState = null;
			_openIdleState = null;
			_closeState = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (TowerDefenseManager.Instance.IsGameRunning() && inGame && !_timerComponent.IsRunning("Open"))
		{
			_timerComponent.Run("Open");
		}
	}

	public virtual void OpenEntered()
	{
		isTanglekelp = (double)GD.Randf() < 0.1;
		sprite.SetAnimation(isTanglekelp ? "OpenB" : "OpenA", loop: false, 0.2);
	}

	public virtual void OpenProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public virtual void OpenExited()
	{
	}

	public virtual void OpenIdleEntered()
	{
		_mousePressComponent.SetAlive(alive: true);
		if (instance.hypnoses)
		{
			Pressed(GetLogicalGlobalPosition());
			_mousePressComponent.SetAlive(alive: false);
		}
		sprite.SetAnimation(isTanglekelp ? "IdleB" : "IdleA", loop: true, 0.2);
	}

	public virtual void OpenIdleProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public virtual void OpenIdleExited()
	{
		_mousePressComponent.SetAlive(alive: false);
	}

	public virtual void CloseEntered()
	{
		sprite.SetAnimation("Close", loop: true, 0.2);
	}

	public virtual void CloseProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public virtual void CloseExited()
	{
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		switch (clip)
		{
		case "OpenA":
		case "OpenB":
			SendStateEvent("ToOpenIdle");
			break;
		case "Close":
			Idle();
			break;
		}
	}

	public void Timeout(string timerName)
	{
		if (timerName == "Open")
		{
			SendStateEvent("ToOpen");
		}
	}

	public void Pressed(Vector2 pos)
	{
		Vector2 pos2 = (instance.hypnoses ? pos : GetLogicalGlobalPosition());
		long num = (isTanglekelp ? 50 : 25);
		EconomyAccountId accountId = (HasEconomyOwner ? EconomyOwnerAccountId : EconomyAccountId.Local);
		if (TowerDefenseManager.Instance.IsIZMMode())
		{
			if (!instance.hypnoses && !TowerDefenseManager.Instance.TrySpendSun(accountId, num))
			{
				return;
			}
		}
		else
		{
			BrainSunCreate(pos2, num);
		}
		SendStateEvent("ToClose");
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		string packetName = (isTanglekelp ? "ZombieSnorkleTanglekelp" : "ZombieSnorkle");
		TowerDefenseCharacter towerDefenseCharacter = (HasEconomyOwner ? TowerDefenseCharacter.CreateCharacter(EconomyOwnerAccountId, packetName, pos2, gridPos, groundHeight) : TowerDefenseCharacter.CreateCharacter(packetName, pos2, gridPos, groundHeight));
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			return;
		}
		towerDefenseCharacter.CallDeferred("Walk");
		if (!instance.hypnoses)
		{
			towerDefenseCharacter.Hypnoses();
		}
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
				MultiPlayerManager.Instance.SendSpawnCharacterAt(packetName, gridPos.X, gridPos.Y, nextSyncId, instance.hitpointScale, transformPoint.Scale.X, !instance.hypnoses, 0.0, useCreate: true, pos2.X, pos2.Y, walkAfterSpawn: true, groundHeight);
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "isTanglekelp", isTanglekelp } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		isTanglekelp = data.GetValueOrDefault("isTanglekelp", Variant.From<bool>(false)).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
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
			new MethodInfo(MethodName.OpenIdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenIdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenIdleExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.Timeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Pressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.OpenIdleEntered && args.Count == 0)
		{
			OpenIdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenIdleProcessing && args.Count == 1)
		{
			OpenIdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenIdleExited && args.Count == 0)
		{
			OpenIdleExited();
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
		if (method == MethodName.Timeout && args.Count == 1)
		{
			Timeout(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Pressed && args.Count == 1)
		{
			Pressed(VariantUtils.ConvertTo<Vector2>(in args[0]));
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
		if (method == MethodName.OpenIdleEntered)
		{
			return true;
		}
		if (method == MethodName.OpenIdleProcessing)
		{
			return true;
		}
		if (method == MethodName.OpenIdleExited)
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
		if (method == MethodName.Timeout)
		{
			return true;
		}
		if (method == MethodName.Pressed)
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
		if (name == PropertyName.isTanglekelp)
		{
			isTanglekelp = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.isTanglekelp)
		{
			value = VariantUtils.CreateFrom(in isTanglekelp);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.isTanglekelp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName.isTanglekelp, Variant.From(in isTanglekelp));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value))
		{
			_roleStateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isTanglekelp, out var value2))
		{
			isTanglekelp = value2.As<bool>();
		}
	}
}
