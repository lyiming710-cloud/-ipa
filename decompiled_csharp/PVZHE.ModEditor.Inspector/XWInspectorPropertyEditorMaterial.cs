using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/ResourceInspectorExtend/Material/XWInspectorPropertyEditorMaterial.cs")]
public class XWInspectorPropertyEditorMaterial : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName SetupOptions = "SetupOptions";

		public static readonly StringName RefreshMaterial = "RefreshMaterial";

		public static readonly StringName ReadMaterial = "ReadMaterial";

		public static readonly StringName OnDrawMaterialPreview = "OnDrawMaterialPreview";

		public static readonly StringName OnShaderChanged = "OnShaderChanged";

		public static readonly StringName PopulateShaderParamTree = "PopulateShaderParamTree";

		public static readonly StringName ReapplySelectedShaderParameter = "ReapplySelectedShaderParameter";

		public static readonly StringName OnBlendModeSelected = "OnBlendModeSelected";

		public static readonly StringName OnLightModeSelected = "OnLightModeSelected";

		public static readonly StringName OnParticlesAnimationToggled = "OnParticlesAnimationToggled";

		public static readonly StringName OnParticlesAnimLoopToggled = "OnParticlesAnimLoopToggled";

		public static readonly StringName OnParticlesHFramesChanged = "OnParticlesHFramesChanged";

		public static readonly StringName OnParticlesVFramesChanged = "OnParticlesVFramesChanged";

		public static readonly StringName OnAlbedoColorChanged = "OnAlbedoColorChanged";

		public static readonly StringName OnMetallicChanged = "OnMetallicChanged";

		public static readonly StringName OnRoughnessChanged = "OnRoughnessChanged";

		public static readonly StringName CommitMaterialChange = "CommitMaterialChange";

		public static readonly StringName GetItemIndexById = "GetItemIndexById";

		public static readonly StringName FormatVariant = "FormatVariant";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _materialPreview = "_materialPreview";

		public static readonly StringName _shaderControls = "_shaderControls";

		public static readonly StringName _canvasControls = "_canvasControls";

		public static readonly StringName _base3DControls = "_base3DControls";

		public static readonly StringName _shaderPicker = "_shaderPicker";

		public static readonly StringName _shaderParamTree = "_shaderParamTree";

		public static readonly StringName _blendModeOption = "_blendModeOption";

		public static readonly StringName _lightModeOption = "_lightModeOption";

		public static readonly StringName _particlesAnimationCheckBox = "_particlesAnimationCheckBox";

		public static readonly StringName _particlesAnimLoopCheckBox = "_particlesAnimLoopCheckBox";

		public static readonly StringName _particlesHFramesSpinBox = "_particlesHFramesSpinBox";

		public static readonly StringName _particlesVFramesSpinBox = "_particlesVFramesSpinBox";

		public static readonly StringName _albedoColorPicker = "_albedoColorPicker";

		public static readonly StringName _metallicSpinBox = "_metallicSpinBox";

		public static readonly StringName _roughnessSpinBox = "_roughnessSpinBox";

		public static readonly StringName _material = "_material";

		public static readonly StringName _updating = "_updating";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private Control _materialPreview;

	private VBoxContainer _shaderControls;

	private VBoxContainer _canvasControls;

	private VBoxContainer _base3DControls;

	private XWResourcePicker _shaderPicker;

	private Tree _shaderParamTree;

	private OptionButton _blendModeOption;

	private OptionButton _lightModeOption;

	private CheckBox _particlesAnimationCheckBox;

	private CheckBox _particlesAnimLoopCheckBox;

	private SpinBox _particlesHFramesSpinBox;

	private SpinBox _particlesVFramesSpinBox;

	private ColorPickerButton _albedoColorPicker;

	private SpinBox _metallicSpinBox;

	private SpinBox _roughnessSpinBox;

	private Material _material;

	private bool _updating;

	public override void _Ready()
	{
		base._Ready();
		_materialPreview = GetNode<Control>("%MaterialPreview");
		_shaderControls = GetNode<VBoxContainer>("%ShaderControls");
		_canvasControls = GetNode<VBoxContainer>("%CanvasControls");
		_base3DControls = GetNode<VBoxContainer>("%Base3DControls");
		_shaderPicker = GetNode<XWResourcePicker>("%ShaderPicker");
		_shaderParamTree = GetNode<Tree>("%ShaderParamTree");
		_blendModeOption = GetNode<OptionButton>("%BlendModeOption");
		_lightModeOption = GetNode<OptionButton>("%LightModeOption");
		_particlesAnimationCheckBox = GetNode<CheckBox>("%ParticlesAnimationCheckBox");
		_particlesAnimLoopCheckBox = GetNode<CheckBox>("%ParticlesAnimLoopCheckBox");
		_particlesHFramesSpinBox = GetNode<SpinBox>("%ParticlesHFramesSpinBox");
		_particlesVFramesSpinBox = GetNode<SpinBox>("%ParticlesVFramesSpinBox");
		_albedoColorPicker = GetNode<ColorPickerButton>("%AlbedoColorPicker");
		_metallicSpinBox = GetNode<SpinBox>("%MetallicSpinBox");
		_roughnessSpinBox = GetNode<SpinBox>("%RoughnessSpinBox");
		SetupOptions();
		_shaderPicker.Setup("Shader");
		_shaderParamTree.SetColumnTitle(0, "参数");
		_shaderParamTree.SetColumnTitle(1, "类型");
		_shaderParamTree.SetColumnTitle(2, "值");
		_materialPreview.Draw += OnDrawMaterialPreview;
		_shaderPicker.ResourceChanged += OnShaderChanged;
		_shaderParamTree.ItemActivated += ReapplySelectedShaderParameter;
		_blendModeOption.ItemSelected += OnBlendModeSelected;
		_lightModeOption.ItemSelected += OnLightModeSelected;
		_particlesAnimationCheckBox.Toggled += OnParticlesAnimationToggled;
		_particlesAnimLoopCheckBox.Toggled += OnParticlesAnimLoopToggled;
		_particlesHFramesSpinBox.ValueChanged += OnParticlesHFramesChanged;
		_particlesVFramesSpinBox.ValueChanged += OnParticlesVFramesChanged;
		_albedoColorPicker.ColorChanged += OnAlbedoColorChanged;
		_metallicSpinBox.ValueChanged += OnMetallicChanged;
		_roughnessSpinBox.ValueChanged += OnRoughnessChanged;
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		base.SetEditProperty(property, field);
		RefreshMaterial();
	}

	public override void UpdateValue()
	{
		RefreshMaterial();
	}

	public override Variant GetValue()
	{
		return _material;
	}

	private void SetupOptions()
	{
		_blendModeOption.Clear();
		_blendModeOption.AddItem("混合", 0);
		_blendModeOption.AddItem("叠加", 1);
		_blendModeOption.AddItem("相减", 2);
		_blendModeOption.AddItem("相乘", 3);
		_blendModeOption.AddItem("预乘 Alpha", 4);
		_lightModeOption.Clear();
		_lightModeOption.AddItem("正常", 0);
		_lightModeOption.AddItem("无光照", 1);
		_lightModeOption.AddItem("仅光照", 2);
	}

	private void RefreshMaterial()
	{
		_material = ReadMaterial();
		_updating = true;
		bool visible = _material is ShaderMaterial;
		bool visible2 = _material is CanvasItemMaterial;
		bool visible3 = _material is BaseMaterial3D;
		_shaderControls.Visible = visible;
		_canvasControls.Visible = visible2;
		_base3DControls.Visible = visible3;
		if (_material is ShaderMaterial shaderMaterial)
		{
			_shaderPicker.SetEditedResource(shaderMaterial.Shader);
			PopulateShaderParamTree(shaderMaterial);
		}
		else
		{
			_shaderPicker.SetEditedResource(null);
			_shaderParamTree.Clear();
		}
		if (_material is CanvasItemMaterial canvasItemMaterial)
		{
			_blendModeOption.Select(GetItemIndexById(_blendModeOption, (int)canvasItemMaterial.BlendMode));
			_lightModeOption.Select(GetItemIndexById(_lightModeOption, (int)canvasItemMaterial.LightMode));
			_particlesAnimationCheckBox.ButtonPressed = canvasItemMaterial.ParticlesAnimation;
			_particlesAnimLoopCheckBox.ButtonPressed = canvasItemMaterial.ParticlesAnimLoop;
			_particlesHFramesSpinBox.Value = canvasItemMaterial.ParticlesAnimHFrames;
			_particlesVFramesSpinBox.Value = canvasItemMaterial.ParticlesAnimVFrames;
		}
		if (_material is BaseMaterial3D baseMaterial3D)
		{
			_albedoColorPicker.Color = baseMaterial3D.AlbedoColor;
			_metallicSpinBox.Value = baseMaterial3D.Metallic;
			_roughnessSpinBox.Value = baseMaterial3D.Roughness;
		}
		_updating = false;
		_materialPreview.QueueRedraw();
	}

	private Material ReadMaterial()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Object || !(propertyValue.As<GodotObject>() is Material result))
		{
			return null;
		}
		return result;
	}

	private void OnDrawMaterialPreview()
	{
		Rect2 rect = new Rect2(Vector2.Zero, _materialPreview.Size);
		_materialPreview.DrawRect(rect, new Color(0.075f, 0.075f, 0.075f));
		Color color = new Color(0.35f, 0.5f, 0.95f);
		if (_material is BaseMaterial3D baseMaterial3D)
		{
			color = baseMaterial3D.AlbedoColor;
		}
		else if (_material is ShaderMaterial)
		{
			color = new Color(0.58f, 0.42f, 0.94f);
		}
		else if (_material is CanvasItemMaterial canvasItemMaterial)
		{
			color = ((canvasItemMaterial.BlendMode == CanvasItemMaterial.BlendModeEnum.Add) ? new Color(0.95f, 0.7f, 0.22f) : new Color(0.32f, 0.62f, 0.92f));
		}
		Rect2 rect2 = rect.Grow(-10f);
		_materialPreview.DrawRect(rect2, color);
		_materialPreview.DrawRect(rect2, new Color(1f, 1f, 1f, 0.18f), filled: false, 1f);
	}

	private void OnShaderChanged(Resource resource)
	{
		if (!_updating && _material is ShaderMaterial shaderMaterial)
		{
			shaderMaterial.Shader = resource as Shader;
			PopulateShaderParamTree(shaderMaterial);
			CommitMaterialChange();
		}
	}

	private void PopulateShaderParamTree(ShaderMaterial shaderMaterial)
	{
		_shaderParamTree.Clear();
		TreeItem parent = _shaderParamTree.CreateItem();
		if (!GodotObject.IsInstanceValid(shaderMaterial?.Shader))
		{
			return;
		}
		foreach (Variant shaderUniform in shaderMaterial.Shader.GetShaderUniformList())
		{
			Dictionary dictionary = (Dictionary)shaderUniform;
			if (dictionary.TryGetValue("name", out var value))
			{
				string text = value.AsString();
				if (!string.IsNullOrWhiteSpace(text))
				{
					TreeItem treeItem = _shaderParamTree.CreateItem(parent);
					treeItem.SetText(0, text);
					treeItem.SetText(1, dictionary.TryGetValue("type", out var value2) ? ((Variant.Type)value2.AsInt32()/*cast due to constrained. prefix*/).ToString() : "");
					Variant shaderParameter = shaderMaterial.GetShaderParameter(new StringName(text));
					treeItem.SetText(2, FormatVariant(shaderParameter));
					treeItem.SetMetadata(0, text);
				}
			}
		}
	}

	private void ReapplySelectedShaderParameter()
	{
		if (!(_material is ShaderMaterial shaderMaterial))
		{
			return;
		}
		TreeItem selected = _shaderParamTree.GetSelected();
		if (selected != null)
		{
			string text = selected.GetMetadata(0).AsString();
			if (!string.IsNullOrWhiteSpace(text))
			{
				Variant shaderParameter = shaderMaterial.GetShaderParameter(new StringName(text));
				shaderMaterial.SetShaderParameter(new StringName(text), shaderParameter);
				CommitMaterialChange();
			}
		}
	}

	private void OnBlendModeSelected(long index)
	{
		if (!_updating && _material is CanvasItemMaterial canvasItemMaterial)
		{
			canvasItemMaterial.BlendMode = (CanvasItemMaterial.BlendModeEnum)_blendModeOption.GetItemId((int)index);
			CommitMaterialChange();
		}
	}

	private void OnLightModeSelected(long index)
	{
		if (!_updating && _material is CanvasItemMaterial canvasItemMaterial)
		{
			canvasItemMaterial.LightMode = (CanvasItemMaterial.LightModeEnum)_lightModeOption.GetItemId((int)index);
			CommitMaterialChange();
		}
	}

	private void OnParticlesAnimationToggled(bool toggled)
	{
		if (!_updating && _material is CanvasItemMaterial canvasItemMaterial)
		{
			canvasItemMaterial.ParticlesAnimation = toggled;
			CommitMaterialChange();
		}
	}

	private void OnParticlesAnimLoopToggled(bool toggled)
	{
		if (!_updating && _material is CanvasItemMaterial canvasItemMaterial)
		{
			canvasItemMaterial.ParticlesAnimLoop = toggled;
			CommitMaterialChange();
		}
	}

	private void OnParticlesHFramesChanged(double value)
	{
		if (!_updating && _material is CanvasItemMaterial canvasItemMaterial)
		{
			canvasItemMaterial.ParticlesAnimHFrames = Mathf.Max(1, (int)value);
			CommitMaterialChange();
		}
	}

	private void OnParticlesVFramesChanged(double value)
	{
		if (!_updating && _material is CanvasItemMaterial canvasItemMaterial)
		{
			canvasItemMaterial.ParticlesAnimVFrames = Mathf.Max(1, (int)value);
			CommitMaterialChange();
		}
	}

	private void OnAlbedoColorChanged(Color color)
	{
		if (!_updating && _material is BaseMaterial3D baseMaterial3D)
		{
			baseMaterial3D.AlbedoColor = color;
			CommitMaterialChange();
		}
	}

	private void OnMetallicChanged(double value)
	{
		if (!_updating && _material is BaseMaterial3D baseMaterial3D)
		{
			baseMaterial3D.Metallic = Mathf.Clamp((float)value, 0f, 1f);
			CommitMaterialChange();
		}
	}

	private void OnRoughnessChanged(double value)
	{
		if (!_updating && _material is BaseMaterial3D baseMaterial3D)
		{
			baseMaterial3D.Roughness = Mathf.Clamp((float)value, 0f, 1f);
			CommitMaterialChange();
		}
	}

	private void CommitMaterialChange()
	{
		if (GodotObject.IsInstanceValid(_material))
		{
			_material.EmitChanged();
			if (GodotObject.IsInstanceValid(Property) && Property.PropName != (StringName)"")
			{
				ValueChange(_material);
			}
			else
			{
				Property?.SetCall();
			}
			_materialPreview.QueueRedraw();
		}
	}

	private static int GetItemIndexById(OptionButton option, int id)
	{
		for (int i = 0; i < option.ItemCount; i++)
		{
			if (option.GetItemId(i) == id)
			{
				return i;
			}
		}
		return 0;
	}

	private static string FormatVariant(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		if ((ulong)variantType <= 3uL)
		{
			switch ((int)variantType)
			{
			case 0:
				return "空";
			case 1:
				return value.AsBool() ? "true" : "false";
			case 2:
				return value.AsInt64().ToString();
			case 3:
				return value.AsDouble().ToString("0.###");
			}
		}
		return variantType switch
		{
			Variant.Type.Color => value.AsColor().ToHtml(), 
			Variant.Type.Object => value.AsGodotObject()?.GetClass() ?? "空", 
			_ => value.ToString(), 
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(23)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupOptions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshMaterial, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadMaterial, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Material"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDrawMaterialPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnShaderChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateShaderParamTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shaderMaterial", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ShaderMaterial"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReapplySelectedShaderParameter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnBlendModeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnLightModeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnParticlesAnimationToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnParticlesAnimLoopToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnParticlesHFramesChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnParticlesVFramesChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAlbedoColorChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnMetallicChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnRoughnessChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitMaterialChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetItemIndexById, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVariant, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
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
		if (method == MethodName.SetEditProperty && args.Count == 2)
		{
			SetEditProperty(VariantUtils.ConvertTo<XWInspectorProperty>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateValue && args.Count == 0)
		{
			UpdateValue();
			ret = default;
			return true;
		}
		if (method == MethodName.GetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetValue());
			return true;
		}
		if (method == MethodName.SetupOptions && args.Count == 0)
		{
			SetupOptions();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshMaterial && args.Count == 0)
		{
			RefreshMaterial();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadMaterial && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Material>(ReadMaterial());
			return true;
		}
		if (method == MethodName.OnDrawMaterialPreview && args.Count == 0)
		{
			OnDrawMaterialPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.OnShaderChanged && args.Count == 1)
		{
			OnShaderChanged(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateShaderParamTree && args.Count == 1)
		{
			PopulateShaderParamTree(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReapplySelectedShaderParameter && args.Count == 0)
		{
			ReapplySelectedShaderParameter();
			ret = default;
			return true;
		}
		if (method == MethodName.OnBlendModeSelected && args.Count == 1)
		{
			OnBlendModeSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnLightModeSelected && args.Count == 1)
		{
			OnLightModeSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnParticlesAnimationToggled && args.Count == 1)
		{
			OnParticlesAnimationToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnParticlesAnimLoopToggled && args.Count == 1)
		{
			OnParticlesAnimLoopToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnParticlesHFramesChanged && args.Count == 1)
		{
			OnParticlesHFramesChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnParticlesVFramesChanged && args.Count == 1)
		{
			OnParticlesVFramesChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAlbedoColorChanged && args.Count == 1)
		{
			OnAlbedoColorChanged(VariantUtils.ConvertTo<Color>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnMetallicChanged && args.Count == 1)
		{
			OnMetallicChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnRoughnessChanged && args.Count == 1)
		{
			OnRoughnessChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitMaterialChange && args.Count == 0)
		{
			CommitMaterialChange();
			ret = default;
			return true;
		}
		if (method == MethodName.GetItemIndexById && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetItemIndexById(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetItemIndexById && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetItemIndexById(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
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
		if (method == MethodName.SetEditProperty)
		{
			return true;
		}
		if (method == MethodName.UpdateValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.SetupOptions)
		{
			return true;
		}
		if (method == MethodName.RefreshMaterial)
		{
			return true;
		}
		if (method == MethodName.ReadMaterial)
		{
			return true;
		}
		if (method == MethodName.OnDrawMaterialPreview)
		{
			return true;
		}
		if (method == MethodName.OnShaderChanged)
		{
			return true;
		}
		if (method == MethodName.PopulateShaderParamTree)
		{
			return true;
		}
		if (method == MethodName.ReapplySelectedShaderParameter)
		{
			return true;
		}
		if (method == MethodName.OnBlendModeSelected)
		{
			return true;
		}
		if (method == MethodName.OnLightModeSelected)
		{
			return true;
		}
		if (method == MethodName.OnParticlesAnimationToggled)
		{
			return true;
		}
		if (method == MethodName.OnParticlesAnimLoopToggled)
		{
			return true;
		}
		if (method == MethodName.OnParticlesHFramesChanged)
		{
			return true;
		}
		if (method == MethodName.OnParticlesVFramesChanged)
		{
			return true;
		}
		if (method == MethodName.OnAlbedoColorChanged)
		{
			return true;
		}
		if (method == MethodName.OnMetallicChanged)
		{
			return true;
		}
		if (method == MethodName.OnRoughnessChanged)
		{
			return true;
		}
		if (method == MethodName.CommitMaterialChange)
		{
			return true;
		}
		if (method == MethodName.GetItemIndexById)
		{
			return true;
		}
		if (method == MethodName.FormatVariant)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._materialPreview)
		{
			_materialPreview = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._shaderControls)
		{
			_shaderControls = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._canvasControls)
		{
			_canvasControls = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._base3DControls)
		{
			_base3DControls = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._shaderPicker)
		{
			_shaderPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._shaderParamTree)
		{
			_shaderParamTree = VariantUtils.ConvertTo<Tree>(in value);
			return true;
		}
		if (name == PropertyName._blendModeOption)
		{
			_blendModeOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._lightModeOption)
		{
			_lightModeOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._particlesAnimationCheckBox)
		{
			_particlesAnimationCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._particlesAnimLoopCheckBox)
		{
			_particlesAnimLoopCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._particlesHFramesSpinBox)
		{
			_particlesHFramesSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._particlesVFramesSpinBox)
		{
			_particlesVFramesSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._albedoColorPicker)
		{
			_albedoColorPicker = VariantUtils.ConvertTo<ColorPickerButton>(in value);
			return true;
		}
		if (name == PropertyName._metallicSpinBox)
		{
			_metallicSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._roughnessSpinBox)
		{
			_roughnessSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._material)
		{
			_material = VariantUtils.ConvertTo<Material>(in value);
			return true;
		}
		if (name == PropertyName._updating)
		{
			_updating = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._materialPreview)
		{
			value = VariantUtils.CreateFrom(in _materialPreview);
			return true;
		}
		if (name == PropertyName._shaderControls)
		{
			value = VariantUtils.CreateFrom(in _shaderControls);
			return true;
		}
		if (name == PropertyName._canvasControls)
		{
			value = VariantUtils.CreateFrom(in _canvasControls);
			return true;
		}
		if (name == PropertyName._base3DControls)
		{
			value = VariantUtils.CreateFrom(in _base3DControls);
			return true;
		}
		if (name == PropertyName._shaderPicker)
		{
			value = VariantUtils.CreateFrom(in _shaderPicker);
			return true;
		}
		if (name == PropertyName._shaderParamTree)
		{
			value = VariantUtils.CreateFrom(in _shaderParamTree);
			return true;
		}
		if (name == PropertyName._blendModeOption)
		{
			value = VariantUtils.CreateFrom(in _blendModeOption);
			return true;
		}
		if (name == PropertyName._lightModeOption)
		{
			value = VariantUtils.CreateFrom(in _lightModeOption);
			return true;
		}
		if (name == PropertyName._particlesAnimationCheckBox)
		{
			value = VariantUtils.CreateFrom(in _particlesAnimationCheckBox);
			return true;
		}
		if (name == PropertyName._particlesAnimLoopCheckBox)
		{
			value = VariantUtils.CreateFrom(in _particlesAnimLoopCheckBox);
			return true;
		}
		if (name == PropertyName._particlesHFramesSpinBox)
		{
			value = VariantUtils.CreateFrom(in _particlesHFramesSpinBox);
			return true;
		}
		if (name == PropertyName._particlesVFramesSpinBox)
		{
			value = VariantUtils.CreateFrom(in _particlesVFramesSpinBox);
			return true;
		}
		if (name == PropertyName._albedoColorPicker)
		{
			value = VariantUtils.CreateFrom(in _albedoColorPicker);
			return true;
		}
		if (name == PropertyName._metallicSpinBox)
		{
			value = VariantUtils.CreateFrom(in _metallicSpinBox);
			return true;
		}
		if (name == PropertyName._roughnessSpinBox)
		{
			value = VariantUtils.CreateFrom(in _roughnessSpinBox);
			return true;
		}
		if (name == PropertyName._material)
		{
			value = VariantUtils.CreateFrom(in _material);
			return true;
		}
		if (name == PropertyName._updating)
		{
			value = VariantUtils.CreateFrom(in _updating);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._materialPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shaderControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._canvasControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._base3DControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shaderPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shaderParamTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._blendModeOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lightModeOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._particlesAnimationCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._particlesAnimLoopCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._particlesHFramesSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._particlesVFramesSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._albedoColorPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._metallicSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._roughnessSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._material, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._materialPreview, Variant.From(in _materialPreview));
		info.AddProperty(PropertyName._shaderControls, Variant.From(in _shaderControls));
		info.AddProperty(PropertyName._canvasControls, Variant.From(in _canvasControls));
		info.AddProperty(PropertyName._base3DControls, Variant.From(in _base3DControls));
		info.AddProperty(PropertyName._shaderPicker, Variant.From(in _shaderPicker));
		info.AddProperty(PropertyName._shaderParamTree, Variant.From(in _shaderParamTree));
		info.AddProperty(PropertyName._blendModeOption, Variant.From(in _blendModeOption));
		info.AddProperty(PropertyName._lightModeOption, Variant.From(in _lightModeOption));
		info.AddProperty(PropertyName._particlesAnimationCheckBox, Variant.From(in _particlesAnimationCheckBox));
		info.AddProperty(PropertyName._particlesAnimLoopCheckBox, Variant.From(in _particlesAnimLoopCheckBox));
		info.AddProperty(PropertyName._particlesHFramesSpinBox, Variant.From(in _particlesHFramesSpinBox));
		info.AddProperty(PropertyName._particlesVFramesSpinBox, Variant.From(in _particlesVFramesSpinBox));
		info.AddProperty(PropertyName._albedoColorPicker, Variant.From(in _albedoColorPicker));
		info.AddProperty(PropertyName._metallicSpinBox, Variant.From(in _metallicSpinBox));
		info.AddProperty(PropertyName._roughnessSpinBox, Variant.From(in _roughnessSpinBox));
		info.AddProperty(PropertyName._material, Variant.From(in _material));
		info.AddProperty(PropertyName._updating, Variant.From(in _updating));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._materialPreview, out var value))
		{
			_materialPreview = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._shaderControls, out var value2))
		{
			_shaderControls = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._canvasControls, out var value3))
		{
			_canvasControls = value3.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._base3DControls, out var value4))
		{
			_base3DControls = value4.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._shaderPicker, out var value5))
		{
			_shaderPicker = value5.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._shaderParamTree, out var value6))
		{
			_shaderParamTree = value6.As<Tree>();
		}
		if (info.TryGetProperty(PropertyName._blendModeOption, out var value7))
		{
			_blendModeOption = value7.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._lightModeOption, out var value8))
		{
			_lightModeOption = value8.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._particlesAnimationCheckBox, out var value9))
		{
			_particlesAnimationCheckBox = value9.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._particlesAnimLoopCheckBox, out var value10))
		{
			_particlesAnimLoopCheckBox = value10.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._particlesHFramesSpinBox, out var value11))
		{
			_particlesHFramesSpinBox = value11.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._particlesVFramesSpinBox, out var value12))
		{
			_particlesVFramesSpinBox = value12.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._albedoColorPicker, out var value13))
		{
			_albedoColorPicker = value13.As<ColorPickerButton>();
		}
		if (info.TryGetProperty(PropertyName._metallicSpinBox, out var value14))
		{
			_metallicSpinBox = value14.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._roughnessSpinBox, out var value15))
		{
			_roughnessSpinBox = value15.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._material, out var value16))
		{
			_material = value16.As<Material>();
		}
		if (info.TryGetProperty(PropertyName._updating, out var value17))
		{
			_updating = value17.As<bool>();
		}
	}
}
