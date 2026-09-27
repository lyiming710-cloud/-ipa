using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseGravestone.cs")]
public class TowerDefenseGravestone : TowerDefenseCharacter
{
	public new class MethodName : TowerDefenseCharacter.MethodName
	{
		public static readonly StringName OnPlantingBlocked = "OnPlantingBlocked";

		public new static readonly StringName PrepareForProgressRestore = "PrepareForProgressRestore";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName _Ready = "_Ready";
	}

	public new class PropertyName : TowerDefenseCharacter.PropertyName
	{
		public static readonly StringName rise = "rise";

		public static readonly StringName IsPermanentObstacle = "IsPermanentObstacle";

		public static readonly StringName _preparedForProgressRestore = "_preparedForProgressRestore";
	}

	public new class SignalName : TowerDefenseCharacter.SignalName
	{
	}

	private bool _preparedForProgressRestore;

	[Export(PropertyHint.None, "")]
	public bool rise { get; set; } = true;

	public virtual bool IsPermanentObstacle => false;

	public virtual void OnPlantingBlocked()
	{
	}

	public override void PrepareForProgressRestore()
	{
		base.PrepareForProgressRestore();
		_preparedForProgressRestore = true;
		rise = false;
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		if (_preparedForProgressRestore && isRise)
		{
			riseComponent?.CompleteProgressRestore();
		}
		_preparedForProgressRestore = false;
	}

	public override void _Ready()
	{
		base._Ready();
		if (editorPreviewMode || Engine.IsEditorHint())
		{
			return;
		}
		instance.hitpointsEmpty += () =>
		{
			Destroy();
		};
		AddToGroup("Gravestone", persistent: true);
		if (rise)
		{
			if (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage")
			{
				rise = false;
			}
			else
			{
				Rise(GD.RandRange(0.75, 1.25));
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.OnPlantingBlocked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareForProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.OnPlantingBlocked && args.Count == 0)
		{
			OnPlantingBlocked();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareForProgressRestore && args.Count == 0)
		{
			PrepareForProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportVariantSaveWhenEmpty());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.OnPlantingBlocked)
		{
			return true;
		}
		if (method == MethodName.PrepareForProgressRestore)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.rise)
		{
			rise = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._preparedForProgressRestore)
		{
			_preparedForProgressRestore = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.rise)
		{
			from = rise;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsPermanentObstacle)
		{
			from = IsPermanentObstacle;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._preparedForProgressRestore)
		{
			value = VariantUtils.CreateFrom(in _preparedForProgressRestore);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.rise, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preparedForProgressRestore, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsPermanentObstacle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.rise, Variant.From<bool>(rise));
		info.AddProperty(PropertyName._preparedForProgressRestore, Variant.From(in _preparedForProgressRestore));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.rise, out var value))
		{
			rise = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._preparedForProgressRestore, out var value2))
		{
			_preparedForProgressRestore = value2.As<bool>();
		}
	}
}
