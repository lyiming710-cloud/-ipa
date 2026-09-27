using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentHypnotizedPlantRecharmRuntimeTest.cs")]
public class BugDepartmentHypnotizedPlantRecharmRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Place = "Place";

		public static readonly StringName IsPurified = "IsPurified";

		public static readonly StringName ContainsCharacter = "ContainsCharacter";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string PeashooterScenePath = "res://Asset/Anime/Character/Plant/Chapter0/PeaShooter/Scene/TowerDefensePlantPeaShooter.tscn";

	private const string HypnoBeanScenePath = "res://Asset/Anime/Character/Plant/Chapter8/HypnoBean/Scene/TowerDefensePlantHypnoBean.tscn";

	private const string HypnoBloverScenePath = "res://Asset/Anime/Character/Plant/Chapter8/HypnoBlover/Scene/TowerDefensePlantHypnoBlover.tscn";

	private static readonly Vector2I SharedGrid = new Vector2I(3, 2);

	private static readonly Vector2I BloverGrid = new Vector2I(1, 2);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		BugDepartmentHypnotizedPlantRecharmRuntimeControlStub control = null;
		TowerDefensePlant peashooter = null;
		TowerDefensePlantHypnoBean hypnoBean = null;
		TowerDefenseCellInstance sharedCell = null;
		try
		{
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00cd;
				}
				control = new BugDepartmentHypnotizedPlantRecharmRuntimeControlStub
				{
					Name = "HypnotizedPlantRecharmRuntimeControl",
					isGameRunning = false,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = Vector2.Zero;
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				peashooter = Instantiate<TowerDefensePlant>("res://Asset/Anime/Character/Plant/Chapter0/PeaShooter/Scene/TowerDefensePlantPeaShooter.tscn");
				hypnoBean = Instantiate<TowerDefensePlantHypnoBean>("res://Asset/Anime/Character/Plant/Chapter8/HypnoBean/Scene/TowerDefensePlantHypnoBean.tscn");
				TowerDefensePlantHypnoBlover hypnoBlover = Instantiate<TowerDefensePlantHypnoBlover>("res://Asset/Anime/Character/Plant/Chapter8/HypnoBlover/Scene/TowerDefensePlantHypnoBlover.tscn");
				Check(GodotObject.IsInstanceValid(peashooter) && GodotObject.IsInstanceValid(hypnoBean) && GodotObject.IsInstanceValid(hypnoBlover) && peashooter.config?.name == "PlantPeaShooter" && hypnoBean.config?.name == "PlantHypnoBean" && hypnoBlover.config?.name == "PlantHypnoBlover", "The fixture must use the real Peashooter, Hypno Bean, and Hypno Blover scenes.");
				if (!GodotObject.IsInstanceValid(peashooter) || !GodotObject.IsInstanceValid(hypnoBean) || !GodotObject.IsInstanceValid(hypnoBlover))
				{
					goto end_IL_00cd;
				}
				control.characterNode.AddChild(peashooter, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(hypnoBean, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(hypnoBlover, forceReadableName: false, InternalMode.Disabled);
				Place(peashooter, SharedGrid);
				Place(hypnoBean, SharedGrid);
				Place(hypnoBlover, BloverGrid);
				await WaitFrames(5);
				peashooter.ProcessMode = ProcessModeEnum.Disabled;
				hypnoBean.ProcessMode = ProcessModeEnum.Disabled;
				hypnoBlover.ProcessMode = ProcessModeEnum.Disabled;
				peashooter.Scale = new Vector2(1.35f, peashooter.Scale.Y);
				sharedCell = new TowerDefenseCellInstance
				{
					gridPos = SharedGrid
				};
				sharedCell.Init(new TowerDefenseCellConfig());
				sharedCell.characterList.Add(peashooter);
				sharedCell.characterList.Add(hypnoBean);
				peashooter.cell = sharedCell;
				hypnoBean.cell = sharedCell;
				Check(peashooter.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT && !peashooter.instance.hypnoses && peashooter.BuffGet("Hypnoses") == null && peashooter.Scale.X > 0f, "The real Peashooter must begin uncharmed in the Plant camp.");
				peashooter.Hypnoses();
				Check(peashooter.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE && peashooter.instance.hypnoses && peashooter.BuffGet("Hypnoses") is TowerDefenseCharacterBuffHypnoses && peashooter.Scale.X < 0f, "The first hypnosis must move the real Peashooter to the Zombie camp with an active Buff.");
				Check(Mathf.IsEqualApprox(Mathf.Abs(peashooter.Scale.X), 1.35f), "Hypnosis entry overwrote the Peashooter's authored X scale magnitude.");
				Check(sharedCell.characterList.Contains(peashooter) && sharedCell.characterList.Contains(hypnoBean) && peashooter.camp != hypnoBean.camp, "The real Hypno Bean must share the target cell and see the hypnotized Peashooter as hostile.");
				hypnoBean.Explode();
				Check(IsPurified(peashooter), "The real Hypno Bean explosion must remove hypnosis and return the Peashooter to Plant camp.");
				Check(Mathf.IsEqualApprox(peashooter.Scale.X, 1.35f), "Hypnosis exit failed to restore forward facing without changing the X scale magnitude.");
				peashooter.Hypnoses();
				Check(peashooter.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE && peashooter.instance.hypnoses && peashooter.BuffGet("Hypnoses") is TowerDefenseCharacterBuffHypnoses, "The Peashooter must support a fresh first hypnosis before the Hypno Blover scenario.");
				Check(ContainsCharacter(manager.GetCampTarget(hypnoBlover.camp), peashooter), "The production camp registry must expose the hypnotized Peashooter to the real Hypno Blover.");
				hypnoBlover.Run();
				Check(hypnoBlover.run && IsPurified(peashooter), "The real Hypno Blover must run its authored global sweep and return the Peashooter to Plant camp.");
				goto end_IL_00c4;
				end_IL_00cd:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[HypnotizedPlantRecharm] Unexpected exception: {value}");
				goto end_IL_00c4;
			}
			return;
			end_IL_00c4:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(peashooter))
			{
				peashooter.cell = null;
			}
			if (GodotObject.IsInstanceValid(hypnoBean))
			{
				hypnoBean.cell = null;
			}
			sharedCell?.characterList.Clear();
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(8);
		}
		bool flag = _failures == 0 && _checks == 11;
		GD.Print($"HYPNOTIZED_PLANT_RECHARM_RESULT version=2 passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static T Instantiate<T>(string path) where T : Node
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
	}

	private static void Place(TowerDefenseCharacter character, Vector2I grid)
	{
		character.inGame = true;
		character.editorPreviewMode = false;
		character.gridPos = grid;
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		character.GlobalPosition = instance.gridBeginPos + new Vector2((float)grid.X * instance.gridSize.X, (float)grid.Y * instance.gridSize.Y);
	}

	private static bool IsPurified(TowerDefenseCharacter character)
	{
		if (character.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT && !character.instance.hypnoses && character.BuffGet("Hypnoses") == null)
		{
			return character.Scale.X > 0f;
		}
		return false;
	}

	private static bool ContainsCharacter(Godot.Collections.Array characters, TowerDefenseCharacter target)
	{
		foreach (Variant character in characters)
		{
			if (character.As<TowerDefenseCharacter>() == target)
			{
				return true;
			}
		}
		return false;
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[HypnotizedPlantRecharm] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Place, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "grid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPurified, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ContainsCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "characters", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Place && args.Count == 2)
		{
			Place(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsPurified && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPurified(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.ContainsCharacter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ContainsCharacter(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Place && args.Count == 2)
		{
			Place(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsPurified && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPurified(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.ContainsCharacter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ContainsCharacter(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.Place)
		{
			return true;
		}
		if (method == MethodName.IsPurified)
		{
			return true;
		}
		if (method == MethodName.ContainsCharacter)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
