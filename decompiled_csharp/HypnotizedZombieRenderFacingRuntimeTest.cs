using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/HypnotizedZombieRenderFacingRuntimeTest.cs")]
public class HypnotizedZombieRenderFacingRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ReadRenderTransformDirty = "ReadRenderTransformDirty";

		public static readonly StringName ReadRenderedTransform = "ReadRenderedTransform";

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

	private const string NormalZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private readonly List<string> _failures = new List<string>();

	private int _checks;

	public override async void _Ready()
	{
		AdobeAnimateRenderBackend previousBackend = Global.Instance.adobeAnimateRenderBackend;
		TowerDefenseZombieNormal zombie = null;
		try
		{
			try
			{
				Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
				zombie = Instantiate<TowerDefenseZombieNormal>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
				Check(GodotObject.IsInstanceValid(zombie), "测试必须实例化真实普通僵尸场景。");
				if (!GodotObject.IsInstanceValid(zombie))
				{
					goto end_IL_0047;
				}
				zombie.inGame = false;
				zombie.editorPreviewMode = false;
				zombie.GlobalPosition = new Vector2(480f, 300f);
				AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				await WaitProcessFrames(4);
				Check(GodotObject.IsInstanceValid(zombie.sprite), "真实普通僵尸必须提供 Adobe Animate 身体根节点。");
				if (!GodotObject.IsInstanceValid(zombie.sprite))
				{
					goto end_IL_0047;
				}
				zombie.sprite.ClearRetainedRootMotionForStoppedMovement();
				Transform2D transform2D = ReadRenderedTransform(zombie.sprite);
				Check(zombie.Scale.X > 0f && !ReadRenderTransformDirty(zombie.sprite), "魅惑前必须是朝向左侧且渲染变换缓存干净的普通僵尸。");
				zombie.ProcessMode = ProcessModeEnum.Disabled;
				zombie.Hypnoses();
				Check(zombie.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT && zombie.instance.hypnoses, "魅惑必须立即把真实普通僵尸切换到植物阵营。");
				Check(zombie.Scale.X < 0f, "魅惑必须立即翻转真实普通僵尸的逻辑朝向。");
				Check(ReadRenderTransformDirty(zombie.sprite), "魅惑必须在普通轮询停用时同步标脏身体渲染变换。");
				AdobeAnimateRenderManager.RenderActive(new AdobeAnimateSprite[1] { zombie.sprite }, activeListAlreadyValidated: true, renderRootsAlreadyFiltered: true);
				Transform2D transform2D2 = ReadRenderedTransform(zombie.sprite);
				Check(transform2D2.IsEqualApprox(zombie.sprite.GlobalTransform), "魅惑后的下一次 Crowd 事务必须消费翻转后的身体变换。");
				Check(transform2D.Determinant() * transform2D2.Determinant() < 0f, "魅惑后的渲染变换必须与普通僵尸使用相反朝向。");
				zombie.BuffDelete("Hypnoses");
				Check(zombie.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE && !zombie.instance.hypnoses && zombie.Scale.X > 0f, "解除魅惑必须恢复真实普通僵尸的阵营和逻辑朝向。");
				Check(ReadRenderTransformDirty(zombie.sprite), "解除魅惑也必须同步标脏身体渲染变换。");
				AdobeAnimateRenderManager.RenderActive(new AdobeAnimateSprite[1] { zombie.sprite }, activeListAlreadyValidated: true, renderRootsAlreadyFiltered: true);
				Transform2D transform2D3 = ReadRenderedTransform(zombie.sprite);
				Check(transform2D3.IsEqualApprox(zombie.sprite.GlobalTransform), "解除魅惑后的下一次 Crowd 事务必须消费恢复后的身体变换。");
				Check(transform2D.Determinant() * transform2D3.Determinant() > 0f, "解除魅惑后的渲染朝向必须恢复为普通僵尸朝向。");
				goto end_IL_003e;
				end_IL_0047:;
			}
			catch (Exception value)
			{
				_failures.Add($"运行测试出现异常：{value}");
				goto end_IL_003e;
			}
			end_IL_003e:;
		}
		finally
		{
			Global.Instance.adobeAnimateRenderBackend = previousBackend;
			if (GodotObject.IsInstanceValid(zombie))
			{
				zombie.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			Finish();
		}
	}

	private static bool ReadRenderTransformDirty(AdobeAnimateSprite sprite)
	{
		return ReadPrivateField<bool>(sprite, "_renderGlobalTransformDirty");
	}

	private static Transform2D ReadRenderedTransform(AdobeAnimateSprite sprite)
	{
		return ReadPrivateField<Transform2D>(sprite, "_renderGlobalTransform");
	}

	private static T ReadPrivateField<T>(object owner, string fieldName)
	{
		FieldInfo? field = typeof(AdobeAnimateSprite).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
		if (field == null)
		{
			throw new MissingFieldException(typeof(AdobeAnimateSprite).FullName, fieldName);
		}
		return (T)field.GetValue(owner);
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
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
			GD.PushError("[HypnotizedZombieRenderFacing] " + failure);
		}
		bool flag = _failures.Count == 0 && _checks == 12;
		GD.Print($"HYPNOTIZED_ZOMBIE_RENDER_FACING_RESULT passed={flag} checks={_checks} failures={_failures.Count}");
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(5)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ReadRenderTransformDirty, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ReadRenderedTransform, new Godot.Bridge.PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Finish, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ReadRenderTransformDirty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadRenderTransformDirty(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadRenderedTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ReadRenderedTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
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
		if (method == MethodName.ReadRenderTransformDirty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadRenderTransformDirty(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadRenderedTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ReadRenderedTransform(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
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
		if (method == MethodName.ReadRenderTransformDirty)
		{
			return true;
		}
		if (method == MethodName.ReadRenderedTransform)
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
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
