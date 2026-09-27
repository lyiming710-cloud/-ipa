using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/RainMode/RainManager/RainManager.cs")]
public class RainManager : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ApplyMode = "ApplyMode";

		public static readonly StringName Init = "Init";

		public static readonly StringName SetSunDisplay = "SetSunDisplay";

		public static readonly StringName ShowMobileSunBar = "ShowMobileSunBar";

		public static readonly StringName GetMobileSunLabel = "GetMobileSunLabel";

		public static readonly StringName GetPCSunLabel = "GetPCSunLabel";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName pcRainContainer = "pcRainContainer";

		public static readonly StringName pcSunBarTexture = "pcSunBarTexture";

		public static readonly StringName pcSunLabel = "pcSunLabel";

		public static readonly StringName mobileSunBarTexture = "mobileSunBarTexture";

		public static readonly StringName mobileSunLabel = "mobileSunLabel";

		public static readonly StringName isMobileUI = "isMobileUI";

		public static readonly StringName isSunType = "isSunType";

		public static readonly StringName sunNumShow = "sunNumShow";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public HBoxContainer pcRainContainer;

	public TextureRect pcSunBarTexture;

	public Label pcSunLabel;

	public TextureRect mobileSunBarTexture;

	public Label mobileSunLabel;

	public bool isMobileUI;

	public bool isSunType;

	public long sunNumShow;

	public override void _Ready()
	{
		pcRainContainer = GetNode<HBoxContainer>("%PCRainContainer");
		pcSunBarTexture = GetNode<TextureRect>("%PCSunBarTexture");
		pcSunLabel = GetNode<Label>("%PCSunLabel");
		mobileSunBarTexture = GetNode<TextureRect>("%MobileSunBarTexture");
		mobileSunLabel = GetNode<Label>("%MobileSunLabel");
		BattleEventBus.Instance.OnUiSwitched += ApplyMode;
		isMobileUI = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
		ApplyMode(isMobileUI);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (GodotObject.IsInstanceValid(BattleEventBus.Instance))
		{
			BattleEventBus.Instance.OnUiSwitched -= ApplyMode;
		}
	}

	public void ApplyMode(bool mobilePreset)
	{
		isMobileUI = mobilePreset;
		if (mobilePreset)
		{
			pcRainContainer.Visible = false;
			mobileSunBarTexture.Visible = isSunType;
			CustomMinimumSize = new Vector2(0f, CustomMinimumSize.Y);
		}
		else
		{
			pcRainContainer.Visible = true;
			mobileSunBarTexture.Visible = false;
			pcSunBarTexture.Visible = isSunType;
			pcSunLabel.Visible = isSunType;
			CustomMinimumSize = new Vector2(isSunType ? 80 : 0, CustomMinimumSize.Y);
		}
	}

	public void Init(string type, long initialSun)
	{
		if (type == "Sun")
		{
			isSunType = true;
			SetSunDisplay(initialSun);
			if (!isMobileUI)
			{
				pcSunBarTexture.Visible = true;
				pcSunLabel.Visible = true;
				CustomMinimumSize = new Vector2(80f, CustomMinimumSize.Y);
			}
			else
			{
				mobileSunBarTexture.Visible = true;
			}
		}
	}

	public void SetSunDisplay(long sunNum)
	{
		if (isSunType && sunNumShow != sunNum)
		{
			sunNumShow = sunNum;
			if (GodotObject.IsInstanceValid(pcSunLabel))
			{
				pcSunLabel.Text = sunNumShow.ToString();
			}
			if (GodotObject.IsInstanceValid(mobileSunLabel))
			{
				mobileSunLabel.Text = sunNumShow.ToString();
			}
		}
	}

	public void ShowMobileSunBar(bool visible)
	{
		if (GodotObject.IsInstanceValid(mobileSunBarTexture))
		{
			mobileSunBarTexture.Visible = visible;
		}
	}

	public Label GetMobileSunLabel()
	{
		return mobileSunLabel;
	}

	public Label GetPCSunLabel()
	{
		return pcSunLabel;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "mobilePreset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "initialSun", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSunDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowMobileSunBar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMobileSunLabel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPCSunLabel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ApplyMode && args.Count == 1)
		{
			ApplyMode(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 2)
		{
			Init(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSunDisplay && args.Count == 1)
		{
			SetSunDisplay(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowMobileSunBar && args.Count == 1)
		{
			ShowMobileSunBar(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMobileSunLabel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Label>(GetMobileSunLabel());
			return true;
		}
		if (method == MethodName.GetPCSunLabel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Label>(GetPCSunLabel());
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
		if (method == MethodName.ApplyMode)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.SetSunDisplay)
		{
			return true;
		}
		if (method == MethodName.ShowMobileSunBar)
		{
			return true;
		}
		if (method == MethodName.GetMobileSunLabel)
		{
			return true;
		}
		if (method == MethodName.GetPCSunLabel)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.pcRainContainer)
		{
			pcRainContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.pcSunBarTexture)
		{
			pcSunBarTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.pcSunLabel)
		{
			pcSunLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.mobileSunBarTexture)
		{
			mobileSunBarTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.mobileSunLabel)
		{
			mobileSunLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.isMobileUI)
		{
			isMobileUI = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isSunType)
		{
			isSunType = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sunNumShow)
		{
			sunNumShow = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.pcRainContainer)
		{
			value = VariantUtils.CreateFrom(in pcRainContainer);
			return true;
		}
		if (name == PropertyName.pcSunBarTexture)
		{
			value = VariantUtils.CreateFrom(in pcSunBarTexture);
			return true;
		}
		if (name == PropertyName.pcSunLabel)
		{
			value = VariantUtils.CreateFrom(in pcSunLabel);
			return true;
		}
		if (name == PropertyName.mobileSunBarTexture)
		{
			value = VariantUtils.CreateFrom(in mobileSunBarTexture);
			return true;
		}
		if (name == PropertyName.mobileSunLabel)
		{
			value = VariantUtils.CreateFrom(in mobileSunLabel);
			return true;
		}
		if (name == PropertyName.isMobileUI)
		{
			value = VariantUtils.CreateFrom(in isMobileUI);
			return true;
		}
		if (name == PropertyName.isSunType)
		{
			value = VariantUtils.CreateFrom(in isSunType);
			return true;
		}
		if (name == PropertyName.sunNumShow)
		{
			value = VariantUtils.CreateFrom(in sunNumShow);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.pcRainContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcSunBarTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcSunLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileSunBarTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileSunLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isMobileUI, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isSunType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunNumShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.pcRainContainer, Variant.From(in pcRainContainer));
		info.AddProperty(PropertyName.pcSunBarTexture, Variant.From(in pcSunBarTexture));
		info.AddProperty(PropertyName.pcSunLabel, Variant.From(in pcSunLabel));
		info.AddProperty(PropertyName.mobileSunBarTexture, Variant.From(in mobileSunBarTexture));
		info.AddProperty(PropertyName.mobileSunLabel, Variant.From(in mobileSunLabel));
		info.AddProperty(PropertyName.isMobileUI, Variant.From(in isMobileUI));
		info.AddProperty(PropertyName.isSunType, Variant.From(in isSunType));
		info.AddProperty(PropertyName.sunNumShow, Variant.From(in sunNumShow));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.pcRainContainer, out var value))
		{
			pcRainContainer = value.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.pcSunBarTexture, out var value2))
		{
			pcSunBarTexture = value2.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.pcSunLabel, out var value3))
		{
			pcSunLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.mobileSunBarTexture, out var value4))
		{
			mobileSunBarTexture = value4.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.mobileSunLabel, out var value5))
		{
			mobileSunLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.isMobileUI, out var value6))
		{
			isMobileUI = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isSunType, out var value7))
		{
			isSunType = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sunNumShow, out var value8))
		{
			sunNumShow = value8.As<long>();
		}
	}
}
