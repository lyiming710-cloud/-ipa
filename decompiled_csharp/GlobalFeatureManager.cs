using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/GlobalFeatureManager/GlobalFeatureManager.cs")]
public class GlobalFeatureManager : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName IsRegistered = "IsRegistered";

		public static readonly StringName GetValue = "GetValue";

		public static readonly StringName IsUnlocked = "IsUnlocked";

		public static readonly StringName Unlock = "Unlock";

		public static readonly StringName Lock = "Lock";

		public static readonly StringName SetValue = "SetValue";

		public static readonly StringName AddValue = "AddValue";

		public static readonly StringName ValidateFeature = "ValidateFeature";

		public static readonly StringName OnFeatureValueChanged = "OnFeatureValueChanged";

		public static readonly StringName OnUserChanged = "OnUserChanged";

		public static readonly StringName MigrateLegacyFeatures = "MigrateLegacyFeatures";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _saveManager = "_saveManager";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string LegacyShovelUnlockLevel = "Level1_5";

	private GameSaveManager _saveManager;

	public static GlobalFeatureManager Instance { get; private set; }

	public event Action<string, int, int> FeatureChanged;

	public event Action FeaturesReloaded;

	public override void _Ready()
	{
		Instance = this;
		_saveManager = GameSaveManager.Instance;
		if (_saveManager == null)
		{
			GD.PushError("GlobalFeatureManager requires GameSaveManager to be loaded first.");
			return;
		}
		_saveManager.FeatureValueChanged += OnFeatureValueChanged;
		_saveManager.UserChanged += OnUserChanged;
		MigrateLegacyFeatures();
	}

	public override void _ExitTree()
	{
		if (_saveManager != null)
		{
			_saveManager.FeatureValueChanged -= OnFeatureValueChanged;
			_saveManager.UserChanged -= OnUserChanged;
		}
		if (Instance == this)
		{
			Instance = null;
		}
		base._ExitTree();
	}

	public bool IsRegistered(string featureId)
	{
		if (!string.IsNullOrWhiteSpace(featureId) && _saveManager != null)
		{
			return _saveManager.HasFeatureDefinition(featureId);
		}
		return false;
	}

	public int GetValue(string featureId)
	{
		if (!ValidateFeature(featureId))
		{
			return 0;
		}
		return _saveManager.GetFeatureValue(featureId);
	}

	public bool IsUnlocked(string featureId)
	{
		return GetValue(featureId) > 0;
	}

	public bool Unlock(string featureId, bool saveImmediately = true)
	{
		if (!ValidateFeature(featureId))
		{
			return false;
		}
		if (_saveManager.GetFeatureValue(featureId) > 0)
		{
			return false;
		}
		return SetValue(featureId, 1, saveImmediately);
	}

	public bool Lock(string featureId, bool saveImmediately = true)
	{
		return SetValue(featureId, 0, saveImmediately);
	}

	public bool SetValue(string featureId, int value, bool saveImmediately = true)
	{
		if (!ValidateFeature(featureId))
		{
			return false;
		}
		if (_saveManager.GetFeatureValue(featureId) == value)
		{
			return false;
		}
		_saveManager.SetFeatureValue(featureId, value);
		if (saveImmediately)
		{
			_saveManager.Save();
		}
		return true;
	}

	public bool AddValue(string featureId, int amount, bool saveImmediately = true)
	{
		if (!ValidateFeature(featureId))
		{
			return false;
		}
		return SetValue(featureId, _saveManager.GetFeatureValue(featureId) + amount, saveImmediately);
	}

	private bool ValidateFeature(string featureId)
	{
		if (_saveManager == null || string.IsNullOrWhiteSpace(featureId))
		{
			return false;
		}
		if (_saveManager.GetUserCurrent() == "")
		{
			return false;
		}
		if (_saveManager.HasFeatureDefinition(featureId))
		{
			return true;
		}
		GD.PushError("Global feature '" + featureId + "' is not registered in FeatureInit.json.");
		return false;
	}

	private void OnFeatureValueChanged(string featureId, int oldValue, int newValue)
	{
		FeatureChanged?.Invoke(featureId, oldValue, newValue);
	}

	private void OnUserChanged(string _user)
	{
		MigrateLegacyFeatures();
		FeaturesReloaded?.Invoke();
	}

	private void MigrateLegacyFeatures()
	{
		if (_saveManager != null && !(_saveManager.GetUserCurrent() == "") && !IsUnlocked("Shovel") && _saveManager.GetLevelValue("Level1_5").GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary()
			.GetValueOrDefault("Finish", 0)
			.AsInt32() > 0)
		{
			Unlock("Shovel");
		}
	}

	public GlobalFeatureManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/GlobalFeatureManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsRegistered, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsUnlocked, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Unlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "saveImmediately", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Lock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "saveImmediately", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetValue, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "saveImmediately", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddValue, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "amount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "saveImmediately", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ValidateFeature, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnFeatureValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "oldValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnUserChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_user", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MigrateLegacyFeatures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.IsRegistered && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRegistered(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsUnlocked && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsUnlocked(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Unlock && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(Unlock(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.Lock && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(Lock(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.SetValue && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(SetValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.AddValue && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(AddValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.ValidateFeature && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ValidateFeature(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnFeatureValueChanged && args.Count == 3)
		{
			OnFeatureValueChanged(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnUserChanged && args.Count == 1)
		{
			OnUserChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MigrateLegacyFeatures && args.Count == 0)
		{
			MigrateLegacyFeatures();
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
		if (method == MethodName.IsRegistered)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.IsUnlocked)
		{
			return true;
		}
		if (method == MethodName.Unlock)
		{
			return true;
		}
		if (method == MethodName.Lock)
		{
			return true;
		}
		if (method == MethodName.SetValue)
		{
			return true;
		}
		if (method == MethodName.AddValue)
		{
			return true;
		}
		if (method == MethodName.ValidateFeature)
		{
			return true;
		}
		if (method == MethodName.OnFeatureValueChanged)
		{
			return true;
		}
		if (method == MethodName.OnUserChanged)
		{
			return true;
		}
		if (method == MethodName.MigrateLegacyFeatures)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._saveManager)
		{
			_saveManager = VariantUtils.ConvertTo<GameSaveManager>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._saveManager)
		{
			value = VariantUtils.CreateFrom(in _saveManager);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._saveManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._saveManager, Variant.From(in _saveManager));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._saveManager, out var value))
		{
			_saveManager = value.As<GameSaveManager>();
		}
	}
}
