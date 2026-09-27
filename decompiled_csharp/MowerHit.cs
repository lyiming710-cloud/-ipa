using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Mower/MowerHit.cs")]
public class MowerHit : TowerDefenseGroundItemBase
{
	public new class MethodName : TowerDefenseGroundItemBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName AnimeComplete = "AnimeComplete";
	}

	public new class PropertyName : TowerDefenseGroundItemBase.PropertyName
	{
		public static readonly StringName _slot = "_slot";

		public static readonly StringName _mowerHitSprite = "_mowerHitSprite";

		public static readonly StringName _isReady = "_isReady";

		public static readonly StringName character = "character";
	}

	public new class SignalName : TowerDefenseGroundItemBase.SignalName
	{
	}

	private AdobeAnimateSlot _slot;

	private AdobeAnimateSprite _mowerHitSprite;

	public bool _isReady;

	public TowerDefenseCharacter character;

	public override void _Ready()
	{
		_slot = GetNodeOrNull<AdobeAnimateSlot>("%AdobeAnimateSlot");
		_mowerHitSprite = GetNodeOrNull<AdobeAnimateSprite>("MowerHitSprite");
		if (GodotObject.IsInstanceValid(_mowerHitSprite))
		{
			_mowerHitSprite.OnAnimeCompleted += AnimeComplete;
			_mowerHitSprite.frameIndex = 0;
		}
	}

	public bool Init(TowerDefenseCharacter target)
	{
		if (!GodotObject.IsInstanceValid(target) || !GodotObject.IsInstanceValid(_slot) || !GodotObject.IsInstanceValid(_mowerHitSprite))
		{
			return false;
		}
		character = target;
		character.mowerDeathVisualOwned = true;
		Vector2 globalScale = character.GlobalScale;
		character.Reparent(_slot);
		character.SetLogicalGlobalPosition(_slot.GlobalPosition);
		character.GlobalScale = globalScale;
		character.Rotation = 0f;
		if (GodotObject.IsInstanceValid(character.shadowSprite))
		{
			character.shadowSprite.Visible = false;
		}
		TowerDefenseGroundItemBase towerDefenseGroundItemBase = character;
		if (towerDefenseGroundItemBase != null)
		{
			itemLayer = towerDefenseGroundItemBase.itemLayer;
		}
		_mowerHitSprite.SetAnimation("animation", loop: false);
		_mowerHitSprite.pause = false;
		_isReady = true;
		return true;
	}

	public void AnimeComplete(string clip)
	{
		if (_isReady)
		{
			QueueFree();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeComplete, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Init && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Init(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.AnimeComplete && args.Count == 1)
		{
			AnimeComplete(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.AnimeComplete)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._slot)
		{
			_slot = VariantUtils.ConvertTo<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._mowerHitSprite)
		{
			_mowerHitSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._isReady)
		{
			_isReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.character)
		{
			character = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._slot)
		{
			value = VariantUtils.CreateFrom(in _slot);
			return true;
		}
		if (name == PropertyName._mowerHitSprite)
		{
			value = VariantUtils.CreateFrom(in _mowerHitSprite);
			return true;
		}
		if (name == PropertyName._isReady)
		{
			value = VariantUtils.CreateFrom(in _isReady);
			return true;
		}
		if (name == PropertyName.character)
		{
			value = VariantUtils.CreateFrom(in character);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._slot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mowerHitSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.character, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._slot, Variant.From(in _slot));
		info.AddProperty(PropertyName._mowerHitSprite, Variant.From(in _mowerHitSprite));
		info.AddProperty(PropertyName._isReady, Variant.From(in _isReady));
		info.AddProperty(PropertyName.character, Variant.From(in character));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._slot, out var value))
		{
			_slot = value.As<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._mowerHitSprite, out var value2))
		{
			_mowerHitSprite = value2.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._isReady, out var value3))
		{
			_isReady = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.character, out var value4))
		{
			character = value4.As<TowerDefenseCharacter>();
		}
	}
}
