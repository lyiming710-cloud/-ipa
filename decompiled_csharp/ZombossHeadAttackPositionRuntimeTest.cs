using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ZombossHeadAttackPositionRuntimeTest.cs")]
public class ZombossHeadAttackPositionRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifyBallSpawnMarkerRefresh = "VerifyBallSpawnMarkerRefresh";

		public static readonly StringName ReadBallSpawnMarkerPosition = "ReadBallSpawnMarkerPosition";

		public static readonly StringName BeginHeadAttack = "BeginHeadAttack";

		public static readonly StringName IsHeadTweenRunning = "IsHeadTweenRunning";

		public static readonly StringName GetExpectedHeadOffset = "GetExpectedHeadOffset";

		public static readonly StringName GetExpectedArmOffset = "GetExpectedArmOffset";

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

	private const string RegularSpriteScenePath = "res://Asset/Anime/Character/Zombie/Boss/Boss/ZombieBoss.tscn";

	private const string DaveSpriteScenePath = "res://Asset/Anime/Character/Zombie/Boss/BossDave/ZombieBossDave.tscn";

	private int _checks;

	private readonly List<string> _failures = new List<string>();

	public override async void _Ready()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		try
		{
			Check(GodotObject.IsInstanceValid(instance), "测试必须能够使用正式战斗管理器。");
			if (!GodotObject.IsInstanceValid(instance))
			{
				throw new InvalidOperationException("缺少正式战斗管理器。");
			}
			await VerifySprite("res://Asset/Anime/Character/Zombie/Boss/Boss/ZombieBoss.tscn", isDave: false, "普通僵王");
			await VerifySprite("res://Asset/Anime/Character/Zombie/Boss/BossDave/ZombieBossDave.tscn", isDave: true, "戴夫僵王");
		}
		catch (Exception value)
		{
			_failures.Add($"运行测试出现异常：{value}");
		}
		finally
		{
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		Finish();
	}

	private async Task VerifySprite(string scenePath, bool isDave, string label)
	{
		PackedScene scene = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Ignore);
		AdobeAnimateSprite sprite = scene?.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		Check(GodotObject.IsInstanceValid(sprite), label + "必须从正式动画场景实例化。");
		if (!GodotObject.IsInstanceValid(sprite))
		{
			scene?.Dispose();
			return;
		}
		Node2D host = new Node2D
		{
			Name = label + "HeadAttackHost"
		};
		AddChild(host, forceReadableName: false, InternalMode.Disabled);
		host.AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
		await WaitFrames(3);
		Node2D armNode = sprite.GetNodeOrNull<Node2D>("%ArmNode");
		Check(GodotObject.IsInstanceValid(armNode), label + "必须保留正式机械臂节点。");
		bool flag = AdobeAnimateGpuRenderGraphBuilder.TryBuild(sprite, out var graph, out var ownerSprites, out var failureReason);
		Check(flag && Array.Exists(ownerSprites, (AdobeAnimateSprite owner) => owner == sprite), $"{label}必须进入正式 GPU 渲染图；原因={failureReason}，拥有者={ownerSprites?.Length ?? 0}，槽位={(graph?.RenderSlots?.Length).GetValueOrDefault()}。");
		float initialHeadOffset = sprite.offset.Y;
		ulong initialPoseRevision = sprite.ManagedPoseRevision;
		BeginHeadAttack(sprite, isDave, 1, 1.0);
		await WaitFrames(6);
		float topLineHeadOffset = GetExpectedHeadOffset(1);
		Check(sprite.offset.Y < initialHeadOffset - 1f && !Mathf.IsEqualApprox(sprite.offset.Y, topLineHeadOffset), $"{label}吐球前头部补间必须真实移动且尚未结束；初始={initialHeadOffset}，当前={sprite.offset.Y}，目标={topLineHeadOffset}。");
		Check(sprite.ManagedPoseRevision > initialPoseRevision + 2, $"{label}完整 offset 补间必须持续发布 GPU 姿态；初始修订={initialPoseRevision}，当前修订={sprite.ManagedPoseRevision}。");
		await WaitFrames(90);
		Check(Mathf.IsEqualApprox(sprite.offset.Y, topLineHeadOffset), $"{label}补间完成后头部必须同步到第一行；期望={topLineHeadOffset}，实际={sprite.offset.Y}。");
		Check(GodotObject.IsInstanceValid(armNode) && Mathf.IsEqualApprox(armNode.Position.Y, GetExpectedArmOffset(1)), label + "补间完成后机械臂必须同步到第一行。");
		Check(!IsHeadTweenRunning(sprite, isDave), label + "补间完成后不得保留继续改写位置的头部补间。");
		int lastLine = TowerDefenseManager.Instance.GetMapGridNum().Y;
		BeginHeadAttack(sprite, isDave, lastLine, 1.0);
		await WaitFrames(90);
		Check(Mathf.IsEqualApprox(sprite.offset.Y, GetExpectedHeadOffset(lastLine)), label + "补间完成后头部必须同步到最后一行。");
		Check(GodotObject.IsInstanceValid(armNode) && Mathf.IsEqualApprox(armNode.Position.Y, GetExpectedArmOffset(lastLine)), label + "补间完成后机械臂必须同步到最后一行。");
		VerifyBallSpawnMarkerRefresh(sprite, isDave, label);
		sprite.QueueFree();
		host.QueueFree();
		await WaitFrames(3);
		scene.Dispose();
	}

	private void VerifyBallSpawnMarkerRefresh(AdobeAnimateSprite sprite, bool isDave, string label)
	{
		Node2D nodeOrNull = sprite.GetNodeOrNull<Node2D>("HeadSlot");
		Marker2D nodeOrNull2 = sprite.GetNodeOrNull<Marker2D>("%BallSpawnMarker");
		Check(GodotObject.IsInstanceValid(nodeOrNull), label + "必须保留正式头部插槽。");
		Check(GodotObject.IsInstanceValid(nodeOrNull2), label + "必须保留正式吐球 Marker。");
		if (!GodotObject.IsInstanceValid(nodeOrNull) || !GodotObject.IsInstanceValid(nodeOrNull2))
		{
			return;
		}
		sprite.SetAnimation("HeadAttack4", loop: false);
		sprite.frameIndex = sprite.clipRange.X;
		sprite.elapsedTimer = 0.0;
		sprite.UpdateChild();
		Vector2 globalPosition = nodeOrNull2.GlobalPosition;
		Vector2 to = (nodeOrNull.GlobalPosition = new Vector2(-4096f, -4096f));
		TowerDefenseCharacter towerDefenseCharacter = new TowerDefenseCharacter
		{
			Name = label + "BallMarkerOwner"
		};
		try
		{
			Vector2 value = ReadBallSpawnMarkerPosition(sprite, isDave, towerDefenseCharacter);
			Check(value.DistanceTo(globalPosition) <= 0.5f, $"{label}吐球嘴部坐标读取前必须刷新当前帧头部插槽；期望={globalPosition}，实际={value}。");
			Check(value.DistanceTo(to) > 100f, $"{label}吐球嘴部坐标不得沿用旧 HeadSlot 左上角位置；实际={value}。");
		}
		finally
		{
			towerDefenseCharacter.Free();
		}
	}

	private static Vector2 ReadBallSpawnMarkerPosition(AdobeAnimateSprite sprite, bool isDave, TowerDefenseCharacter owner)
	{
		if (isDave && sprite is ZombieBossDave zombieBossDave)
		{
			return zombieBossDave.GetBallSpawnMarkerGlobalPos(owner);
		}
		if (sprite is ZombieBoss zombieBoss)
		{
			return zombieBoss.GetBallSpawnMarkerGlobalPos(owner);
		}
		return Vector2.Zero;
	}

	private static void BeginHeadAttack(AdobeAnimateSprite sprite, bool isDave, int line, double time)
	{
		if (isDave && sprite is ZombieBossDave zombieBossDave)
		{
			zombieBossDave.SetHeadAttack(line, time);
		}
		else if (sprite is ZombieBoss zombieBoss)
		{
			zombieBoss.SetHeadAttack(line, time);
		}
	}

	private static bool IsHeadTweenRunning(AdobeAnimateSprite sprite, bool isDave)
	{
		Tween tween = ((isDave && sprite is ZombieBossDave zombieBossDave) ? zombieBossDave.headTween : (sprite as ZombieBoss)?.headTween);
		if (GodotObject.IsInstanceValid(tween))
		{
			return tween.IsRunning();
		}
		return false;
	}

	private static float GetExpectedHeadOffset(int line)
	{
		int y = TowerDefenseManager.Instance.GetMapGridNum().Y;
		float y2 = TowerDefenseManager.Instance.GetMapGridSize().Y;
		int num = (int)Mathf.Floor((float)y / 2f) + 2;
		return -300f + y2 * (float)(line - num);
	}

	private static float GetExpectedArmOffset(int line)
	{
		int y = TowerDefenseManager.Instance.GetMapGridNum().Y;
		float y2 = TowerDefenseManager.Instance.GetMapGridSize().Y;
		int num = (int)Mathf.Floor((float)y / 2f);
		return y2 * (float)(line - num * 2);
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
			GD.PushError("[ZombossHeadAttackPositionRuntimeTest] " + failure);
		}
		bool flag = _failures.Count == 0 && _checks == 29;
		GD.Print($"ZOMBOSS_HEAD_ATTACK_POSITION_RESULT passed={flag} checks={_checks} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyBallSpawnMarkerRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadBallSpawnMarkerPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.BeginHeadAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsHeadTweenRunning, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetExpectedHeadOffset, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetExpectedArmOffset, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.VerifyBallSpawnMarkerRefresh && args.Count == 3)
		{
			VerifyBallSpawnMarkerRefresh(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadBallSpawnMarkerPosition && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ReadBallSpawnMarkerPosition(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[2])));
			return true;
		}
		if (method == MethodName.BeginHeadAttack && args.Count == 4)
		{
			BeginHeadAttack(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsHeadTweenRunning && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsHeadTweenRunning(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetExpectedHeadOffset && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(GetExpectedHeadOffset(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetExpectedArmOffset && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(GetExpectedArmOffset(VariantUtils.ConvertTo<int>(in args[0])));
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
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ReadBallSpawnMarkerPosition && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ReadBallSpawnMarkerPosition(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[2])));
			return true;
		}
		if (method == MethodName.BeginHeadAttack && args.Count == 4)
		{
			BeginHeadAttack(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsHeadTweenRunning && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsHeadTweenRunning(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetExpectedHeadOffset && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(GetExpectedHeadOffset(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetExpectedArmOffset && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(GetExpectedArmOffset(VariantUtils.ConvertTo<int>(in args[0])));
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
		if (method == MethodName.VerifyBallSpawnMarkerRefresh)
		{
			return true;
		}
		if (method == MethodName.ReadBallSpawnMarkerPosition)
		{
			return true;
		}
		if (method == MethodName.BeginHeadAttack)
		{
			return true;
		}
		if (method == MethodName.IsHeadTweenRunning)
		{
			return true;
		}
		if (method == MethodName.GetExpectedHeadOffset)
		{
			return true;
		}
		if (method == MethodName.GetExpectedArmOffset)
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
