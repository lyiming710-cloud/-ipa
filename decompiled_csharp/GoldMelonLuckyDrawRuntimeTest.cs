using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/GoldMelonLuckyDrawRuntimeTest.cs")]
public class GoldMelonLuckyDrawRuntimeTest : Node
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

	private const string GoldMelonPath = "res://Registry/Projectile/Config/Melon/GoldMelon.tres";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		try
		{
			TowerDefenseProjectileData towerDefenseProjectileData = ResourceLoader.Load<TowerDefenseProjectileData>("res://Registry/Projectile/Config/Melon/GoldMelon.tres", null, ResourceLoader.CacheMode.Ignore);
			Check(towerDefenseProjectileData != null, "GoldMelon projectile data must load.");
			Check(towerDefenseProjectileData != null && towerDefenseProjectileData.hitTargetEventList.Count == 1, "GoldMelon must have one hit-target lottery event.");
			TowerDefenseCharacterEventLuckyDraw towerDefenseCharacterEventLuckyDraw = ((towerDefenseProjectileData != null && towerDefenseProjectileData.hitTargetEventList.Count > 0) ? (towerDefenseProjectileData.hitTargetEventList[0] as TowerDefenseCharacterEventLuckyDraw) : null);
			Check(towerDefenseCharacterEventLuckyDraw != null, "GoldMelon hit event must be the lucky-draw event.");
			Check(towerDefenseCharacterEventLuckyDraw != null && towerDefenseCharacterEventLuckyDraw.eventList.Count == 6, "GoldMelon design contains exactly six documented prize outcomes.");
			if (towerDefenseCharacterEventLuckyDraw != null && towerDefenseCharacterEventLuckyDraw.eventList.Count == 6)
			{
				double[] array = new double[6] { 50.0, 30.0, 10.0, 6.0, 3.0, 1.0 };
				for (int i = 0; i < array.Length; i++)
				{
					TowerDefenseCharacterEventLuckyDrawItem towerDefenseCharacterEventLuckyDrawItem = towerDefenseCharacterEventLuckyDraw.eventList[i];
					Check(towerDefenseCharacterEventLuckyDrawItem?._event != null, $"Prize outcome {i + 1} must have an executable event.");
					Check(towerDefenseCharacterEventLuckyDrawItem != null && Math.Abs(towerDefenseCharacterEventLuckyDrawItem.weight - array[i]) < 0.0001, $"Prize outcome {i + 1} weight must remain {array[i]}%.");
				}
				double num = towerDefenseCharacterEventLuckyDraw.eventList.Sum((TowerDefenseCharacterEventLuckyDrawItem item) => item.weight);
				Check(Math.Abs(num - 100.0) < 0.0001, $"GoldMelon prize weights must total 100%; got {num}.");
				GoldMelonLuckyDrawRuntimeTest goldMelonLuckyDrawRuntimeTest = this;
				Array<TowerDefenseCharacterEventLuckyDrawItem> eventList = towerDefenseCharacterEventLuckyDraw.eventList;
				goldMelonLuckyDrawRuntimeTest.Check(eventList[eventList.Count - 1]._event is TowerDefenseCharacterEventHypnoses, "The documented 1% first prize must remain the hypnosis outcome.");
				Array<TowerDefenseCharacterEventLuckyDrawItem> array2 = new Array<TowerDefenseCharacterEventLuckyDrawItem>();
				GoldMelonLuckyDrawProbeEvent[] array3 = new GoldMelonLuckyDrawProbeEvent[array.Length];
				for (int num2 = 0; num2 < array.Length; num2++)
				{
					array3[num2] = new GoldMelonLuckyDrawProbeEvent();
					array2.Add(new TowerDefenseCharacterEventLuckyDrawItem
					{
						_event = array3[num2],
						weight = towerDefenseCharacterEventLuckyDraw.eventList[num2].weight
					});
				}
				GoldMelonLuckyDrawProbeProjectile goldMelonLuckyDrawProbeProjectile = new GoldMelonLuckyDrawProbeProjectile();
				AddChild(goldMelonLuckyDrawProbeProjectile, forceReadableName: false, InternalMode.Disabled);
				try
				{
					GD.Seed(20260717uL);
					for (int num3 = 0; num3 < 20000; num3++)
					{
						TowerDefenseCharacterEventLuckyDraw.Run(Vector2.Zero, null, array2, goldMelonLuckyDrawProbeProjectile);
					}
					Check(array3.Sum((GoldMelonLuckyDrawProbeEvent probe) => probe.ProjectDispatchCount) == 20000, "Every draw must dispatch exactly one projectile event.");
					Check(array3.All((GoldMelonLuckyDrawProbeEvent probe) => probe.ProjectDispatchCount > 0), "Every configured prize, including the 1% final item, must be reachable.");
					Check(array3[^1].ProjectDispatchCount > 0, "The 1% first-prize item must execute through the projectile dispatch path.");
					Check(array3.All((GoldMelonLuckyDrawProbeEvent probe) => probe.PositionDispatchCount == 0), "A valid projectile must use ExecuteProject rather than the position-only path.");
				}
				finally
				{
					goldMelonLuckyDrawProbeProjectile.QueueFree();
				}
			}
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[GoldMelonLuckyDrawRuntimeTest] Unexpected exception: {value}");
		}
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		bool flag = _failures == 0;
		GD.Print($"GOLD_MELON_LUCKY_DRAW_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[GoldMelonLuckyDrawRuntimeTest] " + message);
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
