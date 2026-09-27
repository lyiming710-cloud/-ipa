using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewSunGodJalapenoSunRuntimeTest.cs")]
public class BugOverviewSunGodJalapenoSunRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Check = "Check";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string SunGodScenePath = "res://Asset/Anime/Character/Plant/Diamond/SunGodBean/Scene/TowerDefensePlantSunGodBean.tscn";

	private const string JalaSunShroomScenePath = "res://Asset/Anime/Character/Plant/Chapter2/JalaSunShroom/Scene/TowerDefensePlantJalaSunShroom.tscn";

	private readonly List<string> _failures = new List<string>();

	private int _checks;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		SunGodJalapenoSunRuntimeControlStub control = null;
		TowerDefensePlantSunGodBean sunGod = null;
		TowerDefensePlantJalaSunShroom jalaSunShroom = null;
		try
		{
			_ = 3;
			try
			{
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ResourceManager.Instance) && GodotObject.IsInstanceValid(ObjectManager.Instance), "Required runtime autoloads must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance) || !GodotObject.IsInstanceValid(ObjectManager.Instance))
				{
					goto end_IL_007b;
				}
				await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
				control = new SunGodJalapenoSunRuntimeControlStub
				{
					Name = "SunGodJalapenoSunRuntimeControl",
					isGameRunning = true,
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
				jalaSunShroom = Instantiate<TowerDefensePlantJalaSunShroom>("res://Asset/Anime/Character/Plant/Chapter2/JalaSunShroom/Scene/TowerDefensePlantJalaSunShroom.tscn");
				Check(GodotObject.IsInstanceValid(jalaSunShroom) && jalaSunShroom.config?.name == "PlantJalaSunShroom", "The fixture must load the real Jalapeno Sun-shroom scene.");
				if (!GodotObject.IsInstanceValid(jalaSunShroom))
				{
					goto end_IL_007b;
				}
				jalaSunShroom.inGame = true;
				jalaSunShroom.gridPos = new Vector2I(3, 2);
				jalaSunShroom.GlobalPosition = new Vector2(500f, 300f);
				control.characterNode.AddChild(jalaSunShroom, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(5);
				manager.CharacterRegister(jalaSunShroom);
				ProduceComponent produce = jalaSunShroom.componentManager?.GetRuntime<ProduceComponent>();
				Check(produce != null && !produce.IsReleased && produce.produceType == "JalaSun" && produce.num == 15, "The real Jalapeno Sun-shroom must expose its authored JalaSun production runtime.");
				sunGod = Instantiate<TowerDefensePlantSunGodBean>("res://Asset/Anime/Character/Plant/Diamond/SunGodBean/Scene/TowerDefensePlantSunGodBean.tscn");
				Check(GodotObject.IsInstanceValid(sunGod) && sunGod.config?.name == "PlantSunGodBean", "The fixture must load the real Sun God Bean scene.");
				if (!GodotObject.IsInstanceValid(sunGod))
				{
					goto end_IL_007b;
				}
				sunGod.inGame = false;
				sunGod.gridPos = new Vector2I(2, 2);
				sunGod.GlobalPosition = new Vector2(400f, 300f);
				control.characterNode.AddChild(sunGod, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(5);
				manager.CharacterRegister(sunGod);
				Check(manager.GetCampFriendly(TowerDefenseEnum.CHARACTER_CAMP.PLANT).Contains(jalaSunShroom), "Sun God Bean must see the real Jalapeno Sun-shroom as a friendly production target.");
				int beforeCount = GetTree().GetNodeCountInGroup("JalapenoSun");
				produce.timer = 0f;
				sunGod.Explode();
				Check(produce.timer >= produce.produceInterval, "Sun God Bean must advance the Jalapeno Sun-shroom to its immediate production tick.");
				produce.PhysicsProcess(0.0, TowerDefenseProcessModeDispatch.CurrentPhysicsFrame);
				await WaitFrames(2);
				Array<Node> nodesInGroup = GetTree().GetNodesInGroup("JalapenoSun");
				Check(nodesInGroup.Count == beforeCount + 1, "Sun God Bean must summon one real Jalapeno Sun drop from the Jalapeno Sun-shroom.");
				bool condition = false;
				foreach (Node item in nodesInGroup)
				{
					if (item is TowerDefenseSunJalapeno { sunNum: var sunNum } && sunNum == produce?.num)
					{
						condition = true;
						break;
					}
				}
				Check(condition, "The summoned Jalapeno Sun must preserve the mushroom's authored production amount.");
				goto end_IL_0060;
				end_IL_007b:;
			}
			catch (Exception value)
			{
				_failures.Add($"Unexpected runtime exception: {value}");
				goto end_IL_0060;
			}
			return;
			end_IL_0060:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			if (GodotObject.IsInstanceValid(sunGod) && !sunGod.IsQueuedForDeletion())
			{
				sunGod.QueueFree();
			}
			if (GodotObject.IsInstanceValid(jalaSunShroom) && !jalaSunShroom.IsQueuedForDeletion())
			{
				jalaSunShroom.QueueFree();
			}
			await WaitFrames(3);
			ObjectManager.Instance?.Clear();
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
		}
		Finish();
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
			_failures.Add(message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PushError("[BugOverviewSunGodJalapenoSunRuntimeTest] " + failure);
		}
		bool flag = _failures.Count == 0 && _checks == 8;
		GD.Print($"SUN_GOD_JALAPENO_SUN_RESULT passed={flag} checks={_checks} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
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
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Finish)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
	}
}
