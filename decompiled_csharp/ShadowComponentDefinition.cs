using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ShadowComponent/ShadowComponentDefinition.cs")]
public class ShadowComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName followHeight = "followHeight";

		public static readonly StringName heightScaleFactor = "heightScaleFactor";

		public static readonly StringName minimumHeightScale = "minimumHeightScale";

		public static readonly StringName preferMultiMesh = "preferMultiMesh";

		public static readonly StringName hideWhenInvisible = "hideWhenInvisible";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool followHeight;

	[Export(PropertyHint.Range, "1,10000,1")]
	public float heightScaleFactor = 900f;

	[Export(PropertyHint.Range, "0,1,0.01")]
	public float minimumHeightScale;

	[Export(PropertyHint.None, "")]
	public bool preferMultiMesh = true;

	[Export(PropertyHint.None, "")]
	public bool hideWhenInvisible = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ShadowComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.followHeight)
		{
			followHeight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.heightScaleFactor)
		{
			heightScaleFactor = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.minimumHeightScale)
		{
			minimumHeightScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.preferMultiMesh)
		{
			preferMultiMesh = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hideWhenInvisible)
		{
			hideWhenInvisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.followHeight)
		{
			value = VariantUtils.CreateFrom(in followHeight);
			return true;
		}
		if (name == PropertyName.heightScaleFactor)
		{
			value = VariantUtils.CreateFrom(in heightScaleFactor);
			return true;
		}
		if (name == PropertyName.minimumHeightScale)
		{
			value = VariantUtils.CreateFrom(in minimumHeightScale);
			return true;
		}
		if (name == PropertyName.preferMultiMesh)
		{
			value = VariantUtils.CreateFrom(in preferMultiMesh);
			return true;
		}
		if (name == PropertyName.hideWhenInvisible)
		{
			value = VariantUtils.CreateFrom(in hideWhenInvisible);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.followHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.heightScaleFactor, PropertyHint.Range, "1,10000,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.minimumHeightScale, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.preferMultiMesh, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hideWhenInvisible, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.followHeight, Variant.From(in followHeight));
		info.AddProperty(PropertyName.heightScaleFactor, Variant.From(in heightScaleFactor));
		info.AddProperty(PropertyName.minimumHeightScale, Variant.From(in minimumHeightScale));
		info.AddProperty(PropertyName.preferMultiMesh, Variant.From(in preferMultiMesh));
		info.AddProperty(PropertyName.hideWhenInvisible, Variant.From(in hideWhenInvisible));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.followHeight, out var value))
		{
			followHeight = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.heightScaleFactor, out var value2))
		{
			heightScaleFactor = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.minimumHeightScale, out var value3))
		{
			minimumHeightScale = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.preferMultiMesh, out var value4))
		{
			preferMultiMesh = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hideWhenInvisible, out var value5))
		{
			hideWhenInvisible = value5.As<bool>();
		}
	}
}
