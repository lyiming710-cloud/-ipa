using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Registry/Battle/Feature/Wave/TrioAmbush/TrioSeaweed.cs")]
public class TrioSeaweed : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName Attach = "Attach";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName HeadLayer = "HeadLayer";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName UpdateAttachments = "UpdateAttachments";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _zombie = "_zombie";

		public static readonly StringName _layers = "_layers";

		public static readonly StringName _offsets = "_offsets";

		public static readonly StringName _material = "_material";

		public static readonly StringName _age = "_age";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private TowerDefenseZombie _zombie;

	private readonly List<Node2D> _anchors = new List<Node2D>();

	private readonly List<Sprite2D> _blades = new List<Sprite2D>();

	private readonly string[] _layers = new string[3] { "anim_head1", "Zombie_outerarm_upper", "Zombie_duckytube" };

	private readonly Vector2[] _offsets = new Vector2[3]
	{
		new Vector2(30f, 20f),
		new Vector2(5f, 5f),
		new Vector2(77f, 20f)
	};

	private ShaderMaterial _material;

	private double _age;

	public static void Attach(TowerDefenseZombie zombie)
	{
		if (!zombie.HasNode("TrioSeaweed"))
		{
			zombie.AddChild(new TrioSeaweed
			{
				Name = "TrioSeaweed",
				_zombie = zombie
			}, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public override void _Ready()
	{
		ZIndex = 5;
		ProcessPhysicsPriority = 10002;
		_material = new ShaderMaterial
		{
			Shader = GD.Load<Shader>("res://Registry/Battle/Feature/Wave/TrioAmbush/TrioSeaweed.gdshader")
		};
		Texture2D texture = GD.Load<Texture2D>("res://Asset/Texture/Character/Effect/Zombie/ZombieSeaweed.png");
		for (int i = 0; i < 3; i++)
		{
			Node2D node2D = new Node2D
			{
				Name = "Attachment" + i
			};
			AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
			_anchors.Add(node2D);
			for (int j = 0; j < 2; j++)
			{
				Sprite2D sprite2D = new Sprite2D
				{
					Texture = texture,
					Hframes = 4,
					Frame = (i + j) % 4,
					Material = _material,
					Scale = Vector2.One * ((j == 0) ? 0.65f : 0.45f),
					Position = new Vector2(j * 12 - 6, j * 5),
					FlipH = ((i + j) % 2 == 0)
				};
				node2D.AddChild(sprite2D, forceReadableName: false, InternalMode.Disabled);
				_blades.Add(sprite2D);
			}
		}
		UpdateAttachments();
	}

	private string HeadLayer()
	{
		foreach (TowerDefenseArmorInstance armor in _zombie.instance.armorList)
		{
			if (!armor.isRemove)
			{
				if (armor.slotConfig.armorName == "Cone")
				{
					return "anim_cone";
				}
				if (armor.slotConfig.armorName == "Bucket")
				{
					return "anim_bucket";
				}
			}
		}
		return "anim_head1";
	}

	public override void _Process(double delta)
	{
		if (GodotObject.IsInstanceValid(_zombie) && _zombie.IsNodeReady())
		{
			_age += delta;
			UpdateAttachments();
		}
	}

	private void UpdateAttachments()
	{
		_layers[0] = HeadLayer();
		for (int i = 0; i < _anchors.Count; i++)
		{
			Node2D node2D = _anchors[i];
			int slotLayerId = _zombie.sprite.flashAnimeData.layerDictionary[_layers[i]].AsInt32();
			bool flag = _zombie.sprite.TryGetInterpolatedSlotPose(slotLayerId, out var mediaId, out var transform);
			node2D.Visible = flag && mediaId >= 0 && !_zombie.invisible && !_zombie.die && !_zombie.nearDie && (i != 1 || !(_zombie is TowerDefenseZombieNormal towerDefenseZombieNormal) || !towerDefenseZombieNormal.halfHp);
			if (flag)
			{
				transform = transform.Translated(_zombie.sprite.offset).TranslatedLocal(_offsets[i]);
				node2D.GlobalTransform = _zombie.GetLogicalGlobalTransform(_zombie.sprite) * transform;
			}
		}
		for (int j = 0; j < _blades.Count; j++)
		{
			_blades[j].Rotation = Mathf.Sin((float)_age * 2f + (float)j) * 0.05f;
		}
		_material.SetShaderParameter("surface_y", _zombie.inWater ? (_zombie.GlobalPosition.Y + 36f) : 100000f);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.Attach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HeadLayer, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateAttachments, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Attach && args.Count == 1)
		{
			Attach(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.HeadLayer && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(HeadLayer());
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateAttachments && args.Count == 0)
		{
			UpdateAttachments();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Attach && args.Count == 1)
		{
			Attach(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Attach)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.HeadLayer)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.UpdateAttachments)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._zombie)
		{
			_zombie = VariantUtils.ConvertTo<TowerDefenseZombie>(in value);
			return true;
		}
		if (name == PropertyName._material)
		{
			_material = VariantUtils.ConvertTo<ShaderMaterial>(in value);
			return true;
		}
		if (name == PropertyName._age)
		{
			_age = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._zombie)
		{
			value = VariantUtils.CreateFrom(in _zombie);
			return true;
		}
		if (name == PropertyName._layers)
		{
			value = VariantUtils.CreateFrom(in _layers);
			return true;
		}
		if (name == PropertyName._offsets)
		{
			value = VariantUtils.CreateFrom(in _offsets);
			return true;
		}
		if (name == PropertyName._material)
		{
			value = VariantUtils.CreateFrom(in _material);
			return true;
		}
		if (name == PropertyName._age)
		{
			value = VariantUtils.CreateFrom(in _age);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._zombie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._layers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedVector2Array, PropertyName._offsets, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._material, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._age, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._zombie, Variant.From(in _zombie));
		info.AddProperty(PropertyName._material, Variant.From(in _material));
		info.AddProperty(PropertyName._age, Variant.From(in _age));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._zombie, out var value))
		{
			_zombie = value.As<TowerDefenseZombie>();
		}
		if (info.TryGetProperty(PropertyName._material, out var value2))
		{
			_material = value2.As<ShaderMaterial>();
		}
		if (info.TryGetProperty(PropertyName._age, out var value3))
		{
			_age = value3.As<double>();
		}
	}
}
