using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Glove/GloveManager/GloveManager.cs")]
public class GloveManager : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName OnGloveButtonToggled = "OnGloveButtonToggled";

		public static readonly StringName ApplyMapGloveSpriteScale = "ApplyMapGloveSpriteScale";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName Init = "Init";

		public static readonly StringName GloveButtonPressed = "GloveButtonPressed";

		public static readonly StringName UpdateDisplay = "UpdateDisplay";

		public static readonly StringName UpdateGloveSprite = "UpdateGloveSprite";

		public static readonly StringName ShowMapGloveSprite = "ShowMapGloveSprite";

		public static readonly StringName SetMapGloveSpritePos = "SetMapGloveSpritePos";

		public static readonly StringName SetGloveButtonPressed = "SetGloveButtonPressed";

		public static readonly StringName IsGloveButtonPressed = "IsGloveButtonPressed";

		public static readonly StringName CreatePreviewSprite = "CreatePreviewSprite";

		public static readonly StringName FreePreviewSprite = "FreePreviewSprite";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName gloveButton = "gloveButton";

		public static readonly StringName gloveSprite = "gloveSprite";

		public static readonly StringName gloveShow = "gloveShow";

		public static readonly StringName glovePressedAwait = "glovePressedAwait";

		public static readonly StringName mapControl = "mapControl";

		public static readonly StringName mapFeature = "mapFeature";

		public static readonly StringName mapGloveSprite = "mapGloveSprite";

		public static readonly StringName previewSprite = "previewSprite";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public TextureButton gloveButton;

	public Sprite2D gloveSprite;

	public bool gloveShow;

	public bool glovePressedAwait;

	public TowerDefenseMapControl mapControl;

	public TowerDefenseBattleFeatureMap mapFeature;

	public Sprite2D mapGloveSprite;

	public AdobeAnimateSprite previewSprite;

	public override void _Ready()
	{
		gloveButton = GetNode<TextureButton>("%GloveButton");
		gloveSprite = GetNode<Sprite2D>("%GloveSprite");
		if (GodotObject.IsInstanceValid(gloveButton))
		{
			gloveButton.ActionMode = BaseButton.ActionModeEnum.Press;
			gloveButton.Pressed += GloveButtonPressed;
			gloveButton.Toggled += OnGloveButtonToggled;
		}
		mapGloveSprite = new Sprite2D();
		mapGloveSprite.Visible = false;
		if (GodotObject.IsInstanceValid(mapControl) && GodotObject.IsInstanceValid(mapControl.spriteNode))
		{
			mapControl.spriteNode.AddChild(mapGloveSprite, forceReadableName: false, InternalMode.Disabled);
		}
		if (GodotObject.IsInstanceValid(gloveSprite) && GodotObject.IsInstanceValid(gloveSprite.Texture))
		{
			mapGloveSprite.Texture = gloveSprite.Texture;
			ApplyMapGloveSpriteScale();
		}
		UpdateDisplay();
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(gloveButton))
		{
			gloveButton.Pressed -= GloveButtonPressed;
			gloveButton.Toggled -= OnGloveButtonToggled;
		}
		if (GodotObject.IsInstanceValid(mapGloveSprite))
		{
			mapGloveSprite.QueueFree();
		}
		FreePreviewSprite();
	}

	private void OnGloveButtonToggled(bool _pressed)
	{
		UpdateDisplay();
	}

	private void ApplyMapGloveSpriteScale()
	{
		if (GodotObject.IsInstanceValid(mapGloveSprite) && GodotObject.IsInstanceValid(mapGloveSprite.Texture))
		{
			mapGloveSprite.Scale = Vector2.One * 80f / mapGloveSprite.Texture.GetWidth();
		}
	}

	public override void _Input(InputEvent _event)
	{
		if (GodotObject.IsInstanceValid(gloveButton) && !glovePressedAwait && Input.IsActionJustPressed("Glove") && !TowerDefenseManager.Instance.IsIZMMode() && !TowerDefenseManager.Instance.IsIZM2Mode())
		{
			gloveButton.ButtonPressed = !gloveButton.ButtonPressed;
			TowerDefenseBattleFeatureGlove gloveFeature = TowerDefenseManager.Instance.GetGloveFeature();
			if (GodotObject.IsInstanceValid(gloveFeature))
			{
				gloveFeature.GloveButtonPressed();
			}
		}
	}

	public void Init(TowerDefenseMapControl _mapControl, TowerDefenseBattleFeatureMap _mapFeature)
	{
		mapControl = _mapControl;
		mapFeature = _mapFeature;
		if (GodotObject.IsInstanceValid(mapControl) && GodotObject.IsInstanceValid(mapControl.spriteNode) && GodotObject.IsInstanceValid(mapGloveSprite) && mapGloveSprite.GetParent() == null)
		{
			mapControl.spriteNode.AddChild(mapGloveSprite, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public void GloveButtonPressed()
	{
		TowerDefenseBattleFeatureGlove gloveFeature = TowerDefenseManager.Instance.GetGloveFeature();
		if (GodotObject.IsInstanceValid(gloveFeature))
		{
			gloveFeature.GloveButtonPressed();
		}
	}

	public void UpdateDisplay()
	{
		if (GodotObject.IsInstanceValid(gloveButton) && GodotObject.IsInstanceValid(gloveSprite))
		{
			gloveSprite.Visible = !gloveButton.ButtonPressed;
			if (gloveSprite.Visible && GodotObject.IsInstanceValid(gloveSprite.Texture))
			{
				gloveSprite.Scale = Vector2.One * 80f / gloveSprite.Texture.GetWidth();
			}
		}
	}

	public void UpdateGloveSprite(TowerDefenseMapControl _mapControl)
	{
		if (mapGloveSprite != null)
		{
			mapGloveSprite.Visible = true;
			if (GodotObject.IsInstanceValid(_mapControl) && GodotObject.IsInstanceValid(_mapControl.spriteNode))
			{
				mapGloveSprite.Position = _mapControl.spriteNode.GetLocalMousePosition() - new Vector2(-35f, 35f);
			}
		}
	}

	public void ShowMapGloveSprite(bool show)
	{
		if (GodotObject.IsInstanceValid(mapGloveSprite))
		{
			mapGloveSprite.Visible = show;
		}
	}

	public void SetMapGloveSpritePos(Vector2 pos)
	{
		if (GodotObject.IsInstanceValid(mapGloveSprite))
		{
			mapGloveSprite.Position = pos;
		}
	}

	public void SetGloveButtonPressed(bool pressed)
	{
		if (GodotObject.IsInstanceValid(gloveButton))
		{
			gloveButton.ButtonPressed = pressed;
		}
	}

	public bool IsGloveButtonPressed()
	{
		if (GodotObject.IsInstanceValid(gloveButton))
		{
			return gloveButton.ButtonPressed;
		}
		return false;
	}

	public void CreatePreviewSprite(TowerDefenseCharacter character)
	{
		FreePreviewSprite();
		if (GodotObject.IsInstanceValid(character))
		{
			TowerDefenseCharacterConfig config = character.config;
			previewSprite = TowerDefenseManager.GetCharacterSprite(config.name);
			previewSprite.LightMask = 0;
			Color modulate = previewSprite.Modulate;
			modulate.A = 0.5f;
			previewSprite.Modulate = modulate;
			previewSprite.ZIndex = 1000;
			previewSprite.Position = new Vector2(-100f, -100f);
			Color meshColor = previewSprite.meshColor;
			meshColor.A = 0.5f;
			previewSprite.meshColor = meshColor;
			if (GodotObject.IsInstanceValid(mapControl) && GodotObject.IsInstanceValid(mapControl.spriteNode))
			{
				mapControl.spriteNode.AddChild(previewSprite, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	public void FreePreviewSprite()
	{
		if (GodotObject.IsInstanceValid(previewSprite))
		{
			previewSprite.QueueFree();
		}
		previewSprite = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGloveButtonToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "_pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMapGloveSpriteScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "_mapFeature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GloveButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateGloveSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowMapGloveSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "show", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMapGloveSpritePos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetGloveButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsGloveButtonPressed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreatePreviewSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FreePreviewSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnGloveButtonToggled && args.Count == 1)
		{
			OnGloveButtonToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMapGloveSpriteScale && args.Count == 0)
		{
			ApplyMapGloveSpriteScale();
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 2)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GloveButtonPressed && args.Count == 0)
		{
			GloveButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDisplay && args.Count == 0)
		{
			UpdateDisplay();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateGloveSprite && args.Count == 1)
		{
			UpdateGloveSprite(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowMapGloveSprite && args.Count == 1)
		{
			ShowMapGloveSprite(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMapGloveSpritePos && args.Count == 1)
		{
			SetMapGloveSpritePos(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetGloveButtonPressed && args.Count == 1)
		{
			SetGloveButtonPressed(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsGloveButtonPressed && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsGloveButtonPressed());
			return true;
		}
		if (method == MethodName.CreatePreviewSprite && args.Count == 1)
		{
			CreatePreviewSprite(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FreePreviewSprite && args.Count == 0)
		{
			FreePreviewSprite();
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
		if (method == MethodName.OnGloveButtonToggled)
		{
			return true;
		}
		if (method == MethodName.ApplyMapGloveSpriteScale)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.GloveButtonPressed)
		{
			return true;
		}
		if (method == MethodName.UpdateDisplay)
		{
			return true;
		}
		if (method == MethodName.UpdateGloveSprite)
		{
			return true;
		}
		if (method == MethodName.ShowMapGloveSprite)
		{
			return true;
		}
		if (method == MethodName.SetMapGloveSpritePos)
		{
			return true;
		}
		if (method == MethodName.SetGloveButtonPressed)
		{
			return true;
		}
		if (method == MethodName.IsGloveButtonPressed)
		{
			return true;
		}
		if (method == MethodName.CreatePreviewSprite)
		{
			return true;
		}
		if (method == MethodName.FreePreviewSprite)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.gloveButton)
		{
			gloveButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.gloveSprite)
		{
			gloveSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.gloveShow)
		{
			gloveShow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.glovePressedAwait)
		{
			glovePressedAwait = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mapControl)
		{
			mapControl = VariantUtils.ConvertTo<TowerDefenseMapControl>(in value);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName.mapGloveSprite)
		{
			mapGloveSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.previewSprite)
		{
			previewSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.gloveButton)
		{
			value = VariantUtils.CreateFrom(in gloveButton);
			return true;
		}
		if (name == PropertyName.gloveSprite)
		{
			value = VariantUtils.CreateFrom(in gloveSprite);
			return true;
		}
		if (name == PropertyName.gloveShow)
		{
			value = VariantUtils.CreateFrom(in gloveShow);
			return true;
		}
		if (name == PropertyName.glovePressedAwait)
		{
			value = VariantUtils.CreateFrom(in glovePressedAwait);
			return true;
		}
		if (name == PropertyName.mapControl)
		{
			value = VariantUtils.CreateFrom(in mapControl);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			value = VariantUtils.CreateFrom(in mapFeature);
			return true;
		}
		if (name == PropertyName.mapGloveSprite)
		{
			value = VariantUtils.CreateFrom(in mapGloveSprite);
			return true;
		}
		if (name == PropertyName.previewSprite)
		{
			value = VariantUtils.CreateFrom(in previewSprite);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.gloveButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.gloveSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.gloveShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.glovePressedAwait, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapGloveSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.previewSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.gloveButton, Variant.From(in gloveButton));
		info.AddProperty(PropertyName.gloveSprite, Variant.From(in gloveSprite));
		info.AddProperty(PropertyName.gloveShow, Variant.From(in gloveShow));
		info.AddProperty(PropertyName.glovePressedAwait, Variant.From(in glovePressedAwait));
		info.AddProperty(PropertyName.mapControl, Variant.From(in mapControl));
		info.AddProperty(PropertyName.mapFeature, Variant.From(in mapFeature));
		info.AddProperty(PropertyName.mapGloveSprite, Variant.From(in mapGloveSprite));
		info.AddProperty(PropertyName.previewSprite, Variant.From(in previewSprite));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.gloveButton, out var value))
		{
			gloveButton = value.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.gloveSprite, out var value2))
		{
			gloveSprite = value2.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.gloveShow, out var value3))
		{
			gloveShow = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.glovePressedAwait, out var value4))
		{
			glovePressedAwait = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mapControl, out var value5))
		{
			mapControl = value5.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName.mapFeature, out var value6))
		{
			mapFeature = value6.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName.mapGloveSprite, out var value7))
		{
			mapGloveSprite = value7.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.previewSprite, out var value8))
		{
			previewSprite = value8.As<AdobeAnimateSprite>();
		}
	}
}
