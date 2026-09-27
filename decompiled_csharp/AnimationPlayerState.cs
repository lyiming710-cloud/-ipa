using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[Icon("res://addons/godot_state_charts/animation_player_state.svg")]
[ScriptPath("res://addons/godot_state_charts/AnimationPlayerState.cs")]
public class AnimationPlayerState : AtomicState
{
	public new class MethodName : AtomicState.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _StateEnter = "_StateEnter";

		public new static readonly StringName _GetConfigurationWarnings = "_GetConfigurationWarnings";
	}

	public new class PropertyName : AtomicState.PropertyName
	{
		public static readonly StringName animationPlayer = "animationPlayer";

		public static readonly StringName _animationPlayerPath = "_animationPlayerPath";

		public static readonly StringName animationName = "animationName";

		public static readonly StringName customBlend = "customBlend";

		public static readonly StringName customSpeed = "customSpeed";

		public static readonly StringName fromEnd = "fromEnd";

		public static readonly StringName _animationPlayer = "_animationPlayer";
	}

	public new class SignalName : AtomicState.SignalName
	{
	}

	private NodePath _animationPlayerPath = new NodePath();

	[Export(PropertyHint.None, "")]
	public StringName animationName = "";

	[Export(PropertyHint.None, "")]
	public float customBlend = -1f;

	[Export(PropertyHint.None, "")]
	public float customSpeed = 1f;

	[Export(PropertyHint.None, "")]
	public bool fromEnd;

	private AnimationPlayer _animationPlayer;

	[Export(PropertyHint.None, "")]
	public NodePath animationPlayer
	{
		get
		{
			return _animationPlayerPath;
		}
		set
		{
			_animationPlayerPath = value;
			UpdateConfigurationWarnings();
		}
	}

	public override void _Ready()
	{
		if (!Engine.IsEditorHint())
		{
			base._Ready();
			_animationPlayer = GetNodeOrNull<AnimationPlayer>(_animationPlayerPath);
			if (!GodotObject.IsInstanceValid(_animationPlayer))
			{
				GD.PushError("The animation player is invalid. This node will not work.");
			}
		}
	}

	public override void _StateEnter(StateChartState transitionTarget)
	{
		base._StateEnter(transitionTarget);
		if (GodotObject.IsInstanceValid(_animationPlayer))
		{
			StringName name = animationName;
			if (name == (StringName)"")
			{
				name = Name;
			}
			if (!(_animationPlayer.CurrentAnimation == name) || !_animationPlayer.IsPlaying())
			{
				_animationPlayer.Play(name, customBlend, customSpeed, fromEnd);
			}
		}
	}

	public override string[] _GetConfigurationWarnings()
	{
		List<string> list = new List<string>(base._GetConfigurationWarnings());
		list.Add("This node is deprecated and will be removed in a future version.");
		if (_animationPlayerPath == null || _animationPlayerPath == (NodePath)"")
		{
			list.Add("No animation player is set.");
		}
		else if (GetNodeOrNull(_animationPlayerPath) == null)
		{
			list.Add("The animation player path is invalid.");
		}
		return list.ToArray();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._StateEnter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transitionTarget", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._GetConfigurationWarnings, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._StateEnter && args.Count == 1)
		{
			_StateEnter(VariantUtils.ConvertTo<StateChartState>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(_GetConfigurationWarnings());
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
		if (method == MethodName._StateEnter)
		{
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.animationPlayer)
		{
			animationPlayer = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName._animationPlayerPath)
		{
			_animationPlayerPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.animationName)
		{
			animationName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.customBlend)
		{
			customBlend = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.customSpeed)
		{
			customSpeed = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.fromEnd)
		{
			fromEnd = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._animationPlayer)
		{
			_animationPlayer = VariantUtils.ConvertTo<AnimationPlayer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.animationPlayer)
		{
			value = VariantUtils.CreateFrom<NodePath>(animationPlayer);
			return true;
		}
		if (name == PropertyName._animationPlayerPath)
		{
			value = VariantUtils.CreateFrom(in _animationPlayerPath);
			return true;
		}
		if (name == PropertyName.animationName)
		{
			value = VariantUtils.CreateFrom(in animationName);
			return true;
		}
		if (name == PropertyName.customBlend)
		{
			value = VariantUtils.CreateFrom(in customBlend);
			return true;
		}
		if (name == PropertyName.customSpeed)
		{
			value = VariantUtils.CreateFrom(in customSpeed);
			return true;
		}
		if (name == PropertyName.fromEnd)
		{
			value = VariantUtils.CreateFrom(in fromEnd);
			return true;
		}
		if (name == PropertyName._animationPlayer)
		{
			value = VariantUtils.CreateFrom(in _animationPlayer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.NodePath, PropertyName.animationPlayer, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName._animationPlayerPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.animationName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.customBlend, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.customSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.fromEnd, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.animationPlayer, Variant.From<NodePath>(animationPlayer));
		info.AddProperty(PropertyName._animationPlayerPath, Variant.From(in _animationPlayerPath));
		info.AddProperty(PropertyName.animationName, Variant.From(in animationName));
		info.AddProperty(PropertyName.customBlend, Variant.From(in customBlend));
		info.AddProperty(PropertyName.customSpeed, Variant.From(in customSpeed));
		info.AddProperty(PropertyName.fromEnd, Variant.From(in fromEnd));
		info.AddProperty(PropertyName._animationPlayer, Variant.From(in _animationPlayer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.animationPlayer, out var value))
		{
			animationPlayer = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName._animationPlayerPath, out var value2))
		{
			_animationPlayerPath = value2.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.animationName, out var value3))
		{
			animationName = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.customBlend, out var value4))
		{
			customBlend = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.customSpeed, out var value5))
		{
			customSpeed = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.fromEnd, out var value6))
		{
			fromEnd = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._animationPlayer, out var value7))
		{
			_animationPlayer = value7.As<AnimationPlayer>();
		}
	}
}
