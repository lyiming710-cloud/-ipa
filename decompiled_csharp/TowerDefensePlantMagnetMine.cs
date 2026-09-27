using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter2/MagnetMine/Scene/TowerDefensePlantMagnetMine.cs")]
public class TowerDefensePlantMagnetMine : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ProcessDrawAsync = "ProcessDrawAsync";

		public static readonly StringName FireEntered = "FireEntered";

		public static readonly StringName FireProcessing = "FireProcessing";

		public static readonly StringName FireExited = "FireExited";

		public static readonly StringName ReadyRise = "ReadyRise";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName readyTime = "readyTime";

		public static readonly StringName _isProcessing = "_isProcessing";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName _readyTime = "_readyTime";

		public static readonly StringName over = "over";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private AttackComponent attackComponent;

	private MagnetComponent magnetComponent;

	private PotatoComponent potatoComponent;

	private bool _isProcessing;

	private StateHandle _fireState;

	private bool _roleStateSignalsConnected;

	private double _readyTime = 5.0;

	public List<TowerDefenseCharacter> drawCharacter = new List<TowerDefenseCharacter>();

	public bool over;

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
			PotatoComponent potatoComponent = this.potatoComponent;
			if (potatoComponent != null && !potatoComponent.IsReleased)
			{
				PotatoComponent potatoComponent2 = this.potatoComponent;
				if (potatoComponent2 != null && !potatoComponent2.IsReleased)
				{
					this.potatoComponent.readyTime = (float)value;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			magnetComponent = componentManager.GetRuntime<MagnetComponent>();
			if (magnetComponent == null)
			{
				GD.PushError("Magnet Mine is missing its Magnet resource runtime.");
			}
			this.potatoComponent = componentManager.GetRuntime<PotatoComponent>();
			PotatoComponent potatoComponent = this.potatoComponent;
			if (potatoComponent != null && !potatoComponent.IsReleased)
			{
				this.potatoComponent.readyTime = (float)readyTime;
			}
			_fireState = StateMachine?.GetStateById("plant.magnet_mine.fire");
			ConnectRoleStateSignals();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	private void ConnectRoleStateSignals()
	{
		if (!_roleStateSignalsConnected)
		{
			StateHandle fireState = _fireState;
			if (fireState != null && fireState.IsValid)
			{
				_fireState.Entered += FireEntered;
				_fireState.Exited += FireExited;
				_fireState.PhysicsProcessing += FireProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			_fireState.Entered -= FireEntered;
			_fireState.Exited -= FireExited;
			_fireState.PhysicsProcessing -= FireProcessing;
			_fireState = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && potatoComponent.isCharge && sprite.clip != "Fire" && !_isProcessing)
		{
			_isProcessing = true;
			ProcessDrawAsync();
		}
	}

	private async void ProcessDrawAsync()
	{
		try
		{
			drawCharacter = await magnetComponent.GetCanArmorDrawCharacterList();
			if (GodotObject.IsInstanceValid(this) && drawCharacter.Count > 0)
			{
				SendStateEvent("ToFire");
			}
		}
		finally
		{
			_isProcessing = false;
		}
	}

	public void FireEntered()
	{
		instance.invincible = true;
		sprite.SetAnimation("Fire", loop: false);
		potatoComponent.SetAlive(alive: false);
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		foreach (TowerDefenseCharacter item in drawCharacter)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.gridPos = new Vector2I(item.gridPos.X, gridPos.Y);
				Tween tween = item.CreateTween();
				tween.SetParallel();
				tween.SetEase(Tween.EaseType.Out);
				tween.SetTrans(Tween.TransitionType.Quart);
				Vector2 logicalGlobalPosition2 = item.GetLogicalGlobalPosition();
				tween.TweenMethod(Callable.From<Vector2>(item.SetLogicalGlobalPosition), logicalGlobalPosition2, logicalGlobalPosition, 0.5);
				ShadowComponent shadowComponent = item.shadowComponent;
				if (shadowComponent != null && !shadowComponent.IsReleased)
				{
					item.shadowComponent.TweenSaveShadowPositionY(tween, item.shadowComponent.saveShadowPosition.Y + logicalGlobalPosition.Y - logicalGlobalPosition2.Y, 0.5);
				}
			}
		}
	}

	public void FireProcessing(double delta)
	{
		sprite.timeScale = timeScale * 2.0;
	}

	public void FireExited()
	{
	}

	public void ReadyRise()
	{
		potatoComponent.ReadyRise();
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Fire" && !over)
		{
			over = true;
			potatoComponent.Explode();
			Destroy();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "readyTime", readyTime },
			{ "over", over }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		readyTime = data.GetValueOrDefault("readyTime", 5.0).AsDouble();
		over = data.GetValueOrDefault("over", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessDrawAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FireEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FireProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FireExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadyRise, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessDrawAsync && args.Count == 0)
		{
			ProcessDrawAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.FireEntered && args.Count == 0)
		{
			FireEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.FireProcessing && args.Count == 1)
		{
			FireProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FireExited && args.Count == 0)
		{
			FireExited();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadyRise && args.Count == 0)
		{
			ReadyRise();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.ProcessDrawAsync)
		{
			return true;
		}
		if (method == MethodName.FireEntered)
		{
			return true;
		}
		if (method == MethodName.FireProcessing)
		{
			return true;
		}
		if (method == MethodName.FireExited)
		{
			return true;
		}
		if (method == MethodName.ReadyRise)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
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
		if (name == PropertyName._isProcessing)
		{
			_isProcessing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._readyTime)
		{
			_readyTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._isProcessing)
		{
			value = VariantUtils.CreateFrom(in _isProcessing);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName._readyTime)
		{
			value = VariantUtils.CreateFrom(in _readyTime);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._isProcessing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._readyTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.readyTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.readyTime, Variant.From<double>(readyTime));
		info.AddProperty(PropertyName._isProcessing, Variant.From(in _isProcessing));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName._readyTime, Variant.From(in _readyTime));
		info.AddProperty(PropertyName.over, Variant.From(in over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.readyTime, out var value))
		{
			readyTime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._isProcessing, out var value2))
		{
			_isProcessing = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value3))
		{
			_roleStateSignalsConnected = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._readyTime, out var value4))
		{
			_readyTime = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value5))
		{
			over = value5.As<bool>();
		}
	}
}
