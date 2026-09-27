using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/BattleEventBus/BattleEventBus.cs")]
public class BattleEventBus : Node
{
	public delegate void CharacterDestroyEventHandler(TowerDefensePacketConfig packet, Vector2 pos, Vector2 gridPos, TowerDefenseEnum.CHARACTER_CAMP camp, double scale, double hitpointScale);

	public delegate void CharacterSpawnedEventHandler(TowerDefenseCharacter character);

	public delegate void CharacterHurtEventHandler(TowerDefenseCharacter character, int damage, Node source);

	public delegate void ColdEffectEmitEventHandler();

	public delegate void BlowAllEffectEmitEventHandler();

	public delegate void BlowLineEffectEmitEventHandler(int line);

	public delegate void JalaLineEffectEmitEventHandler(int line);

	public delegate void JalaRowEffectEmitEventHandler(int row);

	public delegate void JalaGridEffectEmitEventHandler(Vector2I gridPos);

	public delegate void GameStartedEventHandler();

	public delegate void GameFailedEventHandler();

	public delegate void GameVictoryEventHandler();

	public delegate void GamePausedEventHandler(bool paused);

	public delegate void WaveStartedEventHandler(int waveIndex);

	public delegate void UiSwitchedEventHandler(bool shown);

	public delegate void CharacterSkinSwitchedEventHandler(string packetSaveKey, string customKey);

	public delegate void PacketUIFrontEventHandler(bool open);

	public delegate void ShowPlantHealthEventHandler(bool show);

	public delegate void ShowZombieHealthEventHandler(bool show);

	public delegate void ShowBossHealthBarEventHandler(bool show);

	public delegate void ScreenTransformChangedEventHandler();

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName EmitCharacterHurt = "EmitCharacterHurt";

		public static readonly StringName EmitCharacterDestroy = "EmitCharacterDestroy";

		public static readonly StringName EmitCharacterSpawned = "EmitCharacterSpawned";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnViewportSizeChanged = "OnViewportSizeChanged";

		public static readonly StringName EmitColdEffectEmit = "EmitColdEffectEmit";

		public static readonly StringName EmitBlowAllEffectEmit = "EmitBlowAllEffectEmit";

		public static readonly StringName EmitBlowLineEffectEmit = "EmitBlowLineEffectEmit";

		public static readonly StringName EmitJalaLineEffectEmit = "EmitJalaLineEffectEmit";

		public static readonly StringName EmitJalaRowEffectEmit = "EmitJalaRowEffectEmit";

		public static readonly StringName EmitJalaGridEffectEmit = "EmitJalaGridEffectEmit";

		public static readonly StringName EmitGameStarted = "EmitGameStarted";

		public static readonly StringName EmitGameFailed = "EmitGameFailed";

		public static readonly StringName EmitGameVictory = "EmitGameVictory";

		public static readonly StringName EmitGamePaused = "EmitGamePaused";

		public static readonly StringName EmitWaveStarted = "EmitWaveStarted";

		public static readonly StringName EmitUiSwitched = "EmitUiSwitched";

		public static readonly StringName EmitCharacterSkinSwitched = "EmitCharacterSkinSwitched";

		public static readonly StringName EmitPacketUIFront = "EmitPacketUIFront";

		public static readonly StringName EmitShowPlantHealth = "EmitShowPlantHealth";

		public static readonly StringName EmitShowZombieHealth = "EmitShowZombieHealth";

		public static readonly StringName EmitShowBossHealthBar = "EmitShowBossHealthBar";

		public static readonly StringName EmitScreenTransformChanged = "EmitScreenTransformChanged";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName HasCharacterHurtSubscribers = "HasCharacterHurtSubscribers";
	}

	public new class SignalName : Node.SignalName
	{
	}

	public static BattleEventBus Instance;

	public bool HasCharacterHurtSubscribers => OnCharacterHurt != null;

	public event CharacterDestroyEventHandler OnCharacterDestroy;

	public event CharacterSpawnedEventHandler OnCharacterSpawned;

	public event CharacterHurtEventHandler OnCharacterHurt;

	public event ColdEffectEmitEventHandler OnColdEffectEmit;

	public event BlowAllEffectEmitEventHandler OnBlowAllEffectEmit;

	public event BlowLineEffectEmitEventHandler OnBlowLineEffectEmit;

	public event JalaLineEffectEmitEventHandler OnJalaLineEffectEmit;

	public event JalaRowEffectEmitEventHandler OnJalaRowEffectEmit;

	public event JalaGridEffectEmitEventHandler OnJalaGridEffectEmit;

	public event GameStartedEventHandler OnGameStarted;

	public event GameFailedEventHandler OnGameFailed;

	public event GameVictoryEventHandler OnGameVictory;

	public event GamePausedEventHandler OnGamePaused;

	public event WaveStartedEventHandler OnWaveStarted;

	public event UiSwitchedEventHandler OnUiSwitched;

	public event CharacterSkinSwitchedEventHandler OnCharacterSkinSwitched;

	public event PacketUIFrontEventHandler OnPacketUIFront;

	public event ShowPlantHealthEventHandler OnShowPlantHealth;

	public event ShowZombieHealthEventHandler OnShowZombieHealth;

	public event ShowBossHealthBarEventHandler OnShowBossHealthBar;

	public event ScreenTransformChangedEventHandler OnScreenTransformChanged;

	public void EmitCharacterHurt(TowerDefenseCharacter character, int damage, Node source)
	{
		OnCharacterHurt?.Invoke(character, damage, source);
	}

	public void EmitCharacterDestroy(TowerDefensePacketConfig packet, Vector2 pos, Vector2 gridPos, TowerDefenseEnum.CHARACTER_CAMP camp, double scale, double hitpointScale)
	{
		OnCharacterDestroy?.Invoke(packet, pos, gridPos, camp, scale, hitpointScale);
	}

	public void EmitCharacterSpawned(TowerDefenseCharacter character)
	{
		OnCharacterSpawned?.Invoke(character);
	}

	public override void _Ready()
	{
		Instance = this;
		GetViewport().SizeChanged += OnViewportSizeChanged;
	}

	private void OnViewportSizeChanged()
	{
		OnScreenTransformChanged?.Invoke();
	}

	public void EmitColdEffectEmit()
	{
		OnColdEffectEmit?.Invoke();
	}

	public void EmitBlowAllEffectEmit()
	{
		OnBlowAllEffectEmit?.Invoke();
	}

	public void EmitBlowLineEffectEmit(int line)
	{
		OnBlowLineEffectEmit?.Invoke(line);
	}

	public void EmitJalaLineEffectEmit(int line)
	{
		OnJalaLineEffectEmit?.Invoke(line);
	}

	public void EmitJalaRowEffectEmit(int row)
	{
		OnJalaRowEffectEmit?.Invoke(row);
	}

	public void EmitJalaGridEffectEmit(Vector2I gridPos)
	{
		OnJalaGridEffectEmit?.Invoke(gridPos);
	}

	public void EmitGameStarted()
	{
		OnGameStarted?.Invoke();
	}

	public void EmitGameFailed()
	{
		OnGameFailed?.Invoke();
	}

	public void EmitGameVictory()
	{
		OnGameVictory?.Invoke();
	}

	public void EmitGamePaused(bool paused)
	{
		OnGamePaused?.Invoke(paused);
	}

	public void EmitWaveStarted(int waveIndex)
	{
		OnWaveStarted?.Invoke(waveIndex);
	}

	public void EmitUiSwitched(bool shown)
	{
		OnUiSwitched?.Invoke(shown);
	}

	public void EmitCharacterSkinSwitched(string packetSaveKey, string customKey)
	{
		OnCharacterSkinSwitched?.Invoke(packetSaveKey, customKey);
	}

	public void EmitPacketUIFront(bool open)
	{
		OnPacketUIFront?.Invoke(open);
	}

	public void EmitShowPlantHealth(bool show)
	{
		OnShowPlantHealth?.Invoke(show);
	}

	public void EmitShowZombieHealth(bool show)
	{
		OnShowZombieHealth?.Invoke(show);
	}

	public void EmitShowBossHealthBar(bool show)
	{
		OnShowBossHealthBar?.Invoke(show);
	}

	public void EmitScreenTransformChanged()
	{
		OnScreenTransformChanged?.Invoke();
	}

	public BattleEventBus()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/BattleEventBus");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(23)
		{
			new MethodInfo(MethodName.EmitCharacterHurt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "damage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.EmitCharacterDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "scale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "hitpointScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitCharacterSpawned, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnViewportSizeChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitColdEffectEmit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitBlowAllEffectEmit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitBlowLineEffectEmit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitJalaLineEffectEmit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitJalaRowEffectEmit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "row", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitJalaGridEffectEmit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitGameStarted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitGameFailed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitGameVictory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitGamePaused, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "paused", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitWaveStarted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitUiSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "shown", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitCharacterSkinSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetSaveKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitPacketUIFront, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitShowPlantHealth, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "show", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitShowZombieHealth, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "show", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitShowBossHealthBar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "show", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitScreenTransformChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EmitCharacterHurt && args.Count == 3)
		{
			EmitCharacterHurt(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Node>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitCharacterDestroy && args.Count == 6)
		{
			EmitCharacterDestroy(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitCharacterSpawned && args.Count == 1)
		{
			EmitCharacterSpawned(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.OnViewportSizeChanged && args.Count == 0)
		{
			OnViewportSizeChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitColdEffectEmit && args.Count == 0)
		{
			EmitColdEffectEmit();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitBlowAllEffectEmit && args.Count == 0)
		{
			EmitBlowAllEffectEmit();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitBlowLineEffectEmit && args.Count == 1)
		{
			EmitBlowLineEffectEmit(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitJalaLineEffectEmit && args.Count == 1)
		{
			EmitJalaLineEffectEmit(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitJalaRowEffectEmit && args.Count == 1)
		{
			EmitJalaRowEffectEmit(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitJalaGridEffectEmit && args.Count == 1)
		{
			EmitJalaGridEffectEmit(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitGameStarted && args.Count == 0)
		{
			EmitGameStarted();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitGameFailed && args.Count == 0)
		{
			EmitGameFailed();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitGameVictory && args.Count == 0)
		{
			EmitGameVictory();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitGamePaused && args.Count == 1)
		{
			EmitGamePaused(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitWaveStarted && args.Count == 1)
		{
			EmitWaveStarted(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitUiSwitched && args.Count == 1)
		{
			EmitUiSwitched(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitCharacterSkinSwitched && args.Count == 2)
		{
			EmitCharacterSkinSwitched(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitPacketUIFront && args.Count == 1)
		{
			EmitPacketUIFront(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitShowPlantHealth && args.Count == 1)
		{
			EmitShowPlantHealth(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitShowZombieHealth && args.Count == 1)
		{
			EmitShowZombieHealth(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitShowBossHealthBar && args.Count == 1)
		{
			EmitShowBossHealthBar(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitScreenTransformChanged && args.Count == 0)
		{
			EmitScreenTransformChanged();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EmitCharacterHurt)
		{
			return true;
		}
		if (method == MethodName.EmitCharacterDestroy)
		{
			return true;
		}
		if (method == MethodName.EmitCharacterSpawned)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.OnViewportSizeChanged)
		{
			return true;
		}
		if (method == MethodName.EmitColdEffectEmit)
		{
			return true;
		}
		if (method == MethodName.EmitBlowAllEffectEmit)
		{
			return true;
		}
		if (method == MethodName.EmitBlowLineEffectEmit)
		{
			return true;
		}
		if (method == MethodName.EmitJalaLineEffectEmit)
		{
			return true;
		}
		if (method == MethodName.EmitJalaRowEffectEmit)
		{
			return true;
		}
		if (method == MethodName.EmitJalaGridEffectEmit)
		{
			return true;
		}
		if (method == MethodName.EmitGameStarted)
		{
			return true;
		}
		if (method == MethodName.EmitGameFailed)
		{
			return true;
		}
		if (method == MethodName.EmitGameVictory)
		{
			return true;
		}
		if (method == MethodName.EmitGamePaused)
		{
			return true;
		}
		if (method == MethodName.EmitWaveStarted)
		{
			return true;
		}
		if (method == MethodName.EmitUiSwitched)
		{
			return true;
		}
		if (method == MethodName.EmitCharacterSkinSwitched)
		{
			return true;
		}
		if (method == MethodName.EmitPacketUIFront)
		{
			return true;
		}
		if (method == MethodName.EmitShowPlantHealth)
		{
			return true;
		}
		if (method == MethodName.EmitShowZombieHealth)
		{
			return true;
		}
		if (method == MethodName.EmitShowBossHealthBar)
		{
			return true;
		}
		if (method == MethodName.EmitScreenTransformChanged)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.HasCharacterHurtSubscribers)
		{
			value = VariantUtils.CreateFrom<bool>(HasCharacterHurtSubscribers);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasCharacterHurtSubscribers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
