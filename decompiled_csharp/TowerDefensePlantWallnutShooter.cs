using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/WallnutShooter/Scene/TowerDefensePlantWallnutShooter.cs")]
public class TowerDefensePlantWallnutShooter : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName SetIsroom = "SetIsroom";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public static readonly StringName Restore = "Restore";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName CreateBowling = "CreateBowling";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName isroom = "isroom";

		public static readonly StringName _isroom = "_isroom";

		public static readonly StringName currentDamagePoint = "currentDamagePoint";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private FireComponent _fireComponent;

	private const string WALLNUT_SHOOTER = "uid://dtaxmmgke1nkh";

	private const string WALLNUT_SHOOTERROOM = "uid://cn4ms671wsyqg";

	private const string WALLNUT_SHOOTERROOM_DAMAGE1 = "uid://cwgg1u47m01gs";

	private const string WALLNUT_SHOOTERROOM_DAMAGE2 = "uid://bu75ftg0rf725";

	private const string WALLNUT_SHOOTER_DAMAGE1 = "uid://cpj46i6gydv5s";

	private const string WALLNUT_SHOOTER_DAMAGE2 = "uid://dd7025whr48re";

	private bool _isroom;

	public string currentDamagePoint = "Damage0";

	public bool isroom
	{
		get
		{
			return _isroom;
		}
		set
		{
			SetIsroom(value);
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			_fireComponent.OnRestore += Restore;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		FireComponent fireComponent = _fireComponent;
		if (fireComponent != null && !fireComponent.IsReleased)
		{
			_fireComponent.OnRestore -= Restore;
		}
	}

	public void SetIsroom(bool isroom)
	{
		_isroom = isroom;
		if (_isroom)
		{
			sprite.SetFliters(new Array { "anim_face2" }, open: true);
			sprite.SetFliters(new Array { "anim_face" }, open: false);
		}
		else
		{
			sprite.SetFliters(new Array { "anim_face2" }, open: false);
			sprite.SetFliters(new Array { "anim_face" }, open: true);
		}
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		currentDamagePoint = damagePointName;
		switch (damagePointName)
		{
		case "Damage0":
			sprite.SetAtlasReplace("WallnutShooter.png", "uid://dtaxmmgke1nkh");
			sprite.SetAtlasReplace("WallnutShooter_boom.png", "uid://cn4ms671wsyqg");
			break;
		case "Damage1":
			sprite.SetAtlasReplace("WallnutShooter.png", "uid://cpj46i6gydv5s");
			sprite.SetAtlasReplace("WallnutShooter_boom.png", "uid://cwgg1u47m01gs");
			break;
		case "Damage2":
			sprite.SetAtlasReplace("WallnutShooter.png", "uid://dd7025whr48re");
			sprite.SetAtlasReplace("WallnutShooter_boom.png", "uid://bu75ftg0rf725");
			break;
		}
	}

	public void Restore()
	{
		isroom = (double)GD.Randf() < 0.05;
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Fire")
		{
			CreateBowling();
		}
	}

	public void CreateBowling()
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		string packetName = (isroom ? "PlantBowlingWallnutShooterBoom" : "PlantBowlingWallnutShooterNormal");
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		TowerDefenseCharacter towerDefenseCharacter = TowerDefenseCharacter.CreateCharacter(packetName, logicalGlobalPosition, gridPos, groundHeight);
		if (instance.hypnoses)
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
				MultiPlayerManager.Instance.SendSpawnCharacterAt(packetName, gridPos.X, gridPos.Y, nextSyncId, 1.0, 1.0, instance.hypnoses, 0.0, useCreate: true, logicalGlobalPosition.X, logicalGlobalPosition.Y, walkAfterSpawn: false, groundHeight);
			}
		}
		switch (currentDamagePoint)
		{
		case "Damage0":
			towerDefenseCharacter.sprite.SetAtlasReplace("WallnutShooter.png", "uid://dtaxmmgke1nkh");
			towerDefenseCharacter.sprite.SetAtlasReplace("WallnutShooter_boom.png", "uid://cn4ms671wsyqg");
			break;
		case "Damage1":
			towerDefenseCharacter.sprite.SetAtlasReplace("WallnutShooter.png", "uid://cpj46i6gydv5s");
			towerDefenseCharacter.sprite.SetAtlasReplace("WallnutShooter_boom.png", "uid://cwgg1u47m01gs");
			break;
		case "Damage2":
			towerDefenseCharacter.sprite.SetAtlasReplace("WallnutShooter.png", "uid://dd7025whr48re");
			break;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "isBoom", isroom },
			{ "currentDamagePoint", currentDamagePoint }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		isroom = data.GetValueOrDefault("isBoom", Variant.From<bool>(false)).AsBool();
		currentDamagePoint = data.GetValueOrDefault("currentDamagePoint", Variant.From<string>("Damage0")).AsString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetIsroom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "isroom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Restore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateBowling, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.SetIsroom && args.Count == 1)
		{
			SetIsroom(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Restore && args.Count == 0)
		{
			Restore();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateBowling && args.Count == 0)
		{
			CreateBowling();
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
		if (method == MethodName.SetIsroom)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.Restore)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.CreateBowling)
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
		if (name == PropertyName.isroom)
		{
			isroom = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isroom)
		{
			_isroom = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.currentDamagePoint)
		{
			currentDamagePoint = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.isroom)
		{
			value = VariantUtils.CreateFrom<bool>(isroom);
			return true;
		}
		if (name == PropertyName._isroom)
		{
			value = VariantUtils.CreateFrom(in _isroom);
			return true;
		}
		if (name == PropertyName.currentDamagePoint)
		{
			value = VariantUtils.CreateFrom(in currentDamagePoint);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._isroom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isroom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentDamagePoint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.isroom, Variant.From<bool>(isroom));
		info.AddProperty(PropertyName._isroom, Variant.From(in _isroom));
		info.AddProperty(PropertyName.currentDamagePoint, Variant.From(in currentDamagePoint));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.isroom, out var value))
		{
			isroom = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isroom, out var value2))
		{
			_isroom = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.currentDamagePoint, out var value3))
		{
			currentDamagePoint = value3.As<string>();
		}
	}
}
