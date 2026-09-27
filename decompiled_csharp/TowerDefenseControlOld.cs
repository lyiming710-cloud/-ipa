using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Scene/TowerDefesne/TowerDefenseOld/TowerDefenseControlOld.cs")]
public class TowerDefenseControlOld : TowerDefenseControl
{
	public new class MethodName : TowerDefenseControl.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName OnGlobalFeatureChanged = "OnGlobalFeatureChanged";

		public static readonly StringName RefreshShopVisibility = "RefreshShopVisibility";

		public static readonly StringName ProcessZombieWonArea = "ProcessZombieWonArea";

		public static readonly StringName HandleZombieWon = "HandleZombieWon";

		public static readonly StringName ShopButtonPressed = "ShopButtonPressed";

		public static readonly StringName AlmanacButtonPressed = "AlmanacButtonPressed";
	}

	public new class PropertyName : TowerDefenseControl.PropertyName
	{
		public static readonly StringName _zombieWonArea = "_zombieWonArea";

		public static readonly StringName _shopButton = "_shopButton";
	}

	public new class SignalName : TowerDefenseControl.SignalName
	{
	}

	private AabbArea2D _zombieWonArea;

	private NinePatchButtonBase _shopButton;

	private readonly HashSet<TowerDefenseZombie> _zombieWonOverlaps = new HashSet<TowerDefenseZombie>();

	private readonly HashSet<TowerDefenseZombie> _zombieWonScratch = new HashSet<TowerDefenseZombie>();

	public override void _Ready()
	{
		base._Ready();
		_zombieWonArea = GetNode<AabbArea2D>("%ZombieWonArea");
		_shopButton = GetNode<NinePatchButtonBase>("GUILayerFront/ShopButton");
		RefreshShopVisibility();
		_shopButton.OnPressed += ShopButtonPressed;
		if (GlobalFeatureManager.Instance != null)
		{
			GlobalFeatureManager.Instance.FeatureChanged += OnGlobalFeatureChanged;
		}
		GetNode<NinePatchButtonBase>("GUILayerFront/AlmanacButton").OnPressed += AlmanacButtonPressed;
		GetNode<SpriteBrightButton>("%OptionButton").OnPressed += OptionButtonPressed;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		ProcessZombieWonArea();
	}

	public override void _ExitTree()
	{
		if (GlobalFeatureManager.Instance != null)
		{
			GlobalFeatureManager.Instance.FeatureChanged -= OnGlobalFeatureChanged;
		}
		base._ExitTree();
	}

	private void OnGlobalFeatureChanged(string featureId, int _oldValue, int _newValue)
	{
		if (featureId == "Shop")
		{
			RefreshShopVisibility();
		}
	}

	private void RefreshShopVisibility()
	{
		if (GodotObject.IsInstanceValid(_shopButton))
		{
			_shopButton.Visible = GlobalFeatureManager.Instance?.IsUnlocked("Shop") ?? false;
		}
	}

	private void ProcessZombieWonArea()
	{
		if (!TowerDefenseManager._IsGameRunning())
		{
			return;
		}
		if (!GodotObject.IsInstanceValid(_zombieWonArea) || _zombieWonArea.ProcessMode == ProcessModeEnum.Disabled)
		{
			_zombieWonOverlaps.Clear();
		}
		else
		{
			if (TowerDefenseManager.Instance == null || TowerDefenseManager.Instance.characterRegistry == null)
			{
				return;
			}
			Rect2 checkRect = AabbShapeUtil.ComputeAreaWorldRect(_zombieWonArea);
			List<TowerDefenseCharacter> charactersIntersectingRectList = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectList(checkRect);
			_zombieWonScratch.Clear();
			for (int i = 0; i < charactersIntersectingRectList.Count; i++)
			{
				if (charactersIntersectingRectList[i] is TowerDefenseZombie towerDefenseZombie)
				{
					_zombieWonScratch.Add(towerDefenseZombie);
					if (!_zombieWonOverlaps.Contains(towerDefenseZombie))
					{
						HandleZombieWon(towerDefenseZombie);
					}
				}
			}
			_zombieWonOverlaps.RemoveWhere((TowerDefenseZombie zombie) => !GodotObject.IsInstanceValid(zombie) || !_zombieWonScratch.Contains(zombie));
			foreach (TowerDefenseZombie item in _zombieWonScratch)
			{
				_zombieWonOverlaps.Add(item);
			}
		}
	}

	private void HandleZombieWon(TowerDefenseZombie zombie)
	{
		if (GodotObject.IsInstanceValid(zombie) && !zombie.instance.die && !zombie.instance.nearDie && !zombie.instance.hypnoses && !(zombie.Scale.X < 0f))
		{
			TowerDefenseZombieWon node = GetNode<TowerDefenseZombieWon>("%TowerDefenseZombieWon");
			if (GodotObject.IsInstanceValid(node))
			{
				node.LevelFail();
			}
		}
	}

	public void ShopButtonPressed()
	{
		GlobalFeatureManager instance = GlobalFeatureManager.Instance;
		if (instance != null && instance.IsUnlocked("Shop"))
		{
			DialogManager.Instance.DialogCreate("Shop");
		}
	}

	public void AlmanacButtonPressed()
	{
		DialogManager.Instance.DialogCreate("Almanac");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGlobalFeatureChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_oldValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshShopVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessZombieWonArea, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleZombieWon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShopButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AlmanacButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGlobalFeatureChanged && args.Count == 3)
		{
			OnGlobalFeatureChanged(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshShopVisibility && args.Count == 0)
		{
			RefreshShopVisibility();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessZombieWonArea && args.Count == 0)
		{
			ProcessZombieWonArea();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleZombieWon && args.Count == 1)
		{
			HandleZombieWon(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShopButtonPressed && args.Count == 0)
		{
			ShopButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.AlmanacButtonPressed && args.Count == 0)
		{
			AlmanacButtonPressed();
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.OnGlobalFeatureChanged)
		{
			return true;
		}
		if (method == MethodName.RefreshShopVisibility)
		{
			return true;
		}
		if (method == MethodName.ProcessZombieWonArea)
		{
			return true;
		}
		if (method == MethodName.HandleZombieWon)
		{
			return true;
		}
		if (method == MethodName.ShopButtonPressed)
		{
			return true;
		}
		if (method == MethodName.AlmanacButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._zombieWonArea)
		{
			_zombieWonArea = VariantUtils.ConvertTo<AabbArea2D>(in value);
			return true;
		}
		if (name == PropertyName._shopButton)
		{
			_shopButton = VariantUtils.ConvertTo<NinePatchButtonBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._zombieWonArea)
		{
			value = VariantUtils.CreateFrom(in _zombieWonArea);
			return true;
		}
		if (name == PropertyName._shopButton)
		{
			value = VariantUtils.CreateFrom(in _shopButton);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._zombieWonArea, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shopButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._zombieWonArea, Variant.From(in _zombieWonArea));
		info.AddProperty(PropertyName._shopButton, Variant.From(in _shopButton));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._zombieWonArea, out var value))
		{
			_zombieWonArea = value.As<AabbArea2D>();
		}
		if (info.TryGetProperty(PropertyName._shopButton, out var value2))
		{
			_shopButton = value2.As<NinePatchButtonBase>();
		}
	}
}
