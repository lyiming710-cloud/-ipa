using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/SeedbankEditor/SeedBank/LevelEditorSeedbank.cs")]
public class LevelEditorSeedbank : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName CanAddPacket = "CanAddPacket";

		public static readonly StringName GetPacketPos = "GetPacketPos";

		public static readonly StringName AddPacket = "AddPacket";

		public static readonly StringName DeletePacket = "DeletePacket";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _posMask = "_posMask";

		public static readonly StringName sunLabel = "sunLabel";

		public static readonly StringName seedContanin = "seedContanin";

		public static readonly StringName conveyor = "conveyor";

		public static readonly StringName _packetSlotContainer = "_packetSlotContainer";

		public static readonly StringName packetContainer = "packetContainer";

		public static readonly StringName seedBankTexture = "seedBankTexture";

		public static readonly StringName conveyorBeltRectPC = "conveyorBeltRectPC";

		public static readonly StringName belt = "belt";

		public static readonly StringName conveyorSunBankTexture = "conveyorSunBankTexture";

		public static readonly StringName conveyorSunLabel = "conveyorSunLabel";

		public static readonly StringName packetNum = "packetNum";

		public static readonly StringName packetList = "packetList";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public static Texture2D SEED_BANK_ZOMBIE;

	public static Texture2D SEED_BANK;

	public static Texture2D CONVEYOR_BELT_SUN;

	public static Texture2D CONVEYOR_BELT_SUN_BACKDROP;

	public static Texture2D CONVEYOR_BELT;

	public static Texture2D CONVEYOR_BELT_BACKDROP;

	private Control _posMask;

	public Label sunLabel;

	public MarginContainer seedContanin;

	public Control conveyor;

	private HBoxContainer _packetSlotContainer;

	public HBoxContainer packetContainer;

	public NinePatchRect seedBankTexture;

	public NinePatchRect conveyorBeltRectPC;

	public TextureRect belt;

	public TextureRect conveyorSunBankTexture;

	public Label conveyorSunLabel;

	public static LevelEditorSeedbank Instance;

	public int packetNum;

	public Array<TowerDefenseInGamePacketShow> packetList = new Array<TowerDefenseInGamePacketShow>();

	public override void _Ready()
	{
		SEED_BANK_ZOMBIE = GD.Load<Texture2D>("uid://1styv80tn18a");
		SEED_BANK = GD.Load<Texture2D>("uid://c2ihkj4xv50hg");
		CONVEYOR_BELT_SUN = GD.Load<Texture2D>("uid://i6rwl358wldw");
		CONVEYOR_BELT_SUN_BACKDROP = GD.Load<Texture2D>("uid://6h0njlvpp86");
		CONVEYOR_BELT = GD.Load<Texture2D>("uid://dbyiqyhraff1b");
		CONVEYOR_BELT_BACKDROP = GD.Load<Texture2D>("uid://ddglfhcpxg451");
		_posMask = GetNode<Control>("%PosMask");
		sunLabel = GetNode<Label>("%SunLabel");
		seedContanin = GetNode<MarginContainer>("%SeedContanin");
		conveyor = GetNode<Control>("%Conveyor");
		_packetSlotContainer = GetNode<HBoxContainer>("%PacketSlotContainer");
		packetContainer = GetNode<HBoxContainer>("%PacketContainer");
		seedBankTexture = GetNode<NinePatchRect>("%SeedBankTexture");
		conveyorBeltRectPC = GetNode<NinePatchRect>("%ConveyorBeltRectPC");
		belt = GetNode<TextureRect>("%Belt");
		conveyorSunBankTexture = GetNode<TextureRect>("%ConveyorSunBankTexture");
		conveyorSunLabel = GetNode<Label>("%ConveyorSunLabel");
		Instance = this;
	}

	public void Clear()
	{
		foreach (TowerDefenseInGamePacketShow packet in packetList)
		{
			packet.QueueFree();
		}
		packetList.Clear();
		packetNum = 0;
	}

	public bool CanAddPacket()
	{
		return packetNum < 16;
	}

	public Vector2 GetPacketPos(int id)
	{
		return _posMask.GlobalPosition + new Vector2(51 * id, 0f);
	}

	public TowerDefenseInGamePacketShow AddPacket(TowerDefensePacketConfig _packetConfig)
	{
		packetNum++;
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
		towerDefenseInGamePacketShow.setPcLayout = true;
		towerDefenseInGamePacketShow.enforceRuntimeAvailabilityOnPress = false;
		packetContainer.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
		towerDefenseInGamePacketShow.Init(_packetConfig);
		towerDefenseInGamePacketShow.onlyDraw = false;
		towerDefenseInGamePacketShow.OnPressed += DeletePacket;
		packetList.Add(towerDefenseInGamePacketShow);
		return towerDefenseInGamePacketShow;
	}

	public void DeletePacket(TowerDefenseInGamePacketShow _packet)
	{
		LevelEditorSeedbankEditor.Instance.levelConfig.canExport = false;
		int index = packetList.IndexOf(_packet);
		packetList.RemoveAt(index);
		_packet.QueueFree();
		packetNum--;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanAddPacket, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPacketPos, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DeletePacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
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
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.CanAddPacket && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanAddPacket());
			return true;
		}
		if (method == MethodName.GetPacketPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetPacketPos(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.AddPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(AddPacket(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.DeletePacket && args.Count == 1)
		{
			DeletePacket(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
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
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.CanAddPacket)
		{
			return true;
		}
		if (method == MethodName.GetPacketPos)
		{
			return true;
		}
		if (method == MethodName.AddPacket)
		{
			return true;
		}
		if (method == MethodName.DeletePacket)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._posMask)
		{
			_posMask = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.sunLabel)
		{
			sunLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.seedContanin)
		{
			seedContanin = VariantUtils.ConvertTo<MarginContainer>(in value);
			return true;
		}
		if (name == PropertyName.conveyor)
		{
			conveyor = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._packetSlotContainer)
		{
			_packetSlotContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.packetContainer)
		{
			packetContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.seedBankTexture)
		{
			seedBankTexture = VariantUtils.ConvertTo<NinePatchRect>(in value);
			return true;
		}
		if (name == PropertyName.conveyorBeltRectPC)
		{
			conveyorBeltRectPC = VariantUtils.ConvertTo<NinePatchRect>(in value);
			return true;
		}
		if (name == PropertyName.belt)
		{
			belt = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.conveyorSunBankTexture)
		{
			conveyorSunBankTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.conveyorSunLabel)
		{
			conveyorSunLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.packetNum)
		{
			packetNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			packetList = VariantUtils.ConvertToArray<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._posMask)
		{
			value = VariantUtils.CreateFrom(in _posMask);
			return true;
		}
		if (name == PropertyName.sunLabel)
		{
			value = VariantUtils.CreateFrom(in sunLabel);
			return true;
		}
		if (name == PropertyName.seedContanin)
		{
			value = VariantUtils.CreateFrom(in seedContanin);
			return true;
		}
		if (name == PropertyName.conveyor)
		{
			value = VariantUtils.CreateFrom(in conveyor);
			return true;
		}
		if (name == PropertyName._packetSlotContainer)
		{
			value = VariantUtils.CreateFrom(in _packetSlotContainer);
			return true;
		}
		if (name == PropertyName.packetContainer)
		{
			value = VariantUtils.CreateFrom(in packetContainer);
			return true;
		}
		if (name == PropertyName.seedBankTexture)
		{
			value = VariantUtils.CreateFrom(in seedBankTexture);
			return true;
		}
		if (name == PropertyName.conveyorBeltRectPC)
		{
			value = VariantUtils.CreateFrom(in conveyorBeltRectPC);
			return true;
		}
		if (name == PropertyName.belt)
		{
			value = VariantUtils.CreateFrom(in belt);
			return true;
		}
		if (name == PropertyName.conveyorSunBankTexture)
		{
			value = VariantUtils.CreateFrom(in conveyorSunBankTexture);
			return true;
		}
		if (name == PropertyName.conveyorSunLabel)
		{
			value = VariantUtils.CreateFrom(in conveyorSunLabel);
			return true;
		}
		if (name == PropertyName.packetNum)
		{
			value = VariantUtils.CreateFrom(in packetNum);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			value = VariantUtils.CreateFromArray(packetList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._posMask, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sunLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.seedContanin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.conveyor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetSlotContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.seedBankTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.conveyorBeltRectPC, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.belt, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.conveyorSunBankTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.conveyorSunLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.packetNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._posMask, Variant.From(in _posMask));
		info.AddProperty(PropertyName.sunLabel, Variant.From(in sunLabel));
		info.AddProperty(PropertyName.seedContanin, Variant.From(in seedContanin));
		info.AddProperty(PropertyName.conveyor, Variant.From(in conveyor));
		info.AddProperty(PropertyName._packetSlotContainer, Variant.From(in _packetSlotContainer));
		info.AddProperty(PropertyName.packetContainer, Variant.From(in packetContainer));
		info.AddProperty(PropertyName.seedBankTexture, Variant.From(in seedBankTexture));
		info.AddProperty(PropertyName.conveyorBeltRectPC, Variant.From(in conveyorBeltRectPC));
		info.AddProperty(PropertyName.belt, Variant.From(in belt));
		info.AddProperty(PropertyName.conveyorSunBankTexture, Variant.From(in conveyorSunBankTexture));
		info.AddProperty(PropertyName.conveyorSunLabel, Variant.From(in conveyorSunLabel));
		info.AddProperty(PropertyName.packetNum, Variant.From(in packetNum));
		info.AddProperty(PropertyName.packetList, Variant.CreateFrom(packetList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._posMask, out var value))
		{
			_posMask = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.sunLabel, out var value2))
		{
			sunLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.seedContanin, out var value3))
		{
			seedContanin = value3.As<MarginContainer>();
		}
		if (info.TryGetProperty(PropertyName.conveyor, out var value4))
		{
			conveyor = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._packetSlotContainer, out var value5))
		{
			_packetSlotContainer = value5.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.packetContainer, out var value6))
		{
			packetContainer = value6.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.seedBankTexture, out var value7))
		{
			seedBankTexture = value7.As<NinePatchRect>();
		}
		if (info.TryGetProperty(PropertyName.conveyorBeltRectPC, out var value8))
		{
			conveyorBeltRectPC = value8.As<NinePatchRect>();
		}
		if (info.TryGetProperty(PropertyName.belt, out var value9))
		{
			belt = value9.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.conveyorSunBankTexture, out var value10))
		{
			conveyorSunBankTexture = value10.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.conveyorSunLabel, out var value11))
		{
			conveyorSunLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.packetNum, out var value12))
		{
			packetNum = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName.packetList, out var value13))
		{
			packetList = value13.AsGodotArray<TowerDefenseInGamePacketShow>();
		}
	}
}
