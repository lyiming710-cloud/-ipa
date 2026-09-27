using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/GUI/InGame/Packet/TowerDefenseInGamePacketSlot.cs")]
public class TowerDefenseInGamePacketSlot : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetMobileMode = "SetMobileMode";

		public static readonly StringName SetContainerFootprint = "SetContainerFootprint";

		public static readonly StringName ApplyStyle = "ApplyStyle";

		public static readonly StringName GetVisualCenterGlobalPosition = "GetVisualCenterGlobalPosition";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _textureRect = "_textureRect";

		public static readonly StringName _reserveContainerFootprint = "_reserveContainerFootprint";

		public static readonly StringName isMobileSlot = "isMobileSlot";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static Texture2D _packetSilhouettePc;

	private static Texture2D _packetSilhouetteMobile;

	private TextureRect _textureRect;

	private bool _reserveContainerFootprint;

	public bool isMobileSlot;

	private static Texture2D PACKET_SILHOUETTE_PC => _packetSilhouettePc ?? (_packetSilhouettePc = GD.Load<Texture2D>("uid://d131d07o455pm"));

	private static Texture2D PACKET_SILHOUETTE_MOBILE => _packetSilhouetteMobile ?? (_packetSilhouetteMobile = GD.Load<Texture2D>("uid://celgn60f027kl"));

	public override void _Ready()
	{
		_textureRect = GetNodeOrNull<TextureRect>("TextureRect");
		ApplyStyle();
	}

	public void SetMobileMode(bool enabled)
	{
		isMobileSlot = enabled;
		if (IsNodeReady())
		{
			ApplyStyle();
		}
	}

	public void SetContainerFootprint(bool enabled)
	{
		_reserveContainerFootprint = enabled;
		if (IsNodeReady())
		{
			ApplyStyle();
		}
	}

	private void ApplyStyle()
	{
		if (_textureRect != null)
		{
			if (isMobileSlot)
			{
				CustomMinimumSize = (_reserveContainerFootprint ? new Vector2(96f, 60f) : Vector2.Zero);
				_textureRect.Texture = PACKET_SILHOUETTE_MOBILE;
				_textureRect.Size = new Vector2(96f, 60f);
				_textureRect.Position = (_reserveContainerFootprint ? Vector2.Zero : new Vector2(-48f, -30f));
				Color modulate = _textureRect.Modulate;
				modulate.A = 0.5f;
				_textureRect.Modulate = modulate;
			}
			else
			{
				CustomMinimumSize = (_reserveContainerFootprint ? new Vector2(50f, 70f) : Vector2.Zero);
				_textureRect.Texture = PACKET_SILHOUETTE_PC;
				_textureRect.Size = new Vector2(50f, 70f);
				_textureRect.Position = (_reserveContainerFootprint ? Vector2.Zero : new Vector2(-25f, -33f));
				Color modulate2 = _textureRect.Modulate;
				modulate2.A = 1f;
				_textureRect.Modulate = modulate2;
			}
		}
	}

	public Vector2 GetVisualCenterGlobalPosition()
	{
		if (!_reserveContainerFootprint)
		{
			return GlobalPosition;
		}
		return GlobalPosition + Size / 2f;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetMobileMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetContainerFootprint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyStyle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetVisualCenterGlobalPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetMobileMode && args.Count == 1)
		{
			SetMobileMode(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetContainerFootprint && args.Count == 1)
		{
			SetContainerFootprint(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyStyle && args.Count == 0)
		{
			ApplyStyle();
			ret = default;
			return true;
		}
		if (method == MethodName.GetVisualCenterGlobalPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetVisualCenterGlobalPosition());
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
		if (method == MethodName.SetMobileMode)
		{
			return true;
		}
		if (method == MethodName.SetContainerFootprint)
		{
			return true;
		}
		if (method == MethodName.ApplyStyle)
		{
			return true;
		}
		if (method == MethodName.GetVisualCenterGlobalPosition)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._textureRect)
		{
			_textureRect = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._reserveContainerFootprint)
		{
			_reserveContainerFootprint = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isMobileSlot)
		{
			isMobileSlot = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._textureRect)
		{
			value = VariantUtils.CreateFrom(in _textureRect);
			return true;
		}
		if (name == PropertyName._reserveContainerFootprint)
		{
			value = VariantUtils.CreateFrom(in _reserveContainerFootprint);
			return true;
		}
		if (name == PropertyName.isMobileSlot)
		{
			value = VariantUtils.CreateFrom(in isMobileSlot);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._textureRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._reserveContainerFootprint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isMobileSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._textureRect, Variant.From(in _textureRect));
		info.AddProperty(PropertyName._reserveContainerFootprint, Variant.From(in _reserveContainerFootprint));
		info.AddProperty(PropertyName.isMobileSlot, Variant.From(in isMobileSlot));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._textureRect, out var value))
		{
			_textureRect = value.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._reserveContainerFootprint, out var value2))
		{
			_reserveContainerFootprint = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isMobileSlot, out var value3))
		{
			isMobileSlot = value3.As<bool>();
		}
	}
}
