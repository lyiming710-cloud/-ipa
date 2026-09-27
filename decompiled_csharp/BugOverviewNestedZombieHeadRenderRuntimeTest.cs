using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewNestedZombieHeadRenderRuntimeTest.cs")]
public class BugOverviewNestedZombieHeadRenderRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Prepare = "Prepare";

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

	private const string ChomperScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Chomper/TowerDefenseZombieNormalChomper.tscn";

	private const string GatlingZamboniScenePath = "res://Asset/Anime/Character/Zombie/Chapter2/Zamboni/Scene/GatlingPea/TowerDefenseZombieZamboniGatlingPea.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		NestedZombieHeadRenderControlStub control = null;
		TowerDefenseZombieNormalChomper chomper = null;
		TowerDefenseZombieZamboniGatlingPea gatlingZamboni = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00ce;
				}
				control = new NestedZombieHeadRenderControlStub
				{
					Name = "NestedZombieHeadRenderControl",
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
				chomper = Instantiate<TowerDefenseZombieNormalChomper>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Chomper/TowerDefenseZombieNormalChomper.tscn");
				gatlingZamboni = Instantiate<TowerDefenseZombieZamboniGatlingPea>("res://Asset/Anime/Character/Zombie/Chapter2/Zamboni/Scene/GatlingPea/TowerDefenseZombieZamboniGatlingPea.tscn");
				Check(GodotObject.IsInstanceValid(chomper), "The real Chomper Zombie scene must instantiate.");
				Check(GodotObject.IsInstanceValid(gatlingZamboni), "The real Gatling Zamboni scene must instantiate.");
				if (!GodotObject.IsInstanceValid(chomper) || !GodotObject.IsInstanceValid(gatlingZamboni))
				{
					goto end_IL_00ce;
				}
				Prepare(chomper, new Vector2(350f, 152f), new Vector2I(3, 2));
				Prepare(gatlingZamboni, new Vector2(650f, 228f), new Vector2I(6, 3));
				control.characterNode.AddChild(chomper, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(gatlingZamboni, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(5);
				await VerifyNestedHead(chomper, "SpriteGroup/TransformPoint/ZombieNormalChomper/Head", 7, new string[2] { "Idle2", "Walk1&Walk2" }, "Chomper Zombie");
				await VerifyNestedHead(gatlingZamboni, "SpriteGroup/TransformPoint/ZombieZamboniGatlingPea/Head", 11, new string[2] { "Drive", "Wheelie&Wheelie2" }, "Gatling Zamboni");
				goto end_IL_00b7;
				end_IL_00ce:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewNestedZombieHeadRenderRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00b7;
			}
			return;
			end_IL_00b7:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(chomper))
			{
				chomper.QueueFree();
			}
			if (GodotObject.IsInstanceValid(gatlingZamboni))
			{
				gatlingZamboni.QueueFree();
			}
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
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks == 29;
		GD.Print($"NESTED_ZOMBIE_HEAD_RENDER_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyNestedHead(TowerDefenseCharacter character, NodePath headPath, int expectedFollowLayer, string[] bodyClips, string label)
	{
		AdobeAnimateSprite body = character.sprite;
		AdobeAnimateSprite head = character.GetNodeOrNull<AdobeAnimateSprite>(headPath);
		Check(GodotObject.IsInstanceValid(body) && GodotObject.IsInstanceValid(head), label + " must expose its real body and nested Head sprites.");
		if (!GodotObject.IsInstanceValid(body) || !GodotObject.IsInstanceValid(head))
		{
			return;
		}
		Check(head.Visible, label + " nested Head must begin visible.");
		Check(head.parentSprite == body && head.followParentSpriteLayerId == expectedFollowLayer, $"{label} nested Head must remain attached to authored body layer {expectedFollowLayer}.");
		Check(head.flashAnimeData != null && head.flashAnimeData.mediaDictionary.Count > 0 && head.flashAnimeData.clips.Count > 0, label + " nested Head must retain its real animation media and clips.");
		int num = 0;
		int num2 = Math.Min(body.mediaReplace.Count, body.mediaReplaceUse.Count);
		for (int i = 0; i < num2; i++)
		{
			if (body.mediaReplaceUse[i] && GodotObject.IsInstanceValid(body.mediaReplace[i]))
			{
				num++;
			}
		}
		Check(num > 0, label + " must retain its serialized body media replacements after initialization.");
		Check(body.mediaReplaceAtlasShared && body.mediaReplaceAtlasUsesTextureArray && GodotObject.IsInstanceValid(body.mediaReplaceAtlasArray), label + " serialized body media replacements must resolve into the shared texture array before first render.");
		bool graphBuilt = AdobeAnimateGpuRenderGraphBuilder.TryBuild(body, out var graph, out var ownerSprites, out var failureReason);
		Check(graphBuilt, label + " must build a GPU render graph containing its nested Head; reason=" + failureReason + ".");
		int headOwnerIndex = -1;
		if (graphBuilt)
		{
			for (int j = 0; j < ownerSprites.Length; j++)
			{
				if (ownerSprites[j] == head)
				{
					headOwnerIndex = j;
					break;
				}
			}
		}
		Check(headOwnerIndex > 0, $"{label} nested Head must be a child owner in the GPU render graph; owners={ownerSprites?.Length ?? 0}.");
		int num3 = 0;
		if (headOwnerIndex > 0)
		{
			AdobeAnimateGpuRenderSlot[] renderSlots = graph.RenderSlots;
			foreach (AdobeAnimateGpuRenderSlot adobeAnimateGpuRenderSlot in renderSlots)
			{
				if (adobeAnimateGpuRenderSlot.OwnerIndex == headOwnerIndex)
				{
					num3++;
				}
			}
		}
		Check(num3 > 0, $"{label} nested Head GPU owner must expose render slots; slots={num3}.");
		foreach (string bodyClip in bodyClips)
		{
			body.SetAnimation(bodyClip);
			bool everySampleRenderedHead = true;
			bool everyGpuSampleRenderedHead = graphBuilt && headOwnerIndex > 0;
			int minimumHeadItems = 2147483647;
			int minimumGpuHeadSlots = 2147483647;
			for (int sample = 0; sample < 12; sample++)
			{
				await WaitFrames(1);
				bool flag = body.TryBuildRenderSnapshot(out var snapshot, allowUnchanged: false);
				List<AdobeAnimateDrawItem> list = new List<AdobeAnimateDrawItem>();
				if (flag)
				{
					AdobeAnimateDrawItemBuilder.Build(snapshot, list, snapshot.Definition?.GpuPoseTextureArray);
				}
				int num4 = 0;
				foreach (AdobeAnimateDrawItem item in list)
				{
					if (item.Owner == head)
					{
						num4++;
					}
				}
				minimumHeadItems = Math.Min(minimumHeadItems, num4);
				everySampleRenderedHead &= flag && num4 > 0;
				AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult = body.TryBuildCrowdRenderState(out var state);
				int num5 = 0;
				if (adobeAnimateCrowdRenderStateResult == AdobeAnimateCrowdRenderStateResult.Submitted && state.Mode == AdobeAnimateCrowdRenderMode.GpuGraph && headOwnerIndex < state.GpuGraphOwners.Length && state.GpuGraphOwners[headOwnerIndex] == head && head.TryBuildGpuGraphOwnerStateForRender(state.RenderMountParent, out var state2))
				{
					AdobeAnimateGpuRenderOwner adobeAnimateGpuRenderOwner = graph.Owners[headOwnerIndex];
					int num6 = Math.Clamp(state2.FrameIndex, 0, Math.Max(0, adobeAnimateGpuRenderOwner.Definition.Frames.Length - 1));
					for (int m = 0; m < adobeAnimateGpuRenderOwner.LocalSlotCount; m++)
					{
						int num7 = adobeAnimateGpuRenderOwner.FrameLookupBase + num6 * adobeAnimateGpuRenderOwner.LocalSlotCount + m;
						if ((uint)num7 < (uint)graph.FrameSlotLookup.Length && graph.FrameSlotLookup[num7].Visible)
						{
							num5++;
						}
					}
				}
				minimumGpuHeadSlots = Math.Min(minimumGpuHeadSlots, num5);
				everyGpuSampleRenderedHead &= num5 > 0;
			}
			Check(everySampleRenderedHead, $"{label} nested Head must contribute GPU-pose draw items in all 12 {bodyClip} samples; minimum={minimumHeadItems}.");
			Check(everyGpuSampleRenderedHead, $"{label} nested Head must remain visible in GPU Graph state in all 12 {bodyClip} samples; minimum={minimumGpuHeadSlots}.");
		}
	}

	private static void Prepare(TowerDefenseCharacter character, Vector2 position, Vector2I gridPos)
	{
		character.editorPreviewMode = true;
		character.inGame = false;
		character.GlobalPosition = position;
		character.gridPos = gridPos;
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
			GD.PushError("[BugOverviewNestedZombieHeadRenderRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Prepare, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Prepare && args.Count == 3)
		{
			Prepare(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]));
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
		if (method == MethodName.Prepare && args.Count == 3)
		{
			Prepare(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]));
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
		if (method == MethodName.Prepare)
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
