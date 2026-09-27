using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Boss/BossDave/ZombieBossDave.cs")]
public class ZombieBossDave : AdobeAnimateSpriteBase
{
	public new class MethodName : AdobeAnimateSpriteBase.MethodName
	{
		public static readonly StringName SetEffectFlag = "SetEffectFlag";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchPhysicsUpdate = "BatchPhysicsUpdate";

		public new static readonly StringName SetClip = "SetClip";

		public static readonly StringName SetSpawn = "SetSpawn";

		public static readonly StringName SetHeadAttack = "SetHeadAttack";

		public static readonly StringName SetRVPos = "SetRVPos";

		public static readonly StringName SetHeadAttackBall = "SetHeadAttackBall";

		public static readonly StringName PlayDriverDamage = "PlayDriverDamage";

		public static readonly StringName DamagePointSet = "DamagePointSet";

		public static readonly StringName SetArmOffset = "SetArmOffset";

		public static readonly StringName SetHeadOffset = "SetHeadOffset";

		public static readonly StringName SetRVOffset = "SetRVOffset";

		public static readonly StringName SetRVVisible = "SetRVVisible";

		public static readonly StringName GetSpawnMarkerGlobalPos = "GetSpawnMarkerGlobalPos";

		public static readonly StringName GetBallSpawnMarkerGlobalPos = "GetBallSpawnMarkerGlobalPos";

		public static readonly StringName AnimeCompleted = "AnimeCompleted";
	}

	public new class PropertyName : AdobeAnimateSpriteBase.PropertyName
	{
		public static readonly StringName _driver = "_driver";

		public static readonly StringName _arm = "_arm";

		public static readonly StringName _armNode = "_armNode";

		public static readonly StringName _rv = "_rv";

		public static readonly StringName _rvNode = "_rvNode";

		public static readonly StringName _headSprite = "_headSprite";

		public static readonly StringName _headSmoke1 = "_headSmoke1";

		public static readonly StringName _headSmoke2 = "_headSmoke2";

		public static readonly StringName _headSmoke3 = "_headSmoke3";

		public static readonly StringName _spawnMarker = "_spawnMarker";

		public static readonly StringName _ballSpawnMarker = "_ballSpawnMarker";

		public static readonly StringName armTween = "armTween";

		public static readonly StringName headTween = "headTween";
	}

	public new class SignalName : AdobeAnimateSpriteBase.SignalName
	{
	}

	private static readonly Dictionary _EFFECT_FLAGS = new Dictionary
	{
		["ash"] = 1,
		["iceSpeedDown"] = 2,
		["cover"] = 4,
		["hypnoses"] = 8,
		["imitater"] = 16,
		["redHeat"] = 32,
		["puzzle"] = 64,
		["poisoning"] = 128,
		["blink"] = 256
	};

	private const string ZOMBIE_BOSS_DAVE_HEAD = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_head.png";

	private const string ZOMBIE_BOSS_DAVE_HEAD_DAMAGE1 = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_head_damage1.png";

	private const string ZOMBIE_BOSS_DAVE_HEAD_DAMAGE2 = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_head_damage2.png";

	private const string ZOMBIE_BOSS_DAVE_JAW = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_jaw.png";

	private const string ZOMBIE_BOSS_DAVE_JAW_DAMAGE1 = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_jaw_damage1.png";

	private const string ZOMBIE_BOSS_DAVE_JAW_DAMAGE2 = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_jaw_damage2.png";

	private const string ZOMBIE_BOSS_MOUTHGLOW_BLUE = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/Boss/DamagePoint/Zombie_boss_mouthglow_blue.png";

	private const string ZOMBIE_BOSS_MOUTHGLOW_RED = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/Boss/DamagePoint/Zombie_boss_mouthglow_red.png";

	private const string ZOMBIE_BOSS_EYEGLOW_BLUE = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/Boss/DamagePoint/Zombie_boss_eyeglow_blue.png";

	private const string ZOMBIE_BOSS_EYEGLOW_RED = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/Boss/DamagePoint/Zombie_boss_eyeglow_red.png";

	private const string ZOMBIE_BOSS_MOUTHGLOW_GREEN = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_boss_mouthglow_green.png";

	private const string ZOMBIE_BOSS_MOUTHGLOW_PURPLE = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_boss_mouthglow_purple.png";

	private const string ZOMBIE_BOSS_EYEGLOW_GREEN = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_boss_eyeglow_green.png";

	private const string ZOMBIE_BOSS_EYEGLOW_PURPLE = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_boss_eyeglow_purple.png";

	private AdobeAnimateSpriteBase _driver;

	private AdobeAnimateSpriteBase _arm;

	private Node2D _armNode;

	private AdobeAnimateSpriteBase _rv;

	private Node2D _rvNode;

	private AdobeAnimatePart _headSprite;

	private GpuParticles2D _headSmoke1;

	private GpuParticles2D _headSmoke2;

	private GpuParticles2D _headSmoke3;

	private Marker2D _spawnMarker;

	private Marker2D _ballSpawnMarker;

	public Tween armTween;

	public Tween headTween;

	private void SetEffectFlag(AdobeAnimateSpriteBase _sprite, string effect, bool enable)
	{
		if (GodotObject.IsInstanceValid(_sprite) && _EFFECT_FLAGS.ContainsKey(effect))
		{
			_sprite.Modulate = (enable ? new Color(1.35f, 1.35f, 1.35f, _sprite.Modulate.A) : Colors.White);
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_driver = GetNode<AdobeAnimateSpriteBase>("%Driver");
			_arm = GetNode<AdobeAnimateSpriteBase>("%Arm");
			_armNode = GetNode<Node2D>("%ArmNode");
			_rv = GetNode<AdobeAnimateSpriteBase>("%RV");
			_rvNode = GetNode<Node2D>("%RVNode");
			_headSprite = GetNode<AdobeAnimatePart>("%HeadSprite");
			_headSmoke1 = GetNode<GpuParticles2D>("%HeadSmoke1");
			_headSmoke2 = GetNode<GpuParticles2D>("%HeadSmoke2");
			_headSmoke3 = GetNode<GpuParticles2D>("%HeadSmoke3");
			_spawnMarker = GetNode<Marker2D>("%SpawnMarker");
			_ballSpawnMarker = GetNode<Marker2D>("%BallSpawnMarker");
			OnAnimeCompleted += AnimeCompleted;
		}
	}

	public override void BatchPhysicsUpdate(double delta)
	{
		base.BatchPhysicsUpdate(delta);
		if (GodotObject.IsInstanceValid(_arm))
		{
			_arm.frameIndex = frameIndex;
			_arm.elapsedTimer = elapsedTimer;
		}
		if (GodotObject.IsInstanceValid(_rv))
		{
			_rv.frameIndex = frameIndex;
			_rv.elapsedTimer = elapsedTimer;
		}
		if (GodotObject.IsInstanceValid(_driver))
		{
			_driver.pause = pause;
		}
	}

	public override void SetClip(string clipName)
	{
		base.SetClip(clipName);
		if (GodotObject.IsInstanceValid(_headSprite))
		{
			AdobeAnimateManagedSprite2D.SetLogicalVisible(_headSprite, clipName != "Death");
		}
		if (GodotObject.IsInstanceValid(_arm))
		{
			_arm.loop = loop;
			_arm.SetClip(clipName);
		}
		if (GodotObject.IsInstanceValid(_rv))
		{
			_rv.loop = loop;
			_rv.SetClip(clipName);
		}
	}

	public void SetSpawn(int line = 1, double time = 0.5)
	{
		double num = TowerDefenseManager.Instance.GetMapGridSize().Y * ((float)line - (Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().Y / 2f) - 1f));
		SetArmOffset((float)num, time);
	}

	public void SetHeadAttack(int line = 4, double time = 1.0)
	{
		double num = TowerDefenseManager.Instance.GetMapGridSize().Y * ((float)line - (Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().Y / 2f) + 2f));
		SetHeadOffset((float)num, time);
		SetSpawn(line - ((int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().Y / 2f) + 1), time);
	}

	public void SetRVPos(Vector2I gridPos)
	{
		Vector2 vector = TowerDefenseManager.Instance.GetMapGridSize() * (new Vector2(gridPos.X, gridPos.Y) - new Vector2(1f, 2f)) - new Vector2(0f, 100f);
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
		float y = 0f;
		if (GodotObject.IsInstanceValid(mapCell))
		{
			y = (float)mapCell.GetGroundHeight(0.0);
		}
		SetRVOffset(vector - new Vector2(0f, y));
	}

	public void SetHeadAttackBall(string mode)
	{
		switch (mode)
		{
		case "Fire":
			SetAtlasReplace("Zombie_boss_mouthglow_red.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/Boss/DamagePoint/Zombie_boss_mouthglow_red.png");
			SetAtlasReplace("Zombie_boss_eyeglow_red.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/Boss/DamagePoint/Zombie_boss_eyeglow_red.png");
			break;
		case "Ice":
			SetAtlasReplace("Zombie_boss_mouthglow_red.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/Boss/DamagePoint/Zombie_boss_mouthglow_blue.png");
			SetAtlasReplace("Zombie_boss_eyeglow_red.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/Boss/DamagePoint/Zombie_boss_eyeglow_blue.png");
			break;
		case "Green":
			SetAtlasReplace("Zombie_boss_mouthglow_red.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_boss_mouthglow_green.png");
			SetAtlasReplace("Zombie_boss_eyeglow_red.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_boss_eyeglow_green.png");
			break;
		case "Purple":
			SetAtlasReplace("Zombie_boss_mouthglow_red.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_boss_mouthglow_purple.png");
			SetAtlasReplace("Zombie_boss_eyeglow_red.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_boss_eyeglow_purple.png");
			break;
		}
	}

	public void PlayDriverDamage()
	{
		if (GodotObject.IsInstanceValid(_driver))
		{
			_driver.SetAnimation("Damage", loop: false, 0.2);
		}
	}

	public void DamagePointSet(string damagePointName)
	{
		switch (damagePointName)
		{
		case "Stage1":
			SetAtlasReplace("Zombie_bossDave_head.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_head_damage1.png");
			SetAtlasReplace("Zombie_bossDave_jaw.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_jaw_damage1.png");
			if (GodotObject.IsInstanceValid(_headSprite))
			{
				_headSprite.externalAtlasTexturePath = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_head_damage1.png";
			}
			break;
		case "Stage2":
			SetAtlasReplace("Zombie_bossDave_head.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_head_damage2.png");
			SetAtlasReplace("Zombie_bossDave_jaw.png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_jaw_damage2.png");
			if (GodotObject.IsInstanceValid(_headSprite))
			{
				_headSprite.externalAtlasTexturePath = "res://Asset/AtlasSource/DamagePoint/Anime/Character/Zombie/Boss/BossDave/DamagePoint/Zombie_bossDave_head_damage2.png";
			}
			if (GodotObject.IsInstanceValid(_headSmoke1))
			{
				_headSmoke1.Visible = true;
			}
			if (GodotObject.IsInstanceValid(_headSmoke2))
			{
				_headSmoke2.Visible = true;
			}
			break;
		case "Stage3":
			SetEffectFlag(this, "blink", enable: true);
			SetEffectFlag(_arm, "blink", enable: true);
			if (GodotObject.IsInstanceValid(_headSprite))
			{
				_headSprite.Modulate = new Color(1.35f, 1.35f, 1.35f, _headSprite.Modulate.A);
			}
			if (GodotObject.IsInstanceValid(_headSmoke3))
			{
				_headSmoke3.Visible = true;
			}
			break;
		case "Death":
			if (GodotObject.IsInstanceValid(_driver))
			{
				_driver.timeScale = 1.5;
				_driver.SetAnimation("Death", loop: false, 0.2);
				_driver.AddAnimation("Flag", 0.0, loop: false);
				_driver.AddAnimation("FlagLoop", 0.0);
			}
			SetEffectFlag(this, "blink", enable: false);
			SetEffectFlag(_arm, "blink", enable: false);
			if (GodotObject.IsInstanceValid(_headSprite))
			{
				_headSprite.Modulate = Colors.White;
			}
			if (GodotObject.IsInstanceValid(_headSmoke1))
			{
				_headSmoke1.Visible = false;
			}
			if (GodotObject.IsInstanceValid(_headSmoke2))
			{
				_headSmoke2.Visible = false;
			}
			if (GodotObject.IsInstanceValid(_headSmoke3))
			{
				_headSmoke3.Visible = false;
			}
			break;
		}
	}

	public void SetArmOffset(float offset, double time = 0.5)
	{
		if (GodotObject.IsInstanceValid(armTween) && armTween.IsRunning())
		{
			armTween.Kill();
		}
		armTween = CreateTween();
		armTween.SetEase(Tween.EaseType.Out);
		armTween.SetTrans(Tween.TransitionType.Linear);
		armTween.TweenProperty(_armNode, "position:y", offset, time);
	}

	public void SetHeadOffset(float offset, double time = 1.0)
	{
		if (GodotObject.IsInstanceValid(headTween) && headTween.IsRunning())
		{
			headTween.Kill();
		}
		Vector2 vector = new Vector2(base.offset.X, -300f + offset);
		headTween = CreateTween();
		headTween.SetEase(Tween.EaseType.Out);
		headTween.SetTrans(Tween.TransitionType.Linear);
		headTween.TweenProperty(this, "offset", vector, time);
	}

	public void SetRVOffset(Vector2 offset, double time = 0.2)
	{
		if (GodotObject.IsInstanceValid(headTween) && headTween.IsRunning())
		{
			headTween.Kill();
		}
		headTween = CreateTween();
		headTween.SetEase(Tween.EaseType.Out);
		headTween.SetTrans(Tween.TransitionType.Linear);
		headTween.TweenProperty(_rvNode, "position", offset, time);
	}

	public void SetRVVisible(bool visible)
	{
		if (GodotObject.IsInstanceValid(_rvNode))
		{
			_rvNode.Visible = visible;
		}
		SetFliters((Array?)new Array<string> { "Boss_RV", "Boss_RV_wheel1", "Boss_RV_wheel2" }, !visible);
	}

	public Vector2 GetSpawnMarkerGlobalPos(TowerDefenseCharacter owner)
	{
		if (!GodotObject.IsInstanceValid(_spawnMarker) || !GodotObject.IsInstanceValid(owner))
		{
			return Vector2.Zero;
		}
		return owner.GetLogicalGlobalPosition(_spawnMarker);
	}

	public Vector2 GetBallSpawnMarkerGlobalPos(TowerDefenseCharacter owner)
	{
		if (!GodotObject.IsInstanceValid(_ballSpawnMarker) || !GodotObject.IsInstanceValid(owner))
		{
			return Vector2.Zero;
		}
		UpdateChild();
		return owner.GetLogicalGlobalPosition(_ballSpawnMarker);
	}

	public void AnimeCompleted(string _clip)
	{
		if (!(_clip == "Spawn1"))
		{
			if (_clip == "RV")
			{
				SetRVVisible(visible: false);
				if (GodotObject.IsInstanceValid(_rvNode))
				{
					_rvNode.Position = Vector2.Zero;
				}
			}
		}
		else
		{
			SetSpawn();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName.SetEffectFlag, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "effect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "enable", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchPhysicsUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHeadAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRVPos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHeadAttackBall, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayDriverDamage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetArmOffset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHeadOffset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRVOffset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRVVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSpawnMarkerGlobalPos, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetBallSpawnMarkerGlobalPos, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetEffectFlag && args.Count == 3)
		{
			SetEffectFlag(VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchPhysicsUpdate && args.Count == 1)
		{
			BatchPhysicsUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetClip && args.Count == 1)
		{
			SetClip(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSpawn && args.Count == 2)
		{
			SetSpawn(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHeadAttack && args.Count == 2)
		{
			SetHeadAttack(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRVPos && args.Count == 1)
		{
			SetRVPos(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHeadAttackBall && args.Count == 1)
		{
			SetHeadAttackBall(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayDriverDamage && args.Count == 0)
		{
			PlayDriverDamage();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointSet && args.Count == 1)
		{
			DamagePointSet(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetArmOffset && args.Count == 2)
		{
			SetArmOffset(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHeadOffset && args.Count == 2)
		{
			SetHeadOffset(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRVOffset && args.Count == 2)
		{
			SetRVOffset(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRVVisible && args.Count == 1)
		{
			SetRVVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSpawnMarkerGlobalPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetSpawnMarkerGlobalPos(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.GetBallSpawnMarkerGlobalPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetBallSpawnMarkerGlobalPos(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SetEffectFlag)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.BatchPhysicsUpdate)
		{
			return true;
		}
		if (method == MethodName.SetClip)
		{
			return true;
		}
		if (method == MethodName.SetSpawn)
		{
			return true;
		}
		if (method == MethodName.SetHeadAttack)
		{
			return true;
		}
		if (method == MethodName.SetRVPos)
		{
			return true;
		}
		if (method == MethodName.SetHeadAttackBall)
		{
			return true;
		}
		if (method == MethodName.PlayDriverDamage)
		{
			return true;
		}
		if (method == MethodName.DamagePointSet)
		{
			return true;
		}
		if (method == MethodName.SetArmOffset)
		{
			return true;
		}
		if (method == MethodName.SetHeadOffset)
		{
			return true;
		}
		if (method == MethodName.SetRVOffset)
		{
			return true;
		}
		if (method == MethodName.SetRVVisible)
		{
			return true;
		}
		if (method == MethodName.GetSpawnMarkerGlobalPos)
		{
			return true;
		}
		if (method == MethodName.GetBallSpawnMarkerGlobalPos)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._driver)
		{
			_driver = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._arm)
		{
			_arm = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._armNode)
		{
			_armNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._rv)
		{
			_rv = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._rvNode)
		{
			_rvNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._headSprite)
		{
			_headSprite = VariantUtils.ConvertTo<AdobeAnimatePart>(in value);
			return true;
		}
		if (name == PropertyName._headSmoke1)
		{
			_headSmoke1 = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._headSmoke2)
		{
			_headSmoke2 = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._headSmoke3)
		{
			_headSmoke3 = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._spawnMarker)
		{
			_spawnMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName._ballSpawnMarker)
		{
			_ballSpawnMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.armTween)
		{
			armTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName.headTween)
		{
			headTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._driver)
		{
			value = VariantUtils.CreateFrom(in _driver);
			return true;
		}
		if (name == PropertyName._arm)
		{
			value = VariantUtils.CreateFrom(in _arm);
			return true;
		}
		if (name == PropertyName._armNode)
		{
			value = VariantUtils.CreateFrom(in _armNode);
			return true;
		}
		if (name == PropertyName._rv)
		{
			value = VariantUtils.CreateFrom(in _rv);
			return true;
		}
		if (name == PropertyName._rvNode)
		{
			value = VariantUtils.CreateFrom(in _rvNode);
			return true;
		}
		if (name == PropertyName._headSprite)
		{
			value = VariantUtils.CreateFrom(in _headSprite);
			return true;
		}
		if (name == PropertyName._headSmoke1)
		{
			value = VariantUtils.CreateFrom(in _headSmoke1);
			return true;
		}
		if (name == PropertyName._headSmoke2)
		{
			value = VariantUtils.CreateFrom(in _headSmoke2);
			return true;
		}
		if (name == PropertyName._headSmoke3)
		{
			value = VariantUtils.CreateFrom(in _headSmoke3);
			return true;
		}
		if (name == PropertyName._spawnMarker)
		{
			value = VariantUtils.CreateFrom(in _spawnMarker);
			return true;
		}
		if (name == PropertyName._ballSpawnMarker)
		{
			value = VariantUtils.CreateFrom(in _ballSpawnMarker);
			return true;
		}
		if (name == PropertyName.armTween)
		{
			value = VariantUtils.CreateFrom(in armTween);
			return true;
		}
		if (name == PropertyName.headTween)
		{
			value = VariantUtils.CreateFrom(in headTween);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._driver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._arm, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._armNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rv, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rvNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._headSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._headSmoke1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._headSmoke2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._headSmoke3, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spawnMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ballSpawnMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.armTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.headTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._driver, Variant.From(in _driver));
		info.AddProperty(PropertyName._arm, Variant.From(in _arm));
		info.AddProperty(PropertyName._armNode, Variant.From(in _armNode));
		info.AddProperty(PropertyName._rv, Variant.From(in _rv));
		info.AddProperty(PropertyName._rvNode, Variant.From(in _rvNode));
		info.AddProperty(PropertyName._headSprite, Variant.From(in _headSprite));
		info.AddProperty(PropertyName._headSmoke1, Variant.From(in _headSmoke1));
		info.AddProperty(PropertyName._headSmoke2, Variant.From(in _headSmoke2));
		info.AddProperty(PropertyName._headSmoke3, Variant.From(in _headSmoke3));
		info.AddProperty(PropertyName._spawnMarker, Variant.From(in _spawnMarker));
		info.AddProperty(PropertyName._ballSpawnMarker, Variant.From(in _ballSpawnMarker));
		info.AddProperty(PropertyName.armTween, Variant.From(in armTween));
		info.AddProperty(PropertyName.headTween, Variant.From(in headTween));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._driver, out var value))
		{
			_driver = value.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._arm, out var value2))
		{
			_arm = value2.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._armNode, out var value3))
		{
			_armNode = value3.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._rv, out var value4))
		{
			_rv = value4.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._rvNode, out var value5))
		{
			_rvNode = value5.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._headSprite, out var value6))
		{
			_headSprite = value6.As<AdobeAnimatePart>();
		}
		if (info.TryGetProperty(PropertyName._headSmoke1, out var value7))
		{
			_headSmoke1 = value7.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._headSmoke2, out var value8))
		{
			_headSmoke2 = value8.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._headSmoke3, out var value9))
		{
			_headSmoke3 = value9.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._spawnMarker, out var value10))
		{
			_spawnMarker = value10.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName._ballSpawnMarker, out var value11))
		{
			_ballSpawnMarker = value11.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.armTween, out var value12))
		{
			armTween = value12.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName.headTween, out var value13))
		{
			headTween = value13.As<Tween>();
		}
	}
}
