using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Behavior/Card/Action/CardActionBehaviorDefinition.cs")]
public class CardActionBehaviorDefinition : CardBehaviorDefinition
{
	private sealed class ActionRuntime : CardBehaviorRuntime
	{
		private readonly CardActionBehaviorDefinition _definition;

		private readonly CardActionBehaviorTrigger _trigger;

		public ActionRuntime(CardActionBehaviorDefinition definition, CardActionBehaviorTrigger trigger)
		{
			_definition = definition;
			_trigger = trigger;
		}

		public override void OnPressed()
		{
			if ((_trigger & CardActionBehaviorTrigger.Pressed) != 0)
			{
				_definition.ExecuteAction(Owner);
			}
		}

		public override void OnUseSucceeded(TowerDefenseCharacter createdCharacter)
		{
			if ((_trigger & CardActionBehaviorTrigger.UseSucceeded) != 0)
			{
				_definition.ExecuteAction(Owner);
			}
		}
	}

	public new class MethodName : CardBehaviorDefinition.MethodName
	{
		public static readonly StringName ImportConfiguration = "ImportConfiguration";

		public static readonly StringName ExecuteAction = "ExecuteAction";

		public static readonly StringName ExportConfiguration = "ExportConfiguration";

		public static readonly StringName ReadLegacyArray = "ReadLegacyArray";
	}

	public new class PropertyName : CardBehaviorDefinition.PropertyName
	{
		public static readonly StringName triggerFlags = "triggerFlags";
	}

	public new class SignalName : CardBehaviorDefinition.SignalName
	{
	}

	[Export(PropertyHint.Flags, "Pressed,UseSucceeded")]
	public int triggerFlags = 2;

	public virtual void ImportConfiguration(Dictionary data)
	{
	}

	public virtual void ExecuteAction(TowerDefenseInGamePacketShow packet)
	{
	}

	public virtual Dictionary ExportConfiguration()
	{
		return new Dictionary();
	}

	public static Array<CardActionBehaviorDefinition> ReadLegacyArray(Variant value, CardActionBehaviorTrigger trigger)
	{
		Array<CardActionBehaviorDefinition> array = new Array<CardActionBehaviorDefinition>();
		if (value.VariantType != Variant.Type.Array)
		{
			return array;
		}
		foreach (Variant item in value.AsGodotArray())
		{
			if (item.VariantType == Variant.Type.Object && item.AsGodotObject() is CardActionBehaviorDefinition cardActionBehaviorDefinition)
			{
				cardActionBehaviorDefinition.triggerFlags = (int)trigger;
				array.Add(cardActionBehaviorDefinition);
			}
		}
		return array;
	}

	public override CardBehaviorRuntime CreateRuntime()
	{
		return CreateRuntimeForTrigger((CardActionBehaviorTrigger)triggerFlags);
	}

	internal CardBehaviorRuntime CreateRuntimeForTrigger(CardActionBehaviorTrigger trigger)
	{
		return new ActionRuntime(this, trigger);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.ImportConfiguration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExportConfiguration, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadLegacyArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Int, "trigger", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ImportConfiguration && args.Count == 1)
		{
			ImportConfiguration(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteAction && args.Count == 1)
		{
			ExecuteAction(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportConfiguration && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportConfiguration());
			return true;
		}
		if (method == MethodName.ReadLegacyArray && args.Count == 2)
		{
			Array<CardActionBehaviorDefinition> array = ReadLegacyArray(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<CardActionBehaviorTrigger>(in args[1]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ReadLegacyArray && args.Count == 2)
		{
			Array<CardActionBehaviorDefinition> array = ReadLegacyArray(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<CardActionBehaviorTrigger>(in args[1]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ImportConfiguration)
		{
			return true;
		}
		if (method == MethodName.ExecuteAction)
		{
			return true;
		}
		if (method == MethodName.ExportConfiguration)
		{
			return true;
		}
		if (method == MethodName.ReadLegacyArray)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.triggerFlags)
		{
			triggerFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.triggerFlags)
		{
			value = VariantUtils.CreateFrom(in triggerFlags);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.triggerFlags, PropertyHint.Flags, "Pressed,UseSucceeded", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.triggerFlags, Variant.From(in triggerFlags));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.triggerFlags, out var value))
		{
			triggerFlags = value.As<int>();
		}
	}
}
