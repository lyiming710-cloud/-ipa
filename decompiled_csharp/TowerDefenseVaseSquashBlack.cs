using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Vase/SquashBlack/Scene/TowerDefenseVaseSquashBlack.cs")]
public class TowerDefenseVaseSquashBlack : TowerDefenseVase
{
	public new class MethodName : TowerDefenseVase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName _InitCharacters = "_InitCharacters";

		public static readonly StringName ApplyCapturedCharacters = "ApplyCapturedCharacters";

		public static readonly StringName ApplyCapturedCharacter = "ApplyCapturedCharacter";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ResolvePendingNetworkCaptures = "ResolvePendingNetworkCaptures";

		public static readonly StringName ReleaseCapturedCharacters = "ReleaseCapturedCharacters";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";
	}

	public new class PropertyName : TowerDefenseVase.PropertyName
	{
		public static readonly StringName _captureReleased = "_captureReleased";
	}

	public new class SignalName : TowerDefenseVase.SignalName
	{
	}

	public List<TowerDefenseCharacter> CharacterList = new List<TowerDefenseCharacter>();

	private readonly List<int> _pendingCaptureSyncIds = new List<int>();

	private bool _captureReleased;

	public override void _Ready()
	{
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			base._Ready();
			instance.invincibleSmash = true;
			_InitCharacters();
		}
	}

	private async void _InitCharacters()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		ApplyCapturedCharacters();
	}

	private void ApplyCapturedCharacters()
	{
		for (int num = CharacterList.Count - 1; num >= 0; num--)
		{
			TowerDefenseCharacter towerDefenseCharacter = CharacterList[num];
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter) || towerDefenseCharacter == this)
			{
				CharacterList.RemoveAt(num);
			}
			else
			{
				ApplyCapturedCharacter(towerDefenseCharacter);
			}
		}
	}

	private void ApplyCapturedCharacter(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character) && character != this)
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			Vector2 logicalGlobalPosition2 = character.GetLogicalGlobalPosition();
			character.SetLogicalGlobalPosition(new Vector2(logicalGlobalPosition.X, logicalGlobalPosition2.Y));
			TargetRegistrationComponent targetRegistrationComponent = character.targetRegistrationComponent;
			if (targetRegistrationComponent != null && !targetRegistrationComponent.IsReleased)
			{
				character.targetRegistrationComponent.canProjectileCheck = false;
			}
			character.ProcessMode = ProcessModeEnum.Disabled;
			character.Visible = false;
			character.shadowComponent?.SetShadowVisible(visible: false);
			if (character is TowerDefenseZombie)
			{
				character.RemoveFromGroup("Zombie");
			}
			if (character is TowerDefensePlant)
			{
				character.RemoveFromGroup("Plant");
			}
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		ResolvePendingNetworkCaptures();
	}

	private void ResolvePendingNetworkCaptures()
	{
		if (_captureReleased || _pendingCaptureSyncIds.Count == 0)
		{
			return;
		}
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		if (!GodotObject.IsInstanceValid(currentControl))
		{
			return;
		}
		for (int num = _pendingCaptureSyncIds.Count - 1; num >= 0; num--)
		{
			int key = _pendingCaptureSyncIds[num];
			if (currentControl._syncCharacters.TryGetValue(key, out var value) && GodotObject.IsInstanceValid(value) && value != this)
			{
				if (!CharacterList.Contains(value))
				{
					CharacterList.Add(value);
				}
				ApplyCapturedCharacter(value);
				_pendingCaptureSyncIds.RemoveAt(num);
			}
		}
	}

	private void ReleaseCapturedCharacters()
	{
		_captureReleased = true;
		_pendingCaptureSyncIds.Clear();
		foreach (TowerDefenseCharacter character in CharacterList)
		{
			if (GodotObject.IsInstanceValid(character))
			{
				character.ProcessMode = ProcessModeEnum.Inherit;
				character.Visible = true;
				character.shadowComponent?.SetShadowVisible(!character.inWater);
				TargetRegistrationComponent targetRegistrationComponent = character.targetRegistrationComponent;
				if (targetRegistrationComponent != null && !targetRegistrationComponent.IsReleased)
				{
					character.targetRegistrationComponent.canProjectileCheck = true;
				}
				if (character is TowerDefenseZombie towerDefenseZombie)
				{
					character.AddToGroup("Zombie");
					towerDefenseZombie.WalkReady();
				}
				if (character is TowerDefensePlant)
				{
					character.AddToGroup("Plant");
				}
			}
		}
		CharacterList.Clear();
	}

	public override void DestroySet()
	{
		if (!over)
		{
			over = true;
			ReleaseCapturedCharacters();
			AudioManager.Instance.AudioPlay("VaseBreaking");
			TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(chunkParticles, gridPos);
			towerDefenseEffectParticlesOnce.GlobalPosition = GetLogicalGlobalPosition(transformPoint) - new Vector2(0f, 30f);
			TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Array<string> array = new Array<string>();
		foreach (TowerDefenseCharacter character in CharacterList)
		{
			if (GodotObject.IsInstanceValid(character))
			{
				array.Add(character.Name);
			}
		}
		return new Dictionary
		{
			["capturedCharacterNodeNames"] = array,
			["captureReleased"] = _captureReleased
		};
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		CharacterList.Clear();
		_captureReleased = data.GetValueOrDefault("captureReleased", false).AsBool();
		if (_captureReleased || !data.ContainsKey("capturedCharacterNodeNames"))
		{
			return;
		}
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(node2D))
		{
			return;
		}
		foreach (Variant item in data["capturedCharacterNodeNames"].AsGodotArray())
		{
			string text = item.AsString();
			if (!string.IsNullOrEmpty(text))
			{
				TowerDefenseCharacter nodeOrNull = node2D.GetNodeOrNull<TowerDefenseCharacter>(new NodePath(text));
				if (GodotObject.IsInstanceValid(nodeOrNull) && nodeOrNull != this && !CharacterList.Contains(nodeOrNull))
				{
					CharacterList.Add(nodeOrNull);
				}
			}
		}
		ApplyCapturedCharacters();
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		Array<int> array = new Array<int>();
		foreach (TowerDefenseCharacter character in CharacterList)
		{
			if (GodotObject.IsInstanceValid(character) && character.syncId >= 0)
			{
				array.Add(character.syncId);
			}
		}
		return new Dictionary
		{
			["capturedSyncIds"] = array,
			["captureReleased"] = _captureReleased
		};
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		if (data.GetValueOrDefault("captureReleased", false).AsBool())
		{
			ReleaseCapturedCharacters();
			return;
		}
		_captureReleased = false;
		_pendingCaptureSyncIds.Clear();
		foreach (Variant item in data.GetValueOrDefault("capturedSyncIds", new Array<int>()).AsGodotArray())
		{
			int num = item.AsInt32();
			if (num >= 0 && !_pendingCaptureSyncIds.Contains(num))
			{
				_pendingCaptureSyncIds.Add(num);
			}
		}
		ResolvePendingNetworkCaptures();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._InitCharacters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyCapturedCharacters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyCapturedCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolvePendingNetworkCaptures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseCapturedCharacters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName._InitCharacters && args.Count == 0)
		{
			_InitCharacters();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCapturedCharacters && args.Count == 0)
		{
			ApplyCapturedCharacters();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCapturedCharacter && args.Count == 1)
		{
			ApplyCapturedCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePendingNetworkCaptures && args.Count == 0)
		{
			ResolvePendingNetworkCaptures();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseCapturedCharacters && args.Count == 0)
		{
			ReleaseCapturedCharacters();
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
		if (method == MethodName.ImportVariantSaveWhenEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportVariantSaveWhenEmpty());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpecialState());
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState && args.Count == 1)
		{
			ImportNetworkSpecialState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName._InitCharacters)
		{
			return true;
		}
		if (method == MethodName.ApplyCapturedCharacters)
		{
			return true;
		}
		if (method == MethodName.ApplyCapturedCharacter)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.ResolvePendingNetworkCaptures)
		{
			return true;
		}
		if (method == MethodName.ReleaseCapturedCharacters)
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
		if (method == MethodName.ImportVariantSaveWhenEmpty)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._captureReleased)
		{
			_captureReleased = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._captureReleased)
		{
			value = VariantUtils.CreateFrom(in _captureReleased);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._captureReleased, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._captureReleased, Variant.From(in _captureReleased));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._captureReleased, out var value))
		{
			_captureReleased = value.As<bool>();
		}
	}
}
