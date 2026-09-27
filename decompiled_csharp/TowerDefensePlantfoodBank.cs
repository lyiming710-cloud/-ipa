using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/GUI/InGame/PlantfoodBank/TowerDefensePlantfoodBank.cs")]
public class TowerDefensePlantfoodBank : Control
{
	public delegate void PickEventHandler();

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PlantfoodButtonPressed = "PlantfoodButtonPressed";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _plantfoodSlotContainer = "_plantfoodSlotContainer";

		public static readonly StringName _plantfoodButton = "_plantfoodButton";

		public static readonly StringName beginSizeX = "beginSizeX";

		public static readonly StringName sizeInterval = "sizeInterval";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private HBoxContainer _plantfoodSlotContainer;

	private TextureButton _plantfoodButton;

	public int beginSizeX = 314;

	public int sizeInterval = 50;

	public event PickEventHandler OnPick;

	public override void _Ready()
	{
		_plantfoodSlotContainer = GetNodeOrNull<HBoxContainer>("%PlantfoodSlotContainer");
		_plantfoodButton = GetNodeOrNull<TextureButton>("%PlantfoodButton");
		if (GodotObject.IsInstanceValid(_plantfoodButton))
		{
			_plantfoodButton.Pressed += PlantfoodButtonPressed;
		}
	}

	public void PlantfoodButtonPressed()
	{
		OnPick?.Invoke();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlantfoodButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.PlantfoodButtonPressed && args.Count == 0)
		{
			PlantfoodButtonPressed();
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
		if (method == MethodName.PlantfoodButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._plantfoodSlotContainer)
		{
			_plantfoodSlotContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._plantfoodButton)
		{
			_plantfoodButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.beginSizeX)
		{
			beginSizeX = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.sizeInterval)
		{
			sizeInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._plantfoodSlotContainer)
		{
			value = VariantUtils.CreateFrom(in _plantfoodSlotContainer);
			return true;
		}
		if (name == PropertyName._plantfoodButton)
		{
			value = VariantUtils.CreateFrom(in _plantfoodButton);
			return true;
		}
		if (name == PropertyName.beginSizeX)
		{
			value = VariantUtils.CreateFrom(in beginSizeX);
			return true;
		}
		if (name == PropertyName.sizeInterval)
		{
			value = VariantUtils.CreateFrom(in sizeInterval);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._plantfoodSlotContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._plantfoodButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.beginSizeX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.sizeInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._plantfoodSlotContainer, Variant.From(in _plantfoodSlotContainer));
		info.AddProperty(PropertyName._plantfoodButton, Variant.From(in _plantfoodButton));
		info.AddProperty(PropertyName.beginSizeX, Variant.From(in beginSizeX));
		info.AddProperty(PropertyName.sizeInterval, Variant.From(in sizeInterval));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._plantfoodSlotContainer, out var value))
		{
			_plantfoodSlotContainer = value.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._plantfoodButton, out var value2))
		{
			_plantfoodButton = value2.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.beginSizeX, out var value3))
		{
			beginSizeX = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.sizeInterval, out var value4))
		{
			sizeInterval = value4.As<int>();
		}
	}
}
