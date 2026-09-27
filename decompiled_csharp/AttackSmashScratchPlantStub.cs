using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class AttackSmashScratchPlantStub : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName SmashHurt = "SmashHurt";

		public new static readonly StringName AttackDeal = "AttackDeal";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName SmashHurtCalls = "SmashHurtCalls";

		public static readonly StringName AttackDealCalls = "AttackDealCalls";

		public static readonly StringName ThrowOnSmash = "ThrowOnSmash";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	public int SmashHurtCalls;

	public int AttackDealCalls;

	public bool ThrowOnSmash;

	public Action OnAttackDeal;

	public override double SmashHurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		SmashHurtCalls++;
		if (ThrowOnSmash)
		{
			throw new InvalidOperationException("intentional smash scratch exception");
		}
		return num;
	}

	public override void AttackDeal(TowerDefenseCharacter character, string type, double num)
	{
		AttackDealCalls++;
		OnAttackDeal?.Invoke();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.SmashHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackDeal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SmashHurt && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(SmashHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.AttackDeal && args.Count == 3)
		{
			AttackDeal(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SmashHurt)
		{
			return true;
		}
		if (method == MethodName.AttackDeal)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.SmashHurtCalls)
		{
			SmashHurtCalls = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.AttackDealCalls)
		{
			AttackDealCalls = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ThrowOnSmash)
		{
			ThrowOnSmash = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.SmashHurtCalls)
		{
			value = VariantUtils.CreateFrom(in SmashHurtCalls);
			return true;
		}
		if (name == PropertyName.AttackDealCalls)
		{
			value = VariantUtils.CreateFrom(in AttackDealCalls);
			return true;
		}
		if (name == PropertyName.ThrowOnSmash)
		{
			value = VariantUtils.CreateFrom(in ThrowOnSmash);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.SmashHurtCalls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AttackDealCalls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ThrowOnSmash, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.SmashHurtCalls, Variant.From(in SmashHurtCalls));
		info.AddProperty(PropertyName.AttackDealCalls, Variant.From(in AttackDealCalls));
		info.AddProperty(PropertyName.ThrowOnSmash, Variant.From(in ThrowOnSmash));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.SmashHurtCalls, out var value))
		{
			SmashHurtCalls = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.AttackDealCalls, out var value2))
		{
			AttackDealCalls = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ThrowOnSmash, out var value3))
		{
			ThrowOnSmash = value3.As<bool>();
		}
	}
}
