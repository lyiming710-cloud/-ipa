using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/GemMatch/GemBoard/GemPiece.cs")]
public class GemPiece : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName Setup = "Setup";

		public static readonly StringName SetGridPos = "SetGridPos";

		public static readonly StringName SetAsHole = "SetAsHole";

		public static readonly StringName PlayRemoveAnimation = "PlayRemoveAnimation";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName gridPos = "gridPos";

		public static readonly StringName characterKey = "characterKey";

		public static readonly StringName character = "character";

		public static readonly StringName isAnimating = "isAnimating";

		public static readonly StringName isHole = "isHole";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	public const double RemoveAnimationDuration = 0.2;

	public Vector2I gridPos = new Vector2I(-1, -1);

	public StringName characterKey = "";

	public TowerDefenseCharacter character;

	public bool isAnimating;

	public bool isHole;

	public void Setup(Vector2I _gridPos, StringName _characterKey)
	{
		gridPos = _gridPos;
		characterKey = _characterKey;
	}

	public void SetGridPos(Vector2I pos)
	{
		gridPos = pos;
	}

	public void SetAsHole()
	{
		isHole = true;
		characterKey = "";
		character = null;
		for (int num = GetChildCount() - 1; num >= 0; num--)
		{
			if (GetChild(num) is ColorRect colorRect)
			{
				colorRect.QueueFree();
			}
		}
	}

	public void PlayRemoveAnimation()
	{
		isAnimating = true;
		TowerDefenseCharacter towerDefenseCharacter = character;
		character = null;
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			QueueFree();
			return;
		}
		Tween tween = CreateTween();
		tween.SetEase(Tween.EaseType.In);
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(towerDefenseCharacter, "scale", Vector2.Zero, 0.2);
		tween.TweenCallback(Callable.From(QueueFree));
		towerDefenseCharacter.DestroyWithVisualDelay(0.2);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.Setup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "_gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "_characterKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetGridPos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAsHole, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayRemoveAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Setup && args.Count == 2)
		{
			Setup(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetGridPos && args.Count == 1)
		{
			SetGridPos(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAsHole && args.Count == 0)
		{
			SetAsHole();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayRemoveAnimation && args.Count == 0)
		{
			PlayRemoveAnimation();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Setup)
		{
			return true;
		}
		if (method == MethodName.SetGridPos)
		{
			return true;
		}
		if (method == MethodName.SetAsHole)
		{
			return true;
		}
		if (method == MethodName.PlayRemoveAnimation)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.gridPos)
		{
			gridPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.characterKey)
		{
			characterKey = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.character)
		{
			character = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.isAnimating)
		{
			isAnimating = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isHole)
		{
			isHole = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.gridPos)
		{
			value = VariantUtils.CreateFrom(in gridPos);
			return true;
		}
		if (name == PropertyName.characterKey)
		{
			value = VariantUtils.CreateFrom(in characterKey);
			return true;
		}
		if (name == PropertyName.character)
		{
			value = VariantUtils.CreateFrom(in character);
			return true;
		}
		if (name == PropertyName.isAnimating)
		{
			value = VariantUtils.CreateFrom(in isAnimating);
			return true;
		}
		if (name == PropertyName.isHole)
		{
			value = VariantUtils.CreateFrom(in isHole);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.characterKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.character, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isAnimating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isHole, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.gridPos, Variant.From(in gridPos));
		info.AddProperty(PropertyName.characterKey, Variant.From(in characterKey));
		info.AddProperty(PropertyName.character, Variant.From(in character));
		info.AddProperty(PropertyName.isAnimating, Variant.From(in isAnimating));
		info.AddProperty(PropertyName.isHole, Variant.From(in isHole));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.gridPos, out var value))
		{
			gridPos = value.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.characterKey, out var value2))
		{
			characterKey = value2.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.character, out var value3))
		{
			character = value3.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.isAnimating, out var value4))
		{
			isAnimating = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isHole, out var value5))
		{
			isHole = value5.As<bool>();
		}
	}
}
