using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateCpuNativeSpriteProbe.cs")]
public class AdobeAnimateCpuNativeSpriteProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RecordDisposition = "RecordDisposition";

		public static readonly StringName CreateTexture = "CreateTexture";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";

		public static readonly StringName _root = "_root";

		public static readonly StringName _checks = "_checks";

		public static readonly StringName _merged = "_merged";

		public static readonly StringName _native = "_native";

		public static readonly StringName _ignored = "_ignored";

		public static readonly StringName _cpuFallbackRoots = "_cpuFallbackRoots";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string Result = "ADOBE_ANIMATE_CPU_NATIVE_SPRITE_RESULT";

	private const string SharedTexturePath = "res://Asset/Texture/Character/Effect/Icetrap.png";

	private const string AnimationScenePath = "res://Asset/Anime/Character/Zombie/Challenge/JacksonX/ZombieJacksonX.tscn";

	private readonly List<string> _failures = new List<string>();

	private readonly List<Node> _ownedNodes = new List<Node>();

	private readonly List<AdobeAnimateExternalVisualHandle> _externalHandles = new List<AdobeAnimateExternalVisualHandle>();

	private AdobeAnimateRenderBackend _originalBackend;

	private AdobeAnimateSprite _root;

	private int _checks;

	private int _merged;

	private int _native;

	private int _ignored;

	private int _cpuFallbackRoots;

	public override async void _Ready()
	{
		try
		{
			if (Global.Instance == null)
			{
				throw new InvalidOperationException("Global autoload is unavailable.");
			}
			_originalBackend = Global.Instance.adobeAnimateRenderBackend;
			await RunProbe();
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		finally
		{
			await Cleanup();
		}
		bool flag = _failures.Count == 0 && _checks == 10;
		GD.Print($"{"ADOBE_ANIMATE_CPU_NATIVE_SPRITE_RESULT"} passed={flag} checks={_checks} failures={_failures.Count} merged={_merged} native={_native} ignored={_ignored} cpuFallbackRoots={_cpuFallbackRoots}");
		foreach (string failure in _failures)
		{
			GD.PrintErr("ADOBE_ANIMATE_CPU_NATIVE_SPRITE_RESULT failure=" + failure);
		}
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task RunProbe()
	{
		PackedScene packedScene = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Challenge/JacksonX/ZombieJacksonX.tscn");
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			throw new InvalidOperationException("JacksonX animation scene must load.");
		}
		Node2D node2D = new Node2D
		{
			Name = "CpuNativeSpriteHost"
		};
		AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
		_ownedNodes.Add(node2D);
		_root = packedScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		_root.Name = "CpuNativeSpriteRoot";
		node2D.AddChild(_root, forceReadableName: false, InternalMode.Disabled);
		AdobeAnimateSlot nodeOrNull = _root.GetNodeOrNull<AdobeAnimateSlot>("HeadSlot");
		if (!GodotObject.IsInstanceValid(nodeOrNull))
		{
			throw new InvalidOperationException("JacksonX HeadSlot fixture must exist.");
		}
		Texture2D texture2D = GD.Load<Texture2D>("res://Asset/Texture/Character/Effect/Icetrap.png");
		Sprite2D mergedSprite = new Sprite2D
		{
			Name = "MergedManagedSprite",
			Texture = texture2D,
			Visible = true
		};
		nodeOrNull.AddChild(mergedSprite, forceReadableName: false, InternalMode.Disabled);
		Sprite2D materialNative = new Sprite2D
		{
			Name = "MaterialNativeSprite",
			Texture = texture2D,
			Visible = true,
			Material = new ShaderMaterial()
		};
		Sprite2D foreignNative = new Sprite2D
		{
			Name = "ForeignNativeSprite",
			Texture = CreateTexture(Colors.Magenta),
			Visible = true
		};
		Sprite2D hidden = new Sprite2D
		{
			Name = "HiddenExternalSprite",
			Texture = CreateTexture(Colors.Cyan),
			Visible = false
		};
		node2D.AddChild(materialNative, forceReadableName: false, InternalMode.Disabled);
		node2D.AddChild(foreignNative, forceReadableName: false, InternalMode.Disabled);
		node2D.AddChild(hidden, forceReadableName: false, InternalMode.Disabled);
		int materialOriginalZIndex = materialNative.ZIndex;
		bool materialOriginalZRelative = materialNative.ZAsRelative;
		int foreignOriginalZIndex = foreignNative.ZIndex;
		bool foreignOriginalZRelative = foreignNative.ZAsRelative;
		AdobeAnimateExternalVisualDescriptor descriptor = new AdobeAnimateExternalVisualDescriptor(AdobeAnimateExternalVisualAttachmentMode.Root, AdobeAnimateExternalVisualDrawBand.InFrontOfAnimation);
		_externalHandles.Add(_root.RegisterExternalVisual(materialNative, descriptor));
		_externalHandles.Add(_root.RegisterExternalVisual(foreignNative, descriptor));
		_externalHandles.Add(_root.RegisterExternalVisual(hidden, descriptor));
		TextureLayered textureLayered = AdobeAnimateGlobalAtlasCache.PreloadVisualTextureArray();
		Check(GodotObject.IsInstanceValid(textureLayered) && textureLayered.GetRid().IsValid && GodotObject.IsInstanceValid(texture2D) && AdobeAnimateGlobalAtlasCache.TryGetExternalTextureAllocation(texture2D, new Rect2(Vector2.Zero, texture2D.GetSize()), out var allocation) && allocation.UsesTextureArray && allocation.TextureArrayRid == textureLayered.GetRid(), "The managed fixture texture must use the shared visual atlas array.");
		AdobeAnimateCpuVisualClassification adobeAnimateCpuVisualClassification = AdobeAnimateCpuVisualClassifier.ClassifyManagedSlot(new AdobeAnimateManagedSlotSprite(nodeOrNull, mergedSprite), textureLayered.GetRid(), default(AdobeAnimateDrawItem), default(AdobeAnimateSortPath));
		AdobeAnimateCpuVisualClassification adobeAnimateCpuVisualClassification2 = ClassifyExternal(_externalHandles[0], textureLayered.GetRid());
		AdobeAnimateCpuVisualClassification adobeAnimateCpuVisualClassification3 = ClassifyExternal(_externalHandles[1], textureLayered.GetRid());
		AdobeAnimateCpuVisualClassification adobeAnimateCpuVisualClassification4 = ClassifyExternal(_externalHandles[2], textureLayered.GetRid());
		RecordDisposition(adobeAnimateCpuVisualClassification.Disposition);
		RecordDisposition(adobeAnimateCpuVisualClassification2.Disposition);
		RecordDisposition(adobeAnimateCpuVisualClassification3.Disposition);
		RecordDisposition(adobeAnimateCpuVisualClassification4.Disposition);
		Check(adobeAnimateCpuVisualClassification.Disposition == AdobeAnimateCpuVisualDisposition.MergedMesh, "The compatible managed Slot sprite must merge into the CPU root Mesh.");
		Check(adobeAnimateCpuVisualClassification2.Disposition == AdobeAnimateCpuVisualDisposition.NativeSprite, "The material-bearing Sprite2D must remain native.");
		Check(adobeAnimateCpuVisualClassification3.Disposition == AdobeAnimateCpuVisualDisposition.NativeSprite, "The foreign-texture Sprite2D must remain native.");
		Check(adobeAnimateCpuVisualClassification4.Disposition == AdobeAnimateCpuVisualDisposition.IgnoredInvisible, "The hidden registered visual must be ignored.");
		Check(_merged + _native == 3 && _ignored == 1, "Every mixed-visual fixture must receive exactly one classification.");
		_root.SetAnimation("Walk");
		Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.CpuPose;
		await WaitProcessFrames(5);
		AdobeAnimateCrowdAggregateStats aggregateRenderStats = AdobeAnimateRenderManager.GetAggregateRenderStats();
		_cpuFallbackRoots = aggregateRenderStats.CpuFallbackRoots;
		Check(aggregateRenderStats.CpuRoots == 1 && aggregateRenderStats.CpuFallbackRoots == 0 && aggregateRenderStats.CrowdRoots == 0 && aggregateRenderStats.FallbackRoots == aggregateRenderStats.CpuRoots, $"The live CPU Pose transaction must publish through the ordered fallback batcher (cpuRoots={aggregateRenderStats.CpuRoots}, cpuFallback={aggregateRenderStats.CpuFallbackRoots}, crowd={aggregateRenderStats.CrowdRoots}, fallback={aggregateRenderStats.FallbackRoots}).");
		Check(AdobeAnimateManagedSprite2D.IsCrowdManaged(mergedSprite) && !mergedSprite.Visible, "The merged managed Sprite2D must be hidden by CPU Pose batch takeover.");
		Check(!materialNative.Visible && foreignNative.Visible && AdobeAnimateManagedSprite2D.IsCrowdManaged(materialNative) && !AdobeAnimateManagedSprite2D.IsCrowdManaged(foreignNative), $"CPU Pose must merge the shared-atlas visual and leave the foreign-texture visual native (materialVisible={materialNative.Visible}, foreignVisible={foreignNative.Visible}, materialManaged={AdobeAnimateManagedSprite2D.IsCrowdManaged(materialNative)}, foreignManaged={AdobeAnimateManagedSprite2D.IsCrowdManaged(foreignNative)}).");
		_root.GetExternalVisualDiagnosticStats(out var activeExternal, out var nativeExternal);
		Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
		await WaitProcessFrames(3);
		Check(!hidden.Visible && activeExternal == 2 && nativeExternal == 1 && materialNative.ZIndex == materialOriginalZIndex && materialNative.ZAsRelative == materialOriginalZRelative && foreignNative.ZIndex == foreignOriginalZIndex && foreignNative.ZAsRelative == foreignOriginalZRelative, $"The ignored visual must stay hidden and GPU publication must restore native order states (active={activeExternal}, native={nativeExternal}).");
	}

	private AdobeAnimateCpuVisualClassification ClassifyExternal(AdobeAnimateExternalVisualHandle handle, Rid atlasRid)
	{
		if (!_root.TryGetExternalVisualForRender(handle, out var visual))
		{
			throw new InvalidOperationException("Registered external visual snapshot must resolve.");
		}
		return AdobeAnimateCpuVisualClassifier.ClassifyExternalVisual(in visual, atlasRid, default(AdobeAnimateDrawItem), default(AdobeAnimateSortPath));
	}

	private void RecordDisposition(AdobeAnimateCpuVisualDisposition disposition)
	{
		switch (disposition)
		{
		case AdobeAnimateCpuVisualDisposition.MergedMesh:
			_merged++;
			break;
		case AdobeAnimateCpuVisualDisposition.NativeSprite:
			_native++;
			break;
		case AdobeAnimateCpuVisualDisposition.IgnoredInvisible:
			_ignored++;
			break;
		}
	}

	private async Task Cleanup()
	{
		if (Global.Instance != null)
		{
			Global.Instance.adobeAnimateRenderBackend = _originalBackend;
		}
		if (GodotObject.IsInstanceValid(_root))
		{
			for (int i = 0; i < _externalHandles.Count; i++)
			{
				_root.UnregisterExternalVisual(_externalHandles[i]);
			}
		}
		for (int num = _ownedNodes.Count - 1; num >= 0; num--)
		{
			Node node = _ownedNodes[num];
			if (GodotObject.IsInstanceValid(node) && !node.IsQueuedForDeletion())
			{
				node.QueueFree();
			}
		}
		await WaitProcessFrames(2);
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private static ImageTexture CreateTexture(Color color)
	{
		using Image image = Image.CreateEmpty(2, 2, useMipmaps: false, Image.Format.Rgba8);
		image.Fill(color);
		return ImageTexture.CreateFromImage(image);
	}

	private void Check(bool condition, string failure)
	{
		_checks++;
		if (!condition)
		{
			_failures.Add(failure);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RecordDisposition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "disposition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ImageTexture"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.RecordDisposition && args.Count == 1)
		{
			RecordDisposition(VariantUtils.ConvertTo<AdobeAnimateCpuVisualDisposition>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ImageTexture>(CreateTexture(VariantUtils.ConvertTo<Color>(in args[0])));
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
		if (method == MethodName.CreateTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ImageTexture>(CreateTexture(VariantUtils.ConvertTo<Color>(in args[0])));
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
		if (method == MethodName.RecordDisposition)
		{
			return true;
		}
		if (method == MethodName.CreateTexture)
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
		if (name == PropertyName._originalBackend)
		{
			_originalBackend = VariantUtils.ConvertTo<AdobeAnimateRenderBackend>(in value);
			return true;
		}
		if (name == PropertyName._root)
		{
			_root = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._merged)
		{
			_merged = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._native)
		{
			_native = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._ignored)
		{
			_ignored = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cpuFallbackRoots)
		{
			_cpuFallbackRoots = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._originalBackend)
		{
			value = VariantUtils.CreateFrom(in _originalBackend);
			return true;
		}
		if (name == PropertyName._root)
		{
			value = VariantUtils.CreateFrom(in _root);
			return true;
		}
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._merged)
		{
			value = VariantUtils.CreateFrom(in _merged);
			return true;
		}
		if (name == PropertyName._native)
		{
			value = VariantUtils.CreateFrom(in _native);
			return true;
		}
		if (name == PropertyName._ignored)
		{
			value = VariantUtils.CreateFrom(in _ignored);
			return true;
		}
		if (name == PropertyName._cpuFallbackRoots)
		{
			value = VariantUtils.CreateFrom(in _cpuFallbackRoots);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._originalBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._root, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._merged, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._native, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._ignored, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cpuFallbackRoots, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._originalBackend, Variant.From(in _originalBackend));
		info.AddProperty(PropertyName._root, Variant.From(in _root));
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._merged, Variant.From(in _merged));
		info.AddProperty(PropertyName._native, Variant.From(in _native));
		info.AddProperty(PropertyName._ignored, Variant.From(in _ignored));
		info.AddProperty(PropertyName._cpuFallbackRoots, Variant.From(in _cpuFallbackRoots));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._originalBackend, out var value))
		{
			_originalBackend = value.As<AdobeAnimateRenderBackend>();
		}
		if (info.TryGetProperty(PropertyName._root, out var value2))
		{
			_root = value2.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._checks, out var value3))
		{
			_checks = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._merged, out var value4))
		{
			_merged = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._native, out var value5))
		{
			_native = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._ignored, out var value6))
		{
			_ignored = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cpuFallbackRoots, out var value7))
		{
			_cpuFallbackRoots = value7.As<int>();
		}
	}
}
