using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/VasePacketChildClipProbe.cs")]
public class VasePacketChildClipProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "VASE_PACKET_CHILD_CLIP_RESULT";

	private const string PacketScenePath = "res://Prefab/TowerDefense/GUI/InGame/Packet/TowerDefenseInGamePacketShow.tscn";

	private const string PacketConfigPath = "res://Asset/Anime/Character/Plant/Chapter0/ThreePeater/Packet/PlantThreePeater.tres";

	private const string CharacterSpritePath = "res://Asset/Anime/Character/Plant/Chapter0/ThreePeater/ThreePeater.tscn";

	public override async void _Ready()
	{
		bool passed = false;
		string failure = string.Empty;
		try
		{
			PackedScene packedScene = GD.Load<PackedScene>("res://Prefab/TowerDefense/GUI/InGame/Packet/TowerDefenseInGamePacketShow.tscn");
			TowerDefensePacketConfig packetConfig = GD.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Plant/Chapter0/ThreePeater/Packet/PlantThreePeater.tres");
			PackedScene packedScene2 = GD.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/ThreePeater/ThreePeater.tscn");
			if (packedScene == null || packetConfig == null || packedScene2 == null)
			{
				throw new InvalidOperationException("Unable to load the packet, config, or ThreePeater sprite scene.");
			}
			if (ResourceManager.Instance == null)
			{
				throw new InvalidOperationException("ResourceManager autoload is unavailable.");
			}
			ResourceManager.Instance.CHARCTAER_SPRITE[packetConfig.saveKey] = packedScene2;
			TowerDefenseInGamePacketShow packet = packedScene.Instantiate<TowerDefenseInGamePacketShow>(PackedScene.GenEditState.Disabled);
			AddChild(packet, forceReadableName: false, InternalMode.Disabled);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			packet.Init(packetConfig);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			AdobeAnimateSprite sprite = packet.sprite;
			AdobeAnimateSprite adobeAnimateSprite = sprite?.FindChild("Head1", recursive: true, owned: false) as AdobeAnimateSprite;
			AdobeAnimateSprite adobeAnimateSprite2 = sprite?.FindChild("Head2", recursive: true, owned: false) as AdobeAnimateSprite;
			AdobeAnimateSprite adobeAnimateSprite3 = sprite?.FindChild("Head3", recursive: true, owned: false) as AdobeAnimateSprite;
			passed = GodotObject.IsInstanceValid(sprite) && sprite.clip == "BodyIdle" && GodotObject.IsInstanceValid(adobeAnimateSprite) && adobeAnimateSprite.clip == "HeadIdle1" && GodotObject.IsInstanceValid(adobeAnimateSprite2) && adobeAnimateSprite2.clip == "HeadIdle2" && GodotObject.IsInstanceValid(adobeAnimateSprite3) && adobeAnimateSprite3.clip == "HeadIdle3";
			if (!passed)
			{
				failure = $"root={sprite?.clip ?? "<missing>"} head1={adobeAnimateSprite?.clip ?? "<missing>"} head2={adobeAnimateSprite2?.clip ?? "<missing>"} head3={adobeAnimateSprite3?.clip ?? "<missing>"}";
			}
		}
		catch (Exception ex)
		{
			failure = ex.ToString();
		}
		GD.Print(passed ? "VASE_PACKET_CHILD_CLIP_RESULT passed=True" : ("VASE_PACKET_CHILD_CLIP_RESULT passed=False failure=" + failure));
		GetTree().Quit((!passed) ? 2 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
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
