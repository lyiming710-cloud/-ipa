using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/RichwoodChestBulletFieldRuntimeTest.cs")]
public class RichwoodChestBulletFieldRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FillAndGetEventProxy = "FillAndGetEventProxy";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		BulletField bulletField = null;
		try
		{
			try
			{
				TowerDefenseProjectileRegistry.Init();
				bulletField = new BulletField
				{
					Name = "RichwoodChestBulletField"
				};
				AddChild(bulletField, forceReadableName: false, InternalMode.Disabled);
				TowerDefenseProjectileConfig towerDefenseProjectileConfig = new TowerDefenseProjectileCreateData(new StringName("Chest")).BuildConfig();
				TowerDefenseProjectileConfig towerDefenseProjectileConfig2 = new TowerDefenseProjectileCreateData(new StringName("Pea")).BuildConfig();
				Check(GodotObject.IsInstanceValid(towerDefenseProjectileConfig) && GodotObject.IsInstanceValid(towerDefenseProjectileConfig.projectileScene), "The canonical Chest projectile and its scene must load from the live registry.");
				Check(GodotObject.IsInstanceValid(towerDefenseProjectileConfig2), "The captured projectile config must load from the live registry.");
				if (!GodotObject.IsInstanceValid(towerDefenseProjectileConfig) || !GodotObject.IsInstanceValid(towerDefenseProjectileConfig2))
				{
					goto end_IL_0024;
				}
				TowerDefenseCharacterEventExplodeProjectileFromMetaData towerDefenseCharacterEventExplodeProjectileFromMetaData = null;
				foreach (TowerDefenseCharacterEventBase hitTargetEvent in towerDefenseProjectileConfig.hitTargetEventList)
				{
					if (hitTargetEvent is TowerDefenseCharacterEventExplodeProjectileFromMetaData towerDefenseCharacterEventExplodeProjectileFromMetaData2)
					{
						towerDefenseCharacterEventExplodeProjectileFromMetaData = towerDefenseCharacterEventExplodeProjectileFromMetaData2;
						break;
					}
				}
				Check(GodotObject.IsInstanceValid(towerDefenseCharacterEventExplodeProjectileFromMetaData), "The canonical Chest projectile must retain its metadata scatter hit event.");
				if (!GodotObject.IsInstanceValid(towerDefenseCharacterEventExplodeProjectileFromMetaData))
				{
					goto end_IL_0024;
				}
				int num = towerDefenseProjectileConfig2.fireMethodFlags | 8;
				BulletFieldStoredProjectile[] array = new BulletFieldStoredProjectile[6];
				for (int i = 0; i < 6; i++)
				{
					array[i] = new BulletFieldStoredProjectile(towerDefenseProjectileConfig2, num);
				}
				List<int> list = new List<int>();
				bulletField.OnBulletSpawned += list.Add;
				int num2 = bulletField.TrySpawnFromConfig(towerDefenseProjectileConfig, new Vector2(240f, 240f), new Vector2(600f, 0f), 600.0, null, TowerDefenseEnum.CHARACTER_CAMP.PLANT, new Vector2I(3, 3), 3, new Rect2(-2000f, -2000f, 4000f, 4000f), null, 0.0, 0.0, 1, -1f, checkHeight: false, checkAll: false, useFall: false, useGravity: false, 1.5f, 0f, 0f, new BulletFieldSpawnOverrides
				{
					gridYOverride = 3,
					eventProjectiles = array
				});
				Check(num2 >= 0 && bulletField.ActiveCount == 1, "The real Chest projectile must spawn into BulletField.");
				if (num2 < 0)
				{
					goto end_IL_0024;
				}
				IDictionary eventProjectileDictionary = GetEventProjectileDictionary(bulletField);
				Check(eventProjectileDictionary.Count == 1 && eventProjectileDictionary.Contains(num2), "BulletField must retain metadata only for the Chest slot.");
				TowerDefenseProjectile towerDefenseProjectile = FillAndGetEventProxy(bulletField, num2);
				RichwoodChestBulletFieldRuntimeTest richwoodChestBulletFieldRuntimeTest = this;
				int condition;
				if (GodotObject.IsInstanceValid(towerDefenseProjectile))
				{
					BulletFieldStoredProjectile[] eventProjectiles = towerDefenseProjectile.eventProjectiles;
					condition = ((eventProjectiles != null && eventProjectiles.Length == 6) ? 1 : 0);
				}
				else
				{
					condition = 0;
				}
				richwoodChestBulletFieldRuntimeTest.Check((byte)condition != 0, "The shared event proxy must receive all six captured projectile configs.");
				if (!GodotObject.IsInstanceValid(towerDefenseProjectile))
				{
					goto end_IL_0024;
				}
				int activeCount = bulletField.ActiveCount;
				towerDefenseCharacterEventExplodeProjectileFromMetaData.ExecuteProject(towerDefenseProjectile, null);
				Check(bulletField.ActiveCount == activeCount + 6, "The real Chest metadata hit event must emit all six stored projectiles.");
				Check(list.Count == 7, "The Chest plus six released projectiles must all use BulletField slots.");
				Check(GetTree().GetNodesInGroup("Projectile").Count == 0, "Richwood output must not restore per-projectile Nodes.");
				for (int j = 1; j < list.Count; j++)
				{
					ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(list[j]);
					Check(bulletDataRef.active && bulletDataRef.config.NameSN == towerDefenseProjectileConfig2.NameSN && bulletDataRef.fireMethodFlags == num && bulletDataRef.checkAll && bulletDataRef.vel.LengthSquared() > 0f, $"Released projectile {j} must preserve its config and radial BulletField motion.");
				}
				bulletField.ClearActiveBullets();
				Check(bulletField.ActiveCount == 0 && eventProjectileDictionary.Count == 0, "Despawn must release the Chest metadata sidecar.");
				goto end_IL_0024_2;
				end_IL_0024:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[RichwoodChestBulletField] Unexpected exception: {value}");
				goto end_IL_0024_2;
			}
			end_IL_0024_2:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(bulletField))
			{
				bulletField.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			bool flag = _failures == 0 && _checks == 16;
			GD.Print($"RICHWOOD_CHEST_BULLET_FIELD_RESULT passed={flag} checks={_checks} failures={_failures}");
			GetTree().Quit((!flag) ? 2 : 0);
		}
	}

	private static IDictionary GetEventProjectileDictionary(BulletField bulletField)
	{
		return (typeof(BulletField).GetField("_eventProjectilesByIndex", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(bulletField) as IDictionary) ?? throw new MissingFieldException(typeof(BulletField).FullName, "_eventProjectilesByIndex");
	}

	private static TowerDefenseProjectile FillAndGetEventProxy(BulletField bulletField, int index)
	{
		System.Reflection.MethodInfo? method = typeof(BulletField).GetMethod("FillEventProxy", BindingFlags.Instance | BindingFlags.NonPublic);
		if (method == null)
		{
			throw new MissingMethodException(typeof(BulletField).FullName, "FillEventProxy");
		}
		BulletData bulletDataRef = bulletField.GetBulletDataRef(index);
		method.Invoke(bulletField, new object[2] { index, bulletDataRef });
		return (typeof(BulletField).GetField("_eventProxy", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null) as TowerDefenseProjectile) ?? throw new MissingFieldException(typeof(BulletField).FullName, "_eventProxy");
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[RichwoodChestBulletField] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(3)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.FillAndGetEventProxy, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "bulletField", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.FillAndGetEventProxy && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectile>(FillAndGetEventProxy(VariantUtils.ConvertTo<BulletField>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FillAndGetEventProxy && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectile>(FillAndGetEventProxy(VariantUtils.ConvertTo<BulletField>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.FillAndGetEventProxy)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
