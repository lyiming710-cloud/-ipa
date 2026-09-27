using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/AppleGreen/Scene/TowerDefensePlantAppleGreen.cs")]
public class TowerDefensePlantAppleGreen : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName RunApple = "RunApple";

		public static readonly StringName ActivateEffect = "ActivateEffect";

		public static readonly StringName PauseEffect = "PauseEffect";

		public static readonly StringName ResumeEffect = "ResumeEffect";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName ReleaseEffectLease = "ReleaseEffectLease";

		public static readonly StringName ClearGlobalEffectIfUnowned = "ClearGlobalEffectIfUnowned";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName run = "run";

		public static readonly StringName runTimer = "runTimer";

		public static readonly StringName _effectActive = "_effectActive";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	public bool run;

	public double runTimer;

	private bool _effectActive;

	private const double RunDuration = 8.0;

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _ExitTree()
	{
		if (!Engine.IsEditorHint())
		{
			ReleaseEffectLease();
		}
		base._ExitTree();
	}

	public override void IdleEntered()
	{
		if (inGame && GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) && TowerDefenseManager.CurrentControl.isGameRunning && !characterDisabled)
		{
			if (run)
			{
				ResumeEffect();
			}
			else
			{
				RunApple();
			}
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || !inGame || !GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) || !TowerDefenseManager.CurrentControl.isGameRunning || !run)
		{
			return;
		}
		if (characterDisabled)
		{
			if (_effectActive)
			{
				PauseEffect();
			}
		}
		else if (!_effectActive)
		{
			ResumeEffect();
		}
		runTimer -= delta;
		if (runTimer <= 0.0)
		{
			Destroy();
		}
	}

	public void RunApple()
	{
		AudioManager.Instance.AudioPlay("Apple");
		sprite.SetAnimation("Running", loop: true, 0.2);
		instance.invincible = true;
		run = true;
		runTimer = 8.0;
		ActivateEffect();
	}

	private void ActivateEffect()
	{
		AddToGroup("AppleBack");
		_effectActive = true;
		TowerDefenseManager.Instance.backPacket = true;
		TowerDefenseManager.Instance.backZombie = true;
	}

	private void PauseEffect()
	{
		ReleaseEffectLease();
		sprite.pause = true;
	}

	private void ResumeEffect()
	{
		AddToGroup("AppleBack");
		_effectActive = true;
		TowerDefenseManager.Instance.backPacket = true;
		TowerDefenseManager.Instance.backZombie = true;
		sprite.pause = false;
	}

	public override async void DestroySet()
	{
		base.DestroySet();
		ReleaseEffectLease(clearGlobalEffect: false);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		ClearGlobalEffectIfUnowned();
	}

	private void ReleaseEffectLease(bool clearGlobalEffect = true)
	{
		if (_effectActive || run)
		{
			_effectActive = false;
			RemoveFromGroup("AppleBack");
			if (clearGlobalEffect)
			{
				ClearGlobalEffectIfUnowned();
			}
		}
	}

	private void ClearGlobalEffectIfUnowned()
	{
		if (IsInsideTree() && GetTree().GetNodeCountInGroup("AppleBack") <= 0 && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			TowerDefenseManager.Instance.backPacket = false;
			TowerDefenseManager.Instance.backZombie = false;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "run", run },
			{ "runTimer", runTimer }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		run = data.GetValueOrDefault("run", Variant.From<bool>(false)).AsBool();
		runTimer = data.GetValueOrDefault("runTimer", Variant.From<double>(0.0)).AsDouble();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunApple, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ActivateEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PauseEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResumeEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseEffectLease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "clearGlobalEffect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearGlobalEffectIfUnowned, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunApple && args.Count == 0)
		{
			RunApple();
			ret = default;
			return true;
		}
		if (method == MethodName.ActivateEffect && args.Count == 0)
		{
			ActivateEffect();
			ret = default;
			return true;
		}
		if (method == MethodName.PauseEffect && args.Count == 0)
		{
			PauseEffect();
			ret = default;
			return true;
		}
		if (method == MethodName.ResumeEffect && args.Count == 0)
		{
			ResumeEffect();
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseEffectLease && args.Count == 1)
		{
			ReleaseEffectLease(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearGlobalEffectIfUnowned && args.Count == 0)
		{
			ClearGlobalEffectIfUnowned();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.RunApple)
		{
			return true;
		}
		if (method == MethodName.ActivateEffect)
		{
			return true;
		}
		if (method == MethodName.PauseEffect)
		{
			return true;
		}
		if (method == MethodName.ResumeEffect)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.ReleaseEffectLease)
		{
			return true;
		}
		if (method == MethodName.ClearGlobalEffectIfUnowned)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.run)
		{
			run = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.runTimer)
		{
			runTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._effectActive)
		{
			_effectActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.run)
		{
			value = VariantUtils.CreateFrom(in run);
			return true;
		}
		if (name == PropertyName.runTimer)
		{
			value = VariantUtils.CreateFrom(in runTimer);
			return true;
		}
		if (name == PropertyName._effectActive)
		{
			value = VariantUtils.CreateFrom(in _effectActive);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.run, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.runTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._effectActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.run, Variant.From(in run));
		info.AddProperty(PropertyName.runTimer, Variant.From(in runTimer));
		info.AddProperty(PropertyName._effectActive, Variant.From(in _effectActive));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.run, out var value))
		{
			run = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.runTimer, out var value2))
		{
			runTimer = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._effectActive, out var value3))
		{
			_effectActive = value3.As<bool>();
		}
	}
}
