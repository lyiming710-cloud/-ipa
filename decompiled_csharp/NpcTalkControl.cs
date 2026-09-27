using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/NpcTalk/Control/NpcTalkControl.cs")]
public class NpcTalkControl : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName AddNpc = "AddNpc";

		public static readonly StringName ShowTalk = "ShowTalk";

		public static readonly StringName ShowHand = "ShowHand";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName npcTalkFeature = "npcTalkFeature";

		public static readonly StringName _npcRenderMount = "_npcRenderMount";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	public TowerDefenseBattleFeatureNpcTalk npcTalkFeature;

	private Control _npcRenderMount;

	public override void _Ready()
	{
		_npcRenderMount = GetNode<Control>("%NpcRenderMount");
	}

	public void AddNpc(NpcBase npc)
	{
		if (GodotObject.IsInstanceValid(npc))
		{
			if (!GodotObject.IsInstanceValid(_npcRenderMount))
			{
				_npcRenderMount = GetNodeOrNull<Control>("%NpcRenderMount");
			}
			if (GodotObject.IsInstanceValid(npc.sprite) && GodotObject.IsInstanceValid(_npcRenderMount))
			{
				npc.sprite.forceLocalRender = true;
				npc.sprite.SetRenderClipControl(_npcRenderMount);
			}
			AddChild(npc, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public void ShowTalk(NpcBase npc, NpcTalkBaseConfig talk)
	{
		npc.Talk(talk.text, talk.anime, talk.audio);
	}

	public void ShowHand(NpcBase npc, NpcTalkHandConfig talk)
	{
		npc.Hand(talk);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddNpc, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "npc", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowTalk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "npc", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowHand, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "npc", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.AddNpc && args.Count == 1)
		{
			AddNpc(VariantUtils.ConvertTo<NpcBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowTalk && args.Count == 2)
		{
			ShowTalk(VariantUtils.ConvertTo<NpcBase>(in args[0]), VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowHand && args.Count == 2)
		{
			ShowHand(VariantUtils.ConvertTo<NpcBase>(in args[0]), VariantUtils.ConvertTo<NpcTalkHandConfig>(in args[1]));
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
		if (method == MethodName.AddNpc)
		{
			return true;
		}
		if (method == MethodName.ShowTalk)
		{
			return true;
		}
		if (method == MethodName.ShowHand)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.npcTalkFeature)
		{
			npcTalkFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureNpcTalk>(in value);
			return true;
		}
		if (name == PropertyName._npcRenderMount)
		{
			_npcRenderMount = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.npcTalkFeature)
		{
			value = VariantUtils.CreateFrom(in npcTalkFeature);
			return true;
		}
		if (name == PropertyName._npcRenderMount)
		{
			value = VariantUtils.CreateFrom(in _npcRenderMount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.npcTalkFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._npcRenderMount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.npcTalkFeature, Variant.From(in npcTalkFeature));
		info.AddProperty(PropertyName._npcRenderMount, Variant.From(in _npcRenderMount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.npcTalkFeature, out var value))
		{
			npcTalkFeature = value.As<TowerDefenseBattleFeatureNpcTalk>();
		}
		if (info.TryGetProperty(PropertyName._npcRenderMount, out var value2))
		{
			_npcRenderMount = value2.As<Control>();
		}
	}
}
