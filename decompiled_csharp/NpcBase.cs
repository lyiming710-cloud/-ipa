using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/Npc/NpcBase.cs")]
public class NpcBase : Node2D
{
	public delegate void NpcReadyEventHandler();

	public delegate void TalkNextEventHandler();

	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName EmitNpcReady = "EmitNpcReady";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName IsLeaveAnimation = "IsLeaveAnimation";

		public static readonly StringName Talk = "Talk";

		public static readonly StringName Hand = "Hand";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _talkLabel = "_talkLabel";

		public static readonly StringName _talkTween = "_talkTween";

		public static readonly StringName _waitingForEntry = "_waitingForEntry";

		public static readonly StringName _finishing = "_finishing";

		public static readonly StringName talkrubble = "talkrubble";

		public static readonly StringName sprite = "sprite";

		public static readonly StringName animationConfig = "animationConfig";

		public static readonly StringName canPress = "canPress";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private Label _talkLabel;

	private Tween _talkTween;

	private bool _waitingForEntry;

	private bool _finishing;

	protected TextureRect talkrubble;

	[Export(PropertyHint.None, "")]
	public AdobeAnimateSprite sprite;

	[Export(PropertyHint.None, "")]
	public NpcAnimationConfig animationConfig;

	public bool canPress;

	public event NpcReadyEventHandler OnNpcReady;

	public event TalkNextEventHandler OnTalkNext;

	public void EmitNpcReady()
	{
		OnNpcReady?.Invoke();
	}

	public override void _Ready()
	{
		_talkLabel = GetNode<Label>("%TalkLabel");
		talkrubble = GetNode<TextureRect>("%TalkBubble");
		sprite.OnAnimeCompleted += AnimeCompleted;
		_waitingForEntry = !string.IsNullOrEmpty(animationConfig?.ResolveAnimation(animationConfig.enterAnimation));
		if (_waitingForEntry)
		{
			PlayAnimation(animationConfig.enterAnimation);
			return;
		}
		PlayAnimation(animationConfig?.idleAnimation);
		EmitNpcReady();
	}

	public override void _ExitTree()
	{
		_talkTween?.Kill();
		_talkTween = null;
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.OnAnimeCompleted -= AnimeCompleted;
		}
	}

	public override void _Input(InputEvent _event)
	{
		if (canPress && Input.IsActionJustPressed("Press"))
		{
			OnTalkNext?.Invoke();
			canPress = false;
		}
	}

	public virtual void AnimeCompleted(string clip)
	{
		if (IsLeaveAnimation(clip))
		{
			QueueFree();
		}
		else if (!_finishing && animationConfig != null)
		{
			bool flag = _waitingForEntry && clip == animationConfig.ResolveAnimation(animationConfig.enterAnimation);
			if (flag)
			{
				_waitingForEntry = false;
			}
			if (animationConfig.nextAnimations.TryGetValue(clip, out var value))
			{
				PlayAnimation(value);
			}
			else if (flag)
			{
				PlayAnimation(animationConfig.idleAnimation);
			}
			if (flag)
			{
				EmitNpcReady();
			}
		}
	}

	public bool IsLeaveAnimation(string animation)
	{
		return animationConfig?.IsLeaveAnimation(animation) ?? false;
	}

	private void PlayAnimation(string animation, bool? loopOverride = null)
	{
		string text = animationConfig?.ResolveAnimation(animation) ?? animation;
		if (!string.IsNullOrEmpty(text))
		{
			sprite.SetAnimation(text, loopOverride ?? animationConfig?.GetLoop(text) ?? true, animationConfig?.GetBlendTime(text) ?? 0.0);
		}
	}

	public virtual void Talk(string text, string animeClip, string audio)
	{
		if (_finishing)
		{
			return;
		}
		if (IsLeaveAnimation(animeClip))
		{
			Finish();
			return;
		}
		canPress = false;
		_talkTween?.Kill();
		AudioManager.Instance.AudioPlay(audio);
		PlayAnimation(animeClip);
		_talkLabel.Text = text;
		talkrubble.Visible = true;
		_talkTween = CreateTween();
		_talkTween.SetEase(Tween.EaseType.InOut);
		_talkTween.SetTrans(Tween.TransitionType.Quart);
		_talkTween.TweenProperty(talkrubble, "scale", Vector2.One, 0.5).From(Vector2.Zero);
		_talkTween.TweenCallback(Callable.From(() => canPress = !_finishing && IsInsideTree()));
	}

	public virtual void Hand(NpcTalkHandConfig hand)
	{
	}

	public virtual void Finish()
	{
		if (!_finishing)
		{
			_finishing = true;
			_waitingForEntry = false;
			canPress = false;
			_talkTween?.Kill();
			_talkTween = null;
			talkrubble.Visible = false;
			if (string.IsNullOrEmpty(animationConfig?.ResolveAnimation(animationConfig.leaveAnimation)))
			{
				QueueFree();
			}
			else
			{
				PlayAnimation(animationConfig.leaveAnimation, false);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.EmitNpcReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsLeaveAnimation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.EmitNpcReady && args.Count == 0)
		{
			EmitNpcReady();
			ret = default;
			return true;
		}
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
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsLeaveAnimation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLeaveAnimation(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.EmitNpcReady)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.IsLeaveAnimation)
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
		if (name == PropertyName._talkLabel)
		{
			_talkLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._talkTween)
		{
			_talkTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._waitingForEntry)
		{
			_waitingForEntry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._finishing)
		{
			_finishing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.talkrubble)
		{
			talkrubble = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.animationConfig)
		{
			animationConfig = VariantUtils.ConvertTo<NpcAnimationConfig>(in value);
			return true;
		}
		if (name == PropertyName.canPress)
		{
			canPress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._talkLabel)
		{
			value = VariantUtils.CreateFrom(in _talkLabel);
			return true;
		}
		if (name == PropertyName._talkTween)
		{
			value = VariantUtils.CreateFrom(in _talkTween);
			return true;
		}
		if (name == PropertyName._waitingForEntry)
		{
			value = VariantUtils.CreateFrom(in _waitingForEntry);
			return true;
		}
		if (name == PropertyName._finishing)
		{
			value = VariantUtils.CreateFrom(in _finishing);
			return true;
		}
		if (name == PropertyName.talkrubble)
		{
			value = VariantUtils.CreateFrom(in talkrubble);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom(in sprite);
			return true;
		}
		if (name == PropertyName.animationConfig)
		{
			value = VariantUtils.CreateFrom(in animationConfig);
			return true;
		}
		if (name == PropertyName.canPress)
		{
			value = VariantUtils.CreateFrom(in canPress);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._talkLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._talkTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._waitingForEntry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._finishing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.talkrubble, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.NodeType, "AdobeAnimateSprite", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.animationConfig, PropertyHint.ResourceType, "NpcAnimationConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canPress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._talkLabel, Variant.From(in _talkLabel));
		info.AddProperty(PropertyName._talkTween, Variant.From(in _talkTween));
		info.AddProperty(PropertyName._waitingForEntry, Variant.From(in _waitingForEntry));
		info.AddProperty(PropertyName._finishing, Variant.From(in _finishing));
		info.AddProperty(PropertyName.talkrubble, Variant.From(in talkrubble));
		info.AddProperty(PropertyName.sprite, Variant.From(in sprite));
		info.AddProperty(PropertyName.animationConfig, Variant.From(in animationConfig));
		info.AddProperty(PropertyName.canPress, Variant.From(in canPress));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._talkLabel, out var value))
		{
			_talkLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._talkTween, out var value2))
		{
			_talkTween = value2.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._waitingForEntry, out var value3))
		{
			_waitingForEntry = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._finishing, out var value4))
		{
			_finishing = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.talkrubble, out var value5))
		{
			talkrubble = value5.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.sprite, out var value6))
		{
			sprite = value6.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.animationConfig, out var value7))
		{
			animationConfig = value7.As<NpcAnimationConfig>();
		}
		if (info.TryGetProperty(PropertyName.canPress, out var value8))
		{
			canPress = value8.As<bool>();
		}
	}
}
