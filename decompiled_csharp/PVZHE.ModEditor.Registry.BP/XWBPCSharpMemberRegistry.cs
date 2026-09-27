using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Registry.BP;

[ScriptPath("res://addons/ModEditor/Registry/BP/XWBPCSharpMemberRegistry.cs")]
public sealed class XWBPCSharpMemberRegistry : RefCounted
{
	private sealed class TypeResolutionContext
	{
		public string NamespaceName = "";

		public readonly List<string> Usings = new List<string>();

		public readonly System.Collections.Generic.Dictionary<string, string> Aliases = new System.Collections.Generic.Dictionary<string, string>(StringComparer.Ordinal);
	}

	private sealed class TypeInfo
	{
		public int Type;

		public string ClassName = "";

		public string CsTypeName = "";

		public string SourceType = "";

		public TypeResolutionContext ResolutionContext;
	}

	private sealed class ArgumentInfo
	{
		public string Name = "";

		public TypeInfo Type = new TypeInfo();
	}

	private sealed class MethodInfo
	{
		public string Name = "";

		public string CsName = "";

		public string BaseClassName = "";

		public string CsClassName = "";

		public string CsQualifiedClassName = "";

		public string SourcePath = "";

		public int Flags;

		public bool IsAbstract;

		public bool IsStatic;

		public TypeInfo Return = new TypeInfo();

		public readonly List<ArgumentInfo> Arguments = new List<ArgumentInfo>();
	}

	private sealed class PropertyInfo
	{
		public string Name = "";

		public string CsName = "";

		public string BaseClassName = "";

		public string CsClassName = "";

		public string CsQualifiedClassName = "";

		public string SourcePath = "";

		public TypeInfo Type = new TypeInfo();

		public int Usage;

		public bool HasGetter;

		public bool HasSetter;

		public bool IsStatic;
	}

	private sealed class SignalInfo
	{
		public string Name = "";

		public string CsName = "";

		public string BaseClassName = "";

		public string CsClassName = "";

		public string CsQualifiedClassName = "";

		public string SourcePath = "";

		public readonly List<ArgumentInfo> Arguments = new List<ArgumentInfo>();
	}

	private sealed class ClassInfo
	{
		public string Name = "";

		public string QualifiedName = "";

		public string BaseClass = "";

		public string RawBaseClass = "";

		public string ScriptPath = "";

		public string NamespaceName = "";

		public TypeResolutionContext BaseResolutionContext;

		public bool IsStatic;

		public bool IsSealed;

		public bool IsAbstract;

		public bool IsGeneric;

		public bool HasExplicitConstructor;

		public bool HasAccessibleParameterlessConstructor;

		public readonly List<MethodInfo> Methods = new List<MethodInfo>();

		public readonly List<PropertyInfo> Properties = new List<PropertyInfo>();

		public readonly List<SignalInfo> Signals = new List<SignalInfo>();
	}

	private sealed class ScanSnapshot
	{
		public string Root = "";

		public int ContextVersion;

		public int FilesScanned;

		public string Warning = "";

		public System.Collections.Generic.Dictionary<string, ClassInfo> Classes = new System.Collections.Generic.Dictionary<string, ClassInfo>(StringComparer.Ordinal);

		public HashSet<string> KnownTypes = new HashSet<string>(StringComparer.Ordinal);
	}

	public sealed class CSharpClassDescriptor
	{
		public string Name { get; init; } = "";

		public string QualifiedName { get; init; } = "";

		public string BaseClass { get; init; } = "";

		public string ScriptPath { get; init; } = "";

		public bool IsProjectClass { get; init; }

		public bool CanInherit { get; init; }

		public string InheritanceBlockReason { get; init; } = "";
	}

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName ConfigureProjectRoot = "ConfigureProjectRoot";

		public static readonly StringName NotifyScriptSaved = "NotifyScriptSaved";

		public static readonly StringName RefreshCurrentProject = "RefreshCurrentProject";

		public static readonly StringName HasClass = "HasClass";

		public static readonly StringName IsProjectClass = "IsProjectClass";

		public static readonly StringName GetExternalBaseClassName = "GetExternalBaseClassName";

		public static readonly StringName DoesClassImplementMethod = "DoesClassImplementMethod";

		public static readonly StringName EnsureScanned = "EnsureScanned";

		public static readonly StringName EnsureClassSourceScanned = "EnsureClassSourceScanned";

		public static readonly StringName EnsureHostScanned = "EnsureHostScanned";

		public static readonly StringName EnsureProjectScanned = "EnsureProjectScanned";

		public static readonly StringName ShouldSkip = "ShouldSkip";

		public static readonly StringName FindNamespace = "FindNamespace";

		public static readonly StringName CleanBaseClass = "CleanBaseClass";

		public static readonly StringName BuildMemberIdentity = "BuildMemberIdentity";

		public static readonly StringName ShouldSkipMethod = "ShouldSkipMethod";

		public static readonly StringName ShouldSkipProperty = "ShouldSkipProperty";

		public static readonly StringName IsVirtual = "IsVirtual";

		public static readonly StringName CreateArgumentDictionary = "CreateArgumentDictionary";

		public static readonly StringName CreateTypeDictionary = "CreateTypeDictionary";

		public static readonly StringName PrefixGlobal = "PrefixGlobal";

		public static readonly StringName NormalizeBaseClassForLookup = "NormalizeBaseClassForLookup";

		public static readonly StringName IsBlueprintSupportedType = "IsBlueprintSupportedType";

		public static readonly StringName NormalizeTypeName = "NormalizeTypeName";

		public static readonly StringName StripQualifiedTypeName = "StripQualifiedTypeName";

		public static readonly StringName FindMatchingBrace = "FindMatchingBrace";

		public static readonly StringName ResolveHostRoot = "ResolveHostRoot";

		public static readonly StringName NormalizeRoot = "NormalizeRoot";

		public static readonly StringName IsCSharpPathInsideProject = "IsCSharpPathInsideProject";

		public static readonly StringName AttachProjectWatcher = "AttachProjectWatcher";

		public static readonly StringName DetachProjectWatcher = "DetachProjectWatcher";

		public static readonly StringName ScheduleProjectWatcherRefresh = "ScheduleProjectWatcherRefresh";

		public static readonly StringName InvalidateHostContext = "InvalidateHostContext";

		public static readonly StringName InvalidateProjectContext = "InvalidateProjectContext";

		public static readonly StringName InvalidateProjectContextLocked = "InvalidateProjectContextLocked";

		public static readonly StringName ResetInstance = "ResetInstance";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName ActiveProjectRoot = "ActiveProjectRoot";

		public static readonly StringName HostClassCount = "HostClassCount";

		public static readonly StringName HostScanGeneration = "HostScanGeneration";

		public static readonly StringName IsHostScanReady = "IsHostScanReady";

		public static readonly StringName LastHostScanWarning = "LastHostScanWarning";

		public static readonly StringName ProjectClassCount = "ProjectClassCount";

		public static readonly StringName ProjectScanGeneration = "ProjectScanGeneration";

		public static readonly StringName ProjectContextVersion = "ProjectContextVersion";

		public static readonly StringName ProjectFilesScanned = "ProjectFilesScanned";

		public static readonly StringName IsProjectScanReady = "IsProjectScanReady";

		public static readonly StringName LastProjectScanWarning = "LastProjectScanWarning";

		public static readonly StringName _hostDirty = "_hostDirty";

		public static readonly StringName _hostRoot = "_hostRoot";

		public static readonly StringName _hostContextVersion = "_hostContextVersion";

		public static readonly StringName _hostScanGeneration = "_hostScanGeneration";

		public static readonly StringName _projectDirty = "_projectDirty";

		public static readonly StringName _projectRoot = "_projectRoot";

		public static readonly StringName _projectContextVersion = "_projectContextVersion";

		public static readonly StringName _projectScanGeneration = "_projectScanGeneration";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private const int MethodFlagVirtual = 64;

	private const int PropertyUsageStorage = 2;

	private const int VariantTypeNil = 0;

	private const int VariantTypeBool = 1;

	private const int VariantTypeInt = 2;

	private const int VariantTypeFloat = 3;

	private const int VariantTypeString = 4;

	private const int VariantTypeVector2 = 5;

	private const int VariantTypeVector2I = 6;

	private const int VariantTypeRect2 = 7;

	private const int VariantTypeRect2I = 8;

	private const int VariantTypeVector3 = 9;

	private const int VariantTypeVector3I = 10;

	private const int VariantTypeTransform2D = 11;

	private const int VariantTypeVector4 = 12;

	private const int VariantTypeVector4I = 13;

	private const int VariantTypePlane = 14;

	private const int VariantTypeQuaternion = 15;

	private const int VariantTypeAabb = 16;

	private const int VariantTypeBasis = 17;

	private const int VariantTypeTransform3D = 18;

	private const int VariantTypeProjection = 19;

	private const int VariantTypeColor = 20;

	private const int VariantTypeStringName = 21;

	private const int VariantTypeNodePath = 22;

	private const int VariantTypeRid = 23;

	private const int VariantTypeObject = 24;

	private const int VariantTypeCallable = 25;

	private const int VariantTypeSignal = 26;

	private const int VariantTypeDictionary = 27;

	private const int VariantTypeArray = 28;

	private static readonly string[] SkippedDirectoryNames = new string[5] { "bin", "obj", ".godot", ".godot_ide", "generated" };

	private static readonly Regex NamespaceRegex = new Regex("(?m)^\\s*namespace\\s+([A-Za-z_][A-Za-z0-9_.]*)\\s*[;{]?", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex UsingRegex = new Regex("(?m)^\\s*(?:global\\s+)?using\\s+(?!static\\b)(?:(?<alias>[A-Za-z_][A-Za-z0-9_]*)\\s*=\\s*)?(?<target>(?:global::)?[A-Za-z_][A-Za-z0-9_.]*)\\s*;", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex ClassRegex = new Regex("(?m)^\\s*(?:(?:\\[[^\\]\\r\\n]+\\]\\s*)+)?\\s*(?<mods>(?:(?:public|internal|private|protected|static|abstract|sealed|partial|unsafe|new)\\s+)*)class\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)(?<generic>\\s*<[^{\\r\\n>]+>)?(?:\\s*:\\s*(?<base>[^{\\r\\n]+))?", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex TypeDeclarationRegex = new Regex("(?m)^\\s*(?:(?:\\[[^\\]\\r\\n]+\\]\\s*)+)?\\s*(?:(?:public|internal|private|protected|static|abstract|sealed|partial|readonly|unsafe|new|ref)\\s+)*(?:class|struct|interface|enum)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex MethodRegex = new Regex("(?m)^\\s*(?:(?:\\[[^\\]\\r\\n]+\\]\\s*)+)?\\s*public\\s+(?<mods>(?:(?:static|override|virtual|abstract|async|sealed|new|unsafe|extern|partial)\\s+)*)?(?<return>[A-Za-z_][A-Za-z0-9_<>.,\\[\\]? ]*)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*\\((?<args>[^\\)]*)\\)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex PropertyRegex = new Regex("(?m)^\\s*(?<attrs>(?:(?:\\[[^\\]\\r\\n]+\\]\\s*)+)?)\\s*public\\s+(?<mods>(?:(?:static|override|virtual|readonly|const|new|required)\\s+)*)?(?<type>[A-Za-z_][A-Za-z0-9_<>.,\\[\\]? ]*)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*(?<tail>[{=;])", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex SignalRegex = new Regex("(?m)^\\s*\\[Signal\\]\\s*public\\s+delegate\\s+void\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*\\((?<args>[^\\)]*)\\)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex ArgumentAttributeRegex = new Regex("\\[[^\\]]+\\]\\s*", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex ArgumentModifierRegex = new Regex("\\b(params|ref|out|in|this)\\b\\s*", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex ArgumentNameRegex = new Regex("(?<type>.+?)\\s+(?<name>@?[A-Za-z_][A-Za-z0-9_]*)$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex TypeModifierRegex = new Regex("\\b(public|private|protected|internal|static|readonly|const|required|volatile)\\b\\s*", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex PropertyAccessorRegex = new Regex("(?m)(?<access>(?:(?:public|private|protected|internal)\\s+)*)?(?<kind>get|set|init)\\s*(?:;|=>|\\{)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly Regex UnsupportedPassingModeRegex = new Regex("\\b(ref|out|in)\\b", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	private static readonly HashSet<string> SystemTypeNames = new HashSet<string>(StringComparer.Ordinal)
	{
		"Array", "Attribute", "BitConverter", "Boolean", "Byte", "Char", "Console", "Convert", "DateOnly", "DateTime",
		"DateTimeOffset", "Decimal", "Delegate", "Double", "Enum", "Environment", "EventArgs", "Exception", "Guid", "Int16",
		"Int32", "Int64", "Math", "MathF", "Object", "Random", "SByte", "Single", "String", "StringComparer",
		"TimeOnly", "TimeSpan", "Tuple", "Type", "UInt16", "UInt32", "UInt64", "Uri", "ValueTuple", "Version",
		"WeakReference"
	};

	private static readonly HashSet<string> GenericCollectionTypeNames = new HashSet<string>(StringComparer.Ordinal)
	{
		"Collection", "Comparer", "Dictionary", "EqualityComparer", "HashSet", "ICollection", "IComparer", "IDictionary", "IEnumerable", "IEnumerator",
		"IEqualityComparer", "IList", "IReadOnlyCollection", "IReadOnlyDictionary", "IReadOnlyList", "KeyValuePair", "LinkedList", "List", "Queue", "SortedDictionary",
		"SortedList", "SortedSet", "Stack"
	};

	private static readonly HashSet<string> GodotObjectTypeNames = new HashSet<string>(StringComparer.Ordinal)
	{
		"Animation", "AnimationLibrary", "AnimationPlayer", "Area2D", "Area3D", "Button", "Camera2D", "Camera3D", "CanvasItem", "CharacterBody2D",
		"CharacterBody3D", "CollisionObject2D", "CollisionObject3D", "Control", "Engine", "Font", "GodotObject", "Image", "ImageTexture", "InputEvent",
		"Label", "Node", "Node2D", "Node3D", "PackedScene", "RefCounted", "Resource", "SceneTree", "Script", "Sprite2D",
		"Texture2D", "Timer", "Tween", "Viewport", "Window"
	};

	private static XWBPCSharpMemberRegistry _instance;

	private readonly object _hostSync = new object();

	private readonly object _projectSync = new object();

	private volatile bool _hostDirty = true;

	private string _hostRoot = "";

	private int _hostContextVersion;

	private int _hostScanGeneration;

	private ScanSnapshot _hostSnapshot = new ScanSnapshot();

	private Task<bool> _hostScanTask;

	private CancellationTokenSource _hostScanCancellation;

	private volatile bool _projectDirty = true;

	private string _projectRoot = "";

	private FileSystemWatcher _projectWatcher;

	private int _projectContextVersion;

	private int _projectScanGeneration;

	private ScanSnapshot _projectSnapshot = new ScanSnapshot();

	private Task<bool> _projectScanTask;

	private CancellationTokenSource _projectScanCancellation;

	private CancellationTokenSource _projectWatcherDebounceCancellation;

	public static XWBPCSharpMemberRegistry Instance
	{
		get
		{
			if (_instance == null || !GodotObject.IsInstanceValid(_instance))
			{
				_instance = new XWBPCSharpMemberRegistry();
			}
			return _instance;
		}
	}

	private System.Collections.Generic.Dictionary<string, ClassInfo> _projectClasses => GetProjectClassesSnapshot();

	private System.Collections.Generic.Dictionary<string, ClassInfo> _hostClasses => GetHostClassesSnapshot();

	public string ActiveProjectRoot
	{
		get
		{
			lock (_projectSync)
			{
				return _projectRoot;
			}
		}
	}

	public int HostClassCount => Volatile.Read(in _hostSnapshot)?.Classes.Count ?? 0;

	public int HostScanGeneration => Volatile.Read(in _hostScanGeneration);

	public bool IsHostScanReady
	{
		get
		{
			ScanSnapshot scanSnapshot = Volatile.Read(in _hostSnapshot);
			lock (_hostSync)
			{
				return !_hostDirty && scanSnapshot != null && scanSnapshot.ContextVersion == _hostContextVersion && string.Equals(scanSnapshot.Root, _hostRoot, StringComparison.OrdinalIgnoreCase);
			}
		}
	}

	public string LastHostScanWarning => Volatile.Read(in _hostSnapshot)?.Warning ?? "";

	public int ProjectClassCount => Volatile.Read(in _projectSnapshot)?.Classes.Count ?? 0;

	public int ProjectScanGeneration => Volatile.Read(in _projectScanGeneration);

	public int ProjectContextVersion => Volatile.Read(in _projectContextVersion);

	public int ProjectFilesScanned => Volatile.Read(in _projectSnapshot)?.FilesScanned ?? 0;

	public bool IsProjectScanReady
	{
		get
		{
			ScanSnapshot scanSnapshot = Volatile.Read(in _projectSnapshot);
			lock (_projectSync)
			{
				return !_projectDirty && scanSnapshot != null && scanSnapshot.ContextVersion == _projectContextVersion && string.Equals(scanSnapshot.Root, _projectRoot, StringComparison.OrdinalIgnoreCase);
			}
		}
	}

	public string LastProjectScanWarning => Volatile.Read(in _projectSnapshot)?.Warning ?? "";

	public XWBPCSharpMemberRegistry()
	{
		_hostRoot = ResolveHostRoot();
		_hostSnapshot = new ScanSnapshot
		{
			Root = _hostRoot,
			ContextVersion = _hostContextVersion
		};
	}

	public void Refresh()
	{
		InvalidateHostContext(clearSnapshot: true);
		InvalidateProjectContext(clearSnapshot: true);
	}

	public bool ConfigureProjectRoot(string projectRoot)
	{
		string text = NormalizeRoot(projectRoot);
		lock (_projectSync)
		{
			if (string.Equals(_projectRoot, text, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
		}
		DetachProjectWatcher();
		lock (_projectSync)
		{
			_projectRoot = text;
			InvalidateProjectContextLocked(clearSnapshot: true);
		}
		AttachProjectWatcher(text);
		return true;
	}

	public void NotifyScriptSaved(string scriptPath)
	{
		if (IsCSharpPathInsideProject(scriptPath))
		{
			InvalidateProjectContext();
			EnsureProjectScannedAsync();
		}
	}

	public void RefreshCurrentProject()
	{
		InvalidateProjectContext();
	}

	public async Task<bool> EnsureProjectScannedAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Task<bool> hostTask = EnsureHostScannedAsync(cancellationToken);
		Task<bool> projectTask = EnsureProjectSnapshotScannedAsync(cancellationToken);
		_003C_003Ey__InlineArray2<Task<bool>> buffer = default;
		buffer[0] = hostTask;
		buffer[1] = projectTask;
		await Task.WhenAll<bool>(buffer).ConfigureAwait(continueOnCapturedContext: false);
		return hostTask.Result && projectTask.Result;
	}

	public Task<bool> EnsureHostScannedAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Task<bool> hostScanTask;
		lock (_hostSync)
		{
			ScanSnapshot hostSnapshot = _hostSnapshot;
			if (!_hostDirty && hostSnapshot != null && hostSnapshot.ContextVersion == _hostContextVersion && string.Equals(hostSnapshot.Root, _hostRoot, StringComparison.OrdinalIgnoreCase))
			{
				return Task.FromResult(result: true);
			}
			if (_hostScanTask == null || _hostScanTask.IsCompleted)
			{
				string hostRoot = _hostRoot;
				int hostContextVersion = _hostContextVersion;
				_hostScanCancellation?.Dispose();
				_hostScanCancellation = new CancellationTokenSource();
				CancellationToken token = _hostScanCancellation.Token;
				_hostScanTask = RunHostScanAsync(hostRoot, hostContextVersion, token);
			}
			hostScanTask = _hostScanTask;
		}
		if (!cancellationToken.CanBeCanceled)
		{
			return hostScanTask;
		}
		return hostScanTask.WaitAsync(cancellationToken);
	}

	private Task<bool> EnsureProjectSnapshotScannedAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Task<bool> projectScanTask;
		lock (_projectSync)
		{
			ScanSnapshot projectSnapshot = _projectSnapshot;
			if (!_projectDirty && projectSnapshot != null && projectSnapshot.ContextVersion == _projectContextVersion && string.Equals(projectSnapshot.Root, _projectRoot, StringComparison.OrdinalIgnoreCase))
			{
				return Task.FromResult(result: true);
			}
			if (_projectScanTask == null || _projectScanTask.IsCompleted)
			{
				string projectRoot = _projectRoot;
				int projectContextVersion = _projectContextVersion;
				_projectScanCancellation?.Dispose();
				_projectScanCancellation = new CancellationTokenSource();
				CancellationToken token = _projectScanCancellation.Token;
				_projectScanTask = RunProjectScanAsync(projectRoot, projectContextVersion, token);
			}
			projectScanTask = _projectScanTask;
		}
		if (!cancellationToken.CanBeCanceled)
		{
			return projectScanTask;
		}
		return projectScanTask.WaitAsync(cancellationToken);
	}

	public Task<bool> RefreshCurrentProjectAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		RefreshCurrentProject();
		return EnsureProjectScannedAsync(cancellationToken);
	}

	public Task<bool> RefreshAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Refresh();
		return EnsureProjectScannedAsync(cancellationToken);
	}

	private async Task<bool> RunHostScanAsync(string rootSnapshot, int versionSnapshot, CancellationToken cancellationToken)
	{
		ScanSnapshot value;
		try
		{
			value = await Task.Run(() => ScanRoot(rootSnapshot, versionSnapshot, isProjectRoot: false, cancellationToken), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (OperationCanceledException)
		{
			return false;
		}
		lock (_hostSync)
		{
			if (cancellationToken.IsCancellationRequested || versionSnapshot != _hostContextVersion || !string.Equals(rootSnapshot, _hostRoot, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			Volatile.Write(ref _hostSnapshot, value);
			_hostDirty = false;
			Interlocked.Increment(ref _hostScanGeneration);
			return true;
		}
	}

	private async Task<bool> RunProjectScanAsync(string rootSnapshot, int versionSnapshot, CancellationToken cancellationToken)
	{
		ScanSnapshot value;
		try
		{
			value = await Task.Run(() => ScanRoot(rootSnapshot, versionSnapshot, isProjectRoot: true, cancellationToken), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (OperationCanceledException)
		{
			return false;
		}
		lock (_projectSync)
		{
			if (cancellationToken.IsCancellationRequested || versionSnapshot != _projectContextVersion || !string.Equals(rootSnapshot, _projectRoot, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			Volatile.Write(ref _projectSnapshot, value);
			_projectDirty = false;
			Interlocked.Increment(ref _projectScanGeneration);
			return true;
		}
	}

	private System.Collections.Generic.Dictionary<string, ClassInfo> GetProjectClassesSnapshot()
	{
		return Volatile.Read(in _projectSnapshot)?.Classes ?? new System.Collections.Generic.Dictionary<string, ClassInfo>(StringComparer.Ordinal);
	}

	private System.Collections.Generic.Dictionary<string, ClassInfo> GetHostClassesSnapshot()
	{
		return Volatile.Read(in _hostSnapshot)?.Classes ?? new System.Collections.Generic.Dictionary<string, ClassInfo>(StringComparer.Ordinal);
	}

	public bool HasClass(string className)
	{
		if (string.IsNullOrEmpty(className))
		{
			return false;
		}
		EnsureClassSourceScanned(className);
		ClassInfo info;
		return TryGetClass(className, out info);
	}

	public IReadOnlyCollection<string> GetAllClassNames()
	{
		EnsureScanned();
		HashSet<string> hashSet = new HashSet<string>(_hostClasses.Keys, StringComparer.Ordinal);
		hashSet.UnionWith(GetProjectClassesSnapshot().Keys);
		return hashSet.OrderBy((string name) => name, StringComparer.OrdinalIgnoreCase).ToArray();
	}

	public IReadOnlyList<CSharpClassDescriptor> GetProjectClassDescriptors()
	{
		EnsureProjectScanned();
		return GetProjectClassesSnapshot().Values.OrderBy((ClassInfo info) => info.Name, StringComparer.OrdinalIgnoreCase).Select((ClassInfo info) =>
		{
			string inheritanceBlockReason = GetInheritanceBlockReason(info);
			return new CSharpClassDescriptor
			{
				Name = info.Name,
				QualifiedName = info.QualifiedName,
				BaseClass = info.BaseClass,
				ScriptPath = info.ScriptPath,
				IsProjectClass = true,
				CanInherit = string.IsNullOrWhiteSpace(inheritanceBlockReason),
				InheritanceBlockReason = inheritanceBlockReason
			};
		}).ToArray();
	}

	public bool IsProjectClass(string className)
	{
		EnsureProjectScanned();
		ClassInfo info;
		if (!string.IsNullOrWhiteSpace(className))
		{
			return TryGetClass(GetProjectClassesSnapshot(), className, out info);
		}
		return false;
	}

	public string GetExternalBaseClassName(string className)
	{
		EnsureClassSourceScanned(className);
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		string text = className;
		while (!string.IsNullOrWhiteSpace(text) && hashSet.Add(text))
		{
			if (!TryGetClass(text, out var info))
			{
				return text;
			}
			text = info.BaseClass;
		}
		return "";
	}

	public List<Dictionary> GetClassMethodList(string className, bool includeBase = true, bool includeLifecycle = true)
	{
		EnsureClassSourceScanned(className);
		List<Dictionary> result = new List<Dictionary>();
		AppendClassChain(className, includeBase, (ClassInfo info) =>
		{
			result.AddRange(info.Methods.Select(ToGodotDictionary));
		});
		if (includeLifecycle)
		{
			AddLifecycleMethods(className, result);
		}
		RemoveDuplicateMembers(result);
		return result;
	}

	public List<Dictionary> GetClassPropertyList(string className, bool includeBase = true)
	{
		EnsureClassSourceScanned(className);
		List<Dictionary> result = new List<Dictionary>();
		AppendClassChain(className, includeBase, (ClassInfo info) =>
		{
			result.AddRange(info.Properties.Select(ToGodotDictionary));
		});
		RemoveDuplicateMembers(result);
		return result;
	}

	public List<Dictionary> GetClassSignalList(string className, bool includeBase = true)
	{
		EnsureClassSourceScanned(className);
		List<Dictionary> result = new List<Dictionary>();
		AppendClassChain(className, includeBase, (ClassInfo info) =>
		{
			result.AddRange(info.Signals.Select(ToGodotDictionary));
		});
		RemoveDuplicateMembers(result);
		return result;
	}

	public bool DoesClassImplementMethod(string className, string methodName)
	{
		foreach (Dictionary classMethod in GetClassMethodList(className))
		{
			string text = (classMethod.TryGetValue("name", out var value) ? value.AsString() : "");
			string text2 = (classMethod.TryGetValue("cs_name", out var value2) ? value2.AsString() : text);
			if (text == methodName || text2 == methodName)
			{
				return true;
			}
		}
		return false;
	}

	private void EnsureScanned()
	{
		EnsureHostScanned();
		EnsureProjectScanned();
	}

	private void EnsureClassSourceScanned(string className)
	{
		EnsureProjectScanned();
		if (!TryGetClass(GetProjectClassesSnapshot(), className, out var _))
		{
			EnsureHostScanned();
		}
	}

	private void EnsureHostScanned()
	{
	}

	private void EnsureProjectScanned()
	{
	}

	private static ScanSnapshot ScanRoot(string root, int contextVersion, bool isProjectRoot, CancellationToken cancellationToken)
	{
		ScanSnapshot scanSnapshot = new ScanSnapshot
		{
			Root = (root ?? ""),
			ContextVersion = contextVersion
		};
		if (string.IsNullOrWhiteSpace(root) || !Directory.Exists(root))
		{
			return scanSnapshot;
		}
		try
		{
			foreach (string item in Directory.EnumerateFiles(root, "*.cs", SearchOption.AllDirectories))
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (!ShouldSkip(item, root, isProjectRoot))
				{
					ScanFile(item, root, scanSnapshot.Classes, scanSnapshot.KnownTypes, isProjectRoot);
					scanSnapshot.FilesScanned++;
				}
			}
			ResolveSnapshotTypes(scanSnapshot.Classes, scanSnapshot.KnownTypes);
		}
		catch (Exception ex)
		{
			if (ex is OperationCanceledException)
			{
				throw;
			}
			scanSnapshot.Warning = "Blueprint C# API scan skipped part of '" + root + "': " + ex.Message;
		}
		return scanSnapshot;
	}

	private static bool ShouldSkip(string path, string root, bool isProjectRoot)
	{
		string text;
		try
		{
			text = Path.GetRelativePath(root, path).Replace('\\', '/');
		}
		catch
		{
			return true;
		}
		if (text.StartsWith("../", StringComparison.Ordinal) || text.Equals("..", StringComparison.Ordinal))
		{
			return true;
		}
		if (!isProjectRoot && text.StartsWith("addons/ModEditor/", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (text.EndsWith(".generated.cs", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		string[] array = text.Split('/', StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length - 1; i++)
		{
			string a = array[i];
			string[] skippedDirectoryNames = SkippedDirectoryNames;
			foreach (string b in skippedDirectoryNames)
			{
				if (string.Equals(a, b, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static void ScanFile(string path, string root, System.Collections.Generic.Dictionary<string, ClassInfo> target, HashSet<string> knownTypes, bool isProjectRoot)
	{
		string text;
		try
		{
			text = File.ReadAllText(path);
		}
		catch
		{
			return;
		}
		string text2 = FindNamespace(text);
		TypeResolutionContext typeResolutionContext = ParseTypeResolutionContext(text, text2);
		foreach (Match item in TypeDeclarationRegex.Matches(text))
		{
			string value = item.Groups["name"].Value;
			if (!string.IsNullOrWhiteSpace(value))
			{
				knownTypes.Add(string.IsNullOrEmpty(text2) ? value : (text2 + "." + value));
			}
		}
		string scriptPath = (isProjectRoot ? ("mod://" + Path.GetRelativePath(root, path).Replace('\\', '/')) : ("res://" + Path.GetRelativePath(root, path).Replace('\\', '/')));
		foreach (Match item2 in ClassRegex.Matches(text))
		{
			string value2 = item2.Groups["name"].Value;
			string qualifiedName = (string.IsNullOrEmpty(text2) ? value2 : (text2 + "." + value2));
			int num = text.IndexOf('{', item2.Index + item2.Length);
			if (num < 0)
			{
				continue;
			}
			int num2 = FindMatchingBrace(text, num);
			if (num2 > num)
			{
				ClassInfo orCreateClass = GetOrCreateClass(target, qualifiedName, value2);
				orCreateClass.QualifiedName = qualifiedName;
				orCreateClass.ScriptPath = scriptPath;
				orCreateClass.NamespaceName = text2;
				string value3 = item2.Groups["mods"].Value;
				orCreateClass.IsStatic |= value3.Contains("static", StringComparison.Ordinal);
				orCreateClass.IsSealed |= value3.Contains("sealed", StringComparison.Ordinal);
				orCreateClass.IsAbstract |= value3.Contains("abstract", StringComparison.Ordinal);
				orCreateClass.IsGeneric |= item2.Groups["generic"].Success;
				string text3 = CleanBaseClass(item2.Groups["base"].Value);
				if (!string.IsNullOrEmpty(text3) && string.IsNullOrEmpty(orCreateClass.BaseClass))
				{
					orCreateClass.BaseClass = text3;
					orCreateClass.RawBaseClass = text3;
					orCreateClass.BaseResolutionContext = typeResolutionContext;
				}
				string body = text.Substring(num + 1, num2 - num - 1);
				ReadConstructorCapabilities(body, value2, out var hasExplicitConstructor, out var hasAccessibleParameterlessConstructor);
				orCreateClass.HasExplicitConstructor |= hasExplicitConstructor;
				orCreateClass.HasAccessibleParameterlessConstructor |= hasAccessibleParameterlessConstructor;
				ScanMethods(body, orCreateClass, typeResolutionContext);
				ScanProperties(body, orCreateClass, typeResolutionContext);
				ScanSignals(body, orCreateClass, typeResolutionContext);
			}
		}
	}

	private static ClassInfo GetOrCreateClass(System.Collections.Generic.Dictionary<string, ClassInfo> target, string qualifiedName, string className)
	{
		if (!target.TryGetValue(qualifiedName, out var value))
		{
			value = (target[qualifiedName] = new ClassInfo
			{
				Name = className
			});
		}
		return value;
	}

	private static string FindNamespace(string source)
	{
		Match match = NamespaceRegex.Match(source);
		if (!match.Success)
		{
			return "";
		}
		return match.Groups[1].Value;
	}

	private static TypeResolutionContext ParseTypeResolutionContext(string source, string namespaceName)
	{
		TypeResolutionContext typeResolutionContext = new TypeResolutionContext
		{
			NamespaceName = (namespaceName ?? "")
		};
		foreach (Match item in UsingRegex.Matches(source ?? ""))
		{
			string text = item.Groups["target"].Value.Replace("global::", "", StringComparison.Ordinal).Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				continue;
			}
			string text2 = item.Groups["alias"].Value.Trim();
			if (string.IsNullOrWhiteSpace(text2))
			{
				if (!typeResolutionContext.Usings.Contains(text, StringComparer.Ordinal))
				{
					typeResolutionContext.Usings.Add(text);
				}
			}
			else
			{
				typeResolutionContext.Aliases[text2] = text;
			}
		}
		return typeResolutionContext;
	}

	private static string CleanBaseClass(string basePart)
	{
		if (string.IsNullOrWhiteSpace(basePart))
		{
			return "";
		}
		string text = SplitTopLevel(basePart, ',')[0].Trim();
		text = text.Replace("global::", "");
		if ((text == "object" || text == "System.Object") ? true : false)
		{
			return "System.Object";
		}
		int num = text.IndexOf('<');
		if (num >= 0)
		{
			text = text.Substring(0, num);
		}
		return text.Trim();
	}

	private static void ReadConstructorCapabilities(string body, string className, out bool hasExplicitConstructor, out bool hasAccessibleParameterlessConstructor)
	{
		hasExplicitConstructor = false;
		hasAccessibleParameterlessConstructor = false;
		if (string.IsNullOrWhiteSpace(body) || string.IsNullOrWhiteSpace(className))
		{
			return;
		}
		MatchCollection matchCollection = new Regex("(?m)^\\s*(?<access>(?:(?:public|private|protected|internal)\\s+)*)" + Regex.Escape(className) + "\\s*\\((?<args>[^\\)]*)\\)", RegexOptions.CultureInvariant).Matches(body);
		hasExplicitConstructor = matchCollection.Count > 0;
		foreach (Match item in matchCollection)
		{
			if (string.IsNullOrWhiteSpace(item.Groups["args"].Value))
			{
				string value = item.Groups["access"].Value;
				if (value.Contains("public", StringComparison.Ordinal) || value.Contains("protected", StringComparison.Ordinal) || value.Contains("internal", StringComparison.Ordinal))
				{
					hasAccessibleParameterlessConstructor = true;
					break;
				}
			}
		}
	}

	private static void ReadPropertyAccessors(string accessors, out bool hasGetter, out bool hasSetter)
	{
		hasGetter = false;
		hasSetter = false;
		foreach (Match item in PropertyAccessorRegex.Matches(accessors ?? ""))
		{
			string text = item.Groups["access"].Value.Trim();
			if (string.IsNullOrWhiteSpace(text) || string.Equals(text, "public", StringComparison.Ordinal))
			{
				string value = item.Groups["kind"].Value;
				if (string.Equals(value, "get", StringComparison.Ordinal))
				{
					hasGetter = true;
				}
				else if (string.Equals(value, "set", StringComparison.Ordinal))
				{
					hasSetter = true;
				}
			}
		}
	}

	private static void ScanMethods(string body, ClassInfo info, TypeResolutionContext resolutionContext)
	{
		foreach (Match item in MethodRegex.Matches(body))
		{
			string value = item.Groups["name"].Value;
			if (ShouldSkipMethod(value))
			{
				continue;
			}
			string value2 = item.Groups["args"].Value;
			if (UnsupportedPassingModeRegex.IsMatch(value2))
			{
				continue;
			}
			string value3 = item.Groups["mods"].Value;
			string typeName = item.Groups["return"].Value.Trim();
			if (IsBlueprintSupportedType(typeName))
			{
				TypeInfo typeInfo = ConvertType(typeName, resolutionContext);
				List<ArgumentInfo> list = ParseArguments(value2, resolutionContext);
				if (AreBlueprintSupportedArguments(list))
				{
					MethodInfo methodInfo = new MethodInfo
					{
						Name = value,
						CsName = value,
						BaseClassName = (string.IsNullOrWhiteSpace(info.QualifiedName) ? info.Name : info.QualifiedName),
						CsClassName = info.Name,
						CsQualifiedClassName = (string.IsNullOrEmpty(info.QualifiedName) ? info.Name : info.QualifiedName),
						SourcePath = info.ScriptPath,
						Flags = (IsVirtual(value3) ? 64 : 0),
						IsAbstract = value3.Contains("abstract", StringComparison.Ordinal),
						IsStatic = value3.Contains("static", StringComparison.Ordinal),
						Return = typeInfo
					};
					methodInfo.Arguments.AddRange(list);
					AddUnique(info.Methods, methodInfo);
				}
			}
		}
	}

	private static void ScanProperties(string body, ClassInfo info, TypeResolutionContext resolutionContext)
	{
		foreach (Match item in PropertyRegex.Matches(body))
		{
			string value = item.Groups["name"].Value;
			if (ShouldSkipProperty(value))
			{
				continue;
			}
			string text = item.Groups["type"].Value.Trim();
			if (!(text == "event") && !(text == "delegate") && !text.StartsWith("event ", StringComparison.Ordinal) && IsBlueprintSupportedType(text))
			{
				string value2 = item.Groups["mods"].Value;
				TypeInfo type = ConvertType(text, resolutionContext);
				string value3 = item.Groups["tail"].Value;
				int index = item.Groups["tail"].Index;
				bool flag = value2.Contains("const", StringComparison.Ordinal);
				bool flag2 = value2.Contains("readonly", StringComparison.Ordinal);
				bool hasGetter = true;
				bool hasSetter = !flag2 && !flag;
				if (value3 == "{")
				{
					int num = FindMatchingBrace(body, index);
					ReadPropertyAccessors((num > index) ? body.Substring(index + 1, num - index - 1) : "", out hasGetter, out hasSetter);
				}
				else if (value3 == "=" && index + 1 < body.Length && body[index + 1] == '>')
				{
					hasGetter = true;
					hasSetter = false;
				}
				PropertyInfo member = new PropertyInfo
				{
					Name = value,
					CsName = value,
					BaseClassName = (string.IsNullOrWhiteSpace(info.QualifiedName) ? info.Name : info.QualifiedName),
					CsClassName = info.Name,
					CsQualifiedClassName = (string.IsNullOrEmpty(info.QualifiedName) ? info.Name : info.QualifiedName),
					SourcePath = info.ScriptPath,
					Type = type,
					Usage = 2,
					HasGetter = hasGetter,
					HasSetter = hasSetter,
					IsStatic = (flag || value2.Contains("static", StringComparison.Ordinal))
				};
				AddUnique(info.Properties, member);
			}
		}
	}

	private static void ScanSignals(string body, ClassInfo info, TypeResolutionContext resolutionContext)
	{
		foreach (Match item in SignalRegex.Matches(body))
		{
			string value = item.Groups["args"].Value;
			if (!UnsupportedPassingModeRegex.IsMatch(value))
			{
				List<ArgumentInfo> list = ParseArguments(value, resolutionContext);
				if (AreBlueprintSupportedArguments(list))
				{
					string value2 = item.Groups["name"].Value;
					string text = (value2.EndsWith("EventHandler", StringComparison.Ordinal) ? value2.Substring(0, value2.Length - "EventHandler".Length) : value2);
					SignalInfo signalInfo = new SignalInfo
					{
						Name = text,
						CsName = text,
						BaseClassName = (string.IsNullOrWhiteSpace(info.QualifiedName) ? info.Name : info.QualifiedName),
						CsClassName = info.Name,
						CsQualifiedClassName = (string.IsNullOrEmpty(info.QualifiedName) ? info.Name : info.QualifiedName),
						SourcePath = info.ScriptPath
					};
					signalInfo.Arguments.AddRange(list);
					AddUnique(info.Signals, signalInfo);
				}
			}
		}
	}

	private void AppendClassChain(string className, bool includeBase, Action<ClassInfo> append)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		string text = className;
		ClassInfo info;
		while (!string.IsNullOrEmpty(text) && hashSet.Add(text) && TryGetClass(text, out info))
		{
			append(info);
			if (includeBase)
			{
				text = info.BaseClass;
				continue;
			}
			break;
		}
	}

	private bool TryGetClass(string className, out ClassInfo info)
	{
		if (TryGetClass(GetProjectClassesSnapshot(), className, out info))
		{
			return true;
		}
		return TryGetClass(_hostClasses, className, out info);
	}

	private static bool TryGetClass(IReadOnlyDictionary<string, ClassInfo> source, string className, out ClassInfo info)
	{
		if (source.TryGetValue(className, out info))
		{
			return true;
		}
		foreach (ClassInfo value in source.Values)
		{
			if (string.Equals(value.Name, className, StringComparison.Ordinal) || string.Equals(value.QualifiedName, className, StringComparison.Ordinal))
			{
				info = value;
				return true;
			}
		}
		info = null;
		return false;
	}

	private string GetInheritanceBlockReason(ClassInfo info)
	{
		if (info == null)
		{
			return "类信息不可用";
		}
		if (info.IsStatic)
		{
			return "静态工具类不能作为蓝图父类，但其静态 API 仍可作为拼图使用";
		}
		if (info.IsSealed)
		{
			return "sealed 类不能被蓝图继承";
		}
		if (info.IsGeneric)
		{
			return "开放泛型类不能直接作为蓝图父类";
		}
		if (info.IsAbstract)
		{
			return "抽象类需要先实现抽象成员，不能直接作为安全蓝图父类";
		}
		if (info.HasExplicitConstructor && !info.HasAccessibleParameterlessConstructor)
		{
			return "缺少 public/protected/internal 无参构造函数";
		}
		if (!IsGodotCompatibleBase(info))
		{
			return "蓝图父类必须继承 GodotObject（例如 Node、Node2D、Control 或 Resource）";
		}
		return "";
	}

	private bool IsGodotCompatibleBase(ClassInfo info)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		string text = info?.BaseClass ?? "";
		while (!string.IsNullOrWhiteSpace(text) && hashSet.Add(text))
		{
			if (TryGetClass(text, out var info2))
			{
				text = info2.BaseClass;
				continue;
			}
			if ((text == "Object" || text == "GodotObject") ? true : false)
			{
				return true;
			}
			if (ClassDB.ClassExists(text))
			{
				if (!string.Equals(text, "Object", StringComparison.Ordinal))
				{
					return ClassDB.IsParentClass(text, "Object");
				}
				return true;
			}
			return false;
		}
		return false;
	}

	private static void AddLifecycleMethods(string className, List<Dictionary> result)
	{
		string owner = (string.IsNullOrEmpty(className) ? "Node" : className);
		AddLifecycle(result, owner, "_ready", "_Ready");
		AddLifecycle(result, owner, "_enter_tree", "_EnterTree");
		AddLifecycle(result, owner, "_exit_tree", "_ExitTree");
		AddLifecycle(result, owner, "_process", "_Process", ("delta", Variant.Type.Float, ""));
		AddLifecycle(result, owner, "_physics_process", "_PhysicsProcess", ("delta", Variant.Type.Float, ""));
		AddLifecycle(result, owner, "_input", "_Input", ("event_", Variant.Type.Object, "InputEvent"));
		AddLifecycle(result, owner, "_unhandled_input", "_UnhandledInput", ("event_", Variant.Type.Object, "InputEvent"));
	}

	private static void AddLifecycle(List<Dictionary> result, string owner, string godotName, string csName, params (string Name, Variant.Type Type, string ClassName)[] args)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		for (int i = 0; i < args.Length; i++)
		{
			(string, Variant.Type, string) tuple = args[i];
			array.Add(CreateArgumentDictionary(tuple.Item1, (int)tuple.Item2, tuple.Item3));
		}
		result.Add(new Dictionary
		{
			["name"] = godotName,
			["cs_name"] = csName,
			["base_class_name"] = owner,
			["cs_class_name"] = owner,
			["cs_qualified_class_name"] = owner,
			["flags"] = 64,
			["is_static"] = false,
			["return"] = CreateTypeDictionary(0, ""),
			["args"] = array
		});
	}

	private static Dictionary ToGodotDictionary(MethodInfo method)
	{
		return new Dictionary
		{
			["name"] = method.Name,
			["cs_name"] = method.CsName,
			["base_class_name"] = method.BaseClassName,
			["cs_class_name"] = method.CsClassName,
			["cs_qualified_class_name"] = method.CsQualifiedClassName,
			["source_path"] = method.SourcePath,
			["flags"] = method.Flags,
			["is_abstract"] = method.IsAbstract,
			["is_static"] = method.IsStatic,
			["return"] = CreateTypeDictionary(method.Return),
			["args"] = CreateArgumentArray(method.Arguments)
		};
	}

	private static Dictionary ToGodotDictionary(PropertyInfo property)
	{
		return new Dictionary
		{
			["name"] = property.Name,
			["cs_name"] = property.CsName,
			["base_class_name"] = property.BaseClassName,
			["cs_class_name"] = property.CsClassName,
			["cs_qualified_class_name"] = property.CsQualifiedClassName,
			["source_path"] = property.SourcePath,
			["type"] = property.Type.Type,
			["class_name"] = property.Type.ClassName,
			["cs_type_name"] = property.Type.CsTypeName,
			["usage"] = property.Usage,
			["has_get"] = property.HasGetter,
			["has_set"] = property.HasSetter,
			["is_static"] = property.IsStatic
		};
	}

	private static Dictionary ToGodotDictionary(SignalInfo signal)
	{
		return new Dictionary
		{
			["name"] = signal.Name,
			["cs_name"] = signal.CsName,
			["base_class_name"] = signal.BaseClassName,
			["cs_class_name"] = signal.CsClassName,
			["cs_qualified_class_name"] = signal.CsQualifiedClassName,
			["source_path"] = signal.SourcePath,
			["args"] = CreateArgumentArray(signal.Arguments)
		};
	}

	private static Godot.Collections.Array CreateArgumentArray(IEnumerable<ArgumentInfo> arguments)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (ArgumentInfo argument in arguments)
		{
			array.Add(CreateArgumentDictionary(argument.Name, argument.Type.Type, argument.Type.ClassName, argument.Type.CsTypeName));
		}
		return array;
	}

	private static void RemoveDuplicateMembers(List<Dictionary> members)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		for (int i = 0; i < members.Count; i++)
		{
			Dictionary dictionary = members[i];
			string item = (dictionary.TryGetValue("base_class_name", out var value) ? value.AsString() : "") + "::" + BuildMemberIdentity(dictionary);
			if (!hashSet.Add(item))
			{
				members.RemoveAt(i);
				i--;
			}
		}
	}

	private static void AddUnique(List<Dictionary> target, Dictionary member)
	{
		string b = BuildMemberIdentity(member);
		foreach (Dictionary item in target)
		{
			if (string.Equals(BuildMemberIdentity(item), b, StringComparison.Ordinal))
			{
				return;
			}
		}
		target.Add(member);
	}

	private static void AddUnique(List<MethodInfo> target, MethodInfo member)
	{
		string identity = BuildMemberIdentity(member.Name, member.Arguments);
		if (!target.Any((MethodInfo existing) => string.Equals(BuildMemberIdentity(existing.Name, existing.Arguments), identity, StringComparison.Ordinal)))
		{
			target.Add(member);
		}
	}

	private static void AddUnique(List<PropertyInfo> target, PropertyInfo member)
	{
		if (!target.Any((PropertyInfo existing) => string.Equals(existing.Name, member.Name, StringComparison.Ordinal)))
		{
			target.Add(member);
		}
	}

	private static void AddUnique(List<SignalInfo> target, SignalInfo member)
	{
		string identity = BuildMemberIdentity(member.Name, member.Arguments);
		if (!target.Any((SignalInfo existing) => string.Equals(BuildMemberIdentity(existing.Name, existing.Arguments), identity, StringComparison.Ordinal)))
		{
			target.Add(member);
		}
	}

	private static string BuildMemberIdentity(string name, IEnumerable<ArgumentInfo> arguments)
	{
		StringBuilder stringBuilder = new StringBuilder(name ?? "");
		stringBuilder.Append('(');
		foreach (ArgumentInfo argument in arguments)
		{
			TypeInfo typeInfo = argument?.Type;
			stringBuilder.Append(typeInfo?.Type ?? 0).Append(':').Append(typeInfo?.ClassName ?? "")
				.Append(':')
				.Append(typeInfo?.SourceType ?? "")
				.Append(';');
		}
		stringBuilder.Append(')');
		return stringBuilder.ToString();
	}

	private static string BuildMemberIdentity(Dictionary member)
	{
		string text = (member.TryGetValue("name", out var value) ? value.AsString() : "");
		if (!member.TryGetValue("args", out var value2))
		{
			return text;
		}
		Godot.Collections.Array array = value2.As<Godot.Collections.Array>();
		StringBuilder stringBuilder = new StringBuilder(text);
		stringBuilder.Append('(');
		foreach (Variant item in array)
		{
			Dictionary dictionary = item.As<Dictionary>();
			int value3 = (dictionary.TryGetValue("type", out var value4) ? value4.AsInt32() : 0);
			string value5 = (dictionary.TryGetValue("class_name", out var value6) ? value6.AsString() : "");
			string value7 = (dictionary.TryGetValue("cs_type_name", out var value8) ? value8.AsString() : "");
			stringBuilder.Append(value3).Append(':').Append(value5)
				.Append(':')
				.Append(value7)
				.Append(';');
		}
		stringBuilder.Append(')');
		return stringBuilder.ToString();
	}

	private static bool ShouldSkipMethod(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return true;
		}
		if (name.StartsWith("get_", StringComparison.Ordinal))
		{
			return true;
		}
		if (name.StartsWith("set_", StringComparison.Ordinal))
		{
			return true;
		}
		if (name.StartsWith("add_", StringComparison.Ordinal))
		{
			return true;
		}
		if (name.StartsWith("remove_", StringComparison.Ordinal))
		{
			return true;
		}
		return false;
	}

	private static bool ShouldSkipProperty(string name)
	{
		if (!string.IsNullOrEmpty(name))
		{
			return name.StartsWith("_", StringComparison.Ordinal);
		}
		return true;
	}

	private static bool IsVirtual(string modifiers)
	{
		if (!modifiers.Contains("virtual", StringComparison.Ordinal) && !modifiers.Contains("override", StringComparison.Ordinal))
		{
			return modifiers.Contains("abstract", StringComparison.Ordinal);
		}
		return true;
	}

	private static List<ArgumentInfo> ParseArguments(string argsSource, TypeResolutionContext resolutionContext)
	{
		List<ArgumentInfo> list = new List<ArgumentInfo>();
		if (string.IsNullOrWhiteSpace(argsSource))
		{
			return list;
		}
		foreach (string item in SplitTopLevel(argsSource, ','))
		{
			string text = ArgumentAttributeRegex.Replace(item.Trim(), "");
			int num = text.IndexOf('=');
			if (num >= 0)
			{
				text = text.Substring(0, num).Trim();
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = ArgumentModifierRegex.Replace(text, "");
				Match match = ArgumentNameRegex.Match(text);
				if (match.Success)
				{
					string typeName = match.Groups["type"].Value.Trim();
					string name = match.Groups["name"].Value.TrimStart('@');
					list.Add(new ArgumentInfo
					{
						Name = name,
						Type = ConvertType(typeName, resolutionContext)
					});
				}
			}
		}
		return list;
	}

	private static Dictionary CreateArgumentDictionary(string name, int type, string className, string csTypeName = "")
	{
		return new Dictionary
		{
			["name"] = name,
			["type"] = type,
			["class_name"] = className ?? "",
			["cs_type_name"] = csTypeName ?? ""
		};
	}

	private static Dictionary CreateTypeDictionary(int type, string className, string csTypeName = "")
	{
		return new Dictionary
		{
			["type"] = type,
			["class_name"] = className ?? "",
			["cs_type_name"] = csTypeName ?? ""
		};
	}

	private static Dictionary CreateTypeDictionary(TypeInfo type)
	{
		if (type == null)
		{
			return CreateTypeDictionary(0, "");
		}
		return CreateTypeDictionary(type.Type, type.ClassName, type.CsTypeName);
	}

	private static void ResolveSnapshotTypes(System.Collections.Generic.Dictionary<string, ClassInfo> classes, HashSet<string> knownTypes)
	{
		knownTypes.UnionWith(classes.Keys);
		foreach (ClassInfo value in classes.Values)
		{
			if (!string.IsNullOrWhiteSpace(value.RawBaseClass))
			{
				string resolvedType = ResolveCSharpTypeName(value.RawBaseClass, value.BaseResolutionContext, 24, knownTypes);
				value.BaseClass = NormalizeBaseClassForLookup(resolvedType);
			}
			foreach (MethodInfo method in value.Methods)
			{
				ResolveTypeInfo(method.Return, knownTypes);
				foreach (ArgumentInfo argument in method.Arguments)
				{
					ResolveTypeInfo(argument.Type, knownTypes);
				}
			}
			foreach (PropertyInfo property in value.Properties)
			{
				ResolveTypeInfo(property.Type, knownTypes);
			}
			foreach (SignalInfo signal in value.Signals)
			{
				foreach (ArgumentInfo argument2 in signal.Arguments)
				{
					ResolveTypeInfo(argument2.Type, knownTypes);
				}
			}
		}
	}

	private static void ResolveTypeInfo(TypeInfo typeInfo, HashSet<string> knownTypes)
	{
		if (typeInfo != null)
		{
			typeInfo.CsTypeName = ResolveCSharpTypeName(typeInfo.SourceType, typeInfo.ResolutionContext, typeInfo.Type, knownTypes);
		}
	}

	private static string ResolveCSharpTypeName(string sourceType, TypeResolutionContext context, int variantType, HashSet<string> knownTypes)
	{
		string text = (sourceType ?? "").Trim();
		bool flag = text.StartsWith("global::", StringComparison.Ordinal);
		string text2 = NormalizeTypeName(text);
		if (string.IsNullOrWhiteSpace(text2))
		{
			return "";
		}
		if (variantType != 24)
		{
			return text2;
		}
		if ((text2 == "object" || text2 == "Object") ? true : false)
		{
			if (!(text2 == "Object"))
			{
				return text2;
			}
			return "GodotObject";
		}
		if (GodotObjectTypeNames.Contains(text2))
		{
			return text2;
		}
		string text3 = ResolveUsingAlias(text2, context);
		if (!string.Equals(text3, text2, StringComparison.Ordinal))
		{
			return PrefixGlobal(text3);
		}
		if (flag)
		{
			return PrefixGlobal(text2);
		}
		if (text2.Contains(".", StringComparison.Ordinal))
		{
			return PrefixGlobal(text2);
		}
		string text4 = context?.NamespaceName ?? "";
		if (!string.IsNullOrWhiteSpace(text4))
		{
			string text5 = text4 + "." + text2;
			if (knownTypes.Contains(text5))
			{
				return PrefixGlobal(text5);
			}
		}
		if (context != null)
		{
			foreach (string @using in context.Usings)
			{
				string text6 = @using + "." + text2;
				if (knownTypes.Contains(text6))
				{
					return PrefixGlobal(text6);
				}
			}
			if (context.Usings.Contains("System", StringComparer.Ordinal) && SystemTypeNames.Contains(text2))
			{
				return PrefixGlobal("System." + text2);
			}
			if (context.Usings.Contains("System.Collections.Generic", StringComparer.Ordinal) && GenericCollectionTypeNames.Contains(text2))
			{
				return PrefixGlobal("System.Collections.Generic." + text2);
			}
		}
		return text2;
	}

	private static string ResolveUsingAlias(string normalizedType, TypeResolutionContext context)
	{
		if (context == null || string.IsNullOrWhiteSpace(normalizedType))
		{
			return normalizedType;
		}
		int num = normalizedType.IndexOf('.');
		string key = ((num >= 0) ? normalizedType.Substring(0, num) : normalizedType);
		if (!context.Aliases.TryGetValue(key, out var value))
		{
			return normalizedType;
		}
		if (num < 0)
		{
			return value;
		}
		return value + normalizedType.Substring(num);
	}

	private static string PrefixGlobal(string typeName)
	{
		if (string.IsNullOrWhiteSpace(typeName) || typeName.StartsWith("global::", StringComparison.Ordinal))
		{
			return typeName ?? "";
		}
		return "global::" + typeName;
	}

	private static string NormalizeBaseClassForLookup(string resolvedType)
	{
		string text = (resolvedType ?? "").Replace("global::", "", StringComparison.Ordinal).Trim();
		if (text.StartsWith("Godot.", StringComparison.Ordinal))
		{
			return text.Substring("Godot.".Length);
		}
		return text;
	}

	private static TypeInfo ConvertType(string typeName, TypeResolutionContext resolutionContext)
	{
		string text = NormalizeTypeName(typeName);
		TypeInfo typeInfo = new TypeInfo
		{
			SourceType = (typeName ?? ""),
			ResolutionContext = resolutionContext
		};
		if (!string.IsNullOrEmpty(text))
		{
			bool flag;
			switch (text)
			{
			case "void":
				break;
			case "bool":
			case "Boolean":
				flag = true;
				goto IL_0062;
			default:
				{
					flag = false;
					goto IL_0062;
				}
				IL_0062:
				if (flag)
				{
					typeInfo.Type = 1;
				}
				else
				{
					switch (text)
					{
					case "byte":
					case "long":
					case "uint":
					case "sbyte":
					case "short":
					case "ulong":
					case "ushort":
					case "int":
						flag = true;
						break;
					default:
						flag = false;
						break;
					}
					if (flag)
					{
						typeInfo.Type = 2;
					}
					else
					{
						switch (text)
						{
						case "float":
						case "double":
						case "decimal":
							flag = true;
							break;
						default:
							flag = false;
							break;
						}
						if (flag)
						{
							typeInfo.Type = 3;
						}
						else if ((text == "string" || text == "String") ? true : false)
						{
							typeInfo.Type = 4;
						}
						else
						{
							switch (text)
							{
							case "StringName":
								typeInfo.Type = 21;
								break;
							case "NodePath":
								typeInfo.Type = 22;
								break;
							case "Vector2":
								typeInfo.Type = 5;
								break;
							case "Vector2I":
							case "Vector2i":
								flag = true;
								goto IL_023b;
							default:
								{
									flag = false;
									goto IL_023b;
								}
								IL_023b:
								if (flag)
								{
									typeInfo.Type = 6;
									break;
								}
								switch (text)
								{
								case "Rect2":
									typeInfo.Type = 7;
									break;
								case "Rect2I":
								case "Rect2i":
									flag = true;
									goto IL_0283;
								default:
									{
										flag = false;
										goto IL_0283;
									}
									IL_0283:
									if (flag)
									{
										typeInfo.Type = 8;
										break;
									}
									switch (text)
									{
									case "Vector3":
										typeInfo.Type = 9;
										break;
									case "Vector3I":
									case "Vector3i":
										flag = true;
										goto IL_02cc;
									default:
										{
											flag = false;
											goto IL_02cc;
										}
										IL_02cc:
										if (flag)
										{
											typeInfo.Type = 10;
											break;
										}
										switch (text)
										{
										case "Transform2D":
											typeInfo.Type = 11;
											break;
										case "Vector4":
											typeInfo.Type = 12;
											break;
										case "Vector4I":
										case "Vector4i":
											flag = true;
											goto IL_0330;
										default:
											{
												flag = false;
												goto IL_0330;
											}
											IL_0330:
											if (flag)
											{
												typeInfo.Type = 13;
												break;
											}
											switch (text)
											{
											case "Plane":
												typeInfo.Type = 14;
												break;
											case "Quaternion":
												typeInfo.Type = 15;
												break;
											case "Aabb":
											case "AABB":
												flag = true;
												goto IL_0394;
											default:
												{
													flag = false;
													goto IL_0394;
												}
												IL_0394:
												if (flag)
												{
													typeInfo.Type = 16;
													break;
												}
												switch (text)
												{
												case "Basis":
													typeInfo.Type = 17;
													break;
												case "Transform3D":
													typeInfo.Type = 18;
													break;
												case "Projection":
													typeInfo.Type = 19;
													break;
												case "Color":
													typeInfo.Type = 20;
													break;
												case "Rid":
												case "RID":
													flag = true;
													goto IL_0426;
												default:
													{
														flag = false;
														goto IL_0426;
													}
													IL_0426:
													if (flag)
													{
														typeInfo.Type = 23;
													}
													else if (text == "Callable")
													{
														typeInfo.Type = 25;
													}
													else if (text == "Signal")
													{
														typeInfo.Type = 26;
													}
													break;
												}
												break;
											}
											break;
										}
										break;
									}
									break;
								}
								break;
							}
						}
					}
				}
				if (text.EndsWith("[]", StringComparison.Ordinal) || text.StartsWith("Array<", StringComparison.Ordinal) || text.StartsWith("Godot.Collections.Array<", StringComparison.Ordinal) || text.StartsWith("List<", StringComparison.Ordinal) || text.StartsWith("System.Collections.Generic.List<", StringComparison.Ordinal))
				{
					typeInfo.Type = 28;
				}
				else if (text == "Array" || text == "Godot.Collections.Array")
				{
					typeInfo.Type = 28;
				}
				else if (text.StartsWith("Dictionary<", StringComparison.Ordinal) || text.StartsWith("Godot.Collections.Dictionary<", StringComparison.Ordinal) || text.StartsWith("System.Collections.Generic.Dictionary<", StringComparison.Ordinal) || text == "Dictionary" || text == "Godot.Collections.Dictionary")
				{
					typeInfo.Type = 27;
				}
				else if (typeInfo.Type == 0)
				{
					typeInfo.Type = 24;
					typeInfo.ClassName = StripQualifiedTypeName(text);
				}
				return typeInfo;
			}
		}
		typeInfo.Type = 0;
		return typeInfo;
	}

	private static bool IsBlueprintSupportedType(string typeName)
	{
		string text = NormalizeTypeName(typeName);
		if (string.IsNullOrWhiteSpace(text) || string.Equals(text, "void", StringComparison.Ordinal))
		{
			return true;
		}
		if (text.EndsWith("[]", StringComparison.Ordinal) || text.Contains("<", StringComparison.Ordinal) || text.Contains(">", StringComparison.Ordinal))
		{
			return false;
		}
		if (!text.StartsWith("System.Array", StringComparison.Ordinal))
		{
			return !text.StartsWith("System.Collections.", StringComparison.Ordinal);
		}
		return false;
	}

	private static bool AreBlueprintSupportedArguments(IEnumerable<ArgumentInfo> arguments)
	{
		foreach (ArgumentInfo argument in arguments)
		{
			if (!IsBlueprintSupportedType(argument?.Type?.SourceType))
			{
				return false;
			}
		}
		return true;
	}

	private static string NormalizeTypeName(string typeName)
	{
		string text = (typeName ?? "").Trim();
		text = text.Replace("global::", "");
		text = TypeModifierRegex.Replace(text, "");
		if (text.EndsWith("?", StringComparison.Ordinal))
		{
			text = text.Substring(0, text.Length - 1);
		}
		return text.Trim();
	}

	private static string StripQualifiedTypeName(string type)
	{
		int num = type.IndexOf('<');
		if (num >= 0)
		{
			type = type.Substring(0, num);
		}
		type = type.TrimEnd('[', ']');
		int num2 = type.LastIndexOf('.');
		if (num2 < 0)
		{
			return type;
		}
		return type.Substring(num2 + 1);
	}

	private static List<string> SplitTopLevel(string text, char separator)
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			switch (c)
			{
			case '(':
			case '<':
			case '[':
				num++;
				continue;
			case ')':
			case '>':
			case ']':
				num = Math.Max(0, num - 1);
				continue;
			}
			if (c == separator && num == 0)
			{
				list.Add(text.Substring(num2, i - num2));
				num2 = i + 1;
			}
		}
		list.Add(text.Substring(num2));
		return list;
	}

	private static int FindMatchingBrace(string text, int openBrace)
	{
		int num = 0;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		for (int i = openBrace; i < text.Length; i++)
		{
			char c = text[i];
			char c2 = ((i + 1 < text.Length) ? text[i + 1] : '\0');
			if (flag3)
			{
				if (c == '\n')
				{
					flag3 = false;
				}
				continue;
			}
			if (flag4)
			{
				if (c == '*' && c2 == '/')
				{
					flag4 = false;
					i++;
				}
				continue;
			}
			if (flag)
			{
				if (!flag5 && c == '\\')
				{
					i++;
				}
				else if (flag5 && c == '"' && c2 == '"')
				{
					i++;
				}
				else if (c == '"')
				{
					flag = false;
					flag5 = false;
				}
				continue;
			}
			if (flag2)
			{
				switch (c)
				{
				case '\\':
					i++;
					break;
				case '\'':
					flag2 = false;
					break;
				}
				continue;
			}
			if (c == '/' && c2 == '/')
			{
				flag3 = true;
				i++;
				continue;
			}
			if (c == '/' && c2 == '*')
			{
				flag4 = true;
				i++;
				continue;
			}
			if (c == '@' && c2 == '"')
			{
				flag = true;
				flag5 = true;
				i++;
				continue;
			}
			switch (c)
			{
			case '"':
				flag = true;
				break;
			case '\'':
				flag2 = true;
				break;
			case '{':
				num++;
				break;
			case '}':
				num--;
				if (num == 0)
				{
					return i;
				}
				break;
			}
		}
		return -1;
	}

	private static string ResolveHostRoot()
	{
		string text = NormalizeRoot(ProjectSettings.GlobalizePath("res://"));
		if (string.IsNullOrEmpty(text) || !Directory.Exists(text))
		{
			text = NormalizeRoot(Directory.GetCurrentDirectory());
		}
		return text;
	}

	private static string NormalizeRoot(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		try
		{
			string text = path.Replace('\\', '/');
			if (text.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || text.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
			{
				text = ProjectSettings.GlobalizePath(text);
			}
			return Path.GetFullPath(text).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace('\\', '/');
		}
		catch
		{
			return "";
		}
	}

	private bool IsCSharpPathInsideProject(string scriptPath)
	{
		string projectRoot;
		lock (_projectSync)
		{
			projectRoot = _projectRoot;
		}
		if (string.IsNullOrWhiteSpace(projectRoot) || string.IsNullOrWhiteSpace(scriptPath) || !string.Equals(Path.GetExtension(scriptPath), ".cs", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		try
		{
			string fullPath = Path.GetFullPath((scriptPath.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || scriptPath.StartsWith("user://", StringComparison.OrdinalIgnoreCase)) ? ProjectSettings.GlobalizePath(scriptPath) : scriptPath);
			string text = Path.GetRelativePath(projectRoot, fullPath).Replace('\\', '/');
			return !text.Equals("..", StringComparison.Ordinal) && !text.StartsWith("../", StringComparison.Ordinal) && !Path.IsPathRooted(text) && !ShouldSkip(fullPath, projectRoot, isProjectRoot: true);
		}
		catch
		{
			return false;
		}
	}

	private void AttachProjectWatcher(string projectRoot)
	{
		if (string.IsNullOrWhiteSpace(projectRoot) || !Directory.Exists(projectRoot))
		{
			return;
		}
		try
		{
			_projectWatcher = new FileSystemWatcher(projectRoot, "*.cs")
			{
				IncludeSubdirectories = true,
				NotifyFilter = (NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size | NotifyFilters.LastWrite),
				EnableRaisingEvents = true
			};
			_projectWatcher.Created += OnProjectScriptChanged;
			_projectWatcher.Changed += OnProjectScriptChanged;
			_projectWatcher.Deleted += OnProjectScriptChanged;
			_projectWatcher.Renamed += OnProjectScriptRenamed;
			_projectWatcher.Error += OnProjectWatcherError;
		}
		catch (Exception ex)
		{
			DetachProjectWatcher();
			GD.PushWarning("Blueprint C# API watcher unavailable for '" + projectRoot + "': " + ex.Message);
		}
	}

	private void DetachProjectWatcher()
	{
		CancellationTokenSource projectWatcherDebounceCancellation;
		lock (_projectSync)
		{
			projectWatcherDebounceCancellation = _projectWatcherDebounceCancellation;
			_projectWatcherDebounceCancellation = null;
		}
		CancelAndDisposeWatcherRefresh(projectWatcherDebounceCancellation);
		if (_projectWatcher != null)
		{
			try
			{
				_projectWatcher.EnableRaisingEvents = false;
				_projectWatcher.Created -= OnProjectScriptChanged;
				_projectWatcher.Changed -= OnProjectScriptChanged;
				_projectWatcher.Deleted -= OnProjectScriptChanged;
				_projectWatcher.Renamed -= OnProjectScriptRenamed;
				_projectWatcher.Error -= OnProjectWatcherError;
				_projectWatcher.Dispose();
			}
			catch
			{
			}
			_projectWatcher = null;
		}
	}

	private void OnProjectScriptChanged(object sender, FileSystemEventArgs args)
	{
		if (sender == _projectWatcher)
		{
			InvalidateProjectContext();
			ScheduleProjectWatcherRefresh();
		}
	}

	private void OnProjectScriptRenamed(object sender, RenamedEventArgs args)
	{
		if (sender == _projectWatcher)
		{
			InvalidateProjectContext();
			ScheduleProjectWatcherRefresh();
		}
	}

	private void OnProjectWatcherError(object sender, ErrorEventArgs args)
	{
		if (sender == _projectWatcher)
		{
			InvalidateProjectContext();
			ScheduleProjectWatcherRefresh();
		}
	}

	private void ScheduleProjectWatcherRefresh()
	{
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		CancellationTokenSource projectWatcherDebounceCancellation;
		lock (_projectSync)
		{
			projectWatcherDebounceCancellation = _projectWatcherDebounceCancellation;
			_projectWatcherDebounceCancellation = cancellationTokenSource;
		}
		CancelAndDisposeWatcherRefresh(projectWatcherDebounceCancellation);
		RefreshAfterWatcherDebounceAsync(cancellationTokenSource);
	}

	private static void CancelAndDisposeWatcherRefresh(CancellationTokenSource cancellation)
	{
		if (cancellation == null)
		{
			return;
		}
		try
		{
			cancellation.Cancel();
		}
		catch (ObjectDisposedException)
		{
		}
		try
		{
			cancellation.Dispose();
		}
		catch (ObjectDisposedException)
		{
		}
	}

	private async Task RefreshAfterWatcherDebounceAsync(CancellationTokenSource debounceCancellation)
	{
		_ = 1;
		try
		{
			await Task.Delay(120, debounceCancellation.Token).ConfigureAwait(continueOnCapturedContext: false);
			await EnsureProjectScannedAsync(debounceCancellation.Token).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (OperationCanceledException)
		{
		}
		finally
		{
			lock (_projectSync)
			{
				if (_projectWatcherDebounceCancellation == debounceCancellation)
				{
					_projectWatcherDebounceCancellation = null;
					debounceCancellation.Dispose();
				}
			}
		}
	}

	private void InvalidateHostContext(bool clearSnapshot)
	{
		lock (_hostSync)
		{
			_hostDirty = true;
			Interlocked.Increment(ref _hostContextVersion);
			if (clearSnapshot)
			{
				Volatile.Write(ref _hostSnapshot, new ScanSnapshot
				{
					Root = _hostRoot,
					ContextVersion = _hostContextVersion
				});
			}
			if (_hostScanCancellation != null)
			{
				try
				{
					_hostScanCancellation.Cancel();
				}
				catch (ObjectDisposedException)
				{
				}
				_hostScanCancellation.Dispose();
				_hostScanCancellation = null;
			}
			_hostScanTask = null;
		}
	}

	private void InvalidateProjectContext(bool clearSnapshot = false)
	{
		lock (_projectSync)
		{
			InvalidateProjectContextLocked(clearSnapshot);
		}
	}

	private void InvalidateProjectContextLocked(bool clearSnapshot)
	{
		_projectDirty = true;
		Interlocked.Increment(ref _projectContextVersion);
		if (clearSnapshot)
		{
			Volatile.Write(ref _projectSnapshot, new ScanSnapshot
			{
				Root = _projectRoot,
				ContextVersion = _projectContextVersion
			});
		}
		if (_projectScanCancellation != null)
		{
			try
			{
				_projectScanCancellation.Cancel();
			}
			catch (ObjectDisposedException)
			{
			}
			_projectScanCancellation.Dispose();
			_projectScanCancellation = null;
		}
		_projectScanTask = null;
	}

	internal static void ResetInstance()
	{
		if (_instance != null)
		{
			_instance.DetachProjectWatcher();
			_instance.InvalidateHostContext(clearSnapshot: true);
			_instance.InvalidateProjectContext(clearSnapshot: true);
		}
		_instance = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(37)
		{
			new Godot.Bridge.MethodInfo(MethodName.Refresh, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ConfigureProjectRoot, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "projectRoot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.NotifyScriptSaved, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "scriptPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RefreshCurrentProject, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.HasClass, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.IsProjectClass, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetExternalBaseClassName, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.DoesClassImplementMethod, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "methodName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.EnsureScanned, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.EnsureClassSourceScanned, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.EnsureHostScanned, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.EnsureProjectScanned, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ShouldSkip, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "root", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "isProjectRoot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.FindNamespace, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CleanBaseClass, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "basePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.BuildMemberIdentity, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Dictionary, "member", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShouldSkipMethod, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShouldSkipProperty, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.IsVirtual, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "modifiers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateArgumentDictionary, new Godot.Bridge.PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "csTypeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateTypeDictionary, new Godot.Bridge.PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "csTypeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.PrefixGlobal, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "typeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.NormalizeBaseClassForLookup, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "resolvedType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.IsBlueprintSupportedType, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "typeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.NormalizeTypeName, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "typeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.StripQualifiedTypeName, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.FindMatchingBrace, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "openBrace", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ResolveHostRoot, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new Godot.Bridge.MethodInfo(MethodName.NormalizeRoot, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.IsCSharpPathInsideProject, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "scriptPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.AttachProjectWatcher, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "projectRoot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.DetachProjectWatcher, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ScheduleProjectWatcherRefresh, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.InvalidateHostContext, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "clearSnapshot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.InvalidateProjectContext, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "clearSnapshot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.InvalidateProjectContextLocked, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "clearSnapshot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ResetInstance, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureProjectRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ConfigureProjectRoot(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NotifyScriptSaved && args.Count == 1)
		{
			NotifyScriptSaved(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCurrentProject && args.Count == 0)
		{
			RefreshCurrentProject();
			ret = default;
			return true;
		}
		if (method == MethodName.HasClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsProjectClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProjectClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetExternalBaseClassName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetExternalBaseClassName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DoesClassImplementMethod && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(DoesClassImplementMethod(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.EnsureScanned && args.Count == 0)
		{
			EnsureScanned();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureClassSourceScanned && args.Count == 1)
		{
			EnsureClassSourceScanned(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureHostScanned && args.Count == 0)
		{
			EnsureHostScanned();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureProjectScanned && args.Count == 0)
		{
			EnsureProjectScanned();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldSkip && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkip(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.FindNamespace && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FindNamespace(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CleanBaseClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CleanBaseClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildMemberIdentity && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildMemberIdentity(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldSkipMethod && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipMethod(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldSkipProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipProperty(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsVirtual && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsVirtual(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateArgumentDictionary && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreateArgumentDictionary(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.CreateTypeDictionary && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreateTypeDictionary(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.PrefixGlobal && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(PrefixGlobal(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeBaseClassForLookup && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeBaseClassForLookup(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsBlueprintSupportedType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBlueprintSupportedType(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeTypeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.StripQualifiedTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(StripQualifiedTypeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindMatchingBrace && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindMatchingBrace(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolveHostRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveHostRoot());
			return true;
		}
		if (method == MethodName.NormalizeRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeRoot(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCSharpPathInsideProject && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCSharpPathInsideProject(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AttachProjectWatcher && args.Count == 1)
		{
			AttachProjectWatcher(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DetachProjectWatcher && args.Count == 0)
		{
			DetachProjectWatcher();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleProjectWatcherRefresh && args.Count == 0)
		{
			ScheduleProjectWatcherRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateHostContext && args.Count == 1)
		{
			InvalidateHostContext(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateProjectContext && args.Count == 1)
		{
			InvalidateProjectContext(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateProjectContextLocked && args.Count == 1)
		{
			InvalidateProjectContextLocked(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetInstance && args.Count == 0)
		{
			ResetInstance();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ShouldSkip && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkip(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.FindNamespace && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FindNamespace(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CleanBaseClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CleanBaseClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildMemberIdentity && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildMemberIdentity(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldSkipMethod && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipMethod(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldSkipProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipProperty(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsVirtual && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsVirtual(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateArgumentDictionary && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreateArgumentDictionary(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.CreateTypeDictionary && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreateTypeDictionary(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.PrefixGlobal && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(PrefixGlobal(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeBaseClassForLookup && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeBaseClassForLookup(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsBlueprintSupportedType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBlueprintSupportedType(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeTypeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.StripQualifiedTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(StripQualifiedTypeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindMatchingBrace && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindMatchingBrace(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolveHostRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveHostRoot());
			return true;
		}
		if (method == MethodName.NormalizeRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeRoot(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResetInstance && args.Count == 0)
		{
			ResetInstance();
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.ConfigureProjectRoot)
		{
			return true;
		}
		if (method == MethodName.NotifyScriptSaved)
		{
			return true;
		}
		if (method == MethodName.RefreshCurrentProject)
		{
			return true;
		}
		if (method == MethodName.HasClass)
		{
			return true;
		}
		if (method == MethodName.IsProjectClass)
		{
			return true;
		}
		if (method == MethodName.GetExternalBaseClassName)
		{
			return true;
		}
		if (method == MethodName.DoesClassImplementMethod)
		{
			return true;
		}
		if (method == MethodName.EnsureScanned)
		{
			return true;
		}
		if (method == MethodName.EnsureClassSourceScanned)
		{
			return true;
		}
		if (method == MethodName.EnsureHostScanned)
		{
			return true;
		}
		if (method == MethodName.EnsureProjectScanned)
		{
			return true;
		}
		if (method == MethodName.ShouldSkip)
		{
			return true;
		}
		if (method == MethodName.FindNamespace)
		{
			return true;
		}
		if (method == MethodName.CleanBaseClass)
		{
			return true;
		}
		if (method == MethodName.BuildMemberIdentity)
		{
			return true;
		}
		if (method == MethodName.ShouldSkipMethod)
		{
			return true;
		}
		if (method == MethodName.ShouldSkipProperty)
		{
			return true;
		}
		if (method == MethodName.IsVirtual)
		{
			return true;
		}
		if (method == MethodName.CreateArgumentDictionary)
		{
			return true;
		}
		if (method == MethodName.CreateTypeDictionary)
		{
			return true;
		}
		if (method == MethodName.PrefixGlobal)
		{
			return true;
		}
		if (method == MethodName.NormalizeBaseClassForLookup)
		{
			return true;
		}
		if (method == MethodName.IsBlueprintSupportedType)
		{
			return true;
		}
		if (method == MethodName.NormalizeTypeName)
		{
			return true;
		}
		if (method == MethodName.StripQualifiedTypeName)
		{
			return true;
		}
		if (method == MethodName.FindMatchingBrace)
		{
			return true;
		}
		if (method == MethodName.ResolveHostRoot)
		{
			return true;
		}
		if (method == MethodName.NormalizeRoot)
		{
			return true;
		}
		if (method == MethodName.IsCSharpPathInsideProject)
		{
			return true;
		}
		if (method == MethodName.AttachProjectWatcher)
		{
			return true;
		}
		if (method == MethodName.DetachProjectWatcher)
		{
			return true;
		}
		if (method == MethodName.ScheduleProjectWatcherRefresh)
		{
			return true;
		}
		if (method == MethodName.InvalidateHostContext)
		{
			return true;
		}
		if (method == MethodName.InvalidateProjectContext)
		{
			return true;
		}
		if (method == MethodName.InvalidateProjectContextLocked)
		{
			return true;
		}
		if (method == MethodName.ResetInstance)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._hostDirty)
		{
			_hostDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hostRoot)
		{
			_hostRoot = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._hostContextVersion)
		{
			_hostContextVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._hostScanGeneration)
		{
			_hostScanGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._projectDirty)
		{
			_projectDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._projectRoot)
		{
			_projectRoot = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._projectContextVersion)
		{
			_projectContextVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._projectScanGeneration)
		{
			_projectScanGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.ActiveProjectRoot)
		{
			from = ActiveProjectRoot;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.HostClassCount)
		{
			from2 = HostClassCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.HostScanGeneration)
		{
			from2 = HostScanGeneration;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		bool from3;
		if (name == PropertyName.IsHostScanReady)
		{
			from3 = IsHostScanReady;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.LastHostScanWarning)
		{
			from = LastHostScanWarning;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ProjectClassCount)
		{
			from2 = ProjectClassCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ProjectScanGeneration)
		{
			from2 = ProjectScanGeneration;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ProjectContextVersion)
		{
			from2 = ProjectContextVersion;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ProjectFilesScanned)
		{
			from2 = ProjectFilesScanned;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsProjectScanReady)
		{
			from3 = IsProjectScanReady;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.LastProjectScanWarning)
		{
			from = LastProjectScanWarning;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._hostDirty)
		{
			value = VariantUtils.CreateFrom(in _hostDirty);
			return true;
		}
		if (name == PropertyName._hostRoot)
		{
			value = VariantUtils.CreateFrom(in _hostRoot);
			return true;
		}
		if (name == PropertyName._hostContextVersion)
		{
			value = VariantUtils.CreateFrom(in _hostContextVersion);
			return true;
		}
		if (name == PropertyName._hostScanGeneration)
		{
			value = VariantUtils.CreateFrom(in _hostScanGeneration);
			return true;
		}
		if (name == PropertyName._projectDirty)
		{
			value = VariantUtils.CreateFrom(in _projectDirty);
			return true;
		}
		if (name == PropertyName._projectRoot)
		{
			value = VariantUtils.CreateFrom(in _projectRoot);
			return true;
		}
		if (name == PropertyName._projectContextVersion)
		{
			value = VariantUtils.CreateFrom(in _projectContextVersion);
			return true;
		}
		if (name == PropertyName._projectScanGeneration)
		{
			value = VariantUtils.CreateFrom(in _projectScanGeneration);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._hostDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName._hostRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._hostContextVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._hostScanGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._projectDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName._projectRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._projectContextVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._projectScanGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName.ActiveProjectRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName.HostClassCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName.HostScanGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName.IsHostScanReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName.LastHostScanWarning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName.ProjectClassCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName.ProjectScanGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName.ProjectContextVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName.ProjectFilesScanned, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName.IsProjectScanReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName.LastProjectScanWarning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._hostDirty, Variant.From(in _hostDirty));
		info.AddProperty(PropertyName._hostRoot, Variant.From(in _hostRoot));
		info.AddProperty(PropertyName._hostContextVersion, Variant.From(in _hostContextVersion));
		info.AddProperty(PropertyName._hostScanGeneration, Variant.From(in _hostScanGeneration));
		info.AddProperty(PropertyName._projectDirty, Variant.From(in _projectDirty));
		info.AddProperty(PropertyName._projectRoot, Variant.From(in _projectRoot));
		info.AddProperty(PropertyName._projectContextVersion, Variant.From(in _projectContextVersion));
		info.AddProperty(PropertyName._projectScanGeneration, Variant.From(in _projectScanGeneration));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._hostDirty, out var value))
		{
			_hostDirty = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hostRoot, out var value2))
		{
			_hostRoot = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName._hostContextVersion, out var value3))
		{
			_hostContextVersion = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._hostScanGeneration, out var value4))
		{
			_hostScanGeneration = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._projectDirty, out var value5))
		{
			_projectDirty = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._projectRoot, out var value6))
		{
			_projectRoot = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName._projectContextVersion, out var value7))
		{
			_projectContextVersion = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName._projectScanGeneration, out var value8))
		{
			_projectScanGeneration = value8.As<int>();
		}
	}
}
