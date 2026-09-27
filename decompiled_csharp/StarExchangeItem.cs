using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/StarExchange/StarExchangeItem/StarExchangeItem.cs")]
public class StarExchangeItem : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName Pressed = "Pressed";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName costLabel = "costLabel";

		public static readonly StringName button = "button";

		public static readonly StringName packetNode = "packetNode";

		public static readonly StringName packet = "packet";

		public static readonly StringName finishTexture = "finishTexture";

		public static readonly StringName data = "data";

		public static readonly StringName finishNum = "finishNum";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private Label costLabel;

	private Button button;

	private Control packetNode;

	private TowerDefenseInGamePacketShow packet;

	private TextureRect finishTexture;

	public Dictionary data;

	public int finishNum;

	public override void _Ready()
	{
		costLabel = GetNode<Label>("%CostLabel");
		button = GetNode<Button>("%Button");
		packetNode = GetNode<Control>("%PacketNode");
		packet = GetNode<TowerDefenseInGamePacketShow>("%Packet");
		finishTexture = GetNode<TextureRect>("%FinishTexture");
		button.Pressed += Pressed;
	}

	public void Init(Dictionary _data, int _finishNum = 0)
	{
		data = _data;
		finishNum = _finishNum;
		if (data["Type"].AsString() == "Packet")
		{
			packet.Init(TowerDefenseManager.GetPacketConfig(data["Key"].AsString()));
		}
		costLabel.Text = string.Format("x{0}", data["FinishNum"].AsInt32());
		if (finishNum >= data["FinishNum"].AsInt32() && GameSaveManager.Instance.GetTowerDefensePacketValue(data["Key"].AsString()).GetValueOrDefault("Unlock", false).AsBool())
		{
			finishTexture.Visible = true;
		}
	}

	public void Pressed()
	{
		if (finishNum >= data["FinishNum"].AsInt32())
		{
			Dictionary towerDefensePacketValue = GameSaveManager.Instance.GetTowerDefensePacketValue(data["Key"].AsString());
			if (!towerDefensePacketValue.GetValueOrDefault("Unlock", false).AsBool())
			{
				towerDefensePacketValue["Unlock"] = true;
				finishTexture.Visible = true;
				GameSaveManager.Instance.SetTowerDefensePacketValue(data["Key"].AsString(), towerDefensePacketValue);
				GameSaveManager.Instance.Save();
				DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]您成功兑换该植物[/font_size][/center]");
			}
			else
			{
				DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]您已经兑换该植物[/font_size][/center]");
			}
		}
		else
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]您的星星不足[/font_size][/center]");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_finishNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Pressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Init && args.Count == 2)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Pressed && args.Count == 0)
		{
			Pressed();
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Pressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.costLabel)
		{
			costLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.button)
		{
			button = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName.packetNode)
		{
			packetNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.packet)
		{
			packet = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName.finishTexture)
		{
			finishTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.data)
		{
			data = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.finishNum)
		{
			finishNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.costLabel)
		{
			value = VariantUtils.CreateFrom(in costLabel);
			return true;
		}
		if (name == PropertyName.button)
		{
			value = VariantUtils.CreateFrom(in button);
			return true;
		}
		if (name == PropertyName.packetNode)
		{
			value = VariantUtils.CreateFrom(in packetNode);
			return true;
		}
		if (name == PropertyName.packet)
		{
			value = VariantUtils.CreateFrom(in packet);
			return true;
		}
		if (name == PropertyName.finishTexture)
		{
			value = VariantUtils.CreateFrom(in finishTexture);
			return true;
		}
		if (name == PropertyName.data)
		{
			value = VariantUtils.CreateFrom(in data);
			return true;
		}
		if (name == PropertyName.finishNum)
		{
			value = VariantUtils.CreateFrom(in finishNum);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.costLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.button, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packet, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.finishTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.finishNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.costLabel, Variant.From(in costLabel));
		info.AddProperty(PropertyName.button, Variant.From(in button));
		info.AddProperty(PropertyName.packetNode, Variant.From(in packetNode));
		info.AddProperty(PropertyName.packet, Variant.From(in packet));
		info.AddProperty(PropertyName.finishTexture, Variant.From(in finishTexture));
		info.AddProperty(PropertyName.data, Variant.From(in data));
		info.AddProperty(PropertyName.finishNum, Variant.From(in finishNum));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.costLabel, out var value))
		{
			costLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.button, out var value2))
		{
			button = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.packetNode, out var value3))
		{
			packetNode = value3.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.packet, out var value4))
		{
			packet = value4.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName.finishTexture, out var value5))
		{
			finishTexture = value5.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.data, out var value6))
		{
			data = value6.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.finishNum, out var value7))
		{
			finishNum = value7.As<int>();
		}
	}
}
