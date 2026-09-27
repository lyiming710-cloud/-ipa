using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/AttackComponent/AttackComponentDefinition.cs")]
public class AttackComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName checkShapeResources = "checkShapeResources";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";

		public static readonly StringName attackType = "attackType";

		public static readonly StringName useParentHitBox = "useParentHitBox";

		public static readonly StringName checkIntreval = "checkIntreval";

		public static readonly StringName spritePath = "spritePath";

		public static readonly StringName attackAnimeClipsArray = "attackAnimeClipsArray";

		public static readonly StringName attackAnimeClips = "attackAnimeClips";

		public static readonly StringName attackEventName = "attackEventName";

		public static readonly StringName attackAnimeTimeScale = "attackAnimeTimeScale";

		public static readonly StringName attackIntervalBase = "attackIntervalBase";

		public static readonly StringName attackInterval = "attackInterval";

		public static readonly StringName attackIntervalOffset = "attackIntervalOffset";

		public static readonly StringName spliceIdleAnimeClips = "spliceIdleAnimeClips";

		public static readonly StringName spliceIdleAnimeTimeScale = "spliceIdleAnimeTimeScale";

		public static readonly StringName eventList = "eventList";

		public static readonly StringName useZombieAttackCheck = "useZombieAttackCheck";

		public static readonly StringName checkGrid = "checkGrid";

		public static readonly StringName useCheckAreaGridColumn = "useCheckAreaGridColumn";

		public static readonly StringName checkEachShape = "checkEachShape";

		public static readonly StringName checkLine = "checkLine";

		public static readonly StringName checkTall = "checkTall";

		public static readonly StringName checkVase = "checkVase";

		public static readonly StringName checkBowling = "checkBowling";

		public static readonly StringName checkGravestone = "checkGravestone";

		public static readonly StringName checkAll = "checkAll";

		public static readonly StringName fliterLadder = "fliterLadder";

		public static readonly StringName eatAudio = "eatAudio";

		public static readonly StringName plantDefeatAudio = "plantDefeatAudio";

		public static readonly StringName eatAudioInterval = "eatAudioInterval";

		public static readonly StringName playPlantDefeatAudio = "playPlantDefeatAudio";

		public static readonly StringName attackStateEvent = "attackStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.Enum, "Default,Eat,Smash,Chomp")]
	public string attackType = "Eat";

	[ExportGroup("Collision", "")]
	[Export(PropertyHint.None, "")]
	public bool useParentHitBox;

	[Export(PropertyHint.Range, "0,120,1")]
	public int checkIntreval = 5;

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public NodePath spritePath = new NodePath();

	[Export(PropertyHint.None, "")]
	public Array<string> attackAnimeClipsArray = new Array<string> { "Attack" };

	[Export(PropertyHint.None, "")]
	public string attackAnimeClips = "Attack";

	[Export(PropertyHint.None, "")]
	public string attackEventName = "attack";

	[Export(PropertyHint.None, "")]
	public double attackAnimeTimeScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double attackIntervalBase = 1.5;

	[Export(PropertyHint.None, "")]
	public double attackInterval = 1.5;

	[Export(PropertyHint.None, "")]
	public double attackIntervalOffset = 0.1;

	[Export(PropertyHint.None, "")]
	public string spliceIdleAnimeClips = "";

	[Export(PropertyHint.None, "")]
	public double spliceIdleAnimeTimeScale = 1.0;

	[ExportGroup("Attack Events", "")]
	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList = new Array<TowerDefenseCharacterEventBase>();

	[ExportGroup("Targeting", "")]
	[Export(PropertyHint.None, "")]
	public bool useZombieAttackCheck = true;

	[Export(PropertyHint.None, "")]
	public bool checkGrid = true;

	[Export(PropertyHint.None, "")]
	public bool useCheckAreaGridColumn;

	[Export(PropertyHint.None, "")]
	public bool checkEachShape;

	[Export(PropertyHint.None, "")]
	public bool checkLine;

	[Export(PropertyHint.None, "")]
	public bool checkTall;

	[Export(PropertyHint.None, "")]
	public bool checkVase;

	[Export(PropertyHint.None, "")]
	public bool checkBowling;

	[Export(PropertyHint.None, "")]
	public bool checkGravestone = true;

	[Export(PropertyHint.None, "")]
	public bool checkAll;

	[Export(PropertyHint.None, "")]
	public bool fliterLadder;

	[ExportGroup("Audio", "")]
	[Export(PropertyHint.None, "")]
	public string eatAudio = "Chomp";

	[Export(PropertyHint.None, "")]
	public string plantDefeatAudio = "Gulp";

	[Export(PropertyHint.Range, "0,10,0.05")]
	public double eatAudioInterval = 1.0;

	[Export(PropertyHint.None, "")]
	public bool playPlantDefeatAudio = true;

	[ExportGroup("State Events", "")]
	[Export(PropertyHint.None, "")]
	public StringName attackStateEvent = "ToAttack";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent = "ToIdle";

	[Export(PropertyHint.None, "")]
	public Array<AabbShape2DResource> checkShapeResources { get; set; } = new Array<AabbShape2DResource>();

	public int CollisionPreviewShapeCount => checkShapeResources?.Count ?? 0;

	public int CollisionPreviewRayCount => 0;

	public AabbShape2DResource GetCollisionPreviewShape(int index)
	{
		if (index < 0 || index >= (checkShapeResources?.Count ?? 0))
		{
			return null;
		}
		return checkShapeResources[index];
	}

	public AabbRay2DResource GetCollisionPreviewRay(int index)
	{
		return null;
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new AttackComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.GetCollisionPreviewShape, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCollisionPreviewRay, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetCollisionPreviewShape && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AabbShape2DResource>(GetCollisionPreviewShape(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCollisionPreviewRay && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AabbRay2DResource>(GetCollisionPreviewRay(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetCollisionPreviewShape)
		{
			return true;
		}
		if (method == MethodName.GetCollisionPreviewRay)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.checkShapeResources)
		{
			checkShapeResources = VariantUtils.ConvertToArray<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.attackType)
		{
			attackType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.useParentHitBox)
		{
			useParentHitBox = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkIntreval)
		{
			checkIntreval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.attackAnimeClipsArray)
		{
			attackAnimeClipsArray = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.attackAnimeClips)
		{
			attackAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.attackEventName)
		{
			attackEventName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.attackAnimeTimeScale)
		{
			attackAnimeTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.attackIntervalBase)
		{
			attackIntervalBase = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.attackInterval)
		{
			attackInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.attackIntervalOffset)
		{
			attackIntervalOffset = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spliceIdleAnimeClips)
		{
			spliceIdleAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.spliceIdleAnimeTimeScale)
		{
			spliceIdleAnimeTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.useZombieAttackCheck)
		{
			useZombieAttackCheck = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkGrid)
		{
			checkGrid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useCheckAreaGridColumn)
		{
			useCheckAreaGridColumn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkEachShape)
		{
			checkEachShape = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkLine)
		{
			checkLine = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkTall)
		{
			checkTall = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkVase)
		{
			checkVase = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkBowling)
		{
			checkBowling = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkGravestone)
		{
			checkGravestone = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkAll)
		{
			checkAll = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fliterLadder)
		{
			fliterLadder = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.eatAudio)
		{
			eatAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.plantDefeatAudio)
		{
			plantDefeatAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.eatAudioInterval)
		{
			eatAudioInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.playPlantDefeatAudio)
		{
			playPlantDefeatAudio = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.attackStateEvent)
		{
			attackStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.checkShapeResources)
		{
			value = VariantUtils.CreateFromArray(checkShapeResources);
			return true;
		}
		int from;
		if (name == PropertyName.CollisionPreviewShapeCount)
		{
			from = CollisionPreviewShapeCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CollisionPreviewRayCount)
		{
			from = CollisionPreviewRayCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.attackType)
		{
			value = VariantUtils.CreateFrom(in attackType);
			return true;
		}
		if (name == PropertyName.useParentHitBox)
		{
			value = VariantUtils.CreateFrom(in useParentHitBox);
			return true;
		}
		if (name == PropertyName.checkIntreval)
		{
			value = VariantUtils.CreateFrom(in checkIntreval);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			value = VariantUtils.CreateFrom(in spritePath);
			return true;
		}
		if (name == PropertyName.attackAnimeClipsArray)
		{
			value = VariantUtils.CreateFromArray(attackAnimeClipsArray);
			return true;
		}
		if (name == PropertyName.attackAnimeClips)
		{
			value = VariantUtils.CreateFrom(in attackAnimeClips);
			return true;
		}
		if (name == PropertyName.attackEventName)
		{
			value = VariantUtils.CreateFrom(in attackEventName);
			return true;
		}
		if (name == PropertyName.attackAnimeTimeScale)
		{
			value = VariantUtils.CreateFrom(in attackAnimeTimeScale);
			return true;
		}
		if (name == PropertyName.attackIntervalBase)
		{
			value = VariantUtils.CreateFrom(in attackIntervalBase);
			return true;
		}
		if (name == PropertyName.attackInterval)
		{
			value = VariantUtils.CreateFrom(in attackInterval);
			return true;
		}
		if (name == PropertyName.attackIntervalOffset)
		{
			value = VariantUtils.CreateFrom(in attackIntervalOffset);
			return true;
		}
		if (name == PropertyName.spliceIdleAnimeClips)
		{
			value = VariantUtils.CreateFrom(in spliceIdleAnimeClips);
			return true;
		}
		if (name == PropertyName.spliceIdleAnimeTimeScale)
		{
			value = VariantUtils.CreateFrom(in spliceIdleAnimeTimeScale);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		if (name == PropertyName.useZombieAttackCheck)
		{
			value = VariantUtils.CreateFrom(in useZombieAttackCheck);
			return true;
		}
		if (name == PropertyName.checkGrid)
		{
			value = VariantUtils.CreateFrom(in checkGrid);
			return true;
		}
		if (name == PropertyName.useCheckAreaGridColumn)
		{
			value = VariantUtils.CreateFrom(in useCheckAreaGridColumn);
			return true;
		}
		if (name == PropertyName.checkEachShape)
		{
			value = VariantUtils.CreateFrom(in checkEachShape);
			return true;
		}
		if (name == PropertyName.checkLine)
		{
			value = VariantUtils.CreateFrom(in checkLine);
			return true;
		}
		if (name == PropertyName.checkTall)
		{
			value = VariantUtils.CreateFrom(in checkTall);
			return true;
		}
		if (name == PropertyName.checkVase)
		{
			value = VariantUtils.CreateFrom(in checkVase);
			return true;
		}
		if (name == PropertyName.checkBowling)
		{
			value = VariantUtils.CreateFrom(in checkBowling);
			return true;
		}
		if (name == PropertyName.checkGravestone)
		{
			value = VariantUtils.CreateFrom(in checkGravestone);
			return true;
		}
		if (name == PropertyName.checkAll)
		{
			value = VariantUtils.CreateFrom(in checkAll);
			return true;
		}
		if (name == PropertyName.fliterLadder)
		{
			value = VariantUtils.CreateFrom(in fliterLadder);
			return true;
		}
		if (name == PropertyName.eatAudio)
		{
			value = VariantUtils.CreateFrom(in eatAudio);
			return true;
		}
		if (name == PropertyName.plantDefeatAudio)
		{
			value = VariantUtils.CreateFrom(in plantDefeatAudio);
			return true;
		}
		if (name == PropertyName.eatAudioInterval)
		{
			value = VariantUtils.CreateFrom(in eatAudioInterval);
			return true;
		}
		if (name == PropertyName.playPlantDefeatAudio)
		{
			value = VariantUtils.CreateFrom(in playPlantDefeatAudio);
			return true;
		}
		if (name == PropertyName.attackStateEvent)
		{
			value = VariantUtils.CreateFrom(in attackStateEvent);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			value = VariantUtils.CreateFrom(in idleStateEvent);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.attackType, PropertyHint.Enum, "Default,Eat,Smash,Chomp", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Collision", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useParentHitBox, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.checkShapeResources, PropertyHint.TypeString, "24/17:AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.checkIntreval, PropertyHint.Range, "0,120,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.attackAnimeClipsArray, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.attackAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.attackEventName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackIntervalBase, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackIntervalOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.spliceIdleAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.spliceIdleAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Attack Events", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Targeting", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useZombieAttackCheck, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkGrid, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useCheckAreaGridColumn, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkEachShape, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkLine, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkTall, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkVase, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkBowling, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkGravestone, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkAll, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.fliterLadder, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Audio", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.eatAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.plantDefeatAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.eatAudioInterval, PropertyHint.Range, "0,10,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.playPlantDefeatAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "State Events", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.attackStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.checkShapeResources, Variant.CreateFrom(checkShapeResources));
		info.AddProperty(PropertyName.attackType, Variant.From(in attackType));
		info.AddProperty(PropertyName.useParentHitBox, Variant.From(in useParentHitBox));
		info.AddProperty(PropertyName.checkIntreval, Variant.From(in checkIntreval));
		info.AddProperty(PropertyName.spritePath, Variant.From(in spritePath));
		info.AddProperty(PropertyName.attackAnimeClipsArray, Variant.CreateFrom(attackAnimeClipsArray));
		info.AddProperty(PropertyName.attackAnimeClips, Variant.From(in attackAnimeClips));
		info.AddProperty(PropertyName.attackEventName, Variant.From(in attackEventName));
		info.AddProperty(PropertyName.attackAnimeTimeScale, Variant.From(in attackAnimeTimeScale));
		info.AddProperty(PropertyName.attackIntervalBase, Variant.From(in attackIntervalBase));
		info.AddProperty(PropertyName.attackInterval, Variant.From(in attackInterval));
		info.AddProperty(PropertyName.attackIntervalOffset, Variant.From(in attackIntervalOffset));
		info.AddProperty(PropertyName.spliceIdleAnimeClips, Variant.From(in spliceIdleAnimeClips));
		info.AddProperty(PropertyName.spliceIdleAnimeTimeScale, Variant.From(in spliceIdleAnimeTimeScale));
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.useZombieAttackCheck, Variant.From(in useZombieAttackCheck));
		info.AddProperty(PropertyName.checkGrid, Variant.From(in checkGrid));
		info.AddProperty(PropertyName.useCheckAreaGridColumn, Variant.From(in useCheckAreaGridColumn));
		info.AddProperty(PropertyName.checkEachShape, Variant.From(in checkEachShape));
		info.AddProperty(PropertyName.checkLine, Variant.From(in checkLine));
		info.AddProperty(PropertyName.checkTall, Variant.From(in checkTall));
		info.AddProperty(PropertyName.checkVase, Variant.From(in checkVase));
		info.AddProperty(PropertyName.checkBowling, Variant.From(in checkBowling));
		info.AddProperty(PropertyName.checkGravestone, Variant.From(in checkGravestone));
		info.AddProperty(PropertyName.checkAll, Variant.From(in checkAll));
		info.AddProperty(PropertyName.fliterLadder, Variant.From(in fliterLadder));
		info.AddProperty(PropertyName.eatAudio, Variant.From(in eatAudio));
		info.AddProperty(PropertyName.plantDefeatAudio, Variant.From(in plantDefeatAudio));
		info.AddProperty(PropertyName.eatAudioInterval, Variant.From(in eatAudioInterval));
		info.AddProperty(PropertyName.playPlantDefeatAudio, Variant.From(in playPlantDefeatAudio));
		info.AddProperty(PropertyName.attackStateEvent, Variant.From(in attackStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From(in idleStateEvent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.checkShapeResources, out var value))
		{
			checkShapeResources = value.AsGodotArray<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.attackType, out var value2))
		{
			attackType = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.useParentHitBox, out var value3))
		{
			useParentHitBox = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkIntreval, out var value4))
		{
			checkIntreval = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.spritePath, out var value5))
		{
			spritePath = value5.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.attackAnimeClipsArray, out var value6))
		{
			attackAnimeClipsArray = value6.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.attackAnimeClips, out var value7))
		{
			attackAnimeClips = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.attackEventName, out var value8))
		{
			attackEventName = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.attackAnimeTimeScale, out var value9))
		{
			attackAnimeTimeScale = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.attackIntervalBase, out var value10))
		{
			attackIntervalBase = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.attackInterval, out var value11))
		{
			attackInterval = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.attackIntervalOffset, out var value12))
		{
			attackIntervalOffset = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spliceIdleAnimeClips, out var value13))
		{
			spliceIdleAnimeClips = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName.spliceIdleAnimeTimeScale, out var value14))
		{
			spliceIdleAnimeTimeScale = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.eventList, out var value15))
		{
			eventList = value15.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.useZombieAttackCheck, out var value16))
		{
			useZombieAttackCheck = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkGrid, out var value17))
		{
			checkGrid = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useCheckAreaGridColumn, out var value18))
		{
			useCheckAreaGridColumn = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkEachShape, out var value19))
		{
			checkEachShape = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkLine, out var value20))
		{
			checkLine = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkTall, out var value21))
		{
			checkTall = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkVase, out var value22))
		{
			checkVase = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkBowling, out var value23))
		{
			checkBowling = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkGravestone, out var value24))
		{
			checkGravestone = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkAll, out var value25))
		{
			checkAll = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fliterLadder, out var value26))
		{
			fliterLadder = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.eatAudio, out var value27))
		{
			eatAudio = value27.As<string>();
		}
		if (info.TryGetProperty(PropertyName.plantDefeatAudio, out var value28))
		{
			plantDefeatAudio = value28.As<string>();
		}
		if (info.TryGetProperty(PropertyName.eatAudioInterval, out var value29))
		{
			eatAudioInterval = value29.As<double>();
		}
		if (info.TryGetProperty(PropertyName.playPlantDefeatAudio, out var value30))
		{
			playPlantDefeatAudio = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.attackStateEvent, out var value31))
		{
			attackStateEvent = value31.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value32))
		{
			idleStateEvent = value32.As<StringName>();
		}
	}
}
