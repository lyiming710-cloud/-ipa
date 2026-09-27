using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/GUI/InGame/ZombieWon/TowerDefenseZombieWon.cs")]
public class TowerDefenseZombieWon : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LevelFail = "LevelFail";

		public static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName ShowBattleFailDialogOnce = "ShowBattleFailDialogOnce";

		public static readonly StringName ShakeSprite = "ShakeSprite";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _colorRect = "_colorRect";

		public static readonly StringName _sprite = "_sprite";

		public static readonly StringName _failurePresentationStarted = "_failurePresentationStarted";

		public static readonly StringName _failureDialogCreated = "_failureDialogCreated";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private ColorRect _colorRect;

	private AdobeAnimateSprite _sprite;

	private bool _failurePresentationStarted;

	private bool _failureDialogCreated;

	public override void _Ready()
	{
		_colorRect = GetNodeOrNull<ColorRect>("%ColorRect");
		_sprite = GetNodeOrNull<AdobeAnimateSprite>("%ZombiesWonSprite");
		if (_sprite != null)
		{
			_sprite.pause = true;
			_sprite.OnAnimeCompleted += AnimeCompleted;
		}
	}

	public async void LevelFail(bool playAnime = true)
	{
		if (_failurePresentationStarted)
		{
			return;
		}
		_failurePresentationStarted = true;
		AudioManager.Instance.AudioStopAll();
		AudioManager.Instance.AudioPlay("ZombieWon", AudioManagerEnum.TYPE.MUSIC);
		if (playAnime)
		{
			if (_sprite != null)
			{
				_sprite.Visible = true;
				_sprite.pause = false;
			}
			CreateTween().TweenProperty(_colorRect, "modulate:a", 0.5, 1.0);
			await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);
			AudioManager.Instance.AudioPlay("CrazyDaveScream", AudioManagerEnum.TYPE.MUSIC);
			ShakeSprite();
		}
		else
		{
			ShowBattleFailDialogOnce();
		}
	}

	public void AnimeCompleted(string clip)
	{
		if (!(clip != "Idle") && _failurePresentationStarted)
		{
			if (_sprite != null)
			{
				_sprite.Visible = false;
			}
			ShowBattleFailDialogOnce();
		}
	}

	private void ShowBattleFailDialogOnce()
	{
		if (!_failureDialogCreated && GodotObject.IsInstanceValid(DialogManager.Instance))
		{
			_failureDialogCreated = true;
			DialogManager.Instance.DialogCreate("BattleFail");
		}
	}

	private async void ShakeSprite()
	{
		for (int i = 0; i < 5; i++)
		{
			if (_sprite != null)
			{
				_sprite.Position = new Vector2(540f, 300f) + new Vector2(GD.RandRange(-10, 10), GD.RandRange(-10, 10));
			}
			await ToSignal(GetTree().CreateTimer(0.05), SceneTreeTimer.SignalName.Timeout);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LevelFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "playAnime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowBattleFailDialogOnce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShakeSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.LevelFail && args.Count == 1)
		{
			LevelFail(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowBattleFailDialogOnce && args.Count == 0)
		{
			ShowBattleFailDialogOnce();
			ret = default;
			return true;
		}
		if (method == MethodName.ShakeSprite && args.Count == 0)
		{
			ShakeSprite();
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
		if (method == MethodName.LevelFail)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.ShowBattleFailDialogOnce)
		{
			return true;
		}
		if (method == MethodName.ShakeSprite)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._colorRect)
		{
			_colorRect = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			_sprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._failurePresentationStarted)
		{
			_failurePresentationStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._failureDialogCreated)
		{
			_failureDialogCreated = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._colorRect)
		{
			value = VariantUtils.CreateFrom(in _colorRect);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			value = VariantUtils.CreateFrom(in _sprite);
			return true;
		}
		if (name == PropertyName._failurePresentationStarted)
		{
			value = VariantUtils.CreateFrom(in _failurePresentationStarted);
			return true;
		}
		if (name == PropertyName._failureDialogCreated)
		{
			value = VariantUtils.CreateFrom(in _failureDialogCreated);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._colorRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._failurePresentationStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._failureDialogCreated, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._colorRect, Variant.From(in _colorRect));
		info.AddProperty(PropertyName._sprite, Variant.From(in _sprite));
		info.AddProperty(PropertyName._failurePresentationStarted, Variant.From(in _failurePresentationStarted));
		info.AddProperty(PropertyName._failureDialogCreated, Variant.From(in _failureDialogCreated));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._colorRect, out var value))
		{
			_colorRect = value.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName._sprite, out var value2))
		{
			_sprite = value2.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._failurePresentationStarted, out var value3))
		{
			_failurePresentationStarted = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._failureDialogCreated, out var value4))
		{
			_failureDialogCreated = value4.As<bool>();
		}
	}
}
