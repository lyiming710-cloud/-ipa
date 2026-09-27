using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Effect/Once/TowerDefenseEffectSpriteOnce.cs")]
public class TowerDefenseEffectSpriteOnce : TowerDefenseEffectBase
{
	public new class MethodName : TowerDefenseEffectBase.MethodName
	{
		public static readonly StringName MarkGpuBatchEligibilityChanged = "MarkGpuBatchEligibilityChanged";

		public static readonly StringName GetGpuBatchVisualRevision = "GetGpuBatchVisualRevision";

		public static readonly StringName GetGpuBatchEligibilityRevision = "GetGpuBatchEligibilityRevision";

		public static readonly StringName Create = "Create";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName Recycle = "Recycle";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitScene = "InitScene";

		public static readonly StringName Init = "Init";

		public static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName AbortGpuBatchAndResumeCpu = "AbortGpuBatchAndResumeCpu";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName CompleteClip = "CompleteClip";

		public static readonly StringName IsValid = "IsValid";

		public static readonly StringName ConnectSpriteSignals = "ConnectSpriteSignals";

		public static readonly StringName DisconnectSpriteSignals = "DisconnectSpriteSignals";

		public static readonly StringName EnsureSpriteChild = "EnsureSpriteChild";

		public static readonly StringName ConfigureSpriteForRuntimeManager = "ConfigureSpriteForRuntimeManager";

		public static readonly StringName PlayCurrentClip = "PlayCurrentClip";

		public static readonly StringName TryActivatePreparedGpuBatchForCurrentClip = "TryActivatePreparedGpuBatchForCurrentClip";

		public static readonly StringName TryActivateGpuBatch = "TryActivateGpuBatch";

		public static readonly StringName StopGpuBatch = "StopGpuBatch";
	}

	public new class PropertyName : TowerDefenseEffectBase.PropertyName
	{
		public static readonly StringName sprite = "sprite";

		public static readonly StringName useGpuBatch = "useGpuBatch";

		public new static readonly StringName Modulate = "Modulate";

		public new static readonly StringName SelfModulate = "SelfModulate";

		public new static readonly StringName ZIndex = "ZIndex";

		public new static readonly StringName ZAsRelative = "ZAsRelative";

		public static readonly StringName objectId = "objectId";

		public static readonly StringName _sprite = "_sprite";

		public static readonly StringName clipsList = "clipsList";

		public static readonly StringName _useGpuBatch = "_useGpuBatch";

		public static readonly StringName currentIndex = "currentIndex";

		public static readonly StringName _animeCompletedSprite = "_animeCompletedSprite";

		public static readonly StringName _gpuBatchActive = "_gpuBatchActive";

		public static readonly StringName _gpuBatcher = "_gpuBatcher";

		public static readonly StringName _gpuSuppressionToken = "_gpuSuppressionToken";

		public static readonly StringName _gpuSuppressedSprite = "_gpuSuppressedSprite";

		public static readonly StringName _gpuBatchVisualRevision = "_gpuBatchVisualRevision";

		public static readonly StringName _gpuBatchEligibilityRevision = "_gpuBatchEligibilityRevision";

		public static readonly StringName _preparedRequestedClip = "_preparedRequestedClip";
	}

	public new class SignalName : TowerDefenseEffectBase.SignalName
	{
	}

	private static PackedScene _towerDefenseEffectSpriteOnce;

	private static readonly StringName EffectGroupName = "Effect";

	[Export(PropertyHint.None, "")]
	public ObjectManagerConfig.OBJECT objectId;

	private AdobeAnimateSprite _sprite;

	[Export(PropertyHint.None, "")]
	public string[] clipsList;

	private bool _useGpuBatch = true;

	public int currentIndex;

	private AdobeAnimateSprite _animeCompletedSprite;

	private bool _gpuBatchActive;

	private TowerDefenseEffectSpriteOnceBatcher _gpuBatcher;

	private int _gpuSuppressionToken;

	private AdobeAnimateSprite _gpuSuppressedSprite;

	private int _gpuBatchVisualRevision;

	private int _gpuBatchEligibilityRevision;

	private string _preparedRequestedClip;

	private static PackedScene TOWER_DEFENSE_EFFECT_SPRITE_ONCE => _towerDefenseEffectSpriteOnce ?? (_towerDefenseEffectSpriteOnce = GD.Load<PackedScene>("uid://dwvgduivkprow"));

	[Export(PropertyHint.None, "")]
	public AdobeAnimateSprite sprite
	{
		get
		{
			return _sprite;
		}
		set
		{
			if (_sprite != value)
			{
				_sprite = value;
				MarkGpuBatchEligibilityChanged();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public bool useGpuBatch
	{
		get
		{
			return _useGpuBatch;
		}
		set
		{
			if (_useGpuBatch != value)
			{
				_useGpuBatch = value;
				MarkGpuBatchEligibilityChanged();
			}
		}
	}

	public new Color Modulate
	{
		get
		{
			return base.Modulate;
		}
		set
		{
			if (!(base.Modulate == value))
			{
				base.Modulate = value;
				_gpuBatchVisualRevision++;
				MarkGpuBatchEligibilityChanged();
			}
		}
	}

	public new Color SelfModulate
	{
		get
		{
			return base.SelfModulate;
		}
		set
		{
			if (!(base.SelfModulate == value))
			{
				base.SelfModulate = value;
				_gpuBatchVisualRevision++;
				MarkGpuBatchEligibilityChanged();
			}
		}
	}

	public new int ZIndex
	{
		get
		{
			return base.ZIndex;
		}
		set
		{
			if (base.ZIndex != value)
			{
				base.ZIndex = value;
				if (IsValid(sprite))
				{
					sprite.InvalidateEffectOnceBatchRenderState();
				}
			}
		}
	}

	public new bool ZAsRelative
	{
		get
		{
			return base.ZAsRelative;
		}
		set
		{
			if (base.ZAsRelative != value)
			{
				base.ZAsRelative = value;
				if (IsValid(sprite))
				{
					sprite.InvalidateEffectOnceBatchRenderState();
				}
			}
		}
	}

	internal event Action EffectOnceBatchEligibilityChanged;

	private void MarkGpuBatchEligibilityChanged()
	{
		_gpuBatchEligibilityRevision++;
		EffectOnceBatchEligibilityChanged?.Invoke();
	}

	internal int GetGpuBatchVisualRevision()
	{
		return _gpuBatchVisualRevision;
	}

	internal int GetGpuBatchEligibilityRevision()
	{
		return _gpuBatchEligibilityRevision;
	}

	public static TowerDefenseEffectSpriteOnce Create()
	{
		return TOWER_DEFENSE_EFFECT_SPRITE_ONCE.Instantiate<TowerDefenseEffectSpriteOnce>(PackedScene.GenEditState.Disabled);
	}

	public void Refresh()
	{
		AddToGroup(EffectGroupName);
		currentIndex = 0;
		StopGpuBatch();
		ConnectSpriteSignals();
		ConfigureSpriteForRuntimeManager();
		PlayCurrentClip(0.0);
	}

	public void Recycle()
	{
		RemoveFromGroup(EffectGroupName);
		StopGpuBatch();
		if (IsValid(sprite))
		{
			sprite.pause = true;
			sprite.Visible = false;
			sprite.RefreshProcessScheduling();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		ConnectSpriteSignals();
		ConfigureSpriteForRuntimeManager();
		if (!TryActivatePreparedGpuBatchForCurrentClip())
		{
			PlayCurrentClip(0.0);
		}
	}

	public void InitScene(AdobeAnimateSprite _sprite, string _clip = "")
	{
		StopGpuBatch();
		if (sprite != _sprite)
		{
			DisconnectSpriteSignals();
			sprite = _sprite;
		}
		if (_clip != "")
		{
			currentIndex = 0;
			clipsList = _clip.Split('|', StringSplitOptions.RemoveEmptyEntries);
		}
		EnsureSpriteChild();
		ConnectSpriteSignals();
		ConfigureSpriteForRuntimeManager();
		PlayCurrentClip(0.0);
	}

	public void Init(PackedScene spriteScene, string _clip = "")
	{
		if (spriteScene != null)
		{
			AdobeAnimateSprite adobeAnimateSprite = spriteScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			InitScene(adobeAnimateSprite, _clip);
		}
	}

	public void AnimeCompleted(string clip)
	{
		if (_gpuBatchActive)
		{
			StopGpuBatch();
		}
		CompleteClip();
	}

	internal void AbortGpuBatchAndResumeCpu(string clip, float elapsedFrame)
	{
		AdobeAnimateSprite gpuSuppressedSprite = _gpuSuppressedSprite;
		AdobeAnimateSprite adobeAnimateSprite = sprite;
		int gpuSuppressionToken = _gpuSuppressionToken;
		_gpuBatchActive = false;
		_gpuBatcher = null;
		_gpuSuppressionToken = 0;
		_gpuSuppressedSprite = null;
		if (IsValid(gpuSuppressedSprite))
		{
			gpuSuppressedSprite.EndEffectOnceGpuBatchSuppression(gpuSuppressionToken);
		}
		if (!IsValid(adobeAnimateSprite))
		{
			return;
		}
		if (adobeAnimateSprite == gpuSuppressedSprite)
		{
			adobeAnimateSprite.forceLocalRender = false;
			adobeAnimateSprite.RefreshProcessScheduling();
			return;
		}
		if (IsValid(gpuSuppressedSprite))
		{
			gpuSuppressedSprite.Visible = false;
			gpuSuppressedSprite.pause = true;
		}
		EnsureSpriteChild();
		ConnectSpriteSignals();
		ConfigureSpriteForRuntimeManager();
		if (!string.IsNullOrEmpty(clip))
		{
			adobeAnimateSprite.SetAnimation(clip, loop: false);
			adobeAnimateSprite.TryImportEffectOnceGpuPlaybackPositionWithoutEvents(adobeAnimateSprite.clip, elapsedFrame);
		}
	}

	public override void _ExitTree()
	{
		StopGpuBatch();
		DisconnectSpriteSignals();
	}

	private void CompleteClip()
	{
		if (clipsList != null && clipsList.Length != 0 && currentIndex < clipsList.Length - 1)
		{
			currentIndex++;
			PlayCurrentClip(0.1);
		}
		else if (objectId == ObjectManagerConfig.OBJECT.NOONE)
		{
			QueueFree();
		}
		else
		{
			ObjectManager.PoolPush(objectId, this);
		}
	}

	private static bool IsValid(GodotObject obj)
	{
		if (obj != null)
		{
			return GodotObject.IsInstanceValid(obj);
		}
		return false;
	}

	private void ConnectSpriteSignals()
	{
		if (_animeCompletedSprite != sprite || !IsValid(_animeCompletedSprite))
		{
			DisconnectSpriteSignals();
			if (IsValid(sprite))
			{
				sprite.OnAnimeCompleted += AnimeCompleted;
				_animeCompletedSprite = sprite;
			}
		}
	}

	private void DisconnectSpriteSignals()
	{
		AdobeAnimateSprite animeCompletedSprite = _animeCompletedSprite;
		_animeCompletedSprite = null;
		if (IsValid(animeCompletedSprite))
		{
			animeCompletedSprite.OnAnimeCompleted -= AnimeCompleted;
		}
	}

	private void EnsureSpriteChild()
	{
		if (!IsValid(sprite))
		{
			return;
		}
		Node parent = sprite.GetParent();
		if (parent != this)
		{
			if (IsValid(parent))
			{
				sprite.Reparent(this, keepGlobalTransform: false);
			}
			else
			{
				AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void ConfigureSpriteForRuntimeManager()
	{
		if (IsValid(sprite))
		{
			sprite.forceLocalRender = false;
			sprite.pause = false;
			sprite.Visible = true;
			sprite.RefreshProcessScheduling();
		}
	}

	private void PlayCurrentClip(double blendTime)
	{
		if (!IsValid(sprite) || clipsList == null || clipsList.Length == 0)
		{
			return;
		}
		if (currentIndex < 0 || currentIndex >= clipsList.Length)
		{
			currentIndex = 0;
		}
		string text = clipsList[currentIndex];
		if (!string.IsNullOrEmpty(text))
		{
			StopGpuBatch();
			ConfigureSpriteForRuntimeManager();
			sprite.SetAnimation(text, loop: false, blendTime);
			_preparedRequestedClip = text;
			if (clipsList.Length <= 1 && !(blendTime > 0.0) && !sprite.blend)
			{
				TryActivateGpuBatch(sprite.clip);
			}
		}
	}

	private bool TryActivatePreparedGpuBatchForCurrentClip()
	{
		if (!IsValid(sprite) || clipsList == null || clipsList.Length == 0)
		{
			return false;
		}
		if (currentIndex < 0 || currentIndex >= clipsList.Length)
		{
			currentIndex = 0;
		}
		string b = clipsList[currentIndex];
		if (clipsList.Length > 1 || !string.Equals(_preparedRequestedClip, b, StringComparison.Ordinal) || !sprite.IsEffectOnceGpuBatchPlaybackPrepared(sprite.clip))
		{
			return false;
		}
		return TryActivateGpuBatch(sprite.clip);
	}

	private bool TryActivateGpuBatch(string clip)
	{
		if (!useGpuBatch || Engine.IsEditorHint() || !IsInsideTree() || !IsValid(sprite) || string.IsNullOrEmpty(clip))
		{
			return false;
		}
		TowerDefenseEffectSpriteOnceBatcher orCreate = TowerDefenseEffectSpriteOnceBatcher.GetOrCreate();
		if (!GodotObject.IsInstanceValid(orCreate))
		{
			return false;
		}
		int num = sprite.BeginEffectOnceGpuBatchSuppression();
		if (!orCreate.RegisterOrRestart(this, sprite, clip))
		{
			sprite.EndEffectOnceGpuBatchSuppression(num);
			return false;
		}
		_gpuBatcher = orCreate;
		_gpuBatchActive = true;
		_gpuSuppressedSprite = sprite;
		_gpuSuppressionToken = num;
		return true;
	}

	private void StopGpuBatch()
	{
		AdobeAnimateSprite gpuSuppressedSprite = _gpuSuppressedSprite;
		int gpuSuppressionToken = _gpuSuppressionToken;
		if (_gpuBatchActive)
		{
			if (GodotObject.IsInstanceValid(_gpuBatcher))
			{
				_gpuBatcher.Unregister(this);
			}
			else
			{
				TowerDefenseEffectSpriteOnceBatcher.UnregisterIfExists(this);
			}
		}
		_gpuBatcher = null;
		_gpuBatchActive = false;
		_gpuSuppressionToken = 0;
		_gpuSuppressedSprite = null;
		if (IsValid(gpuSuppressedSprite))
		{
			gpuSuppressedSprite.EndEffectOnceGpuBatchSuppression(gpuSuppressionToken);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(22)
		{
			new MethodInfo(MethodName.MarkGpuBatchEligibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGpuBatchVisualRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGpuBatchEligibilityRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Recycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "_clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spriteScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.String, "_clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AbortGpuBatchAndResumeCpu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "elapsedFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsValid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectSpriteSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectSpriteSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureSpriteChild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureSpriteForRuntimeManager, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayCurrentClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "blendTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryActivatePreparedGpuBatchForCurrentClip, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryActivateGpuBatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StopGpuBatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.MarkGpuBatchEligibilityChanged && args.Count == 0)
		{
			MarkGpuBatchEligibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.GetGpuBatchVisualRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetGpuBatchVisualRevision());
			return true;
		}
		if (method == MethodName.GetGpuBatchEligibilityRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetGpuBatchEligibilityRevision());
			return true;
		}
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectSpriteOnce>(Create());
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.Recycle && args.Count == 0)
		{
			Recycle();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.InitScene && args.Count == 2)
		{
			InitScene(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 2)
		{
			Init(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AbortGpuBatchAndResumeCpu && args.Count == 2)
		{
			AbortGpuBatchAndResumeCpu(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteClip && args.Count == 0)
		{
			CompleteClip();
			ret = default;
			return true;
		}
		if (method == MethodName.IsValid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValid(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.ConnectSpriteSignals && args.Count == 0)
		{
			ConnectSpriteSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectSpriteSignals && args.Count == 0)
		{
			DisconnectSpriteSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureSpriteChild && args.Count == 0)
		{
			EnsureSpriteChild();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureSpriteForRuntimeManager && args.Count == 0)
		{
			ConfigureSpriteForRuntimeManager();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayCurrentClip && args.Count == 1)
		{
			PlayCurrentClip(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryActivatePreparedGpuBatchForCurrentClip && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryActivatePreparedGpuBatchForCurrentClip());
			return true;
		}
		if (method == MethodName.TryActivateGpuBatch && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryActivateGpuBatch(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.StopGpuBatch && args.Count == 0)
		{
			StopGpuBatch();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectSpriteOnce>(Create());
			return true;
		}
		if (method == MethodName.IsValid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValid(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.MarkGpuBatchEligibilityChanged)
		{
			return true;
		}
		if (method == MethodName.GetGpuBatchVisualRevision)
		{
			return true;
		}
		if (method == MethodName.GetGpuBatchEligibilityRevision)
		{
			return true;
		}
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.Recycle)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.InitScene)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.AbortGpuBatchAndResumeCpu)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.CompleteClip)
		{
			return true;
		}
		if (method == MethodName.IsValid)
		{
			return true;
		}
		if (method == MethodName.ConnectSpriteSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectSpriteSignals)
		{
			return true;
		}
		if (method == MethodName.EnsureSpriteChild)
		{
			return true;
		}
		if (method == MethodName.ConfigureSpriteForRuntimeManager)
		{
			return true;
		}
		if (method == MethodName.PlayCurrentClip)
		{
			return true;
		}
		if (method == MethodName.TryActivatePreparedGpuBatchForCurrentClip)
		{
			return true;
		}
		if (method == MethodName.TryActivateGpuBatch)
		{
			return true;
		}
		if (method == MethodName.StopGpuBatch)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.useGpuBatch)
		{
			useGpuBatch = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Modulate)
		{
			Modulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.SelfModulate)
		{
			SelfModulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.ZIndex)
		{
			ZIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ZAsRelative)
		{
			ZAsRelative = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.objectId)
		{
			objectId = VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in value);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			_sprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.clipsList)
		{
			clipsList = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName._useGpuBatch)
		{
			_useGpuBatch = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.currentIndex)
		{
			currentIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animeCompletedSprite)
		{
			_animeCompletedSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._gpuBatchActive)
		{
			_gpuBatchActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._gpuBatcher)
		{
			_gpuBatcher = VariantUtils.ConvertTo<TowerDefenseEffectSpriteOnceBatcher>(in value);
			return true;
		}
		if (name == PropertyName._gpuSuppressionToken)
		{
			_gpuSuppressionToken = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gpuSuppressedSprite)
		{
			_gpuSuppressedSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._gpuBatchVisualRevision)
		{
			_gpuBatchVisualRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gpuBatchEligibilityRevision)
		{
			_gpuBatchEligibilityRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._preparedRequestedClip)
		{
			_preparedRequestedClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateSprite>(sprite);
			return true;
		}
		bool from;
		if (name == PropertyName.useGpuBatch)
		{
			from = useGpuBatch;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Color from2;
		if (name == PropertyName.Modulate)
		{
			from2 = Modulate;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.SelfModulate)
		{
			from2 = SelfModulate;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ZIndex)
		{
			value = VariantUtils.CreateFrom<int>(ZIndex);
			return true;
		}
		if (name == PropertyName.ZAsRelative)
		{
			from = ZAsRelative;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.objectId)
		{
			value = VariantUtils.CreateFrom(in objectId);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			value = VariantUtils.CreateFrom(in _sprite);
			return true;
		}
		if (name == PropertyName.clipsList)
		{
			value = VariantUtils.CreateFrom(in clipsList);
			return true;
		}
		if (name == PropertyName._useGpuBatch)
		{
			value = VariantUtils.CreateFrom(in _useGpuBatch);
			return true;
		}
		if (name == PropertyName.currentIndex)
		{
			value = VariantUtils.CreateFrom(in currentIndex);
			return true;
		}
		if (name == PropertyName._animeCompletedSprite)
		{
			value = VariantUtils.CreateFrom(in _animeCompletedSprite);
			return true;
		}
		if (name == PropertyName._gpuBatchActive)
		{
			value = VariantUtils.CreateFrom(in _gpuBatchActive);
			return true;
		}
		if (name == PropertyName._gpuBatcher)
		{
			value = VariantUtils.CreateFrom(in _gpuBatcher);
			return true;
		}
		if (name == PropertyName._gpuSuppressionToken)
		{
			value = VariantUtils.CreateFrom(in _gpuSuppressionToken);
			return true;
		}
		if (name == PropertyName._gpuSuppressedSprite)
		{
			value = VariantUtils.CreateFrom(in _gpuSuppressedSprite);
			return true;
		}
		if (name == PropertyName._gpuBatchVisualRevision)
		{
			value = VariantUtils.CreateFrom(in _gpuBatchVisualRevision);
			return true;
		}
		if (name == PropertyName._gpuBatchEligibilityRevision)
		{
			value = VariantUtils.CreateFrom(in _gpuBatchEligibilityRevision);
			return true;
		}
		if (name == PropertyName._preparedRequestedClip)
		{
			value = VariantUtils.CreateFrom(in _preparedRequestedClip);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.objectId, PropertyHint.Enum, "NOONE:0,PROJECTILE:1,damagePart:2,GAMECOLLECT:10,SUN:11,SUN_BRAIN:12,SUN_JALAPENO:13,SUN_QX:14,SUN_MAGIC:15,COIN:20,COIN_SILVER:21,COIN_GOLD:22,COIN_DIAMOND:23,COIN_LUCKY_BAG:24,COIN_TQ:25,COIN_YB1:26,COIN_YB2:27,COIN_GOLD_SHARD:28,PARTICLES_SPLASH:201,PARTICLES_RISE_DIRT:202,PARTICLES_ICE_TRAP:203,SHOW_HEALTH_VIEW:301,MAX:1000", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.NodeType, "AdobeAnimateSprite", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName.clipsList, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._useGpuBatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useGpuBatch, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animeCompletedSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._gpuBatchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gpuBatcher, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuSuppressionToken, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gpuSuppressedSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuBatchVisualRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuBatchEligibilityRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._preparedRequestedClip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName.Modulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName.SelfModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ZIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ZAsRelative, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.sprite, Variant.From<AdobeAnimateSprite>(sprite));
		info.AddProperty(PropertyName.useGpuBatch, Variant.From<bool>(useGpuBatch));
		info.AddProperty(PropertyName.Modulate, Variant.From<Color>(Modulate));
		info.AddProperty(PropertyName.SelfModulate, Variant.From<Color>(SelfModulate));
		info.AddProperty(PropertyName.ZIndex, Variant.From<int>(ZIndex));
		info.AddProperty(PropertyName.ZAsRelative, Variant.From<bool>(ZAsRelative));
		info.AddProperty(PropertyName.objectId, Variant.From(in objectId));
		info.AddProperty(PropertyName._sprite, Variant.From(in _sprite));
		info.AddProperty(PropertyName.clipsList, Variant.From(in clipsList));
		info.AddProperty(PropertyName._useGpuBatch, Variant.From(in _useGpuBatch));
		info.AddProperty(PropertyName.currentIndex, Variant.From(in currentIndex));
		info.AddProperty(PropertyName._animeCompletedSprite, Variant.From(in _animeCompletedSprite));
		info.AddProperty(PropertyName._gpuBatchActive, Variant.From(in _gpuBatchActive));
		info.AddProperty(PropertyName._gpuBatcher, Variant.From(in _gpuBatcher));
		info.AddProperty(PropertyName._gpuSuppressionToken, Variant.From(in _gpuSuppressionToken));
		info.AddProperty(PropertyName._gpuSuppressedSprite, Variant.From(in _gpuSuppressedSprite));
		info.AddProperty(PropertyName._gpuBatchVisualRevision, Variant.From(in _gpuBatchVisualRevision));
		info.AddProperty(PropertyName._gpuBatchEligibilityRevision, Variant.From(in _gpuBatchEligibilityRevision));
		info.AddProperty(PropertyName._preparedRequestedClip, Variant.From(in _preparedRequestedClip));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.sprite, out var value))
		{
			sprite = value.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.useGpuBatch, out var value2))
		{
			useGpuBatch = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Modulate, out var value3))
		{
			Modulate = value3.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.SelfModulate, out var value4))
		{
			SelfModulate = value4.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.ZIndex, out var value5))
		{
			ZIndex = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ZAsRelative, out var value6))
		{
			ZAsRelative = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.objectId, out var value7))
		{
			objectId = value7.As<ObjectManagerConfig.OBJECT>();
		}
		if (info.TryGetProperty(PropertyName._sprite, out var value8))
		{
			_sprite = value8.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.clipsList, out var value9))
		{
			clipsList = value9.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName._useGpuBatch, out var value10))
		{
			_useGpuBatch = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.currentIndex, out var value11))
		{
			currentIndex = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animeCompletedSprite, out var value12))
		{
			_animeCompletedSprite = value12.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._gpuBatchActive, out var value13))
		{
			_gpuBatchActive = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._gpuBatcher, out var value14))
		{
			_gpuBatcher = value14.As<TowerDefenseEffectSpriteOnceBatcher>();
		}
		if (info.TryGetProperty(PropertyName._gpuSuppressionToken, out var value15))
		{
			_gpuSuppressionToken = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gpuSuppressedSprite, out var value16))
		{
			_gpuSuppressedSprite = value16.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._gpuBatchVisualRevision, out var value17))
		{
			_gpuBatchVisualRevision = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gpuBatchEligibilityRevision, out var value18))
		{
			_gpuBatchEligibilityRevision = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName._preparedRequestedClip, out var value19))
		{
			_preparedRequestedClip = value19.As<string>();
		}
	}
}
