using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/JewelBomb/Scene/TowerDefensePlantJewelBomb.cs")]
public class TowerDefensePlantJewelBomb : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Explode = "Explode";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private AttackComponent _attackComponent;

	private ExplodeComponent _explodeComponent;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
			_explodeComponent.OnExplode += Explode;
			_attackComponent.SetCheckAreaRectangleSize(0, TowerDefenseManager.Instance.GetMapGridSize() * 2.75f);
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

	public void Explode()
	{
		Dictionary dictionary = new Dictionary();
		foreach (TowerDefenseCharacter item in new List<TowerDefenseCharacter>(_attackComponent.GetCharcterList()))
		{
			if (!(item is TowerDefenseZombie) || item.instance.zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
			{
				string name = item.config.name;
				if (!dictionary.ContainsKey(name))
				{
					dictionary[name] = new Dictionary
					{
						{ "Num", 0 },
						{
							"Type",
							(item is TowerDefensePlant) ? "Plant" : "Zombie"
						},
						{
							"CharacterList",
							new Array()
						}
					};
				}
				((Dictionary)dictionary[name])["Num"] = ((Dictionary)dictionary[name])["Num"].AsInt32() + 1;
				((Array)((Dictionary)dictionary[name])["CharacterList"]).Add(item);
			}
		}
		foreach (Variant key in dictionary.Keys)
		{
			Dictionary dictionary2 = (Dictionary)dictionary[key];
			if (dictionary2["Num"].AsInt32() < 3)
			{
				continue;
			}
			string text = dictionary2["Type"].AsString();
			if (!(text == "Plant"))
			{
				if (!(text == "Zombie"))
				{
					continue;
				}
				foreach (Variant item2 in (Array)dictionary2["CharacterList"])
				{
					TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item2;
					towerDefenseCharacter.SunCreate(towerDefenseCharacter.GetLogicalGlobalPosition(), 25L);
					towerDefenseCharacter.Destroy();
				}
				continue;
			}
			foreach (Variant item3 in (Array)dictionary2["CharacterList"])
			{
				TowerDefenseCharacter towerDefenseCharacter2 = (TowerDefenseCharacter)(GodotObject)item3;
				towerDefenseCharacter2.SunCreate(towerDefenseCharacter2.GetLogicalGlobalPosition(), (int)towerDefenseCharacter2.cost);
				towerDefenseCharacter2.Destroy();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.Explode)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
