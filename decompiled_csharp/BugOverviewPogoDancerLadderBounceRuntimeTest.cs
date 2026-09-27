using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewPogoDancerLadderBounceRuntimeTest.cs")]
public class BugOverviewPogoDancerLadderBounceRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PrepareDescendingBounce = "PrepareDescendingBounce";

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

	private const string PogoDancerScenePath = "res://Asset/Anime/Character/Zombie/Chapter5/PogoDancer/Scene/TowerDefenseZombiePogoDancer.tscn";

	private const float PhysicsDelta = 1f / 60f;

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseZombiePogoDancer pogoDancer = null;
		TowerDefenseCharacter ladder = null;
		PackedScene scene = null;
		try
		{
			try
			{
				scene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter5/PogoDancer/Scene/TowerDefenseZombiePogoDancer.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(scene), "The real Pogo Dancer scene must load.");
				pogoDancer = scene?.Instantiate<TowerDefenseZombiePogoDancer>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(pogoDancer), "The real Pogo Dancer scene must instantiate its production script.");
				if (!GodotObject.IsInstanceValid(pogoDancer))
				{
					goto end_IL_0045;
				}
				pogoDancer.editorPreviewMode = true;
				pogoDancer.inGame = false;
				AddChild(pogoDancer, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				pogoDancer.ProcessMode = ProcessModeEnum.Disabled;
				BugOverviewPogoDancerLadderBounceRuntimeTest bugOverviewPogoDancerLadderBounceRuntimeTest = this;
				int condition;
				if (pogoDancer.IsNodeReady())
				{
					GroundHeightComponent groundHeightComponent = pogoDancer.groundHeightComponent;
					condition = ((groundHeightComponent != null && groundHeightComponent.Lifecycle == ComponentRuntimeLifecycle.Active) ? 1 : 0);
				}
				else
				{
					condition = 0;
				}
				bugOverviewPogoDancerLadderBounceRuntimeTest.Check((byte)condition != 0, "The real Pogo Dancer ground-height runtime must be active.");
				Check(pogoDancer.hasPogo && pogoDancer.ySpeed < 0.0, "The authored Pogo Dancer must begin with an active pogo bounce.");
				ladder = new TowerDefenseCharacter();
				TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
				{
					gridPos = new Vector2I(4, 2),
					characterLadder = ladder
				};
				towerDefenseCellInstance.Init(new TowerDefenseCellConfig());
				towerDefenseCellInstance.characterLadder = ladder;
				pogoDancer.cell = towerDefenseCellInstance;
				pogoDancer.cellPercentage = 0.75;
				PrepareDescendingBounce(pogoDancer);
				pogoDancer.groundHeightComponent.DetectEnvironment();
				Check(pogoDancer.groundHeightComponent.onLadder, "The production ground-height runtime must detect the ladder.");
				pogoDancer.groundHeightComponent.BatchUpdate(0.5);
				Check(Mathf.IsEqualApprox((float)pogoDancer.groundHeight, pogoDancer.groundHeightComponent.ladderHeight) && pogoDancer.z < pogoDancer.groundHeight, "The ladder must rise through the descending Pogo Dancer before landing resolution.");
				pogoDancer.PhysiceUpdate(1f / 60f);
				Check(!pogoDancer.isGround && pogoDancer.ySpeed < 0.0 && Mathf.IsEqualApprox((float)pogoDancer.z, (float)pogoDancer.groundHeight), "A ladder-raised landing must invoke Pogo Dancer's landing callback and relaunch it.");
				pogoDancer.PhysiceUpdate(1f / 60f);
				Check(pogoDancer.z > pogoDancer.groundHeight, "The relaunched Pogo Dancer must visibly leave the ladder height.");
				towerDefenseCellInstance.characterLadder = null;
				pogoDancer.groundHeightComponent.DetectEnvironment();
				pogoDancer.groundHeightComponent.BatchUpdate(0.5);
				Check(!pogoDancer.groundHeightComponent.onLadder && Mathf.IsEqualApprox((float)pogoDancer.groundHeight, 0f), "The fixture must leave the ladder before testing repeated reuse.");
				towerDefenseCellInstance.characterLadder = ladder;
				PrepareDescendingBounce(pogoDancer);
				pogoDancer.groundHeightComponent.DetectEnvironment();
				pogoDancer.groundHeightComponent.BatchUpdate(0.5);
				pogoDancer.PhysiceUpdate(1f / 60f);
				Check(pogoDancer.groundHeightComponent.onLadder && !pogoDancer.isGround && pogoDancer.ySpeed < 0.0, "A second ladder ascent must relaunch the same Pogo Dancer again.");
				pogoDancer.hasPogo = false;
				towerDefenseCellInstance.characterLadder = null;
				pogoDancer.groundHeightComponent.DetectEnvironment();
				pogoDancer.groundHeightComponent.BatchUpdate(0.5);
				towerDefenseCellInstance.characterLadder = ladder;
				PrepareDescendingBounce(pogoDancer);
				pogoDancer.groundHeightComponent.DetectEnvironment();
				pogoDancer.groundHeightComponent.BatchUpdate(0.5);
				pogoDancer.PhysiceUpdate(1f / 60f);
				Check(pogoDancer.isGround && Mathf.IsEqualApprox((float)pogoDancer.ySpeed, 0f), "A Pogo Dancer without its pogo must settle on the ladder instead of relaunching.");
				goto end_IL_003c;
				end_IL_0045:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewPogoDancerLadderBounceRuntimeTest] Unexpected exception: {value}");
				goto end_IL_003c;
			}
			return;
			end_IL_003c:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(pogoDancer) && !pogoDancer.IsQueuedForDeletion())
			{
				pogoDancer.QueueFree();
			}
			if (GodotObject.IsInstanceValid(ladder))
			{
				ladder.Free();
			}
			await WaitFrames(4);
			scene?.Dispose();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag = _failures == 0 && _checks == 11;
		GD.Print($"POGO_DANCER_LADDER_BOUNCE_RESULT version=1 passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static void PrepareDescendingBounce(TowerDefenseZombiePogoDancer pogoDancer)
	{
		pogoDancer.groundHeight = 0.0;
		pogoDancer.z = 20.0;
		pogoDancer.isGround = false;
		pogoDancer.ySpeed = 120.0;
		pogoDancer.gravityUse = true;
		pogoDancer.gravity = 490.0;
		pogoDancer.gravityScale = 1.5;
		pogoDancer.pogoPlant = false;
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
			GD.PushError("[BugOverviewPogoDancerLadderBounceRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareDescendingBounce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "pogoDancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.PrepareDescendingBounce && args.Count == 1)
		{
			PrepareDescendingBounce(VariantUtils.ConvertTo<TowerDefenseZombiePogoDancer>(in args[0]));
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
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.PrepareDescendingBounce && args.Count == 1)
		{
			PrepareDescendingBounce(VariantUtils.ConvertTo<TowerDefenseZombiePogoDancer>(in args[0]));
			ret = default;
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
		if (method == MethodName.PrepareDescendingBounce)
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
