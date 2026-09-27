using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/FireComponent/FireComponentDefinition.cs")]
public class FireComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName firePosMarkerPaths = "firePosMarkerPaths";

		public static readonly StringName spritePath = "spritePath";

		public static readonly StringName spliceSpritePaths = "spliceSpritePaths";

		public static readonly StringName preExtendInstanceId = "preExtendInstanceId";

		public static readonly StringName checkRayResources = "checkRayResources";

		public static readonly StringName checkShapeResources = "checkShapeResources";

		public static readonly StringName checkUse = "checkUse";

		public static readonly StringName fireAudioName = "fireAudioName";

		public static readonly StringName fireEventName = "fireEventName";

		public static readonly StringName attackStateEvent = "attackStateEvent";

		public static readonly StringName restoreStateEvent = "restoreStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";

		public static readonly StringName fireOverEventName = "fireOverEventName";

		public static readonly StringName fireAnimeClipsArray = "fireAnimeClipsArray";

		public static readonly StringName fireAnimeClips = "fireAnimeClips";

		public static readonly StringName fireAnimeTimeScale = "fireAnimeTimeScale";

		public static readonly StringName restoreAnimeClips = "restoreAnimeClips";

		public static readonly StringName restoreTime = "restoreTime";

		public static readonly StringName spliceIdleAnimeClips = "spliceIdleAnimeClips";

		public static readonly StringName spliceIdleAnimeTimeScale = "spliceIdleAnimeTimeScale";

		public static readonly StringName isSpliceSprite = "isSpliceSprite";

		public static readonly StringName spliceSpriteFireAnimeClipsList = "spliceSpriteFireAnimeClipsList";

		public static readonly StringName spliceSpriteIdleAnimeClipsList = "spliceSpriteIdleAnimeClipsList";

		public static readonly StringName onlyEmitSignal = "onlyEmitSignal";

		public static readonly StringName fireLength = "fireLength";

		public static readonly StringName fireDirect = "fireDirect";

		public static readonly StringName fireIntervalBase = "fireIntervalBase";

		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName fireIntervalOffset = "fireIntervalOffset";

		public static readonly StringName fireNum = "fireNum";

		public static readonly StringName fireNumAtOnce = "fireNumAtOnce";

		public static readonly StringName fireCheckList = "fireCheckList";

		public static readonly StringName fireProjectileList = "fireProjectileList";

		public static readonly StringName useCollisionEveryPos = "useCollisionEveryPos";

		public static readonly StringName readyConfirmProjectile = "readyConfirmProjectile";

		public static readonly StringName lockProjectileGridY = "lockProjectileGridY";

		public static readonly StringName snapStraightProjectileToSameCellTarget = "snapStraightProjectileToSameCellTarget";

		public static readonly StringName offscreenTargetMarginColumns = "offscreenTargetMarginColumns";

		public static readonly StringName defaultCheckLengthWorld = "defaultCheckLengthWorld";

		public static readonly StringName fireAnimationStartPosition = "fireAnimationStartPosition";

		public static readonly StringName repeatFireAnimationStartPosition = "repeatFireAnimationStartPosition";

		public static readonly StringName spliceAnimationStartPosition = "spliceAnimationStartPosition";

		public static readonly StringName restoreAnimationStartPosition = "restoreAnimationStartPosition";

		public static readonly StringName offsetLineTweenDuration = "offsetLineTweenDuration";

		public static readonly StringName blockedOffsetLineX = "blockedOffsetLineX";

		public static readonly StringName checkAllLine = "checkAllLine";

		public static readonly StringName checkLength = "checkLength";

		public static readonly StringName checkIntervalMax = "checkIntervalMax";

		public static readonly StringName checkHeight = "checkHeight";

		public static readonly StringName airFirst = "airFirst";

		public static readonly StringName catapultFirstFar = "catapultFirstFar";

		public static readonly StringName randomChoose = "randomChoose";

		public static readonly StringName canTargetGargantuar = "canTargetGargantuar";

		public static readonly StringName checkGravestone = "checkGravestone";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Owner-relative References", "")]
	[Export(PropertyHint.None, "")]
	public Array<NodePath> firePosMarkerPaths { get; set; } = new Array<NodePath>();

	[Export(PropertyHint.None, "")]
	public NodePath spritePath { get; set; } = new NodePath();

	[Export(PropertyHint.None, "")]
	public Array<NodePath> spliceSpritePaths { get; set; } = new Array<NodePath>();

	[Export(PropertyHint.None, "")]
	public string preExtendInstanceId { get; set; } = string.Empty;

	[ExportGroup("Target Geometry", "")]
	[Export(PropertyHint.None, "")]
	public Array<AabbRay2DResource> checkRayResources { get; set; } = new Array<AabbRay2DResource>();

	[Export(PropertyHint.None, "")]
	public Array<AabbShape2DResource> checkShapeResources { get; set; } = new Array<AabbShape2DResource>();

	[Export(PropertyHint.None, "")]
	public bool checkUse { get; set; } = true;

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public string fireAudioName { get; set; } = "ProjectileThrow";

	[Export(PropertyHint.None, "")]
	public string fireEventName { get; set; } = "fire";

	[Export(PropertyHint.None, "")]
	public StringName attackStateEvent { get; set; } = "ToAttack";

	[Export(PropertyHint.None, "")]
	public StringName restoreStateEvent { get; set; } = "ToRestore";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent { get; set; } = "ToIdle";

	[Export(PropertyHint.None, "")]
	public string fireOverEventName { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public Array<string> fireAnimeClipsArray { get; set; } = new Array<string> { "Fire" };

	[Export(PropertyHint.None, "")]
	public string fireAnimeClips { get; set; } = "Fire";

	[Export(PropertyHint.None, "")]
	public float fireAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public string restoreAnimeClips { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public float restoreTime { get; set; } = 10f;

	[Export(PropertyHint.None, "")]
	public string spliceIdleAnimeClips { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public float spliceIdleAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public bool isSpliceSprite { get; set; }

	[Export(PropertyHint.None, "")]
	public Array<string> spliceSpriteFireAnimeClipsList { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<string> spliceSpriteIdleAnimeClipsList { get; set; } = new Array<string>();

	[ExportGroup("Fire", "")]
	[Export(PropertyHint.None, "")]
	public bool onlyEmitSignal { get; set; }

	[Export(PropertyHint.None, "")]
	public float fireLength { get; set; } = -1f;

	[Export(PropertyHint.None, "")]
	public bool fireDirect { get; set; }

	[Export(PropertyHint.None, "")]
	public float fireIntervalBase { get; set; } = 1.5f;

	[Export(PropertyHint.None, "")]
	public float fireInterval { get; set; } = 1.5f;

	[Export(PropertyHint.None, "")]
	public float fireIntervalOffset { get; set; } = 0.1f;

	[Export(PropertyHint.None, "")]
	public int fireNum { get; set; } = 1;

	[Export(PropertyHint.None, "")]
	public bool fireNumAtOnce { get; set; }

	[Export(PropertyHint.None, "")]
	public Array<FireComponentCheckConfig> fireCheckList { get; set; } = new Array<FireComponentCheckConfig>();

	[Export(PropertyHint.None, "")]
	public Array<FireComponentFireProjectileConfig> fireProjectileList { get; set; } = new Array<FireComponentFireProjectileConfig>();

	[Export(PropertyHint.None, "")]
	public bool useCollisionEveryPos { get; set; }

	[Export(PropertyHint.None, "")]
	public bool readyConfirmProjectile { get; set; }

	[Export(PropertyHint.None, "")]
	public bool lockProjectileGridY { get; set; }

	[Export(PropertyHint.None, "")]
	public bool snapStraightProjectileToSameCellTarget { get; set; }

	[Export(PropertyHint.None, "")]
	public float offscreenTargetMarginColumns { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public float defaultCheckLengthWorld { get; set; } = 2000f;

	[Export(PropertyHint.None, "")]
	public float fireAnimationStartPosition { get; set; } = 0.2f;

	[Export(PropertyHint.None, "")]
	public float repeatFireAnimationStartPosition { get; set; } = 0.1f;

	[Export(PropertyHint.None, "")]
	public float spliceAnimationStartPosition { get; set; } = 0.2f;

	[Export(PropertyHint.None, "")]
	public float restoreAnimationStartPosition { get; set; } = 0.2f;

	[Export(PropertyHint.None, "")]
	public float offsetLineTweenDuration { get; set; } = 0.15f;

	[Export(PropertyHint.None, "")]
	public float blockedOffsetLineX { get; set; } = 25f;

	[ExportGroup("Targeting", "")]
	[Export(PropertyHint.None, "")]
	public bool checkAllLine { get; set; }

	[Export(PropertyHint.None, "")]
	public float checkLength { get; set; } = -1f;

	[Export(PropertyHint.None, "")]
	public int checkIntervalMax { get; set; } = 2;

	[Export(PropertyHint.None, "")]
	public bool checkHeight { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool airFirst { get; set; }

	[Export(PropertyHint.None, "")]
	public bool catapultFirstFar { get; set; }

	[Export(PropertyHint.None, "")]
	public bool randomChoose { get; set; }

	[Export(PropertyHint.None, "")]
	public bool canTargetGargantuar { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool checkGravestone { get; set; } = true;

	public int CollisionPreviewShapeCount => checkShapeResources?.Count ?? 0;

	public int CollisionPreviewRayCount => checkRayResources?.Count ?? 0;

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
		if (index < 0 || index >= (checkRayResources?.Count ?? 0))
		{
			return null;
		}
		return checkRayResources[index];
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new FireComponent();
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
		if (name == PropertyName.firePosMarkerPaths)
		{
			firePosMarkerPaths = VariantUtils.ConvertToArray<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.spliceSpritePaths)
		{
			spliceSpritePaths = VariantUtils.ConvertToArray<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.preExtendInstanceId)
		{
			preExtendInstanceId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.checkRayResources)
		{
			checkRayResources = VariantUtils.ConvertToArray<AabbRay2DResource>(in value);
			return true;
		}
		if (name == PropertyName.checkShapeResources)
		{
			checkShapeResources = VariantUtils.ConvertToArray<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.checkUse)
		{
			checkUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fireAudioName)
		{
			fireAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.fireEventName)
		{
			fireEventName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.attackStateEvent)
		{
			attackStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.restoreStateEvent)
		{
			restoreStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.fireOverEventName)
		{
			fireOverEventName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimeClipsArray)
		{
			fireAnimeClipsArray = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimeClips)
		{
			fireAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimeTimeScale)
		{
			fireAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.restoreAnimeClips)
		{
			restoreAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.restoreTime)
		{
			restoreTime = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.spliceIdleAnimeClips)
		{
			spliceIdleAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.spliceIdleAnimeTimeScale)
		{
			spliceIdleAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.isSpliceSprite)
		{
			isSpliceSprite = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.spliceSpriteFireAnimeClipsList)
		{
			spliceSpriteFireAnimeClipsList = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.spliceSpriteIdleAnimeClipsList)
		{
			spliceSpriteIdleAnimeClipsList = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.onlyEmitSignal)
		{
			onlyEmitSignal = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fireLength)
		{
			fireLength = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.fireDirect)
		{
			fireDirect = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fireIntervalBase)
		{
			fireIntervalBase = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.fireInterval)
		{
			fireInterval = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.fireIntervalOffset)
		{
			fireIntervalOffset = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.fireNum)
		{
			fireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.fireNumAtOnce)
		{
			fireNumAtOnce = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fireCheckList)
		{
			fireCheckList = VariantUtils.ConvertToArray<FireComponentCheckConfig>(in value);
			return true;
		}
		if (name == PropertyName.fireProjectileList)
		{
			fireProjectileList = VariantUtils.ConvertToArray<FireComponentFireProjectileConfig>(in value);
			return true;
		}
		if (name == PropertyName.useCollisionEveryPos)
		{
			useCollisionEveryPos = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.readyConfirmProjectile)
		{
			readyConfirmProjectile = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.lockProjectileGridY)
		{
			lockProjectileGridY = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.snapStraightProjectileToSameCellTarget)
		{
			snapStraightProjectileToSameCellTarget = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.offscreenTargetMarginColumns)
		{
			offscreenTargetMarginColumns = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.defaultCheckLengthWorld)
		{
			defaultCheckLengthWorld = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimationStartPosition)
		{
			fireAnimationStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.repeatFireAnimationStartPosition)
		{
			repeatFireAnimationStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.spliceAnimationStartPosition)
		{
			spliceAnimationStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.restoreAnimationStartPosition)
		{
			restoreAnimationStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.offsetLineTweenDuration)
		{
			offsetLineTweenDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.blockedOffsetLineX)
		{
			blockedOffsetLineX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.checkAllLine)
		{
			checkAllLine = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkLength)
		{
			checkLength = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.checkIntervalMax)
		{
			checkIntervalMax = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.checkHeight)
		{
			checkHeight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.airFirst)
		{
			airFirst = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.catapultFirstFar)
		{
			catapultFirstFar = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.randomChoose)
		{
			randomChoose = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.canTargetGargantuar)
		{
			canTargetGargantuar = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkGravestone)
		{
			checkGravestone = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.firePosMarkerPaths)
		{
			value = VariantUtils.CreateFromArray(firePosMarkerPaths);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			value = VariantUtils.CreateFrom<NodePath>(spritePath);
			return true;
		}
		if (name == PropertyName.spliceSpritePaths)
		{
			value = VariantUtils.CreateFromArray(spliceSpritePaths);
			return true;
		}
		string from;
		if (name == PropertyName.preExtendInstanceId)
		{
			from = preExtendInstanceId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.checkRayResources)
		{
			value = VariantUtils.CreateFromArray(checkRayResources);
			return true;
		}
		if (name == PropertyName.checkShapeResources)
		{
			value = VariantUtils.CreateFromArray(checkShapeResources);
			return true;
		}
		bool from2;
		if (name == PropertyName.checkUse)
		{
			from2 = checkUse;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.fireAudioName)
		{
			from = fireAudioName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.fireEventName)
		{
			from = fireEventName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		StringName from3;
		if (name == PropertyName.attackStateEvent)
		{
			from3 = attackStateEvent;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.restoreStateEvent)
		{
			from3 = restoreStateEvent;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			from3 = idleStateEvent;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.fireOverEventName)
		{
			from = fireOverEventName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.fireAnimeClipsArray)
		{
			value = VariantUtils.CreateFromArray(fireAnimeClipsArray);
			return true;
		}
		if (name == PropertyName.fireAnimeClips)
		{
			from = fireAnimeClips;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		float from4;
		if (name == PropertyName.fireAnimeTimeScale)
		{
			from4 = fireAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.restoreAnimeClips)
		{
			from = restoreAnimeClips;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.restoreTime)
		{
			from4 = restoreTime;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.spliceIdleAnimeClips)
		{
			from = spliceIdleAnimeClips;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.spliceIdleAnimeTimeScale)
		{
			from4 = spliceIdleAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.isSpliceSprite)
		{
			from2 = isSpliceSprite;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.spliceSpriteFireAnimeClipsList)
		{
			value = VariantUtils.CreateFromArray(spliceSpriteFireAnimeClipsList);
			return true;
		}
		if (name == PropertyName.spliceSpriteIdleAnimeClipsList)
		{
			value = VariantUtils.CreateFromArray(spliceSpriteIdleAnimeClipsList);
			return true;
		}
		if (name == PropertyName.onlyEmitSignal)
		{
			from2 = onlyEmitSignal;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.fireLength)
		{
			from4 = fireLength;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.fireDirect)
		{
			from2 = fireDirect;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.fireIntervalBase)
		{
			from4 = fireIntervalBase;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.fireInterval)
		{
			from4 = fireInterval;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.fireIntervalOffset)
		{
			from4 = fireIntervalOffset;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		int from5;
		if (name == PropertyName.fireNum)
		{
			from5 = fireNum;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.fireNumAtOnce)
		{
			from2 = fireNumAtOnce;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.fireCheckList)
		{
			value = VariantUtils.CreateFromArray(fireCheckList);
			return true;
		}
		if (name == PropertyName.fireProjectileList)
		{
			value = VariantUtils.CreateFromArray(fireProjectileList);
			return true;
		}
		if (name == PropertyName.useCollisionEveryPos)
		{
			from2 = useCollisionEveryPos;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.readyConfirmProjectile)
		{
			from2 = readyConfirmProjectile;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.lockProjectileGridY)
		{
			from2 = lockProjectileGridY;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.snapStraightProjectileToSameCellTarget)
		{
			from2 = snapStraightProjectileToSameCellTarget;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.offscreenTargetMarginColumns)
		{
			from4 = offscreenTargetMarginColumns;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.defaultCheckLengthWorld)
		{
			from4 = defaultCheckLengthWorld;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.fireAnimationStartPosition)
		{
			from4 = fireAnimationStartPosition;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.repeatFireAnimationStartPosition)
		{
			from4 = repeatFireAnimationStartPosition;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.spliceAnimationStartPosition)
		{
			from4 = spliceAnimationStartPosition;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.restoreAnimationStartPosition)
		{
			from4 = restoreAnimationStartPosition;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.offsetLineTweenDuration)
		{
			from4 = offsetLineTweenDuration;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.blockedOffsetLineX)
		{
			from4 = blockedOffsetLineX;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.checkAllLine)
		{
			from2 = checkAllLine;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.checkLength)
		{
			from4 = checkLength;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.checkIntervalMax)
		{
			from5 = checkIntervalMax;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.checkHeight)
		{
			from2 = checkHeight;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.airFirst)
		{
			from2 = airFirst;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.catapultFirstFar)
		{
			from2 = catapultFirstFar;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.randomChoose)
		{
			from2 = randomChoose;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.canTargetGargantuar)
		{
			from2 = canTargetGargantuar;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.checkGravestone)
		{
			from2 = checkGravestone;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CollisionPreviewShapeCount)
		{
			from5 = CollisionPreviewShapeCount;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.CollisionPreviewRayCount)
		{
			from5 = CollisionPreviewRayCount;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Owner-relative References", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.firePosMarkerPaths, PropertyHint.TypeString, "22/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.spliceSpritePaths, PropertyHint.TypeString, "22/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.preExtendInstanceId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Target Geometry", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.checkRayResources, PropertyHint.TypeString, "24/17:AabbRay2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.checkShapeResources, PropertyHint.TypeString, "24/17:AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireEventName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.attackStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.restoreStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireOverEventName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.fireAnimeClipsArray, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.restoreAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.restoreTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.spliceIdleAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.spliceIdleAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isSpliceSprite, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.spliceSpriteFireAnimeClipsList, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.spliceSpriteIdleAnimeClipsList, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Fire", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.onlyEmitSignal, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireLength, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.fireDirect, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireIntervalBase, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireIntervalOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.fireNumAtOnce, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.fireCheckList, PropertyHint.TypeString, "24/17:FireComponentCheckConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.fireProjectileList, PropertyHint.TypeString, "24/17:FireComponentFireProjectileConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useCollisionEveryPos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.readyConfirmProjectile, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.lockProjectileGridY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.snapStraightProjectileToSameCellTarget, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.offscreenTargetMarginColumns, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.defaultCheckLengthWorld, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireAnimationStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.repeatFireAnimationStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.spliceAnimationStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.restoreAnimationStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.offsetLineTweenDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.blockedOffsetLineX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Targeting", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkAllLine, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.checkLength, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.checkIntervalMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.airFirst, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.catapultFirstFar, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.randomChoose, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canTargetGargantuar, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkGravestone, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.firePosMarkerPaths, Variant.CreateFrom(firePosMarkerPaths));
		info.AddProperty(PropertyName.spritePath, Variant.From<NodePath>(spritePath));
		info.AddProperty(PropertyName.spliceSpritePaths, Variant.CreateFrom(spliceSpritePaths));
		info.AddProperty(PropertyName.preExtendInstanceId, Variant.From<string>(preExtendInstanceId));
		info.AddProperty(PropertyName.checkRayResources, Variant.CreateFrom(checkRayResources));
		info.AddProperty(PropertyName.checkShapeResources, Variant.CreateFrom(checkShapeResources));
		info.AddProperty(PropertyName.checkUse, Variant.From<bool>(checkUse));
		info.AddProperty(PropertyName.fireAudioName, Variant.From<string>(fireAudioName));
		info.AddProperty(PropertyName.fireEventName, Variant.From<string>(fireEventName));
		info.AddProperty(PropertyName.attackStateEvent, Variant.From<StringName>(attackStateEvent));
		info.AddProperty(PropertyName.restoreStateEvent, Variant.From<StringName>(restoreStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From<StringName>(idleStateEvent));
		info.AddProperty(PropertyName.fireOverEventName, Variant.From<string>(fireOverEventName));
		info.AddProperty(PropertyName.fireAnimeClipsArray, Variant.CreateFrom(fireAnimeClipsArray));
		info.AddProperty(PropertyName.fireAnimeClips, Variant.From<string>(fireAnimeClips));
		info.AddProperty(PropertyName.fireAnimeTimeScale, Variant.From<float>(fireAnimeTimeScale));
		info.AddProperty(PropertyName.restoreAnimeClips, Variant.From<string>(restoreAnimeClips));
		info.AddProperty(PropertyName.restoreTime, Variant.From<float>(restoreTime));
		info.AddProperty(PropertyName.spliceIdleAnimeClips, Variant.From<string>(spliceIdleAnimeClips));
		info.AddProperty(PropertyName.spliceIdleAnimeTimeScale, Variant.From<float>(spliceIdleAnimeTimeScale));
		info.AddProperty(PropertyName.isSpliceSprite, Variant.From<bool>(isSpliceSprite));
		info.AddProperty(PropertyName.spliceSpriteFireAnimeClipsList, Variant.CreateFrom(spliceSpriteFireAnimeClipsList));
		info.AddProperty(PropertyName.spliceSpriteIdleAnimeClipsList, Variant.CreateFrom(spliceSpriteIdleAnimeClipsList));
		info.AddProperty(PropertyName.onlyEmitSignal, Variant.From<bool>(onlyEmitSignal));
		info.AddProperty(PropertyName.fireLength, Variant.From<float>(fireLength));
		info.AddProperty(PropertyName.fireDirect, Variant.From<bool>(fireDirect));
		info.AddProperty(PropertyName.fireIntervalBase, Variant.From<float>(fireIntervalBase));
		info.AddProperty(PropertyName.fireInterval, Variant.From<float>(fireInterval));
		info.AddProperty(PropertyName.fireIntervalOffset, Variant.From<float>(fireIntervalOffset));
		info.AddProperty(PropertyName.fireNum, Variant.From<int>(fireNum));
		info.AddProperty(PropertyName.fireNumAtOnce, Variant.From<bool>(fireNumAtOnce));
		info.AddProperty(PropertyName.fireCheckList, Variant.CreateFrom(fireCheckList));
		info.AddProperty(PropertyName.fireProjectileList, Variant.CreateFrom(fireProjectileList));
		info.AddProperty(PropertyName.useCollisionEveryPos, Variant.From<bool>(useCollisionEveryPos));
		info.AddProperty(PropertyName.readyConfirmProjectile, Variant.From<bool>(readyConfirmProjectile));
		info.AddProperty(PropertyName.lockProjectileGridY, Variant.From<bool>(lockProjectileGridY));
		info.AddProperty(PropertyName.snapStraightProjectileToSameCellTarget, Variant.From<bool>(snapStraightProjectileToSameCellTarget));
		info.AddProperty(PropertyName.offscreenTargetMarginColumns, Variant.From<float>(offscreenTargetMarginColumns));
		info.AddProperty(PropertyName.defaultCheckLengthWorld, Variant.From<float>(defaultCheckLengthWorld));
		info.AddProperty(PropertyName.fireAnimationStartPosition, Variant.From<float>(fireAnimationStartPosition));
		info.AddProperty(PropertyName.repeatFireAnimationStartPosition, Variant.From<float>(repeatFireAnimationStartPosition));
		info.AddProperty(PropertyName.spliceAnimationStartPosition, Variant.From<float>(spliceAnimationStartPosition));
		info.AddProperty(PropertyName.restoreAnimationStartPosition, Variant.From<float>(restoreAnimationStartPosition));
		info.AddProperty(PropertyName.offsetLineTweenDuration, Variant.From<float>(offsetLineTweenDuration));
		info.AddProperty(PropertyName.blockedOffsetLineX, Variant.From<float>(blockedOffsetLineX));
		info.AddProperty(PropertyName.checkAllLine, Variant.From<bool>(checkAllLine));
		info.AddProperty(PropertyName.checkLength, Variant.From<float>(checkLength));
		info.AddProperty(PropertyName.checkIntervalMax, Variant.From<int>(checkIntervalMax));
		info.AddProperty(PropertyName.checkHeight, Variant.From<bool>(checkHeight));
		info.AddProperty(PropertyName.airFirst, Variant.From<bool>(airFirst));
		info.AddProperty(PropertyName.catapultFirstFar, Variant.From<bool>(catapultFirstFar));
		info.AddProperty(PropertyName.randomChoose, Variant.From<bool>(randomChoose));
		info.AddProperty(PropertyName.canTargetGargantuar, Variant.From<bool>(canTargetGargantuar));
		info.AddProperty(PropertyName.checkGravestone, Variant.From<bool>(checkGravestone));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.firePosMarkerPaths, out var value))
		{
			firePosMarkerPaths = value.AsGodotArray<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.spritePath, out var value2))
		{
			spritePath = value2.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.spliceSpritePaths, out var value3))
		{
			spliceSpritePaths = value3.AsGodotArray<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.preExtendInstanceId, out var value4))
		{
			preExtendInstanceId = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.checkRayResources, out var value5))
		{
			checkRayResources = value5.AsGodotArray<AabbRay2DResource>();
		}
		if (info.TryGetProperty(PropertyName.checkShapeResources, out var value6))
		{
			checkShapeResources = value6.AsGodotArray<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.checkUse, out var value7))
		{
			checkUse = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fireAudioName, out var value8))
		{
			fireAudioName = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.fireEventName, out var value9))
		{
			fireEventName = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.attackStateEvent, out var value10))
		{
			attackStateEvent = value10.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.restoreStateEvent, out var value11))
		{
			restoreStateEvent = value11.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value12))
		{
			idleStateEvent = value12.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.fireOverEventName, out var value13))
		{
			fireOverEventName = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimeClipsArray, out var value14))
		{
			fireAnimeClipsArray = value14.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimeClips, out var value15))
		{
			fireAnimeClips = value15.As<string>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimeTimeScale, out var value16))
		{
			fireAnimeTimeScale = value16.As<float>();
		}
		if (info.TryGetProperty(PropertyName.restoreAnimeClips, out var value17))
		{
			restoreAnimeClips = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName.restoreTime, out var value18))
		{
			restoreTime = value18.As<float>();
		}
		if (info.TryGetProperty(PropertyName.spliceIdleAnimeClips, out var value19))
		{
			spliceIdleAnimeClips = value19.As<string>();
		}
		if (info.TryGetProperty(PropertyName.spliceIdleAnimeTimeScale, out var value20))
		{
			spliceIdleAnimeTimeScale = value20.As<float>();
		}
		if (info.TryGetProperty(PropertyName.isSpliceSprite, out var value21))
		{
			isSpliceSprite = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.spliceSpriteFireAnimeClipsList, out var value22))
		{
			spliceSpriteFireAnimeClipsList = value22.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.spliceSpriteIdleAnimeClipsList, out var value23))
		{
			spliceSpriteIdleAnimeClipsList = value23.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.onlyEmitSignal, out var value24))
		{
			onlyEmitSignal = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fireLength, out var value25))
		{
			fireLength = value25.As<float>();
		}
		if (info.TryGetProperty(PropertyName.fireDirect, out var value26))
		{
			fireDirect = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fireIntervalBase, out var value27))
		{
			fireIntervalBase = value27.As<float>();
		}
		if (info.TryGetProperty(PropertyName.fireInterval, out var value28))
		{
			fireInterval = value28.As<float>();
		}
		if (info.TryGetProperty(PropertyName.fireIntervalOffset, out var value29))
		{
			fireIntervalOffset = value29.As<float>();
		}
		if (info.TryGetProperty(PropertyName.fireNum, out var value30))
		{
			fireNum = value30.As<int>();
		}
		if (info.TryGetProperty(PropertyName.fireNumAtOnce, out var value31))
		{
			fireNumAtOnce = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fireCheckList, out var value32))
		{
			fireCheckList = value32.AsGodotArray<FireComponentCheckConfig>();
		}
		if (info.TryGetProperty(PropertyName.fireProjectileList, out var value33))
		{
			fireProjectileList = value33.AsGodotArray<FireComponentFireProjectileConfig>();
		}
		if (info.TryGetProperty(PropertyName.useCollisionEveryPos, out var value34))
		{
			useCollisionEveryPos = value34.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.readyConfirmProjectile, out var value35))
		{
			readyConfirmProjectile = value35.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.lockProjectileGridY, out var value36))
		{
			lockProjectileGridY = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.snapStraightProjectileToSameCellTarget, out var value37))
		{
			snapStraightProjectileToSameCellTarget = value37.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.offscreenTargetMarginColumns, out var value38))
		{
			offscreenTargetMarginColumns = value38.As<float>();
		}
		if (info.TryGetProperty(PropertyName.defaultCheckLengthWorld, out var value39))
		{
			defaultCheckLengthWorld = value39.As<float>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimationStartPosition, out var value40))
		{
			fireAnimationStartPosition = value40.As<float>();
		}
		if (info.TryGetProperty(PropertyName.repeatFireAnimationStartPosition, out var value41))
		{
			repeatFireAnimationStartPosition = value41.As<float>();
		}
		if (info.TryGetProperty(PropertyName.spliceAnimationStartPosition, out var value42))
		{
			spliceAnimationStartPosition = value42.As<float>();
		}
		if (info.TryGetProperty(PropertyName.restoreAnimationStartPosition, out var value43))
		{
			restoreAnimationStartPosition = value43.As<float>();
		}
		if (info.TryGetProperty(PropertyName.offsetLineTweenDuration, out var value44))
		{
			offsetLineTweenDuration = value44.As<float>();
		}
		if (info.TryGetProperty(PropertyName.blockedOffsetLineX, out var value45))
		{
			blockedOffsetLineX = value45.As<float>();
		}
		if (info.TryGetProperty(PropertyName.checkAllLine, out var value46))
		{
			checkAllLine = value46.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkLength, out var value47))
		{
			checkLength = value47.As<float>();
		}
		if (info.TryGetProperty(PropertyName.checkIntervalMax, out var value48))
		{
			checkIntervalMax = value48.As<int>();
		}
		if (info.TryGetProperty(PropertyName.checkHeight, out var value49))
		{
			checkHeight = value49.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.airFirst, out var value50))
		{
			airFirst = value50.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.catapultFirstFar, out var value51))
		{
			catapultFirstFar = value51.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.randomChoose, out var value52))
		{
			randomChoose = value52.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.canTargetGargantuar, out var value53))
		{
			canTargetGargantuar = value53.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkGravestone, out var value54))
		{
			checkGravestone = value54.As<bool>();
		}
	}
}
