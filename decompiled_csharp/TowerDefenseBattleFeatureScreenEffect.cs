using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/ScreenEffect/TowerDefenseBattleFeatureScreenEffect.cs")]
public class TowerDefenseBattleFeatureScreenEffect : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName RegisterScreenEffect = "RegisterScreenEffect";

		public static readonly StringName UnregisterScreenEffect = "UnregisterScreenEffect";

		public static readonly StringName AddScreenEffect = "AddScreenEffect";

		public static readonly StringName DeleteScreenEffect = "DeleteScreenEffect";

		public static readonly StringName GetScreenEffect = "GetScreenEffect";

		public static readonly StringName HasScreenEffect = "HasScreenEffect";

		public new static readonly StringName Destroy = "Destroy";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public static readonly StringName SerializeEffects = "SerializeEffects";

		public static readonly StringName RestoreEffects = "RestoreEffects";

		public static readonly StringName EffectsMatch = "EffectsMatch";

		public static readonly StringName ClearScreenEffects = "ClearScreenEffects";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName screenEffectControl = "screenEffectControl";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _screenEffectControlScene;

	private static PackedScene _rainEffectScene;

	private static PackedScene _stormEffectScene;

	public Control screenEffectControl;

	private readonly System.Collections.Generic.Dictionary<string, PackedScene> _effectScenes = new System.Collections.Generic.Dictionary<string, PackedScene>(StringComparer.OrdinalIgnoreCase);

	private readonly System.Collections.Generic.Dictionary<string, Node> _currentEffects = new System.Collections.Generic.Dictionary<string, Node>(StringComparer.OrdinalIgnoreCase);

	private static PackedScene ScreenEffectControlScene => _screenEffectControlScene ?? (_screenEffectControlScene = GD.Load<PackedScene>("uid://cn2idxrgpsjc5"));

	private static PackedScene RainEffectScene => _rainEffectScene ?? (_rainEffectScene = GD.Load<PackedScene>("uid://brduo5wtglu0"));

	private static PackedScene StormEffectScene => _stormEffectScene ?? (_stormEffectScene = GD.Load<PackedScene>("uid://qha0env3257g"));

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		RegisterScreenEffect("Rain", RainEffectScene);
		RegisterScreenEffect("Storm", StormEffectScene);
		if (!GodotObject.IsInstanceValid(ScreenEffectControlScene) || !GodotObject.IsInstanceValid(control))
		{
			GD.PushError("[ScreenEffect] Cannot create the screen-effect container.");
			return;
		}
		screenEffectControl = ScreenEffectControlScene.Instantiate<Control>(PackedScene.GenEditState.Disabled);
		control.AddUI(screenEffectControl, 10);
		bool flag = data.GetValueOrDefault("StormOpen", false).AsBool();
		int num = data.GetValueOrDefault("PacketBankMethod", -1).AsInt32();
		if (flag)
		{
			AddScreenEffect("Rain");
			AddScreenEffect("Storm");
		}
		if (num == 4)
		{
			AddScreenEffect("Rain");
		}
	}

	public bool RegisterScreenEffect(string effectName, PackedScene effectScene)
	{
		if (string.IsNullOrWhiteSpace(effectName) || !GodotObject.IsInstanceValid(effectScene))
		{
			return false;
		}
		_effectScenes[effectName.Trim()] = effectScene;
		return true;
	}

	public bool UnregisterScreenEffect(string effectName)
	{
		if (!string.IsNullOrWhiteSpace(effectName) && !HasScreenEffect(effectName))
		{
			return _effectScenes.Remove(effectName.Trim());
		}
		return false;
	}

	public bool AddScreenEffect(string effectName)
	{
		if (string.IsNullOrWhiteSpace(effectName) || !GodotObject.IsInstanceValid(screenEffectControl))
		{
			return false;
		}
		effectName = effectName.Trim();
		if (_currentEffects.TryGetValue(effectName, out var value))
		{
			if (GodotObject.IsInstanceValid(value))
			{
				return false;
			}
			_currentEffects.Remove(effectName);
		}
		if (!_effectScenes.TryGetValue(effectName, out var value2) || !GodotObject.IsInstanceValid(value2))
		{
			GD.PushWarning("[ScreenEffect] Unknown effect '" + effectName + "'.");
			return false;
		}
		Node node = value2.Instantiate(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(node))
		{
			return false;
		}
		_currentEffects[effectName] = node;
		screenEffectControl.AddChild(node, forceReadableName: false, Node.InternalMode.Disabled);
		return true;
	}

	public void DeleteScreenEffect(string effectName)
	{
		if (!string.IsNullOrWhiteSpace(effectName) && _currentEffects.Remove(effectName.Trim(), out var value) && GodotObject.IsInstanceValid(value))
		{
			value.QueueFree();
		}
	}

	public Node GetScreenEffect(string effectName)
	{
		if (string.IsNullOrWhiteSpace(effectName) || !_currentEffects.TryGetValue(effectName.Trim(), out var value))
		{
			return null;
		}
		if (GodotObject.IsInstanceValid(value))
		{
			return value;
		}
		_currentEffects.Remove(effectName.Trim());
		return null;
	}

	public bool HasScreenEffect(string effectName)
	{
		return GetScreenEffect(effectName) != null;
	}

	public override void Destroy()
	{
		ClearScreenEffects();
		_effectScenes.Clear();
		if (GodotObject.IsInstanceValid(screenEffectControl))
		{
			screenEffectControl.QueueFree();
		}
		screenEffectControl = null;
		base.Destroy();
	}

	public override Dictionary SaveFeature()
	{
		return SerializeEffects();
	}

	public override Dictionary SyncSerialize()
	{
		return SerializeEffects();
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		RestoreEffects(_data, force: false);
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		RestoreEffects(_data, force: true);
	}

	private Dictionary SerializeEffects()
	{
		Array<string> array = new Array<string>();
		foreach (KeyValuePair<string, Node> currentEffect in _currentEffects)
		{
			if (GodotObject.IsInstanceValid(currentEffect.Value))
			{
				array.Add(currentEffect.Key);
			}
		}
		return new Dictionary { ["effects"] = array };
	}

	private void RestoreEffects(Dictionary state, bool force)
	{
		if (!state.TryGetValue("effects", out var value) || value.VariantType != Variant.Type.Array)
		{
			return;
		}
		Godot.Collections.Array array = value.AsGodotArray();
		if (!force && EffectsMatch(array))
		{
			return;
		}
		ClearScreenEffects();
		foreach (Variant item in array)
		{
			AddScreenEffect(item.AsString());
		}
	}

	private bool EffectsMatch(Godot.Collections.Array effects)
	{
		int num = 0;
		foreach (KeyValuePair<string, Node> currentEffect in _currentEffects)
		{
			if (!GodotObject.IsInstanceValid(currentEffect.Value))
			{
				continue;
			}
			num++;
			bool flag = false;
			foreach (Variant effect in effects)
			{
				if (string.Equals(currentEffect.Key, effect.AsString(), StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return num == effects.Count;
	}

	private void ClearScreenEffects()
	{
		foreach (Node value in _currentEffects.Values)
		{
			if (GodotObject.IsInstanceValid(value))
			{
				value.QueueFree();
			}
		}
		_currentEffects.Clear();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterScreenEffect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "effectName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "effectScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.UnregisterScreenEffect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "effectName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddScreenEffect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "effectName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DeleteScreenEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "effectName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetScreenEffect, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "effectName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasScreenEffect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "effectName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SerializeEffects, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreEffects, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "force", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EffectsMatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "effects", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearScreenEffects, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterScreenEffect && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(RegisterScreenEffect(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1])));
			return true;
		}
		if (method == MethodName.UnregisterScreenEffect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(UnregisterScreenEffect(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AddScreenEffect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(AddScreenEffect(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DeleteScreenEffect && args.Count == 1)
		{
			DeleteScreenEffect(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetScreenEffect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(GetScreenEffect(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasScreenEffect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasScreenEffect(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveFeature());
			return true;
		}
		if (method == MethodName.SyncSerialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SyncSerialize());
			return true;
		}
		if (method == MethodName.SyncDeserialize && args.Count == 1)
		{
			SyncDeserialize(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadFeature && args.Count == 2)
		{
			LoadFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SerializeEffects && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SerializeEffects());
			return true;
		}
		if (method == MethodName.RestoreEffects && args.Count == 2)
		{
			RestoreEffects(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EffectsMatch && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(EffectsMatch(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearScreenEffects && args.Count == 0)
		{
			ClearScreenEffects();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.RegisterScreenEffect)
		{
			return true;
		}
		if (method == MethodName.UnregisterScreenEffect)
		{
			return true;
		}
		if (method == MethodName.AddScreenEffect)
		{
			return true;
		}
		if (method == MethodName.DeleteScreenEffect)
		{
			return true;
		}
		if (method == MethodName.GetScreenEffect)
		{
			return true;
		}
		if (method == MethodName.HasScreenEffect)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.SyncSerialize)
		{
			return true;
		}
		if (method == MethodName.SyncDeserialize)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		if (method == MethodName.SerializeEffects)
		{
			return true;
		}
		if (method == MethodName.RestoreEffects)
		{
			return true;
		}
		if (method == MethodName.EffectsMatch)
		{
			return true;
		}
		if (method == MethodName.ClearScreenEffects)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.screenEffectControl)
		{
			screenEffectControl = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.screenEffectControl)
		{
			value = VariantUtils.CreateFrom(in screenEffectControl);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.screenEffectControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.screenEffectControl, Variant.From(in screenEffectControl));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.screenEffectControl, out var value))
		{
			screenEffectControl = value.As<Control>();
		}
	}
}
