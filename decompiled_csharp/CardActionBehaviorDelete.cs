using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Behavior/Card/Action/CardActionBehaviorDelete.cs")]
public class CardActionBehaviorDelete : CardActionBehaviorDefinition
{
	public new class MethodName : CardActionBehaviorDefinition.MethodName
	{
		public new static readonly StringName ImportConfiguration = "ImportConfiguration";

		public new static readonly StringName ExecuteAction = "ExecuteAction";

		public new static readonly StringName ExportConfiguration = "ExportConfiguration";
	}

	public new class PropertyName : CardActionBehaviorDefinition.PropertyName
	{
	}

	public new class SignalName : CardActionBehaviorDefinition.SignalName
	{
	}

	public override void ImportConfiguration(Dictionary data)
	{
	}

	public override void ExecuteAction(TowerDefenseInGamePacketShow packet)
	{
		TowerDefenseManager.Instance.GetSeedBank().packetList.Remove(packet);
		packet.QueueFree();
	}

	public override Dictionary ExportConfiguration()
	{
		return new Dictionary
		{
			["ActionId"] = "Delete",
			["Configuration"] = new Dictionary()
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.ImportConfiguration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExportConfiguration, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
