using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/GargantuarDiggerThrownImpRuntimeTest.cs")]
public class GargantuarDiggerThrownImpRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

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

	private const string ImpDiggerScenePath = "res://Asset/Anime/Character/Zombie/Chapter7/ImpDigger/Scene/TowerDefenseZombieImpDigger.tscn";

	private static readonly Vector2I TestGrid = new Vector2I(4, 2);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		AdobeAnimateRenderBackend previousBackend = Global.Instance.adobeAnimateRenderBackend;
		GargantuarDiggerThrownImpRuntimeControlStub control = null;
		TowerDefenseZombieImpDigger imp = null;
		PackedScene packed = null;
		try
		{
			_ = 5;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					throw new InvalidOperationException("TowerDefenseManager autoload is unavailable.");
				}
				control = new GargantuarDiggerThrownImpRuntimeControlStub
				{
					Name = "GargantuarDiggerThrownImpRuntimeControl",
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
				manager.gridBeginPos = Vector2.Zero;
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.CpuPose;
				packed = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter7/ImpDigger/Scene/TowerDefenseZombieImpDigger.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
				imp = packed?.Instantiate<TowerDefenseZombieImpDigger>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(imp), "The regression must instantiate the real Miner Imp scene.");
				if (!GodotObject.IsInstanceValid(imp))
				{
					throw new InvalidOperationException("The real Miner Imp scene could not be instantiated.");
				}
				imp.inGame = true;
				imp.editorPreviewMode = false;
				imp.camp = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE;
				imp.gridPos = TestGrid;
				imp.GlobalPosition = new Vector2(130f, 152f);
				imp.currentArmor = new Array<string>();
				control.characterNode.AddChild(imp, forceReadableName: false, InternalMode.Disabled);
				manager.CharacterRegister(imp);
				await WaitFrames(6);
				Check(imp.SceneFilePath == "res://Asset/Anime/Character/Zombie/Chapter7/ImpDigger/Scene/TowerDefenseZombieImpDigger.tscn" && imp.config?.name == "ZombieImpDigger", "The regression must use the authored Miner Imp character/config chain.");
				imp._throw = true;
				imp.landOver = true;
				imp.digOver = false;
				imp.Scale = Vector2.One;
				imp.timeScale = 1.0;
				imp.sprite.pause = false;
				imp.sprite.timeScale = 1.0;
				imp.GlobalPosition = new Vector2(130f, 152f);
				imp.Walk();
				await WaitFrames(1);
				Check(!imp.digOver && imp.CurrentStateHandle?.StableId == "zombie.imp_digger.dig", "A thrown Miner Imp must enter Dig after landing instead of starting its return walk.");
				float x = imp.GlobalPosition.X;
				imp.DigProcessing(1.0);
				float x2 = imp.GlobalPosition.X;
				Check(x2 < x && !imp.digOver && imp.CurrentStateHandle?.StableId == "zombie.imp_digger.dig", "The landed Miner Imp must continue digging toward the left boundary.");
				for (int i = 0; i < 4; i++)
				{
					if (imp.digOver)
					{
						break;
					}
					imp.DigProcessing(1.0);
				}
				await WaitFrames(1);
				Check(imp.digOver && (double)imp.GlobalPosition.X < manager.GetMapGroundLeft() + 30.0 && imp.CurrentStateHandle?.StableId == "zombie.imp_digger.drill", "The Miner Imp must finish digging only at the authored left-edge threshold.");
				Check(imp.Scale.X < 0f, "At the left edge the non-hypnotized Miner Imp must flip toward its return route.");
				imp.AnimeCompleted("Drill");
				await WaitFrames(1);
				Check(imp.CurrentStateHandle?.StableId == "zombie.imp_digger.land2", "The Miner Imp must use its authored emergence landing state.");
				imp.AnimeCompleted("Dizzy");
				await WaitFrames(1);
				Check(imp.CurrentStateHandle?.StableId == "zombie.walk" && imp.Scale.X < 0f, "After emerging, the Miner Imp must enter the right-facing return walk.");
				float returnStartX = imp.GlobalPosition.X;
				imp.timeScale = 4.0;
				await WaitFrames(45);
				Check(imp.GlobalPosition.X > returnStartX, "The real return-walk animation must move the Miner Imp back to the right.");
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[GargantuarDiggerThrownImpRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(manager))
			{
				if (GodotObject.IsInstanceValid(imp))
				{
					manager.CharacterUnregister(imp);
				}
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(imp))
			{
				imp.QueueFree();
			}
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
			}
			Global.Instance.adobeAnimateRenderBackend = previousBackend;
			await WaitFrames(8);
			packed?.Dispose();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag = _failures == 0 && _checks == 10;
		GD.Print($"GARGANTUAR_DIGGER_THROWN_IMP_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
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
			GD.PushError("[GargantuarDiggerThrownImpRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
