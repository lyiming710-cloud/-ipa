using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/IceSunShroomBalloonColourSlowRuntimeTest.cs")]
public class IceSunShroomBalloonColourSlowRuntimeTest : Node
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

	private const string BalloonColourConfigPath = "res://Asset/Anime/Character/Zombie/Chapter1/BalloonColour/Config/TowerDefenseZombieBalloonColour.tres";

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		TowerDefenseCharacter towerDefenseCharacter = null;
		TowerDefenseCharacterInstance towerDefenseCharacterInstance = null;
		try
		{
			int num = 2;
			int num2 = 2;
			int num3 = 4;
			int num4 = 1;
			string fileAsString = FileAccess.GetFileAsString("res://Asset/Anime/Character/Zombie/Chapter1/BalloonColour/Config/TowerDefenseZombieBalloonColour.tres");
			int num5 = num2 | num3;
			Check(fileAsString.Contains($"maskFlags = {num}") && fileAsString.Contains($"unUseBuffFlags = {num5}"), "ZombieBalloonColour config must retain its airborne mask and authored Buff immunities.");
			towerDefenseCharacter = new TowerDefenseCharacter();
			towerDefenseCharacterInstance = (towerDefenseCharacter.instance = new TowerDefenseCharacterInstance
			{
				maskFlags = num,
				unUseBuffFlags = num5
			});
			towerDefenseCharacter.buff = new BuffComponent
			{
				parent = towerDefenseCharacter
			};
			Check((towerDefenseCharacterInstance.maskFlags & num) != 0, "The fixture must still be an airborne ZombieBalloonColour.");
			Check((towerDefenseCharacterInstance.unUseBuffFlags & num2) != 0, "ZombieBalloonColour must retain its authored Frozen immunity.");
			Check((towerDefenseCharacterInstance.unUseBuffFlags & num4) == 0, "ZombieBalloonColour must remain eligible for IceSpeedDown.");
			Array<TowerDefenseCharacterEventBase> array = TowerDefenseCharacter.CreateSnowEventList(new Array<TowerDefenseCharacterEventBase>());
			TowerDefenseCharacterEventForzen towerDefenseCharacterEventForzen = null;
			foreach (TowerDefenseCharacterEventBase item in array)
			{
				if (item is TowerDefenseCharacterEventForzen towerDefenseCharacterEventForzen2)
				{
					towerDefenseCharacterEventForzen = towerDefenseCharacterEventForzen2;
					break;
				}
			}
			Check(towerDefenseCharacterEventForzen != null, "The Ice Sun-shroom cold pipeline must include its Frozen event.");
			if (towerDefenseCharacterEventForzen == null)
			{
				return;
			}
			towerDefenseCharacterEventForzen.Execute(towerDefenseCharacter.GetLogicalGlobalPosition(), towerDefenseCharacter);
			Check(!towerDefenseCharacter.buff.BuffHas("Frozen"), "Airborne ZombieBalloonColour must still reject Frozen.");
			Check(towerDefenseCharacter.buff.BuffHas("IceSpeedDown"), "Rejected Ice Sun-shroom Frozen must fall back to IceSpeedDown.");
			towerDefenseCharacter.timeScale = 1.0;
			towerDefenseCharacter.buff.BuffUpdate(0.25);
			Check(Math.Abs(towerDefenseCharacter.timeScale - 0.5) < 0.0001, $"IceSpeedDown must halve ZombieBalloonColour timeScale; got {towerDefenseCharacter.timeScale}.");
			towerDefenseCharacterEventForzen.Execute(towerDefenseCharacter.GetLogicalGlobalPosition(), towerDefenseCharacter);
			Check(!towerDefenseCharacter.buff.BuffHas("Frozen") && towerDefenseCharacter.buff.BuffHas("IceSpeedDown"), "Repeated Ice Sun-shroom effects must refresh only IceSpeedDown while airborne.");
			TowerDefenseCharacterBuffIceSpeedDown towerDefenseCharacterBuffIceSpeedDown = towerDefenseCharacter.buff.BuffGet("IceSpeedDown") as TowerDefenseCharacterBuffIceSpeedDown;
			Check(towerDefenseCharacterBuffIceSpeedDown != null && Math.Abs(towerDefenseCharacterBuffIceSpeedDown.currentTime) < 0.0001, "Repeated cold application must refresh the active slowdown timer.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[IceSunShroomBalloonColourSlowRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				towerDefenseCharacter.buff?.BuffClear();
				towerDefenseCharacter.Free();
			}
			towerDefenseCharacterInstance?.Dispose();
		}
		bool flag = _failures == 0;
		GD.Print($"ICE_SUN_SHROOM_BALLOON_COLOUR_SLOW_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[IceSunShroomBalloonColourSlowRuntimeTest] " + message);
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
