using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentBossHeadHealthLayerRuntimeTest.cs")]
public class BugDepartmentBossHeadHealthLayerRuntimeTest : Node
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

	private const string BossScenePath = "res://Asset/Anime/Character/Zombie/Boss/Boss/Scene/TowerDefenseZombieBoss.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseZombieBoss boss = null;
		try
		{
			_ = 1;
			try
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Boss/Boss/Scene/TowerDefenseZombieBoss.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(packedScene), "The production Zomboss character scene must load.");
				if (!GodotObject.IsInstanceValid(packedScene))
				{
					goto end_IL_0041;
				}
				boss = packedScene.Instantiate<TowerDefenseZombieBoss>(PackedScene.GenEditState.Disabled);
				boss.Name = "RuntimeZomboss";
				boss.ProcessMode = ProcessModeEnum.Disabled;
				boss.inGame = true;
				boss.gridPos = new Vector2I(8, 3);
				AddChild(boss, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				Check(GodotObject.IsInstanceValid(boss) && boss.config?.name == "ZombieBoss" && GodotObject.IsInstanceValid(boss.sprite), "The regression must instantiate the corresponding real Zomboss object.");
				ShowHealthComponent showHealth = boss.showHealthComponent;
				Check(showHealth != null && !showHealth.IsReleased, "The real Zomboss must retain its production ShowHealth runtime.");
				Check(GodotObject.IsInstanceValid(boss.instance), "The real Zomboss must expose its production health instance.");
				if (showHealth == null || !GodotObject.IsInstanceValid(boss.instance))
				{
					goto end_IL_0041;
				}
				showHealth.SetAlive(alive: true);
				showHealth.MarkDirty();
				showHealth.BatchUpdate();
				Node2D anchor = boss.spriteGroup.GetNodeOrNull<Node2D>("ShowHealthViewAnchor");
				Check(GodotObject.IsInstanceValid(anchor), "The live Zomboss scene must create the production health-display anchor.");
				Check((showHealth.bodyHitpointLabel?.Visible ?? false) && !string.IsNullOrWhiteSpace(showHealth.bodyHitpointLabel.Text), "The live Zomboss body-health label must be visible before lowering its head.");
				Check((anchor?.TopLevel ?? false) && !anchor.ZAsRelative, "The Zomboss health anchor must use the screen-aligned absolute-Z path.");
				boss.sprite.pause = true;
				boss.HeadIdleProcessing(0.0);
				await WaitFrames(2);
				showHealth.BatchUpdate();
				int num = Math.Clamp(boss.ZIndex + showHealth.displayZIndex, -4096, 4096);
				Check(boss.ZIndex == 1000, $"The real head-idle state must place Zomboss at Z=1000; got {boss.ZIndex}.");
				Check(anchor != null && anchor.ZIndex == num, $"The health display must follow Zomboss above the lowered head; expected={num}, actual={anchor?.ZIndex}.");
				Check(anchor?.ZIndex > boss.ZIndex, $"The visible health label must render above the lowered Zomboss body; boss={boss.ZIndex}, health={anchor?.ZIndex}.");
				Check(showHealth.bodyHitpointLabel?.IsVisibleInTree() ?? false, "The Zomboss body-health label must remain visible in the real scene tree.");
				goto end_IL_002f;
				end_IL_0041:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugDepartmentBossHeadHealthLayerRuntimeTest] Unexpected exception: {value}");
				goto end_IL_002f;
			}
			return;
			end_IL_002f:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(boss))
			{
				boss.Free();
			}
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks >= 10;
		GD.Print($"BUG_DEPARTMENT_BOSS_HEAD_HEALTH_LAYER_RESULT passed={flag} checks={_checks} failures={_failures}");
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
			GD.PushError("[BugDepartmentBossHeadHealthLayerRuntimeTest] " + message);
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
