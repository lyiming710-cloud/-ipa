using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/BattleOption/BattleOption.cs")]
public class BattleOption : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PlantHealthCheckBoxToggled = "PlantHealthCheckBoxToggled";

		public static readonly StringName ZombieHealthCheckBoxToggled = "ZombieHealthCheckBoxToggled";

		public static readonly StringName BossHealthBarCheckBoxToggled = "BossHealthBarCheckBoxToggled";

		public static readonly StringName PacketUIFrontCheckBoxToggled = "PacketUIFrontCheckBoxToggled";

		public static readonly StringName MapEffectCheckBoxToggled = "MapEffectCheckBoxToggled";

		public static readonly StringName BackgrounderCheckBoxToggled = "BackgrounderCheckBoxToggled";

		public static readonly StringName PhonkCheckBoxToggled = "PhonkCheckBoxToggled";

		public static readonly StringName PhonkIntensitySliderChanged = "PhonkIntensitySliderChanged";

		public static readonly StringName BackButtonPressed = "BackButtonPressed";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName plantHealthCheckBox = "plantHealthCheckBox";

		public static readonly StringName zombieHealthCheckBox = "zombieHealthCheckBox";

		public static readonly StringName bossHealthBarCheckBox = "bossHealthBarCheckBox";

		public static readonly StringName packetUIFrontCheckBox = "packetUIFrontCheckBox";

		public static readonly StringName mapEffectCheckBox = "mapEffectCheckBox";

		public static readonly StringName backgrounderCheckBox = "backgrounderCheckBox";

		public static readonly StringName phonkCheckBox = "phonkCheckBox";

		public static readonly StringName phonkIntensityLabel = "phonkIntensityLabel";

		public static readonly StringName phonkIntensitySlider = "phonkIntensitySlider";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	public CheckBox plantHealthCheckBox;

	public CheckBox zombieHealthCheckBox;

	public CheckBox bossHealthBarCheckBox;

	public CheckBox packetUIFrontCheckBox;

	public CheckBox mapEffectCheckBox;

	public CheckBox backgrounderCheckBox;

	public CheckBox phonkCheckBox;

	public Label phonkIntensityLabel;

	public HSlider phonkIntensitySlider;

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		base._Ready();
		plantHealthCheckBox = GetNode<CheckBox>("%PlantHealthCheckBox");
		zombieHealthCheckBox = GetNode<CheckBox>("%ZombieHealthCheckBox");
		bossHealthBarCheckBox = GetNode<CheckBox>("%BossHealthBarCheckBox");
		packetUIFrontCheckBox = GetNode<CheckBox>("%PacketUIFrontCheckBox");
		mapEffectCheckBox = GetNode<CheckBox>("%MapEffectCheckBox");
		backgrounderCheckBox = GetNode<CheckBox>("%BackgrounderCheckBox");
		phonkCheckBox = GetNode<CheckBox>("%PhonkCheckBox");
		phonkIntensityLabel = GetNode<Label>("%PhonkIntensityLabel");
		phonkIntensitySlider = GetNode<HSlider>("%PhonkIntensitySlider");
		plantHealthCheckBox.ButtonPressed = GameSaveManager.Instance.GetConfigValue("ShowPlantHealth").AsBool();
		zombieHealthCheckBox.ButtonPressed = GameSaveManager.Instance.GetConfigValue("ShowZombieHealth").AsBool();
		bossHealthBarCheckBox.ButtonPressed = GameSaveManager.Instance.GetConfigValue("ShowBossHealthBar").AsBool();
		packetUIFrontCheckBox.ButtonPressed = GameSaveManager.Instance.GetConfigValue("PacketUIFront").AsBool();
		mapEffectCheckBox.ButtonPressed = GameSaveManager.Instance.GetConfigValue("MapEffect").AsBool();
		backgrounderCheckBox.ButtonPressed = GameSaveManager.Instance.GetConfigValue("Backgrounder").AsBool();
		phonkCheckBox.ButtonPressed = GameSaveManager.Instance.GetConfigValue("PhonkEnabled").AsBool();
		phonkIntensitySlider.Value = GameSaveManager.Instance.GetConfigValue("PhonkIntensity").AsDouble();
		phonkIntensityLabel.Text = $"果冻弹性强度:{phonkIntensitySlider.Value:F1}";
		PhonkComponent.phonkEnabled = phonkCheckBox.ButtonPressed;
		PhonkComponent.phonkIntensity = (float)phonkIntensitySlider.Value;
		plantHealthCheckBox.Toggled += PlantHealthCheckBoxToggled;
		zombieHealthCheckBox.Toggled += ZombieHealthCheckBoxToggled;
		bossHealthBarCheckBox.Toggled += BossHealthBarCheckBoxToggled;
		packetUIFrontCheckBox.Toggled += PacketUIFrontCheckBoxToggled;
		mapEffectCheckBox.Toggled += MapEffectCheckBoxToggled;
		backgrounderCheckBox.Toggled += BackgrounderCheckBoxToggled;
		phonkCheckBox.Toggled += PhonkCheckBoxToggled;
		phonkIntensitySlider.ValueChanged += PhonkIntensitySliderChanged;
		GetNode<NinePatchButtonBase>("CenterContainer/VBoxContainer/BackButton").OnPressed += BackButtonPressed;
		if (Global.IsMultiplayerMode)
		{
			pasue = false;
			if (GetTree().Paused)
			{
				GetTree().Paused = false;
			}
		}
	}

	public void PlantHealthCheckBoxToggled(bool toggledOn)
	{
		GameSaveManager.Instance.SetConfigValue("ShowPlantHealth", toggledOn);
		BattleEventBus.Instance.EmitShowPlantHealth(toggledOn);
		GameSaveManager.Instance.SaveGameConfig();
	}

	public void ZombieHealthCheckBoxToggled(bool toggledOn)
	{
		GameSaveManager.Instance.SetConfigValue("ShowZombieHealth", toggledOn);
		BattleEventBus.Instance.EmitShowZombieHealth(toggledOn);
		GameSaveManager.Instance.SaveGameConfig();
	}

	public void BossHealthBarCheckBoxToggled(bool toggledOn)
	{
		GameSaveManager.Instance.SetConfigValue("ShowBossHealthBar", toggledOn);
		BattleEventBus.Instance.EmitShowBossHealthBar(toggledOn);
		GameSaveManager.Instance.SaveGameConfig();
	}

	public void PacketUIFrontCheckBoxToggled(bool toggledOn)
	{
		GameSaveManager.Instance.SetConfigValue("PacketUIFront", toggledOn);
		BattleEventBus.Instance.EmitPacketUIFront(toggledOn);
		GameSaveManager.Instance.SaveGameConfig();
	}

	public void MapEffectCheckBoxToggled(bool toggledOn)
	{
		GameSaveManager.Instance.SetConfigValue("MapEffect", toggledOn);
		GameSaveManager.Instance.SaveGameConfig();
	}

	public void BackgrounderCheckBoxToggled(bool toggledOn)
	{
		GameSaveManager.Instance.SetConfigValue("Backgrounder", toggledOn);
		GameSaveManager.Instance.SaveGameConfig();
	}

	public void PhonkCheckBoxToggled(bool toggledOn)
	{
		GameSaveManager.Instance.SetConfigValue("PhonkEnabled", toggledOn);
		PhonkComponent.phonkEnabled = toggledOn;
		if (toggledOn)
		{
			PhonkComponent.InjectAll();
		}
		else
		{
			PhonkComponent.RemoveAll();
		}
		GameSaveManager.Instance.SaveGameConfig();
	}

	public void PhonkIntensitySliderChanged(double value)
	{
		GameSaveManager.Instance.SetConfigValue("PhonkIntensity", value);
		PhonkComponent.phonkIntensity = (float)value;
		phonkIntensityLabel.Text = $"果冻弹性强度:{value:F1}";
		GameSaveManager.Instance.SaveGameConfig();
	}

	public void BackButtonPressed()
	{
		CloseDialog();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlantHealthCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ZombieHealthCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BossHealthBarCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PacketUIFrontCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MapEffectCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BackgrounderCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PhonkCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PhonkIntensitySliderChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BackButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.PlantHealthCheckBoxToggled && args.Count == 1)
		{
			PlantHealthCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ZombieHealthCheckBoxToggled && args.Count == 1)
		{
			ZombieHealthCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BossHealthBarCheckBoxToggled && args.Count == 1)
		{
			BossHealthBarCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PacketUIFrontCheckBoxToggled && args.Count == 1)
		{
			PacketUIFrontCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MapEffectCheckBoxToggled && args.Count == 1)
		{
			MapEffectCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BackgrounderCheckBoxToggled && args.Count == 1)
		{
			BackgrounderCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PhonkCheckBoxToggled && args.Count == 1)
		{
			PhonkCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PhonkIntensitySliderChanged && args.Count == 1)
		{
			PhonkIntensitySliderChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BackButtonPressed && args.Count == 0)
		{
			BackButtonPressed();
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
		if (method == MethodName.PlantHealthCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.ZombieHealthCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.BossHealthBarCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.PacketUIFrontCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.MapEffectCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.BackgrounderCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.PhonkCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.PhonkIntensitySliderChanged)
		{
			return true;
		}
		if (method == MethodName.BackButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.plantHealthCheckBox)
		{
			plantHealthCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.zombieHealthCheckBox)
		{
			zombieHealthCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.bossHealthBarCheckBox)
		{
			bossHealthBarCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.packetUIFrontCheckBox)
		{
			packetUIFrontCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.mapEffectCheckBox)
		{
			mapEffectCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.backgrounderCheckBox)
		{
			backgrounderCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.phonkCheckBox)
		{
			phonkCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.phonkIntensityLabel)
		{
			phonkIntensityLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.phonkIntensitySlider)
		{
			phonkIntensitySlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.plantHealthCheckBox)
		{
			value = VariantUtils.CreateFrom(in plantHealthCheckBox);
			return true;
		}
		if (name == PropertyName.zombieHealthCheckBox)
		{
			value = VariantUtils.CreateFrom(in zombieHealthCheckBox);
			return true;
		}
		if (name == PropertyName.bossHealthBarCheckBox)
		{
			value = VariantUtils.CreateFrom(in bossHealthBarCheckBox);
			return true;
		}
		if (name == PropertyName.packetUIFrontCheckBox)
		{
			value = VariantUtils.CreateFrom(in packetUIFrontCheckBox);
			return true;
		}
		if (name == PropertyName.mapEffectCheckBox)
		{
			value = VariantUtils.CreateFrom(in mapEffectCheckBox);
			return true;
		}
		if (name == PropertyName.backgrounderCheckBox)
		{
			value = VariantUtils.CreateFrom(in backgrounderCheckBox);
			return true;
		}
		if (name == PropertyName.phonkCheckBox)
		{
			value = VariantUtils.CreateFrom(in phonkCheckBox);
			return true;
		}
		if (name == PropertyName.phonkIntensityLabel)
		{
			value = VariantUtils.CreateFrom(in phonkIntensityLabel);
			return true;
		}
		if (name == PropertyName.phonkIntensitySlider)
		{
			value = VariantUtils.CreateFrom(in phonkIntensitySlider);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.plantHealthCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.zombieHealthCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.bossHealthBarCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetUIFrontCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapEffectCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.backgrounderCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.phonkCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.phonkIntensityLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.phonkIntensitySlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.plantHealthCheckBox, Variant.From(in plantHealthCheckBox));
		info.AddProperty(PropertyName.zombieHealthCheckBox, Variant.From(in zombieHealthCheckBox));
		info.AddProperty(PropertyName.bossHealthBarCheckBox, Variant.From(in bossHealthBarCheckBox));
		info.AddProperty(PropertyName.packetUIFrontCheckBox, Variant.From(in packetUIFrontCheckBox));
		info.AddProperty(PropertyName.mapEffectCheckBox, Variant.From(in mapEffectCheckBox));
		info.AddProperty(PropertyName.backgrounderCheckBox, Variant.From(in backgrounderCheckBox));
		info.AddProperty(PropertyName.phonkCheckBox, Variant.From(in phonkCheckBox));
		info.AddProperty(PropertyName.phonkIntensityLabel, Variant.From(in phonkIntensityLabel));
		info.AddProperty(PropertyName.phonkIntensitySlider, Variant.From(in phonkIntensitySlider));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.plantHealthCheckBox, out var value))
		{
			plantHealthCheckBox = value.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.zombieHealthCheckBox, out var value2))
		{
			zombieHealthCheckBox = value2.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.bossHealthBarCheckBox, out var value3))
		{
			bossHealthBarCheckBox = value3.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.packetUIFrontCheckBox, out var value4))
		{
			packetUIFrontCheckBox = value4.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.mapEffectCheckBox, out var value5))
		{
			mapEffectCheckBox = value5.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.backgrounderCheckBox, out var value6))
		{
			backgrounderCheckBox = value6.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.phonkCheckBox, out var value7))
		{
			phonkCheckBox = value7.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.phonkIntensityLabel, out var value8))
		{
			phonkIntensityLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.phonkIntensitySlider, out var value9))
		{
			phonkIntensitySlider = value9.As<HSlider>();
		}
	}
}
