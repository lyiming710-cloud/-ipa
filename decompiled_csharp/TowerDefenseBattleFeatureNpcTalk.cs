using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/NpcTalk/TowerDefenseBattleFeatureNpcTalk.cs")]
public class TowerDefenseBattleFeatureNpcTalk : TowerDefenseBattleFeature
{
	public delegate void TalkFinishEventHandler();

	public delegate void FinishEventHandler();

	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public static readonly StringName EmitTalkFinish = "EmitTalkFinish";

		public static readonly StringName EmitFinish = "EmitFinish";

		public new static readonly StringName Init = "Init";

		public static readonly StringName TalkNext = "TalkNext";

		public static readonly StringName RestoreTutorialToolState = "RestoreTutorialToolState";

		public static readonly StringName ContainsEmbeddedTutorial = "ContainsEmbeddedTutorial";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName npcTalkControl = "npcTalkControl";

		public static readonly StringName config = "config";

		public static readonly StringName currentIndex = "currentIndex";

		public static readonly StringName npcDictionary = "npcDictionary";

		public static readonly StringName _talkRunning = "_talkRunning";

		public static readonly StringName _tutorialActive = "_tutorialActive";

		public static readonly StringName _startDelay = "_startDelay";

		public static readonly StringName _temporaryBgm = "_temporaryBgm";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _npcTalkControl;

	private static PackedScene _npcCrazyDave;

	private static PackedScene _npcWeiWeiMi;

	public NpcTalkControl npcTalkControl;

	public NpcTalkConfig config;

	public int currentIndex;

	public Dictionary npcDictionary = new Dictionary();

	private bool _talkRunning;

	private bool _tutorialActive;

	private double _startDelay = 1.5;

	private string _temporaryBgm = "MainMenu";

	private static PackedScene NPC_TALK_CONTROL => _npcTalkControl ?? (_npcTalkControl = GD.Load<PackedScene>("uid://cjc8tc43kdv47"));

	private static PackedScene NPC_CRAZY_DAVE => _npcCrazyDave ?? (_npcCrazyDave = GD.Load<PackedScene>("uid://cvadcalsore2n"));

	private static PackedScene NPC_WEI_WEI_MI => _npcWeiWeiMi ?? (_npcWeiWeiMi = GD.Load<PackedScene>("uid://dolnrfw30x8ec"));

	public event TalkFinishEventHandler OnTalkFinish;

	public event FinishEventHandler OnFinish;

	public void EmitTalkFinish()
	{
		OnTalkFinish?.Invoke();
	}

	public void EmitFinish()
	{
		OnFinish?.Invoke();
	}

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		npcTalkControl = NPC_TALK_CONTROL.Instantiate<NpcTalkControl>(PackedScene.GenEditState.Disabled);
		npcTalkControl.npcTalkFeature = this;
		control.AddUI(npcTalkControl, 4);
		_startDelay = Math.Max(0.0, data.GetValueOrDefault("StartDelay", 1.5).AsDouble());
		_temporaryBgm = data.GetValueOrDefault("TemporaryBGM", "MainMenu").AsString();
		if (!data.GetValueOrDefault("isCustom", false).AsBool())
		{
			string text = data.GetValueOrDefault("TalkName", "").AsString();
			if (!(text != ""))
			{
				return;
			}
			NpcTalkConfig npcTalk = TowerDefenseManager.GetNpcTalk(text);
			if (!GodotObject.IsInstanceValid(npcTalk))
			{
				GD.PushWarning("[NpcTalk] Talk '" + text + "' is unavailable.");
				return;
			}
			config = npcTalk.Duplicate(deep: true) as NpcTalkConfig;
			if (GodotObject.IsInstanceValid(config))
			{
				config.Init();
			}
		}
		else
		{
			config = new NpcTalkConfig();
			config.Load(data);
		}
	}

	public override Task GameInit()
	{
		return Task.CompletedTask;
	}

	public async Task StartTalk()
	{
		if (!GodotObject.IsInstanceValid(config))
		{
			EmitTalkFinish();
			return;
		}
		bool flag = false;
		if (config.saveKey != "")
		{
			flag = GameSaveManager.Instance.GetTutorialValue(config.saveKey);
		}
		TaskCompletionSource<bool> tcs;
		if (!flag)
		{
			if (_startDelay > 0.0)
			{
				await ToSignal(control.GetTree().CreateTimer(_startDelay, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			}
			if (!IsLifetimeActive || !GodotObject.IsInstanceValid(npcTalkControl))
			{
				return;
			}
			TowerDefenseBattleFeatureBGM bgmFeature = GetFeature("BGM") as TowerDefenseBattleFeatureBGM;
			if (bgmFeature != null)
			{
				bgmFeature.StopBGM();
				if (_temporaryBgm != "")
				{
					bgmFeature.backgroundAudio = AudioManager.Instance.AudioPlay(_temporaryBgm, AudioManagerEnum.TYPE.MUSIC);
				}
			}
			currentIndex = 0;
			tcs = new TaskCompletionSource<bool>();
			OnFinish += FinishHandler;
			try
			{
				TalkNext();
				if (!(await WaitForTaskOrLifetime(tcs.Task)))
				{
					return;
				}
			}
			finally
			{
				OnFinish -= FinishHandler;
			}
			if (IsLifetimeActive)
			{
				bgmFeature?.PlayEntryBGM();
			}
		}
		if (IsLifetimeActive)
		{
			EmitTalkFinish();
		}
		void FinishHandler()
		{
			tcs.TrySetResult(result: true);
		}
	}

	public void TalkNext()
	{
		if (!_talkRunning && IsLifetimeActive)
		{
			_talkRunning = true;
			RunLifetimeTask(TalkNextAsync, "TalkNext");
		}
	}

	private async Task TalkNextAsync()
	{
		_ = 3;
		try
		{
			while (IsLifetimeActive && GodotObject.IsInstanceValid(config) && currentIndex < config.talkList.Count)
			{
				NpcTalkBaseConfig talk = config.talkList[currentIndex];
				NpcBase npc = await GetNpc(talk.npc);
				if (!IsLifetimeActive || !GodotObject.IsInstanceValid(npc) || !GodotObject.IsInstanceValid(npcTalkControl))
				{
					return;
				}
				npcTalkControl.ShowTalk(npc, talk);
				if (talk is NpcTalkHandConfig talk2)
				{
					npcTalkControl.ShowHand(npc, talk2);
				}
				if (npc.IsLeaveAnimation(talk.anime))
				{
					if (npcDictionary.ContainsKey(talk.npc))
					{
						NpcBase npcBase = npcDictionary[talk.npc].As<NpcBase>();
						if (GodotObject.IsInstanceValid(npcBase))
						{
							OnFinish -= npcBase.Finish;
						}
						npcDictionary.Remove(talk.npc);
					}
					NpcTalkTutorialConfig npcTalkTutorialConfig = talk as NpcTalkTutorialConfig;
					bool flag = npcTalkTutorialConfig != null;
					if (flag)
					{
						flag = !(await RunEmbeddedTutorial(npcTalkTutorialConfig));
					}
					if (flag)
					{
						return;
					}
				}
				else
				{
					NpcTalkTutorialConfig npcTalkTutorialConfig2 = talk as NpcTalkTutorialConfig;
					bool flag = npcTalkTutorialConfig2 != null;
					if (flag)
					{
						flag = !(await RunEmbeddedTutorial(npcTalkTutorialConfig2));
					}
					if (flag || !(await WaitForNpcTalkNext(npc)))
					{
						return;
					}
				}
				currentIndex++;
			}
			if (IsLifetimeActive)
			{
				EmitFinish();
			}
		}
		finally
		{
			_talkRunning = false;
		}
	}

	private async Task<bool> RunEmbeddedTutorial(NpcTalkTutorialConfig tutorialTalk)
	{
		if (!GodotObject.IsInstanceValid(TutorialManager.Instance))
		{
			return false;
		}
		_tutorialActive = true;
		TutorialManager.Instance.StartTutorial(tutorialTalk.tutorial);
		try
		{
			if (!(await WaitForTaskOrLifetime(TutorialManager.Instance.WaitForFinish())))
			{
				return false;
			}
			RestoreTutorialToolState();
			return true;
		}
		finally
		{
			_tutorialActive = false;
		}
	}

	private async Task<bool> WaitForNpcTalkNext(NpcBase npc)
	{
		if (!GodotObject.IsInstanceValid(npc))
		{
			return false;
		}
		TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
		npc.OnTalkNext += TalkNextHandler;
		try
		{
			return await WaitForTaskOrLifetime(tcs.Task);
		}
		finally
		{
			if (GodotObject.IsInstanceValid(npc))
			{
				npc.OnTalkNext -= TalkNextHandler;
			}
		}
		void TalkNextHandler()
		{
			tcs.TrySetResult(result: true);
		}
	}

	private void RestoreTutorialToolState()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			TowerDefenseMap currentMap = instance.GetCurrentMap();
			if (GodotObject.IsInstanceValid(currentMap))
			{
				currentMap.BackShovel();
			}
			TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = GetFeature("Map") as TowerDefenseBattleFeatureMap;
			if (GodotObject.IsInstanceValid(towerDefenseBattleFeatureMap) && GodotObject.IsInstanceValid(towerDefenseBattleFeatureMap.shovelManager))
			{
				towerDefenseBattleFeatureMap.shovelManager.ShovelReset();
			}
		}
	}

	public bool ContainsEmbeddedTutorial()
	{
		if (!GodotObject.IsInstanceValid(config))
		{
			return false;
		}
		foreach (NpcTalkBaseConfig talk in config.talkList)
		{
			if (talk is NpcTalkTutorialConfig)
			{
				return true;
			}
		}
		return false;
	}

	public async Task<NpcBase> GetNpc(string npcName = "CrazyDave")
	{
		if (npcDictionary.ContainsKey(npcName) && !GodotObject.IsInstanceValid(npcDictionary[npcName].AsGodotObject()))
		{
			npcDictionary.Remove(npcName);
		}
		TaskCompletionSource<bool> tcs;
		if (!npcDictionary.ContainsKey(npcName))
		{
			PackedScene packedScene;
			if (npcName == "CrazyDave")
			{
				packedScene = NPC_CRAZY_DAVE;
			}
			else
			{
				packedScene = ((!(npcName == "WeiWeiMi")) ? null : NPC_WEI_WEI_MI);
			}
			PackedScene packedScene2 = packedScene;
			if (!GodotObject.IsInstanceValid(packedScene2))
			{
				GD.PushWarning("[NpcTalk] Unsupported NPC '" + npcName + "'.");
				return null;
			}
			if (!IsLifetimeActive || !GodotObject.IsInstanceValid(npcTalkControl))
			{
				return null;
			}
			NpcBase instance = packedScene2.Instantiate<NpcBase>(PackedScene.GenEditState.Disabled);
			tcs = new TaskCompletionSource<bool>();
			instance.OnNpcReady += NpcReadyHandler;
			npcDictionary[npcName] = instance;
			OnFinish += instance.Finish;
			try
			{
				npcTalkControl.AddNpc(instance);
				if (!(await WaitForTaskOrLifetime(tcs.Task)))
				{
					return null;
				}
			}
			finally
			{
				if (GodotObject.IsInstanceValid(instance))
				{
					instance.OnNpcReady -= NpcReadyHandler;
				}
			}
		}
		return npcDictionary[npcName].As<NpcBase>();
		void NpcReadyHandler()
		{
			tcs.TrySetResult(result: true);
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		if (_tutorialActive && GodotObject.IsInstanceValid(TutorialManager.Instance))
		{
			TutorialManager.Instance.TutorialClear();
		}
		RestoreTutorialToolState();
		foreach (Variant value in npcDictionary.Values)
		{
			NpcBase npcBase = value.As<NpcBase>();
			if (GodotObject.IsInstanceValid(npcBase))
			{
				OnFinish -= npcBase.Finish;
			}
		}
		npcDictionary.Clear();
		OnFinish = null;
		OnTalkFinish = null;
		if (GodotObject.IsInstanceValid(npcTalkControl))
		{
			npcTalkControl.npcTalkFeature = null;
			npcTalkControl.QueueFree();
		}
		npcTalkControl = null;
		config = null;
		_talkRunning = false;
		_tutorialActive = false;
	}

	public override async Task GameEntry()
	{
		if (!control.hasProgress)
		{
			await StartTalk();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.EmitTalkFinish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitFinish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TalkNext, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreTutorialToolState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ContainsEmbeddedTutorial, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EmitTalkFinish && args.Count == 0)
		{
			EmitTalkFinish();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitFinish && args.Count == 0)
		{
			EmitFinish();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TalkNext && args.Count == 0)
		{
			TalkNext();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreTutorialToolState && args.Count == 0)
		{
			RestoreTutorialToolState();
			ret = default;
			return true;
		}
		if (method == MethodName.ContainsEmbeddedTutorial && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ContainsEmbeddedTutorial());
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EmitTalkFinish)
		{
			return true;
		}
		if (method == MethodName.EmitFinish)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.TalkNext)
		{
			return true;
		}
		if (method == MethodName.RestoreTutorialToolState)
		{
			return true;
		}
		if (method == MethodName.ContainsEmbeddedTutorial)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.npcTalkControl)
		{
			npcTalkControl = VariantUtils.ConvertTo<NpcTalkControl>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<NpcTalkConfig>(in value);
			return true;
		}
		if (name == PropertyName.currentIndex)
		{
			currentIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.npcDictionary)
		{
			npcDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._talkRunning)
		{
			_talkRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._tutorialActive)
		{
			_tutorialActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._startDelay)
		{
			_startDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._temporaryBgm)
		{
			_temporaryBgm = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.npcTalkControl)
		{
			value = VariantUtils.CreateFrom(in npcTalkControl);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.currentIndex)
		{
			value = VariantUtils.CreateFrom(in currentIndex);
			return true;
		}
		if (name == PropertyName.npcDictionary)
		{
			value = VariantUtils.CreateFrom(in npcDictionary);
			return true;
		}
		if (name == PropertyName._talkRunning)
		{
			value = VariantUtils.CreateFrom(in _talkRunning);
			return true;
		}
		if (name == PropertyName._tutorialActive)
		{
			value = VariantUtils.CreateFrom(in _tutorialActive);
			return true;
		}
		if (name == PropertyName._startDelay)
		{
			value = VariantUtils.CreateFrom(in _startDelay);
			return true;
		}
		if (name == PropertyName._temporaryBgm)
		{
			value = VariantUtils.CreateFrom(in _temporaryBgm);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.npcTalkControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.npcDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._talkRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._tutorialActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._startDelay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._temporaryBgm, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.npcTalkControl, Variant.From(in npcTalkControl));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.currentIndex, Variant.From(in currentIndex));
		info.AddProperty(PropertyName.npcDictionary, Variant.From(in npcDictionary));
		info.AddProperty(PropertyName._talkRunning, Variant.From(in _talkRunning));
		info.AddProperty(PropertyName._tutorialActive, Variant.From(in _tutorialActive));
		info.AddProperty(PropertyName._startDelay, Variant.From(in _startDelay));
		info.AddProperty(PropertyName._temporaryBgm, Variant.From(in _temporaryBgm));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.npcTalkControl, out var value))
		{
			npcTalkControl = value.As<NpcTalkControl>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<NpcTalkConfig>();
		}
		if (info.TryGetProperty(PropertyName.currentIndex, out var value3))
		{
			currentIndex = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.npcDictionary, out var value4))
		{
			npcDictionary = value4.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._talkRunning, out var value5))
		{
			_talkRunning = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._tutorialActive, out var value6))
		{
			_tutorialActive = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._startDelay, out var value7))
		{
			_startDelay = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._temporaryBgm, out var value8))
		{
			_temporaryBgm = value8.As<string>();
		}
	}
}
