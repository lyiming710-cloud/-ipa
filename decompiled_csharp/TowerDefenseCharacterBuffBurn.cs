using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Buff/TowerDefenseCharacterBuffBurn.cs")]
public class TowerDefenseCharacterBuffBurn : TowerDefenseCharacterBuffConfig
{
	public new class MethodName : TowerDefenseCharacterBuffConfig.MethodName
	{
		public new static readonly StringName _Init = "_Init";

		public new static readonly StringName Enter = "Enter";

		public new static readonly StringName Step = "Step";

		public new static readonly StringName StepReadOnlyClient = "StepReadOnlyClient";

		public static readonly StringName StepSplatVisual = "StepSplatVisual";

		public new static readonly StringName Exit = "Exit";

		public new static readonly StringName Refresh = "Refresh";
	}

	public new class PropertyName : TowerDefenseCharacterBuffConfig.PropertyName
	{
		public static readonly StringName time = "time";

		public static readonly StringName dpsAttack = "dpsAttack";

		public static readonly StringName splatSceneType = "splatSceneType";

		public static readonly StringName splatScene = "splatScene";

		public static readonly StringName splatInterval = "splatInterval";

		public static readonly StringName currentTime = "currentTime";

		public static readonly StringName splatTime = "splatTime";
	}

	public new class SignalName : TowerDefenseCharacterBuffConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double time = 3.0;

	[Export(PropertyHint.None, "")]
	public double dpsAttack = 100.0;

	[Export(PropertyHint.Enum, "Particles,Sprite")]
	public string splatSceneType = "Particles";

	[Export(PropertyHint.None, "")]
	public PackedScene splatScene;

	[Export(PropertyHint.None, "")]
	public double splatInterval = 1.0;

	[Export(PropertyHint.None, "")]
	public double currentTime;

	[Export(PropertyHint.None, "")]
	public double splatTime;

	public override void _Init()
	{
		key = "Burn";
	}

	public override void Enter()
	{
		splatTime = splatInterval;
	}

	public override bool Step(double delta)
	{
		if (character.IsDie())
		{
			return true;
		}
		currentTime += delta;
		if (StepSplatVisual(delta))
		{
			character.buff.ApplyFireHit();
		}
		character.FlagHurt(dpsAttack * delta, 6, playSplatAudio: false);
		if (!(currentTime > time))
		{
			return character.IsDie();
		}
		return true;
	}

	public override void StepReadOnlyClient(double delta)
	{
		if (!character.IsDie())
		{
			currentTime += delta;
			StepSplatVisual(delta);
		}
	}

	private bool StepSplatVisual(double delta)
	{
		if (splatScene == null)
		{
			return false;
		}
		splatTime += delta;
		if (splatTime <= splatInterval)
		{
			return false;
		}
		string text = splatSceneType;
		Node2D node2D;
		if (text == "Particles")
		{
			node2D = TowerDefenseManager.CreateEffectParticlesOnce(splatScene, character.gridPos);
		}
		else
		{
			node2D = ((!(text == "Sprite")) ? ((TowerDefenseEffectBase)TowerDefenseManager.CreateEffectParticlesOnce(splatScene, character.gridPos)) : ((TowerDefenseEffectBase)TowerDefenseManager.CreateEffectSpriteOnce(splatScene, character.gridPos)));
		}
		TowerDefenseManager.GetCharacterNode().AddChild(node2D, forceReadableName: false, Node.InternalMode.Disabled);
		node2D.GlobalPosition = character.GetLogicalGlobalPosition();
		splatTime = 0.0;
		return true;
	}

	public override void Exit()
	{
	}

	public override void Refresh(TowerDefenseCharacterBuffConfig config)
	{
		if (config is TowerDefenseCharacterBuffBurn towerDefenseCharacterBuffBurn)
		{
			time = Mathf.Max(time, towerDefenseCharacterBuffBurn.time);
			dpsAttack = towerDefenseCharacterBuffBurn.dpsAttack;
			splatSceneType = towerDefenseCharacterBuffBurn.splatSceneType;
			splatScene = towerDefenseCharacterBuffBurn.splatScene;
			splatInterval = towerDefenseCharacterBuffBurn.splatInterval;
			splatTime = Mathf.Min(splatTime, splatInterval);
			currentTime = 0.0;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Enter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Step, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StepReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StepSplatVisual, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Exit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Init && args.Count == 0)
		{
			_Init();
			ret = default;
			return true;
		}
		if (method == MethodName.Enter && args.Count == 0)
		{
			Enter();
			ret = default;
			return true;
		}
		if (method == MethodName.Step && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Step(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.StepReadOnlyClient && args.Count == 1)
		{
			StepReadOnlyClient(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StepSplatVisual && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(StepSplatVisual(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.Exit && args.Count == 0)
		{
			Exit();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 1)
		{
			Refresh(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Init)
		{
			return true;
		}
		if (method == MethodName.Enter)
		{
			return true;
		}
		if (method == MethodName.Step)
		{
			return true;
		}
		if (method == MethodName.StepReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.StepSplatVisual)
		{
			return true;
		}
		if (method == MethodName.Exit)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.time)
		{
			time = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dpsAttack)
		{
			dpsAttack = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.splatSceneType)
		{
			splatSceneType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.splatScene)
		{
			splatScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.splatInterval)
		{
			splatInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			currentTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.splatTime)
		{
			splatTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.time)
		{
			value = VariantUtils.CreateFrom(in time);
			return true;
		}
		if (name == PropertyName.dpsAttack)
		{
			value = VariantUtils.CreateFrom(in dpsAttack);
			return true;
		}
		if (name == PropertyName.splatSceneType)
		{
			value = VariantUtils.CreateFrom(in splatSceneType);
			return true;
		}
		if (name == PropertyName.splatScene)
		{
			value = VariantUtils.CreateFrom(in splatScene);
			return true;
		}
		if (name == PropertyName.splatInterval)
		{
			value = VariantUtils.CreateFrom(in splatInterval);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			value = VariantUtils.CreateFrom(in currentTime);
			return true;
		}
		if (name == PropertyName.splatTime)
		{
			value = VariantUtils.CreateFrom(in splatTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dpsAttack, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.splatSceneType, PropertyHint.Enum, "Particles,Sprite", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.splatScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.splatInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.splatTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.time, Variant.From(in time));
		info.AddProperty(PropertyName.dpsAttack, Variant.From(in dpsAttack));
		info.AddProperty(PropertyName.splatSceneType, Variant.From(in splatSceneType));
		info.AddProperty(PropertyName.splatScene, Variant.From(in splatScene));
		info.AddProperty(PropertyName.splatInterval, Variant.From(in splatInterval));
		info.AddProperty(PropertyName.currentTime, Variant.From(in currentTime));
		info.AddProperty(PropertyName.splatTime, Variant.From(in splatTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.time, out var value))
		{
			time = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dpsAttack, out var value2))
		{
			dpsAttack = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.splatSceneType, out var value3))
		{
			splatSceneType = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.splatScene, out var value4))
		{
			splatScene = value4.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.splatInterval, out var value5))
		{
			splatInterval = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.currentTime, out var value6))
		{
			currentTime = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.splatTime, out var value7))
		{
			splatTime = value7.As<double>();
		}
	}
}
