using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter9/PlanternTime/Scene/TowerDefensePlanternTime.cs")]
public class TowerDefensePlanternTime : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName UpdateCooldownEffect = "UpdateCooldownEffect";

		public static readonly StringName ApplyCooldownToPacketShow = "ApplyCooldownToPacketShow";

		public static readonly StringName ClearAllCooldownEffect = "ClearAllCooldownEffect";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _light = "_light";

		public static readonly StringName _planternFog = "_planternFog";

		public static readonly StringName _paused = "_paused";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private PointLight2D _light;

	private GpuParticles2D _planternFog;

	private const double CooldownPercentage = 0.25;

	private static readonly Vector2I[] SURROUND_OFFSETS = new Vector2I[9]
	{
		new Vector2I(-1, -1),
		new Vector2I(0, -1),
		new Vector2I(1, -1),
		new Vector2I(-1, 0),
		new Vector2I(0, 0),
		new Vector2I(1, 0),
		new Vector2I(-1, 1),
		new Vector2I(0, 1),
		new Vector2I(1, 1)
	};

	private readonly HashSet<string> _accelerateKeys = new HashSet<string>();

	private readonly HashSet<string> _decelerateKeys = new HashSet<string>();

	private bool _paused;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_light = GetNode<PointLight2D>("%Light");
			_planternFog = GetNodeOrNull<GpuParticles2D>("%PlanternFog");
			AudioManager.Instance.AudioPlay("Plantern");
		}
	}

	public override void _ExitTree()
	{
		if (!Engine.IsEditorHint())
		{
			ClearAllCooldownEffect();
		}
		base._ExitTree();
	}

	public override void BatchUpdate(double delta)
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		base.BatchUpdate(delta);
		if (GodotObject.IsInstanceValid(_light))
		{
			_light.Visible = TowerDefenseManager.GetMapIsNight() && GameSaveManager.Instance.GetConfigValue("MapEffect").AsBool();
		}
		if (GodotObject.IsInstanceValid(_planternFog) && !_planternFog.Visible)
		{
			_planternFog.Visible = true;
		}
		if (inGame && GodotObject.IsInstanceValid(TowerDefenseManager.Instance) && TowerDefenseManager.Instance.IsGameRunning())
		{
			if (die)
			{
				ClearAllCooldownEffect();
			}
			else
			{
				UpdateCooldownEffect();
			}
		}
	}

	private void UpdateCooldownEffect()
	{
		if (TowerDefenseManager.Instance.backPacket || TowerDefenseManager.Instance.pausePacket)
		{
			if (!_paused)
			{
				ClearAllCooldownEffect();
				_paused = true;
			}
			return;
		}
		_paused = false;
		HashSet<string> hashSet = new HashSet<string>();
		HashSet<string> hashSet2 = new HashSet<string>();
		Vector2I[] sURROUND_OFFSETS = SURROUND_OFFSETS;
		foreach (Vector2I vector2I in sURROUND_OFFSETS)
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos + vector2I);
			if (!GodotObject.IsInstanceValid(mapCell))
			{
				continue;
			}
			foreach (TowerDefenseCharacter character in mapCell.GetCharacterList())
			{
				if (!(character is TowerDefensePlant towerDefensePlant) || character is TowerDefensePlantBowlingBase || towerDefensePlant == this || towerDefensePlant.die)
				{
					continue;
				}
				TowerDefensePacketConfig towerDefensePacketConfig = towerDefensePlant.packet;
				if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
				{
					continue;
				}
				TowerDefenseEnum.PACKET_TYPE pACKET_TYPE = towerDefensePacketConfig._GetType();
				if ((pACKET_TYPE == TowerDefenseEnum.PACKET_TYPE.WHITE || pACKET_TYPE == TowerDefenseEnum.PACKET_TYPE.ORIGINAL) ? true : false)
				{
					string saveKey = towerDefensePacketConfig.saveKey;
					if (towerDefensePacketConfig.GetHypnoses() == instance.hypnoses)
					{
						hashSet.Add(saveKey);
					}
					else
					{
						hashSet2.Add(saveKey);
					}
				}
			}
		}
		UpdateCooldownSet(_accelerateKeys, hashSet, 0.25, "_PTAcc");
		UpdateCooldownSet(_decelerateKeys, hashSet2, -0.25, "_PTDec");
	}

	private void UpdateCooldownSet(HashSet<string> activeKeys, HashSet<string> currentKeys, double percentage, string keySuffix)
	{
		foreach (string currentKey in currentKeys)
		{
			if (activeKeys.Add(currentKey))
			{
				ApplyCooldownToPacketShow(currentKey, percentage, keySuffix);
			}
		}
		activeKeys.RemoveWhere((string saveKey) =>
		{
			if (!currentKeys.Contains(saveKey))
			{
				ApplyCooldownToPacketShow(saveKey, percentage, keySuffix, remove: true);
				return true;
			}
			return false;
		});
	}

	private void ApplyCooldownToPacketShow(string saveKey, double percentage, string keySuffix, bool remove = false)
	{
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return;
		}
		foreach (TowerDefenseInGamePacketShow seedBank in TowerDefenseManager.Instance.GetSeedBankList())
		{
			if (!GodotObject.IsInstanceValid(seedBank?.config) || seedBank.config.saveKey != saveKey)
			{
				continue;
			}
			TowerDefensePacketConfig towerDefensePacketConfig = seedBank.config;
			string key = saveKey + keySuffix;
			if (remove)
			{
				towerDefensePacketConfig.ColdDownDecreaseDelete(this, key);
			}
			else
			{
				towerDefensePacketConfig.ColdDownDecreaseAdd(this, key, percentage);
			}
			if (!seedBank.coldDownOpen)
			{
				break;
			}
			double coldDown = seedBank.coldDown;
			double packetCooldown = towerDefensePacketConfig.GetPacketCooldown();
			if (coldDown > 0.0 && packetCooldown > 0.0)
			{
				double num = packetCooldown / coldDown;
				seedBank.coldDownTimer = Math.Clamp(seedBank.coldDownTimer * num, 0.0, packetCooldown);
				seedBank.coldDown = packetCooldown;
				if (GodotObject.IsInstanceValid(seedBank.coldDownProgressBar))
				{
					seedBank.coldDownProgressBar.MaxValue = packetCooldown;
					seedBank.coldDownProgressBar.Value = seedBank.coldDownTimer;
				}
			}
			break;
		}
	}

	private void ClearAllCooldownEffect()
	{
		foreach (string accelerateKey in _accelerateKeys)
		{
			ApplyCooldownToPacketShow(accelerateKey, 0.25, "_PTAcc", remove: true);
		}
		_accelerateKeys.Clear();
		foreach (string decelerateKey in _decelerateKeys)
		{
			ApplyCooldownToPacketShow(decelerateKey, -0.25, "_PTDec", remove: true);
		}
		_decelerateKeys.Clear();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCooldownEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyCooldownToPacketShow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "saveKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "keySuffix", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "remove", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearAllCooldownEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCooldownEffect && args.Count == 0)
		{
			UpdateCooldownEffect();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCooldownToPacketShow && args.Count == 4)
		{
			ApplyCooldownToPacketShow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearAllCooldownEffect && args.Count == 0)
		{
			ClearAllCooldownEffect();
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.UpdateCooldownEffect)
		{
			return true;
		}
		if (method == MethodName.ApplyCooldownToPacketShow)
		{
			return true;
		}
		if (method == MethodName.ClearAllCooldownEffect)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._light)
		{
			_light = VariantUtils.ConvertTo<PointLight2D>(in value);
			return true;
		}
		if (name == PropertyName._planternFog)
		{
			_planternFog = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._paused)
		{
			_paused = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._light)
		{
			value = VariantUtils.CreateFrom(in _light);
			return true;
		}
		if (name == PropertyName._planternFog)
		{
			value = VariantUtils.CreateFrom(in _planternFog);
			return true;
		}
		if (name == PropertyName._paused)
		{
			value = VariantUtils.CreateFrom(in _paused);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._light, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._planternFog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._paused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._light, Variant.From(in _light));
		info.AddProperty(PropertyName._planternFog, Variant.From(in _planternFog));
		info.AddProperty(PropertyName._paused, Variant.From(in _paused));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._light, out var value))
		{
			_light = value.As<PointLight2D>();
		}
		if (info.TryGetProperty(PropertyName._planternFog, out var value2))
		{
			_planternFog = value2.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._paused, out var value3))
		{
			_paused = value3.As<bool>();
		}
	}
}
