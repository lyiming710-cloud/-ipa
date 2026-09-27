using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Script/Component/LegacyComponentManagerNode.cs")]
public class LegacyComponentManagerNode : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName MigrateComponentsTo = "MigrateComponentsTo";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName ComponentSet = "ComponentSet";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public CharacterComponentSet ComponentSet { get; set; }

	public void MigrateComponentsTo(TowerDefenseCharacter owner)
	{
		if (!GodotObject.IsInstanceValid(owner))
		{
			return;
		}
		Array<Node> children = GetChildren();
		for (int i = 0; i < children.Count; i++)
		{
			if (children[i] is ComponentBase componentBase)
			{
				componentBase.Reparent(owner);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.MigrateComponentsTo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.MigrateComponentsTo && args.Count == 1)
		{
			MigrateComponentsTo(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.MigrateComponentsTo)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ComponentSet)
		{
			ComponentSet = VariantUtils.ConvertTo<CharacterComponentSet>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ComponentSet)
		{
			value = VariantUtils.CreateFrom<CharacterComponentSet>(ComponentSet);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.ComponentSet, PropertyHint.ResourceType, "CharacterComponentSet", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ComponentSet, Variant.From<CharacterComponentSet>(ComponentSet));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ComponentSet, out var value))
		{
			ComponentSet = value.As<CharacterComponentSet>();
		}
	}
}
