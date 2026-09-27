using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewGatlingDolphinPoseRuntimeTest.cs")]
public class BugOverviewGatlingDolphinPoseRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName IsSane = "IsSane";

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

	private const string CharacterScenePath = "res://Asset/Anime/Character/Zombie/Chapter3/Dolphinrider/Scene/TowerDefenseZombieDolphinriderGatlingPea.tscn";

	private static readonly string[] MovementClips = new string[4] { "DolphinWalk", "JumpInWater", "DolphinRun", "DolphinJump" };

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		GatlingDolphinPoseControlStub control = null;
		TowerDefenseZombieDolphinriderGatlingPea zombie = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00c7;
				}
				control = new GatlingDolphinPoseControlStub
				{
					Name = "GatlingDolphinPoseControl",
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
				zombie = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter3/Dolphinrider/Scene/TowerDefenseZombieDolphinriderGatlingPea.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombieDolphinriderGatlingPea>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(zombie), "The real Gatling Dolphin Rider character scene must instantiate.");
				if (!GodotObject.IsInstanceValid(zombie))
				{
					goto end_IL_00c7;
				}
				zombie.editorPreviewMode = true;
				zombie.inGame = false;
				zombie.GlobalPosition = new Vector2(650f, 228f);
				zombie.gridPos = new Vector2I(6, 3);
				control.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(5);
				AdobeAnimateSprite body = zombie.sprite;
				AdobeAnimateSprite head = zombie.GetNodeOrNull<AdobeAnimateSprite>("SpriteGroup/TransformPoint/ZombieDolphinriderGatlingPea/Head");
				AdobeAnimateSlot armSlot = zombie.GetNodeOrNull<AdobeAnimateSlot>("SpriteGroup/TransformPoint/ZombieDolphinriderGatlingPea/ArmSlot");
				Check(GodotObject.IsInstanceValid(body) && GodotObject.IsInstanceValid(head) && GodotObject.IsInstanceValid(armSlot), "The real character must expose its body, nested Gatling Head, and ArmSlot.");
				if (!GodotObject.IsInstanceValid(body) || !GodotObject.IsInstanceValid(head) || !GodotObject.IsInstanceValid(armSlot))
				{
					goto end_IL_00c7;
				}
				bool flag = true;
				string[] movementClips = MovementClips;
				foreach (string clipName in movementClips)
				{
					flag &= body.HasClip(clipName);
				}
				Check(flag, "The shared dolphin body must retain all walk, water-entry, run, and jump clips.");
				Check(head.Visible, "The nested Gatling Head must begin visible in the real character scene.");
				Check(head.parentSprite == body && head.insertLayerId == 19 && head.followParentSpriteLayerId == 19, "The nested Gatling Head must insert into and follow authored body layer 19.");
				Check(armSlot.followSlotId == 27 && armSlot.drawLayerId == 27, "ArmSlot must continue to follow and draw at authored outer-arm layer 27.");
				await VerifyRunTransitionRefreshesHeadAttachment(zombie, body, head);
				string[] movementClips2 = MovementClips;
				foreach (string clip in movementClips2)
				{
					await VerifyMovementClip(zombie, body, head, armSlot, clip);
				}
				goto end_IL_00b0;
				end_IL_00c7:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewGatlingDolphinPoseRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00b0;
			}
			return;
			end_IL_00b0:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(zombie) && !zombie.IsQueuedForDeletion())
			{
				zombie.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			await WaitFrames(3);
		}
		bool flag2 = _failures == 0 && _checks == 26;
		GD.Print($"GATLING_DOLPHIN_POSE_RESULT passed={flag2} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag2) ? 2 : 0);
	}

	private async Task VerifyRunTransitionRefreshesHeadAttachment(TowerDefenseZombieDolphinriderGatlingPea zombie, AdobeAnimateSprite body, AdobeAnimateSprite head)
	{
		body.offset = new Vector2(-40f, body.offset.Y);
		body.SetAnimation("JumpInWater");
		AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
		await WaitFrames(2);
		bool resolvedBefore = TryResolveHeadGraphOwner(body, head, out var beforeGraph, out var headOwner);
		Check(resolvedBefore, "The JumpInWater pose must resolve a GPU graph owner for the nested Gatling Head.");
		Check(resolvedBefore && Mathf.IsEqualApprox(headOwner.ParentOffset.X, -40f), $"The pre-run Gatling Head graph must retain body offset -40; actual={headOwner.ParentOffset.X}.");
		body.SetAnimation("DolphinRun");
		bool flag = body.TryGetInterpolatedSlotPose(head.followParentSpriteLayerId, out var mediaId, out var transform);
		Vector2 vector = transform.Origin + body.offset;
		Check((Mathf.IsEqualApprox(body.offset.X, 24f) & flag) && mediaId != 65535 && head.Position.IsEqualApprox(vector), $"DolphinRun must immediately reattach the Gatling Head after body offset changes to 24; actual={head.Position}, expected={vector}, offset={body.offset}.");
		await WaitFrames(2);
		bool flag2 = TryResolveHeadGraphOwner(body, head, out var graph, out var headOwner2);
		Check(flag2, "The DolphinRun pose must resolve a GPU graph owner for the nested Gatling Head.");
		Check(flag2 && Mathf.IsEqualApprox(headOwner2.ParentOffset.X, 24f), $"The DolphinRun Gatling Head graph must rebuild with body offset 24; actual={headOwner2.ParentOffset.X}.");
		Check((resolvedBefore & flag2) && beforeGraph.Signature != graph.Signature, $"Changing the body attachment offset must invalidate the cached GPU graph; before=0x{beforeGraph?.Signature ?? 0:X16}, after=0x{graph?.Signature ?? 0:X16}.");
		Vector2 globalPosition = head.GlobalPosition;
		Vector2 vector2 = new Vector2(-64f, 0f);
		zombie.GlobalPosition += vector2;
		Check(head.GlobalPosition.IsEqualApprox(globalPosition + vector2), "The Gatling Head must remain attached when the water-entry completion shifts the character root.");
	}

	private static bool TryResolveHeadGraphOwner(AdobeAnimateSprite body, AdobeAnimateSprite head, out AdobeAnimateGpuRenderGraphDefinition graph, out AdobeAnimateGpuRenderOwner headOwner)
	{
		headOwner = default;
		if (!AdobeAnimateRenderManager.TryResolveGpuRenderGraph(body, out graph, out var graphOwners, out var _, out var _, out var _))
		{
			return false;
		}
		for (int i = 0; i < graphOwners.Length; i++)
		{
			if (graphOwners[i] == head)
			{
				headOwner = graph.Owners[i];
				return true;
			}
		}
		return false;
	}

	private async Task VerifyMovementClip(TowerDefenseZombieDolphinriderGatlingPea zombie, AdobeAnimateSprite body, AdobeAnimateSprite head, AdobeAnimateSlot armSlot, string clip)
	{
		body.SetAnimation(clip);
		bool everySampleRenderedHead = true;
		bool everySampleHasHeadPose = true;
		bool everySampleHasArmPose = true;
		int minimumHeadItems = 2147483647;
		for (int sample = 0; sample < 12; sample++)
		{
			zombie.GlobalPosition += new Vector2(-2f, 0f);
			await WaitFrames(1);
			bool flag = body.TryBuildRenderSnapshot(out var snapshot, allowUnchanged: false);
			List<AdobeAnimateDrawItem> list = new List<AdobeAnimateDrawItem>();
			if (flag)
			{
				AdobeAnimateDrawItemBuilder.Build(snapshot, list, snapshot.Definition?.GpuPoseTextureArray);
			}
			int num = 0;
			foreach (AdobeAnimateDrawItem item in list)
			{
				if (item.Owner == head)
				{
					num++;
				}
			}
			minimumHeadItems = Math.Min(minimumHeadItems, num);
			everySampleRenderedHead &= flag && num > 0;
			bool flag2 = body.TryGetInterpolatedSlotPose(head.followParentSpriteLayerId, out var mediaId, out var transform);
			everySampleHasHeadPose &= flag2 && mediaId != 65535 && IsSane(transform);
			bool flag3 = body.TryGetInterpolatedSlotPose(armSlot.followSlotId - 1, out var mediaId2, out var transform2);
			everySampleHasArmPose &= flag3 && mediaId2 != 65535 && IsSane(transform2);
		}
		Check(everySampleRenderedHead, $"The nested Gatling Head must render in all 12 moving {clip} samples; minimum={minimumHeadItems}.");
		Check(everySampleHasHeadPose, "Body layer 19 must provide a finite Gatling Head follow pose in all 12 " + clip + " samples.");
		Check(everySampleHasArmPose, "Body layer 27 must provide a finite outer-arm pose in all 12 " + clip + " samples.");
	}

	private static bool IsSane(Transform2D transform)
	{
		if (IsSane(transform.X.X) && IsSane(transform.X.Y) && IsSane(transform.Y.X) && IsSane(transform.Y.Y) && IsSane(transform.Origin.X))
		{
			return IsSane(transform.Origin.Y);
		}
		return false;
	}

	private static bool IsSane(float value)
	{
		if (float.IsFinite(value))
		{
			return Math.Abs(value) < 4096f;
		}
		return false;
	}

	private async Task WaitFrames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewGatlingDolphinPoseRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsSane, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Transform2D, "transform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IsSane && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSane(VariantUtils.ConvertTo<Transform2D>(in args[0])));
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
		if (method == MethodName.IsSane && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSane(VariantUtils.ConvertTo<Transform2D>(in args[0])));
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
		if (method == MethodName.IsSane)
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
