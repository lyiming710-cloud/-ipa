using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/FullGameplayResourceFailureRuntimeTest.cs")]
public class FullGameplayResourceFailureRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ThrowingLoadFailedSubscriber = "ThrowingLoadFailedSubscriber";

		public static readonly StringName ThrowingLoadOverSubscriber = "ThrowingLoadOverSubscriber";

		public static readonly StringName ThrowingLoadPercentageSubscriber = "ThrowingLoadPercentageSubscriber";

		public static readonly StringName OnLoadFailed = "OnLoadFailed";

		public static readonly StringName OnLoadOver = "OnLoadOver";

		public static readonly StringName OnLoadPercentage = "OnLoadPercentage";

		public static readonly StringName Check = "Check";

		public static readonly StringName Fail = "Fail";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _failureEventCount = "_failureEventCount";

		public static readonly StringName _loadOverEventCount = "_loadOverEventCount";

		public static readonly StringName _percentageEventCount = "_percentageEventCount";

		public static readonly StringName _failureCount = "_failureCount";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string FixtureError = "category=CharacterScene, key=BrokenCharacterFixture, path=res://Test/Fixture/MissingCharacterScene.tscn";

	private const string ResultMarker = "FULL_GAMEPLAY_RESOURCE_FAILURE_RESULT";

	private int _failureEventCount;

	private int _loadOverEventCount;

	private int _percentageEventCount;

	private int _failureCount;

	public override async void _Ready()
	{
		ResourceManager resourceManager = ResourceManager.Instance;
		if (!GodotObject.IsInstanceValid(resourceManager))
		{
			Fail("ResourceManager is unavailable");
			Finish();
			return;
		}
		resourceManager.OnLoadFailed += ThrowingLoadFailedSubscriber;
		resourceManager.OnLoadFailed += OnLoadFailed;
		resourceManager.OnLoadOver += ThrowingLoadOverSubscriber;
		resourceManager.OnLoadOver += OnLoadOver;
		resourceManager.OnLoadPercentage += ThrowingLoadPercentageSubscriber;
		resourceManager.OnLoadPercentage += OnLoadPercentage;
		System.Reflection.MethodInfo method = typeof(ResourceManager).GetMethod("FailFullGameplayLoad", BindingFlags.Instance | BindingFlags.NonPublic);
		System.Reflection.MethodInfo method2 = typeof(ResourceManager).GetMethod("LoadBuiltInRoot", BindingFlags.Instance | BindingFlags.NonPublic);
		System.Reflection.MethodInfo method3 = typeof(ResourceManager).GetMethod("NotifyLoadOverSubscribers", BindingFlags.Instance | BindingFlags.NonPublic);
		System.Reflection.MethodInfo method4 = typeof(ResourceManager).GetMethod("EmitLoadPercentage", BindingFlags.Instance | BindingFlags.NonPublic);
		Check(method != null, "ResourceManager failure transition method was not found");
		Check(method2 != null, "ResourceManager strict root loading method was not found");
		Check(method3 != null, "ResourceManager safe success notification method was not found");
		Check(method4 != null, "ResourceManager safe progress notification method was not found");
		if (method == null || method2 == null || method3 == null || method4 == null)
		{
			Finish();
			return;
		}
		Exception ex = null;
		try
		{
			object[] parameters = new object[4] { "res://Test/Fixture/MissingCharacterScene.tscn", "CharacterScene", "BrokenCharacterFixture", 0.0 };
			method2.Invoke(resourceManager, parameters);
		}
		catch (TargetInvocationException ex2)
		{
			ex = ex2.InnerException;
		}
		Check(ex != null, "Missing CharacterScene root did not produce a strict load failure");
		Check(ex?.Message.Contains("category=CharacterScene, key=BrokenCharacterFixture, path=res://Test/Fixture/MissingCharacterScene.tscn", StringComparison.Ordinal) ?? false, "Strict root failure omitted category, key, or path: " + ex?.Message);
		method3.Invoke(resourceManager, null);
		Check(_loadOverEventCount == 1, $"Throwing success subscriber prevented later subscribers: events={_loadOverEventCount}");
		Check(resourceManager.CurrentGameplayResourceLoadState == GameplayResourceLoadState.NotStarted, "Success subscriber exception changed the pre-load resource state");
		method.Invoke(resourceManager, new object[1] { ex ?? new InvalidOperationException("category=CharacterScene, key=BrokenCharacterFixture, path=res://Test/Fixture/MissingCharacterScene.tscn") });
		Check(resourceManager.CurrentGameplayResourceLoadState == GameplayResourceLoadState.Failed, $"ResourceManager did not enter Failed: {resourceManager.CurrentGameplayResourceLoadState}");
		Check(resourceManager.FullGameplayResourceLoadError.Contains("category=CharacterScene, key=BrokenCharacterFixture, path=res://Test/Fixture/MissingCharacterScene.tscn", StringComparison.Ordinal), "ResourceManager failure omitted fixture path: " + resourceManager.FullGameplayResourceLoadError);
		method4.Invoke(resourceManager, new object[5] { 0.5, "FAILURE_FIXTURE", "BrokenCharacterFixture", 1, 1 });
		Check(_percentageEventCount == 1, $"Throwing progress subscriber prevented later subscribers: events={_percentageEventCount}");
		Check(resourceManager.CurrentGameplayResourceLoadState == GameplayResourceLoadState.Failed, "Progress subscriber exception changed the terminal resource state");
		resourceManager.BeginLoad();
		Check(resourceManager.CurrentGameplayResourceLoadState == GameplayResourceLoadState.Failed, "BeginLoad retried a terminal failure");
		PackedScene packedScene = GD.Load<PackedScene>("res://Scene/Loading/Loading.tscn");
		Check(GodotObject.IsInstanceValid(packedScene), "Loading scene is unavailable");
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			Finish();
			return;
		}
		Loading loading = packedScene.Instantiate<Loading>(PackedScene.GenEditState.Disabled);
		AddChild(loading, forceReadableName: false, InternalMode.Disabled);
		for (int frame = 0; frame < 12; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		Button node = loading.GetNode<Button>("%StartButton");
		Label node2 = loading.GetNode<Label>("%LoadLabel");
		Check(node.Disabled, "Loading enabled StartButton after a terminal resource failure");
		Check(node2.Visible, "Loading hid the terminal resource failure label");
		Check(node2.Text.Contains("CharacterScene", StringComparison.Ordinal) && node2.Text.Contains("BrokenCharacterFixture", StringComparison.Ordinal) && node2.Text.Contains("res://Test/Fixture/MissingCharacterScene.tscn", StringComparison.Ordinal), "Loading failure label omitted category, key, or path: " + node2.Text);
		Check(!resourceManager.AreFullGameplayResourcesReady, "Failed ResourceManager reported Ready");
		Check(_failureEventCount >= 3, $"Terminal failure was not replayed to later callers: events={_failureEventCount}");
		Check(_loadOverEventCount == 1, $"Terminal failure fired an additional OnLoadOver event: events={_loadOverEventCount}");
		loading.QueueFree();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		resourceManager.OnLoadFailed -= OnLoadFailed;
		resourceManager.OnLoadFailed -= ThrowingLoadFailedSubscriber;
		resourceManager.OnLoadOver -= OnLoadOver;
		resourceManager.OnLoadOver -= ThrowingLoadOverSubscriber;
		resourceManager.OnLoadPercentage -= OnLoadPercentage;
		resourceManager.OnLoadPercentage -= ThrowingLoadPercentageSubscriber;
		Finish();
	}

	private void ThrowingLoadFailedSubscriber(string error)
	{
		throw new InvalidOperationException("Failure subscriber fixture exception.");
	}

	private void ThrowingLoadOverSubscriber()
	{
		throw new InvalidOperationException("Success subscriber fixture exception.");
	}

	private void ThrowingLoadPercentageSubscriber(double percentage, string stepName, string resourceName, int resourceIndex, int resourceTotal)
	{
		throw new InvalidOperationException("Progress subscriber fixture exception.");
	}

	private void OnLoadFailed(string error)
	{
		_failureEventCount++;
	}

	private void OnLoadOver()
	{
		_loadOverEventCount++;
	}

	private void OnLoadPercentage(double percentage, string stepName, string resourceName, int resourceIndex, int resourceTotal)
	{
		_percentageEventCount++;
	}

	private void Check(bool condition, string message)
	{
		if (!condition)
		{
			Fail(message);
		}
	}

	private void Fail(string message)
	{
		_failureCount++;
		GD.PrintErr("FULL_GAMEPLAY_RESOURCE_FAILURE_CHECK_FAILED " + message);
	}

	private void Finish()
	{
		GD.Print($"{"FULL_GAMEPLAY_RESOURCE_FAILURE_RESULT"} passed={_failureCount == 0} state={ResourceManager.Instance?.CurrentGameplayResourceLoadState} failureEvents={_failureEventCount} loadOverEvents={_loadOverEventCount} percentageEvents={_percentageEventCount} failures={_failureCount}");
		GetTree().Quit((_failureCount != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(10)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ThrowingLoadFailedSubscriber, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ThrowingLoadOverSubscriber, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ThrowingLoadPercentageSubscriber, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "stepName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "resourceName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "resourceIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "resourceTotal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnLoadFailed, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnLoadOver, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnLoadPercentage, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "stepName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "resourceName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "resourceIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "resourceTotal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Fail, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Finish, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ThrowingLoadFailedSubscriber && args.Count == 1)
		{
			ThrowingLoadFailedSubscriber(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ThrowingLoadOverSubscriber && args.Count == 0)
		{
			ThrowingLoadOverSubscriber();
			ret = default;
			return true;
		}
		if (method == MethodName.ThrowingLoadPercentageSubscriber && args.Count == 5)
		{
			ThrowingLoadPercentageSubscriber(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnLoadFailed && args.Count == 1)
		{
			OnLoadFailed(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnLoadOver && args.Count == 0)
		{
			OnLoadOver();
			ret = default;
			return true;
		}
		if (method == MethodName.OnLoadPercentage && args.Count == 5)
		{
			OnLoadPercentage(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Fail && args.Count == 1)
		{
			Fail(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
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
		if (method == MethodName.ThrowingLoadFailedSubscriber)
		{
			return true;
		}
		if (method == MethodName.ThrowingLoadOverSubscriber)
		{
			return true;
		}
		if (method == MethodName.ThrowingLoadPercentageSubscriber)
		{
			return true;
		}
		if (method == MethodName.OnLoadFailed)
		{
			return true;
		}
		if (method == MethodName.OnLoadOver)
		{
			return true;
		}
		if (method == MethodName.OnLoadPercentage)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Fail)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._failureEventCount)
		{
			_failureEventCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._loadOverEventCount)
		{
			_loadOverEventCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._percentageEventCount)
		{
			_percentageEventCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failureCount)
		{
			_failureCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._failureEventCount)
		{
			value = VariantUtils.CreateFrom(in _failureEventCount);
			return true;
		}
		if (name == PropertyName._loadOverEventCount)
		{
			value = VariantUtils.CreateFrom(in _loadOverEventCount);
			return true;
		}
		if (name == PropertyName._percentageEventCount)
		{
			value = VariantUtils.CreateFrom(in _percentageEventCount);
			return true;
		}
		if (name == PropertyName._failureCount)
		{
			value = VariantUtils.CreateFrom(in _failureCount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failureEventCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._loadOverEventCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._percentageEventCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failureCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._failureEventCount, Variant.From(in _failureEventCount));
		info.AddProperty(PropertyName._loadOverEventCount, Variant.From(in _loadOverEventCount));
		info.AddProperty(PropertyName._percentageEventCount, Variant.From(in _percentageEventCount));
		info.AddProperty(PropertyName._failureCount, Variant.From(in _failureCount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._failureEventCount, out var value))
		{
			_failureEventCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._loadOverEventCount, out var value2))
		{
			_loadOverEventCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._percentageEventCount, out var value3))
		{
			_percentageEventCount = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failureCount, out var value4))
		{
			_failureCount = value4.As<int>();
		}
	}
}
