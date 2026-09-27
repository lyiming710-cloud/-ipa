using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ModSystem;

[ScriptPath("res://Tests/ModEditorRuntimeOverrideStackProbe.cs")]
public class ModEditorRuntimeOverrideStackProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SaveFeature = "SaveFeature";

		public static readonly StringName FindEditorWindow = "FindEditorWindow";

		public static readonly StringName Require = "Require";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	public override async void _Ready()
	{
		string suffix = Guid.NewGuid().ToString("N");
		string directKey = "probe_stack_direct_" + suffix;
		string missingKey = "probe_stack_missing_" + suffix;
		string previewKey = "probe_stack_preview_" + suffix;
		string loaderKey = "probe_stack_loader_" + suffix;
		string provider = "probe.provider." + suffix;
		string firstOverride = "probe.override.first." + suffix;
		string secondOverride = "probe.override.second." + suffix;
		string rejectedProvider = "probe.provider.rejected." + suffix;
		string missingOverride = "probe.override.missing." + suffix;
		string previewOwner = "ModEditorPreview." + suffix;
		string loaderA = "probe.loader.a." + suffix;
		string loaderB = "probe.loader.b." + suffix;
		string workRoot = ProjectSettings.GlobalizePath("user://RuntimeOverrideStackProbe/");
		string[] directOwners = new string[6] { provider, firstOverride, secondOverride, rejectedProvider, missingOverride, previewOwner };
		try
		{
			ModEditorManager modEditorManager = ModEditorManager.Instance;
			if (!GodotObject.IsInstanceValid(modEditorManager))
			{
				modEditorManager = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Core/ModEditorManager.tscn", null, ResourceLoader.CacheMode.Reuse)?.Instantiate<ModEditorManager>(PackedScene.GenEditState.Disabled);
				if (GodotObject.IsInstanceValid(modEditorManager))
				{
					AddChild(modEditorManager, forceReadableName: false, InternalMode.Disabled);
				}
			}
			Require(GodotObject.IsInstanceValid(modEditorManager), "ModEditorManager could not be instantiated.");
			await WaitFrames(2);
			Input.ParseInputEvent(new InputEventKey
			{
				Keycode = Key.F3,
				PhysicalKeycode = Key.F3,
				Pressed = true
			});
			Input.ParseInputEvent(new InputEventKey
			{
				Keycode = Key.F3,
				PhysicalKeycode = Key.F3,
				Pressed = false
			});
			bool flag = await WaitForEditorWindow(900);
			Require(flag, "F3 did not open the real ModEditor window.");
			TowerDefenseBattleFeature from = new TowerDefenseBattleFeature
			{
				ResourceName = "provided"
			};
			TowerDefenseBattleFeature from2 = new TowerDefenseBattleFeature
			{
				ResourceName = "override-first"
			};
			TowerDefenseBattleFeature from3 = new TowerDefenseBattleFeature
			{
				ResourceName = "override-second"
			};
			bool flag2 = XWModRuntimeRegistry.Register(provider, "Features", directKey, Variant.From(in from), allowOverride: false, out var diagnostic) && TowerDefenseBattleRegistry.BattleFeatureDictionary.TryGetValue(new StringName(directKey), out var value) && value == from;
			bool flag3 = !XWModRuntimeRegistry.Register(rejectedProvider, "Feature", directKey, Variant.From<TowerDefenseBattleFeature>(new TowerDefenseBattleFeature()), allowOverride: false, out var diagnostic2) && diagnostic2.Contains("provides collision", StringComparison.Ordinal) && diagnostic2.Contains("declare it under overrides", StringComparison.Ordinal);
			bool flag4 = !XWModRuntimeRegistry.Register(missingOverride, "Feature", missingKey, Variant.From<TowerDefenseBattleFeature>(new TowerDefenseBattleFeature()), allowOverride: true, out var diagnostic3) && diagnostic3.Contains("overrides target is missing", StringComparison.Ordinal) && diagnostic3.Contains("declare it under provides", StringComparison.Ordinal);
			TowerDefenseBattleFeature from4 = new TowerDefenseBattleFeature
			{
				ResourceName = "preview-upsert"
			};
			bool flag5 = XWModRuntimeRegistry.Register(previewOwner, "Feature", previewKey, Variant.From(in from4), allowOverride: true) && TowerDefenseBattleRegistry.BattleFeatureDictionary.TryGetValue(new StringName(previewKey), out var value2) && value2 == from4 && XWModRuntimeRegistry.UnregisterOwner(previewOwner) == 1 && !TowerDefenseBattleRegistry.BattleFeatureDictionary.ContainsKey(new StringName(previewKey));
			bool flag6 = XWModRuntimeRegistry.Register(firstOverride, "Feature", directKey, Variant.From(in from2), allowOverride: true, out var diagnostic4);
			bool flag7 = XWModRuntimeRegistry.Register(secondOverride, "Feature", directKey, Variant.From(in from3), allowOverride: true, out var diagnostic5);
			IReadOnlyList<XWModRuntimeRegistry.Registration> registrationStack = XWModRuntimeRegistry.GetRegistrationStack("Features", directKey);
			bool flag8 = (flag6 & flag7) && registrationStack.Count == 3 && registrationStack[0].OwnerMod == provider && registrationStack[1].OwnerMod == firstOverride && registrationStack[2].OwnerMod == secondOverride && !registrationStack[0].IsOverride && registrationStack[1].IsOverride && registrationStack[2].IsOverride && registrationStack[0].LoadOrder < registrationStack[1].LoadOrder && registrationStack[1].LoadOrder < registrationStack[2].LoadOrder && !registrationStack[0].IsEffective && !registrationStack[1].IsEffective && registrationStack[2].IsEffective && XWModRuntimeRegistry.TryGetEffectiveRegistration("Feature", directKey, out var registration) && registration.OwnerMod == secondOverride && TowerDefenseBattleRegistry.BattleFeatureDictionary.TryGetValue(new StringName(directKey), out var value3) && value3 == from3;
			int num = XWModRuntimeRegistry.UnregisterOwner(firstOverride);
			bool flag9 = TowerDefenseBattleRegistry.BattleFeatureDictionary.TryGetValue(new StringName(directKey), out var value4) && value4 == from3 && XWModRuntimeRegistry.GetRegistrationStack("Feature", directKey).Count == 2;
			int num2 = XWModRuntimeRegistry.UnregisterOwner(provider);
			bool flag10 = TowerDefenseBattleRegistry.BattleFeatureDictionary.TryGetValue(new StringName(directKey), out var value5) && value5 == from3 && XWModRuntimeRegistry.GetRegistrationStack("Feature", directKey).Count == 1;
			int num3 = XWModRuntimeRegistry.UnregisterOwner(secondOverride);
			int num4 = XWModRuntimeRegistry.UnregisterOwner(secondOverride);
			bool flag11 = ((num == 1 && num2 == 1 && num3 == 1 && num4 == 0) & flag9 & flag10) && !TowerDefenseBattleRegistry.BattleFeatureDictionary.ContainsKey(new StringName(directKey));
			bool flag12 = diagnostic.Contains("provided new runtime value", StringComparison.Ordinal) && diagnostic4.Contains("stacked override", StringComparison.Ordinal) && diagnostic5.Contains(firstOverride, StringComparison.Ordinal) && !diagnostic2.Contains(rejectedProvider, StringComparison.Ordinal);
			var (flag13, flag14, flag15) = CheckBuiltInFactoryOverrides(suffix);
			Require(flag13, "A Feature override did not replace the built-in construction factory.");
			Require(flag14, "A Process override did not replace the built-in construction factory.");
			Require(flag15, "Unloading overrides did not restore the original types and metadata.");
			if (Directory.Exists(workRoot))
			{
				Directory.Delete(workRoot, recursive: true);
			}
			Directory.CreateDirectory(workRoot);
			string featurePath = SaveFeature(workRoot, "feature_a.tres", "loader-a");
			string featurePath2 = SaveFeature(workRoot, "feature_b.tres", "loader-b");
			string text = Path.Combine(workRoot, "unsafe.cs");
			File.WriteAllText(text, "using Godot; public partial class UntrustedProbe : Node { }");
			TowerDefenseBattleFeature towerDefenseBattleFeature = new TowerDefenseBattleFeature
			{
				ResourceName = "loader-built-in"
			};
			TowerDefenseBattleRegistry.BattleFeatureDictionary[new StringName(loaderKey)] = towerDefenseBattleFeature;
			ModLoader.LoadedMod loadedMod = CreateLoadedMod(loaderA, loaderKey, featurePath, text);
			ModLoader.LoadedMod mod = CreateLoadedMod(loaderB, loaderKey, featurePath2, null);
			bool flag16 = ModLoader.ApplyMod(loadedMod);
			bool flag17 = ModLoader.ApplyMod(loadedMod);
			bool flag18 = XWModRuntimeRegistry.GetRegistrationStack("Feature", loaderKey).Count == 1 && ModLoader.GetLoadedMods().Count == 1 && TowerDefenseBattleRegistry.BattleFeatureDictionary.TryGetValue(new StringName(loaderKey), out var value6) && value6.ResourceName == "loader-a";
			bool flag19 = ModLoader.ApplyMod(mod);
			bool flag20 = XWModRuntimeRegistry.GetRegistrationStack("Feature", loaderKey).Count == 2 && ModLoader.GetLoadedMods().Count == 2 && TowerDefenseBattleRegistry.BattleFeatureDictionary.TryGetValue(new StringName(loaderKey), out var value7) && value7.ResourceName == "loader-b";
			bool flag21 = flag16 & flag17 & flag19 & flag18 & flag20;
			bool flag22 = loadedMod.Diagnostics.FindAll((string item) => item.Contains("blocked executable package file: Scripts/unsafe.cs", StringComparison.Ordinal)).Count == 1;
			bool flag23 = ModLoader.UnloadMod(loaderA);
			bool flag24 = ModLoader.UnloadMod(loaderA);
			bool flag25 = flag23 && !flag24 && XWModRuntimeRegistry.GetRegistrationStack("Feature", loaderKey).Count == 1 && TowerDefenseBattleRegistry.BattleFeatureDictionary.TryGetValue(new StringName(loaderKey), out var value8) && value8.ResourceName == "loader-b";
			ModLoader.UnloadAll();
			ModLoader.UnloadAll();
			bool flag26 = ModLoader.GetLoadedMods().Count == 0 && XWModRuntimeRegistry.GetRegistrationStack("Feature", loaderKey).Count == 0 && TowerDefenseBattleRegistry.BattleFeatureDictionary.TryGetValue(new StringName(loaderKey), out var value9) && value9 == towerDefenseBattleFeature;
			Require(flag2, "A provides declaration did not add an absent runtime key.");
			Require(flag3, "A second provides declaration did not report a truthful collision.");
			Require(flag4, "An overrides declaration was allowed without a lower target.");
			Require(flag5, "The legacy ModEditor preview upsert path was broken by strict manifest semantics.");
			Require(flag8, "Later overrides did not form an ordered effective layer stack.");
			Require(flag11, "Unloading a middle/lower owner did not preserve the correct effective layer.");
			Require(flag21, "Applying one Mod twice duplicated tracking or stack layers.");
			Require(flag25, "ModLoader could not unload an arbitrary lower owner idempotently.");
			Require(flag26, "Repeated UnloadAll did not restore the built-in runtime value.");
			Require(flag12, "Provide/override registration diagnostics do not match the real result.");
			Require(flag22, "The override-stack path attempted or ignored an untrusted script instead of blocking it.");
			GD.Print($"[MOD_RUNTIME_OVERRIDE_STACK_PROBE] f3={flag} provide={flag2} provideConflict={flag3} overrideMissing={flag4} previewUpsert={flag5} stack={flag8} middleUnload={flag11} loaderIdempotent={flag21} arbitraryUnload={flag25} unloadAll={flag26} diagnostics={flag12} scriptsBlocked={flag22} featureFactory={flag13} processFactory={flag14} restoredFactory={flag15} failures={_failures.Count}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		finally
		{
			string[] array = directOwners;
			for (int num5 = 0; num5 < array.Length; num5++)
			{
				XWModRuntimeRegistry.UnregisterOwner(array[num5]);
			}
			ModLoader.UnloadMod(loaderA);
			ModLoader.UnloadMod(loaderB);
			TowerDefenseBattleRegistry.BattleFeatureDictionary.Remove(new StringName(directKey));
			TowerDefenseBattleRegistry.BattleFeatureDictionary.Remove(new StringName(missingKey));
			TowerDefenseBattleRegistry.BattleFeatureDictionary.Remove(new StringName(previewKey));
			TowerDefenseBattleRegistry.BattleFeatureDictionary.Remove(new StringName(loaderKey));
			if (Directory.Exists(workRoot))
			{
				Directory.Delete(workRoot, recursive: true);
			}
		}
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_RUNTIME_OVERRIDE_STACK_PROBE_FAILURE] " + failure);
		}
		await WaitFrames(1);
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	private static (bool Feature, bool Process, bool Restored) CheckBuiltInFactoryOverrides(string suffix)
	{
		TowerDefenseBattleRegistry.Init();
		StringName stringName = new StringName("Wave");
		TowerDefenseBattleFeature towerDefenseBattleFeature = TowerDefenseBattleRegistry.BattleFeatureDictionary[stringName];
		TowerDefenseBattleProcess towerDefenseBattleProcess = TowerDefenseBattleRegistry.BattleProcessDictionary[stringName];
		string ownerMod = "probe.factory.first." + suffix;
		string ownerMod2 = "probe.factory.second." + suffix;
		string text = "probe.factory.new." + suffix;
		using TowerDefenseBattleFeatureCamera from = new TowerDefenseBattleFeatureCamera
		{
			gameStartPriority = 731
		};
		using TowerDefenseBattleFeatureSun from2 = new TowerDefenseBattleFeatureSun
		{
			gameStartPriority = 732
		};
		using TowerDefenseBattleProcessEmpty from3 = new TowerDefenseBattleProcessEmpty
		{
			gameStartPriority = 831
		};
		using TowerDefenseBattleProcessQuiz from4 = new TowerDefenseBattleProcessQuiz
		{
			gameStartPriority = 832
		};
		from.dependenceData = new TowerDefenseBattleDependenceData
		{
			FeatureNames = new Array<StringName> { "Map" }
		};
		from3.dependenceData = new TowerDefenseBattleDependenceData
		{
			FeatureNames = new Array<StringName> { "Map", "Sun" }
		};
		try
		{
			bool flag = XWModRuntimeRegistry.Register(ownerMod, "Feature", "Wave", Variant.From(in from), allowOverride: true, out var diagnostic) && XWModRuntimeRegistry.Register(ownerMod, "Process", "Wave", Variant.From(in from3), allowOverride: true, out diagnostic) && XWModRuntimeRegistry.Register(ownerMod, "Feature", text, Variant.From(in from), allowOverride: false, out diagnostic) && XWModRuntimeRegistry.Register(ownerMod, "Process", text, Variant.From(in from3), allowOverride: false, out diagnostic);
			using TowerDefenseBattleFeature towerDefenseBattleFeature2 = TowerDefenseBattleRegistry.GetFeature(stringName);
			using TowerDefenseBattleProcess towerDefenseBattleProcess2 = TowerDefenseBattleRegistry.GetProcess(stringName);
			using TowerDefenseBattleFeature towerDefenseBattleFeature3 = TowerDefenseBattleRegistry.GetFeature(text);
			using TowerDefenseBattleProcess towerDefenseBattleProcess3 = TowerDefenseBattleRegistry.GetProcess(text);
			bool flag2 = flag && towerDefenseBattleFeature2?.GetType() == from.GetType() && towerDefenseBattleFeature3?.GetType() == from.GetType() && towerDefenseBattleFeature2.gameStartPriority == 731 && towerDefenseBattleFeature2.dependenceData == from.dependenceData;
			bool flag3 = flag && towerDefenseBattleProcess2?.GetType() == from3.GetType() && towerDefenseBattleProcess3?.GetType() == from3.GetType() && towerDefenseBattleProcess2.gameStartPriority == 831 && towerDefenseBattleProcess2.dependenceData == from3.dependenceData;
			bool flag4 = XWModRuntimeRegistry.Register(ownerMod2, "Feature", "Wave", Variant.From(in from2), allowOverride: true, out diagnostic) && XWModRuntimeRegistry.Register(ownerMod2, "Process", "Wave", Variant.From(in from4), allowOverride: true, out diagnostic);
			bool flag5 = XWModRuntimeRegistry.UnregisterOwner(ownerMod) == 4 && !TowerDefenseBattleRegistry.BattleFeatureDictionary.ContainsKey(text) && !TowerDefenseBattleRegistry.BattleProcessDictionary.ContainsKey(text);
			using TowerDefenseBattleFeature towerDefenseBattleFeature4 = TowerDefenseBattleRegistry.GetFeature(stringName);
			using TowerDefenseBattleProcess towerDefenseBattleProcess4 = TowerDefenseBattleRegistry.GetProcess(stringName);
			flag2 &= (flag4 & flag5) && towerDefenseBattleFeature4?.GetType() == from2.GetType() && towerDefenseBattleFeature4.gameStartPriority == 732;
			flag3 &= (flag4 & flag5) && towerDefenseBattleProcess4?.GetType() == from4.GetType() && towerDefenseBattleProcess4.gameStartPriority == 832;
			bool flag6 = XWModRuntimeRegistry.UnregisterOwner(ownerMod2) == 2;
			using TowerDefenseBattleFeature towerDefenseBattleFeature5 = TowerDefenseBattleRegistry.GetFeature(stringName);
			using TowerDefenseBattleProcess towerDefenseBattleProcess5 = TowerDefenseBattleRegistry.GetProcess(stringName);
			bool item = flag6 && towerDefenseBattleFeature5?.GetType() == towerDefenseBattleFeature.GetType() && towerDefenseBattleProcess5?.GetType() == towerDefenseBattleProcess.GetType() && towerDefenseBattleFeature5.gameStartPriority == towerDefenseBattleFeature.gameStartPriority && towerDefenseBattleProcess5.gameStartPriority == towerDefenseBattleProcess.gameStartPriority && towerDefenseBattleFeature5.dependenceData == towerDefenseBattleFeature.dependenceData && towerDefenseBattleProcess5.dependenceData == towerDefenseBattleProcess.dependenceData && TowerDefenseBattleRegistry.BattleFeatureDictionary[stringName] == towerDefenseBattleFeature && TowerDefenseBattleRegistry.BattleProcessDictionary[stringName] == towerDefenseBattleProcess;
			return (Feature: flag2, Process: flag3, Restored: item);
		}
		finally
		{
			XWModRuntimeRegistry.UnregisterOwner(ownerMod);
			XWModRuntimeRegistry.UnregisterOwner(ownerMod2);
		}
	}

	private static ModLoader.LoadedMod CreateLoadedMod(string owner, string runtimeKey, string featurePath, string scriptPath)
	{
		XWModManifest xWModManifest = new XWModManifest
		{
			Id = owner,
			Name = owner
		};
		xWModManifest.Overrides["Feature"] = new List<string> { runtimeKey };
		if (!string.IsNullOrWhiteSpace(scriptPath))
		{
			xWModManifest.Scripts.Add("Scripts/unsafe.cs");
		}
		ModLoader.LoadedMod loadedMod = new ModLoader.LoadedMod
		{
			Manifest = xWModManifest,
			FilePath = owner + ".pmod"
		};
		loadedMod.RelativeFiles.Add("Battle/Features/" + Path.GetFileName(featurePath));
		loadedMod.ExtractedFiles.Add(featurePath);
		if (!string.IsNullOrWhiteSpace(scriptPath))
		{
			loadedMod.RelativeFiles.Add("Scripts/unsafe.cs");
			loadedMod.ExtractedFiles.Add(scriptPath);
		}
		return loadedMod;
	}

	private static string SaveFeature(string root, string fileName, string resourceName)
	{
		string text = Path.Combine(root, fileName);
		Error error = ResourceSaver.Save(new TowerDefenseBattleFeature
		{
			ResourceName = resourceName
		}, ProjectSettings.LocalizePath(text), ResourceSaver.SaverFlags.None);
		if (error != Error.Ok)
		{
			throw new InvalidOperationException($"Could not save {fileName}: {error}");
		}
		return text;
	}

	private async Task<bool> WaitForEditorWindow(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			Window window = FindEditorWindow(GetTree().Root);
			if (GodotObject.IsInstanceValid(window) && window.Visible)
			{
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private static Window FindEditorWindow(Node root)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		if (root is Window window && window.Title.StartsWith("PVZ Mod", StringComparison.Ordinal))
		{
			return window;
		}
		foreach (Node child in root.GetChildren())
		{
			Window window2 = FindEditorWindow(child);
			if (GodotObject.IsInstanceValid(window2))
			{
				return window2;
			}
		}
		return null;
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
			GD.PrintErr("[MOD_RUNTIME_OVERRIDE_STACK_PROBE_FAILURE] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "root", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "fileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "resourceName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindEditorWindow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Window"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SaveFeature && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(SaveFeature(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.FindEditorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindEditorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SaveFeature && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(SaveFeature(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.FindEditorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindEditorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.FindEditorWindow)
		{
			return true;
		}
		if (method == MethodName.Require)
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
