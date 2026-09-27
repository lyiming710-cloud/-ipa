using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/Robot/Scene/TowerDefensePlantRobot.cs")]
public class TowerDefensePlantRobot : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public static readonly StringName DownEntered = "DownEntered";

		public static readonly StringName DownProcessing = "DownProcessing";

		public static readonly StringName DownExited = "DownExited";

		public static readonly StringName UpEntered = "UpEntered";

		public static readonly StringName UpProcessing = "UpProcessing";

		public static readonly StringName UpExited = "UpExited";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName DoublePressed = "DoublePressed";

		public static readonly StringName Timeout = "Timeout";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName _modeList = "_modeList";

		public static readonly StringName modeId = "modeId";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string PLANT_ROBOTRACK1 = "uid://2ebfsacjxwse";

	private const string PLANT_ROBOTRACK2 = "uid://drjphbxctwqg5";

	private const string PLANT_ROBOTRACK3 = "uid://800yffpdvahb";

	private const string PLANT_ROBOT_BODY_A1 = "uid://ce35mdynvmvys";

	private const string PLANT_ROBOT_BODY_A2 = "uid://dct082g5i6hg2";

	private const string PLANT_ROBOT_BODY_A3 = "uid://dxgmilyjml4ar";

	private const string PLANT_ROBOT_BODYR1 = "uid://chpeoi5efjvh3";

	private const string PLANT_ROBOT_BODYR2 = "uid://b6r70pq04d4ix";

	private const string PLANT_ROBOT_BODYR3 = "uid://btbl5f7ps30pe";

	private const string PLANT_ROBOT_LEFT_A1 = "uid://dixhlofgnr2sn";

	private const string PLANT_ROBOT_LEFT_A2 = "uid://dtj3daoe3i4ti";

	private const string PLANT_ROBOT_LEFT_A3 = "uid://doovkx7gcrx86";

	private const string PLANT_ROBOT_LEFTR1 = "uid://dqh161x05tspr";

	private const string PLANT_ROBOT_LEFTR2 = "uid://bb5kstf508tte";

	private const string PLANT_ROBOT_LEFTR3 = "uid://dmln03hqaw5gp";

	private const string PLANT_ROBOT_LEFT_C1 = "uid://btwt3uvvuwqvi";

	private const string PLANT_ROBOT_LEFT_C2 = "uid://bl70mcjce8gs5";

	private const string PLANT_ROBOT_LEFT_C3 = "uid://ipidh6t2njt5";

	private const string PLANT_ROBOT_LEFT_D1 = "uid://xxnso6kdbkmo";

	private const string PLANT_ROBOT_LEFT_D2 = "uid://dkedocvimlovk";

	private const string PLANT_ROBOT_LEFT_D3 = "uid://cvlfngqme066r";

	private const string PLANT_ROBOT_RIGHT_A1 = "uid://dl3031sobp60x";

	private const string PLANT_ROBOT_RIGHT_A2 = "uid://dhu31tsxoow2k";

	private const string PLANT_ROBOT_RIGHT_A3 = "uid://bik8j6bxg7c6f";

	private const string PLANT_ROBOT_RIGHTR1 = "uid://de2tghwbp36s1";

	private const string PLANT_ROBOT_RIGHTR2 = "uid://crlmumytyhe5g";

	private const string PLANT_ROBOT_RIGHTR3 = "uid://cfmmts1sfdfh7";

	private const string PLANT_ROBOT_RIGHT_C1 = "uid://bjdp1rn525cpp";

	private const string PLANT_ROBOT_RIGHT_C2 = "uid://cy0mkglnhyjb1";

	private const string PLANT_ROBOT_RIGHT_C3 = "uid://dcyvp5l2b6xpf";

	private const string PLANT_ROBOT_RIGHT_D1 = "uid://cxdjxqi46a72q";

	private const string PLANT_ROBOT_RIGHT_D2 = "uid://b23wdixmq6p32";

	private const string PLANT_ROBOT_RIGHT_D3 = "uid://yimmc7j73xhe";

	private FireComponent _fireComponent;

	private FireComponent _fireComponent2;

	private CharacterTimerComponent _timerComponent;

	private MousePressComponent _mousePressComponent;

	private StateHandle _downState;

	private StateHandle _upState;

	private bool _roleStateSignalsConnected;

	private string[] _modeList = new string[3] { "A", "B", "C" };

	public int modeId;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			_fireComponent2 = componentManager.GetRuntime<FireComponent>("character.fire.1");
			_timerComponent = componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
			_mousePressComponent = componentManager.GetRuntime<MousePressComponent>();
			_timerComponent.OnTimeout += Timeout;
			_mousePressComponent.OnDoublePressed += DoublePressed;
			_fireComponent2.BindCheckAreaShapeToGridRow(0, 1);
			_fireComponent2.BindCheckAreaShapeToGridRow(2, -1);
			_downState = StateMachine?.GetStateById("plant.robot.down");
			_upState = StateMachine?.GetStateById("plant.robot.up");
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
			_mousePressComponent.OnDoublePressed -= DoublePressed;
		}
		base._ExitTree();
	}

	private void ConnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			return;
		}
		StateHandle downState = _downState;
		if (downState != null && downState.IsValid)
		{
			StateHandle upState = _upState;
			if (upState != null && upState.IsValid)
			{
				_downState.Entered += DownEntered;
				_downState.Exited += DownExited;
				_downState.PhysicsProcessing += DownProcessing;
				_upState.Entered += UpEntered;
				_upState.Exited += UpExited;
				_upState.PhysicsProcessing += UpProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			_downState.Entered -= DownEntered;
			_downState.Exited -= DownExited;
			_downState.PhysicsProcessing -= DownProcessing;
			_upState.Entered -= UpEntered;
			_upState.Exited -= UpExited;
			_upState.PhysicsProcessing -= UpProcessing;
			_downState = null;
			_upState = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void IdleEntered()
	{
		base.IdleEntered();
		switch (modeId)
		{
		case 0:
			_fireComponent.alive = true;
			sprite.SetAnimation("IdleA", loop: true, 0.1);
			break;
		case 1:
			_fireComponent2.alive = true;
			sprite.SetAnimation("IdleB", loop: true, 0.1);
			break;
		case 2:
			instance.explosionHurt = config.explosionHurt / 2.0;
			instance.smashHurt = config.smashHurt / 2.0;
			instance.dragHurt = config.dragHurt / 2.0;
			instance.spikeHurt = config.spikeHurt / 2.0;
			instance.biteHurt = config.biteHurt / 2.0;
			_timerComponent.Run("Heal", 1.0);
			sprite.SetAnimation("IdleC", loop: true, 0.1);
			break;
		}
	}

	public virtual void DownEntered()
	{
		_timerComponent.Stop("Heal");
		_fireComponent.alive = false;
		_fireComponent2.alive = false;
		instance.explosionHurt = config.explosionHurt;
		instance.smashHurt = config.smashHurt;
		instance.dragHurt = config.dragHurt;
		instance.spikeHurt = config.spikeHurt;
		instance.biteHurt = config.biteHurt;
		switch (modeId)
		{
		case 0:
			sprite.SetAnimation("DownA", loop: false, 0.1);
			break;
		case 1:
			sprite.SetAnimation("DownB", loop: false, 0.1);
			break;
		case 2:
			sprite.SetAnimation("DownC", loop: false, 0.1);
			break;
		}
	}

	public virtual void DownProcessing(double delta)
	{
		sprite.timeScale = timeScale * 2.0;
	}

	public virtual void DownExited()
	{
	}

	public virtual void UpEntered()
	{
		switch (modeId)
		{
		case 0:
			sprite.SetAnimation("UpA", loop: false, 0.1);
			break;
		case 1:
			sprite.SetAnimation("UpB", loop: false, 0.1);
			break;
		case 2:
			sprite.SetAnimation("UpC", loop: false, 0.1);
			break;
		}
	}

	public virtual void UpProcessing(double delta)
	{
		sprite.timeScale = timeScale * 2.0;
	}

	public virtual void UpExited()
	{
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		switch (clip)
		{
		case "UpA":
		case "UpB":
		case "UpC":
			Idle();
			break;
		case "DownA":
		case "DownB":
		case "DownC":
			modeId = (modeId + 1 + _modeList.Length) % _modeList.Length;
			SendStateEvent("ToUp");
			break;
		}
	}

	public virtual void DoublePressed(Vector2 pos)
	{
		SendStateEvent("ToDown");
	}

	public void Timeout(string timerName)
	{
		if (timerName == "Heal")
		{
			_timerComponent.Run("Heal", 1.0);
			Health(50.0);
			if (instance.hitpoints >= instance.hitpointsSave)
			{
				instance.hitpoints = instance.hitpointsSave;
			}
		}
	}

	public override void DamagePointReach(string damagePointName)
	{
		switch (damagePointName)
		{
		case "Damage0":
			sprite.SetAtlasReplace("PlantRobot_Back1.png", "uid://2ebfsacjxwse");
			sprite.SetAtlasReplace("PlantRobot_BodyA_1.png", "uid://ce35mdynvmvys");
			sprite.SetAtlasReplace("PlantRobot_BodyB_1.png", "uid://chpeoi5efjvh3");
			sprite.SetAtlasReplace("PlantRobot_LeftA_1.png", "uid://dixhlofgnr2sn");
			sprite.SetAtlasReplace("PlantRobot_LeftB_1.png", "uid://dqh161x05tspr");
			sprite.SetAtlasReplace("PlantRobot_LeftC_1.png", "uid://btwt3uvvuwqvi");
			sprite.SetAtlasReplace("PlantRobot_LeftD_1.png", "uid://xxnso6kdbkmo");
			sprite.SetAtlasReplace("PlantRobot_RightA_1.png", "uid://dl3031sobp60x");
			sprite.SetAtlasReplace("PlantRobot_RightB_1.png", "uid://de2tghwbp36s1");
			sprite.SetAtlasReplace("PlantRobot_RightC_1.png", "uid://bjdp1rn525cpp");
			sprite.SetAtlasReplace("PlantRobot_RightD_1.png", "uid://cxdjxqi46a72q");
			break;
		case "Damage1":
			sprite.SetAtlasReplace("PlantRobot_Back1.png", "uid://drjphbxctwqg5");
			sprite.SetAtlasReplace("PlantRobot_BodyA_1.png", "uid://dct082g5i6hg2");
			sprite.SetAtlasReplace("PlantRobot_BodyB_1.png", "uid://b6r70pq04d4ix");
			sprite.SetAtlasReplace("PlantRobot_LeftA_1.png", "uid://dtj3daoe3i4ti");
			sprite.SetAtlasReplace("PlantRobot_LeftB_1.png", "uid://bb5kstf508tte");
			sprite.SetAtlasReplace("PlantRobot_LeftC_1.png", "uid://bl70mcjce8gs5");
			sprite.SetAtlasReplace("PlantRobot_LeftD_1.png", "uid://dkedocvimlovk");
			sprite.SetAtlasReplace("PlantRobot_RightA_1.png", "uid://dhu31tsxoow2k");
			sprite.SetAtlasReplace("PlantRobot_RightB_1.png", "uid://crlmumytyhe5g");
			sprite.SetAtlasReplace("PlantRobot_RightC_1.png", "uid://cy0mkglnhyjb1");
			sprite.SetAtlasReplace("PlantRobot_RightD_1.png", "uid://b23wdixmq6p32");
			break;
		case "Damage2":
			sprite.SetAtlasReplace("PlantRobot_Back1.png", "uid://800yffpdvahb");
			sprite.SetAtlasReplace("PlantRobot_BodyA_1.png", "uid://dxgmilyjml4ar");
			sprite.SetAtlasReplace("PlantRobot_BodyB_1.png", "uid://btbl5f7ps30pe");
			sprite.SetAtlasReplace("PlantRobot_LeftA_1.png", "uid://doovkx7gcrx86");
			sprite.SetAtlasReplace("PlantRobot_LeftB_1.png", "uid://dmln03hqaw5gp");
			sprite.SetAtlasReplace("PlantRobot_LeftC_1.png", "uid://ipidh6t2njt5");
			sprite.SetAtlasReplace("PlantRobot_LeftD_1.png", "uid://cvlfngqme066r");
			sprite.SetAtlasReplace("PlantRobot_RightA_1.png", "uid://bik8j6bxg7c6f");
			sprite.SetAtlasReplace("PlantRobot_RightB_1.png", "uid://cfmmts1sfdfh7");
			sprite.SetAtlasReplace("PlantRobot_RightC_1.png", "uid://dcyvp5l2b6xpf");
			sprite.SetAtlasReplace("PlantRobot_RightD_1.png", "uid://yimmc7j73xhe");
			break;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "modeId", modeId } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		modeId = data.GetValueOrDefault("modeId", Variant.From<int>(0)).AsInt32();
		_fireComponent.alive = modeId == 0;
		_fireComponent2.alive = modeId == 1;
		if (modeId == 2)
		{
			instance.explosionHurt = config.explosionHurt / 2.0;
			instance.smashHurt = config.smashHurt / 2.0;
			instance.dragHurt = config.dragHurt / 2.0;
			instance.spikeHurt = config.spikeHurt / 2.0;
			instance.biteHurt = config.biteHurt / 2.0;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DownEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DownProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DownExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoublePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Timeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DownEntered && args.Count == 0)
		{
			DownEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DownProcessing && args.Count == 1)
		{
			DownProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DownExited && args.Count == 0)
		{
			DownExited();
			ret = default;
			return true;
		}
		if (method == MethodName.UpEntered && args.Count == 0)
		{
			UpEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.UpProcessing && args.Count == 1)
		{
			UpProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpExited && args.Count == 0)
		{
			UpExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DoublePressed && args.Count == 1)
		{
			DoublePressed(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Timeout && args.Count == 1)
		{
			Timeout(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.DownEntered)
		{
			return true;
		}
		if (method == MethodName.DownProcessing)
		{
			return true;
		}
		if (method == MethodName.DownExited)
		{
			return true;
		}
		if (method == MethodName.UpEntered)
		{
			return true;
		}
		if (method == MethodName.UpProcessing)
		{
			return true;
		}
		if (method == MethodName.UpExited)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.DoublePressed)
		{
			return true;
		}
		if (method == MethodName.Timeout)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
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
		if (name == PropertyName._modeList)
		{
			_modeList = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName.modeId)
		{
			modeId = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName._modeList)
		{
			value = VariantUtils.CreateFrom(in _modeList);
			return true;
		}
		if (name == PropertyName.modeId)
		{
			value = VariantUtils.CreateFrom(in modeId);
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
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._modeList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.modeId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName._modeList, Variant.From(in _modeList));
		info.AddProperty(PropertyName.modeId, Variant.From(in modeId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value))
		{
			_roleStateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._modeList, out var value2))
		{
			_modeList = value2.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName.modeId, out var value3))
		{
			modeId = value3.As<int>();
		}
	}
}
