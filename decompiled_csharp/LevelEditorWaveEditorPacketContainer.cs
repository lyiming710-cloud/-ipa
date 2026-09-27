using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/WaveEditor/PacketContainer/LevelEditorWaveEditorPacketContainer.cs")]
public class LevelEditorWaveEditorPacketContainer : HBoxContainer
{
	public delegate void SelectedEventHandler(LevelEditorWaveEditorPacketContainer packetContainer);

	public new class MethodName : HBoxContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName CanAddPacket = "CanAddPacket";

		public static readonly StringName GetPacketPos = "GetPacketPos";

		public static readonly StringName AddPacket = "AddPacket";

		public static readonly StringName DeletePacket = "DeletePacket";

		public static readonly StringName HandleSelected = "HandleSelected";
	}

	public new class PropertyName : HBoxContainer.PropertyName
	{
		public static readonly StringName mainButton = "mainButton";

		public static readonly StringName _scrollContainer = "_scrollContainer";

		public static readonly StringName _packetSlotContainer = "_packetSlotContainer";

		public static readonly StringName _packetContainer = "_packetContainer";

		public static readonly StringName packetNum = "packetNum";

		public static readonly StringName packetList = "packetList";
	}

	public new class SignalName : HBoxContainer.SignalName
	{
	}

	public MainButton mainButton;

	private ScrollContainer _scrollContainer;

	private HBoxContainer _packetSlotContainer;

	private HBoxContainer _packetContainer;

	public int packetNum;

	public Array<TowerDefenseInGamePacketShow> packetList = new Array<TowerDefenseInGamePacketShow>();

	public event SelectedEventHandler OnSelected;

	public override void _Ready()
	{
		mainButton = GetNode<MainButton>("%MainButton");
		_scrollContainer = GetNode<ScrollContainer>("%ScrollContainer");
		_packetSlotContainer = GetNode<HBoxContainer>("%PacketSlotContainer");
		_packetContainer = GetNode<HBoxContainer>("%PacketContainer");
		mainButton.Pressed += HandleSelected;
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
		return packetNum < 50;
	}

	public Vector2 GetPacketPos(int id)
	{
		if (!TryGetPacketPos(id, out var position))
		{
			return GlobalPosition;
		}
		return position;
	}

	public bool TryGetPacketPos(int id, out Vector2 position)
	{
		position = GlobalPosition;
		if (!GodotObject.IsInstanceValid(_packetSlotContainer) || id < 0 || id >= _packetSlotContainer.GetChildCount())
		{
			return false;
		}
		Control childOrNull = _packetSlotContainer.GetChildOrNull<Control>(id);
		if (childOrNull == null)
		{
			return false;
		}
		position = childOrNull.GlobalPosition;
		return true;
	}

	public TowerDefenseInGamePacketShow AddPacket(TowerDefensePacketConfig _packetConfig)
	{
		packetNum++;
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
		towerDefenseInGamePacketShow.setPcLayout = true;
		towerDefenseInGamePacketShow.enforceRuntimeAvailabilityOnPress = false;
		_packetContainer.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
		towerDefenseInGamePacketShow.Init(_packetConfig);
		towerDefenseInGamePacketShow.onlyDraw = false;
		towerDefenseInGamePacketShow.OnPressed += DeletePacket;
		packetList.Add(towerDefenseInGamePacketShow);
		return towerDefenseInGamePacketShow;
	}

	public void DeletePacket(TowerDefenseInGamePacketShow _packet)
	{
		LevelEditorWaveEditor.Instance.levelConfig.canExport = false;
		int index = packetList.IndexOf(_packet);
		packetList.RemoveAt(index);
		_packet.QueueFree();
		packetNum--;
	}

	public void HandleSelected()
	{
		OnSelected?.Invoke(this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
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
			}, null),
			new MethodInfo(MethodName.HandleSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.HandleSelected && args.Count == 0)
		{
			HandleSelected();
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
		if (method == MethodName.HandleSelected)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.mainButton)
		{
			mainButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._scrollContainer)
		{
			_scrollContainer = VariantUtils.ConvertTo<ScrollContainer>(in value);
			return true;
		}
		if (name == PropertyName._packetSlotContainer)
		{
			_packetSlotContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._packetContainer)
		{
			_packetContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
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
		if (name == PropertyName.mainButton)
		{
			value = VariantUtils.CreateFrom(in mainButton);
			return true;
		}
		if (name == PropertyName._scrollContainer)
		{
			value = VariantUtils.CreateFrom(in _scrollContainer);
			return true;
		}
		if (name == PropertyName._packetSlotContainer)
		{
			value = VariantUtils.CreateFrom(in _packetSlotContainer);
			return true;
		}
		if (name == PropertyName._packetContainer)
		{
			value = VariantUtils.CreateFrom(in _packetContainer);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.mainButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scrollContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetSlotContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.packetNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mainButton, Variant.From(in mainButton));
		info.AddProperty(PropertyName._scrollContainer, Variant.From(in _scrollContainer));
		info.AddProperty(PropertyName._packetSlotContainer, Variant.From(in _packetSlotContainer));
		info.AddProperty(PropertyName._packetContainer, Variant.From(in _packetContainer));
		info.AddProperty(PropertyName.packetNum, Variant.From(in packetNum));
		info.AddProperty(PropertyName.packetList, Variant.CreateFrom(packetList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mainButton, out var value))
		{
			mainButton = value.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._scrollContainer, out var value2))
		{
			_scrollContainer = value2.As<ScrollContainer>();
		}
		if (info.TryGetProperty(PropertyName._packetSlotContainer, out var value3))
		{
			_packetSlotContainer = value3.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._packetContainer, out var value4))
		{
			_packetContainer = value4.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.packetNum, out var value5))
		{
			packetNum = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.packetList, out var value6))
		{
			packetList = value6.AsGodotArray<TowerDefenseInGamePacketShow>();
		}
	}
}
