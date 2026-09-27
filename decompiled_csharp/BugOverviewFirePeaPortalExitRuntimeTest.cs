using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewFirePeaPortalExitRuntimeTest.cs")]
public class BugOverviewFirePeaPortalExitRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifyZombiePortalRelease = "VerifyZombiePortalRelease";

		public static readonly StringName CreatePortal = "CreatePortal";

		public static readonly StringName EnableCaptureArea = "EnableCaptureArea";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _captureAreaManager = "_captureAreaManager";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private static readonly Vector2I DestinationGrid = new Vector2I(6, 4);

	private static readonly Vector2 DestinationPosition = new Vector2(600f, 304f);

	private int _checks;

	private int _failures;

	private CharacterAabbAreaComponent _captureArea;

	private ComponentManager _captureAreaManager;

	public override async void _Ready()
	{
		BulletField bulletField = null;
		FirePeaPortalExitZombiePortalStub portal = null;
		try
		{
			try
			{
				TowerDefenseProjectileRegistry.Init();
				bulletField = new BulletField
				{
					Name = "FirePeaPortalExitBulletField"
				};
				AddChild(bulletField, forceReadableName: false, InternalMode.Disabled);
				Check(BulletField.Instance == bulletField, "The runtime fixture must mount the live BulletField instance.");
				TowerDefenseProjectileConfig firePeaConfig = new TowerDefenseProjectileCreateData(new StringName("FirePea")).BuildConfig();
				Check(GodotObject.IsInstanceValid(firePeaConfig), "The canonical FirePea config must build from the live projectile registry.");
				if (!GodotObject.IsInstanceValid(firePeaConfig))
				{
					goto end_IL_003e;
				}
				Check(firePeaConfig.NameSN == new StringName("FirePea"), $"The regression must use FirePea, not a stand-in; got {firePeaConfig.NameSN}.");
				Check((firePeaConfig.damageFlags & 4) != 0, "The canonical projectile must retain its fire damage identity.");
				Check((firePeaConfig.fireMethodFlags & 1) != 0 && firePeaConfig.behaviors.Count == 0, "FirePea must remain a standard SHOOTER eligible for the BulletField struct path.");
				portal = CreatePortal();
				AddChild(portal, forceReadableName: false, InternalMode.Disabled);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				EnableCaptureArea(portal);
				VerifyZombiePortalRelease(bulletField, portal, firePeaConfig);
				goto end_IL_0035;
				end_IL_003e:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[FirePeaPortalExit] Unexpected exception: {value}");
				goto end_IL_0035;
			}
			return;
			end_IL_0035:;
		}
		finally
		{
			_captureArea?.Release();
			_captureArea = null;
			_captureAreaManager = null;
			if (GodotObject.IsInstanceValid(portal))
			{
				portal.QueueFree();
			}
			if (GodotObject.IsInstanceValid(bulletField))
			{
				bulletField.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0 && _checks == 17;
		GD.Print($"FIRE_PEA_PORTAL_EXIT_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void VerifyZombiePortalRelease(BulletField bulletField, FirePeaPortalExitZombiePortalStub portal, TowerDefenseProjectileConfig firePeaConfig)
	{
		int num = portal.ReleaseStoredProjectile(firePeaConfig, portal);
		Check(num >= 0, "Portal should release the real FirePea into BulletField.");
		if (num >= 0)
		{
			ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(num);
			Check(bulletDataRef.config == firePeaConfig, "Portal release must reuse the stored FirePea config without duplicating it.");
			Check(bulletDataRef.fireMethodFlags == 1 && bulletDataRef.checkAll && bulletDataRef.gridY == DestinationGrid.Y && bulletDataRef.gridPos.Y == DestinationGrid.Y, "Portal release must apply SHOOTER, cross-row collision, and destination lane state.");
			Check(bulletDataRef.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT && bulletDataRef.vel.X < 0f && bulletDataRef.pos.IsEqualApprox(DestinationPosition + new Vector2(-50f, 20f)) && bulletDataRef.flipX, "The production helper must apply non-hypnotized camp, direction, destination position, and flip.");
			Check(bulletDataRef.portalReleased, "Released portal bullets must not be recaptured.");
			BulletRenderMode renderMode = bulletDataRef.renderMode;
			bool condition = ((renderMode == BulletRenderMode.STATIC || renderMode == BulletRenderMode.ANIMATED_MESH) ? true : false);
			Check(condition, "Released portal bullets must use a MultiMesh render mode.");
			Check(GetTree().GetNodesInGroup("Projectile").Count == 0, "Portal release must not create TowerDefenseProjectile Nodes.");
			int count = portal.projectileConfigList.Count;
			portal.OnBulletIntersect(ref bulletDataRef, num);
			Check(bulletField.IsBulletActive(num) && bulletDataRef.portalReleased && portal.projectileConfigList.Count == count, "The real zombie portal must ignore a released BulletData item without destroying it.");
			TowerDefenseProjectileConfig towerDefenseProjectileConfig = new TowerDefenseProjectileCreateData(new StringName("FirePea"))
			{
				fireMethodFlags = 5
			}.BuildConfig();
			int num2 = bulletField.ChangeBulletData(num, towerDefenseProjectileConfig, null);
			Check(num2 >= 0, "Portal-released FirePea must survive a replacement ChangeBulletData path.");
			if (num2 >= 0)
			{
				ref BulletData bulletDataRef2 = ref bulletField.GetBulletDataRef(num2);
				Check(bulletDataRef2.config == towerDefenseProjectileConfig && bulletDataRef2.fireMethodFlags == towerDefenseProjectileConfig.fireMethodFlags, "The regression must exercise the replacement config rather than an in-place no-op.");
				Check(bulletDataRef2.portalReleased, "Replacement ChangeBulletData must preserve the portal release guard.");
				count = portal.projectileConfigList.Count;
				portal.OnBulletIntersect(ref bulletDataRef2, num2);
				Check(bulletField.IsBulletActive(num2) && bulletDataRef2.portalReleased && portal.projectileConfigList.Count == count, "A replacement of a released bullet must remain immune to zombie-portal recapture.");
			}
		}
	}

	private static FirePeaPortalExitZombiePortalStub CreatePortal()
	{
		return new FirePeaPortalExitZombiePortalStub
		{
			Name = "FirePeaZombiePortal",
			Position = DestinationPosition,
			Scale = Vector2.One,
			gridPos = DestinationGrid,
			camp = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE,
			instance = new TowerDefenseCharacterInstance
			{
				hypnoses = false
			}
		};
	}

	private void EnableCaptureArea(FirePeaPortalExitZombiePortalStub portal)
	{
		_captureAreaManager = new ComponentManager();
		_captureArea = new CharacterAabbAreaComponent();
		_captureArea.Bind(_captureAreaManager, portal, new CharacterAabbAreaComponentDefinition());
		_captureArea.Activate();
		FieldInfo? field = typeof(TowerDefenseZombiePortal).GetField("_checkArea", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field == null)
		{
			throw new MissingFieldException(typeof(TowerDefenseZombiePortal).FullName, "_checkArea");
		}
		field.SetValue(portal, _captureArea);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[FirePeaPortalExit] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(5)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifyZombiePortalRelease, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "bulletField", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "portal", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "firePeaConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreatePortal, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new Godot.Bridge.MethodInfo(MethodName.EnableCaptureArea, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "portal", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.VerifyZombiePortalRelease && args.Count == 3)
		{
			VerifyZombiePortalRelease(VariantUtils.ConvertTo<BulletField>(in args[0]), VariantUtils.ConvertTo<FirePeaPortalExitZombiePortalStub>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePortal && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<FirePeaPortalExitZombiePortalStub>(CreatePortal());
			return true;
		}
		if (method == MethodName.EnableCaptureArea && args.Count == 1)
		{
			EnableCaptureArea(VariantUtils.ConvertTo<FirePeaPortalExitZombiePortalStub>(in args[0]));
			ret = default;
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
		if (method == MethodName.CreatePortal && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<FirePeaPortalExitZombiePortalStub>(CreatePortal());
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
		if (method == MethodName.VerifyZombiePortalRelease)
		{
			return true;
		}
		if (method == MethodName.CreatePortal)
		{
			return true;
		}
		if (method == MethodName.EnableCaptureArea)
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
		if (name == PropertyName._captureAreaManager)
		{
			_captureAreaManager = VariantUtils.ConvertTo<ComponentManager>(in value);
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
		if (name == PropertyName._captureAreaManager)
		{
			value = VariantUtils.CreateFrom(in _captureAreaManager);
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
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._captureAreaManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._captureAreaManager, Variant.From(in _captureAreaManager));
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
		if (info.TryGetProperty(PropertyName._captureAreaManager, out var value3))
		{
			_captureAreaManager = value3.As<ComponentManager>();
		}
	}
}
