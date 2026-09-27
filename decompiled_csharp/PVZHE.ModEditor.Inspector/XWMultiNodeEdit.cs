using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/RefCounted/XWMultiNodeEdit.cs")]
public class XWMultiNodeEdit : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Create = "Create";

		public static readonly StringName GetCommonProperties = "GetCommonProperties";

		public static readonly StringName GetPropertyValue = "GetPropertyValue";

		public static readonly StringName IsPropertySameForAll = "IsPropertySameForAll";

		public static readonly StringName SetPropertyForAll = "SetPropertyForAll";

		public static readonly StringName GetNodeCount = "GetNodeCount";

		public static readonly StringName GetFirstNode = "GetFirstNode";

		public static readonly StringName GetClassName = "GetClassName";

		public static readonly StringName GetDisplayName = "GetDisplayName";

		public static readonly StringName GetCommonBaseClass = "GetCommonBaseClass";

		public static readonly StringName ValuesEqual = "ValuesEqual";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public List<Node> Nodes { get; private set; } = new List<Node>();

	public static XWMultiNodeEdit Create(Node[] nodes)
	{
		XWMultiNodeEdit xWMultiNodeEdit = new XWMultiNodeEdit();
		if (nodes != null)
		{
			xWMultiNodeEdit.Nodes.AddRange(nodes);
		}
		return xWMultiNodeEdit;
	}

	public Array<Dictionary> GetCommonProperties()
	{
		Array<Dictionary> array = new Array<Dictionary>();
		if (Nodes.Count == 0)
		{
			return array;
		}
		Node node = Nodes[0];
		if (!GodotObject.IsInstanceValid(node))
		{
			return array;
		}
		string text = node.GetClass();
		foreach (Node node3 in Nodes)
		{
			if (!GodotObject.IsInstanceValid(node3))
			{
				return array;
			}
			if (node3.GetClass() != text)
			{
				text = GetCommonBaseClass(text, node3.GetClass());
				if (string.IsNullOrEmpty(text))
				{
					return array;
				}
			}
		}
		System.Collections.Generic.Dictionary<string, Dictionary> dictionary = new System.Collections.Generic.Dictionary<string, Dictionary>();
		foreach (Dictionary property in node.GetPropertyList())
		{
			long num = property["usage"].AsInt64();
			if ((num & 2) != 0L || (num & 4) != 0L)
			{
				string key = property["name"].AsString();
				dictionary[key] = property;
			}
		}
		foreach (KeyValuePair<string, Dictionary> item in dictionary)
		{
			bool flag = true;
			for (int i = 1; i < Nodes.Count; i++)
			{
				Node node2 = Nodes[i];
				if (!GodotObject.IsInstanceValid(node2))
				{
					flag = false;
					break;
				}
				bool flag2 = false;
				foreach (Dictionary property2 in node2.GetPropertyList())
				{
					if (property2["name"].AsString() == item.Key)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				array.Add(item.Value);
			}
		}
		return array;
	}

	public Variant GetPropertyValue(StringName propName)
	{
		if (Nodes.Count == 0)
		{
			return default;
		}
		Node node = Nodes[0];
		if (!GodotObject.IsInstanceValid(node))
		{
			return default;
		}
		return node.Get(propName);
	}

	public bool IsPropertySameForAll(StringName propName)
	{
		if (Nodes.Count <= 1)
		{
			return true;
		}
		Variant a = (GodotObject.IsInstanceValid(Nodes[0]) ? Nodes[0].Get(propName) : default(Variant));
		for (int i = 1; i < Nodes.Count; i++)
		{
			if (!GodotObject.IsInstanceValid(Nodes[i]))
			{
				return false;
			}
			Variant b = Nodes[i].Get(propName);
			if (!ValuesEqual(a, b))
			{
				return false;
			}
		}
		return true;
	}

	public void SetPropertyForAll(StringName propName, Variant value)
	{
		foreach (Node node in Nodes)
		{
			if (GodotObject.IsInstanceValid(node))
			{
				node.Set(propName, value);
			}
		}
	}

	public int GetNodeCount()
	{
		return Nodes.Count;
	}

	public Node GetFirstNode()
	{
		if (Nodes.Count <= 0)
		{
			return null;
		}
		return Nodes[0];
	}

	public string GetClassName()
	{
		if (Nodes.Count != 0)
		{
			return Nodes[0].GetClass();
		}
		return "";
	}

	public string GetDisplayName()
	{
		return $"{Nodes.Count} 个节点";
	}

	private static string GetCommonBaseClass(string classA, string classB)
	{
		List<string> classHierarchy = GetClassHierarchy(classA);
		List<string> classHierarchy2 = GetClassHierarchy(classB);
		foreach (string item in classHierarchy)
		{
			if (classHierarchy2.Contains(item))
			{
				return item;
			}
		}
		return "";
	}

	private static List<string> GetClassHierarchy(string cls)
	{
		List<string> list = new List<string>();
		string text = cls;
		while (!string.IsNullOrEmpty(text))
		{
			list.Add(text);
			text = ClassDB.GetParentClass(text);
		}
		return list;
	}

	private static bool ValuesEqual(Variant a, Variant b)
	{
		if (a.VariantType == Variant.Type.Nil && b.VariantType == Variant.Type.Nil)
		{
			return true;
		}
		if (a.VariantType == Variant.Type.Nil || b.VariantType == Variant.Type.Nil)
		{
			return false;
		}
		if (a.VariantType == Variant.Type.Float && b.VariantType == Variant.Type.Float)
		{
			return Math.Abs(a.AsSingle() - b.AsSingle()) < 0.0001f;
		}
		return a.Equals(b);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "nodes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCommonProperties, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPropertyValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "propName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPropertySameForAll, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "propName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPropertyForAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "propName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNodeCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFirstNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetClassName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCommonBaseClass, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "classA", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "classB", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ValuesEqual, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "a", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "b", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWMultiNodeEdit>(Create(VariantUtils.ConvertToSystemArrayOfGodotObject<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCommonProperties && args.Count == 0)
		{
			Array<Dictionary> commonProperties = GetCommonProperties();
			ret = VariantUtils.CreateFromArray(commonProperties);
			return true;
		}
		if (method == MethodName.GetPropertyValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetPropertyValue(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.IsPropertySameForAll && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPropertySameForAll(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.SetPropertyForAll && args.Count == 2)
		{
			SetPropertyForAll(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetNodeCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNodeCount());
			return true;
		}
		if (method == MethodName.GetFirstNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(GetFirstNode());
			return true;
		}
		if (method == MethodName.GetClassName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetClassName());
			return true;
		}
		if (method == MethodName.GetDisplayName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetDisplayName());
			return true;
		}
		if (method == MethodName.GetCommonBaseClass && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetCommonBaseClass(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ValuesEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ValuesEqual(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWMultiNodeEdit>(Create(VariantUtils.ConvertToSystemArrayOfGodotObject<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCommonBaseClass && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetCommonBaseClass(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ValuesEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ValuesEqual(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName.GetCommonProperties)
		{
			return true;
		}
		if (method == MethodName.GetPropertyValue)
		{
			return true;
		}
		if (method == MethodName.IsPropertySameForAll)
		{
			return true;
		}
		if (method == MethodName.SetPropertyForAll)
		{
			return true;
		}
		if (method == MethodName.GetNodeCount)
		{
			return true;
		}
		if (method == MethodName.GetFirstNode)
		{
			return true;
		}
		if (method == MethodName.GetClassName)
		{
			return true;
		}
		if (method == MethodName.GetDisplayName)
		{
			return true;
		}
		if (method == MethodName.GetCommonBaseClass)
		{
			return true;
		}
		if (method == MethodName.ValuesEqual)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
