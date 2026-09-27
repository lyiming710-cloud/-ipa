using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewAxeZombieThrownWeaponVisualRuntimeTest.cs")]
public class BugOverviewAxeZombieThrownWeaponVisualRuntimeTest : Node
{
	private readonly struct PixelDifference(int changedPixels, int armedNonBackground, int thrownNonBackground, ulong armedHash, ulong thrownHash)
	{
		public readonly int ChangedPixels = changedPixels;

		public readonly int ArmedNonBackground = armedNonBackground;

		public readonly int ThrownNonBackground = thrownNonBackground;

		public readonly ulong ArmedHash = armedHash;

		public readonly ulong ThrownHash = thrownHash;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateControlFixture = "CreateControlFixture";

		public static readonly StringName CreateMapConfig = "CreateMapConfig";

		public static readonly StringName ColorDifference = "ColorDifference";

		public static readonly StringName HashColor = "HashColor";

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

	private const string AxeZombieScenePath = "res://Asset/Anime/Character/Zombie/Challenge/Axe/Scene/TowerDefenseZombieAxe.tscn";

	private const int AxeLayerId = 4;

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		BugOverviewAxeZombieThrownWeaponVisualControlStub control = null;
		TowerDefenseZombieAxe zombie = null;
		Image armedImage = null;
		Image thrownImage = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_0091;
				}
				TowerDefenseProjectileRegistry.Init();
				control = CreateControlFixture();
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				TowerDefenseMapConfig config = CreateMapConfig();
				TowerDefenseBattleFeatureMap value = new TowerDefenseBattleFeatureMap
				{
					config = config,
					rect = new Rect2(-100f, 0f, 1100f, 520f)
				};
				control.featureDictionary[new StringName("Map")] = value;
				zombie = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Challenge/Axe/Scene/TowerDefenseZombieAxe.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombieAxe>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(zombie), "The production Axe Zombie scene must instantiate.");
				if (!GodotObject.IsInstanceValid(zombie))
				{
					goto end_IL_0091;
				}
				zombie.editorPreviewMode = false;
				zombie.inGame = true;
				zombie.gridPos = new Vector2I(5, 3);
				zombie.GlobalPosition = new Vector2(430f, 275f);
				control.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				await WaitRenderFrames(8);
				Check(GodotObject.IsInstanceValid(zombie.instance) && GodotObject.IsInstanceValid(zombie.sprite), "The real Axe Zombie runtime and Adobe animation sprite must initialize.");
				if (!GodotObject.IsInstanceValid(zombie.instance) || !GodotObject.IsInstanceValid(zombie.sprite))
				{
					goto end_IL_0091;
				}
				AdobeAnimateSprite sprite = zombie.sprite;
				sprite.ProcessMode = ProcessModeEnum.Always;
				sprite.SetFrozenPreview(frozen: false);
				sprite.SetAnimation("Fire", loop: false);
				sprite.ResetAnimation();
				sprite.SetFrozenPreview(frozen: true);
				sprite.EnsureFrozenPreviewRenderSubmission();
				await WaitRenderFrames(8);
				bool armedLayerVisible = sprite.Get("Animation/LayerVisible/axe").AsBool();
				bool flag = sprite.TryBuildRenderSnapshot(out var snapshot, allowUnchanged: false);
				Check((zombie.hasAxe && zombie.instance.ArmorHas("Axe")) & armedLayerVisible, "Axe Zombie must begin the throw pose with its real Axe armor layer visible.");
				Check(flag && IsSnapshotLayerVisible(snapshot, 4), "The pre-throw render snapshot must publish the axe layer as visible.");
				armedImage = GetViewport().GetTexture().GetImage();
				int bulletsBefore = BulletField.Instance?.ActiveCount ?? 0;
				zombie.AnimeEvent("fire", default);
				int bulletsAfter = BulletField.Instance?.ActiveCount ?? 0;
				sprite.EnsureFrozenPreviewRenderSubmission();
				await WaitRenderFrames(12);
				bool flag2 = sprite.Get("Animation/LayerVisible/axe").AsBool();
				bool flag3 = sprite.TryBuildRenderSnapshot(out var snapshot2, allowUnchanged: false);
				Check(bulletsAfter == bulletsBefore + 1, $"The real fire event must spawn exactly one Axe projectile; before={bulletsBefore}, after={bulletsAfter}.");
				Check(!zombie.hasAxe && !zombie.instance.ArmorHas("Axe"), "The same fire event must consume the Axe gameplay and armor state immediately.");
				Check((!flag2 & flag3) && !IsSnapshotLayerVisible(snapshot2, 4), "The same fire event must hide the in-hand axe in both the sprite property and render snapshot.");
				Check(zombie.useAttackDps && zombie.attackAnimeClip == "Eat" && zombie.attackWaterAnimeClip == "WaterEat", "The disarmed Axe Zombie must switch to its configured bite attack clips.");
				thrownImage = GetViewport().GetTexture().GetImage();
				PixelDifference pixelDifference = MeasurePixelDifference(armedImage, thrownImage, zombie.GlobalPosition, GetViewport().GetVisibleRect());
				GD.Print($"AXE_ZOMBIE_THROW_VISUAL_METRIC changed={pixelDifference.ChangedPixels} armed_non_background={pixelDifference.ArmedNonBackground} thrown_non_background={pixelDifference.ThrownNonBackground} armed_hash={pixelDifference.ArmedHash:X16} thrown_hash={pixelDifference.ThrownHash:X16} layer_before={armedLayerVisible} layer_after={flag2} bullets_before={bulletsBefore} bullets_after={bulletsAfter}");
				Check(pixelDifference.ChangedPixels >= 12 && pixelDifference.ArmedHash != pixelDifference.ThrownHash, $"Removing the authored axe layer must change real Vulkan viewport pixels; changed={pixelDifference.ChangedPixels}, hashes={pixelDifference.ArmedHash:X16}/{pixelDifference.ThrownHash:X16}.");
				goto end_IL_007a;
				end_IL_0091:;
			}
			catch (Exception value2)
			{
				_failures++;
				GD.PushError($"[BugOverviewAxeZombieThrownWeaponVisualRuntimeTest] Unexpected exception: {value2}");
				goto end_IL_007a;
			}
			return;
			end_IL_007a:;
		}
		finally
		{
			armedImage?.Dispose();
			thrownImage?.Dispose();
			if (GodotObject.IsInstanceValid(zombie) && !zombie.IsQueuedForDeletion())
			{
				zombie.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			control?.featureDictionary.Clear();
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			await WaitRenderFrames(6);
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
			}
			await WaitRenderFrames(4);
		}
		bool flag4 = _failures == 0 && _checks == 10;
		GD.Print($"BUG_OVERVIEW_AXE_ZOMBIE_THROWN_WEAPON_VISUAL_RESULT passed={flag4} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag4) ? 2 : 0);
	}

	private static BugOverviewAxeZombieThrownWeaponVisualControlStub CreateControlFixture()
	{
		BugOverviewAxeZombieThrownWeaponVisualControlStub bugOverviewAxeZombieThrownWeaponVisualControlStub = new BugOverviewAxeZombieThrownWeaponVisualControlStub
		{
			Name = "AxeZombieThrownWeaponVisualControl",
			isGameRunning = false,
			isInit = true,
			characterNode = new Node2D
			{
				Name = "CharacterNode"
			}
		};
		bugOverviewAxeZombieThrownWeaponVisualControlStub.AddChild(bugOverviewAxeZombieThrownWeaponVisualControlStub.characterNode, forceReadableName: false, InternalMode.Disabled);
		return bugOverviewAxeZombieThrownWeaponVisualControlStub;
	}

	private static TowerDefenseMapConfig CreateMapConfig()
	{
		return new TowerDefenseMapConfig
		{
			mapSize = new Vector2(1000f, 520f),
			gridNum = new Vector2I(9, 5),
			gridBeginPos = new Vector2(70f, 30f),
			gridSize = new Vector2(100f, 90f),
			edge = new Vector4(0f, 0f, 1000f, 520f)
		};
	}

	private static bool IsSnapshotLayerVisible(AdobeAnimateRenderSnapshot snapshot, int layerId)
	{
		if (layerId < 0 || layerId >= snapshot.LayerVisibleCount)
		{
			return true;
		}
		return snapshot.LayerVisibleValues[layerId];
	}

	private static PixelDifference MeasurePixelDifference(Image armed, Image thrown, Vector2 center, Rect2 logicalViewport)
	{
		float num = (float)armed.GetWidth() / logicalViewport.Size.X;
		float num2 = (float)armed.GetHeight() / logicalViewport.Size.Y;
		Vector2 vector = new Vector2((center.X - logicalViewport.Position.X) * num, (center.Y - logicalViewport.Position.Y) * num2);
		int num3 = Math.Clamp((int)Math.Floor(vector.X - 100f * num), 0, armed.GetWidth());
		int num4 = Math.Clamp((int)Math.Ceiling(vector.X + 100f * num), 0, armed.GetWidth());
		int num5 = Math.Clamp((int)Math.Floor(vector.Y - 150f * num2), 0, armed.GetHeight());
		int num6 = Math.Clamp((int)Math.Ceiling(vector.Y + 80f * num2), 0, armed.GetHeight());
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		ulong num10 = 1469598103934665603uL;
		ulong num11 = 1469598103934665603uL;
		Color pixel = armed.GetPixel(0, 0);
		Color pixel2 = thrown.GetPixel(0, 0);
		for (int i = num5; i < num6; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				Color pixel3 = armed.GetPixel(j, i);
				Color pixel4 = thrown.GetPixel(j, i);
				if (ColorDifference(pixel3, pixel) > 0.05f)
				{
					num8++;
				}
				if (ColorDifference(pixel4, pixel2) > 0.05f)
				{
					num9++;
				}
				if (ColorDifference(pixel3, pixel4) > 0.08f)
				{
					num7++;
				}
				num10 = HashColor(num10, pixel3);
				num11 = HashColor(num11, pixel4);
			}
		}
		return new PixelDifference(num7, num8, num9, num10, num11);
	}

	private static float ColorDifference(Color first, Color second)
	{
		return Math.Abs(first.R - second.R) + Math.Abs(first.G - second.G) + Math.Abs(first.B - second.B) + Math.Abs(first.A - second.A);
	}

	private static ulong HashColor(ulong hash, Color color)
	{
		hash ^= color.ToRgba32();
		return hash * 1099511628211L;
	}

	private async Task WaitRenderFrames(int count)
	{
		for (int index = 0; index < count; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewAxeZombieThrownWeaponVisualRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateControlFixture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateMapConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ColorDifference, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "first", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "second", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HashColor, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "hash", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreateControlFixture && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<BugOverviewAxeZombieThrownWeaponVisualControlStub>(CreateControlFixture());
			return true;
		}
		if (method == MethodName.CreateMapConfig && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMapConfig>(CreateMapConfig());
			return true;
		}
		if (method == MethodName.ColorDifference && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(ColorDifference(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.HashColor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<ulong>(HashColor(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
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
		if (method == MethodName.CreateControlFixture && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<BugOverviewAxeZombieThrownWeaponVisualControlStub>(CreateControlFixture());
			return true;
		}
		if (method == MethodName.CreateMapConfig && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMapConfig>(CreateMapConfig());
			return true;
		}
		if (method == MethodName.ColorDifference && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(ColorDifference(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.HashColor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<ulong>(HashColor(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
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
		if (method == MethodName.CreateControlFixture)
		{
			return true;
		}
		if (method == MethodName.CreateMapConfig)
		{
			return true;
		}
		if (method == MethodName.ColorDifference)
		{
			return true;
		}
		if (method == MethodName.HashColor)
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
