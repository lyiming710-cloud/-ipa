using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

internal sealed class XWAttackContactOverlay : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName RefreshDefinition = "RefreshDefinition";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName GetGeometryProfile = "GetGeometryProfile";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName RedrawRevision = "RedrawRevision";

		public static readonly StringName TargetDummyCount = "TargetDummyCount";

		public static readonly StringName _definition = "_definition";

		public static readonly StringName _owner = "_owner";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private static readonly Vector2[] DummyPositions = new Vector2[6]
	{
		new Vector2(82f, -58f),
		new Vector2(114f, -28f),
		new Vector2(146f, 0f),
		new Vector2(178f, 28f),
		new Vector2(210f, 58f),
		new Vector2(242f, 88f)
	};

	private AttackComponentDefinition _definition;

	private AttackComponent _runtime;

	private Node _owner;

	public int RedrawRevision { get; private set; }

	public int TargetDummyCount => DummyPositions.Length;

	public event Action<int> GeometryProfileRequested;

	public XWAttackContactOverlay()
	{
		SetProcess(enable: false);
		SetPhysicsProcess(enable: false);
		SetProcessInput(enable: false);
	}

	public void Bind(AttackComponentDefinition definition, Node owner, AttackComponent runtime)
	{
		_definition = definition;
		_owner = ((GodotObject.IsInstanceValid(owner) && owner.IsInsideTree()) ? owner : null);
		_runtime = ((runtime != null && !runtime.IsReleased) ? runtime : null);
		RedrawRevision++;
		QueueRedraw();
	}

	public void RefreshDefinition(AttackComponentDefinition definition)
	{
		_definition = definition;
		RedrawRevision++;
		QueueRedraw();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!Visible || !IsProcessingInput() || !(inputEvent is InputEventMouseButton inputEventMouseButton) || inputEventMouseButton.ButtonIndex != MouseButton.Left || !inputEventMouseButton.Pressed)
		{
			return;
		}
		Vector2 vector = ToLocal(inputEventMouseButton.Position);
		for (int i = 0; i < DummyPositions.Length; i++)
		{
			if (!(vector.DistanceTo(DummyPositions[i]) > 25f))
			{
				GeometryProfileRequested?.Invoke(i);
				GetViewport()?.SetInputAsHandled();
				break;
			}
		}
	}

	public override void _Draw()
	{
		if (GodotObject.IsInstanceValid(_owner))
		{
			AttackComponent runtime = _runtime;
			if (runtime != null && !runtime.IsReleased)
			{
				_runtime.DrawCheckAreaCollisionPreview(this);
			}
		}
		if (GodotObject.IsInstanceValid(_definition))
		{
			for (int i = -1; i <= 1; i++)
			{
				float y = (float)i * 42f;
				DrawLine(new Vector2(-42f, y), new Vector2(232f, y), new Color("567082", (i == 0) ? 0.54f : 0.26f), (i != 0) ? 1 : 2);
			}
			int geometryProfile = GetGeometryProfile();
			for (int j = 0; j < DummyPositions.Length; j++)
			{
				Vector2 vector = DummyPositions[j];
				bool flag = j == geometryProfile;
				Color color = (flag ? new Color("b04c3c", 0.88f) : new Color("334652", 0.72f));
				Color color2 = (flag ? new Color("ffb05e") : new Color("83a9ba"));
				DrawCircle(vector + new Vector2(0f, -12f), 10f, color);
				DrawRect(new Rect2(vector + new Vector2(-13f, 0f), new Vector2(26f, 30f)), color);
				DrawCircle(vector + new Vector2(0f, -12f), 10f, color2, filled: false, flag ? 3f : 1.5f);
				DrawLine(new Vector2(vector.X - 13f, vector.Y), new Vector2(vector.X - 13f, vector.Y + 30f), color2, flag ? 3f : 1.5f);
			}
			Vector2 vector2 = DummyPositions[geometryProfile];
			DrawLine(new Vector2(16f, 0f), vector2 - new Vector2(18f, 0f), new Color("ff705a"), 3f);
			DrawCircle(vector2 - new Vector2(18f, 0f), 5f, new Color("ffe0a3"));
			Font fallbackFont = ThemeDB.FallbackFont;
			DrawString(fallbackFont, new Vector2(80f, -72f), _definition.checkAll ? "全体处理 / 无需持续目标" : "点击假人切换目标空间组合", HorizontalAlignment.Left, -1f, 12, new Color("ffd4a1"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}
	}

	private int GetGeometryProfile()
	{
		if (!_definition.checkGrid)
		{
			return _definition.checkLine ? 1 : 0;
		}
		if (!_definition.checkLine)
		{
			if (!_definition.useCheckAreaGridColumn)
			{
				return 2;
			}
			return 3;
		}
		if (!_definition.useCheckAreaGridColumn)
		{
			return 4;
		}
		return 5;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.RefreshDefinition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGeometryProfile, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RefreshDefinition && args.Count == 1)
		{
			RefreshDefinition(VariantUtils.ConvertTo<AttackComponentDefinition>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.GetGeometryProfile && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetGeometryProfile());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.RefreshDefinition)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.GetGeometryProfile)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.RedrawRevision)
		{
			RedrawRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._definition)
		{
			_definition = VariantUtils.ConvertTo<AttackComponentDefinition>(in value);
			return true;
		}
		if (name == PropertyName._owner)
		{
			_owner = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.RedrawRevision)
		{
			from = RedrawRevision;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TargetDummyCount)
		{
			from = TargetDummyCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._definition)
		{
			value = VariantUtils.CreateFrom(in _definition);
			return true;
		}
		if (name == PropertyName._owner)
		{
			value = VariantUtils.CreateFrom(in _owner);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._definition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._owner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RedrawRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TargetDummyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.RedrawRevision, Variant.From<int>(RedrawRevision));
		info.AddProperty(PropertyName._definition, Variant.From(in _definition));
		info.AddProperty(PropertyName._owner, Variant.From(in _owner));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.RedrawRevision, out var value))
		{
			RedrawRevision = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._definition, out var value2))
		{
			_definition = value2.As<AttackComponentDefinition>();
		}
		if (info.TryGetProperty(PropertyName._owner, out var value3))
		{
			_owner = value3.As<Node>();
		}
	}
}
