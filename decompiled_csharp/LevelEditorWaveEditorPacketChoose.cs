using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/WaveEditor/PacketChoose/LevelEditorWaveEditorPacketChoose.cs")]
public class LevelEditorWaveEditorPacketChoose : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RefreshWhenVisible = "RefreshWhenVisible";

		public static readonly StringName Init = "Init";

		public static readonly StringName PacketChoose = "PacketChoose";

		public static readonly StringName PacketNameChoose = "PacketNameChoose";

		public static readonly StringName PacketListChoose = "PacketListChoose";

		public static readonly StringName CreateAnime = "CreateAnime";

		public static readonly StringName PacketClear = "PacketClear";

		public static readonly StringName CategoryChoose = "CategoryChoose";

		public static readonly StringName LoveChange = "LoveChange";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _packetBankScroll = "_packetBankScroll";

		public static readonly StringName _packetBankMargin = "_packetBankMargin";

		public static readonly StringName _packetContainer = "_packetContainer";

		public static readonly StringName _animeNode = "_animeNode";

		public static readonly StringName data = "data";

		public static readonly StringName mapFeature = "mapFeature";

		public static readonly StringName packetList = "packetList";

		public static readonly StringName currentCategory = "currentCategory";

		public static readonly StringName currentIndex = "currentIndex";

		public static readonly StringName _categoryGeneration = "_categoryGeneration";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private ScrollContainer _packetBankScroll;

	private MarginContainer _packetBankMargin;

	private GridContainer _packetContainer;

	private Control _animeNode;

	public static LevelEditorWaveEditorPacketChoose Instance;

	public Variant data;

	public TowerDefenseBattleFeatureMap mapFeature;

	public Array<TowerDefenseInGamePacketShow> packetList = new Array<TowerDefenseInGamePacketShow>();

	public string currentCategory = "";

	public int currentIndex = -1;

	public int _categoryGeneration;

	private const string DefaultCategory = "Zombie";

	public override void _Ready()
	{
		_packetBankScroll = GetNode<ScrollContainer>("%PacketBankScroll");
		_packetBankMargin = GetNode<MarginContainer>("%PacketBankMargin");
		_packetContainer = GetNode<GridContainer>("%PacketContainer");
		_animeNode = GetNode<Control>("%AnimeNode");
		foreach (Node child in GetNode<VBoxContainer>("VBoxContainer").GetChildren())
		{
			if (child is PacketCategoryButton packetCategoryButton)
			{
				packetCategoryButton.OnChoose += (string category) =>
				{
					CategoryChoose(category);
				};
			}
		}
		mapFeature = TowerDefenseManager.GetMapFeature();
		Instance = this;
		data = TowerDefenseManager.GetPacketBankData("Total");
		VisibilityChanged += RefreshWhenVisible;
		CallDeferred("RefreshWhenVisible");
	}

	public void RefreshWhenVisible()
	{
		if (!IsInsideTree() || !GodotObject.IsInstanceValid(_packetContainer))
		{
			return;
		}
		if (IsVisibleInTree())
		{
			if (packetList.Count == 0 && data.VariantType != Variant.Type.Nil)
			{
				CategoryChoose(string.IsNullOrEmpty(currentCategory) ? "Zombie" : currentCategory, reFresh: true);
			}
		}
		else if (packetList.Count > 0)
		{
			PacketClear();
		}
	}

	public async void Init(Variant _data)
	{
		data = _data;
		PacketClear();
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		CategoryChoose("Zombie");
	}

	public void PacketChoose(TowerDefenseInGamePacketShow packet)
	{
		LevelEditorWaveEditor.Instance.levelConfig.canExport = false;
		int num = packetList.IndexOf(packet);
		if (packet.alive && currentIndex != -1 && LevelEditorWaveEditor.Instance.currentPacketContainer != null && LevelEditorWaveEditor.Instance.currentPacketContainer.CanAddPacket())
		{
			CreateAnime(packet.config, packet.GlobalPosition, LevelEditorWaveEditor.Instance.currentPacketContainer);
		}
		if (num != currentIndex)
		{
			if (currentIndex != -1)
			{
				packetList[currentIndex].Reset();
			}
			currentIndex = num;
		}
	}

	public void PacketNameChoose(string packetName)
	{
		AudioManager.Instance.AudioPlay("PacketPick");
		if (LevelEditorWaveEditor.Instance.currentPacketContainer == null || !LevelEditorWaveEditor.Instance.currentPacketContainer.CanAddPacket())
		{
			return;
		}
		bool flag = false;
		foreach (TowerDefenseInGamePacketShow packet in packetList)
		{
			if (packet.config.saveKey == packetName && packet.alive)
			{
				flag = true;
				PacketChoose(packet);
				break;
			}
		}
		if (!flag)
		{
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
			Vector2 globalPosition = GetViewport().GetCamera2D().GlobalPosition;
			CreateAnime(packetConfig, globalPosition + new Vector2(300f, 260f), LevelEditorWaveEditor.Instance.currentPacketContainer);
		}
	}

	public void PacketListChoose(Array _packetList)
	{
		AudioManager.Instance.AudioPlay("PacketPick");
		foreach (Node child in _animeNode.GetChildren())
		{
			child.QueueFree();
		}
		foreach (Variant _packet in _packetList)
		{
			string packetName = (string)_packet;
			PacketNameChoose(packetName);
		}
	}

	public async void CreateAnime(TowerDefensePacketConfig config, Vector2 pos, LevelEditorWaveEditorPacketContainer _packetContainer)
	{
		if (!GodotObject.IsInstanceValid(config) || !GodotObject.IsInstanceValid(_packetContainer) || !_packetContainer.CanAddPacket())
		{
			return;
		}
		TowerDefenseInGamePacketShow packet = _packetContainer.AddPacket(config);
		if (!GodotObject.IsInstanceValid(packet))
		{
			return;
		}
		packet.Visible = false;
		TowerDefenseInGamePacketShow animePacket = TowerDefenseManager.CreatePacketShow();
		animePacket.setPcLayout = true;
		_animeNode.AddChild(animePacket, forceReadableName: false, InternalMode.Disabled);
		animePacket.Init(config);
		animePacket.onlyDraw = true;
		animePacket.GlobalPosition = pos;
		try
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			if (GodotObject.IsInstanceValid(animePacket) && GodotObject.IsInstanceValid(_packetContainer) && !packet.IsQueuedForDeletion())
			{
				int num = _packetContainer.packetList.IndexOf(packet);
				if (!_packetContainer.TryGetPacketPos(num, out var position))
				{
					GD.PushWarning($"[WaveEditor] Packet animation target slot is unavailable: {num}.");
				}
				else
				{
					Tween tween = animePacket.CreateTween();
					tween.SetEase(Tween.EaseType.Out);
					tween.SetTrans(Tween.TransitionType.Quart);
					tween.TweenProperty(animePacket, "global_position", position, 0.5);
					await ToSignal(tween, Tween.SignalName.Finished);
				}
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(packet) && !packet.IsQueuedForDeletion())
			{
				packet.Visible = true;
			}
			if (GodotObject.IsInstanceValid(animePacket) && !animePacket.IsQueuedForDeletion())
			{
				animePacket.QueueFree();
			}
		}
	}

	public void PacketClear()
	{
		foreach (Node child in _packetContainer.GetChildren())
		{
			child.QueueFree();
		}
		packetList = new Array<TowerDefenseInGamePacketShow>();
		currentIndex = 0;
	}

	public async void CategoryChoose(string _category, bool reFresh = false)
	{
		if (currentCategory == _category && !reFresh)
		{
			return;
		}
		currentCategory = _category;
		PacketClear();
		if (!(data.AsGodotObject() is TowerDefensePacketBankData towerDefensePacketBankData) || !towerDefensePacketBankData.category.ContainsKey(_category))
		{
			return;
		}
		_categoryGeneration++;
		int currentGeneration = _categoryGeneration;
		Array array = towerDefensePacketBankData.category[_category].AsGodotArray();
		Array array2 = new Array();
		Array array3 = new Array();
		foreach (Variant item in array)
		{
			string text = (string)item;
			if (GameSaveManager.Instance.GetTowerDefensePacketValue(text).GetValueOrDefault("Love", false).AsBool())
			{
				array2.Add(text);
			}
			else
			{
				array3.Add(text);
			}
		}
		foreach (Variant item2 in array3)
		{
			array2.Add(item2);
		}
		int batchCount = 0;
		foreach (Variant item3 in array2)
		{
			string packetName = (string)item3;
			if (_categoryGeneration != currentGeneration)
			{
				return;
			}
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
			towerDefenseInGamePacketShow.setPcLayout = true;
			towerDefenseInGamePacketShow.enforceRuntimeAvailabilityOnPress = false;
			_packetContainer.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
			towerDefenseInGamePacketShow.Init(packetConfig);
			towerDefenseInGamePacketShow.showLove = true;
			towerDefenseInGamePacketShow.OnLoveChange += LoveChange;
			towerDefenseInGamePacketShow.OnPressed += PacketChoose;
			packetList.Add(towerDefenseInGamePacketShow);
			batchCount++;
			if (batchCount >= 8)
			{
				batchCount = 0;
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				if (_categoryGeneration != currentGeneration)
				{
					return;
				}
			}
		}
	}

	public void LoveChange(TowerDefenseInGamePacketShow packet)
	{
		CategoryChoose(currentCategory, reFresh: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshWhenVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "_data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.PacketChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.PacketNameChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PacketListChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "_packetList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateAnime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_packetContainer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.PacketClear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CategoryChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "reFresh", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoveChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
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
		if (method == MethodName.RefreshWhenVisible && args.Count == 0)
		{
			RefreshWhenVisible();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PacketChoose && args.Count == 1)
		{
			PacketChoose(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PacketNameChoose && args.Count == 1)
		{
			PacketNameChoose(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PacketListChoose && args.Count == 1)
		{
			PacketListChoose(VariantUtils.ConvertTo<Array>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateAnime && args.Count == 3)
		{
			CreateAnime(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<LevelEditorWaveEditorPacketContainer>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.PacketClear && args.Count == 0)
		{
			PacketClear();
			ret = default;
			return true;
		}
		if (method == MethodName.CategoryChoose && args.Count == 2)
		{
			CategoryChoose(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoveChange && args.Count == 1)
		{
			LoveChange(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
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
		if (method == MethodName.RefreshWhenVisible)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.PacketChoose)
		{
			return true;
		}
		if (method == MethodName.PacketNameChoose)
		{
			return true;
		}
		if (method == MethodName.PacketListChoose)
		{
			return true;
		}
		if (method == MethodName.CreateAnime)
		{
			return true;
		}
		if (method == MethodName.PacketClear)
		{
			return true;
		}
		if (method == MethodName.CategoryChoose)
		{
			return true;
		}
		if (method == MethodName.LoveChange)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._packetBankScroll)
		{
			_packetBankScroll = VariantUtils.ConvertTo<ScrollContainer>(in value);
			return true;
		}
		if (name == PropertyName._packetBankMargin)
		{
			_packetBankMargin = VariantUtils.ConvertTo<MarginContainer>(in value);
			return true;
		}
		if (name == PropertyName._packetContainer)
		{
			_packetContainer = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName._animeNode)
		{
			_animeNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.data)
		{
			data = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			packetList = VariantUtils.ConvertToArray<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName.currentCategory)
		{
			currentCategory = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.currentIndex)
		{
			currentIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._categoryGeneration)
		{
			_categoryGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._packetBankScroll)
		{
			value = VariantUtils.CreateFrom(in _packetBankScroll);
			return true;
		}
		if (name == PropertyName._packetBankMargin)
		{
			value = VariantUtils.CreateFrom(in _packetBankMargin);
			return true;
		}
		if (name == PropertyName._packetContainer)
		{
			value = VariantUtils.CreateFrom(in _packetContainer);
			return true;
		}
		if (name == PropertyName._animeNode)
		{
			value = VariantUtils.CreateFrom(in _animeNode);
			return true;
		}
		if (name == PropertyName.data)
		{
			value = VariantUtils.CreateFrom(in data);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			value = VariantUtils.CreateFrom(in mapFeature);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			value = VariantUtils.CreateFromArray(packetList);
			return true;
		}
		if (name == PropertyName.currentCategory)
		{
			value = VariantUtils.CreateFrom(in currentCategory);
			return true;
		}
		if (name == PropertyName.currentIndex)
		{
			value = VariantUtils.CreateFrom(in currentIndex);
			return true;
		}
		if (name == PropertyName._categoryGeneration)
		{
			value = VariantUtils.CreateFrom(in _categoryGeneration);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._packetBankScroll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetBankMargin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animeNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentCategory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._categoryGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._packetBankScroll, Variant.From(in _packetBankScroll));
		info.AddProperty(PropertyName._packetBankMargin, Variant.From(in _packetBankMargin));
		info.AddProperty(PropertyName._packetContainer, Variant.From(in _packetContainer));
		info.AddProperty(PropertyName._animeNode, Variant.From(in _animeNode));
		info.AddProperty(PropertyName.data, Variant.From(in data));
		info.AddProperty(PropertyName.mapFeature, Variant.From(in mapFeature));
		info.AddProperty(PropertyName.packetList, Variant.CreateFrom(packetList));
		info.AddProperty(PropertyName.currentCategory, Variant.From(in currentCategory));
		info.AddProperty(PropertyName.currentIndex, Variant.From(in currentIndex));
		info.AddProperty(PropertyName._categoryGeneration, Variant.From(in _categoryGeneration));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._packetBankScroll, out var value))
		{
			_packetBankScroll = value.As<ScrollContainer>();
		}
		if (info.TryGetProperty(PropertyName._packetBankMargin, out var value2))
		{
			_packetBankMargin = value2.As<MarginContainer>();
		}
		if (info.TryGetProperty(PropertyName._packetContainer, out var value3))
		{
			_packetContainer = value3.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName._animeNode, out var value4))
		{
			_animeNode = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.data, out var value5))
		{
			data = value5.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.mapFeature, out var value6))
		{
			mapFeature = value6.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName.packetList, out var value7))
		{
			packetList = value7.AsGodotArray<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName.currentCategory, out var value8))
		{
			currentCategory = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.currentIndex, out var value9))
		{
			currentIndex = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._categoryGeneration, out var value10))
		{
			_categoryGeneration = value10.As<int>();
		}
	}
}
