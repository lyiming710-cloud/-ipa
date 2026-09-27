using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/Runtime/CharacterComponentSet.cs")]
public class CharacterComponentSet : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName InvalidateFlattenedDefinitions = "InvalidateFlattenedDefinitions";

		public static readonly StringName InvalidateCreationPlan = "InvalidateCreationPlan";

		public static readonly StringName ComputeCreationPlanGraphRevision = "ComputeCreationPlanGraphRevision";

		public static readonly StringName GetFlattenedDefinitionArray = "GetFlattenedDefinitionArray";

		public static readonly StringName ComputeFlattenedSignature = "ComputeFlattenedSignature";

		public static readonly StringName HasInheritanceCycle = "HasInheritanceCycle";

		public static readonly StringName HasRegistryBehaviorIds = "HasRegistryBehaviorIds";

		public static readonly StringName MixSignature = "MixSignature";

		public static readonly StringName CanReplaceInheritedDefinition = "CanReplaceInheritedDefinition";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName ParentSet = "ParentSet";

		public static readonly StringName BehaviorIds = "BehaviorIds";

		public static readonly StringName Components = "Components";

		public static readonly StringName RemovedInstanceIds = "RemovedInstanceIds";

		public static readonly StringName _parentSet = "_parentSet";

		public static readonly StringName _behaviorIds = "_behaviorIds";

		public static readonly StringName _components = "_components";

		public static readonly StringName _removedInstanceIds = "_removedInstanceIds";

		public static readonly StringName _flattenedDefinitionsSignature = "_flattenedDefinitionsSignature";

		public static readonly StringName _flattenedGraphRevision = "_flattenedGraphRevision";

		public static readonly StringName _flattenedRegistryRevision = "_flattenedRegistryRevision";

		public static readonly StringName _flattenedUsesRegistry = "_flattenedUsesRegistry";

		public static readonly StringName _creationPlanGraphRevision = "_creationPlanGraphRevision";

		public static readonly StringName _creationPlanRegistryRevision = "_creationPlanRegistryRevision";

		public static readonly StringName _creationPlanUsesRegistry = "_creationPlanUsesRegistry";

		public static readonly StringName _creationPlanInvalidationRevision = "_creationPlanInvalidationRevision";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private CharacterComponentSet _parentSet;

	private Array<StringName> _behaviorIds = new Array<StringName>();

	private Array<CharacterComponentDefinition> _components = new Array<CharacterComponentDefinition>();

	private Array<string> _removedInstanceIds = new Array<string>();

	private IReadOnlyList<CharacterComponentDefinition> _flattenedDefinitionsCache;

	private ulong _flattenedDefinitionsSignature;

	private ulong _flattenedGraphRevision;

	private ulong _flattenedRegistryRevision;

	private bool _flattenedUsesRegistry;

	private CharacterComponentCreationPlan _creationPlanCache;

	private ulong _creationPlanGraphRevision;

	private ulong _creationPlanRegistryRevision;

	private bool _creationPlanUsesRegistry;

	private ulong _creationPlanInvalidationRevision = 1uL;

	[Export(PropertyHint.None, "")]
	public CharacterComponentSet ParentSet
	{
		get
		{
			return _parentSet;
		}
		set
		{
			if (_parentSet != value)
			{
				_parentSet = value;
				InvalidateFlattenedDefinitions();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<StringName> BehaviorIds
	{
		get
		{
			return _behaviorIds;
		}
		set
		{
			if (value == null)
			{
				value = new Array<StringName>();
			}
			if (_behaviorIds != value)
			{
				_behaviorIds = value;
				InvalidateFlattenedDefinitions();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<CharacterComponentDefinition> Components
	{
		get
		{
			return _components;
		}
		set
		{
			if (value == null)
			{
				value = new Array<CharacterComponentDefinition>();
			}
			if (_components != value)
			{
				_components = value;
				InvalidateFlattenedDefinitions();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<string> RemovedInstanceIds
	{
		get
		{
			return _removedInstanceIds;
		}
		set
		{
			if (value == null)
			{
				value = new Array<string>();
			}
			if (_removedInstanceIds != value)
			{
				_removedInstanceIds = value;
				InvalidateFlattenedDefinitions();
			}
		}
	}

	public void InvalidateFlattenedDefinitions()
	{
		_flattenedDefinitionsCache = null;
		_flattenedDefinitionsSignature = 0uL;
		_flattenedGraphRevision = 0uL;
		_flattenedRegistryRevision = 0uL;
		_flattenedUsesRegistry = false;
		InvalidateCreationPlan();
	}

	private void InvalidateCreationPlan()
	{
		_creationPlanCache = null;
		_creationPlanGraphRevision = 0uL;
		_creationPlanRegistryRevision = 0uL;
		_creationPlanUsesRegistry = false;
		_creationPlanInvalidationRevision++;
		if (_creationPlanInvalidationRevision == 0L)
		{
			_creationPlanInvalidationRevision = 1uL;
		}
	}

	public CharacterComponentCreationPlan GetCreationPlan()
	{
		bool flag = HasInheritanceCycle();
		bool flag2 = !flag && HasRegistryBehaviorIds();
		ulong num = (flag2 ? TowerDefenseBehaviorRegistry.Revision : 0);
		ulong num2 = (flag ? 0 : ComputeCreationPlanGraphRevision());
		if (!Engine.IsEditorHint() && !flag && _creationPlanCache != null && _creationPlanUsesRegistry == flag2 && _creationPlanRegistryRevision == num && _creationPlanGraphRevision == num2)
		{
			return _creationPlanCache;
		}
		CharacterComponentCreationPlan characterComponentCreationPlan = CharacterComponentCreationPlan.Build(BuildFlattenedDefinitions(out var graphValid), graphValid && !flag);
		if (!Engine.IsEditorHint() && !flag && characterComponentCreationPlan.IsGraphValid)
		{
			_creationPlanGraphRevision = num2;
			_creationPlanRegistryRevision = num;
			_creationPlanUsesRegistry = flag2;
			_creationPlanCache = characterComponentCreationPlan;
		}
		return characterComponentCreationPlan;
	}

	private ulong ComputeCreationPlanGraphRevision()
	{
		ulong signature = 14695981039346656037uL;
		for (CharacterComponentSet characterComponentSet = this; characterComponentSet != null; characterComponentSet = characterComponentSet.ParentSet)
		{
			signature = MixSignature(signature, characterComponentSet._creationPlanInvalidationRevision);
		}
		return MixSignature(signature, 0uL);
	}

	public IReadOnlyList<CharacterComponentDefinition> GetFlattenedDefinitions()
	{
		bool flag = !HasInheritanceCycle();
		bool flag2 = flag && HasRegistryBehaviorIds();
		ulong num = (flag2 ? TowerDefenseBehaviorRegistry.Revision : 0);
		ulong num2 = (flag ? ComputeCreationPlanGraphRevision() : 0);
		if ((!Engine.IsEditorHint() & flag) && _flattenedDefinitionsCache != null && _flattenedUsesRegistry == flag2 && _flattenedGraphRevision == num2 && (!_flattenedUsesRegistry || _flattenedRegistryRevision == num))
		{
			return _flattenedDefinitionsCache;
		}
		ulong num3 = (flag ? ComputeFlattenedSignature() : 0);
		if (flag && _flattenedDefinitionsCache != null && _flattenedUsesRegistry == flag2 && _flattenedRegistryRevision == num && _flattenedDefinitionsSignature == num3)
		{
			_flattenedGraphRevision = num2;
			return _flattenedDefinitionsCache;
		}
		List<CharacterComponentDefinition> list = BuildFlattenedDefinitions(out var graphValid);
		if (!flag || !graphValid)
		{
			return list.AsReadOnly();
		}
		_flattenedDefinitionsSignature = num3;
		_flattenedGraphRevision = num2;
		_flattenedRegistryRevision = num;
		_flattenedUsesRegistry = flag2;
		_flattenedDefinitionsCache = list.AsReadOnly();
		return _flattenedDefinitionsCache;
	}

	private List<CharacterComponentDefinition> BuildFlattenedDefinitions(out bool graphValid)
	{
		List<CharacterComponentDefinition> list = new List<CharacterComponentDefinition>();
		HashSet<CharacterComponentSet> visited = new HashSet<CharacterComponentSet>(ReferenceEqualityComparer.Instance);
		graphValid = true;
		AppendFlattenedDefinitions(list, visited, ref graphValid);
		return list;
	}

	public Array<CharacterComponentDefinition> GetFlattenedDefinitionArray()
	{
		IReadOnlyList<CharacterComponentDefinition> flattenedDefinitions = GetFlattenedDefinitions();
		Array<CharacterComponentDefinition> array = new Array<CharacterComponentDefinition>();
		for (int i = 0; i < flattenedDefinitions.Count; i++)
		{
			array.Add(flattenedDefinitions[i]);
		}
		return array;
	}

	private ulong ComputeFlattenedSignature()
	{
		ulong signature = 14695981039346656037uL;
		CharacterComponentSet characterComponentSet = this;
		while (characterComponentSet != null)
		{
			CharacterComponentSet parentSet = characterComponentSet.ParentSet;
			signature = MixSignature(signature, characterComponentSet.GetInstanceId());
			signature = MixSignature(signature, (ulong)characterComponentSet.BehaviorIds.Count);
			for (int i = 0; i < characterComponentSet.BehaviorIds.Count; i++)
			{
				signature = MixSignature(signature, characterComponentSet.BehaviorIds[i].ToString());
			}
			signature = MixSignature(signature, (ulong)characterComponentSet.Components.Count);
			for (int j = 0; j < characterComponentSet.Components.Count; j++)
			{
				CharacterComponentDefinition characterComponentDefinition = characterComponentSet.Components[j];
				if (characterComponentDefinition == null)
				{
					signature = MixSignature(signature, 18446744073709551615uL);
					continue;
				}
				signature = MixSignature(signature, characterComponentDefinition.GetInstanceId());
				signature = MixSignature(signature, characterComponentDefinition.InstanceId);
				signature = MixSignature(signature, characterComponentDefinition.ComponentTypeId);
				signature = MixSignature(signature, (ulong)characterComponentDefinition.WireIndex);
			}
			signature = MixSignature(signature, (ulong)characterComponentSet.RemovedInstanceIds.Count);
			for (int k = 0; k < characterComponentSet.RemovedInstanceIds.Count; k++)
			{
				signature = MixSignature(signature, characterComponentSet.RemovedInstanceIds[k]);
			}
			characterComponentSet = parentSet;
		}
		return MixSignature(signature, 0uL);
	}

	private bool HasInheritanceCycle()
	{
		CharacterComponentSet characterComponentSet = this;
		CharacterComponentSet characterComponentSet2 = this;
		while (characterComponentSet2?.ParentSet != null)
		{
			characterComponentSet = characterComponentSet?.ParentSet;
			characterComponentSet2 = characterComponentSet2.ParentSet?.ParentSet;
			if (characterComponentSet != null && characterComponentSet == characterComponentSet2)
			{
				return true;
			}
		}
		return false;
	}

	private bool HasRegistryBehaviorIds()
	{
		for (CharacterComponentSet characterComponentSet = this; characterComponentSet != null; characterComponentSet = characterComponentSet.ParentSet)
		{
			if (characterComponentSet.BehaviorIds.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	private static ulong MixSignature(ulong signature, ulong value)
	{
		return (signature ^ value) * 1099511628211L;
	}

	private static ulong MixSignature(ulong signature, string value)
	{
		if (value == null)
		{
			return MixSignature(signature, 18446744073709551615uL);
		}
		signature = MixSignature(signature, (ulong)value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			signature = MixSignature(signature, value[i]);
		}
		return signature;
	}

	private void AppendFlattenedDefinitions(List<CharacterComponentDefinition> definitions, HashSet<CharacterComponentSet> visited, ref bool graphValid)
	{
		if (!visited.Add(this))
		{
			graphValid = false;
			GD.PushError("CharacterComponentSet inheritance cycle detected at '" + ResourcePath + "'.");
		}
		else
		{
			ParentSet?.AppendFlattenedDefinitions(definitions, visited, ref graphValid);
			RemoveInheritedDefinitions(definitions);
			MergeLocalDefinitions(definitions, ref graphValid);
		}
	}

	private void RemoveInheritedDefinitions(List<CharacterComponentDefinition> definitions)
	{
		if (RemovedInstanceIds.Count == 0 || definitions.Count == 0)
		{
			return;
		}
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		for (int i = 0; i < RemovedInstanceIds.Count; i++)
		{
			string text = RemovedInstanceIds[i]?.Trim();
			if (!string.IsNullOrEmpty(text))
			{
				hashSet.Add(text);
			}
		}
		for (int num = definitions.Count - 1; num >= 0; num--)
		{
			string text2 = definitions[num]?.InstanceId?.Trim();
			if (!string.IsNullOrEmpty(text2) && hashSet.Contains(text2))
			{
				definitions.RemoveAt(num);
			}
		}
	}

	private void MergeLocalDefinitions(List<CharacterComponentDefinition> definitions, ref bool graphValid)
	{
		System.Collections.Generic.Dictionary<string, int> instanceIndices = BuildInstanceIndex(definitions);
		for (int i = 0; i < BehaviorIds.Count; i++)
		{
			StringName stringName = BehaviorIds[i];
			Resource behavior = TowerDefenseBehaviorRegistry.GetBehavior(stringName);
			if (behavior is CharacterComponentDefinition definition)
			{
				MergeDefinition(definitions, instanceIndices, definition, ref graphValid);
				continue;
			}
			graphValid = false;
			GD.PushError($"[BehaviorRegistry:E_TYPE_MISMATCH] id='{stringName}' expected='{"CharacterComponentDefinition"}' actual='{behavior?.GetType().Name ?? "<missing>"}'");
		}
		for (int j = 0; j < Components.Count; j++)
		{
			CharacterComponentDefinition characterComponentDefinition = Components[j];
			if (characterComponentDefinition == null)
			{
				graphValid = false;
			}
			else
			{
				MergeDefinition(definitions, instanceIndices, characterComponentDefinition, ref graphValid);
			}
		}
	}

	private static void MergeDefinition(List<CharacterComponentDefinition> definitions, System.Collections.Generic.Dictionary<string, int> instanceIndices, CharacterComponentDefinition definition, ref bool graphValid)
	{
		string text = definition.InstanceId?.Trim();
		int value;
		if (string.IsNullOrEmpty(text))
		{
			graphValid = false;
			GD.PushError("Character component definition '" + definition.ResourcePath + "' has no stable InstanceId.");
		}
		else if (instanceIndices.TryGetValue(text, out value))
		{
			if (!CanReplaceInheritedDefinition(definitions[value], definition))
			{
				graphValid = false;
			}
			else
			{
				definitions[value] = definition;
			}
		}
		else
		{
			instanceIndices[text] = definitions.Count;
			definitions.Add(definition);
		}
	}

	private static System.Collections.Generic.Dictionary<string, int> BuildInstanceIndex(List<CharacterComponentDefinition> definitions)
	{
		System.Collections.Generic.Dictionary<string, int> dictionary = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
		for (int i = 0; i < definitions.Count; i++)
		{
			string text = definitions[i]?.InstanceId?.Trim();
			if (!string.IsNullOrEmpty(text))
			{
				dictionary[text] = i;
			}
		}
		return dictionary;
	}

	private static bool CanReplaceInheritedDefinition(CharacterComponentDefinition inherited, CharacterComponentDefinition replacement)
	{
		if (inherited == null)
		{
			return true;
		}
		bool flag = string.Equals(inherited.ComponentTypeId?.Trim(), replacement.ComponentTypeId?.Trim(), StringComparison.Ordinal);
		bool flag2 = inherited.WireIndex == replacement.WireIndex;
		if (flag & flag2)
		{
			return true;
		}
		GD.PushError($"Character component override '{replacement.InstanceId}' must preserve ComponentTypeId '{inherited.ComponentTypeId}' and WireIndex {inherited.WireIndex}.");
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.InvalidateFlattenedDefinitions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateCreationPlan, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComputeCreationPlanGraphRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFlattenedDefinitionArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComputeFlattenedSignature, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasInheritanceCycle, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasRegistryBehaviorIds, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MixSignature, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "signature", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanReplaceInheritedDefinition, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inherited", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "replacement", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.InvalidateFlattenedDefinitions && args.Count == 0)
		{
			InvalidateFlattenedDefinitions();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateCreationPlan && args.Count == 0)
		{
			InvalidateCreationPlan();
			ret = default;
			return true;
		}
		if (method == MethodName.ComputeCreationPlanGraphRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(ComputeCreationPlanGraphRevision());
			return true;
		}
		if (method == MethodName.GetFlattenedDefinitionArray && args.Count == 0)
		{
			Array<CharacterComponentDefinition> flattenedDefinitionArray = GetFlattenedDefinitionArray();
			ret = VariantUtils.CreateFromArray(flattenedDefinitionArray);
			return true;
		}
		if (method == MethodName.ComputeFlattenedSignature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(ComputeFlattenedSignature());
			return true;
		}
		if (method == MethodName.HasInheritanceCycle && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasInheritanceCycle());
			return true;
		}
		if (method == MethodName.HasRegistryBehaviorIds && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasRegistryBehaviorIds());
			return true;
		}
		if (method == MethodName.MixSignature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<ulong>(MixSignature(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1])));
			return true;
		}
		if (method == MethodName.CanReplaceInheritedDefinition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanReplaceInheritedDefinition(VariantUtils.ConvertTo<CharacterComponentDefinition>(in args[0]), VariantUtils.ConvertTo<CharacterComponentDefinition>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.MixSignature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<ulong>(MixSignature(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1])));
			return true;
		}
		if (method == MethodName.CanReplaceInheritedDefinition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanReplaceInheritedDefinition(VariantUtils.ConvertTo<CharacterComponentDefinition>(in args[0]), VariantUtils.ConvertTo<CharacterComponentDefinition>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.InvalidateFlattenedDefinitions)
		{
			return true;
		}
		if (method == MethodName.InvalidateCreationPlan)
		{
			return true;
		}
		if (method == MethodName.ComputeCreationPlanGraphRevision)
		{
			return true;
		}
		if (method == MethodName.GetFlattenedDefinitionArray)
		{
			return true;
		}
		if (method == MethodName.ComputeFlattenedSignature)
		{
			return true;
		}
		if (method == MethodName.HasInheritanceCycle)
		{
			return true;
		}
		if (method == MethodName.HasRegistryBehaviorIds)
		{
			return true;
		}
		if (method == MethodName.MixSignature)
		{
			return true;
		}
		if (method == MethodName.CanReplaceInheritedDefinition)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ParentSet)
		{
			ParentSet = VariantUtils.ConvertTo<CharacterComponentSet>(in value);
			return true;
		}
		if (name == PropertyName.BehaviorIds)
		{
			BehaviorIds = VariantUtils.ConvertToArray<StringName>(in value);
			return true;
		}
		if (name == PropertyName.Components)
		{
			Components = VariantUtils.ConvertToArray<CharacterComponentDefinition>(in value);
			return true;
		}
		if (name == PropertyName.RemovedInstanceIds)
		{
			RemovedInstanceIds = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName._parentSet)
		{
			_parentSet = VariantUtils.ConvertTo<CharacterComponentSet>(in value);
			return true;
		}
		if (name == PropertyName._behaviorIds)
		{
			_behaviorIds = VariantUtils.ConvertToArray<StringName>(in value);
			return true;
		}
		if (name == PropertyName._components)
		{
			_components = VariantUtils.ConvertToArray<CharacterComponentDefinition>(in value);
			return true;
		}
		if (name == PropertyName._removedInstanceIds)
		{
			_removedInstanceIds = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName._flattenedDefinitionsSignature)
		{
			_flattenedDefinitionsSignature = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._flattenedGraphRevision)
		{
			_flattenedGraphRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._flattenedRegistryRevision)
		{
			_flattenedRegistryRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._flattenedUsesRegistry)
		{
			_flattenedUsesRegistry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._creationPlanGraphRevision)
		{
			_creationPlanGraphRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._creationPlanRegistryRevision)
		{
			_creationPlanRegistryRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._creationPlanUsesRegistry)
		{
			_creationPlanUsesRegistry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._creationPlanInvalidationRevision)
		{
			_creationPlanInvalidationRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ParentSet)
		{
			value = VariantUtils.CreateFrom<CharacterComponentSet>(ParentSet);
			return true;
		}
		if (name == PropertyName.BehaviorIds)
		{
			value = VariantUtils.CreateFromArray(BehaviorIds);
			return true;
		}
		if (name == PropertyName.Components)
		{
			value = VariantUtils.CreateFromArray(Components);
			return true;
		}
		if (name == PropertyName.RemovedInstanceIds)
		{
			value = VariantUtils.CreateFromArray(RemovedInstanceIds);
			return true;
		}
		if (name == PropertyName._parentSet)
		{
			value = VariantUtils.CreateFrom(in _parentSet);
			return true;
		}
		if (name == PropertyName._behaviorIds)
		{
			value = VariantUtils.CreateFromArray(_behaviorIds);
			return true;
		}
		if (name == PropertyName._components)
		{
			value = VariantUtils.CreateFromArray(_components);
			return true;
		}
		if (name == PropertyName._removedInstanceIds)
		{
			value = VariantUtils.CreateFromArray(_removedInstanceIds);
			return true;
		}
		if (name == PropertyName._flattenedDefinitionsSignature)
		{
			value = VariantUtils.CreateFrom(in _flattenedDefinitionsSignature);
			return true;
		}
		if (name == PropertyName._flattenedGraphRevision)
		{
			value = VariantUtils.CreateFrom(in _flattenedGraphRevision);
			return true;
		}
		if (name == PropertyName._flattenedRegistryRevision)
		{
			value = VariantUtils.CreateFrom(in _flattenedRegistryRevision);
			return true;
		}
		if (name == PropertyName._flattenedUsesRegistry)
		{
			value = VariantUtils.CreateFrom(in _flattenedUsesRegistry);
			return true;
		}
		if (name == PropertyName._creationPlanGraphRevision)
		{
			value = VariantUtils.CreateFrom(in _creationPlanGraphRevision);
			return true;
		}
		if (name == PropertyName._creationPlanRegistryRevision)
		{
			value = VariantUtils.CreateFrom(in _creationPlanRegistryRevision);
			return true;
		}
		if (name == PropertyName._creationPlanUsesRegistry)
		{
			value = VariantUtils.CreateFrom(in _creationPlanUsesRegistry);
			return true;
		}
		if (name == PropertyName._creationPlanInvalidationRevision)
		{
			value = VariantUtils.CreateFrom(in _creationPlanInvalidationRevision);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._parentSet, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._behaviorIds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._components, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._removedInstanceIds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ParentSet, PropertyHint.ResourceType, "CharacterComponentSet", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.BehaviorIds, PropertyHint.TypeString, "21/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.Components, PropertyHint.TypeString, "24/17:CharacterComponentDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.RemovedInstanceIds, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._flattenedDefinitionsSignature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._flattenedGraphRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._flattenedRegistryRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._flattenedUsesRegistry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._creationPlanGraphRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._creationPlanRegistryRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._creationPlanUsesRegistry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._creationPlanInvalidationRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ParentSet, Variant.From<CharacterComponentSet>(ParentSet));
		info.AddProperty(PropertyName.BehaviorIds, Variant.CreateFrom(BehaviorIds));
		info.AddProperty(PropertyName.Components, Variant.CreateFrom(Components));
		info.AddProperty(PropertyName.RemovedInstanceIds, Variant.CreateFrom(RemovedInstanceIds));
		info.AddProperty(PropertyName._parentSet, Variant.From(in _parentSet));
		info.AddProperty(PropertyName._behaviorIds, Variant.CreateFrom(_behaviorIds));
		info.AddProperty(PropertyName._components, Variant.CreateFrom(_components));
		info.AddProperty(PropertyName._removedInstanceIds, Variant.CreateFrom(_removedInstanceIds));
		info.AddProperty(PropertyName._flattenedDefinitionsSignature, Variant.From(in _flattenedDefinitionsSignature));
		info.AddProperty(PropertyName._flattenedGraphRevision, Variant.From(in _flattenedGraphRevision));
		info.AddProperty(PropertyName._flattenedRegistryRevision, Variant.From(in _flattenedRegistryRevision));
		info.AddProperty(PropertyName._flattenedUsesRegistry, Variant.From(in _flattenedUsesRegistry));
		info.AddProperty(PropertyName._creationPlanGraphRevision, Variant.From(in _creationPlanGraphRevision));
		info.AddProperty(PropertyName._creationPlanRegistryRevision, Variant.From(in _creationPlanRegistryRevision));
		info.AddProperty(PropertyName._creationPlanUsesRegistry, Variant.From(in _creationPlanUsesRegistry));
		info.AddProperty(PropertyName._creationPlanInvalidationRevision, Variant.From(in _creationPlanInvalidationRevision));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ParentSet, out var value))
		{
			ParentSet = value.As<CharacterComponentSet>();
		}
		if (info.TryGetProperty(PropertyName.BehaviorIds, out var value2))
		{
			BehaviorIds = value2.AsGodotArray<StringName>();
		}
		if (info.TryGetProperty(PropertyName.Components, out var value3))
		{
			Components = value3.AsGodotArray<CharacterComponentDefinition>();
		}
		if (info.TryGetProperty(PropertyName.RemovedInstanceIds, out var value4))
		{
			RemovedInstanceIds = value4.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName._parentSet, out var value5))
		{
			_parentSet = value5.As<CharacterComponentSet>();
		}
		if (info.TryGetProperty(PropertyName._behaviorIds, out var value6))
		{
			_behaviorIds = value6.AsGodotArray<StringName>();
		}
		if (info.TryGetProperty(PropertyName._components, out var value7))
		{
			_components = value7.AsGodotArray<CharacterComponentDefinition>();
		}
		if (info.TryGetProperty(PropertyName._removedInstanceIds, out var value8))
		{
			_removedInstanceIds = value8.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName._flattenedDefinitionsSignature, out var value9))
		{
			_flattenedDefinitionsSignature = value9.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._flattenedGraphRevision, out var value10))
		{
			_flattenedGraphRevision = value10.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._flattenedRegistryRevision, out var value11))
		{
			_flattenedRegistryRevision = value11.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._flattenedUsesRegistry, out var value12))
		{
			_flattenedUsesRegistry = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._creationPlanGraphRevision, out var value13))
		{
			_creationPlanGraphRevision = value13.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._creationPlanRegistryRevision, out var value14))
		{
			_creationPlanRegistryRevision = value14.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._creationPlanUsesRegistry, out var value15))
		{
			_creationPlanUsesRegistry = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._creationPlanInvalidationRevision, out var value16))
		{
			_creationPlanInvalidationRevision = value16.As<ulong>();
		}
	}
}
