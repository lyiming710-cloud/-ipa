using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Other/Sealeaf/Scene/TowerDefensePlantSealeaf.cs")]
public class TowerDefensePlantSealeaf : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName BlockCharacter = "BlockCharacter";

		public static readonly StringName PlayBlock2 = "PlayBlock2";

		public static readonly StringName TryKnockbackNearby = "TryKnockbackNearby";

		public static readonly StringName RaiseDiveHeight = "RaiseDiveHeight";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName _isBlocking = "_isBlocking";

		public static readonly StringName _fireInterval = "_fireInterval";

		public static readonly StringName _knockbackTimer = "_knockbackTimer";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const double KnockbackInterval = 0.5;

	private const double KnockbackGrid = 1.0;

	private const double KnockbackDuration = 0.5;

	private const double KnockbackHurt = 100.0;

	private const double DiveStunTime = 3.0;

	private FireComponent _fireComponent;

	private BlockComponent _blockComponent;

	private bool _isBlocking;

	private double _fireInterval = 1.5;

	private double _knockbackTimer;

	[Export(PropertyHint.None, "")]
	public double fireInterval
	{
		get
		{
			return _fireInterval;
		}
		set
		{
			_fireInterval = value;
			if (IsNodeReady() && _fireComponent != null)
			{
				FireComponent fireComponent = _fireComponent;
				if (fireComponent != null && !fireComponent.IsReleased)
				{
					_fireComponent.fireInterval = (float)value;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			_blockComponent = componentManager.GetRuntime<BlockComponent>();
			if (_blockComponent != null)
			{
				_blockComponent.OnBlock += BlockCharacter;
				_blockComponent.SetCheckRectangleSize(TowerDefenseManager.Instance.GetMapGridSize() * 3.5f);
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_blockComponent != null)
		{
			_blockComponent.OnBlock -= BlockCharacter;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && inGame && GodotObject.IsInstanceValid(instance) && !instance.sleep)
		{
			_knockbackTimer += (float)delta;
			if (_knockbackTimer >= 0.5)
			{
				_knockbackTimer = 0.0;
				TryKnockbackNearby();
			}
		}
	}

	public void BlockCharacter()
	{
		if (!_isBlocking)
		{
			_isBlocking = true;
			itemLayer = TowerDefenseEnum.LAYER_GROUNDITEM.EFFECT;
			sprite.SetAnimation("Block", loop: false, 0.1);
			sprite.AddAnimation("Idle", 0.0);
		}
	}

	private void PlayBlock2()
	{
		if (sprite != null)
		{
			sprite.SetAnimation("Block2", loop: false, 0.1);
			sprite.AddAnimation("Idle", 0.0);
		}
	}

	private void TryKnockbackNearby()
	{
		if ((Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return;
		}
		Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		foreach (TowerDefenseCharacter item in TowerDefenseManager.Instance.GetCharacterTarget(this, checkLine: true))
		{
			if (item is TowerDefenseZombie && GodotObject.IsInstanceValid(item.instance) && !item.die && !item.nearDie && item.camp != camp && item.instance.zombiePhysique < TowerDefenseEnum.ZOMBIE_PHYSIQUE.HUGE && (item.instance.unUseBuffFlags & 0x200) == 0 && (item.instance.unUseBuffFlags & -1) != -1 && !((item.GetLogicalGlobalPosition() - logicalGlobalPosition).Abs().X > mapGridSize.X * 1.25f))
			{
				double num = (instance.hypnoses ? (-1.0) : 1.0);
				if ((item.instance.maskFlags & 0x20) != 0)
				{
					item.BlowBack(1.0 * num, 0.3);
					TowerDefenseCharacterBuffDizziness towerDefenseCharacterBuffDizziness = new TowerDefenseCharacterBuffDizziness();
					towerDefenseCharacterBuffDizziness.time = 3.0;
					item.BuffAdd(towerDefenseCharacterBuffDizziness);
					RaiseDiveHeight(item);
				}
				else
				{
					item.BlowBack(1.0 * num, 0.5);
					TowerDefenseCharacterBuffDizziness towerDefenseCharacterBuffDizziness2 = new TowerDefenseCharacterBuffDizziness();
					towerDefenseCharacterBuffDizziness2.time = 0.5;
					item.BuffAdd(towerDefenseCharacterBuffDizziness2);
				}
				Hurt(100.0);
				PlayBlock2();
			}
		}
	}

	private async void RaiseDiveHeight(TowerDefenseCharacter target)
	{
		if (GodotObject.IsInstanceValid(target) && GodotObject.IsInstanceValid(target.instance))
		{
			Vector2 offset = target.sprite.offset;
			int originalMaskFlags = target.instance.maskFlags;
			target.instance.maskFlags = 9;
			Vector2 vector = offset + new Vector2(0f, 30f);
			Tween tween = target.CreateTween();
			tween.TweenProperty(target.sprite, "offset", vector, 0.2);
			tween.TweenInterval(3.0);
			tween.TweenProperty(target.sprite, "offset", offset, 0.2);
			await ToSignal(tween, Tween.SignalName.Finished);
			if (GodotObject.IsInstanceValid(target) && GodotObject.IsInstanceValid(target.instance) && !target.die && !target.nearDie)
			{
				target.instance.maskFlags = originalMaskFlags;
			}
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Block" || clip == "Block2")
		{
			_isBlocking = false;
			itemLayer = TowerDefenseEnum.LAYER_GROUNDITEM.PLANT;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["fireInterval"] = fireInterval,
			["knockbackTimer"] = _knockbackTimer
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		fireInterval = data.GetValueOrDefault("fireInterval", 1.5).AsDouble();
		_knockbackTimer = data.GetValueOrDefault("knockbackTimer", 0.0).AsDouble();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BlockCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayBlock2, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryKnockbackNearby, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RaiseDiveHeight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BlockCharacter && args.Count == 0)
		{
			BlockCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayBlock2 && args.Count == 0)
		{
			PlayBlock2();
			ret = default;
			return true;
		}
		if (method == MethodName.TryKnockbackNearby && args.Count == 0)
		{
			TryKnockbackNearby();
			ret = default;
			return true;
		}
		if (method == MethodName.RaiseDiveHeight && args.Count == 1)
		{
			RaiseDiveHeight(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.BlockCharacter)
		{
			return true;
		}
		if (method == MethodName.PlayBlock2)
		{
			return true;
		}
		if (method == MethodName.TryKnockbackNearby)
		{
			return true;
		}
		if (method == MethodName.RaiseDiveHeight)
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
		if (name == PropertyName.fireInterval)
		{
			fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._isBlocking)
		{
			_isBlocking = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			_fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._knockbackTimer)
		{
			_knockbackTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.fireInterval)
		{
			value = VariantUtils.CreateFrom<double>(fireInterval);
			return true;
		}
		if (name == PropertyName._isBlocking)
		{
			value = VariantUtils.CreateFrom(in _isBlocking);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			value = VariantUtils.CreateFrom(in _fireInterval);
			return true;
		}
		if (name == PropertyName._knockbackTimer)
		{
			value = VariantUtils.CreateFrom(in _knockbackTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._isBlocking, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._knockbackTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fireInterval, Variant.From<double>(fireInterval));
		info.AddProperty(PropertyName._isBlocking, Variant.From(in _isBlocking));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
		info.AddProperty(PropertyName._knockbackTimer, Variant.From(in _knockbackTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fireInterval, out var value))
		{
			fireInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._isBlocking, out var value2))
		{
			_isBlocking = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value3))
		{
			_fireInterval = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._knockbackTimer, out var value4))
		{
			_knockbackTimer = value4.As<double>();
		}
	}
}
