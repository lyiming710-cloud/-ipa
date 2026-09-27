using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter5/Nutleaf/Scene/TowerDefensePlantNutleaf.cs")]
public class TowerDefensePlantNutleaf : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName BlockCharacter = "BlockCharacter";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName DamagePointReach = "DamagePointReach";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _isBlocking = "_isBlocking";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string NUTLEAF_BODY_2 = "uid://dtdgl22bp0385";

	private const string NUTLEAF_BODY = "uid://d1rii54n6ebjt";

	private const string NUTLEAF_LEAF_1 = "uid://du74itvg0v6tv";

	private const string NUTLEAF_LEAF_1_1 = "uid://bvgoi2t06wkjk";

	private const string NUTLEAF_LEAF_1_2 = "uid://0vubdg55l51c";

	private const string NUTLEAF_LEAF_2 = "uid://befoku44k4xgc";

	private const string NUTLEAF_LEAF_2_1 = "uid://cge2vjl6eavp8";

	private const string NUTLEAF_LEAF_2_2 = "uid://r8qcq2gy4hcr";

	private const string NUTLEAF_LEAF_7 = "uid://bh5hfx1p28j8d";

	private const string NUTLEAF_LEAF_7_1 = "uid://61bj8cbbee7r";

	private const string NUTLEAF_LEAF_7_2 = "uid://470542g6yvnh";

	private const string NUTLEAF_SKIN_1_1 = "uid://3bvuwmhgxhhs";

	private const string NUTLEAF_SKIN_1_2 = "uid://c05sqej8dxhvo";

	private const string NUTLEAF_SKIN_1_3 = "uid://c2o3ut38gtgc3";

	private const string NUTLEAF_SKIN_2_1 = "uid://dloldmb0o56i0";

	private const string NUTLEAF_SKIN_2_2 = "uid://f2hitkqe12ar";

	private const string NUTLEAF_SKIN_2_3 = "uid://bwlwi5in4jse3";

	private const string NUTLEAF_SKIN_7_1 = "uid://bpoowutc8v4dt";

	private const string NUTLEAF_SKIN_7_2 = "uid://qkspyxn7b1sv";

	private const string NUTLEAF_SKIN_7_3 = "uid://c1u4rrfof5oa7";

	private BlockComponent _blockComponent;

	private bool _isBlocking;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_blockComponent = componentManager.GetRuntime<BlockComponent>();
			if (_blockComponent != null)
			{
				_blockComponent.OnBlock += BlockCharacter;
				_blockComponent.SetCheckRectangleSize(TowerDefenseManager.Instance.GetMapGridSize() * 4f);
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_blockComponent != null)
		{
			_blockComponent.OnBlock -= BlockCharacter;
		}
		_blockComponent = null;
	}

	public virtual void BlockCharacter()
	{
		if (!_isBlocking)
		{
			_isBlocking = true;
			itemLayer = TowerDefenseEnum.LAYER_GROUNDITEM.EFFECT;
			sprite.SetAnimation("Block", loop: false, 0.1);
			sprite.AddAnimation("Idle", 0.0);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Block")
		{
			_isBlocking = false;
			itemLayer = TowerDefenseEnum.LAYER_GROUNDITEM.PLANT;
		}
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		switch (damangePointName)
		{
		case "Damage0":
			sprite.SetAtlasReplace("Nutleaf_body.png", "uid://d1rii54n6ebjt");
			sprite.SetAtlasReplace("Nutleaf_leaf1.png", "uid://du74itvg0v6tv");
			sprite.SetAtlasReplace("Nutleaf_leaf2.png", "uid://befoku44k4xgc");
			sprite.SetAtlasReplace("Nutleaf_leaf7.png", "uid://bh5hfx1p28j8d");
			if (currentCustom.Contains("Custom0"))
			{
				sprite.SetAtlasReplace("Nutleaf_skin1_1.png", "uid://3bvuwmhgxhhs");
				sprite.SetAtlasReplace("Nutleaf_skin2_1.png", "uid://dloldmb0o56i0");
				sprite.SetAtlasReplace("Nutleaf_skin7_1.png", "uid://bpoowutc8v4dt");
			}
			break;
		case "Damage1":
			sprite.SetAtlasReplace("Nutleaf_leaf1.png", "uid://bvgoi2t06wkjk");
			sprite.SetAtlasReplace("Nutleaf_leaf2.png", "uid://cge2vjl6eavp8");
			sprite.SetAtlasReplace("Nutleaf_leaf7.png", "uid://61bj8cbbee7r");
			if (currentCustom.Contains("Custom0"))
			{
				sprite.SetAtlasReplace("Nutleaf_skin1_1.png", "uid://c05sqej8dxhvo");
				sprite.SetAtlasReplace("Nutleaf_skin2_1.png", "uid://f2hitkqe12ar");
				sprite.SetAtlasReplace("Nutleaf_skin7_1.png", "uid://qkspyxn7b1sv");
			}
			break;
		case "Damage2":
			sprite.SetAtlasReplace("Nutleaf_body.png", "uid://dtdgl22bp0385");
			sprite.SetAtlasReplace("Nutleaf_leaf1.png", "uid://0vubdg55l51c");
			sprite.SetAtlasReplace("Nutleaf_leaf2.png", "uid://r8qcq2gy4hcr");
			sprite.SetAtlasReplace("Nutleaf_leaf7.png", "uid://470542g6yvnh");
			if (currentCustom.Contains("Custom0"))
			{
				sprite.SetAtlasReplace("Nutleaf_skin1_1.png", "uid://c2o3ut38gtgc3");
				sprite.SetAtlasReplace("Nutleaf_skin2_1.png", "uid://bwlwi5in4jse3");
				sprite.SetAtlasReplace("Nutleaf_skin7_1.png", "uid://c1u4rrfof5oa7");
			}
			break;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlockCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BlockCharacter && args.Count == 0)
		{
			BlockCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.BlockCharacter)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._isBlocking)
		{
			_isBlocking = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._isBlocking)
		{
			value = VariantUtils.CreateFrom(in _isBlocking);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._isBlocking, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._isBlocking, Variant.From(in _isBlocking));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._isBlocking, out var value))
		{
			_isBlocking = value.As<bool>();
		}
	}
}
