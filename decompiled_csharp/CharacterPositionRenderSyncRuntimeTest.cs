using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/CharacterPositionRenderSyncRuntimeTest.cs")]
public class CharacterPositionRenderSyncRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PrepareCharacter = "PrepareCharacter";

		public static readonly StringName VerifyLogicalPositionEntry = "VerifyLogicalPositionEntry";

		public static readonly StringName VerifyPhysicsFramePositionEntry = "VerifyPhysicsFramePositionEntry";

		public static readonly StringName VerifyTranslationEntry = "VerifyTranslationEntry";

		public static readonly StringName VerifyImmediateTranslation = "VerifyImmediateTranslation";

		public static readonly StringName PrimeCleanRenderTransform = "PrimeCleanRenderTransform";

		public static readonly StringName ReadRenderedTransform = "ReadRenderedTransform";

		public static readonly StringName ReadRenderTransformDirty = "ReadRenderTransformDirty";

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

	private const string ChomperScenePath = "res://Asset/Anime/Character/Plant/Chapter3/ChomperBlow/Scene/TowerDefensePlantChomperBlow.tscn";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter3/ZombieShieldArmor/Scene/TowerDefenseZombieShieldArmor.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		AdobeAnimateRenderBackend previousBackend = Global.Instance.adobeAnimateRenderBackend;
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		try
		{
			Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
			Check(GodotObject.IsInstanceValid(manager), "测试必须取得正式塔防管理器。");
			if (!GodotObject.IsInstanceValid(manager))
			{
				throw new InvalidOperationException("无法取得正式塔防管理器。");
			}
			CharacterPositionRenderSyncControlStub characterPositionRenderSyncControlStub = new CharacterPositionRenderSyncControlStub
			{
				Name = "CharacterPositionRenderSyncControl",
				isGameRunning = true,
				isInit = false
			};
			AddChild(characterPositionRenderSyncControlStub, forceReadableName: false, InternalMode.Disabled);
			characterPositionRenderSyncControlStub.characterNode = new Node2D
			{
				Name = "CharacterNode"
			};
			characterPositionRenderSyncControlStub.AddChild(characterPositionRenderSyncControlStub.characterNode, forceReadableName: false, InternalMode.Disabled);
			manager.currentControl = characterPositionRenderSyncControlStub;
			TowerDefensePlantChomperBlow chomper = Instantiate<TowerDefensePlantChomperBlow>("res://Asset/Anime/Character/Plant/Chapter3/ChomperBlow/Scene/TowerDefensePlantChomperBlow.tscn");
			TowerDefenseZombie zombie = Instantiate<TowerDefenseZombie>("res://Asset/Anime/Character/Zombie/Chapter3/ZombieShieldArmor/Scene/TowerDefenseZombieShieldArmor.tscn");
			Check(GodotObject.IsInstanceValid(chomper), "测试必须实例化正式三叶大嘴花场景。");
			Check(GodotObject.IsInstanceValid(zombie), "测试必须实例化正式铁门僵尸场景。");
			if (!GodotObject.IsInstanceValid(chomper) || !GodotObject.IsInstanceValid(zombie))
			{
				throw new InvalidOperationException("无法实例化三叶大嘴花或铁门僵尸。");
			}
			PrepareCharacter(chomper, new Vector2I(2, 4), new Vector2(200f, 400f));
			PrepareCharacter(zombie, new Vector2I(6, 4), new Vector2(700f, 400f));
			characterPositionRenderSyncControlStub.characterNode.AddChild(chomper, forceReadableName: false, InternalMode.Disabled);
			characterPositionRenderSyncControlStub.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
			await WaitFrames(6);
			Check(GodotObject.IsInstanceValid(zombie.sprite), "正式铁门僵尸必须提供动画身体根节点。");
			ChomperComponent chomperComponent = chomper.componentManager?.GetRuntime<ChomperComponent>();
			Check(chomperComponent != null && !chomperComponent.IsReleased && chomperComponent.suckUse, "正式三叶大嘴花必须启用吸取组件。");
			if (!GodotObject.IsInstanceValid(zombie.sprite) || chomperComponent == null || chomperComponent.IsReleased || !chomperComponent.suckUse)
			{
				throw new InvalidOperationException("三叶大嘴花吸取渲染依赖未准备完成。");
			}
			zombie.sprite.pause = true;
			PrimeCleanRenderTransform(zombie.sprite);
			Vector2 logicalGlobalPosition = zombie.GetLogicalGlobalPosition();
			Transform2D transform2D = ReadRenderedTransform(zombie.sprite);
			BeginSuction(chomper, chomperComponent, zombie);
			Vector2 logicalGlobalPosition2 = zombie.GetLogicalGlobalPosition();
			Transform2D transform2D2 = ReadRenderedTransform(zombie.sprite);
			Vector2 vector = logicalGlobalPosition2 - logicalGlobalPosition;
			Check(vector.IsEqualApprox(new Vector2(-20f, 0f)), $"三叶大嘴花单步吸取必须把逻辑坐标左移 20 像素，实际位移={vector}。");
			Check((transform2D2.Origin - transform2D.Origin).IsEqualApprox(vector), "三叶大嘴花吸取必须在同一次位置写入中推进身体保留变换。");
			Check(transform2D2.IsEqualApprox(zombie.sprite.GlobalTransform), "三叶大嘴花吸取后的身体保留变换必须立即等于当前场景变换。");
			Check(!ReadRenderTransformDirty(zombie.sprite), "精确吸取位移提交后不能等待后续脏标记才能修正身体位置。");
			chomperComponent.SendStateEvent(chomperComponent.idleStateEvent);
			await WaitFrames(1);
			PrimeCleanRenderTransform(zombie.sprite);
			VerifyLogicalPositionEntry(zombie, new Vector2(35f, 0f));
			PrimeCleanRenderTransform(zombie.sprite);
			VerifyPhysicsFramePositionEntry(zombie, new Vector2(-12f, 8f));
			PrimeCleanRenderTransform(zombie.sprite);
			VerifyTranslationEntry(zombie, new Vector2(9f, -5f));
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[CharacterPositionRenderSyncRuntimeTest] 运行测试出现异常：{value}");
		}
		finally
		{
			Global.Instance.adobeAnimateRenderBackend = previousBackend;
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
		}
		bool flag = _failures == 0 && _checks == 21;
		GD.Print($"CHARACTER_POSITION_RENDER_SYNC_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static void PrepareCharacter(TowerDefenseCharacter character, Vector2I gridPosition, Vector2 globalPosition)
	{
		character.editorPreviewMode = true;
		character.inGame = false;
		character.gridPos = gridPosition;
		character.GlobalPosition = globalPosition;
	}

	private static void BeginSuction(TowerDefensePlantChomperBlow chomper, ChomperComponent component, TowerDefenseZombie zombie)
	{
		chomper.Component();
		component.target = zombie;
		component.isSuck = true;
		component.SendStateEvent(component.attackStateEvent);
		chomper.sprite.SetAnimation(component.biteLoopAnimeClips);
		component.AttackProcessing(0.1);
	}

	private void VerifyLogicalPositionEntry(TowerDefenseCharacter character, Vector2 delta)
	{
		Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
		Transform2D renderBefore = ReadRenderedTransform(character.sprite);
		character.SetLogicalGlobalPosition(logicalGlobalPosition + delta);
		VerifyImmediateTranslation(character, logicalGlobalPosition, renderBefore, delta, "公共逻辑坐标入口");
	}

	private void VerifyPhysicsFramePositionEntry(TowerDefenseCharacter character, Vector2 delta)
	{
		Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
		Transform2D renderBefore = ReadRenderedTransform(character.sprite);
		character.SetGlobalPositionForPhysicsFrame(logicalGlobalPosition + delta, Engine.GetPhysicsFrames());
		VerifyImmediateTranslation(character, logicalGlobalPosition, renderBefore, delta, "物理帧坐标入口");
	}

	private void VerifyTranslationEntry(TowerDefenseCharacter character, Vector2 delta)
	{
		Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
		Transform2D renderBefore = ReadRenderedTransform(character.sprite);
		character.TranslateForPhysicsFrame(delta, delta, Engine.GetPhysicsFrames());
		VerifyImmediateTranslation(character, logicalGlobalPosition, renderBefore, delta, "物理帧平移入口");
	}

	private void VerifyImmediateTranslation(TowerDefenseCharacter character, Vector2 logicalBefore, Transform2D renderBefore, Vector2 expectedDelta, string label)
	{
		Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
		Transform2D transform2D = ReadRenderedTransform(character.sprite);
		Check((logicalGlobalPosition - logicalBefore).IsEqualApprox(expectedDelta), $"{label}必须写入预期逻辑位移 {expectedDelta}。");
		Check((transform2D.Origin - renderBefore.Origin).IsEqualApprox(expectedDelta), label + "必须在同一次调用中推进身体保留变换。");
		Check(transform2D.IsEqualApprox(character.sprite.GlobalTransform), label + "后的身体保留变换必须等于当前场景变换。");
		Check(!ReadRenderTransformDirty(character.sprite), label + "不能依赖后续脏标记才完成身体同步。");
	}

	private static void PrimeCleanRenderTransform(AdobeAnimateSprite sprite)
	{
		AdobeAnimateRenderManager.RenderActive(new AdobeAnimateSprite[1] { sprite }, activeListAlreadyValidated: true, renderRootsAlreadyFiltered: true);
		sprite.ClearRetainedRootMotionForStoppedMovement();
	}

	private static Transform2D ReadRenderedTransform(AdobeAnimateSprite sprite)
	{
		return ReadPrivateField<Transform2D>(sprite, "_renderGlobalTransform");
	}

	private static bool ReadRenderTransformDirty(AdobeAnimateSprite sprite)
	{
		return ReadPrivateField<bool>(sprite, "_renderGlobalTransformDirty");
	}

	private static T ReadPrivateField<T>(AdobeAnimateSprite sprite, string fieldName)
	{
		FieldInfo? field = typeof(AdobeAnimateSprite).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
		if (field == null)
		{
			throw new MissingFieldException(typeof(AdobeAnimateSprite).FullName, fieldName);
		}
		return (T)field.GetValue(sprite);
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
			_failures++;
			GD.PushError("[CharacterPositionRenderSyncRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(10)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.PrepareCharacter, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "globalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifyLogicalPositionEntry, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifyPhysicsFramePositionEntry, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifyTranslationEntry, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifyImmediateTranslation, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "logicalBefore", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Transform2D, "renderBefore", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "expectedDelta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.PrimeCleanRenderTransform, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ReadRenderedTransform, new Godot.Bridge.PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ReadRenderTransformDirty, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.PrepareCharacter && args.Count == 3)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyLogicalPositionEntry && args.Count == 2)
		{
			VerifyLogicalPositionEntry(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyPhysicsFramePositionEntry && args.Count == 2)
		{
			VerifyPhysicsFramePositionEntry(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyTranslationEntry && args.Count == 2)
		{
			VerifyTranslationEntry(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyImmediateTranslation && args.Count == 5)
		{
			VerifyImmediateTranslation(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Transform2D>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrimeCleanRenderTransform && args.Count == 1)
		{
			PrimeCleanRenderTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadRenderedTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ReadRenderedTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadRenderTransformDirty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadRenderTransformDirty(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
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
		if (method == MethodName.PrepareCharacter && args.Count == 3)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrimeCleanRenderTransform && args.Count == 1)
		{
			PrimeCleanRenderTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadRenderedTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ReadRenderedTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadRenderTransformDirty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadRenderTransformDirty(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
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
		if (method == MethodName.VerifyLogicalPositionEntry)
		{
			return true;
		}
		if (method == MethodName.VerifyPhysicsFramePositionEntry)
		{
			return true;
		}
		if (method == MethodName.VerifyTranslationEntry)
		{
			return true;
		}
		if (method == MethodName.VerifyImmediateTranslation)
		{
			return true;
		}
		if (method == MethodName.PrimeCleanRenderTransform)
		{
			return true;
		}
		if (method == MethodName.ReadRenderedTransform)
		{
			return true;
		}
		if (method == MethodName.ReadRenderTransformDirty)
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
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
