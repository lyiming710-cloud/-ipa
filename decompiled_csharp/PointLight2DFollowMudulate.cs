using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Extends/PointLight2D/PointLight2DFollowMudulate.cs")]
public class PointLight2DFollowMudulate : PointLight2D
{
	public new class MethodName : PointLight2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";
	}

	public new class PropertyName : PointLight2D.PropertyName
	{
		public static readonly StringName followNode = "followNode";

		public static readonly StringName saveEnergy = "saveEnergy";
	}

	public new class SignalName : PointLight2D.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public CanvasItem followNode { get; set; }

	[Export(PropertyHint.None, "")]
	public float saveEnergy { get; set; } = 1.5f;

	public override void _Ready()
	{
		saveEnergy = Energy;
		Energy = 0f;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Engine.IsEditorHint())
		{
			Visible = true;
			if (!GameSaveManager.Instance.GetConfigValue("MapEffect").AsBool())
			{
				Visible = false;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.followNode)
		{
			followNode = VariantUtils.ConvertTo<CanvasItem>(in value);
			return true;
		}
		if (name == PropertyName.saveEnergy)
		{
			saveEnergy = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.followNode)
		{
			value = VariantUtils.CreateFrom<CanvasItem>(followNode);
			return true;
		}
		if (name == PropertyName.saveEnergy)
		{
			value = VariantUtils.CreateFrom<float>(saveEnergy);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.followNode, PropertyHint.NodeType, "CanvasItem", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.saveEnergy, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.followNode, Variant.From<CanvasItem>(followNode));
		info.AddProperty(PropertyName.saveEnergy, Variant.From<float>(saveEnergy));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.followNode, out var value))
		{
			followNode = value.As<CanvasItem>();
		}
		if (info.TryGetProperty(PropertyName.saveEnergy, out var value2))
		{
			saveEnergy = value2.As<float>();
		}
	}
}
