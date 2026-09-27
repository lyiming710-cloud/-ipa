using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/PacketBankVirtualizedGridRuntimeTest.cs")]
public class PacketBankVirtualizedGridRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Check = "Check";

		public static readonly StringName Fail = "Fail";

		public static readonly StringName Finish = "Finish";

		public static readonly StringName ElapsedMilliseconds = "ElapsedMilliseconds";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string PacketRoot = "res://Asset/Anime/Character";

	private const string PacketBankScenePath = "res://Registry/Battle/Feature/PacketBank/PacketBank/TowerDefenseInGamePacketBank.tscn";

	private readonly List<string> _failures = new List<string>();

	private List<TowerDefensePacketConfig> _distinctConfigs = new List<TowerDefensePacketConfig>();

	public override async void _Ready()
	{
		if (!(await LoadProductionResourceIndex()))
		{
			Fail("production ResourceManager index did not load");
			Finish();
			return;
		}
		if (_distinctConfigs.Count < 107)
		{
			Fail($"expected at least 107 real packet configs, got {_distinctConfigs.Count}");
			Finish();
			return;
		}
		List<TowerDefensePacketConfig> logicalConfigs = new List<TowerDefensePacketConfig>(1000);
		for (int i = 0; i < 1000; i++)
		{
			logicalConfigs.Add(_distinctConfigs[i % _distinctConfigs.Count]);
		}
		TowerDefenseInGamePacketBank bank = ResourceLoader.Load<PackedScene>("res://Registry/Battle/Feature/PacketBank/PacketBank/TowerDefenseInGamePacketBank.tscn", null, ResourceLoader.CacheMode.Reuse)?.Instantiate<TowerDefenseInGamePacketBank>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(bank))
		{
			Fail("production PacketBank scene could not instantiate");
			Finish();
			return;
		}
		CanvasLayer packetBankLayer = new CanvasLayer
		{
			Name = "PacketBankCanvasLayer",
			Layer = 3
		};
		AddChild(packetBankLayer, forceReadableName: false, InternalMode.Disabled);
		packetBankLayer.AddChild(bank, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		long startTicks = Stopwatch.GetTimestamp();
		bank.SetVirtualizedPackets(logicalConfigs);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		double createMilliseconds = ElapsedMilliseconds(startTicks);
		int topPhysicalCount = bank.VisiblePacketCount;
		List<TowerDefenseInGamePacketShow> physicalCards = GetPhysicalCards(bank);
		int initialPreviewCount = CountPreparedPreviews(physicalCards);
		HashSet<ulong> topIds = new HashSet<ulong>();
		HashSet<string> topKeys = new HashSet<string>(StringComparer.Ordinal);
		HashSet<Vector2> hashSet = new HashSet<Vector2>();
		foreach (TowerDefenseInGamePacketShow item in physicalCards)
		{
			topIds.Add(item.GetInstanceId());
			topKeys.Add(item.config?.saveKey ?? "");
			hashSet.Add(item.Position);
		}
		Check(bank.LogicalPacketCount == 1000, $"logical count was {bank.LogicalPacketCount}, expected 1000");
		Check(topPhysicalCount == 77, $"top physical count was {topPhysicalCount}, expected 77");
		Check(physicalCards.Count == 77, $"top container child count was {physicalCards.Count}, expected 77");
		Check(initialPreviewCount == 77, $"top prepared preview count was {initialPreviewCount}, expected 77");
		Check(CountPacketShowPreviews(physicalCards) == 77, $"PacketShow node preview count was {CountPacketShowPreviews(physicalCards)}, expected 77");
		Check(hashSet.Count == 77, $"top card positions were not unique: {hashSet.Count}/77");
		Check(hashSet.Contains(Vector2.Zero), "the first card did not retain the authored center-origin layout");
		Check(Mathf.IsEqualApprox(bank.packetContainer.CustomMinimumSize.Y, 6300f), $"content height was {bank.packetContainer.CustomMinimumSize.Y}, expected 6300");
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		bank.packetBankScroll.ScrollVertical = 3500;
		bank.RefreshVirtualizedPacketBindings();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		Check(bank.packetBankScroll.ScrollVertical >= 3000, $"scroll position stayed at {bank.packetBankScroll.ScrollVertical}");
		List<TowerDefenseInGamePacketShow> physicalCards2 = GetPhysicalCards(bank);
		int num = CountPreparedPreviews(physicalCards2);
		int num2 = 0;
		HashSet<string> hashSet2 = new HashSet<string>(StringComparer.Ordinal);
		foreach (TowerDefenseInGamePacketShow item2 in physicalCards2)
		{
			if (topIds.Contains(item2.GetInstanceId()))
			{
				num2++;
			}
			hashSet2.Add(item2.config?.saveKey ?? "");
		}
		Check(bank.VisiblePacketCount == 77, $"rebound physical count was {bank.VisiblePacketCount}, expected 77");
		Check(physicalCards2.Count == 77, $"rebound container child count was {physicalCards2.Count}, expected 77");
		Check(num2 == 77, $"only {num2}/77 visible card nodes were recycled");
		Check(num == 77, $"rebound prepared preview count was {num}, expected 77");
		Check(CountPacketShowPreviews(physicalCards2) == 77, $"rebound PacketShow node preview count was {CountPacketShowPreviews(physicalCards2)}, expected 77");
		Check(!topKeys.SetEquals(hashSet2), "scrolling did not rebind the recycled cards to a new logical range");
		GD.Print($"[PacketBankVirtualGridResult] logicalCards={bank.LogicalPacketCount} topPhysical={topPhysicalCount} recycled={num2} initialPreviews={initialPreviewCount} reboundPreviews={num} contentHeight={bank.packetContainer.CustomMinimumSize.Y:F0} createMs={createMilliseconds:F3} failures={_failures.Count} passed={_failures.Count == 0}");
		bank.DisposeOwnedPackets();
		bank.QueueFree();
		packetBankLayer.QueueFree();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		Finish();
	}

	private async Task<bool> LoadProductionResourceIndex()
	{
		ResourceManager resourceManager = ResourceManager.Instance;
		if (!GodotObject.IsInstanceValid(resourceManager))
		{
			return false;
		}
		bool loaded = false;
		resourceManager.OnLoadOver += OnLoadOver;
		try
		{
			resourceManager.BeginLoad();
			for (int frame = 0; frame < 7200; frame++)
			{
				if (loaded)
				{
					break;
				}
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			}
			return loaded;
		}
		finally
		{
			resourceManager.OnLoadOver -= OnLoadOver;
		}
		void OnLoadOver()
		{
			_distinctConfigs = LoadDistinctPacketConfigs();
			int num = Math.Min(107, _distinctConfigs.Count);
			for (int i = 0; i < num; i++)
			{
				TowerDefenseManager.GetPacketSpriteScene(_distinctConfigs[i]);
			}
			resourceManager._Notification(2015);
			loaded = true;
		}
	}

	private static List<TowerDefensePacketConfig> LoadDistinctPacketConfigs()
	{
		string text = ProjectSettings.GlobalizePath("res://Asset/Anime/Character");
		string text2 = text.Replace('\\', '/').TrimEnd('/');
		string[] files = Directory.GetFiles(text, "*.tres", SearchOption.AllDirectories);
		Array.Sort(files, StringComparer.Ordinal);
		List<TowerDefensePacketConfig> list = new List<TowerDefensePacketConfig>(620);
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		string[] array = files;
		for (int i = 0; i < array.Length; i++)
		{
			string text3 = array[i].Replace('\\', '/');
			if (!text3.Contains("/Packet/", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			TowerDefensePacketConfig towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character" + text3.Substring(text2.Length), null, ResourceLoader.CacheMode.Reuse);
			if (GodotObject.IsInstanceValid(towerDefensePacketConfig?.characterConfig))
			{
				string resourcePath = towerDefensePacketConfig.characterConfig.ResourcePath;
				if (!string.IsNullOrWhiteSpace(resourcePath) && hashSet.Add(resourcePath))
				{
					list.Add(towerDefensePacketConfig);
				}
			}
		}
		return list;
	}

	private static List<TowerDefenseInGamePacketShow> GetPhysicalCards(TowerDefenseInGamePacketBank bank)
	{
		List<TowerDefenseInGamePacketShow> list = new List<TowerDefenseInGamePacketShow>(96);
		foreach (Node child in bank.packetContainer.GetChildren())
		{
			if (child is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow && GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				list.Add(towerDefenseInGamePacketShow);
			}
		}
		return list;
	}

	private static int CountPreparedPreviews(IReadOnlyList<TowerDefenseInGamePacketShow> cards)
	{
		int num = 0;
		for (int i = 0; i < cards.Count; i++)
		{
			if (cards[i].HasPreparedPreview)
			{
				num++;
			}
		}
		return num;
	}

	private static int CountPacketShowPreviews(IReadOnlyList<TowerDefenseInGamePacketShow> cards)
	{
		int num = 0;
		for (int i = 0; i < cards.Count; i++)
		{
			if (GodotObject.IsInstanceValid(cards[i]?.sprite))
			{
				num++;
			}
		}
		return num;
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
		_failures.Add(message);
		GD.PrintErr("[PacketBankVirtualGridFailure] " + message);
	}

	private void Finish()
	{
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	private static double ElapsedMilliseconds(long startTicks)
	{
		return (double)(Stopwatch.GetTimestamp() - startTicks) * 1000.0 / (double)Stopwatch.Frequency;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Fail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ElapsedMilliseconds, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "startTicks", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ElapsedMilliseconds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ElapsedMilliseconds(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ElapsedMilliseconds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ElapsedMilliseconds(VariantUtils.ConvertTo<long>(in args[0])));
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
		if (method == MethodName.ElapsedMilliseconds)
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
