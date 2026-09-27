using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Item/GatlingTX/Scene/TowerDefenseItemGatlingTX.cs")]
public class TowerDefenseItemGatlingTX : TowerDefenseItem
{
	private sealed class PendingFireSequence
	{
		public long EventSequence;

		public int NextWave;

		public double DelayRemaining;

		public SceneTreeTimer DelayTimer;

		public int LifecycleVersion;

		public bool Running;
	}

	public new class MethodName : TowerDefenseItem.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName EmitFireWave = "EmitFireWave";

		public static readonly StringName CanContinueFireSequence = "CanContinueFireSequence";

		public static readonly StringName CancelPendingFireSequences = "CancelPendingFireSequences";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";

		public static readonly StringName StartRestoredPendingFireSequences = "StartRestoredPendingFireSequences";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";
	}

	public new class PropertyName : TowerDefenseItem.PropertyName
	{
		public static readonly StringName _fireEventSequence = "_fireEventSequence";

		public static readonly StringName _fireLifecycleVersion = "_fireLifecycleVersion";

		public static readonly StringName projectileName = "projectileName";
	}

	public new class SignalName : TowerDefenseItem.SignalName
	{
	}

	private const int FireWaveCount = 3;

	private const int ProjectilesPerWave = 5;

	private const double FireWaveDelay = 0.05;

	private FireComponent fireComponent;

	private readonly List<PendingFireSequence> _pendingFireSequences = new List<PendingFireSequence>();

	private long _fireEventSequence;

	private int _fireLifecycleVersion;

	[Export(PropertyHint.None, "")]
	public string projectileName = "Pea";

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			HitBoxDestroy();
			sprite.SetAnimation("Fire", loop: false);
		}
	}

	public override void _ExitTree()
	{
		CancelPendingFireSequences();
		base._ExitTree();
	}

	public override void IdleProcessing(double delta)
	{
		sprite.timeScale = timeScale * 0.5;
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (!(command != "fire") && CanContinueFireSequence())
		{
			PendingFireSequence pendingFireSequence = new PendingFireSequence
			{
				EventSequence = ++_fireEventSequence,
				NextWave = 0,
				DelayRemaining = 0.0
			};
			_pendingFireSequences.Add(pendingFireSequence);
			StartPendingFireSequence(pendingFireSequence);
		}
	}

	private void StartPendingFireSequence(PendingFireSequence context)
	{
		if (context != null && !context.Running && context.NextWave < 3)
		{
			context.Running = true;
			context.LifecycleVersion = _fireLifecycleVersion;
			if (context.DelayRemaining > 0.0)
			{
				ResumePendingFireSequenceAfterDelay(context);
			}
			else
			{
				RunPendingFireSequence(context);
			}
		}
	}

	private async void ResumePendingFireSequenceAfterDelay(PendingFireSequence context)
	{
		if (!CanContinueFireSequence(context.LifecycleVersion))
		{
			RemovePendingFireSequence(context);
			return;
		}
		SceneTree tree = GetTree();
		if (!GodotObject.IsInstanceValid(tree))
		{
			RemovePendingFireSequence(context);
			return;
		}
		context.DelayTimer = tree.CreateTimer(context.DelayRemaining);
		await ToSignal(context.DelayTimer, SceneTreeTimer.SignalName.Timeout);
		context.DelayTimer = null;
		context.DelayRemaining = 0.0;
		if (!CanContinueFireSequence(context.LifecycleVersion))
		{
			RemovePendingFireSequence(context);
		}
		else
		{
			RunPendingFireSequence(context);
		}
	}

	private async void RunPendingFireSequence(PendingFireSequence context)
	{
		while (context.NextWave < 3)
		{
			if (!CanContinueFireSequence(context.LifecycleVersion))
			{
				RemovePendingFireSequence(context);
				return;
			}
			int nextWave = context.NextWave;
			EmitFireWave(context.EventSequence, nextWave);
			context.NextWave = nextWave + 1;
			if (context.NextWave >= 3)
			{
				break;
			}
			context.DelayRemaining = 0.05;
			SceneTree tree = GetTree();
			if (!GodotObject.IsInstanceValid(tree))
			{
				RemovePendingFireSequence(context);
				return;
			}
			context.DelayTimer = tree.CreateTimer(context.DelayRemaining);
			await ToSignal(context.DelayTimer, SceneTreeTimer.SignalName.Timeout);
			context.DelayTimer = null;
			context.DelayRemaining = 0.0;
		}
		RemovePendingFireSequence(context);
	}

	private void EmitFireWave(long eventSequence, int waveIndex)
	{
		if (!CanContinueFireSequence())
		{
			return;
		}
		using RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator
		{
			Seed = NetworkDeterministicSeed.ForCharacterEvent(syncId, eventSequence, 111624470uL)
		};
		int num = waveIndex * 5;
		for (int i = 0; i < num; i++)
		{
			randomNumberGenerator.RandfRange(300f, 800f);
			randomNumberGenerator.RandfRange(-30f, 20f);
			randomNumberGenerator.RandfRange(-30f, 20f);
		}
		float num2 = (instance.hypnoses ? (-1f) : 1f);
		bool flag = Global.IsMultiplayerMode && !MultiPlayerManager.IsHost;
		for (int j = 0; j < 5; j++)
		{
			AudioManager.Instance.AudioPlay("ProjectileThrow");
			Vector2 velocity = new Vector2(randomNumberGenerator.RandfRange(300f, 800f) * num2, randomNumberGenerator.RandfRange(-30f, 20f));
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				gridYOverride = gridPos.Y,
				flipXOverride = (num2 < 0f),
				suppressGameplay = flag
			};
			fireComponent.CreateProjectileByData(0, velocity, fireComponent.fireCheckList[0].projectile.GetProjetile(), (!flag) ? (-1) : 0, camp, new Vector2(0f, randomNumberGenerator.RandfRange(-30f, 20f)), overrides);
		}
	}

	private bool CanContinueFireSequence()
	{
		if (inGame && !editorPreviewMode && IsInsideTree() && !isDestroy && GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl))
		{
			FireComponent fireComponent = this.fireComponent;
			if (fireComponent != null && fireComponent.Lifecycle == ComponentRuntimeLifecycle.Active)
			{
				return this.fireComponent.parent == this;
			}
		}
		return false;
	}

	private bool CanContinueFireSequence(int lifecycleVersion)
	{
		if (lifecycleVersion == _fireLifecycleVersion)
		{
			return CanContinueFireSequence();
		}
		return false;
	}

	private void RemovePendingFireSequence(PendingFireSequence context)
	{
		context.Running = false;
		context.DelayTimer = null;
		context.DelayRemaining = 0.0;
		_pendingFireSequences.Remove(context);
	}

	private double GetRemainingDelay(PendingFireSequence context)
	{
		if (context.DelayTimer != null && GodotObject.IsInstanceValid(context.DelayTimer))
		{
			return Math.Max(0.0, context.DelayTimer.TimeLeft);
		}
		return Math.Max(0.0, context.DelayRemaining);
	}

	private void CancelPendingFireSequences()
	{
		_fireLifecycleVersion++;
		foreach (PendingFireSequence pendingFireSequence in _pendingFireSequences)
		{
			pendingFireSequence.Running = false;
			pendingFireSequence.DelayTimer = null;
		}
		_pendingFireSequences.Clear();
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		Array<Dictionary> array = new Array<Dictionary>();
		foreach (PendingFireSequence pendingFireSequence in _pendingFireSequences)
		{
			if (pendingFireSequence.NextWave >= 1 && pendingFireSequence.NextWave < 3)
			{
				array.Add(new Dictionary
				{
					["eventSequence"] = pendingFireSequence.EventSequence,
					["nextWave"] = pendingFireSequence.NextWave,
					["delayRemaining"] = GetRemainingDelay(pendingFireSequence)
				});
			}
		}
		dictionary["fireEventSequence"] = _fireEventSequence;
		dictionary["pendingFireSequences"] = array;
		return dictionary;
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		CancelPendingFireSequences();
		_fireEventSequence = Math.Max(0L, data.GetValueOrDefault("fireEventSequence", 0L).AsInt64());
		foreach (Variant item in data.GetValueOrDefault("pendingFireSequences", new Godot.Collections.Array()).AsGodotArray())
		{
			Dictionary dictionary = item.AsGodotDictionary();
			long num = dictionary.GetValueOrDefault("eventSequence", 0L).AsInt64();
			int num2 = dictionary.GetValueOrDefault("nextWave", 0).AsInt32();
			if (num > 0 && num2 >= 1 && num2 < 3)
			{
				_fireEventSequence = Math.Max(_fireEventSequence, num);
				_pendingFireSequences.Add(new PendingFireSequence
				{
					EventSequence = num,
					NextWave = num2,
					DelayRemaining = Math.Max(0.0, dictionary.GetValueOrDefault("delayRemaining", 0.0).AsDouble())
				});
			}
		}
	}

	public override void FinalizeProgressRestore()
	{
		base.FinalizeProgressRestore();
		CallDeferred("StartRestoredPendingFireSequences");
	}

	private void StartRestoredPendingFireSequences()
	{
		PendingFireSequence[] array = _pendingFireSequences.ToArray();
		foreach (PendingFireSequence context in array)
		{
			StartPendingFireSequence(context);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		if (clip == "Fire")
		{
			Destroy();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitFireWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "eventSequence", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "waveIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanContinueFireSequence, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanContinueFireSequence, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "lifecycleVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelPendingFireSequences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartRestoredPendingFireSequences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitFireWave && args.Count == 2)
		{
			EmitFireWave(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanContinueFireSequence && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanContinueFireSequence());
			return true;
		}
		if (method == MethodName.CanContinueFireSequence && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanContinueFireSequence(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CancelPendingFireSequences && args.Count == 0)
		{
			CancelPendingFireSequences();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportVariantSaveWhenEmpty());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinalizeProgressRestore && args.Count == 0)
		{
			FinalizeProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.StartRestoredPendingFireSequences && args.Count == 0)
		{
			StartRestoredPendingFireSequences();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.EmitFireWave)
		{
			return true;
		}
		if (method == MethodName.CanContinueFireSequence)
		{
			return true;
		}
		if (method == MethodName.CancelPendingFireSequences)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.FinalizeProgressRestore)
		{
			return true;
		}
		if (method == MethodName.StartRestoredPendingFireSequences)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._fireEventSequence)
		{
			_fireEventSequence = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._fireLifecycleVersion)
		{
			_fireLifecycleVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._fireEventSequence)
		{
			value = VariantUtils.CreateFrom(in _fireEventSequence);
			return true;
		}
		if (name == PropertyName._fireLifecycleVersion)
		{
			value = VariantUtils.CreateFrom(in _fireLifecycleVersion);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom(in projectileName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._fireEventSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._fireLifecycleVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._fireEventSequence, Variant.From(in _fireEventSequence));
		info.AddProperty(PropertyName._fireLifecycleVersion, Variant.From(in _fireLifecycleVersion));
		info.AddProperty(PropertyName.projectileName, Variant.From(in projectileName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._fireEventSequence, out var value))
		{
			_fireEventSequence = value.As<long>();
		}
		if (info.TryGetProperty(PropertyName._fireLifecycleVersion, out var value2))
		{
			_fireLifecycleVersion = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projectileName, out var value3))
		{
			projectileName = value3.As<string>();
		}
	}
}
