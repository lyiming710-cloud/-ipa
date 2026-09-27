using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/DialogBox/DailyChallenge/Award/Item/DailyChallengeAwardItem.cs")]
public class DailyChallengeAwardItem : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName ConfigurePreviewRender = "ConfigurePreviewRender";

		public static readonly StringName GetButtonPressed = "GetButtonPressed";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName spriteNode = "spriteNode";

		public static readonly StringName accessLabel = "accessLabel";

		public static readonly StringName getButton = "getButton";

		public static readonly StringName data = "data";

		public static readonly StringName finish = "finish";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public Control spriteNode;

	public Label accessLabel;

	public NinePatchButtonBase getButton;

	public Dictionary data;

	public int finish;

	public override void _Ready()
	{
		spriteNode = GetNode<Control>("%SpriteNode");
		accessLabel = GetNode<Label>("%AccessLabel");
		getButton = GetNode<NinePatchButtonBase>("%GetButton");
		getButton.OnPressed += GetButtonPressed;
	}

	public void Init(Dictionary _data, int _finish)
	{
		data = _data;
		finish = _finish;
		TowerDefenseCharacterConfig characterConfig = TowerDefenseManager.GetPacketConfig((string)data["ShowCharacter"]).characterConfig;
		AdobeAnimateSprite characterSprite = TowerDefenseManager.GetCharacterSprite((string)data["ShowCharacter"]);
		characterConfig.customData.SetCustomFliters(characterSprite, data["ShowCustom"].AsString());
		ScrollContainer scrollContainer = GetParent()?.GetParent<ScrollContainer>();
		if (GodotObject.IsInstanceValid(scrollContainer))
		{
			scrollContainer.ClipContents = true;
			ConfigurePreviewRender(characterSprite, scrollContainer);
		}
		spriteNode.AddChild(characterSprite, forceReadableName: false, InternalMode.Disabled);
		Array array = (Array)data["ConditionArg"];
		accessLabel.Text = $"累计完成{(int)array[0]}天挑战获得";
		if ((int)array[0] > finish)
		{
			getButton.disable = true;
			getButton.text = "无法领取";
		}
		else if (GameSaveManager.Instance.GetFeatureValue((string)data["Key"]) != 0)
		{
			getButton.disable = true;
			getButton.text = "已领取";
		}
	}

	private static void ConfigurePreviewRender(Node node, Control previewClip)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.LightMask = 0;
			adobeAnimateSprite.forceLocalRender = true;
			adobeAnimateSprite.SetRenderClipControl(previewClip);
			adobeAnimateSprite.ProcessMode = ProcessModeEnum.Always;
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			ConfigurePreviewRender(child, previewClip);
		}
	}

	public void GetButtonPressed()
	{
		GameSaveManager.Instance.SetFeatureValue((string)data["Key"], true);
		GameSaveManager.Instance.Save();
		getButton.disable = true;
		getButton.text = "已领取";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_finish", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigurePreviewRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "previewClip", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Init && args.Count == 2)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigurePreviewRender && args.Count == 2)
		{
			ConfigurePreviewRender(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetButtonPressed && args.Count == 0)
		{
			GetButtonPressed();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ConfigurePreviewRender && args.Count == 2)
		{
			ConfigurePreviewRender(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]));
			ret = default;
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.ConfigurePreviewRender)
		{
			return true;
		}
		if (method == MethodName.GetButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.spriteNode)
		{
			spriteNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.accessLabel)
		{
			accessLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.getButton)
		{
			getButton = VariantUtils.ConvertTo<NinePatchButtonBase>(in value);
			return true;
		}
		if (name == PropertyName.data)
		{
			data = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.finish)
		{
			finish = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.spriteNode)
		{
			value = VariantUtils.CreateFrom(in spriteNode);
			return true;
		}
		if (name == PropertyName.accessLabel)
		{
			value = VariantUtils.CreateFrom(in accessLabel);
			return true;
		}
		if (name == PropertyName.getButton)
		{
			value = VariantUtils.CreateFrom(in getButton);
			return true;
		}
		if (name == PropertyName.data)
		{
			value = VariantUtils.CreateFrom(in data);
			return true;
		}
		if (name == PropertyName.finish)
		{
			value = VariantUtils.CreateFrom(in finish);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.spriteNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.accessLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.getButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.finish, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.spriteNode, Variant.From(in spriteNode));
		info.AddProperty(PropertyName.accessLabel, Variant.From(in accessLabel));
		info.AddProperty(PropertyName.getButton, Variant.From(in getButton));
		info.AddProperty(PropertyName.data, Variant.From(in data));
		info.AddProperty(PropertyName.finish, Variant.From(in finish));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.spriteNode, out var value))
		{
			spriteNode = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.accessLabel, out var value2))
		{
			accessLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.getButton, out var value3))
		{
			getButton = value3.As<NinePatchButtonBase>();
		}
		if (info.TryGetProperty(PropertyName.data, out var value4))
		{
			data = value4.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.finish, out var value5))
		{
			finish = value5.As<int>();
		}
	}
}
