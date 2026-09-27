using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Buff/TowerDefenseCharacterBuffFluorescence.cs")]
public class TowerDefenseCharacterBuffFluorescence : TowerDefenseCharacterBuffConfig
{
	public new class MethodName : TowerDefenseCharacterBuffConfig.MethodName
	{
		public new static readonly StringName _Init = "_Init";

		public new static readonly StringName Enter = "Enter";

		public new static readonly StringName EnterReadOnlyClient = "EnterReadOnlyClient";

		public new static readonly StringName Step = "Step";

		public new static readonly StringName StepReadOnlyClient = "StepReadOnlyClient";

		public new static readonly StringName Exit = "Exit";

		public new static readonly StringName ExitReadOnlyClient = "ExitReadOnlyClient";

		public new static readonly StringName Cancel = "Cancel";

		public new static readonly StringName Refresh = "Refresh";

		public static readonly StringName ShowVisuals = "ShowVisuals";

		public static readonly StringName HideVisuals = "HideVisuals";

		public static readonly StringName ApplyGameplayState = "ApplyGameplayState";

		public static readonly StringName RestoreGameplayState = "RestoreGameplayState";
	}

	public new class PropertyName : TowerDefenseCharacterBuffConfig.PropertyName
	{
		public new static readonly StringName FrameMeshColorMultiplier = "FrameMeshColorMultiplier";

		public static readonly StringName time = "time";

		public static readonly StringName currentTime = "currentTime";

		public static readonly StringName fog = "fog";

		public static readonly StringName light = "light";

		public static readonly StringName _gameplayStateApplied = "_gameplayStateApplied";

		public static readonly StringName _originalCanBeCollection = "_originalCanBeCollection";

		public static readonly StringName _originalHadLightPhysique = "_originalHadLightPhysique";
	}

	public new class SignalName : TowerDefenseCharacterBuffConfig.SignalName
	{
	}

	private static readonly Color FLUORESCENCE_COLOR = new Color(1f, 1f, 1f, 0.5f);

	private static PackedScene _LIGHT_AREA;

	private static PackedScene _FLUORESCENCE_FOG;

	[Export(PropertyHint.None, "")]
	public double time = 50.0;

	[Export(PropertyHint.None, "")]
	public double currentTime;

	public Node fog;

	public Node light;

	private bool _gameplayStateApplied;

	private bool _originalCanBeCollection;

	private bool _originalHadLightPhysique;

	public override Color FrameMeshColorMultiplier => FLUORESCENCE_COLOR;

	private static PackedScene LIGHT_AREA => _LIGHT_AREA ?? (_LIGHT_AREA = GD.Load<PackedScene>("uid://byee3s263f1rj"));

	private static PackedScene FLUORESCENCE_FOG => _FLUORESCENCE_FOG ?? (_FLUORESCENCE_FOG = GD.Load<PackedScene>("uid://bmmugje73kf7e"));

	public override void _Init()
	{
		key = "Fluorescence";
	}

	public override void Enter()
	{
		ShowVisuals();
		ApplyGameplayState();
	}

	public override void EnterReadOnlyClient()
	{
		ShowVisuals();
	}

	public override bool Step(double delta)
	{
		currentTime += delta;
		return currentTime >= time;
	}

	public override void StepReadOnlyClient(double delta)
	{
		Step(delta);
	}

	public override void Exit()
	{
		RestoreGameplayState();
		HideVisuals();
	}

	public override void ExitReadOnlyClient()
	{
		HideVisuals();
	}

	public override void Cancel()
	{
		RestoreGameplayState();
		HideVisuals();
	}

	public override void Refresh(TowerDefenseCharacterBuffConfig config)
	{
		time = Mathf.Max(time, ((TowerDefenseCharacterBuffFluorescence)config).time + GD.RandRange(-0.2, 0.2));
		currentTime = 0.0;
	}

	private void ShowVisuals()
	{
		if (GodotObject.IsInstanceValid(character) && GodotObject.IsInstanceValid(character.spriteGroup))
		{
			if (!GodotObject.IsInstanceValid(fog))
			{
				fog = FLUORESCENCE_FOG.Instantiate(PackedScene.GenEditState.Disabled);
				character.spriteGroup.AddChild(fog, forceReadableName: false, Node.InternalMode.Disabled);
			}
			if (!GodotObject.IsInstanceValid(light))
			{
				light = LIGHT_AREA.Instantiate(PackedScene.GenEditState.Disabled);
				character.spriteGroup.AddChild(light, forceReadableName: false, Node.InternalMode.Disabled);
			}
		}
	}

	private void HideVisuals()
	{
		Node node = fog;
		fog = null;
		if (GodotObject.IsInstanceValid(node))
		{
			node.QueueFree();
		}
		Node node2 = light;
		light = null;
		if (GodotObject.IsInstanceValid(node2))
		{
			node2.QueueFree();
		}
	}

	private void ApplyGameplayState()
	{
		if (!_gameplayStateApplied && GodotObject.IsInstanceValid(character) && GodotObject.IsInstanceValid(character.instance))
		{
			int num = 64;
			_originalCanBeCollection = character.instance.canBeCollection;
			_originalHadLightPhysique = (character.instance.physiqueTypeFlags & num) != 0;
			_gameplayStateApplied = true;
			character.instance.canBeCollection = false;
			character.instance.physiqueTypeFlags |= num;
		}
	}

	private void RestoreGameplayState()
	{
		if (!_gameplayStateApplied)
		{
			return;
		}
		_gameplayStateApplied = false;
		if (GodotObject.IsInstanceValid(character) && GodotObject.IsInstanceValid(character.instance))
		{
			int num = 64;
			character.instance.canBeCollection = _originalCanBeCollection;
			if (_originalHadLightPhysique)
			{
				character.instance.physiqueTypeFlags |= num;
			}
			else
			{
				character.instance.physiqueTypeFlags &= ~num;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Enter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnterReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Step, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StepReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Exit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExitReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Cancel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyGameplayState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreGameplayState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.EnterReadOnlyClient && args.Count == 0)
		{
			EnterReadOnlyClient();
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
		if (method == MethodName.Exit && args.Count == 0)
		{
			Exit();
			ret = default;
			return true;
		}
		if (method == MethodName.ExitReadOnlyClient && args.Count == 0)
		{
			ExitReadOnlyClient();
			ret = default;
			return true;
		}
		if (method == MethodName.Cancel && args.Count == 0)
		{
			Cancel();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 1)
		{
			Refresh(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowVisuals && args.Count == 0)
		{
			ShowVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.HideVisuals && args.Count == 0)
		{
			HideVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyGameplayState && args.Count == 0)
		{
			ApplyGameplayState();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreGameplayState && args.Count == 0)
		{
			RestoreGameplayState();
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
		if (method == MethodName.EnterReadOnlyClient)
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
		if (method == MethodName.Exit)
		{
			return true;
		}
		if (method == MethodName.ExitReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.Cancel)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.ShowVisuals)
		{
			return true;
		}
		if (method == MethodName.HideVisuals)
		{
			return true;
		}
		if (method == MethodName.ApplyGameplayState)
		{
			return true;
		}
		if (method == MethodName.RestoreGameplayState)
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
		if (name == PropertyName.currentTime)
		{
			currentTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fog)
		{
			fog = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName.light)
		{
			light = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._gameplayStateApplied)
		{
			_gameplayStateApplied = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._originalCanBeCollection)
		{
			_originalCanBeCollection = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._originalHadLightPhysique)
		{
			_originalHadLightPhysique = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.FrameMeshColorMultiplier)
		{
			value = VariantUtils.CreateFrom<Color>(FrameMeshColorMultiplier);
			return true;
		}
		if (name == PropertyName.time)
		{
			value = VariantUtils.CreateFrom(in time);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			value = VariantUtils.CreateFrom(in currentTime);
			return true;
		}
		if (name == PropertyName.fog)
		{
			value = VariantUtils.CreateFrom(in fog);
			return true;
		}
		if (name == PropertyName.light)
		{
			value = VariantUtils.CreateFrom(in light);
			return true;
		}
		if (name == PropertyName._gameplayStateApplied)
		{
			value = VariantUtils.CreateFrom(in _gameplayStateApplied);
			return true;
		}
		if (name == PropertyName._originalCanBeCollection)
		{
			value = VariantUtils.CreateFrom(in _originalCanBeCollection);
			return true;
		}
		if (name == PropertyName._originalHadLightPhysique)
		{
			value = VariantUtils.CreateFrom(in _originalHadLightPhysique);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Color, PropertyName.FrameMeshColorMultiplier, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.fog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.light, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._gameplayStateApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._originalCanBeCollection, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._originalHadLightPhysique, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.time, Variant.From(in time));
		info.AddProperty(PropertyName.currentTime, Variant.From(in currentTime));
		info.AddProperty(PropertyName.fog, Variant.From(in fog));
		info.AddProperty(PropertyName.light, Variant.From(in light));
		info.AddProperty(PropertyName._gameplayStateApplied, Variant.From(in _gameplayStateApplied));
		info.AddProperty(PropertyName._originalCanBeCollection, Variant.From(in _originalCanBeCollection));
		info.AddProperty(PropertyName._originalHadLightPhysique, Variant.From(in _originalHadLightPhysique));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.time, out var value))
		{
			time = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.currentTime, out var value2))
		{
			currentTime = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fog, out var value3))
		{
			fog = value3.As<Node>();
		}
		if (info.TryGetProperty(PropertyName.light, out var value4))
		{
			light = value4.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._gameplayStateApplied, out var value5))
		{
			_gameplayStateApplied = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._originalCanBeCollection, out var value6))
		{
			_originalCanBeCollection = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._originalHadLightPhysique, out var value7))
		{
			_originalHadLightPhysique = value7.As<bool>();
		}
	}
}
