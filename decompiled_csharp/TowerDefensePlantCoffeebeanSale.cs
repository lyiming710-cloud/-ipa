using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter7/CoffeebeanSale/Scene/TowerDefensePlantCoffeebeanSale.cs")]
public class TowerDefensePlantCoffeebeanSale : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName PreSpawn = "PreSpawn";

		public static readonly StringName Explode = "Explode";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName changeCost = "changeCost";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private ExplodeComponent _explodeComponent;

	[Export(PropertyHint.None, "")]
	public TowerDefensePacketChangeCost changeCost;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
			_explodeComponent.OnExplode += Explode;
			changeCost = (TowerDefensePacketChangeCost)changeCost.Duplicate();
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

	public override void PreSpawn()
	{
		base.PreSpawn();
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		List<TowerDefenseCharacter> characterList = cell.GetCharacterList();
		Array<TowerDefenseCharacter> array = new Array<TowerDefenseCharacter>();
		foreach (TowerDefenseCharacter item in characterList)
		{
			if (item is TowerDefensePlant && item.config.name != config.name && item.camp == camp)
			{
				array.Add(item);
			}
		}
		if (array.Count <= 0)
		{
			return;
		}
		foreach (TowerDefenseCharacter item2 in array)
		{
			if (GodotObject.IsInstanceValid(item2))
			{
				item2.WakeUp();
				if (!instance.hypnoses)
				{
					item2.packet.ChangeCostAdd(changeCost);
				}
			}
		}
	}

	public void Explode()
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		List<TowerDefenseCharacter> characterList = cell.GetCharacterList();
		Array<TowerDefenseCharacter> array = new Array<TowerDefenseCharacter>();
		foreach (TowerDefenseCharacter item in characterList)
		{
			if (item is TowerDefensePlant && item.config.name != config.name && item.camp == camp)
			{
				array.Add(item);
			}
		}
		if (array.Count <= 0)
		{
			return;
		}
		foreach (TowerDefenseCharacter item2 in array)
		{
			if (GodotObject.IsInstanceValid(item2))
			{
				item2.WakeUp();
				if (instance.hypnoses || !item2.packet.ChangeCostAdd(changeCost))
				{
					if (instance.hypnoses)
					{
						BrainSunCreate(logicalGlobalPosition, 50L);
					}
					else
					{
						SunCreate(logicalGlobalPosition, 50L);
					}
				}
			}
			else if (instance.hypnoses)
			{
				BrainSunCreate(logicalGlobalPosition, 50L);
			}
			else
			{
				SunCreate(logicalGlobalPosition, 50L);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.PreSpawn && args.Count == 0)
		{
			PreSpawn();
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
		if (method == MethodName.PreSpawn)
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
		if (name == PropertyName.changeCost)
		{
			changeCost = VariantUtils.ConvertTo<TowerDefensePacketChangeCost>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.changeCost)
		{
			value = VariantUtils.CreateFrom(in changeCost);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.changeCost, PropertyHint.ResourceType, "TowerDefensePacketChangeCost", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.changeCost, Variant.From(in changeCost));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.changeCost, out var value))
		{
			changeCost = value.As<TowerDefensePacketChangeCost>();
		}
	}
}
