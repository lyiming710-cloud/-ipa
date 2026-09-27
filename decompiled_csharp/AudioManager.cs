using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/AudioManager/AudioManager.cs")]
public class AudioManager : Node
{
	public struct AudioRequest
	{
		public string Stream;

		public AudioManagerEnum.TYPE Type;

		public double Pos;

		public bool PauseAlive;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName SetupBuses = "SetupBuses";

		public static readonly StringName _GetBusName = "_GetBusName";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName MemberFind = "MemberFind";

		public static readonly StringName VolumSet = "VolumSet";

		public static readonly StringName VolumGet = "VolumGet";

		public static readonly StringName AudioStopAll = "AudioStopAll";

		public static readonly StringName ReleaseAudioPlayers = "ReleaseAudioPlayers";

		public static readonly StringName AudioPlay = "AudioPlay";

		public static readonly StringName _AudioPlay = "_AudioPlay";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName DuplicateSfxRejectCountForTest = "DuplicateSfxRejectCountForTest";

		public static readonly StringName AudioResourceLookupCountForTest = "AudioResourceLookupCountForTest";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const ulong SfxBatchPhysicsInterval = 4uL;

	public static AudioManager Instance;

	public Dictionary<AudioManagerEnum.TYPE, Dictionary<string, AudioStreamPlayerMember>> audioMemberDictionary = new Dictionary<AudioManagerEnum.TYPE, Dictionary<string, AudioStreamPlayerMember>>();

	public List<AudioRequest> audioStack = new List<AudioRequest>();

	private HashSet<string> _queuedSfxStreams = new HashSet<string>();

	private HashSet<string> _sfxPlayedList = new HashSet<string>();

	public ulong DuplicateSfxRejectCountForTest { get; private set; }

	public ulong AudioResourceLookupCountForTest { get; private set; }

	public override void _Ready()
	{
		Instance = this;
		audioMemberDictionary[AudioManagerEnum.TYPE.MUSIC] = new Dictionary<string, AudioStreamPlayerMember>();
		audioMemberDictionary[AudioManagerEnum.TYPE.SFX] = new Dictionary<string, AudioStreamPlayerMember>();
		SetupBuses();
		GameSaveManager instance = GameSaveManager.Instance;
		if (instance != null)
		{
			VolumSet(AudioManagerEnum.TYPE.MUSIC, (double)instance.GetConfigValue("MusicVolum"));
			VolumSet(AudioManagerEnum.TYPE.SFX, (double)instance.GetConfigValue("SfxVolum"));
		}
	}

	public override void _ExitTree()
	{
		ReleaseAudioPlayers(queueFree: false);
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void SetupBuses()
	{
		if (AudioServer.GetBusIndex("Music") == -1)
		{
			AudioServer.AddBus();
			AudioServer.SetBusName(AudioServer.BusCount - 1, "Music");
			AudioServer.SetBusSend(AudioServer.BusCount - 1, "Master");
		}
		if (AudioServer.GetBusIndex("SFX") == -1)
		{
			AudioServer.AddBus();
			AudioServer.SetBusName(AudioServer.BusCount - 1, "SFX");
			AudioServer.SetBusSend(AudioServer.BusCount - 1, "Master");
		}
	}

	private string _GetBusName(AudioManagerEnum.TYPE type)
	{
		return type switch
		{
			AudioManagerEnum.TYPE.MUSIC => "Music", 
			AudioManagerEnum.TYPE.SFX => "SFX", 
			_ => "Master", 
		};
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Engine.GetPhysicsFrames() % 4 != 0L || audioStack.Count <= 0)
		{
			return;
		}
		_sfxPlayedList.Clear();
		foreach (AudioRequest item in audioStack)
		{
			if (!_sfxPlayedList.Add(item.Stream))
			{
				continue;
			}
			AudioStreamPlayerMember audioStreamPlayerMember = MemberFind(item.Stream, item.Type);
			if (GodotObject.IsInstanceValid(audioStreamPlayerMember))
			{
				audioStreamPlayerMember.Play((float)item.Pos);
				if (item.PauseAlive)
				{
					audioStreamPlayerMember.ProcessMode = ProcessModeEnum.Always;
				}
				else
				{
					audioStreamPlayerMember.ProcessMode = ProcessModeEnum.Pausable;
				}
			}
		}
		audioStack.Clear();
		_queuedSfxStreams.Clear();
	}

	public AudioStreamPlayerMember MemberFind(string stream, AudioManagerEnum.TYPE type = AudioManagerEnum.TYPE.SFX)
	{
		if (string.IsNullOrEmpty(stream))
		{
			return null;
		}
		AudioStream audioStream = null;
		Dictionary<string, AudioStreamPlayerMember> dictionary = audioMemberDictionary[type];
		if (dictionary.TryGetValue(stream, out var value) && GodotObject.IsInstanceValid(value))
		{
			return value;
		}
		if (ResourceManager.Instance != null && ResourceManager.Instance.AUDIOS.ContainsKey(stream))
		{
			audioStream = (AudioStream)ResourceManager.Instance.AUDIOS[stream];
		}
		if (GodotObject.IsInstanceValid(audioStream))
		{
			AudioStreamPlayerMember audioStreamPlayerMember = new AudioStreamPlayerMember();
			audioStreamPlayerMember.type = type;
			audioStreamPlayerMember.Bus = _GetBusName(type);
			audioStreamPlayerMember.Stream = audioStream;
			audioStreamPlayerMember.ProcessMode = ProcessModeEnum.Pausable;
			if (type == AudioManagerEnum.TYPE.SFX)
			{
				audioStreamPlayerMember.MaxPolyphony = 10;
			}
			AddChild(audioStreamPlayerMember, forceReadableName: false, InternalMode.Disabled);
			dictionary[stream] = audioStreamPlayerMember;
			return audioStreamPlayerMember;
		}
		return null;
	}

	public void VolumSet(AudioManagerEnum.TYPE type = AudioManagerEnum.TYPE.SFX, double valum = 1.0)
	{
		int busIndex = AudioServer.GetBusIndex(_GetBusName(type));
		if (busIndex != -1)
		{
			AudioServer.SetBusVolumeDb(busIndex, (float)Mathf.LinearToDb(valum));
			AudioServer.SetBusMute(busIndex, valum <= 0.0);
		}
	}

	public double VolumGet(AudioManagerEnum.TYPE type = AudioManagerEnum.TYPE.SFX)
	{
		int busIndex = AudioServer.GetBusIndex(_GetBusName(type));
		if (busIndex != -1)
		{
			return Mathf.DbToLinear(AudioServer.GetBusVolumeDb(busIndex));
		}
		return 1.0;
	}

	public void AudioStopAll()
	{
		ReleaseAudioPlayers(queueFree: true);
	}

	private void ReleaseAudioPlayers(bool queueFree)
	{
		foreach (Node child in GetChildren())
		{
			if (child is AudioStreamPlayer audioStreamPlayer)
			{
				audioStreamPlayer.Stop();
				audioStreamPlayer.Stream = null;
			}
			if (queueFree && GodotObject.IsInstanceValid(child))
			{
				child.QueueFree();
			}
		}
		foreach (Dictionary<string, AudioStreamPlayerMember> value in audioMemberDictionary.Values)
		{
			value.Clear();
		}
		audioStack.Clear();
		_queuedSfxStreams.Clear();
		_sfxPlayedList.Clear();
	}

	public AudioStreamPlayerMember AudioPlay(string stream, AudioManagerEnum.TYPE type = AudioManagerEnum.TYPE.SFX, double pos = 0.0, bool once = true, bool pauseAlive = false)
	{
		if (string.IsNullOrEmpty(stream))
		{
			return null;
		}
		bool flag = false;
		if (type == AudioManagerEnum.TYPE.SFX)
		{
			if (!_queuedSfxStreams.Add(stream))
			{
				DuplicateSfxRejectCountForTest++;
				return null;
			}
			flag = true;
		}
		AudioResourceLookupCountForTest++;
		if (ResourceManager.Instance == null || !ResourceManager.Instance.AUDIOS.ContainsKey(stream))
		{
			if (flag)
			{
				_queuedSfxStreams.Remove(stream);
			}
			return null;
		}
		AudioStreamPlayerMember audioStreamPlayerMember = null;
		switch (type)
		{
		case AudioManagerEnum.TYPE.SFX:
			audioStack.Add(new AudioRequest
			{
				Stream = stream,
				Type = type,
				Pos = pos,
				PauseAlive = pauseAlive
			});
			break;
		case AudioManagerEnum.TYPE.MUSIC:
			audioStreamPlayerMember = MemberFind(stream, type);
			if (pauseAlive)
			{
				audioStreamPlayerMember.ProcessMode = ProcessModeEnum.Always;
			}
			else
			{
				audioStreamPlayerMember.ProcessMode = ProcessModeEnum.Pausable;
			}
			audioStreamPlayerMember.Play((float)pos);
			break;
		}
		return audioStreamPlayerMember;
	}

	public static AudioStreamPlayerMember _AudioPlay(string stream, AudioManagerEnum.TYPE type = AudioManagerEnum.TYPE.SFX, double pos = 0.0, bool once = true, bool pauseAlive = false)
	{
		if (Instance == null)
		{
			return null;
		}
		return Instance.AudioPlay(stream, type, pos, once, pauseAlive);
	}

	public AudioManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/AudioManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupBuses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetBusName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MemberFind, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AudioStreamPlayer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stream", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.VolumSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "valum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.VolumGet, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AudioStopAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseAudioPlayers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "queueFree", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AudioPlay, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AudioStreamPlayer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stream", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "once", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "pauseAlive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._AudioPlay, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AudioStreamPlayer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stream", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "once", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "pauseAlive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupBuses && args.Count == 0)
		{
			SetupBuses();
			ret = default;
			return true;
		}
		if (method == MethodName._GetBusName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(_GetBusName(VariantUtils.ConvertTo<AudioManagerEnum.TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MemberFind && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<AudioStreamPlayerMember>(MemberFind(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<AudioManagerEnum.TYPE>(in args[1])));
			return true;
		}
		if (method == MethodName.VolumSet && args.Count == 2)
		{
			VolumSet(VariantUtils.ConvertTo<AudioManagerEnum.TYPE>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.VolumGet && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(VolumGet(VariantUtils.ConvertTo<AudioManagerEnum.TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.AudioStopAll && args.Count == 0)
		{
			AudioStopAll();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseAudioPlayers && args.Count == 1)
		{
			ReleaseAudioPlayers(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AudioPlay && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<AudioStreamPlayerMember>(AudioPlay(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<AudioManagerEnum.TYPE>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName._AudioPlay && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<AudioStreamPlayerMember>(_AudioPlay(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<AudioManagerEnum.TYPE>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._AudioPlay && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<AudioStreamPlayerMember>(_AudioPlay(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<AudioManagerEnum.TYPE>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
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
		if (method == MethodName.SetupBuses)
		{
			return true;
		}
		if (method == MethodName._GetBusName)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.MemberFind)
		{
			return true;
		}
		if (method == MethodName.VolumSet)
		{
			return true;
		}
		if (method == MethodName.VolumGet)
		{
			return true;
		}
		if (method == MethodName.AudioStopAll)
		{
			return true;
		}
		if (method == MethodName.ReleaseAudioPlayers)
		{
			return true;
		}
		if (method == MethodName.AudioPlay)
		{
			return true;
		}
		if (method == MethodName._AudioPlay)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.DuplicateSfxRejectCountForTest)
		{
			DuplicateSfxRejectCountForTest = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.AudioResourceLookupCountForTest)
		{
			AudioResourceLookupCountForTest = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		ulong from;
		if (name == PropertyName.DuplicateSfxRejectCountForTest)
		{
			from = DuplicateSfxRejectCountForTest;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AudioResourceLookupCountForTest)
		{
			from = AudioResourceLookupCountForTest;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.DuplicateSfxRejectCountForTest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AudioResourceLookupCountForTest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.DuplicateSfxRejectCountForTest, Variant.From<ulong>(DuplicateSfxRejectCountForTest));
		info.AddProperty(PropertyName.AudioResourceLookupCountForTest, Variant.From<ulong>(AudioResourceLookupCountForTest));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.DuplicateSfxRejectCountForTest, out var value))
		{
			DuplicateSfxRejectCountForTest = value.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.AudioResourceLookupCountForTest, out var value2))
		{
			AudioResourceLookupCountForTest = value2.As<ulong>();
		}
	}
}
