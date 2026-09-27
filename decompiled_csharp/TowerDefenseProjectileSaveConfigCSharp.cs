using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Save/Projectile/TowerDefenseProjectileSaveConfigCSharp.cs")]
public class TowerDefenseProjectileSaveConfigCSharp : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName ReportUnsupportedLegacyRecord = "ReportUnsupportedLegacyRecord";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName nodeName = "nodeName";

		public static readonly StringName configName = "configName";

		public static readonly StringName pos = "pos";

		public static readonly StringName velocity = "velocity";

		public static readonly StringName speed = "speed";

		public static readonly StringName camp = "camp";

		public static readonly StringName damage = "damage";

		public static readonly StringName collisionFlags = "collisionFlags";

		public static readonly StringName damageFlags = "damageFlags";

		public static readonly StringName fireMethodFlags = "fireMethodFlags";

		public static readonly StringName gridPos = "gridPos";

		public static readonly StringName height = "height";

		public static readonly StringName z = "z";

		public static readonly StringName groundHeight = "groundHeight";

		public static readonly StringName isGround = "isGround";

		public static readonly StringName over = "over";

		public static readonly StringName checkAll = "checkAll";

		public static readonly StringName fireCharacterName = "fireCharacterName";

		public static readonly StringName targetName = "targetName";

		public static readonly StringName catapultTime = "catapultTime";

		public static readonly StringName catapultTimer = "catapultTimer";

		public static readonly StringName catapultTargetPos = "catapultTargetPos";

		public static readonly StringName catapultControlPoint = "catapultControlPoint";

		public static readonly StringName catapulCheckLast = "catapulCheckLast";

		public static readonly StringName penetrateNum = "penetrateNum";

		public static readonly StringName hitOver = "hitOver";

		public static readonly StringName checkDistance = "checkDistance";

		public static readonly StringName fireLength = "fireLength";

		public static readonly StringName trackOpen = "trackOpen";

		public static readonly StringName catapultOpen = "catapultOpen";

		public static readonly StringName fireDirX = "fireDirX";

		public static readonly StringName isShooter = "isShooter";

		public static readonly StringName spriteSave = "spriteSave";

		public static readonly StringName useFall = "useFall";

		public static readonly StringName useGravity = "useGravity";

		public static readonly StringName ySpeed = "ySpeed";

		public static readonly StringName gravityUse = "gravityUse";

		public static readonly StringName gravity = "gravity";

		public static readonly StringName gravityScale = "gravityScale";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public StringName nodeName;

	[Export(PropertyHint.None, "")]
	public string configName;

	[Export(PropertyHint.None, "")]
	public Vector2 pos;

	[Export(PropertyHint.None, "")]
	public Vector2 velocity;

	[Export(PropertyHint.None, "")]
	public double speed;

	[Export(PropertyHint.None, "")]
	public int camp;

	[Export(PropertyHint.None, "")]
	public double damage;

	[Export(PropertyHint.None, "")]
	public int collisionFlags;

	[Export(PropertyHint.None, "")]
	public int damageFlags;

	[Export(PropertyHint.None, "")]
	public int fireMethodFlags;

	[Export(PropertyHint.None, "")]
	public Vector2I gridPos;

	[Export(PropertyHint.None, "")]
	public double height;

	[Export(PropertyHint.None, "")]
	public double z;

	[Export(PropertyHint.None, "")]
	public double groundHeight;

	[Export(PropertyHint.None, "")]
	public bool isGround;

	[Export(PropertyHint.None, "")]
	public bool over;

	[Export(PropertyHint.None, "")]
	public bool checkAll;

	[Export(PropertyHint.None, "")]
	public StringName fireCharacterName;

	[Export(PropertyHint.None, "")]
	public StringName targetName;

	[Export(PropertyHint.None, "")]
	public double catapultTime;

	[Export(PropertyHint.None, "")]
	public double catapultTimer;

	[Export(PropertyHint.None, "")]
	public Vector2 catapultTargetPos;

	[Export(PropertyHint.None, "")]
	public Vector2 catapultControlPoint;

	[Export(PropertyHint.None, "")]
	public bool catapulCheckLast;

	[Export(PropertyHint.None, "")]
	public int penetrateNum;

	[Export(PropertyHint.None, "")]
	public bool hitOver;

	[Export(PropertyHint.None, "")]
	public double checkDistance;

	[Export(PropertyHint.None, "")]
	public double fireLength;

	[Export(PropertyHint.None, "")]
	public bool trackOpen;

	[Export(PropertyHint.None, "")]
	public bool catapultOpen;

	[Export(PropertyHint.None, "")]
	public int fireDirX;

	[Export(PropertyHint.None, "")]
	public bool isShooter;

	[Export(PropertyHint.None, "")]
	public Dictionary spriteSave = new Dictionary();

	[Export(PropertyHint.None, "")]
	public bool useFall;

	[Export(PropertyHint.None, "")]
	public bool useGravity;

	[Export(PropertyHint.None, "")]
	public double ySpeed;

	[Export(PropertyHint.None, "")]
	public bool gravityUse;

	[Export(PropertyHint.None, "")]
	public double gravity;

	[Export(PropertyHint.None, "")]
	public double gravityScale;

	public void ReportUnsupportedLegacyRecord()
	{
		string value = (string.IsNullOrEmpty(configName) ? null : TowerDefenseManager.GetProjectileConfig(configName))?.projectileScene?.ResourcePath ?? "<none>";
		GD.PushError($"[BulletField:E_LEGACY_NODE_SAVE_UNSUPPORTED] projectile='{configName}' scene='{value}' reason='legacy projectileList records require the deleted Node loader'");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.ReportUnsupportedLegacyRecord, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ReportUnsupportedLegacyRecord && args.Count == 0)
		{
			ReportUnsupportedLegacyRecord();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ReportUnsupportedLegacyRecord)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.nodeName)
		{
			nodeName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.configName)
		{
			configName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.pos)
		{
			pos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.velocity)
		{
			velocity = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.speed)
		{
			speed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.camp)
		{
			camp = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.damage)
		{
			damage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			collisionFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.damageFlags)
		{
			damageFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.fireMethodFlags)
		{
			fireMethodFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			gridPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.height)
		{
			height = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.z)
		{
			z = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.groundHeight)
		{
			groundHeight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.isGround)
		{
			isGround = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkAll)
		{
			checkAll = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fireCharacterName)
		{
			fireCharacterName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.targetName)
		{
			targetName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.catapultTime)
		{
			catapultTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.catapultTimer)
		{
			catapultTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.catapultTargetPos)
		{
			catapultTargetPos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.catapultControlPoint)
		{
			catapultControlPoint = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.catapulCheckLast)
		{
			catapulCheckLast = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.penetrateNum)
		{
			penetrateNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.hitOver)
		{
			hitOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkDistance)
		{
			checkDistance = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireLength)
		{
			fireLength = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.trackOpen)
		{
			trackOpen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.catapultOpen)
		{
			catapultOpen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fireDirX)
		{
			fireDirX = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.isShooter)
		{
			isShooter = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.spriteSave)
		{
			spriteSave = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.useFall)
		{
			useFall = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useGravity)
		{
			useGravity = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ySpeed)
		{
			ySpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.gravityUse)
		{
			gravityUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.gravity)
		{
			gravity = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.gravityScale)
		{
			gravityScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.nodeName)
		{
			value = VariantUtils.CreateFrom(in nodeName);
			return true;
		}
		if (name == PropertyName.configName)
		{
			value = VariantUtils.CreateFrom(in configName);
			return true;
		}
		if (name == PropertyName.pos)
		{
			value = VariantUtils.CreateFrom(in pos);
			return true;
		}
		if (name == PropertyName.velocity)
		{
			value = VariantUtils.CreateFrom(in velocity);
			return true;
		}
		if (name == PropertyName.speed)
		{
			value = VariantUtils.CreateFrom(in speed);
			return true;
		}
		if (name == PropertyName.camp)
		{
			value = VariantUtils.CreateFrom(in camp);
			return true;
		}
		if (name == PropertyName.damage)
		{
			value = VariantUtils.CreateFrom(in damage);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			value = VariantUtils.CreateFrom(in collisionFlags);
			return true;
		}
		if (name == PropertyName.damageFlags)
		{
			value = VariantUtils.CreateFrom(in damageFlags);
			return true;
		}
		if (name == PropertyName.fireMethodFlags)
		{
			value = VariantUtils.CreateFrom(in fireMethodFlags);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			value = VariantUtils.CreateFrom(in gridPos);
			return true;
		}
		if (name == PropertyName.height)
		{
			value = VariantUtils.CreateFrom(in height);
			return true;
		}
		if (name == PropertyName.z)
		{
			value = VariantUtils.CreateFrom(in z);
			return true;
		}
		if (name == PropertyName.groundHeight)
		{
			value = VariantUtils.CreateFrom(in groundHeight);
			return true;
		}
		if (name == PropertyName.isGround)
		{
			value = VariantUtils.CreateFrom(in isGround);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.checkAll)
		{
			value = VariantUtils.CreateFrom(in checkAll);
			return true;
		}
		if (name == PropertyName.fireCharacterName)
		{
			value = VariantUtils.CreateFrom(in fireCharacterName);
			return true;
		}
		if (name == PropertyName.targetName)
		{
			value = VariantUtils.CreateFrom(in targetName);
			return true;
		}
		if (name == PropertyName.catapultTime)
		{
			value = VariantUtils.CreateFrom(in catapultTime);
			return true;
		}
		if (name == PropertyName.catapultTimer)
		{
			value = VariantUtils.CreateFrom(in catapultTimer);
			return true;
		}
		if (name == PropertyName.catapultTargetPos)
		{
			value = VariantUtils.CreateFrom(in catapultTargetPos);
			return true;
		}
		if (name == PropertyName.catapultControlPoint)
		{
			value = VariantUtils.CreateFrom(in catapultControlPoint);
			return true;
		}
		if (name == PropertyName.catapulCheckLast)
		{
			value = VariantUtils.CreateFrom(in catapulCheckLast);
			return true;
		}
		if (name == PropertyName.penetrateNum)
		{
			value = VariantUtils.CreateFrom(in penetrateNum);
			return true;
		}
		if (name == PropertyName.hitOver)
		{
			value = VariantUtils.CreateFrom(in hitOver);
			return true;
		}
		if (name == PropertyName.checkDistance)
		{
			value = VariantUtils.CreateFrom(in checkDistance);
			return true;
		}
		if (name == PropertyName.fireLength)
		{
			value = VariantUtils.CreateFrom(in fireLength);
			return true;
		}
		if (name == PropertyName.trackOpen)
		{
			value = VariantUtils.CreateFrom(in trackOpen);
			return true;
		}
		if (name == PropertyName.catapultOpen)
		{
			value = VariantUtils.CreateFrom(in catapultOpen);
			return true;
		}
		if (name == PropertyName.fireDirX)
		{
			value = VariantUtils.CreateFrom(in fireDirX);
			return true;
		}
		if (name == PropertyName.isShooter)
		{
			value = VariantUtils.CreateFrom(in isShooter);
			return true;
		}
		if (name == PropertyName.spriteSave)
		{
			value = VariantUtils.CreateFrom(in spriteSave);
			return true;
		}
		if (name == PropertyName.useFall)
		{
			value = VariantUtils.CreateFrom(in useFall);
			return true;
		}
		if (name == PropertyName.useGravity)
		{
			value = VariantUtils.CreateFrom(in useGravity);
			return true;
		}
		if (name == PropertyName.ySpeed)
		{
			value = VariantUtils.CreateFrom(in ySpeed);
			return true;
		}
		if (name == PropertyName.gravityUse)
		{
			value = VariantUtils.CreateFrom(in gravityUse);
			return true;
		}
		if (name == PropertyName.gravity)
		{
			value = VariantUtils.CreateFrom(in gravity);
			return true;
		}
		if (name == PropertyName.gravityScale)
		{
			value = VariantUtils.CreateFrom(in gravityScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.nodeName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.configName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.pos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.velocity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.speed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.camp, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.damage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.damageFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireMethodFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridPos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.height, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.z, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.groundHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isGround, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkAll, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.fireCharacterName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.targetName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.catapultTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.catapultTimer, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.catapultTargetPos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.catapultControlPoint, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.catapulCheckLast, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.penetrateNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hitOver, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.checkDistance, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireLength, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.trackOpen, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.catapultOpen, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireDirX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isShooter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.spriteSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useFall, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useGravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.ySpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.gravityUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.gravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.gravityScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.nodeName, Variant.From(in nodeName));
		info.AddProperty(PropertyName.configName, Variant.From(in configName));
		info.AddProperty(PropertyName.pos, Variant.From(in pos));
		info.AddProperty(PropertyName.velocity, Variant.From(in velocity));
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName.camp, Variant.From(in camp));
		info.AddProperty(PropertyName.damage, Variant.From(in damage));
		info.AddProperty(PropertyName.collisionFlags, Variant.From(in collisionFlags));
		info.AddProperty(PropertyName.damageFlags, Variant.From(in damageFlags));
		info.AddProperty(PropertyName.fireMethodFlags, Variant.From(in fireMethodFlags));
		info.AddProperty(PropertyName.gridPos, Variant.From(in gridPos));
		info.AddProperty(PropertyName.height, Variant.From(in height));
		info.AddProperty(PropertyName.z, Variant.From(in z));
		info.AddProperty(PropertyName.groundHeight, Variant.From(in groundHeight));
		info.AddProperty(PropertyName.isGround, Variant.From(in isGround));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.checkAll, Variant.From(in checkAll));
		info.AddProperty(PropertyName.fireCharacterName, Variant.From(in fireCharacterName));
		info.AddProperty(PropertyName.targetName, Variant.From(in targetName));
		info.AddProperty(PropertyName.catapultTime, Variant.From(in catapultTime));
		info.AddProperty(PropertyName.catapultTimer, Variant.From(in catapultTimer));
		info.AddProperty(PropertyName.catapultTargetPos, Variant.From(in catapultTargetPos));
		info.AddProperty(PropertyName.catapultControlPoint, Variant.From(in catapultControlPoint));
		info.AddProperty(PropertyName.catapulCheckLast, Variant.From(in catapulCheckLast));
		info.AddProperty(PropertyName.penetrateNum, Variant.From(in penetrateNum));
		info.AddProperty(PropertyName.hitOver, Variant.From(in hitOver));
		info.AddProperty(PropertyName.checkDistance, Variant.From(in checkDistance));
		info.AddProperty(PropertyName.fireLength, Variant.From(in fireLength));
		info.AddProperty(PropertyName.trackOpen, Variant.From(in trackOpen));
		info.AddProperty(PropertyName.catapultOpen, Variant.From(in catapultOpen));
		info.AddProperty(PropertyName.fireDirX, Variant.From(in fireDirX));
		info.AddProperty(PropertyName.isShooter, Variant.From(in isShooter));
		info.AddProperty(PropertyName.spriteSave, Variant.From(in spriteSave));
		info.AddProperty(PropertyName.useFall, Variant.From(in useFall));
		info.AddProperty(PropertyName.useGravity, Variant.From(in useGravity));
		info.AddProperty(PropertyName.ySpeed, Variant.From(in ySpeed));
		info.AddProperty(PropertyName.gravityUse, Variant.From(in gravityUse));
		info.AddProperty(PropertyName.gravity, Variant.From(in gravity));
		info.AddProperty(PropertyName.gravityScale, Variant.From(in gravityScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.nodeName, out var value))
		{
			nodeName = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.configName, out var value2))
		{
			configName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.pos, out var value3))
		{
			pos = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.velocity, out var value4))
		{
			velocity = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.speed, out var value5))
		{
			speed = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.camp, out var value6))
		{
			camp = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.damage, out var value7))
		{
			damage = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.collisionFlags, out var value8))
		{
			collisionFlags = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.damageFlags, out var value9))
		{
			damageFlags = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.fireMethodFlags, out var value10))
		{
			fireMethodFlags = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.gridPos, out var value11))
		{
			gridPos = value11.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.height, out var value12))
		{
			height = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.z, out var value13))
		{
			z = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.groundHeight, out var value14))
		{
			groundHeight = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.isGround, out var value15))
		{
			isGround = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value16))
		{
			over = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkAll, out var value17))
		{
			checkAll = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fireCharacterName, out var value18))
		{
			fireCharacterName = value18.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.targetName, out var value19))
		{
			targetName = value19.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.catapultTime, out var value20))
		{
			catapultTime = value20.As<double>();
		}
		if (info.TryGetProperty(PropertyName.catapultTimer, out var value21))
		{
			catapultTimer = value21.As<double>();
		}
		if (info.TryGetProperty(PropertyName.catapultTargetPos, out var value22))
		{
			catapultTargetPos = value22.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.catapultControlPoint, out var value23))
		{
			catapultControlPoint = value23.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.catapulCheckLast, out var value24))
		{
			catapulCheckLast = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.penetrateNum, out var value25))
		{
			penetrateNum = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName.hitOver, out var value26))
		{
			hitOver = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkDistance, out var value27))
		{
			checkDistance = value27.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireLength, out var value28))
		{
			fireLength = value28.As<double>();
		}
		if (info.TryGetProperty(PropertyName.trackOpen, out var value29))
		{
			trackOpen = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.catapultOpen, out var value30))
		{
			catapultOpen = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fireDirX, out var value31))
		{
			fireDirX = value31.As<int>();
		}
		if (info.TryGetProperty(PropertyName.isShooter, out var value32))
		{
			isShooter = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.spriteSave, out var value33))
		{
			spriteSave = value33.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.useFall, out var value34))
		{
			useFall = value34.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useGravity, out var value35))
		{
			useGravity = value35.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ySpeed, out var value36))
		{
			ySpeed = value36.As<double>();
		}
		if (info.TryGetProperty(PropertyName.gravityUse, out var value37))
		{
			gravityUse = value37.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.gravity, out var value38))
		{
			gravity = value38.As<double>();
		}
		if (info.TryGetProperty(PropertyName.gravityScale, out var value39))
		{
			gravityScale = value39.As<double>();
		}
	}
}
