using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/Almanac/AlmanacZombie/AlmanacZombieWidow.cs")]
public class AlmanacZombieWidow : Control
{
	public delegate void PressedEventHandler(TowerDefensePacketConfig config);

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Init = "Init";

		public static readonly StringName ClearEventHandlers = "ClearEventHandlers";

		public static readonly StringName CreateSprite = "CreateSprite";

		public static readonly StringName ApplyInitialArmor = "ApplyInitialArmor";

		public static readonly StringName ApplyCurrentCustomFilter = "ApplyCurrentCustomFilter";

		public static readonly StringName _VisibilityChanged = "_VisibilityChanged";

		public static readonly StringName ApplyDeferredVisibilityState = "ApplyDeferredVisibilityState";

		public static readonly StringName ReleaseSprite = "ReleaseSprite";

		public static readonly StringName ReleaseForPoolDisposal = "ReleaseForPoolDisposal";

		public static readonly StringName ReleasePreviewTree = "ReleasePreviewTree";

		public static readonly StringName _ButtonPressed = "_ButtonPressed";

		public static readonly StringName _MouseEntered = "_MouseEntered";

		public static readonly StringName _MouseExited = "_MouseExited";

		public static readonly StringName Reset = "Reset";

		public static readonly StringName ConfigurePreviewSprite = "ConfigurePreviewSprite";

		public static readonly StringName FreezePreviewTree = "FreezePreviewTree";

		public static readonly StringName PlayPreviewTreeFromStart = "PlayPreviewTreeFromStart";

		public static readonly StringName OnCharacterSkinSwitched = "OnCharacterSkinSwitched";

		public static readonly StringName TrySubscribeCharacterSkinSwitch = "TrySubscribeCharacterSkinSwitch";

		public static readonly StringName TryUnsubscribeCharacterSkinSwitch = "TryUnsubscribeCharacterSkinSwitch";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName spriteNode = "spriteNode";

		public static readonly StringName previewClip = "previewClip";

		public static readonly StringName config = "config";

		public static readonly StringName sprite = "sprite";

		public static readonly StringName _previewHovered = "_previewHovered";

		public static readonly StringName _visibilityRefreshQueued = "_visibilityRefreshQueued";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public Control spriteNode;

	public Control previewClip;

	public TowerDefensePacketConfig config;

	public AdobeAnimateSprite sprite;

	private bool _previewHovered;

	private bool _visibilityRefreshQueued;

	public event PressedEventHandler OnPressed;

	public override void _Ready()
	{
		spriteNode = GetNode<Control>("%SpriteNode");
		previewClip = GetNode<Control>("%Mask");
		VisibilityChanged += _VisibilityChanged;
		GetNode<BaseButton>("%Button").MouseEntered += _MouseEntered;
		GetNode<BaseButton>("%Button").MouseExited += _MouseExited;
		GetNode<BaseButton>("%Button").Pressed += _ButtonPressed;
	}

	public override void _ExitTree()
	{
		_visibilityRefreshQueued = false;
		TryUnsubscribeCharacterSkinSwitch();
		ReleasePreviewTree(sprite);
		base._ExitTree();
	}

	public void Init(TowerDefensePacketConfig packetConfig)
	{
		TryUnsubscribeCharacterSkinSwitch();
		config = packetConfig;
		_previewHovered = false;
		CreateSprite();
		if (GodotObject.IsInstanceValid(sprite))
		{
			FreezePreviewTree(sprite);
			previewClip.Visible = true;
		}
		else
		{
			previewClip.Visible = false;
		}
		TrySubscribeCharacterSkinSwitch();
	}

	public void ClearEventHandlers()
	{
		OnPressed = null;
	}

	public void CreateSprite()
	{
		if (config == null || !GodotObject.IsInstanceValid(config.characterConfig))
		{
			return;
		}
		ReleaseSprite();
		sprite = TowerDefenseManager.GetPacketSprite(config);
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.LightMask = 0;
			sprite.forceLocalRender = true;
			sprite.SetRenderClipControl(previewClip);
			spriteNode.AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
			sprite.Position = config.packetAnimeOffset + new Vector2(-2f, 0f);
			sprite.Scale = config.packetAnimeScale;
			if (config.packetFlip)
			{
				sprite.Scale = new Vector2(0f - sprite.Scale.X, sprite.Scale.Y);
			}
			if (config.overrideHypnoses)
			{
				sprite.Modulate = new Color(0.72f, 0.62f, 1f, sprite.Modulate.A);
			}
			TowerDefenseCharacterConfig characterConfig = config.characterConfig;
			ApplyInitialArmor(characterConfig);
			ApplyCurrentCustomFilter(characterConfig);
			sprite.SetAnimation(config.packetAnimeClip);
			sprite.UpdateMediaReplaceData();
			sprite.UpdateChild();
			FreezePreviewTree(sprite);
			AdobeAnimateRenderManager.WarmupRenderMount(sprite);
		}
	}

	private void ApplyInitialArmor(TowerDefenseCharacterConfig characterConfig)
	{
		if (characterConfig.armorData == null || config.initArmor == null)
		{
			return;
		}
		foreach (string item in config.initArmor)
		{
			ArmorSlotConfig slotConfig = characterConfig.armorData.GetSlotConfig(item);
			TowerDefenseArmorTypeData typeData = characterConfig.armorData.GetTypeData(item);
			if (typeData == null)
			{
				continue;
			}
			string replaceMethod = slotConfig.replaceMethod;
			if (!(replaceMethod == "Media"))
			{
				if (replaceMethod == "Sprite")
				{
					CharacterArmorData.CreateArmorPartNode(sprite, slotConfig, typeData);
				}
			}
			else
			{
				characterConfig.armorData.OpenArmorFliters(sprite, item);
				characterConfig.armorData.SetArmorReplace(sprite, item, 0);
			}
		}
	}

	private void ApplyCurrentCustomFilter(TowerDefenseCharacterConfig characterConfig)
	{
		if (characterConfig.customData != null && TryGetPacketSaveValue(config.saveKey, out var packetData))
		{
			string text = packetData.GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary().GetValueOrDefault("Custom", "")
				.AsString();
			if (text != "" && characterConfig.customData.customDictionary.ContainsKey(text))
			{
				characterConfig.customData.SetCustomFliters(sprite, text);
			}
		}
	}

	public void _VisibilityChanged()
	{
		if (IsInsideTree() && !_visibilityRefreshQueued)
		{
			_visibilityRefreshQueued = true;
			CallDeferred("ApplyDeferredVisibilityState");
		}
	}

	public void ApplyDeferredVisibilityState()
	{
		_visibilityRefreshQueued = false;
		if (!IsInsideTree())
		{
			return;
		}
		if (!IsVisibleInTree())
		{
			if (GodotObject.IsInstanceValid(sprite))
			{
				FreezePreviewTree(sprite);
			}
			return;
		}
		if (!GodotObject.IsInstanceValid(sprite))
		{
			CreateSprite();
		}
		if (GodotObject.IsInstanceValid(sprite) && !_previewHovered)
		{
			FreezePreviewTree(sprite);
		}
		previewClip.Visible = GodotObject.IsInstanceValid(sprite);
	}

	private void ReleaseSprite()
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			ReleasePreviewTree(sprite);
			sprite.QueueFree();
		}
		sprite = null;
	}

	public void ReleaseForPoolDisposal()
	{
		_visibilityRefreshQueued = false;
		_previewHovered = false;
		ClearEventHandlers();
		TryUnsubscribeCharacterSkinSwitch();
		config = null;
		ReleaseSprite();
		if (GodotObject.IsInstanceValid(previewClip))
		{
			previewClip.Visible = false;
		}
	}

	private static void ReleasePreviewTree(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.ReleaseForcedCpuPoseData();
			adobeAnimateSprite.ClearRenderClipControl();
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			ReleasePreviewTree(child);
		}
	}

	public void _ButtonPressed()
	{
		if (config != null)
		{
			AudioManager.Instance.AudioPlay("PacketPick");
			OnPressed?.Invoke(config);
		}
	}

	public void _MouseEntered()
	{
		_previewHovered = true;
		previewClip.Visible = true;
		if (!GodotObject.IsInstanceValid(sprite))
		{
			CreateSprite();
		}
		if (!GodotObject.IsInstanceValid(sprite))
		{
			previewClip.Visible = false;
			return;
		}
		sprite.SetAnimation(config.packetAnimeClip);
		sprite.UpdateMediaReplaceData();
		sprite.Visible = true;
		PlayPreviewTreeFromStart(sprite);
		AdobeAnimateRenderManager.WarmupRenderMount(sprite);
	}

	public void _MouseExited()
	{
		_previewHovered = false;
		if (GodotObject.IsInstanceValid(sprite))
		{
			FreezePreviewTree(sprite);
		}
	}

	public void Reset()
	{
		_previewHovered = false;
		if (GodotObject.IsInstanceValid(sprite))
		{
			FreezePreviewTree(sprite);
		}
	}

	private void ConfigurePreviewSprite(AdobeAnimateSprite animateSprite)
	{
		animateSprite.LightMask = 0;
		animateSprite.keepRenderSubmittedWhenPaused = true;
		animateSprite.forceLocalRender = true;
		animateSprite.SetRenderClipControl(previewClip);
		animateSprite.ProcessMode = ProcessModeEnum.Always;
	}

	private void FreezePreviewTree(Node node, bool forcePoseRefresh = false)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			ConfigurePreviewSprite(adobeAnimateSprite);
			if (forcePoseRefresh || !adobeAnimateSprite.IsFrozenPreview)
			{
				adobeAnimateSprite.ResetAnimation();
				adobeAnimateSprite.UpdateMediaReplaceData();
				adobeAnimateSprite.UpdateChild();
				adobeAnimateSprite.RefreshManagedSlotSpriteCacheForRender();
				adobeAnimateSprite.QueueRedraw();
			}
			adobeAnimateSprite.SetFrozenPreview(frozen: true);
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			FreezePreviewTree(child, forcePoseRefresh);
		}
	}

	private void PlayPreviewTreeFromStart(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			ConfigurePreviewSprite(adobeAnimateSprite);
			adobeAnimateSprite.ResetAnimation();
			adobeAnimateSprite.UpdateMediaReplaceData();
			adobeAnimateSprite.UpdateChild();
			adobeAnimateSprite.RefreshManagedSlotSpriteCacheForRender();
			adobeAnimateSprite.SetFrozenPreview(frozen: false);
			adobeAnimateSprite.RefreshProcessScheduling();
			adobeAnimateSprite.QueueRedraw();
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			PlayPreviewTreeFromStart(child);
		}
	}

	private void OnCharacterSkinSwitched(string packetSaveKey, string customKey)
	{
		if (!GodotObject.IsInstanceValid(config) || config.saveKey != packetSaveKey || !GodotObject.IsInstanceValid(sprite))
		{
			return;
		}
		TowerDefenseCharacterConfig characterConfig = config.characterConfig;
		if (GodotObject.IsInstanceValid(characterConfig) && characterConfig.customData != null && (!(customKey != "") || characterConfig.customData.customDictionary.ContainsKey(customKey)))
		{
			characterConfig.customData.ClearCustomFliters(sprite);
			if (customKey != "")
			{
				characterConfig.customData.SetCustomFliters(sprite, customKey);
			}
			sprite.UpdateMediaReplaceData();
			sprite.UpdateChild();
			sprite.RefreshManagedSlotSpriteCacheForRender();
			if (_previewHovered)
			{
				PlayPreviewTreeFromStart(sprite);
			}
			else
			{
				FreezePreviewTree(sprite, forcePoseRefresh: true);
			}
		}
	}

	private void TrySubscribeCharacterSkinSwitch()
	{
		if (BattleEventBus.Instance != null)
		{
			BattleEventBus.Instance.OnCharacterSkinSwitched -= OnCharacterSkinSwitched;
			BattleEventBus.Instance.OnCharacterSkinSwitched += OnCharacterSkinSwitched;
		}
	}

	private void TryUnsubscribeCharacterSkinSwitch()
	{
		if (BattleEventBus.Instance != null)
		{
			BattleEventBus.Instance.OnCharacterSkinSwitched -= OnCharacterSkinSwitched;
		}
	}

	private static bool TryGetPacketSaveValue(string saveKey, out Dictionary packetData)
	{
		packetData = new Dictionary();
		if (string.IsNullOrWhiteSpace(saveKey) || GameSaveManager.Instance == null)
		{
			return false;
		}
		packetData = XWModPlayerProgressService.GetPacketState(saveKey);
		return packetData != null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(22)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearEventHandlers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyInitialArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "characterConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCurrentCustomFilter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "characterConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._VisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyDeferredVisibilityState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseForPoolDisposal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleasePreviewTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._ButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._MouseEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._MouseExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Reset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigurePreviewSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animateSprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FreezePreviewTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "forcePoseRefresh", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayPreviewTreeFromStart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnCharacterSkinSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetSaveKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TrySubscribeCharacterSkinSwitch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryUnsubscribeCharacterSkinSwitch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearEventHandlers && args.Count == 0)
		{
			ClearEventHandlers();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSprite && args.Count == 0)
		{
			CreateSprite();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyInitialArmor && args.Count == 1)
		{
			ApplyInitialArmor(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCurrentCustomFilter && args.Count == 1)
		{
			ApplyCurrentCustomFilter(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._VisibilityChanged && args.Count == 0)
		{
			_VisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyDeferredVisibilityState && args.Count == 0)
		{
			ApplyDeferredVisibilityState();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseSprite && args.Count == 0)
		{
			ReleaseSprite();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseForPoolDisposal && args.Count == 0)
		{
			ReleaseForPoolDisposal();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleasePreviewTree && args.Count == 1)
		{
			ReleasePreviewTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ButtonPressed && args.Count == 0)
		{
			_ButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName._MouseEntered && args.Count == 0)
		{
			_MouseEntered();
			ret = default;
			return true;
		}
		if (method == MethodName._MouseExited && args.Count == 0)
		{
			_MouseExited();
			ret = default;
			return true;
		}
		if (method == MethodName.Reset && args.Count == 0)
		{
			Reset();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigurePreviewSprite && args.Count == 1)
		{
			ConfigurePreviewSprite(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FreezePreviewTree && args.Count == 2)
		{
			FreezePreviewTree(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayPreviewTreeFromStart && args.Count == 1)
		{
			PlayPreviewTreeFromStart(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCharacterSkinSwitched && args.Count == 2)
		{
			OnCharacterSkinSwitched(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TrySubscribeCharacterSkinSwitch && args.Count == 0)
		{
			TrySubscribeCharacterSkinSwitch();
			ret = default;
			return true;
		}
		if (method == MethodName.TryUnsubscribeCharacterSkinSwitch && args.Count == 0)
		{
			TryUnsubscribeCharacterSkinSwitch();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ReleasePreviewTree && args.Count == 1)
		{
			ReleasePreviewTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.ClearEventHandlers)
		{
			return true;
		}
		if (method == MethodName.CreateSprite)
		{
			return true;
		}
		if (method == MethodName.ApplyInitialArmor)
		{
			return true;
		}
		if (method == MethodName.ApplyCurrentCustomFilter)
		{
			return true;
		}
		if (method == MethodName._VisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.ApplyDeferredVisibilityState)
		{
			return true;
		}
		if (method == MethodName.ReleaseSprite)
		{
			return true;
		}
		if (method == MethodName.ReleaseForPoolDisposal)
		{
			return true;
		}
		if (method == MethodName.ReleasePreviewTree)
		{
			return true;
		}
		if (method == MethodName._ButtonPressed)
		{
			return true;
		}
		if (method == MethodName._MouseEntered)
		{
			return true;
		}
		if (method == MethodName._MouseExited)
		{
			return true;
		}
		if (method == MethodName.Reset)
		{
			return true;
		}
		if (method == MethodName.ConfigurePreviewSprite)
		{
			return true;
		}
		if (method == MethodName.FreezePreviewTree)
		{
			return true;
		}
		if (method == MethodName.PlayPreviewTreeFromStart)
		{
			return true;
		}
		if (method == MethodName.OnCharacterSkinSwitched)
		{
			return true;
		}
		if (method == MethodName.TrySubscribeCharacterSkinSwitch)
		{
			return true;
		}
		if (method == MethodName.TryUnsubscribeCharacterSkinSwitch)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.spriteNode)
		{
			spriteNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.previewClip)
		{
			previewClip = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefensePacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._previewHovered)
		{
			_previewHovered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._visibilityRefreshQueued)
		{
			_visibilityRefreshQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.spriteNode)
		{
			value = VariantUtils.CreateFrom(in spriteNode);
			return true;
		}
		if (name == PropertyName.previewClip)
		{
			value = VariantUtils.CreateFrom(in previewClip);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom(in sprite);
			return true;
		}
		if (name == PropertyName._previewHovered)
		{
			value = VariantUtils.CreateFrom(in _previewHovered);
			return true;
		}
		if (name == PropertyName._visibilityRefreshQueued)
		{
			value = VariantUtils.CreateFrom(in _visibilityRefreshQueued);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.spriteNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.previewClip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._previewHovered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._visibilityRefreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.spriteNode, Variant.From(in spriteNode));
		info.AddProperty(PropertyName.previewClip, Variant.From(in previewClip));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.sprite, Variant.From(in sprite));
		info.AddProperty(PropertyName._previewHovered, Variant.From(in _previewHovered));
		info.AddProperty(PropertyName._visibilityRefreshQueued, Variant.From(in _visibilityRefreshQueued));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.spriteNode, out var value))
		{
			spriteNode = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.previewClip, out var value2))
		{
			previewClip = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value3))
		{
			config = value3.As<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.sprite, out var value4))
		{
			sprite = value4.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._previewHovered, out var value5))
		{
			_previewHovered = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._visibilityRefreshQueued, out var value6))
		{
			_visibilityRefreshQueued = value6.As<bool>();
		}
	}
}
