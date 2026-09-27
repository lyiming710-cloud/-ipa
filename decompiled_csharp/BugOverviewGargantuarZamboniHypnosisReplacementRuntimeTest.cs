using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewGargantuarZamboniHypnosisReplacementRuntimeTest.cs")]
public class BugOverviewGargantuarZamboniHypnosisReplacementRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindReplacement = "FindReplacement";

		public static readonly StringName HasBuff = "HasBuff";

		public static readonly StringName RegisterReplacementFixtures = "RegisterReplacementFixtures";

		public static readonly StringName RegisterPacket = "RegisterPacket";

		public static readonly StringName RegisterCharacter = "RegisterCharacter";

		public static readonly StringName RestoreReplacementFixtures = "RestoreReplacementFixtures";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string BaseScenePath = "res://Asset/Anime/Character/Zombie/Challenge/GargantuarZamboni/Scene/Base/TowerDefenseZombieGargantuarZamboni.tscn";

	private const string RedEyesScenePath = "res://Asset/Anime/Character/Zombie/Challenge/GargantuarZamboni/Scene/RedEyes/TowerDefenseZombieGargantuarZamboniRedEyes.tscn";

	private const string GargantuarScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Scene/Base/TowerDefenseZombieGargantuar.tscn";

	private const string RedEyesGargantuarScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Scene/RedEyes/TowerDefenseZombieGargantuarRedEyes.tscn";

	private const string GargantuarPacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Packet/Base/ZombieGargantuar.tres";

	private const string RedEyesGargantuarPacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Packet/Redeyes/ZombieGargantuarRedeyes.tres";

	private readonly List<TowerDefenseCharacter> _ownedCharacters = new List<TowerDefenseCharacter>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		GargantuarZamboniHypnosisReplacementControlStub control = null;
		FieldInfo resourceStateField = typeof(ResourceManager).GetField("_gameplayResourceLoadState", BindingFlags.Instance | BindingFlags.NonPublic);
		object previousResourceState = resourceStateField.GetValue(ResourceManager.Instance);
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ResourceManager.Instance), "TowerDefenseManager and ResourceManager autoloads must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new InvalidOperationException("Required runtime autoloads are unavailable.");
				}
				resourceStateField.SetValue(ResourceManager.Instance, GameplayResourceLoadState.Ready);
				RegisterReplacementFixtures();
				control = new GargantuarZamboniHypnosisReplacementControlStub
				{
					Name = "GargantuarZamboniHypnosisReplacementControl",
					isGameRunning = true,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = new Vector2(0f, 100f);
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				TowerDefenseZombie baseReplacement = await VerifyVariant("res://Asset/Anime/Character/Zombie/Challenge/GargantuarZamboni/Scene/Base/TowerDefenseZombieGargantuarZamboni.tscn", "ZombieGargantuar", "base");
				await VerifyVariant("res://Asset/Anime/Character/Zombie/Challenge/GargantuarZamboni/Scene/RedEyes/TowerDefenseZombieGargantuarZamboniRedEyes.tscn", "ZombieGargantuarRedEyes", "red-eyes");
				await VerifySaveAndNetworkRoundTrips(baseReplacement, control.characterNode);
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewGargantuarZamboniHypnosisReplacementRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			foreach (TowerDefenseCharacter ownedCharacter in _ownedCharacters)
			{
				if (GodotObject.IsInstanceValid(ownedCharacter) && !ownedCharacter.IsQueuedForDeletion())
				{
					ownedCharacter.QueueFree();
				}
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(4);
			RestoreReplacementFixtures();
			resourceStateField.SetValue(ResourceManager.Instance, previousResourceState);
		}
		bool flag = _failures == 0 && _checks == 21;
		GD.Print($"GARGANTUAR_ZAMBONI_HYPNOSIS_REPLACEMENT_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task<TowerDefenseZombie> VerifyVariant(string scenePath, string expectedReplacementName, string label)
	{
		TowerDefenseZombie source = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombie>(PackedScene.GenEditState.Disabled);
		Check(GodotObject.IsInstanceValid(source), "The real " + label + " Gargantuar Zamboni scene must instantiate.");
		if (!GodotObject.IsInstanceValid(source))
		{
			throw new InvalidOperationException("The " + label + " Gargantuar Zamboni scene is unavailable.");
		}
		_ownedCharacters.Add(source);
		source.editorPreviewMode = false;
		source.inGame = true;
		source.gridPos = new Vector2I(5, 2);
		source.GlobalPosition = new Vector2(600f, 252f);
		TowerDefenseManager.GetCharacterNode().AddChild(source, forceReadableName: false, InternalMode.Disabled);
		await WaitFrames(3);
		source.Hypnoses();
		await WaitFrames(2);
		Check((source.instance?.hypnoses ?? false) && source.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT && source.Scale.X < 0f && source.BuffGet("Hypnoses") is TowerDefenseCharacterBuffHypnoses, "The real " + label + " source must be fully hypnotized before leaving the map.");
		float x = source.GlobalPosition.X;
		source.WalkProcessing(0.5);
		Check(source.GlobalPosition.X > x, "The hypnotized " + label + " Zamboni must travel toward the right exit.");
		double groundRight = TowerDefenseManager.Instance.GetMapGroundRight();
		source.GlobalPosition = new Vector2((float)groundRight + 160f, source.GlobalPosition.Y);
		ulong physicsFrames = Engine.GetPhysicsFrames();
		source.randFreshIndex = (int)((30 - physicsFrames % 30) % 30);
		source.BatchUpdateValidated(0.0, physicsFrames);
		Check(source.isDestroy, "The hypnotized " + label + " Zamboni must take the real off-screen destroy path.");
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		TowerDefenseZombie replacement = FindReplacement(source, expectedReplacementName);
		Check(GodotObject.IsInstanceValid(replacement) && (double)replacement.GlobalPosition.X > groundRight, $"The {label} off-screen conversion must create {expectedReplacementName} outside the right edge.");
		if (!GodotObject.IsInstanceValid(replacement))
		{
			throw new InvalidOperationException("The " + label + " replacement Gargantuar was not created.");
		}
		_ownedCharacters.Add(replacement);
		replacement.ProcessMode = ProcessModeEnum.Disabled;
		replacement.GlobalPosition = new Vector2((float)groundRight - 300f, replacement.GlobalPosition.Y);
		replacement.ProcessMode = ProcessModeEnum.Inherit;
		Check((replacement.instance?.hypnoses ?? false) && replacement.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT && replacement.Scale.X < 0f && replacement.BuffGet("Hypnoses") is TowerDefenseCharacterBuffHypnoses, "The " + label + " replacement Gargantuar must retain hypnosis, camp, facing, and buff state.");
		replacement.Walk();
		for (int frame = 0; frame < 90; frame++)
		{
			if (!(replacement.CurrentStateHandle?.StableId != "zombie.walk"))
			{
				GroundMoveComponent groundMoveComponent = replacement.groundMoveComponent;
				if (groundMoveComponent != null && groundMoveComponent.Alive && groundMoveComponent.HasMovementSource)
				{
					break;
				}
			}
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		float beforeReplacementMove = replacement.GlobalPosition.X;
		await WaitPhysicsFrames(30);
		Check(GodotObject.IsInstanceValid(replacement) && replacement.GlobalPosition.X > beforeReplacementMove, "The hypnotized " + label + " replacement must keep walking out to the right, not return as an enemy.");
		if (!GodotObject.IsInstanceValid(replacement))
		{
			throw new InvalidOperationException("The " + label + " replacement was recycled during the movement probe.");
		}
		replacement.isPause = true;
		replacement.ProcessMode = ProcessModeEnum.Disabled;
		replacement.GlobalPosition = new Vector2(500f, replacement.GlobalPosition.Y);
		return replacement;
	}

	private async Task VerifySaveAndNetworkRoundTrips(TowerDefenseZombie replacement, Node2D characterNode)
	{
		TowerDefenseCharacterSaveConfigCSharp towerDefenseCharacterSaveConfigCSharp = new TowerDefenseCharacterSaveConfigCSharp();
		towerDefenseCharacterSaveConfigCSharp.SaveCharacter(replacement);
		Check(towerDefenseCharacterSaveConfigCSharp.instanceSave.GetValueOrDefault("hypnoses", false).AsBool() && towerDefenseCharacterSaveConfigCSharp.scaleX < 0.0 && HasBuff(towerDefenseCharacterSaveConfigCSharp.buffSave, "Hypnoses"), "Progress save data must retain the replacement Gargantuar's hypnosis instance, facing, and buff.");
		towerDefenseCharacterSaveConfigCSharp.pos = new Vector2(500f, 252f);
		towerDefenseCharacterSaveConfigCSharp.gridPos = new Vector2I(5, 2);
		TowerDefenseCharacter restored = towerDefenseCharacterSaveConfigCSharp.LoadCharacter();
		Check(GodotObject.IsInstanceValid(restored), "The saved replacement Gargantuar must instantiate for progress restore.");
		if (!GodotObject.IsInstanceValid(restored))
		{
			throw new InvalidOperationException("Progress restore did not create the replacement Gargantuar.");
		}
		_ownedCharacters.Add(restored);
		await WaitFrames(2);
		Check((restored.instance?.hypnoses ?? false) && restored.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT && restored.Scale.X < 0f && restored.BuffGet("Hypnoses") is TowerDefenseCharacterBuffHypnoses, "Progress restore must preserve the replacement Gargantuar's hypnotized allegiance.");
		if (restored is TowerDefenseZombie towerDefenseZombie)
		{
			towerDefenseZombie.isPause = true;
		}
		Dictionary networkSnapshot = replacement.buff.SyncSerialize();
		Check(networkSnapshot.ContainsKey("buffs") && HasBuff(networkSnapshot["buffs"].AsGodotArray(), "Hypnoses"), "The component network snapshot must carry the replacement Gargantuar's hypnosis buff.");
		TowerDefenseZombie remote = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Scene/Base/TowerDefenseZombieGargantuar.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombie>(PackedScene.GenEditState.Disabled);
		Check(GodotObject.IsInstanceValid(remote), "The real Gargantuar scene must instantiate for the remote snapshot probe.");
		if (!GodotObject.IsInstanceValid(remote))
		{
			throw new InvalidOperationException("The remote Gargantuar scene is unavailable.");
		}
		_ownedCharacters.Add(remote);
		remote.editorPreviewMode = false;
		remote.inGame = true;
		remote.gridPos = new Vector2I(4, 2);
		remote.GlobalPosition = new Vector2(400f, 252f);
		characterNode.AddChild(remote, forceReadableName: false, InternalMode.Disabled);
		await WaitFrames(2);
		remote.buff.SyncDeserialize(networkSnapshot);
		Check((remote.instance?.hypnoses ?? false) && remote.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT && remote.Scale.X < 0f && remote.BuffGet("Hypnoses") is TowerDefenseCharacterBuffHypnoses, "A remote Gargantuar must apply the network hypnosis snapshot read-only presentation state.");
		remote.isPause = true;
	}

	private static TowerDefenseZombie FindReplacement(TowerDefenseZombie source, string expectedConfigName)
	{
		foreach (Node child in TowerDefenseManager.GetCharacterNode().GetChildren())
		{
			if (child is TowerDefenseZombie towerDefenseZombie && towerDefenseZombie != source && towerDefenseZombie.config?.name == expectedConfigName)
			{
				return towerDefenseZombie;
			}
		}
		return null;
	}

	private static bool HasBuff(Array<Dictionary> buffs, string key)
	{
		if (buffs == null)
		{
			return false;
		}
		foreach (Dictionary buff in buffs)
		{
			if (buff.GetValueOrDefault("key", "").AsString() == key)
			{
				return true;
			}
		}
		return false;
	}

	private static bool HasBuff(Godot.Collections.Array buffs, string key)
	{
		if (buffs == null)
		{
			return false;
		}
		foreach (Variant buff in buffs)
		{
			if (buff.VariantType == Variant.Type.Dictionary && buff.AsGodotDictionary().GetValueOrDefault("key", "").AsString() == key)
			{
				return true;
			}
		}
		return false;
	}

	private void RegisterReplacementFixtures()
	{
		RegisterPacket("ZombieGargantuar", "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Packet/Base/ZombieGargantuar.tres");
		RegisterPacket("ZombieGargantuarRedEyes", "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Packet/Redeyes/ZombieGargantuarRedeyes.tres");
		RegisterCharacter("ZombieGargantuar", "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Scene/Base/TowerDefenseZombieGargantuar.tscn");
		RegisterCharacter("ZombieGargantuarRedEyes", "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Scene/RedEyes/TowerDefenseZombieGargantuarRedEyes.tscn");
	}

	private void RegisterPacket(string key, string path)
	{
		ResourceManager instance = ResourceManager.Instance;
		if (instance.TOWERDEFENSE_PACKETS.TryGetValue(key, out var value))
		{
			_previousPackets[key] = value;
		}
		else
		{
			_missingPackets.Add(key);
		}
		instance.TOWERDEFENSE_PACKETS[key] = ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore);
	}

	private void RegisterCharacter(string key, string path)
	{
		ResourceManager instance = ResourceManager.Instance;
		if (instance.TOWERDEFENSE_CHARCATERS.TryGetValue(key, out var value))
		{
			_previousCharacters[key] = value;
		}
		else
		{
			_missingCharacters.Add(key);
		}
		instance.TOWERDEFENSE_CHARCATERS[key] = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
	}

	private void RestoreReplacementFixtures()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return;
		}
		foreach (string missingPacket in _missingPackets)
		{
			instance.TOWERDEFENSE_PACKETS.Remove(missingPacket);
		}
		foreach (KeyValuePair<string, Resource> previousPacket in _previousPackets)
		{
			instance.TOWERDEFENSE_PACKETS[previousPacket.Key] = previousPacket.Value;
		}
		foreach (string missingCharacter in _missingCharacters)
		{
			instance.TOWERDEFENSE_CHARCATERS.Remove(missingCharacter);
		}
		foreach (KeyValuePair<string, Resource> previousCharacter in _previousCharacters)
		{
			instance.TOWERDEFENSE_CHARCATERS[previousCharacter.Key] = previousCharacter.Value;
		}
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private async Task WaitPhysicsFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewGargantuarZamboniHypnosisReplacementRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(8)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.FindReplacement, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "expectedConfigName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.HasBuff, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Array, "buffs", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterReplacementFixtures, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterPacket, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterCharacter, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RestoreReplacementFixtures, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.FindReplacement && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombie>(FindReplacement(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.HasBuff && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasBuff(VariantUtils.ConvertToArray<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.RegisterReplacementFixtures && args.Count == 0)
		{
			RegisterReplacementFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterPacket && args.Count == 2)
		{
			RegisterPacket(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterCharacter && args.Count == 2)
		{
			RegisterCharacter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreReplacementFixtures && args.Count == 0)
		{
			RestoreReplacementFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FindReplacement && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombie>(FindReplacement(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.HasBuff && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasBuff(VariantUtils.ConvertToArray<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.FindReplacement)
		{
			return true;
		}
		if (method == MethodName.HasBuff)
		{
			return true;
		}
		if (method == MethodName.RegisterReplacementFixtures)
		{
			return true;
		}
		if (method == MethodName.RegisterPacket)
		{
			return true;
		}
		if (method == MethodName.RegisterCharacter)
		{
			return true;
		}
		if (method == MethodName.RestoreReplacementFixtures)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
