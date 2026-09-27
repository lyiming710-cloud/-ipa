using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Event/Resource/Protal/TowerDefenseLevelEventChangeProtalPos.cs")]
public class TowerDefenseLevelEventChangeProtalPos : TowerDefenseLevelEventBase
{
	public new class MethodName : TowerDefenseLevelEventBase.MethodName
	{
		public new static readonly StringName _GetName = "_GetName";

		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName Export = "Export";

		public static readonly StringName GetPortalFeature = "GetPortalFeature";
	}

	public new class PropertyName : TowerDefenseLevelEventBase.PropertyName
	{
	}

	public new class SignalName : TowerDefenseLevelEventBase.SignalName
	{
	}

	public override string _GetName()
	{
		return "LEVLE_EVENT_CHAGE_PROTAL_POS";
	}

	public override void Execute()
	{
		if (Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			return;
		}
		TowerDefenseBattleFeaturePortal portalFeature = GetPortalFeature();
		if (portalFeature == null)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				currentControl.AddFeature("Portal", new Dictionary());
				portalFeature = GetPortalFeature();
			}
		}
		if (GodotObject.IsInstanceValid(portalFeature))
		{
			portalFeature.PortalChangePos();
		}
	}

	public override Dictionary Export()
	{
		return new Dictionary
		{
			["EventName"] = "ChangeProtalPos",
			["Value"] = new Dictionary()
		};
	}

	public TowerDefenseBattleFeaturePortal GetPortalFeature()
	{
		TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
		if (GodotObject.IsInstanceValid(currentControl))
		{
			return currentControl.GetFeature("Portal") as TowerDefenseBattleFeaturePortal;
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._GetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPortalFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._GetName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetName());
			return true;
		}
		if (method == MethodName.Execute && args.Count == 0)
		{
			Execute();
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.GetPortalFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeaturePortal>(GetPortalFeature());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._GetName)
		{
			return true;
		}
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.GetPortalFeature)
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
