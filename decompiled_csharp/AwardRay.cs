using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/Particles/Award/AwardRay.cs")]
public class AwardRay : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Emit = "Emit";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _awardRayFour = "_awardRayFour";

		public static readonly StringName _awardRayGlow = "_awardRayGlow";

		public static readonly StringName _awardRay1 = "_awardRay1";

		public static readonly StringName _awardRay2 = "_awardRay2";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private GpuParticles2D _awardRayFour;

	private GpuParticles2D _awardRayGlow;

	private GpuParticles2D _awardRay1;

	private GpuParticles2D _awardRay2;

	public override void _Ready()
	{
		_awardRayFour = GetNode<GpuParticles2D>("%AwardRayFour");
		_awardRayGlow = GetNode<GpuParticles2D>("%AwardRayGlow");
		_awardRay1 = GetNode<GpuParticles2D>("%AwardRay1");
		_awardRay2 = GetNode<GpuParticles2D>("%AwardRay2");
	}

	public async void Emit()
	{
		_awardRayFour.Restart();
		_awardRay1.Restart();
		_awardRay2.Restart();
		await ToSignal(GetTree().CreateTimer(5.0, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		_awardRayGlow.Restart();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Emit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Emit && args.Count == 0)
		{
			Emit();
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
		if (method == MethodName.Emit)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._awardRayFour)
		{
			_awardRayFour = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._awardRayGlow)
		{
			_awardRayGlow = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._awardRay1)
		{
			_awardRay1 = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._awardRay2)
		{
			_awardRay2 = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._awardRayFour)
		{
			value = VariantUtils.CreateFrom(in _awardRayFour);
			return true;
		}
		if (name == PropertyName._awardRayGlow)
		{
			value = VariantUtils.CreateFrom(in _awardRayGlow);
			return true;
		}
		if (name == PropertyName._awardRay1)
		{
			value = VariantUtils.CreateFrom(in _awardRay1);
			return true;
		}
		if (name == PropertyName._awardRay2)
		{
			value = VariantUtils.CreateFrom(in _awardRay2);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._awardRayFour, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._awardRayGlow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._awardRay1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._awardRay2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._awardRayFour, Variant.From(in _awardRayFour));
		info.AddProperty(PropertyName._awardRayGlow, Variant.From(in _awardRayGlow));
		info.AddProperty(PropertyName._awardRay1, Variant.From(in _awardRay1));
		info.AddProperty(PropertyName._awardRay2, Variant.From(in _awardRay2));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._awardRayFour, out var value))
		{
			_awardRayFour = value.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._awardRayGlow, out var value2))
		{
			_awardRayGlow = value2.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._awardRay1, out var value3))
		{
			_awardRay1 = value3.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._awardRay2, out var value4))
		{
			_awardRay2 = value4.As<GpuParticles2D>();
		}
	}
}
