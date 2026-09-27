using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewChallengeBackupScaleRuntimeTest.cs")]
public class BugOverviewChallengeBackupScaleRuntimeTest : Node
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

	private const string BackupScenePath = "res://Asset/Anime/Character/Zombie/Challenge/Backup/Scene/TowerDefenseZombieBackup.tscn";

	private static readonly Vector2 AuthoredSpriteScale = new Vector2(0.8f, 0.8f);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		PackedScene scene = null;
		TowerDefenseZombieBackup backup = null;
		try
		{
			try
			{
				scene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Challenge/Backup/Scene/TowerDefenseZombieBackup.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(scene), "The real Challenge Backup scene must load.");
				backup = scene?.Instantiate<TowerDefenseZombieBackup>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(backup), "The real Challenge Backup scene must instantiate its production script.");
				if (!GodotObject.IsInstanceValid(backup))
				{
					goto end_IL_003e;
				}
				backup.inGame = false;
				backup.editorPreviewMode = true;
				AddChild(backup, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				backup.ProcessMode = ProcessModeEnum.Disabled;
				Check(backup.IsNodeReady() && GodotObject.IsInstanceValid(backup.sprite), "The real Challenge Backup animation root must be ready.");
				if (!GodotObject.IsInstanceValid(backup.sprite))
				{
					goto end_IL_003e;
				}
				Vector2 scale = backup.sprite.Scale;
				Transform2D globalTransform = backup.sprite.GlobalTransform;
				Check(scale.IsEqualApprox(AuthoredSpriteScale), $"Challenge Backup must begin at the authored uniform scale; got {scale}.");
				backup.DanceEntered();
				backup.DanceExited();
				Vector2 scale2 = backup.sprite.Scale;
				Transform2D globalTransform2 = backup.sprite.GlobalTransform;
				Check(scale2.IsEqualApprox(scale), $"Dance exit must preserve the authored local scale; before={scale}, after={scale2}.");
				Check(Mathf.IsEqualApprox(globalTransform.X.Length(), globalTransform2.X.Length()) && Mathf.IsEqualApprox(globalTransform.Y.Length(), globalTransform2.Y.Length()), $"Dance exit must preserve both rendered basis lengths; before=({globalTransform.X.Length()}, {globalTransform.Y.Length()}), after=({globalTransform2.X.Length()}, {globalTransform2.Y.Length()}).");
				goto end_IL_0035;
				end_IL_003e:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewChallengeBackupScaleRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0035;
			}
			return;
			end_IL_0035:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(backup) && !backup.IsQueuedForDeletion())
			{
				backup.QueueFree();
			}
			await WaitFrames(4);
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			scene?.Dispose();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag = _failures == 0 && _checks == 6;
		GD.Print($"CHALLENGE_BACKUP_SCALE_RESULT passed={flag} checks={_checks} failures={_failures}");
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
			GD.PushError("[BugOverviewChallengeBackupScaleRuntimeTest] " + message);
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
