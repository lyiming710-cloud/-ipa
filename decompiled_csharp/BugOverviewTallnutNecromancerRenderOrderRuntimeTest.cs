using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewTallnutNecromancerRenderOrderRuntimeTest.cs")]
public class BugOverviewTallnutNecromancerRenderOrderRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PrepareCharacter = "PrepareCharacter";

		public static readonly StringName CompareTreePaths = "CompareTreePaths";

		public static readonly StringName PathsEqual = "PathsEqual";

		public static readonly StringName RuntimeRenderRootsMatch = "RuntimeRenderRootsMatch";

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

	private const string TallnutScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Tallnut/TowerDefenseZombieNormalTallnut.tscn";

	private const string NecromancerScenePath = "res://Asset/Anime/Character/Zombie/Chapter8/NecromancerB/Scene/TowerDefenseZombieNecromancerB.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseZombieNormalTallnut tallnut = null;
		TowerDefenseZombieNormalTallnut replacementTallnut = null;
		TowerDefenseZombieNecromancerB necromancer = null;
		try
		{
			_ = 3;
			try
			{
				PackedScene tallnutScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Tallnut/TowerDefenseZombieNormalTallnut.tscn", null, ResourceLoader.CacheMode.Ignore);
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter8/NecromancerB/Scene/TowerDefenseZombieNecromancerB.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(tallnutScene), "The real Tallnut Zombie scene must load.");
				Check(GodotObject.IsInstanceValid(packedScene), "The real giant Necromancer Zombie scene must load.");
				tallnut = tallnutScene?.Instantiate<TowerDefenseZombieNormalTallnut>(PackedScene.GenEditState.Disabled);
				necromancer = packedScene?.Instantiate<TowerDefenseZombieNecromancerB>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(tallnut) && GodotObject.IsInstanceValid(necromancer), "Both real zombies must instantiate.");
				if (!GodotObject.IsInstanceValid(tallnut) || !GodotObject.IsInstanceValid(necromancer))
				{
					goto end_IL_006b;
				}
				PrepareCharacter(tallnut);
				PrepareCharacter(necromancer);
				AddChild(tallnut, forceReadableName: false, InternalMode.Disabled);
				AddChild(necromancer, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(5);
				Check(GodotObject.IsInstanceValid(tallnut.sprite) && GodotObject.IsInstanceValid(necromancer.sprite), "Both real Adobe animation roots must bind.");
				Check(GodotObject.IsInstanceValid(tallnut.headSlot) && GodotObject.IsInstanceValid(necromancer.headSlot), "Both scene-authored HeadSlots must bind.");
				Check(tallnut.headSlot.drawLayerId == -2 && necromancer.headSlot.drawLayerId == -2, $"Scene-authored Top must survive _Ready; tallnut={tallnut.headSlot.drawLayerId}, necromancer={necromancer.headSlot.drawLayerId}.");
				int num = tallnut.headSlot.ResolveDrawLayerId();
				int num2 = necromancer.headSlot.ResolveDrawLayerId();
				Check(num == tallnut.sprite.layerVisible.Count + 1, $"Tallnut HeadSlot must resolve above its current layer range; resolved={num}, layers={tallnut.sprite.layerVisible.Count}.");
				Check(num2 == necromancer.sprite.layerVisible.Count + 1, $"Necromancer HeadSlot must resolve above its current layer range; resolved={num2}, layers={necromancer.sprite.layerVisible.Count}.");
				AdobeAnimateSprite tallnutRoot = tallnut.sprite;
				AdobeAnimateSprite necromancerRoot = necromancer.sprite;
				AdobeAnimateSprite nodeOrNull = tallnutRoot.GetNodeOrNull<AdobeAnimateSprite>("Head");
				AdobeAnimateSprite nodeOrNull2 = necromancerRoot.GetNodeOrNull<AdobeAnimateSprite>("ZombieDuckytube");
				Check(GodotObject.IsInstanceValid(nodeOrNull) && GodotObject.IsInstanceValid(nodeOrNull2), "The real compound child animation roots must exist.");
				Check(tallnutRoot.GetRenderSortRootForRender() == tallnutRoot && necromancerRoot.GetRenderSortRootForRender() == necromancerRoot, "Each zombie must remain its own Crowd render root.");
				Check(nodeOrNull?.GetRenderSortRootForRender() == tallnutRoot && nodeOrNull2?.GetRenderSortRootForRender() == necromancerRoot, "Compound child sprites must stay owned by their character root instead of competing across characters.");
				AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult = tallnutRoot.TryBuildCrowdRenderState(out var tallnutState);
				AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult2 = necromancerRoot.TryBuildCrowdRenderState(out var state);
				Check(adobeAnimateCrowdRenderStateResult == AdobeAnimateCrowdRenderStateResult.Submitted && adobeAnimateCrowdRenderStateResult2 == AdobeAnimateCrowdRenderStateResult.Submitted, $"Both character roots must submit through Crowd; tallnut={adobeAnimateCrowdRenderStateResult}, necromancer={adobeAnimateCrowdRenderStateResult2}.");
				Check(tallnutState.EffectiveZIndex == state.EffectiveZIndex, $"Overlapping fixtures must share one exact-Z ordering group; tallnut={tallnutState.EffectiveZIndex}, necromancer={state.EffectiveZIndex}.");
				Check(tallnutState.Mode != AdobeAnimateCrowdRenderMode.Compact && state.Mode != AdobeAnimateCrowdRenderMode.Compact, $"Compound character roots must use an atomic Crowd graph; tallnut={tallnutState.Mode}, necromancer={state.Mode}.");
				int[] effectiveTreeOrderPathForRender = tallnutRoot.GetEffectiveTreeOrderPathForRender();
				int[] effectiveTreeOrderPathForRender2 = necromancerRoot.GetEffectiveTreeOrderPathForRender();
				Check(CompareTreePaths(effectiveTreeOrderPathForRender, effectiveTreeOrderPathForRender2) < 0, "Exact-Z Crowd roots must retain character tree order when their models overlap.");
				Check(!PathsEqual(effectiveTreeOrderPathForRender, effectiveTreeOrderPathForRender2), "The two character roots must retain distinct stable tree-order paths.");
				Check(RuntimeRenderRootsMatch(tallnutRoot, necromancerRoot), "The topology-sorted runtime root list must match the initial character tree order.");
				AdobeAnimateRenderManager.RenderActive(new AdobeAnimateSprite[2] { necromancerRoot, tallnutRoot }, activeListAlreadyValidated: true, renderRootsAlreadyFiltered: true);
				AdobeAnimateSprite[] array = new AdobeAnimateSprite[2];
				Check((AdobeAnimateRenderManager.TryCopyLastCrowdSubmissionOrderForTests(tallnutRoot, tallnutState.EffectiveZIndex, array, out var totalCount, out var crowdEncoded, out var _) & crowdEncoded) && totalCount == 2 && array[0] == tallnutRoot && array[1] == necromancerRoot, "The public three-parameter RenderActive API must sort reversed Mod input before Crowd encoding.");
				Node parent = tallnutRoot.GetParent();
				Node2D node2D = new Node2D
				{
					Name = "DirectOrderProbe"
				};
				parent.AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
				parent.MoveChild(node2D, 0);
				int[] effectiveTreeOrderPathForRender3 = tallnutRoot.GetEffectiveTreeOrderPathForRender();
				Check(!PathsEqual(effectiveTreeOrderPathForRender, effectiveTreeOrderPathForRender3), "A direct sibling reorder signal must invalidate the render root path immediately.");
				parent.RemoveChild(node2D);
				node2D.Free();
				tallnutRoot.GetEffectiveTreeOrderPathForRender();
				MoveChild(necromancer, 0);
				int[] right = null;
				int[] array2 = null;
				for (int i = 0; i < 601; i++)
				{
					right = tallnutRoot.GetEffectiveTreeOrderPathForRender();
					array2 = necromancerRoot.GetEffectiveTreeOrderPathForRender();
				}
				Check(CompareTreePaths(array2, right) < 0, "The low-frequency fallback must detect an ancestor sibling reorder.");
				Check(!PathsEqual(effectiveTreeOrderPathForRender, right) && !PathsEqual(effectiveTreeOrderPathForRender2, array2), "Fallback rescans must refresh both affected hierarchy paths.");
				await WaitFrames(2);
				Check(RuntimeRenderRootsMatch(necromancerRoot, tallnutRoot), "The runtime root list must publish the ancestor MoveChild order after its fallback rescan.");
				necromancer.QueueFree();
				necromancer = null;
				await WaitFrames(4);
				replacementTallnut = tallnutScene.Instantiate<TowerDefenseZombieNormalTallnut>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(replacementTallnut), "The replacement real Tallnut Zombie must instantiate.");
				if (!GodotObject.IsInstanceValid(replacementTallnut))
				{
					goto end_IL_006b;
				}
				PrepareCharacter(replacementTallnut);
				AddChild(replacementTallnut, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(5);
				AdobeAnimateSprite sprite = replacementTallnut.sprite;
				Check(GodotObject.IsInstanceValid(sprite) && sprite.TryBuildCrowdRenderState(out var state2) == AdobeAnimateCrowdRenderStateResult.Submitted && state2.EffectiveZIndex == tallnutState.EffectiveZIndex, "The appended overlapping animation must submit into the same exact-Z Crowd group.");
				int[] effectiveTreeOrderPathForRender4 = tallnutRoot.GetEffectiveTreeOrderPathForRender();
				int[] right2 = sprite?.GetEffectiveTreeOrderPathForRender();
				Check(CompareTreePaths(effectiveTreeOrderPathForRender4, right2) < 0, "Topology churn must keep the surviving animation before the newly appended animation.");
				Check(!PathsEqual(effectiveTreeOrderPathForRender4, right2), "Live overlapping Crowd roots must not retain colliding cached tree-order paths.");
				Check(RuntimeRenderRootsMatch(tallnutRoot, sprite), "The runtime root list must keep a surviving root before a newly appended replacement.");
				goto end_IL_0050;
				end_IL_006b:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewTallnutNecromancerRenderOrderRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0050;
			}
			return;
			end_IL_0050:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(tallnut))
			{
				tallnut.QueueFree();
			}
			if (GodotObject.IsInstanceValid(necromancer))
			{
				necromancer.QueueFree();
			}
			if (GodotObject.IsInstanceValid(replacementTallnut))
			{
				replacementTallnut.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0 && _checks == 27;
		GD.Print($"TALLNUT_NECROMANCER_RENDER_ORDER_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static void PrepareCharacter(TowerDefenseZombie zombie)
	{
		zombie.inGame = false;
		zombie.ProcessMode = ProcessModeEnum.Disabled;
		zombie.GlobalPosition = new Vector2(600f, 180f);
		zombie.ZIndex = 0;
	}

	private static int CompareTreePaths(int[] left, int[] right)
	{
		int num = Math.Min(left?.Length ?? 0, right?.Length ?? 0);
		for (int i = 0; i < num; i++)
		{
			int num2 = left[i].CompareTo(right[i]);
			if (num2 != 0)
			{
				return num2;
			}
		}
		return (left?.Length ?? 0).CompareTo(right?.Length ?? 0);
	}

	private static bool PathsEqual(int[] left, int[] right)
	{
		if ((left?.Length ?? 0) != (right?.Length ?? 0))
		{
			return false;
		}
		for (int i = 0; i < (left?.Length ?? 0); i++)
		{
			if (left[i] != right[i])
			{
				return false;
			}
		}
		return true;
	}

	private static bool RuntimeRenderRootsMatch(params AdobeAnimateSprite[] expected)
	{
		AdobeAnimateRuntimeManager instance = AdobeAnimateRuntimeManager.Instance;
		if (instance == null)
		{
			return false;
		}
		AdobeAnimateSprite[] array = new AdobeAnimateSprite[Math.Max(8, instance.RegisteredSpriteCount)];
		if (!AdobeAnimateRuntimeManager.TryCopyRenderRootOrderForTests(array, out var totalCount, out var dirty) | dirty)
		{
			return false;
		}
		int num = -1;
		for (int i = 0; i < expected.Length; i++)
		{
			int num2 = -1;
			for (int j = num + 1; j < totalCount; j++)
			{
				if (array[j] == expected[i])
				{
					num2 = j;
					break;
				}
			}
			if (num2 < 0)
			{
				return false;
			}
			num = num2;
		}
		return true;
	}

	private async Task WaitFrames(int count)
	{
		for (int index = 0; index < count; index++)
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
			GD.PushError("[BugOverviewTallnutNecromancerRenderOrderRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CompareTreePaths, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedInt32Array, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedInt32Array, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PathsEqual, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedInt32Array, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedInt32Array, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RuntimeRenderRootsMatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "expected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.PrepareCharacter && args.Count == 1)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CompareTreePaths && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CompareTreePaths(VariantUtils.ConvertTo<int[]>(in args[0]), VariantUtils.ConvertTo<int[]>(in args[1])));
			return true;
		}
		if (method == MethodName.PathsEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(PathsEqual(VariantUtils.ConvertTo<int[]>(in args[0]), VariantUtils.ConvertTo<int[]>(in args[1])));
			return true;
		}
		if (method == MethodName.RuntimeRenderRootsMatch && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RuntimeRenderRootsMatch(VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[0])));
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
		if (method == MethodName.PrepareCharacter && args.Count == 1)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CompareTreePaths && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CompareTreePaths(VariantUtils.ConvertTo<int[]>(in args[0]), VariantUtils.ConvertTo<int[]>(in args[1])));
			return true;
		}
		if (method == MethodName.PathsEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(PathsEqual(VariantUtils.ConvertTo<int[]>(in args[0]), VariantUtils.ConvertTo<int[]>(in args[1])));
			return true;
		}
		if (method == MethodName.RuntimeRenderRootsMatch && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RuntimeRenderRootsMatch(VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[0])));
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
		if (method == MethodName.PrepareCharacter)
		{
			return true;
		}
		if (method == MethodName.CompareTreePaths)
		{
			return true;
		}
		if (method == MethodName.PathsEqual)
		{
			return true;
		}
		if (method == MethodName.RuntimeRenderRootsMatch)
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
