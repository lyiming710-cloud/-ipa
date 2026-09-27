using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/BloverComponent/BloverComponentDefinition.cs")]
public class BloverComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName blowTime = "blowTime";

		public static readonly StringName blowLength = "blowLength";

		public static readonly StringName blowPhysiqueHugeLength = "blowPhysiqueHugeLength";

		public static readonly StringName blowAirCharacterLength = "blowAirCharacterLength";

		public static readonly StringName blowAirCharacterOut = "blowAirCharacterOut";

		public static readonly StringName checkLine = "checkLine";

		public static readonly StringName checkCollision = "checkCollision";

		public static readonly StringName checkPhysiqueHuge = "checkPhysiqueHuge";

		public static readonly StringName blowAudio = "blowAudio";

		public static readonly StringName projectileRowNum = "projectileRowNum";

		public static readonly StringName projectileBatchInterval = "projectileBatchInterval";

		public static readonly StringName projectileHeightOffsetRange = "projectileHeightOffsetRange";

		public static readonly StringName projectileSpeedRange = "projectileSpeedRange";

		public static readonly StringName projectileLeftSpawnX = "projectileLeftSpawnX";

		public static readonly StringName projectileDataList = "projectileDataList";

		public static readonly StringName eventList = "eventList";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double blowTime { get; set; } = 2.0;

	[Export(PropertyHint.None, "")]
	public double blowLength { get; set; } = 0.5;

	[Export(PropertyHint.None, "")]
	public double blowPhysiqueHugeLength { get; set; } = 0.5;

	[Export(PropertyHint.None, "")]
	public double blowAirCharacterLength { get; set; } = 0.5;

	[Export(PropertyHint.None, "")]
	public bool blowAirCharacterOut { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool checkLine { get; set; }

	[Export(PropertyHint.None, "")]
	public bool checkCollision { get; set; }

	[Export(PropertyHint.None, "")]
	public bool checkPhysiqueHuge { get; set; }

	[Export(PropertyHint.None, "")]
	public string blowAudio { get; set; } = "Blover";

	[ExportSubgroup("Projectile", "")]
	[Export(PropertyHint.Range, "0,100,1")]
	public int projectileRowNum { get; set; } = 20;

	[Export(PropertyHint.Range, "0,2,0.01")]
	public float projectileBatchInterval { get; set; } = 0.1f;

	[Export(PropertyHint.None, "")]
	public Vector2 projectileHeightOffsetRange { get; set; } = new Vector2(10f, 60f);

	[Export(PropertyHint.None, "")]
	public Vector2 projectileSpeedRange { get; set; } = new Vector2(400f, 800f);

	[Export(PropertyHint.None, "")]
	public float projectileLeftSpawnX { get; set; } = -50f;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseProjectileCreateData> projectileDataList { get; set; } = new Array<TowerDefenseProjectileCreateData>();

	[ExportSubgroup("Event", "")]
	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList { get; set; } = new Array<TowerDefenseCharacterEventBase>();

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new BloverComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.blowTime)
		{
			blowTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.blowLength)
		{
			blowLength = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.blowPhysiqueHugeLength)
		{
			blowPhysiqueHugeLength = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.blowAirCharacterLength)
		{
			blowAirCharacterLength = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.blowAirCharacterOut)
		{
			blowAirCharacterOut = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkLine)
		{
			checkLine = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkCollision)
		{
			checkCollision = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkPhysiqueHuge)
		{
			checkPhysiqueHuge = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.blowAudio)
		{
			blowAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.projectileRowNum)
		{
			projectileRowNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.projectileBatchInterval)
		{
			projectileBatchInterval = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.projectileHeightOffsetRange)
		{
			projectileHeightOffsetRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.projectileSpeedRange)
		{
			projectileSpeedRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.projectileLeftSpawnX)
		{
			projectileLeftSpawnX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.projectileDataList)
		{
			projectileDataList = VariantUtils.ConvertToArray<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		double from;
		if (name == PropertyName.blowTime)
		{
			from = blowTime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.blowLength)
		{
			from = blowLength;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.blowPhysiqueHugeLength)
		{
			from = blowPhysiqueHugeLength;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.blowAirCharacterLength)
		{
			from = blowAirCharacterLength;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.blowAirCharacterOut)
		{
			from2 = blowAirCharacterOut;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.checkLine)
		{
			from2 = checkLine;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.checkCollision)
		{
			from2 = checkCollision;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.checkPhysiqueHuge)
		{
			from2 = checkPhysiqueHuge;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.blowAudio)
		{
			value = VariantUtils.CreateFrom<string>(blowAudio);
			return true;
		}
		if (name == PropertyName.projectileRowNum)
		{
			value = VariantUtils.CreateFrom<int>(projectileRowNum);
			return true;
		}
		float from3;
		if (name == PropertyName.projectileBatchInterval)
		{
			from3 = projectileBatchInterval;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		Vector2 from4;
		if (name == PropertyName.projectileHeightOffsetRange)
		{
			from4 = projectileHeightOffsetRange;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.projectileSpeedRange)
		{
			from4 = projectileSpeedRange;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.projectileLeftSpawnX)
		{
			from3 = projectileLeftSpawnX;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.projectileDataList)
		{
			value = VariantUtils.CreateFromArray(projectileDataList);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.blowTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.blowLength, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.blowPhysiqueHugeLength, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.blowAirCharacterLength, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.blowAirCharacterOut, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkLine, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkCollision, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkPhysiqueHuge, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.blowAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Projectile", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.projectileRowNum, PropertyHint.Range, "0,100,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.projectileBatchInterval, PropertyHint.Range, "0,2,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.projectileHeightOffsetRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.projectileSpeedRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.projectileLeftSpawnX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.projectileDataList, PropertyHint.TypeString, "24/17:TowerDefenseProjectileCreateData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Event", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.blowTime, Variant.From<double>(blowTime));
		info.AddProperty(PropertyName.blowLength, Variant.From<double>(blowLength));
		info.AddProperty(PropertyName.blowPhysiqueHugeLength, Variant.From<double>(blowPhysiqueHugeLength));
		info.AddProperty(PropertyName.blowAirCharacterLength, Variant.From<double>(blowAirCharacterLength));
		info.AddProperty(PropertyName.blowAirCharacterOut, Variant.From<bool>(blowAirCharacterOut));
		info.AddProperty(PropertyName.checkLine, Variant.From<bool>(checkLine));
		info.AddProperty(PropertyName.checkCollision, Variant.From<bool>(checkCollision));
		info.AddProperty(PropertyName.checkPhysiqueHuge, Variant.From<bool>(checkPhysiqueHuge));
		info.AddProperty(PropertyName.blowAudio, Variant.From<string>(blowAudio));
		info.AddProperty(PropertyName.projectileRowNum, Variant.From<int>(projectileRowNum));
		info.AddProperty(PropertyName.projectileBatchInterval, Variant.From<float>(projectileBatchInterval));
		info.AddProperty(PropertyName.projectileHeightOffsetRange, Variant.From<Vector2>(projectileHeightOffsetRange));
		info.AddProperty(PropertyName.projectileSpeedRange, Variant.From<Vector2>(projectileSpeedRange));
		info.AddProperty(PropertyName.projectileLeftSpawnX, Variant.From<float>(projectileLeftSpawnX));
		info.AddProperty(PropertyName.projectileDataList, Variant.CreateFrom(projectileDataList));
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.blowTime, out var value))
		{
			blowTime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.blowLength, out var value2))
		{
			blowLength = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.blowPhysiqueHugeLength, out var value3))
		{
			blowPhysiqueHugeLength = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.blowAirCharacterLength, out var value4))
		{
			blowAirCharacterLength = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.blowAirCharacterOut, out var value5))
		{
			blowAirCharacterOut = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkLine, out var value6))
		{
			checkLine = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkCollision, out var value7))
		{
			checkCollision = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkPhysiqueHuge, out var value8))
		{
			checkPhysiqueHuge = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.blowAudio, out var value9))
		{
			blowAudio = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.projectileRowNum, out var value10))
		{
			projectileRowNum = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projectileBatchInterval, out var value11))
		{
			projectileBatchInterval = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName.projectileHeightOffsetRange, out var value12))
		{
			projectileHeightOffsetRange = value12.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.projectileSpeedRange, out var value13))
		{
			projectileSpeedRange = value13.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.projectileLeftSpawnX, out var value14))
		{
			projectileLeftSpawnX = value14.As<float>();
		}
		if (info.TryGetProperty(PropertyName.projectileDataList, out var value15))
		{
			projectileDataList = value15.AsGodotArray<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName.eventList, out var value16))
		{
			eventList = value16.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
	}
}
