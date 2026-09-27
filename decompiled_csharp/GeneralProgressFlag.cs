using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/ProgressMeter/ProgressFlag/GeneralProgressFlag.cs")]
public class GeneralProgressFlag : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetReach = "SetReach";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _pos = "_pos";

		public static readonly StringName reach = "reach";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const float SettledDistance = 0.05f;

	private Node2D _pos;

	[Export(PropertyHint.None, "")]
	public bool reach;

	public override void _Ready()
	{
		_pos = GetNode<Node2D>("%Pos");
		SetPhysicsProcess(enable: true);
	}

	public void SetReach(bool value)
	{
		if (reach == value && GodotObject.IsInstanceValid(_pos))
		{
			float num = (value ? 45f : 55f);
			if (Mathf.Abs(_pos.Position.Y - num) <= 0.05f)
			{
				return;
			}
		}
		reach = value;
		SetPhysicsProcess(enable: true);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_pos != null)
		{
			float num = (reach ? 45f : 55f);
			float num2 = Mathf.Lerp(_pos.Position.Y, num, 1f - Mathf.Exp(0f - (float)delta));
			if (Mathf.Abs(num2 - num) <= 0.05f)
			{
				num2 = num;
				SetPhysicsProcess(enable: false);
			}
			_pos.Position = new Vector2(_pos.Position.X, num2);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.SetReach && args.Count == 1)
		{
			SetReach(VariantUtils.ConvertTo<bool>(in args[0]));
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
		if (method == MethodName.SetReach)
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
		if (name == PropertyName._pos)
		{
			_pos = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.reach)
		{
			reach = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._pos)
		{
			value = VariantUtils.CreateFrom(in _pos);
			return true;
		}
		if (name == PropertyName.reach)
		{
			value = VariantUtils.CreateFrom(in reach);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._pos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.reach, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._pos, Variant.From(in _pos));
		info.AddProperty(PropertyName.reach, Variant.From(in reach));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._pos, out var value))
		{
			_pos = value.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.reach, out var value2))
		{
			reach = value2.As<bool>();
		}
	}
}
