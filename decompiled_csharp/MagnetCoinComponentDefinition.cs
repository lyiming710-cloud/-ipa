using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/MagnetCoinComponent/MagnetCoinComponentDefinition.cs")]
public class MagnetCoinComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName posMarkerPath = "posMarkerPath";

		public static readonly StringName magnetTime = "magnetTime";

		public static readonly StringName magnetNum = "magnetNum";

		public static readonly StringName useRegistryCoinTypes = "useRegistryCoinTypes";

		public static readonly StringName objectList = "objectList";

		public static readonly StringName pullSpeed = "pullSpeed";

		public static readonly StringName collectDistance = "collectDistance";

		public static readonly StringName releaseVelocityMin = "releaseVelocityMin";

		public static readonly StringName releaseVelocityMax = "releaseVelocityMax";

		public static readonly StringName releaseGravity = "releaseGravity";

		public static readonly StringName coinGroupName = "coinGroupName";

		public static readonly StringName goldMagnetGroupName = "goldMagnetGroupName";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Targeting", "")]
	[Export(PropertyHint.None, "")]
	public NodePath posMarkerPath { get; set; } = new NodePath();

	[Export(PropertyHint.Range, "0,300,0.1,or_greater")]
	public float magnetTime { get; set; } = 5f;

	[Export(PropertyHint.None, "")]
	public int magnetNum { get; set; } = -1;

	[Export(PropertyHint.None, "")]
	public bool useRegistryCoinTypes { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public int[] objectList { get; set; } = new int[6] { 0, 1, 2, 4, 5, 6 };

	[Export(PropertyHint.Range, "0,100,0.1,or_greater")]
	public float pullSpeed { get; set; } = 5f;

	[Export(PropertyHint.Range, "0,1000,0.1,or_greater")]
	public float collectDistance { get; set; } = 10f;

	[ExportGroup("Release", "")]
	[Export(PropertyHint.None, "")]
	public Vector2 releaseVelocityMin { get; set; } = new Vector2(-50f, -200f);

	[Export(PropertyHint.None, "")]
	public Vector2 releaseVelocityMax { get; set; } = new Vector2(50f, -200f);

	[Export(PropertyHint.None, "")]
	public float releaseGravity { get; set; } = 980f;

	[Export(PropertyHint.None, "")]
	public string coinGroupName { get; set; } = "Coin";

	[Export(PropertyHint.None, "")]
	public string goldMagnetGroupName { get; set; } = "GoldMagnet";

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new MagnetCoinComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.posMarkerPath)
		{
			posMarkerPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.magnetTime)
		{
			magnetTime = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.magnetNum)
		{
			magnetNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.useRegistryCoinTypes)
		{
			useRegistryCoinTypes = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.objectList)
		{
			objectList = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName.pullSpeed)
		{
			pullSpeed = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.collectDistance)
		{
			collectDistance = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.releaseVelocityMin)
		{
			releaseVelocityMin = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.releaseVelocityMax)
		{
			releaseVelocityMax = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.releaseGravity)
		{
			releaseGravity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.coinGroupName)
		{
			coinGroupName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.goldMagnetGroupName)
		{
			goldMagnetGroupName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.posMarkerPath)
		{
			value = VariantUtils.CreateFrom<NodePath>(posMarkerPath);
			return true;
		}
		float from;
		if (name == PropertyName.magnetTime)
		{
			from = magnetTime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.magnetNum)
		{
			value = VariantUtils.CreateFrom<int>(magnetNum);
			return true;
		}
		if (name == PropertyName.useRegistryCoinTypes)
		{
			value = VariantUtils.CreateFrom<bool>(useRegistryCoinTypes);
			return true;
		}
		if (name == PropertyName.objectList)
		{
			value = VariantUtils.CreateFrom<int[]>(objectList);
			return true;
		}
		if (name == PropertyName.pullSpeed)
		{
			from = pullSpeed;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.collectDistance)
		{
			from = collectDistance;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Vector2 from2;
		if (name == PropertyName.releaseVelocityMin)
		{
			from2 = releaseVelocityMin;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.releaseVelocityMax)
		{
			from2 = releaseVelocityMax;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.releaseGravity)
		{
			from = releaseGravity;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		string from3;
		if (name == PropertyName.coinGroupName)
		{
			from3 = coinGroupName;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.goldMagnetGroupName)
		{
			from3 = goldMagnetGroupName;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Targeting", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.posMarkerPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.magnetTime, PropertyHint.Range, "0,300,0.1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.magnetNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useRegistryCoinTypes, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.objectList, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.pullSpeed, PropertyHint.Range, "0,100,0.1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.collectDistance, PropertyHint.Range, "0,1000,0.1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Release", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.releaseVelocityMin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.releaseVelocityMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.releaseGravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.coinGroupName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.goldMagnetGroupName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.posMarkerPath, Variant.From<NodePath>(posMarkerPath));
		info.AddProperty(PropertyName.magnetTime, Variant.From<float>(magnetTime));
		info.AddProperty(PropertyName.magnetNum, Variant.From<int>(magnetNum));
		info.AddProperty(PropertyName.useRegistryCoinTypes, Variant.From<bool>(useRegistryCoinTypes));
		info.AddProperty(PropertyName.objectList, Variant.From<int[]>(objectList));
		info.AddProperty(PropertyName.pullSpeed, Variant.From<float>(pullSpeed));
		info.AddProperty(PropertyName.collectDistance, Variant.From<float>(collectDistance));
		info.AddProperty(PropertyName.releaseVelocityMin, Variant.From<Vector2>(releaseVelocityMin));
		info.AddProperty(PropertyName.releaseVelocityMax, Variant.From<Vector2>(releaseVelocityMax));
		info.AddProperty(PropertyName.releaseGravity, Variant.From<float>(releaseGravity));
		info.AddProperty(PropertyName.coinGroupName, Variant.From<string>(coinGroupName));
		info.AddProperty(PropertyName.goldMagnetGroupName, Variant.From<string>(goldMagnetGroupName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.posMarkerPath, out var value))
		{
			posMarkerPath = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.magnetTime, out var value2))
		{
			magnetTime = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.magnetNum, out var value3))
		{
			magnetNum = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.useRegistryCoinTypes, out var value4))
		{
			useRegistryCoinTypes = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.objectList, out var value5))
		{
			objectList = value5.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName.pullSpeed, out var value6))
		{
			pullSpeed = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.collectDistance, out var value7))
		{
			collectDistance = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.releaseVelocityMin, out var value8))
		{
			releaseVelocityMin = value8.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.releaseVelocityMax, out var value9))
		{
			releaseVelocityMax = value9.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.releaseGravity, out var value10))
		{
			releaseGravity = value10.As<float>();
		}
		if (info.TryGetProperty(PropertyName.coinGroupName, out var value11))
		{
			coinGroupName = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.goldMagnetGroupName, out var value12))
		{
			goldMagnetGroupName = value12.As<string>();
		}
	}
}
