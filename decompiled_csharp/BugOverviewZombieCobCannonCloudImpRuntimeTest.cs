using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewZombieCobCannonCloudImpRuntimeTest.cs")]
public class BugOverviewZombieCobCannonCloudImpRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

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

	private const string CannonScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/CobCannon/TowerDefenseZombieNormalCobCannon.tscn";

	private const string CloudImpScenePath = "res://Asset/Anime/Character/Zombie/Chapter8/ImpCloud/Scene/TowerDefenseZombieImpCloud.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		ZombieCobCannonCloudImpControlStub control = null;
		TowerDefenseZombieNormalCobCannon cannonZombie = null;
		TowerDefenseZombieImpCloud cloudImp = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00ce;
				}
				control = new ZombieCobCannonCloudImpControlStub
				{
					Name = "ZombieCobCannonCloudImpControl",
					isGameRunning = true,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = Vector2.Zero;
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				cannonZombie = Instantiate<TowerDefenseZombieNormalCobCannon>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/CobCannon/TowerDefenseZombieNormalCobCannon.tscn");
				cloudImp = Instantiate<TowerDefenseZombieImpCloud>("res://Asset/Anime/Character/Zombie/Chapter8/ImpCloud/Scene/TowerDefenseZombieImpCloud.tscn");
				Check(GodotObject.IsInstanceValid(cannonZombie), "The real zombie cob cannon scene must instantiate.");
				Check(GodotObject.IsInstanceValid(cloudImp), "The real Cloud Imp scene must instantiate.");
				if (!GodotObject.IsInstanceValid(cannonZombie) || !GodotObject.IsInstanceValid(cloudImp))
				{
					goto end_IL_00ce;
				}
				cannonZombie.editorPreviewMode = true;
				cannonZombie.inGame = true;
				cannonZombie.GlobalPosition = new Vector2(700f, 152f);
				cannonZombie.gridPos = new Vector2I(7, 2);
				cloudImp.editorPreviewMode = true;
				cloudImp.inGame = true;
				cloudImp.GlobalPosition = new Vector2(420f, 152f);
				cloudImp.gridPos = new Vector2I(4, 2);
				control.characterNode.AddChild(cannonZombie, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(cloudImp, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(5);
				cannonZombie.camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT;
				await WaitFrames(2);
				CannonComponent cannon = cannonZombie.componentManager?.GetRuntime<CannonComponent>("character.cannon");
				Check(cannon != null && !cannon.IsReleased, "The real zombie cob cannon must expose its CannonComponent runtime.");
				BugOverviewZombieCobCannonCloudImpRuntimeTest bugOverviewZombieCobCannonCloudImpRuntimeTest = this;
				TowerDefenseCharacterConfig config = cloudImp.config;
				bugOverviewZombieCobCannonCloudImpRuntimeTest.Check(config != null && config.collisionFlags == 2, "The real Cloud Imp must keep its authored air collision flag.");
				Check(cannon?.projectileData != null && (cannon.projectileData.collisionFlags & cloudImp.config.collisionFlags) != 0, "The real zombie cob projectile must be able to collide with the Cloud Imp's air flag.");
				Check(cannonZombie.camp != cloudImp.camp, "The player-side hypnotized zombie cannon and hostile Cloud Imp must be opposing camps.");
				if (cannon == null || cannon.IsReleased)
				{
					goto end_IL_00ce;
				}
				cannon.autoAttack = true;
				cannon.markerVisualTravelDuration = 0.0;
				cannon.canFire = true;
				cannon.PhysicsProcess(1.0 / 60.0, Engine.GetPhysicsFrames());
				await WaitFrames(1);
				Check(!cannon.canFire, "The zombie cob cannon must consume its ready shot when Cloud Imp is the only enemy.");
				Check(cannon.targetPos.IsEqualApprox(cloudImp.GlobalPosition), $"The zombie cob cannon must select the real Cloud Imp position; got {cannon.targetPos}.");
				goto end_IL_00b7;
				end_IL_00ce:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewZombieCobCannonCloudImpRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00b7;
			}
			return;
			end_IL_00b7:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(cannonZombie))
			{
				cannonZombie.QueueFree();
			}
			if (GodotObject.IsInstanceValid(cloudImp))
			{
				cloudImp.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(2);
		}
		bool flag = _failures == 0 && _checks == 9;
		GD.Print($"ZOMBIE_COB_CANNON_CLOUD_IMP_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static T Instantiate<T>(string path) where T : Node
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
	}

	private async Task WaitFrames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewZombieCobCannonCloudImpRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
