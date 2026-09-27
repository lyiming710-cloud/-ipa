using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/Npc/CrazyDave/NpcCrazyDave.cs")]
public class NpcCrazyDave : NpcBase
{
	public new class MethodName : NpcBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName Talk = "Talk";

		public new static readonly StringName Hand = "Hand";

		public new static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : NpcBase.PropertyName
	{
		public static readonly StringName _handSlot = "_handSlot";

		public static readonly StringName _shoulderSlot = "_shoulderSlot";

		public static readonly StringName _headSlot = "_headSlot";
	}

	public new class SignalName : NpcBase.SignalName
	{
	}

	private AdobeAnimateSlot _handSlot;

	private AdobeAnimateSlot _shoulderSlot;

	private AdobeAnimateSlot _headSlot;

	public override void _Ready()
	{
		_handSlot = GetNode<AdobeAnimateSlot>("%HandSlot");
		_shoulderSlot = GetNode<AdobeAnimateSlot>("%ShoulderSlot");
		_headSlot = GetNode<AdobeAnimateSlot>("%HeadSlot");
		base._Ready();
	}

	public override void Talk(string text, string animeClip, string audio)
	{
		base.Talk(text, animeClip, audio);
		if (IsLeaveAnimation(animeClip))
		{
			return;
		}
		foreach (Node child in _handSlot.GetChildren())
		{
			child.QueueFree();
		}
	}

	public override void Hand(NpcTalkHandConfig hand)
	{
		if (hand.handScene != null)
		{
			Node node = hand.handScene.Instantiate(PackedScene.GenEditState.Disabled);
			_handSlot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		}
		if (hand.shoulderScene != null)
		{
			Node node2 = hand.shoulderScene.Instantiate(PackedScene.GenEditState.Disabled);
			if (node2 is Node2D node2D)
			{
				node2D.RotationDegrees = -45f;
			}
			_shoulderSlot.AddChild(node2, forceReadableName: false, InternalMode.Disabled);
		}
		if (hand.shoulder2Scene != null)
		{
			Node node3 = hand.shoulder2Scene.Instantiate(PackedScene.GenEditState.Disabled);
			if (node3 is Node2D node2D2)
			{
				node2D2.Position = new Vector2(274f, -8f);
			}
			if (node3 is Node2D node2D3)
			{
				node2D3.RotationDegrees = 45f;
			}
			_shoulderSlot.AddChild(node3, forceReadableName: false, InternalMode.Disabled);
		}
		if (hand.headScene != null)
		{
			Node node4 = hand.headScene.Instantiate(PackedScene.GenEditState.Disabled);
			if (node4 is Node2D node2D4)
			{
				node2D4.RotationDegrees = -12f;
			}
			_headSlot.AddChild(node4, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public override void Finish()
	{
		foreach (Node child in _handSlot.GetChildren())
		{
			child.QueueFree();
		}
		base.Finish();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Talk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "animeClip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "audio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Hand, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "hand", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Talk && args.Count == 3)
		{
			Talk(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.Hand && args.Count == 1)
		{
			Hand(VariantUtils.ConvertTo<NpcTalkHandConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
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
		if (method == MethodName.Talk)
		{
			return true;
		}
		if (method == MethodName.Hand)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._handSlot)
		{
			_handSlot = VariantUtils.ConvertTo<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._shoulderSlot)
		{
			_shoulderSlot = VariantUtils.ConvertTo<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._headSlot)
		{
			_headSlot = VariantUtils.ConvertTo<AdobeAnimateSlot>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._handSlot)
		{
			value = VariantUtils.CreateFrom(in _handSlot);
			return true;
		}
		if (name == PropertyName._shoulderSlot)
		{
			value = VariantUtils.CreateFrom(in _shoulderSlot);
			return true;
		}
		if (name == PropertyName._headSlot)
		{
			value = VariantUtils.CreateFrom(in _headSlot);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._handSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shoulderSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._headSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._handSlot, Variant.From(in _handSlot));
		info.AddProperty(PropertyName._shoulderSlot, Variant.From(in _shoulderSlot));
		info.AddProperty(PropertyName._headSlot, Variant.From(in _headSlot));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._handSlot, out var value))
		{
			_handSlot = value.As<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._shoulderSlot, out var value2))
		{
			_shoulderSlot = value2.As<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._headSlot, out var value3))
		{
			_headSlot = value3.As<AdobeAnimateSlot>();
		}
	}
}
