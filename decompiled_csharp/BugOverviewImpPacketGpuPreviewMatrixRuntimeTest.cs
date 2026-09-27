using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewImpPacketGpuPreviewMatrixRuntimeTest.cs")]
public class BugOverviewImpPacketGpuPreviewMatrixRuntimeTest : Node
{
	private sealed class ImpPacketCase
	{
		public string Key { get; }

		public string PacketPath { get; }

		public string ExpectedSpritePath { get; }

		public string ExpectedArmor { get; }

		public string ExpectedHeadNode { get; }

		public ImpPacketCase(string key, string packetPath, string expectedSpritePath, string expectedArmor = "", string expectedHeadNode = "")
		{
			Key = key;
			PacketPath = packetPath;
			ExpectedSpritePath = expectedSpritePath;
			ExpectedArmor = expectedArmor;
			ExpectedHeadNode = expectedHeadNode;
		}
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName TransformApproximatelyEqual = "TransformApproximatelyEqual";

		public static readonly StringName ResourcePathMatches = "ResourcePathMatches";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "IMP_PACKET_GPU_PREVIEW_MATRIX_RESULT";

	private const string PacketShowScenePath = "res://Prefab/TowerDefense/GUI/InGame/Packet/TowerDefenseInGamePacketShow.tscn";

	private const string BaseSpritePath = "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/Base/ZombieImp.tscn";

	private static readonly ImpPacketCase[] MatrixCases = new ImpPacketCase[14]
	{
		new ImpPacketCase("ZombieImp", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/Base/ZombieImp.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/Base/ZombieImp.tscn"),
		new ImpPacketCase("ZombieImpBlackHelmet", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/Base/ZombieImpBlackHelmet.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/Base/ZombieImp.tscn", "BlackHelmet"),
		new ImpPacketCase("ZombieImpBucket", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/Base/ZombieImpBucket.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/Base/ZombieImp.tscn", "Bucket"),
		new ImpPacketCase("ZombieImpCone", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/Base/ZombieImpCone.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/Base/ZombieImp.tscn", "Cone"),
		new ImpPacketCase("ZombieImpHelmet", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/Base/ZombieImpHelmet.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/Base/ZombieImp.tscn", "Helmet"),
		new ImpPacketCase("ZombieImpSpecialHelmet", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/Base/ZombieImpSpecialHelmet.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/Base/ZombieImp.tscn", "SpecialHelmet"),
		new ImpPacketCase("ZombieImpHypnoShroom", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/HypnoShroom/ZombieImpHypnoShroom.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/HypnoShroom/ZombieImpHypnoShroom.tscn", "", "Head"),
		new ImpPacketCase("ZombieImpPeashooterSingle", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/PeashooterSingle/ZombieImpPeashooterSingle.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/PeashooterSingle/ZombieImpPeashooterSingle.tscn", "", "Head"),
		new ImpPacketCase("ZombieImpPeashooterZ", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/PeashooterZ/ZombieImpPeashooterZ.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/PeashooterZ/ZombieImpPeashooterZ.tscn", "", "Head"),
		new ImpPacketCase("ZombieImpPresentBox", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/PresentBox/ZombieImpPresentBox.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/PresentBox/ZombieImpPresentBox.tscn", "", "PresentBoxImpHead"),
		new ImpPacketCase("ZombieImpPuffShroom", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/PuffShroom/ZombieImpPuffShroom.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/PuffShroom/ZombieImpPuffShroom.tscn", "", "Head"),
		new ImpPacketCase("ZombieImpSeaShroom", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/SeaShroom/ZombieImpSeaShroom.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/SeaShroom/ZombieImpSeaShroom.tscn", "", "Head"),
		new ImpPacketCase("ZombieImpSunpult", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/Sunpult/ZombieImpSunpult.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/Sunpult/ZombieImpSunpult.tscn", "", "Head"),
		new ImpPacketCase("ZombieImpSunShroom", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/SunShroom/ZombieImpSunShroom.tres", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Sprite/SunShroom/ZombieImpSunShroom.tscn", "", "Head")
	};

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		AdobeAnimateRenderBackend originalBackend = AdobeAnimateRenderBackend.GpuCrowd;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(Global.Instance) && GodotObject.IsInstanceValid(ResourceManager.Instance) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance), "Global, ResourceManager, and TowerDefenseManager autoloads must be available.");
				if (!GodotObject.IsInstanceValid(Global.Instance) || !GodotObject.IsInstanceValid(ResourceManager.Instance) || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
				{
					goto end_IL_0046;
				}
				if (!(await LoadProductionResources()))
				{
					_failures++;
					GD.PushError("[BugOverviewImpPacketGpuPreviewMatrixRuntimeTest] Production ResourceManager loading must complete before packet previews are verified.");
					goto end_IL_0046;
				}
				originalBackend = Global.Instance.adobeAnimateRenderBackend;
				Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
				await WaitFrames(3);
				Check(Global.Instance.adobeAnimateRenderBackend == AdobeAnimateRenderBackend.GpuCrowd, "The regression must execute while GPU optimization selects GpuCrowd.");
				PackedScene packetShowScene = ResourceLoader.Load<PackedScene>("res://Prefab/TowerDefense/GUI/InGame/Packet/TowerDefenseInGamePacketShow.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
				Check(MatrixCases.Length == 14 && GodotObject.IsInstanceValid(packetShowScene) && packetShowScene.CanInstantiate(), "The fixture must contain all 14 authored Imp packets and the production PacketShow scene.");
				if (!GodotObject.IsInstanceValid(packetShowScene) || !packetShowScene.CanInstantiate())
				{
					goto end_IL_0046;
				}
				ImpPacketCase[] matrixCases = MatrixCases;
				foreach (ImpPacketCase matrixCase in matrixCases)
				{
					await VerifyPacket(packetShowScene, matrixCase);
				}
				Check(Global.Instance.adobeAnimateRenderBackend == AdobeAnimateRenderBackend.GpuCrowd, "All 14 previews must leave the selected GPU backend unchanged.");
				goto end_IL_002f;
				end_IL_0046:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[{"BugOverviewImpPacketGpuPreviewMatrixRuntimeTest"}] Unexpected exception: {value}");
				goto end_IL_002f;
			}
			return;
			end_IL_002f:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(Global.Instance))
			{
				Global.Instance.adobeAnimateRenderBackend = originalBackend;
			}
			ObjectManager.Instance?.Clear();
			ResourceManager.Instance?.ReleaseTransientResources();
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			await WaitFrames(3);
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
		bool flag = _checks == 116 && _failures == 0;
		GD.Print($"{"IMP_PACKET_GPU_PREVIEW_MATRIX_RESULT"} passed={flag} checks={_checks} failures={_failures} packets={MatrixCases.Length}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyPacket(PackedScene packetShowScene, ImpPacketCase matrixCase)
	{
		TowerDefenseInGamePacketShow packetShow = null;
		try
		{
			_ = 1;
			try
			{
				TowerDefensePacketConfig packet = TowerDefenseManager.GetPacketConfigReadOnly(matrixCase.Key);
				Check(GodotObject.IsInstanceValid(packet) && packet.saveKey == matrixCase.Key && packet.packetAnimeClip == "Walk" && GodotObject.IsInstanceValid(packet.characterConfig) && ResourcePathMatches(packet, matrixCase.PacketPath), matrixCase.Key + " must load its authored packet through ResourceManager.");
				if (!GodotObject.IsInstanceValid(packet) || !GodotObject.IsInstanceValid(packet.characterConfig))
				{
					goto end_IL_0041;
				}
				PackedScene packetSpriteScene = TowerDefenseManager.GetPacketSpriteScene(packet);
				Check(GodotObject.IsInstanceValid(packetSpriteScene) && packetSpriteScene.CanInstantiate() && ResourcePathMatches(packetSpriteScene, matrixCase.ExpectedSpritePath), $"{matrixCase.Key} must resolve the expected authored Sprite scene; actual={packetSpriteScene?.ResourcePath ?? "<null>"}.");
				if (!GodotObject.IsInstanceValid(packetSpriteScene) || !packetSpriteScene.CanInstantiate())
				{
					goto end_IL_0041;
				}
				packetShow = packetShowScene.Instantiate<TowerDefenseInGamePacketShow>(PackedScene.GenEditState.Disabled);
				if (GodotObject.IsInstanceValid(packetShow))
				{
					packetShow.setPcLayout = true;
					packetShow.onlyDraw = true;
					packetShow.Name = "PacketShow_" + matrixCase.Key;
					AddChild(packetShow, forceReadableName: false, InternalMode.Disabled);
					await WaitFrames(2);
					packetShow.Init(packet);
					await WaitFrames(5);
				}
				Check(GodotObject.IsInstanceValid(packetShow) && packetShow.IsNodeReady() && packetShow.config == packet, matrixCase.Key + " must initialize one production PacketShow control.");
				if (!GodotObject.IsInstanceValid(packetShow))
				{
					goto end_IL_0041;
				}
				AdobeAnimateSprite sprite = packetShow.sprite;
				Check(GodotObject.IsInstanceValid(sprite) && sprite.GetParent() == packetShow.previewSpriteNode && sprite.SceneFilePath == matrixCase.ExpectedSpritePath && sprite.Visible && sprite.IsFrozenPreview && sprite.clip == packet.packetAnimeClip && sprite.forceLocalRender, matrixCase.Key + " must mount its exact visible frozen preview while GpuCrowd is selected.");
				if (!GodotObject.IsInstanceValid(sprite))
				{
					goto end_IL_0041;
				}
				int count = 0;
				Check(TryCountDrawItems(sprite, out count) && count > 0, $"{matrixCase.Key} must submit real preview draw items; count={count}.");
				bool flag = AdobeAnimateGpuRenderGraphBuilder.TryBuild(sprite, out var graph, out var ownerSprites, out var failureReason);
				Check(flag && graph != null && ownerSprites.Length == graph.Owners.Length && graph.Owners.Length != 0 && graph.RenderSlots.Length != 0, $"{matrixCase.Key} must remain representable by the GPU graph; reason={failureReason}, owners={graph?.Owners.Length ?? 0}, slots={graph?.RenderSlots.Length ?? 0}.");
				Check(ValidateRoleMapping(matrixCase, packet, sprite, graph, out var diagnostic), matrixCase.Key + " must retain its authored base armor or dedicated Imp head mapping; " + diagnostic);
				Check(ValidateLocalPreviewChildTransforms(sprite, out var diagnostic2), matrixCase.Key + " local card preview children must inherit the current parent Slot transform; " + diagnostic2);
				goto end_IL_002f;
				end_IL_0041:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[{"BugOverviewImpPacketGpuPreviewMatrixRuntimeTest"}] {matrixCase.Key}: {value}");
				goto end_IL_002f;
			}
			end_IL_002f:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(packetShow))
			{
				packetShow.QueueFree();
			}
			await WaitFrames(3);
		}
	}

	private static bool ValidateRoleMapping(ImpPacketCase matrixCase, TowerDefensePacketConfig packet, AdobeAnimateSprite sprite, AdobeAnimateGpuRenderGraphDefinition graph, out string diagnostic)
	{
		diagnostic = string.Empty;
		if (!string.IsNullOrEmpty(matrixCase.ExpectedHeadNode))
		{
			AdobeAnimateSprite nodeOrNull = sprite.GetNodeOrNull<AdobeAnimateSprite>(matrixCase.ExpectedHeadNode);
			int count = 0;
			bool result = packet.characterConfig.name == matrixCase.Key && GodotObject.IsInstanceValid(nodeOrNull) && nodeOrNull.parentSprite == sprite && nodeOrNull.Visible && nodeOrNull.IsFrozenPreview && TryCountDrawItems(nodeOrNull, out count) && count > 0 && graph != null && graph.Owners.Length >= 2;
			diagnostic = $"head={matrixCase.ExpectedHeadNode}, headValid={GodotObject.IsInstanceValid(nodeOrNull)}, headItems={count}, graphOwners={graph?.Owners.Length ?? 0}";
			return result;
		}
		AdobeAnimateSlot nodeOrNull2 = sprite.GetNodeOrNull<AdobeAnimateSlot>("HeadSlot");
		if (!string.IsNullOrEmpty(matrixCase.ExpectedArmor))
		{
			AdobeAnimatePart adobeAnimatePart = ((GodotObject.IsInstanceValid(nodeOrNull2) && nodeOrNull2.GetChildCount() > 0) ? (nodeOrNull2.GetChild(0) as AdobeAnimatePart) : null);
			bool flag = GodotObject.IsInstanceValid(adobeAnimatePart) && AdobeAnimateGlobalAtlasCache.TryGetReplaceTextureAllocation(adobeAnimatePart.externalAtlasTexturePath, out var allocation) && allocation.UsesTextureArray && graph != null && graph.Owners.Length != 0 && allocation.TextureArrayRid == graph.Owners[0].Definition.AtlasTextureArrayRid;
			bool result2 = ((packet.characterConfig.name == "ZombieImp" && packet.initArmor.Count == 1 && packet.initArmor[0] == matrixCase.ExpectedArmor && GodotObject.IsInstanceValid(adobeAnimatePart) && !adobeAnimatePart.Visible && AdobeAnimateManagedSprite2D.GetLogicalVisible(adobeAnimatePart) && !string.IsNullOrEmpty(adobeAnimatePart.externalAtlasTexturePath)) & flag) && graph != null && graph.Owners.Length != 0 && graph.ManagedVisualBindings.Length != 0;
			diagnostic = $"armor={matrixCase.ExpectedArmor}, children={nodeOrNull2?.GetChildCount() ?? (-1)}, atlasPart={GodotObject.IsInstanceValid(adobeAnimatePart)}, atlas={flag}, managedVisuals={graph?.ManagedVisualBindings.Length ?? 0}";
			return result2;
		}
		bool result3 = packet.characterConfig.name == "ZombieImp" && packet.initArmor.Count == 0 && GodotObject.IsInstanceValid(nodeOrNull2) && nodeOrNull2.GetChildCount() == 0 && graph != null && graph.Owners.Length >= 1;
		diagnostic = $"baseHeadSlotChildren={nodeOrNull2?.GetChildCount() ?? (-1)}";
		return result3;
	}

	private static bool ValidateLocalPreviewChildTransforms(AdobeAnimateSprite root, out string diagnostic)
	{
		List<AdobeAnimateSprite> list = new List<AdobeAnimateSprite>();
		Dictionary<AdobeAnimateSprite, Transform2D> dictionary = new Dictionary<AdobeAnimateSprite, Transform2D>();
		int num = 0;
		foreach (Node item in root.FindChildren("*", "AdobeAnimateSprite", recursive: true, owned: false))
		{
			if (item is AdobeAnimateSprite adobeAnimateSprite && adobeAnimateSprite != root && adobeAnimateSprite.Visible)
			{
				if (!adobeAnimateSprite.TryBuildRenderSnapshot(out var snapshot, allowUnchanged: false))
				{
					diagnostic = $"child={adobeAnimateSprite.GetPath()}, snapshot=false";
					return false;
				}
				Transform2D transform2D = AdobeAnimateRenderManager.ToRenderMountLocalTransform(snapshot.RenderMountParent, adobeAnimateSprite.GlobalTransform);
				if (!TransformApproximatelyEqual(snapshot.GlobalTransform, transform2D))
				{
					diagnostic = $"child={adobeAnimateSprite.GetPath()}, cached={snapshot.GlobalTransform}, current={transform2D}";
					return false;
				}
				list.Add(adobeAnimateSprite);
				dictionary[adobeAnimateSprite] = adobeAnimateSprite.GlobalTransform;
				num++;
			}
		}
		int frameIndex = root.frameIndex;
		int num2 = 0;
		try
		{
			for (int i = root.clipRange.X + 1; i < root.clipRange.Y; i++)
			{
				if (num2 != 0)
				{
					break;
				}
				root.frameIndex = i;
				root.UpdateChild();
				foreach (AdobeAnimateSprite item2 in list)
				{
					if (GodotObject.IsInstanceValid(item2) && !item2.GlobalTransform.IsEqualApprox(dictionary[item2]))
					{
						num2++;
						if (!item2.TryBuildRenderSnapshot(out var snapshot2, allowUnchanged: false))
						{
							diagnostic = $"child={item2.GetPath()}, movedSnapshot=false";
							return false;
						}
						Transform2D transform2D2 = AdobeAnimateRenderManager.ToRenderMountLocalTransform(snapshot2.RenderMountParent, item2.GlobalTransform);
						if (!TransformApproximatelyEqual(snapshot2.GlobalTransform, transform2D2))
						{
							diagnostic = $"child={item2.GetPath()}, frame={i}, cached={snapshot2.GlobalTransform}, current={transform2D2}";
							return false;
						}
					}
				}
			}
			diagnostic = $"checkedChildren={num}, movedChildren={num2}";
			return true;
		}
		finally
		{
			root.frameIndex = frameIndex;
			root.UpdateChild();
		}
	}

	private static bool TransformApproximatelyEqual(Transform2D left, Transform2D right)
	{
		if (left.X.IsEqualApprox(right.X) && left.Y.IsEqualApprox(right.Y))
		{
			return left.Origin.IsEqualApprox(right.Origin);
		}
		return false;
	}

	private static bool TryCountDrawItems(AdobeAnimateSprite sprite, out int count)
	{
		count = 0;
		if (!GodotObject.IsInstanceValid(sprite) || !sprite.TryBuildRenderSnapshot(out var snapshot, allowUnchanged: false))
		{
			return false;
		}
		List<AdobeAnimateDrawItem> list = new List<AdobeAnimateDrawItem>();
		AdobeAnimateDrawItemBuilder.Build(snapshot, list, snapshot.Definition?.GpuPoseTextureArray);
		count = list.Count;
		return true;
	}

	private static bool ResourcePathMatches(Resource resource, string expectedPath)
	{
		if (GodotObject.IsInstanceValid(resource))
		{
			return string.Equals(resource.ResourcePath, expectedPath, StringComparison.Ordinal);
		}
		return false;
	}

	private async Task<bool> LoadProductionResources()
	{
		ResourceManager.Instance.BeginLoad();
		await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
		ResourceManager.Instance.RequireFullGameplayResourcesReady("LoadProductionResources");
		return ResourceManager.Instance.AreFullGameplayResourcesReady && ResourceManager.Instance.LateCharacterResourceLoadCount == 0;
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewImpPacketGpuPreviewMatrixRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TransformApproximatelyEqual, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Transform2D, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResourcePathMatches, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "expectedPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.TransformApproximatelyEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TransformApproximatelyEqual(VariantUtils.ConvertTo<Transform2D>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1])));
			return true;
		}
		if (method == MethodName.ResourcePathMatches && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ResourcePathMatches(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.TransformApproximatelyEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TransformApproximatelyEqual(VariantUtils.ConvertTo<Transform2D>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1])));
			return true;
		}
		if (method == MethodName.ResourcePathMatches && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ResourcePathMatches(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.TransformApproximatelyEqual)
		{
			return true;
		}
		if (method == MethodName.ResourcePathMatches)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
