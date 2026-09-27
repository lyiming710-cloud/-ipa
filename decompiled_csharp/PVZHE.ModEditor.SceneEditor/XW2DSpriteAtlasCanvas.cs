using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://addons/ModEditor/SceneEditor/2D/Atlas/XW2DSpriteAtlasCanvas.cs")]
public class XW2DSpriteAtlasCanvas : Control
{
	[Signal]
	public delegate void FramePickedEventHandler(int frame);

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Bind = "Bind";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName DrawCheckerboard = "DrawCheckerboard";

		public static readonly StringName ResolveSourcePixels = "ResolveSourcePixels";

		public static readonly StringName PixelRectToDraw = "PixelRectToDraw";

		public static readonly StringName CalculateTextureDrawRect = "CalculateTextureDrawRect";

		public static readonly StringName DrawRegionMask = "DrawRegionMask";

		public static readonly StringName DrawGrid = "DrawGrid";

		public static readonly StringName DrawSelection = "DrawSelection";

		public static readonly StringName OnGuiInput = "OnGuiInput";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName Columns = "Columns";

		public static readonly StringName Rows = "Rows";

		public static readonly StringName SelectedFrame = "SelectedFrame";

		public static readonly StringName DrawnAtlasRect = "DrawnAtlasRect";

		public static readonly StringName IsGuideDensityLimited = "IsGuideDensityLimited";

		public static readonly StringName _texture = "_texture";

		public static readonly StringName _columns = "_columns";

		public static readonly StringName _rows = "_rows";

		public static readonly StringName _frame = "_frame";

		public static readonly StringName _regionEnabled = "_regionEnabled";

		public static readonly StringName _regionRect = "_regionRect";

		public static readonly StringName _drawRect = "_drawRect";
	}

	public new class SignalName : Control.SignalName
	{
		public static readonly StringName FramePicked = "FramePicked";
	}

	private const int MaximumGuideLinesPerAxis = 128;

	private Texture2D _texture;

	private int _columns = 1;

	private int _rows = 1;

	private int _frame;

	private bool _regionEnabled;

	private Rect2 _regionRect;

	private Rect2 _drawRect;

	private FramePickedEventHandler backing_FramePicked;

	public int Columns => _columns;

	public int Rows => _rows;

	public int SelectedFrame => _frame;

	public Rect2 DrawnAtlasRect
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_texture))
			{
				return default;
			}
			Vector2 size = _texture.GetSize();
			_drawRect = CalculateTextureDrawRect(size);
			return PixelRectToDraw(ResolveSourcePixels(size), size);
		}
	}

	public bool IsGuideDensityLimited
	{
		get
		{
			if (_columns <= 128)
			{
				return _rows > 128;
			}
			return true;
		}
	}

	public event FramePickedEventHandler FramePicked
	{
		add
		{
			backing_FramePicked = (FramePickedEventHandler)Delegate.Combine(backing_FramePicked, value);
		}
		remove
		{
			backing_FramePicked = (FramePickedEventHandler)Delegate.Remove(backing_FramePicked, value);
		}
	}

	public override void _Ready()
	{
		CustomMinimumSize = new Vector2(360f, 260f);
		MouseDefaultCursorShape = CursorShape.PointingHand;
		MouseFilter = MouseFilterEnum.Stop;
		SetProcess(enable: false);
		Resized += QueueRedraw;
		GuiInput += OnGuiInput;
	}

	public override void _ExitTree()
	{
		Resized -= QueueRedraw;
		GuiInput -= OnGuiInput;
	}

	public void Bind(Texture2D texture, int columns, int rows, int frame, bool regionEnabled, Rect2 regionRect)
	{
		_texture = texture;
		_columns = Math.Clamp(columns, 1, 256);
		_rows = Math.Clamp(rows, 1, 256);
		_frame = Math.Clamp(frame, 0, Math.Max(0, _columns * _rows - 1));
		_regionEnabled = regionEnabled;
		_regionRect = regionRect;
		QueueRedraw();
	}

	public override void _Draw()
	{
		DrawRect(new Rect2(Vector2.Zero, Size), new Color("#0b1119"));
		DrawCheckerboard();
		if (!GodotObject.IsInstanceValid(_texture))
		{
			DrawString(ThemeDB.FallbackFont, Size * 0.5f - new Vector2(78f, 0f), "先给 Sprite2D 选择贴图", HorizontalAlignment.Left, -1f, 16, new Color("#8fa5b8"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			_drawRect = default;
			return;
		}
		Vector2 size = _texture.GetSize();
		if (!(size.X <= 0f) && !(size.Y <= 0f))
		{
			_drawRect = CalculateTextureDrawRect(size);
			DrawTextureRect(_texture, _drawRect, tile: false);
			DrawRect(_drawRect, new Color("#6e8ca5"), filled: false, 1f);
			Rect2 pixels = ResolveSourcePixels(size);
			Rect2 rect = PixelRectToDraw(pixels, size);
			if (_regionEnabled)
			{
				DrawRegionMask(rect);
				DrawRect(rect, new Color("#5ae6a8"), filled: false, 2f);
			}
			DrawGrid(rect);
			DrawSelection(rect);
		}
	}

	private void DrawCheckerboard()
	{
		Color color = new Color("#111c28");
		Color color2 = new Color("#172634");
		int num = Mathf.CeilToInt(Size.X / 20f);
		int num2 = Mathf.CeilToInt(Size.Y / 20f);
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				DrawRect(new Rect2((float)j * 20f, (float)i * 20f, 20f, 20f), (((j + i) & 1) == 0) ? color : color2);
			}
		}
	}

	private Rect2 ResolveSourcePixels(Vector2 textureSize)
	{
		if (!_regionEnabled || _regionRect.Size.X <= 0f || _regionRect.Size.Y <= 0f)
		{
			return new Rect2(Vector2.Zero, textureSize);
		}
		Rect2 b = new Rect2(Vector2.Zero, textureSize);
		return _regionRect.Intersection(b);
	}

	private Rect2 PixelRectToDraw(Rect2 pixels, Vector2 textureSize)
	{
		Vector2 vector = _drawRect.Size / textureSize;
		return new Rect2(_drawRect.Position + pixels.Position * vector, pixels.Size * vector);
	}

	private Rect2 CalculateTextureDrawRect(Vector2 textureSize)
	{
		if (textureSize.X <= 0f || textureSize.Y <= 0f)
		{
			return default;
		}
		Rect2 rect = new Rect2(new Vector2(18f, 18f), new Vector2(Mathf.Max(1f, Size.X - 36f), Mathf.Max(1f, Size.Y - 36f)));
		float num = Mathf.Min(rect.Size.X / textureSize.X, rect.Size.Y / textureSize.Y);
		Vector2 vector = textureSize * num;
		return new Rect2(rect.Position + (rect.Size - vector) * 0.5f, vector);
	}

	private void DrawRegionMask(Rect2 active)
	{
		Color color = new Color(0.02f, 0.04f, 0.07f, 0.7f);
		if (active.Position.Y > _drawRect.Position.Y)
		{
			DrawRect(new Rect2(_drawRect.Position, new Vector2(_drawRect.Size.X, active.Position.Y - _drawRect.Position.Y)), color);
		}
		if (active.End.Y < _drawRect.End.Y)
		{
			DrawRect(new Rect2(new Vector2(_drawRect.Position.X, active.End.Y), new Vector2(_drawRect.Size.X, _drawRect.End.Y - active.End.Y)), color);
		}
		if (active.Position.X > _drawRect.Position.X)
		{
			DrawRect(new Rect2(new Vector2(_drawRect.Position.X, active.Position.Y), new Vector2(active.Position.X - _drawRect.Position.X, active.Size.Y)), color);
		}
		if (active.End.X < _drawRect.End.X)
		{
			DrawRect(new Rect2(new Vector2(active.End.X, active.Position.Y), new Vector2(_drawRect.End.X - active.End.X, active.Size.Y)), color);
		}
	}

	private void DrawGrid(Rect2 sourceDraw)
	{
		Color color = new Color(0.64f, 0.83f, 0.94f, 0.6f);
		int num = Math.Max(1, Mathf.CeilToInt((float)_columns / 128f));
		int num2 = Math.Max(1, Mathf.CeilToInt((float)_rows / 128f));
		for (int i = 0; i <= _columns; i += num)
		{
			float x = sourceDraw.Position.X + sourceDraw.Size.X * (float)i / (float)_columns;
			DrawLine(new Vector2(x, sourceDraw.Position.Y), new Vector2(x, sourceDraw.End.Y), color, 1f);
		}
		if (_columns % num != 0)
		{
			DrawLine(new Vector2(sourceDraw.End.X, sourceDraw.Position.Y), sourceDraw.End, color, 1f);
		}
		for (int j = 0; j <= _rows; j += num2)
		{
			float y = sourceDraw.Position.Y + sourceDraw.Size.Y * (float)j / (float)_rows;
			DrawLine(new Vector2(sourceDraw.Position.X, y), new Vector2(sourceDraw.End.X, y), color, 1f);
		}
		if (_rows % num2 != 0)
		{
			DrawLine(new Vector2(sourceDraw.Position.X, sourceDraw.End.Y), sourceDraw.End, color, 1f);
		}
	}

	private void DrawSelection(Rect2 sourceDraw)
	{
		int num = _frame % _columns;
		int num2 = _frame / _columns;
		Vector2 vector = sourceDraw.Size / new Vector2(_columns, _rows);
		Rect2 rect = new Rect2(sourceDraw.Position + new Vector2(num, num2) * vector, vector);
		DrawRect(rect, new Color(0.98f, 0.72f, 0.23f, 0.22f));
		DrawRect(rect.Grow(-1f), new Color("#ffd166"), filled: false, 3f);
		string text = $"{_frame}";
		DrawString(ThemeDB.FallbackFont, rect.Position + new Vector2(5f, 17f), text, HorizontalAlignment.Left, -1f, 14, new Color("#fff2be"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
	}

	private void OnGuiInput(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left && inputEventMouseButton.Pressed && GodotObject.IsInstanceValid(_texture))
		{
			Vector2 size = _texture.GetSize();
			_drawRect = CalculateTextureDrawRect(size);
			Rect2 rect = PixelRectToDraw(ResolveSourcePixels(size), size);
			if (rect.HasPoint(inputEventMouseButton.Position) && !(rect.Size.X <= 0f) && !(rect.Size.Y <= 0f))
			{
				Vector2 vector = (inputEventMouseButton.Position - rect.Position) / rect.Size;
				int num = Math.Clamp(Mathf.FloorToInt(vector.X * (float)_columns), 0, _columns - 1);
				int num2 = Math.Clamp(Mathf.FloorToInt(vector.Y * (float)_rows), 0, _rows - 1);
				EmitSignal(SignalName.FramePicked, num2 * _columns + num);
				AcceptEvent();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Bind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "columns", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rows", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "regionEnabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "regionRect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawCheckerboard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveSourcePixels, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "textureSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PixelRectToDraw, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "pixels", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "textureSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CalculateTextureDrawRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "textureSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawRegionMask, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawGrid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "sourceDraw", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "sourceDraw", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.Bind && args.Count == 6)
		{
			Bind(VariantUtils.ConvertTo<Texture2D>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<Rect2>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawCheckerboard && args.Count == 0)
		{
			DrawCheckerboard();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveSourcePixels && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(ResolveSourcePixels(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.PixelRectToDraw && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2>(PixelRectToDraw(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.CalculateTextureDrawRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(CalculateTextureDrawRect(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.DrawRegionMask && args.Count == 1)
		{
			DrawRegionMask(VariantUtils.ConvertTo<Rect2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawGrid && args.Count == 1)
		{
			DrawGrid(VariantUtils.ConvertTo<Rect2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawSelection && args.Count == 1)
		{
			DrawSelection(VariantUtils.ConvertTo<Rect2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnGuiInput && args.Count == 1)
		{
			OnGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.Bind)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.DrawCheckerboard)
		{
			return true;
		}
		if (method == MethodName.ResolveSourcePixels)
		{
			return true;
		}
		if (method == MethodName.PixelRectToDraw)
		{
			return true;
		}
		if (method == MethodName.CalculateTextureDrawRect)
		{
			return true;
		}
		if (method == MethodName.DrawRegionMask)
		{
			return true;
		}
		if (method == MethodName.DrawGrid)
		{
			return true;
		}
		if (method == MethodName.DrawSelection)
		{
			return true;
		}
		if (method == MethodName.OnGuiInput)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._texture)
		{
			_texture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._columns)
		{
			_columns = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._rows)
		{
			_rows = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._frame)
		{
			_frame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._regionEnabled)
		{
			_regionEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._regionRect)
		{
			_regionRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName._drawRect)
		{
			_drawRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.Columns)
		{
			from = Columns;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Rows)
		{
			from = Rows;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SelectedFrame)
		{
			from = SelectedFrame;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DrawnAtlasRect)
		{
			value = VariantUtils.CreateFrom<Rect2>(DrawnAtlasRect);
			return true;
		}
		if (name == PropertyName.IsGuideDensityLimited)
		{
			value = VariantUtils.CreateFrom<bool>(IsGuideDensityLimited);
			return true;
		}
		if (name == PropertyName._texture)
		{
			value = VariantUtils.CreateFrom(in _texture);
			return true;
		}
		if (name == PropertyName._columns)
		{
			value = VariantUtils.CreateFrom(in _columns);
			return true;
		}
		if (name == PropertyName._rows)
		{
			value = VariantUtils.CreateFrom(in _rows);
			return true;
		}
		if (name == PropertyName._frame)
		{
			value = VariantUtils.CreateFrom(in _frame);
			return true;
		}
		if (name == PropertyName._regionEnabled)
		{
			value = VariantUtils.CreateFrom(in _regionEnabled);
			return true;
		}
		if (name == PropertyName._regionRect)
		{
			value = VariantUtils.CreateFrom(in _regionRect);
			return true;
		}
		if (name == PropertyName._drawRect)
		{
			value = VariantUtils.CreateFrom(in _drawRect);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._texture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._columns, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._rows, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._regionEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._regionRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._drawRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.Columns, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.Rows, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SelectedFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.DrawnAtlasRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsGuideDensityLimited, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._texture, Variant.From(in _texture));
		info.AddProperty(PropertyName._columns, Variant.From(in _columns));
		info.AddProperty(PropertyName._rows, Variant.From(in _rows));
		info.AddProperty(PropertyName._frame, Variant.From(in _frame));
		info.AddProperty(PropertyName._regionEnabled, Variant.From(in _regionEnabled));
		info.AddProperty(PropertyName._regionRect, Variant.From(in _regionRect));
		info.AddProperty(PropertyName._drawRect, Variant.From(in _drawRect));
		info.AddSignalEventDelegate(SignalName.FramePicked, backing_FramePicked);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._texture, out var value))
		{
			_texture = value.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._columns, out var value2))
		{
			_columns = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._rows, out var value3))
		{
			_rows = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._frame, out var value4))
		{
			_frame = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._regionEnabled, out var value5))
		{
			_regionEnabled = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._regionRect, out var value6))
		{
			_regionRect = value6.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName._drawRect, out var value7))
		{
			_drawRect = value7.As<Rect2>();
		}
		if (info.TryGetSignalEventDelegate<FramePickedEventHandler>(SignalName.FramePicked, out var value8))
		{
			backing_FramePicked = value8;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.FramePicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalFramePicked(int frame)
	{
		EmitSignal(SignalName.FramePicked, new ReadOnlySpan<Variant>((Variant)frame));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.FramePicked && args.Count == 1)
		{
			backing_FramePicked?.Invoke(VariantUtils.ConvertTo<int>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.FramePicked)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
