using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Other/ThreeCornpult/Scene/TowerDefensePlantThreeCornpult.cs")]
public class TowerDefensePlantThreeCornpult : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConfirmProjectile = "ConfirmProjectile";

		public static readonly StringName FireOver = "FireOver";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName FireInterval = "FireInterval";

		public static readonly StringName FireNum = "FireNum";

		public static readonly StringName _fireInterval = "_fireInterval";

		public static readonly StringName _fireNum = "_fireNum";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private FireComponent _fireComponent;

	private double _fireInterval = 3.0;

	private int _fireNum = 1;

	[Export(PropertyHint.None, "")]
	public double FireInterval
	{
		get
		{
			return _fireInterval;
		}
		set
		{
			_fireInterval = value;
			if (IsNodeReady() && _fireComponent != null)
			{
				_fireComponent.fireInterval = (float)_fireInterval;
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int FireNum
	{
		get
		{
			return _fireNum;
		}
		set
		{
			_fireNum = value;
			if (IsNodeReady() && _fireComponent != null)
			{
				_fireComponent.fireNum = _fireNum;
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			_fireComponent.BindCheckAreaShapeToGridRow(0, 1);
			_fireComponent.BindCheckAreaShapeToGridRow(2, -1);
			_fireComponent.OnConfirmProjectile += ConfirmProjectile;
			_fireComponent.OnFireOver += FireOver;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		FireComponent fireComponent = _fireComponent;
		if (fireComponent != null && !fireComponent.IsReleased)
		{
			_fireComponent.OnConfirmProjectile -= ConfirmProjectile;
			_fireComponent.OnFireOver -= FireOver;
		}
	}

	public void ConfirmProjectile(int projectileId, TowerDefenseProjectileCreateData projectileData)
	{
		string text = projectileData.projectileName.ToString();
		switch (projectileId)
		{
		case 0:
			if (!(text == "Butter"))
			{
				if (text == "ButterCatapult")
				{
					sprite.SetFliters(new Array { "Cornpult_butter" }, open: true);
					sprite.SetFliters(new Array { "Cornpult_kernal" }, open: false);
				}
			}
			else
			{
				sprite.SetFliters(new Array { "Cornpult_butter" }, open: false);
				sprite.SetFliters(new Array { "Cornpult_kernal" }, open: true);
			}
			break;
		case 1:
			if (!(text == "Butter"))
			{
				if (text == "ButterCatapult")
				{
					sprite.SetFliters(new Array { "Cornpult_butter 澶嶅埗" }, open: true);
					sprite.SetFliters(new Array { "Cornpult_kernal 澶嶅埗" }, open: false);
				}
			}
			else
			{
				sprite.SetFliters(new Array { "Cornpult_butter 澶嶅埗" }, open: false);
				sprite.SetFliters(new Array { "Cornpult_kernal 澶嶅埗" }, open: true);
			}
			break;
		case 2:
			if (!(text == "Butter"))
			{
				if (text == "ButterCatapult")
				{
					sprite.SetFliters(new Array { "Cornpult_butter   澶嶅埗 2" }, open: true);
					sprite.SetFliters(new Array { "Cornpult_kernal   澶嶅埗 2" }, open: false);
				}
			}
			else
			{
				sprite.SetFliters(new Array { "Cornpult_butter   澶嶅埗 2" }, open: false);
				sprite.SetFliters(new Array { "Cornpult_kernal   澶嶅埗 2" }, open: true);
			}
			break;
		}
	}

	public void FireOver()
	{
		sprite.SetFliters(new Array { "Cornpult_butter" }, open: false);
		sprite.SetFliters(new Array { "Cornpult_butter 澶嶅埗" }, open: false);
		sprite.SetFliters(new Array { "Cornpult_butter  澶嶅埗 2" }, open: false);
		sprite.SetFliters(new Array { "Cornpult_kernal" }, open: true);
		sprite.SetFliters(new Array { "Cornpult_kernal 澶嶅埗" }, open: true);
		sprite.SetFliters(new Array { "Cornpult_kernal  澶嶅埗 2" }, open: true);
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "fireNum", FireNum },
			{ "fireInterval", FireInterval }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		FireNum = data.GetValueOrDefault("fireNum", Variant.From<int>(1)).AsInt32();
		FireInterval = data.GetValueOrDefault("fireInterval", Variant.From<double>(3.0)).AsDouble();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfirmProjectile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "projectileId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FireOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ConfirmProjectile && args.Count == 2)
		{
			ConfirmProjectile(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FireOver && args.Count == 0)
		{
			FireOver();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName.ConfirmProjectile)
		{
			return true;
		}
		if (method == MethodName.FireOver)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.FireInterval)
		{
			FireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.FireNum)
		{
			FireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			_fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._fireNum)
		{
			_fireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.FireInterval)
		{
			value = VariantUtils.CreateFrom<double>(FireInterval);
			return true;
		}
		if (name == PropertyName.FireNum)
		{
			value = VariantUtils.CreateFrom<int>(FireNum);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			value = VariantUtils.CreateFrom(in _fireInterval);
			return true;
		}
		if (name == PropertyName._fireNum)
		{
			value = VariantUtils.CreateFrom(in _fireNum);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.FireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.FireNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._fireNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.FireInterval, Variant.From<double>(FireInterval));
		info.AddProperty(PropertyName.FireNum, Variant.From<int>(FireNum));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
		info.AddProperty(PropertyName._fireNum, Variant.From(in _fireNum));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.FireInterval, out var value))
		{
			FireInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.FireNum, out var value2))
		{
			FireNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value3))
		{
			_fireInterval = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._fireNum, out var value4))
		{
			_fireNum = value4.As<int>();
		}
	}
}
