using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Award/TowerDefenseAwardBase.cs")]
public class TowerDefenseAwardBase : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName Pressed = "Pressed";

		public static readonly StringName Init = "Init";

		public static readonly StringName TrySelectNextLevelFromLevelChoose = "TrySelectNextLevelFromLevelChoose";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName awardRay = "awardRay";

		public static readonly StringName awardPickupGlow = "awardPickupGlow";

		public static readonly StringName downArrow = "downArrow";

		public static readonly StringName button = "button";

		public static readonly StringName press = "press";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private AwardRay awardRay;

	private Sprite2D awardPickupGlow;

	private Sprite2D downArrow;

	private Button button;

	public bool press;

	public override void _Ready()
	{
		awardRay = GetNode<AwardRay>("%AwardRay");
		awardPickupGlow = GetNode<Sprite2D>("%AwardPickupGlow");
		downArrow = GetNode<Sprite2D>("%DownArrow");
		button = GetNode<Button>("%Button");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!press)
		{
			Vector2 screenCenterPosition = GetViewport().GetCamera2D().GetScreenCenterPosition();
			GlobalPosition = GlobalPosition.Lerp(screenCenterPosition, (float)(delta * 0.5));
			GlobalScale = GlobalScale.Lerp(Vector2.One * 1.5f, (float)(delta * 0.5));
		}
	}

	public virtual void Pressed()
	{
	}

	public virtual void Init(string value)
	{
	}

	protected void TrySelectNextLevelFromLevelChoose()
	{
		Global instance = Global.Instance;
		if (instance != null && !(instance.enterLevelMode != "LevelChoose") && !(instance.currentLevelChoose == "TryLevel") && instance.currentChapterId >= 0 && instance.currentLevelId >= 0 && TowerDefenseManager.Instance != null)
		{
			TowerDefenseManager.Instance.SetNextLevel(instance.currentLevelChoose, instance.currentChapterId, instance.currentLevelId);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Pressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TrySelectNextLevelFromLevelChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Pressed && args.Count == 0)
		{
			Pressed();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TrySelectNextLevelFromLevelChoose && args.Count == 0)
		{
			TrySelectNextLevelFromLevelChoose();
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
		if (method == MethodName.Pressed)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.TrySelectNextLevelFromLevelChoose)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.awardRay)
		{
			awardRay = VariantUtils.ConvertTo<AwardRay>(in value);
			return true;
		}
		if (name == PropertyName.awardPickupGlow)
		{
			awardPickupGlow = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.downArrow)
		{
			downArrow = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.button)
		{
			button = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName.press)
		{
			press = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.awardRay)
		{
			value = VariantUtils.CreateFrom(in awardRay);
			return true;
		}
		if (name == PropertyName.awardPickupGlow)
		{
			value = VariantUtils.CreateFrom(in awardPickupGlow);
			return true;
		}
		if (name == PropertyName.downArrow)
		{
			value = VariantUtils.CreateFrom(in downArrow);
			return true;
		}
		if (name == PropertyName.button)
		{
			value = VariantUtils.CreateFrom(in button);
			return true;
		}
		if (name == PropertyName.press)
		{
			value = VariantUtils.CreateFrom(in press);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.awardRay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.awardPickupGlow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.downArrow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.button, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.press, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.awardRay, Variant.From(in awardRay));
		info.AddProperty(PropertyName.awardPickupGlow, Variant.From(in awardPickupGlow));
		info.AddProperty(PropertyName.downArrow, Variant.From(in downArrow));
		info.AddProperty(PropertyName.button, Variant.From(in button));
		info.AddProperty(PropertyName.press, Variant.From(in press));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.awardRay, out var value))
		{
			awardRay = value.As<AwardRay>();
		}
		if (info.TryGetProperty(PropertyName.awardPickupGlow, out var value2))
		{
			awardPickupGlow = value2.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.downArrow, out var value3))
		{
			downArrow = value3.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.button, out var value4))
		{
			button = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.press, out var value5))
		{
			press = value5.As<bool>();
		}
	}
}
