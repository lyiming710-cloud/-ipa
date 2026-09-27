using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/PuzzleShaderComponent/PuzzleShaderComponentDefinition.cs")]
public class PuzzleShaderComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName enablePuzzleShader = "enablePuzzleShader";

		public static readonly StringName freezeInIzmIdle = "freezeInIzmIdle";

		public static readonly StringName shaderParameter = "shaderParameter";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool enablePuzzleShader = true;

	[Export(PropertyHint.None, "")]
	public bool freezeInIzmIdle = true;

	[Export(PropertyHint.None, "")]
	public StringName shaderParameter = "puzzle";

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new PuzzleShaderComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.enablePuzzleShader)
		{
			enablePuzzleShader = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.freezeInIzmIdle)
		{
			freezeInIzmIdle = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.shaderParameter)
		{
			shaderParameter = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.enablePuzzleShader)
		{
			value = VariantUtils.CreateFrom(in enablePuzzleShader);
			return true;
		}
		if (name == PropertyName.freezeInIzmIdle)
		{
			value = VariantUtils.CreateFrom(in freezeInIzmIdle);
			return true;
		}
		if (name == PropertyName.shaderParameter)
		{
			value = VariantUtils.CreateFrom(in shaderParameter);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.enablePuzzleShader, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.freezeInIzmIdle, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.shaderParameter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.enablePuzzleShader, Variant.From(in enablePuzzleShader));
		info.AddProperty(PropertyName.freezeInIzmIdle, Variant.From(in freezeInIzmIdle));
		info.AddProperty(PropertyName.shaderParameter, Variant.From(in shaderParameter));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.enablePuzzleShader, out var value))
		{
			enablePuzzleShader = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.freezeInIzmIdle, out var value2))
		{
			freezeInIzmIdle = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.shaderParameter, out var value3))
		{
			shaderParameter = value3.As<StringName>();
		}
	}
}
