using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Behavior/TowerDefenseBehaviorRegistry.cs")]
public class TowerDefenseBehaviorRegistry : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName RegisterInit = "RegisterInit";

		public static readonly StringName RegisterBehavior = "RegisterBehavior";

		public static readonly StringName UnregisterBehavior = "UnregisterBehavior";

		public static readonly StringName GetBehavior = "GetBehavior";

		public static readonly StringName HasBehavior = "HasBehavior";

		public static readonly StringName GetBehaviorNames = "GetBehaviorNames";

		public static readonly StringName SafeLoad = "SafeLoad";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private static readonly object Gate = new object();

	private static Json _registryJson;

	private static bool _isInitialized;

	private static ulong _revision;

	private static readonly System.Collections.Generic.Dictionary<StringName, Resource> BehaviorDictionary = new System.Collections.Generic.Dictionary<StringName, Resource>();

	private static Json RegistryJson => _registryJson ?? (_registryJson = GD.Load<Json>("res://Registry/Behavior/BehaviorRegistry.json"));

	public static ulong Revision
	{
		get
		{
			Init();
			lock (Gate)
			{
				return _revision;
			}
		}
	}

	public static void Init()
	{
		lock (Gate)
		{
			if (!_isInitialized)
			{
				_isInitialized = true;
				RegisterInit();
			}
		}
	}

	private static void RegisterInit()
	{
		if (!GodotObject.IsInstanceValid(RegistryJson) || RegistryJson.Data.VariantType != Variant.Type.Dictionary)
		{
			return;
		}
		Dictionary dictionary = RegistryJson.Data.AsGodotDictionary().GetValueOrDefault("Behaviors", new Dictionary()).AsGodotDictionary();
		foreach (Variant key in dictionary.Keys)
		{
			StringName id = key.AsStringName();
			Resource resource = SafeLoad(dictionary[key].AsString(), id);
			if (GodotObject.IsInstanceValid(resource))
			{
				RegisterBehavior(id, resource);
			}
		}
	}

	public static bool RegisterBehavior(StringName id, Resource definition)
	{
		if (id == null || id.IsEmpty || !GodotObject.IsInstanceValid(definition))
		{
			GD.PushError("[BehaviorRegistry:E_REGISTER_INVALID] Behavior id and definition are required.");
			return false;
		}
		lock (Gate)
		{
			if (definition is BehaviorDefinitionBase behaviorDefinitionBase && (behaviorDefinitionBase.DefinitionId == null || behaviorDefinitionBase.DefinitionId.IsEmpty))
			{
				behaviorDefinitionBase.DefinitionId = id;
			}
			else if (definition is CharacterComponentDefinition characterComponentDefinition && string.IsNullOrWhiteSpace(characterComponentDefinition.DefinitionId))
			{
				characterComponentDefinition.DefinitionId = id.ToString();
			}
			BehaviorDictionary[id] = definition;
			_revision++;
			return true;
		}
	}

	public static bool RegisterBehavior(BehaviorDefinitionBase definition)
	{
		if (GodotObject.IsInstanceValid(definition))
		{
			return RegisterBehavior(definition.DefinitionId, definition);
		}
		return false;
	}

	public static bool RegisterBehavior(CharacterComponentDefinition definition)
	{
		if (GodotObject.IsInstanceValid(definition) && !string.IsNullOrWhiteSpace(definition.DefinitionId))
		{
			return RegisterBehavior(new StringName(definition.DefinitionId), definition);
		}
		return false;
	}

	public static bool UnregisterBehavior(StringName id)
	{
		if (id == null || id.IsEmpty)
		{
			return false;
		}
		lock (Gate)
		{
			if (!BehaviorDictionary.Remove(id))
			{
				return false;
			}
			_revision++;
			return true;
		}
	}

	public static Resource GetBehavior(StringName id)
	{
		Init();
		if (id == null || id.IsEmpty)
		{
			return null;
		}
		lock (Gate)
		{
			return BehaviorDictionary.GetValueOrDefault(id);
		}
	}

	public static T GetBehavior<T>(StringName id) where T : Resource
	{
		return GetBehavior(id) as T;
	}

	public static bool HasBehavior(StringName id)
	{
		Init();
		if (id == null || id.IsEmpty)
		{
			return false;
		}
		lock (Gate)
		{
			return BehaviorDictionary.ContainsKey(id);
		}
	}

	public static Array<StringName> GetBehaviorNames()
	{
		Init();
		Array<StringName> array = new Array<StringName>();
		lock (Gate)
		{
			foreach (StringName key in BehaviorDictionary.Keys)
			{
				array.Add(key);
			}
			return array;
		}
	}

	public static IEnumerable<T> Resolve<T>(Array<StringName> behaviorIds, IEnumerable<T> inlineDefinitions) where T : Resource
	{
		if (behaviorIds != null)
		{
			for (int index = 0; index < behaviorIds.Count; index++)
			{
				StringName stringName = behaviorIds[index];
				Resource behavior = GetBehavior(stringName);
				if (behavior is T val)
				{
					yield return val;
					continue;
				}
				GD.PushError($"[BehaviorRegistry:E_TYPE_MISMATCH] id='{stringName}' expected='{typeof(T).Name}' actual='{behavior?.GetType().Name ?? "<missing>"}'");
			}
		}
		if (inlineDefinitions == null)
		{
			yield break;
		}
		foreach (T inlineDefinition in inlineDefinitions)
		{
			yield return inlineDefinition;
		}
	}

	private static Resource SafeLoad(string path, StringName id)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return null;
		}
		try
		{
			return ResourceLoader.Load(path, "", ResourceLoader.CacheMode.Reuse);
		}
		catch (Exception ex)
		{
			GD.PushError($"[BehaviorRegistry:E_LOAD] id='{id}' path='{path}' reason='{ex.Message}'");
			return null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterBehavior, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterBehavior, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UnregisterBehavior, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetBehavior, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasBehavior, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetBehaviorNames, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.SafeLoad, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterBehavior && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(RegisterBehavior(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
		if (method == MethodName.RegisterBehavior && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RegisterBehavior(VariantUtils.ConvertTo<BehaviorDefinitionBase>(in args[0])));
			return true;
		}
		if (method == MethodName.UnregisterBehavior && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(UnregisterBehavior(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetBehavior && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(GetBehavior(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.HasBehavior && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasBehavior(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetBehaviorNames && args.Count == 0)
		{
			Array<StringName> behaviorNames = GetBehaviorNames();
			ret = VariantUtils.CreateFromArray(behaviorNames);
			return true;
		}
		if (method == MethodName.SafeLoad && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Resource>(SafeLoad(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterBehavior && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(RegisterBehavior(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1])));
			return true;
		}
		if (method == MethodName.RegisterBehavior && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RegisterBehavior(VariantUtils.ConvertTo<BehaviorDefinitionBase>(in args[0])));
			return true;
		}
		if (method == MethodName.UnregisterBehavior && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(UnregisterBehavior(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetBehavior && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(GetBehavior(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.HasBehavior && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasBehavior(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetBehaviorNames && args.Count == 0)
		{
			Array<StringName> behaviorNames = GetBehaviorNames();
			ret = VariantUtils.CreateFromArray(behaviorNames);
			return true;
		}
		if (method == MethodName.SafeLoad && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Resource>(SafeLoad(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.RegisterInit)
		{
			return true;
		}
		if (method == MethodName.RegisterBehavior)
		{
			return true;
		}
		if (method == MethodName.UnregisterBehavior)
		{
			return true;
		}
		if (method == MethodName.GetBehavior)
		{
			return true;
		}
		if (method == MethodName.HasBehavior)
		{
			return true;
		}
		if (method == MethodName.GetBehaviorNames)
		{
			return true;
		}
		if (method == MethodName.SafeLoad)
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
