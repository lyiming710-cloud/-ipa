using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ComponentManagerResourceOwnershipRuntimeTest.cs")]
public sealed class ComponentManagerResourceOwnershipRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Require = "Require";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string CharacterScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const string LegacyManagerScenePath = "res://Script/Component/ComponentManager.tscn";

	private readonly List<string> _failures = new List<string>();

	public override async void _Ready()
	{
		try
		{
			await RunAsync();
		}
		catch (Exception ex)
		{
			_failures.Add("unexpected:" + ex.GetType().Name + ":" + ex.Message);
		}
		for (int i = 0; i < _failures.Count; i++)
		{
			GD.PrintErr("COMPONENT_MANAGER_RESOURCE_FAILURE " + _failures[i]);
		}
		bool flag = _failures.Count == 0;
		GD.Print($"COMPONENT_MANAGER_RESOURCE_RESULT passed={flag} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task RunAsync()
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", null, ResourceLoader.CacheMode.Reuse);
		PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://Script/Component/ComponentManager.tscn", null, ResourceLoader.CacheMode.Reuse);
		Require(packedScene != null, "character-scene-load");
		Require(packedScene2 != null, "legacy-manager-scene-load");
		if (packedScene != null && packedScene2 != null)
		{
			TowerDefenseCharacter character = packedScene.Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
			LegacyComponentManagerNode legacyComponentManagerNode = packedScene2.Instantiate<LegacyComponentManagerNode>(PackedScene.GenEditState.Disabled);
			ComponentBase legacyComponent = new ComponentBase
			{
				Name = "LegacyProbeComponent"
			};
			CharacterComponentSet expectedSet = character.ComponentSet;
			character.ComponentSet = null;
			legacyComponentManagerNode.Name = "ComponentManager";
			legacyComponentManagerNode.UniqueNameInOwner = true;
			legacyComponentManagerNode.ComponentSet = expectedSet;
			legacyComponentManagerNode.AddChild(legacyComponent, forceReadableName: false, InternalMode.Disabled);
			character.AddChild(legacyComponentManagerNode, forceReadableName: false, InternalMode.Disabled);
			legacyComponentManagerNode.Owner = character;
			legacyComponent.Owner = character;
			character.inGame = false;
			character.editorPreviewMode = false;
			AddChild(character, forceReadableName: false, InternalMode.Disabled);
			await WaitForSetup();
			ComponentManager manager = character.componentManager;
			Require(manager != null, "manager-is-resource");
			Require(manager?.GetParent() == character, "manager-owner");
			Require(character.ComponentSet == expectedSet, "legacy-set-copied");
			Require(manager?.ComponentSet == expectedSet, "manager-set-copied");
			Require(character.GetNodeOrNull("ComponentManager") == null, "legacy-host-removed");
			Require(legacyComponent.GetParent() == character, "legacy-child-migrated");
			Require(manager?.componentList.Contains(legacyComponent) ?? false, "legacy-child-registered");
			string wireKey = default;
			Require((manager?.TryGetWireKey(legacyComponent, out wireKey) ?? false) && wireKey == "ComponentBase", "legacy-wire-key");
			List<CharacterComponentRuntime> runtimes = new List<CharacterComponentRuntime>(manager.ResourceComponents);
			Require(runtimes.Count == expectedSet.GetFlattenedDefinitions().Count, "runtime-count");
			Require(AllActive(runtimes, manager, character), "initial-active");
			RemoveChild(character);
			Require(AllDetached(runtimes), "tree-exit-detached");
			AddChild(character, forceReadableName: false, InternalMode.Disabled);
			await WaitForSetup();
			Require(character.componentManager == manager, "manager-reused");
			Require(AllActive(runtimes, manager, character), "reentry-active");
			RemoveChild(character);
			character.Free();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			Require(AllReleased(runtimes), "final-release");
		}
	}

	private async Task WaitForSetup()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	private static bool AllActive(List<CharacterComponentRuntime> runtimes, ComponentManager manager, TowerDefenseCharacter owner)
	{
		for (int i = 0; i < runtimes.Count; i++)
		{
			CharacterComponentRuntime characterComponentRuntime = runtimes[i];
			if (characterComponentRuntime == null || characterComponentRuntime.Lifecycle != ComponentRuntimeLifecycle.Active || characterComponentRuntime.Manager != manager || characterComponentRuntime.Owner != owner)
			{
				return false;
			}
		}
		return true;
	}

	private static bool AllDetached(List<CharacterComponentRuntime> runtimes)
	{
		for (int i = 0; i < runtimes.Count; i++)
		{
			CharacterComponentRuntime characterComponentRuntime = runtimes[i];
			if (characterComponentRuntime == null || characterComponentRuntime.Lifecycle != ComponentRuntimeLifecycle.Detached)
			{
				return false;
			}
		}
		return true;
	}

	private static bool AllReleased(List<CharacterComponentRuntime> runtimes)
	{
		for (int i = 0; i < runtimes.Count; i++)
		{
			CharacterComponentRuntime characterComponentRuntime = runtimes[i];
			if (characterComponentRuntime == null || !characterComponentRuntime.IsReleased)
			{
				return false;
			}
		}
		return true;
	}

	private void Require(bool condition, string failure)
	{
		if (!condition)
		{
			_failures.Add(failure);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "failure", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
