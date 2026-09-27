using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

public sealed class XWBattleProcessPresenter : IXWGameplayLogicPresenter
{
	private enum ProcessVisualKind
	{
		Wave,
		Vase,
		VaseItem,
		VaseFill,
		IZM,
		IZM2,
		Quiz,
		Empty
	}

	private sealed class ProcessTimelineCanvas : Control
	{
		private sealed class Segment
		{
			public Resource Owner;

			public StringName Property;

			public string Label;

			public Color Color;

			public double Minimum;

			public double Maximum;
		}

		public new class MethodName : Control.MethodName
		{
			public static readonly StringName AddSegment = "AddSegment";

			public static readonly StringName SetPreview = "SetPreview";

			public new static readonly StringName _GuiInput = "_GuiInput";

			public new static readonly StringName _Draw = "_Draw";

			public static readonly StringName FindBoundary = "FindBoundary";

			public static readonly StringName TimeAtX = "TimeAtX";
		}

		public new class PropertyName : Control.PropertyName
		{
			public static readonly StringName Editable = "Editable";

			public static readonly StringName SegmentCount = "SegmentCount";

			public static readonly StringName TotalDuration = "TotalDuration";

			public static readonly StringName _dragSegment = "_dragSegment";

			public static readonly StringName _dragStartX = "_dragStartX";

			public static readonly StringName _dragStartValue = "_dragStartValue";

			public static readonly StringName _previewTime = "_previewTime";
		}

		public new class SignalName : Control.SignalName
		{
		}

		private readonly List<Segment> _segments = new List<Segment>();

		private int _dragSegment = -1;

		private float _dragStartX;

		private double _dragStartValue;

		private double _previewTime;

		public XWVisualPropertyBinding Binding { get; set; }

		public bool Editable { get; set; }

		public Action<double> PreviewChanged { get; set; }

		public int SegmentCount => _segments.Count;

		public double TotalDuration
		{
			get
			{
				double num = 0.0;
				foreach (Segment segment in _segments)
				{
					num += Math.Max(0.01, segment.Owner.Get(segment.Property).AsDouble());
				}
				return num;
			}
		}

		public void AddSegment(Resource owner, StringName property, string label, Color color, double minimum, double maximum)
		{
			_segments.Add(new Segment
			{
				Owner = owner,
				Property = property,
				Label = label,
				Color = color,
				Minimum = minimum,
				Maximum = maximum
			});
			QueueRedraw();
		}

		public void SetPreview(double value)
		{
			_previewTime = value;
			QueueRedraw();
		}

		public override void _GuiInput(InputEvent @event)
		{
			if (@event is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
			{
				if (inputEventMouseButton.Pressed)
				{
					_dragSegment = (Editable ? FindBoundary(inputEventMouseButton.Position.X) : (-1));
					if (_dragSegment >= 0)
					{
						Segment segment = _segments[_dragSegment];
						_dragStartX = inputEventMouseButton.Position.X;
						_dragStartValue = segment.Owner.Get(segment.Property).AsDouble();
						Binding?.BeginEdit(segment.Owner, segment.Property);
					}
					else
					{
						PreviewChanged?.Invoke(TimeAtX(inputEventMouseButton.Position.X));
					}
				}
				else if (_dragSegment >= 0)
				{
					Segment segment2 = _segments[_dragSegment];
					Binding?.CommitEdit(segment2.Owner, segment2.Property, segment2.Owner.Get(segment2.Property), "调整 " + segment2.Label + " 时长");
					_dragSegment = -1;
				}
				AcceptEvent();
			}
			else if (@event is InputEventMouseMotion inputEventMouseMotion && _dragSegment >= 0 && Editable)
			{
				Segment segment3 = _segments[_dragSegment];
				double num = TotalDuration / (double)Math.Max(1f, Size.X);
				double num2 = Math.Clamp(_dragStartValue + (double)(inputEventMouseMotion.Position.X - _dragStartX) * num, segment3.Minimum, segment3.Maximum);
				Binding?.PreviewValue(segment3.Owner, segment3.Property, num2);
				QueueRedraw();
				PreviewChanged?.Invoke(Math.Min(_previewTime, TotalDuration));
				AcceptEvent();
			}
		}

		public override void _Draw()
		{
			double totalDuration = TotalDuration;
			if (_segments.Count == 0 || totalDuration <= 0.0)
			{
				DrawString(ThemeDB.FallbackFont, new Vector2(8f, 30f), "该流程没有自动阶段", HorizontalAlignment.Left, -1f, 14, new Color("b6c0b0"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
				return;
			}
			float num = 0f;
			for (int i = 0; i < _segments.Count; i++)
			{
				Segment segment = _segments[i];
				double num2 = Math.Max(0.01, segment.Owner.Get(segment.Property).AsDouble());
				float num3 = ((i == _segments.Count - 1) ? (Size.X - num) : ((float)((double)Size.X * num2 / totalDuration)));
				Rect2 rect = new Rect2(new Vector2(num, 4f), new Vector2(Math.Max(2f, num3 - 2f), Math.Max(24f, Size.Y - 8f)));
				DrawRect(rect, segment.Color);
				DrawString(ThemeDB.FallbackFont, rect.Position + new Vector2(6f, 19f), segment.Label, HorizontalAlignment.Left, Math.Max(4f, rect.Size.X - 12f), 12, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
				if (i < _segments.Count - 1)
				{
					DrawLine(new Vector2(num + num3, 1f), new Vector2(num + num3, Size.Y - 1f), Editable ? Colors.White : new Color("909890"), 3f);
				}
				num += num3;
			}
			float x = (float)((double)Size.X * Math.Clamp(_previewTime / totalDuration, 0.0, 1.0));
			DrawLine(new Vector2(x, 0f), new Vector2(x, Size.Y), new Color("fff2a8"), 3f);
		}

		private int FindBoundary(float mouseX)
		{
			double totalDuration = TotalDuration;
			float num = 0f;
			for (int i = 0; i < _segments.Count - 1; i++)
			{
				num += (float)((double)Size.X * Math.Max(0.01, _segments[i].Owner.Get(_segments[i].Property).AsDouble()) / totalDuration);
				if (Math.Abs(mouseX - num) <= 8f)
				{
					return i;
				}
			}
			return -1;
		}

		private double TimeAtX(float x)
		{
			return TotalDuration * (double)Math.Clamp(x / Math.Max(1f, Size.X), 0f, 1f);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(6)
			{
				new MethodInfo(MethodName.AddSegment, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
					new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
					new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
					new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
					new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
					new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
				}, null),
				new MethodInfo(MethodName.SetPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
				}, null),
				new MethodInfo(MethodName._GuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
				}, null),
				new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
				new MethodInfo(MethodName.FindBoundary, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Float, "mouseX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
				}, null),
				new MethodInfo(MethodName.TimeAtX, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Float, "x", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
				}, null)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if (method == MethodName.AddSegment && args.Count == 6)
			{
				AddSegment(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]));
				ret = default;
				return true;
			}
			if (method == MethodName.SetPreview && args.Count == 1)
			{
				SetPreview(VariantUtils.ConvertTo<double>(in args[0]));
				ret = default;
				return true;
			}
			if (method == MethodName._GuiInput && args.Count == 1)
			{
				_GuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
				ret = default;
				return true;
			}
			if (method == MethodName._Draw && args.Count == 0)
			{
				_Draw();
				ret = default;
				return true;
			}
			if (method == MethodName.FindBoundary && args.Count == 1)
			{
				ret = VariantUtils.CreateFrom<int>(FindBoundary(VariantUtils.ConvertTo<float>(in args[0])));
				return true;
			}
			if (method == MethodName.TimeAtX && args.Count == 1)
			{
				ret = VariantUtils.CreateFrom<double>(TimeAtX(VariantUtils.ConvertTo<float>(in args[0])));
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			if (method == MethodName.AddSegment)
			{
				return true;
			}
			if (method == MethodName.SetPreview)
			{
				return true;
			}
			if (method == MethodName._GuiInput)
			{
				return true;
			}
			if (method == MethodName._Draw)
			{
				return true;
			}
			if (method == MethodName.FindBoundary)
			{
				return true;
			}
			if (method == MethodName.TimeAtX)
			{
				return true;
			}
			return base.HasGodotClassMethod(in method);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if (name == PropertyName.Editable)
			{
				Editable = VariantUtils.ConvertTo<bool>(in value);
				return true;
			}
			if (name == PropertyName._dragSegment)
			{
				_dragSegment = VariantUtils.ConvertTo<int>(in value);
				return true;
			}
			if (name == PropertyName._dragStartX)
			{
				_dragStartX = VariantUtils.ConvertTo<float>(in value);
				return true;
			}
			if (name == PropertyName._dragStartValue)
			{
				_dragStartValue = VariantUtils.ConvertTo<double>(in value);
				return true;
			}
			if (name == PropertyName._previewTime)
			{
				_previewTime = VariantUtils.ConvertTo<double>(in value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if (name == PropertyName.Editable)
			{
				value = VariantUtils.CreateFrom<bool>(Editable);
				return true;
			}
			if (name == PropertyName.SegmentCount)
			{
				value = VariantUtils.CreateFrom<int>(SegmentCount);
				return true;
			}
			if (name == PropertyName.TotalDuration)
			{
				value = VariantUtils.CreateFrom<double>(TotalDuration);
				return true;
			}
			if (name == PropertyName._dragSegment)
			{
				value = VariantUtils.CreateFrom(in _dragSegment);
				return true;
			}
			if (name == PropertyName._dragStartX)
			{
				value = VariantUtils.CreateFrom(in _dragStartX);
				return true;
			}
			if (name == PropertyName._dragStartValue)
			{
				value = VariantUtils.CreateFrom(in _dragStartValue);
				return true;
			}
			if (name == PropertyName._previewTime)
			{
				value = VariantUtils.CreateFrom(in _previewTime);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, PropertyName._dragSegment, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Float, PropertyName._dragStartX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Float, PropertyName._dragStartValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Float, PropertyName._previewTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Bool, PropertyName.Editable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Int, PropertyName.SegmentCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Float, PropertyName.TotalDuration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(PropertyName.Editable, Variant.From<bool>(Editable));
			info.AddProperty(PropertyName._dragSegment, Variant.From(in _dragSegment));
			info.AddProperty(PropertyName._dragStartX, Variant.From(in _dragStartX));
			info.AddProperty(PropertyName._dragStartValue, Variant.From(in _dragStartValue));
			info.AddProperty(PropertyName._previewTime, Variant.From(in _previewTime));
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			if (info.TryGetProperty(PropertyName.Editable, out var value))
			{
				Editable = value.As<bool>();
			}
			if (info.TryGetProperty(PropertyName._dragSegment, out var value2))
			{
				_dragSegment = value2.As<int>();
			}
			if (info.TryGetProperty(PropertyName._dragStartX, out var value3))
			{
				_dragStartX = value3.As<float>();
			}
			if (info.TryGetProperty(PropertyName._dragStartValue, out var value4))
			{
				_dragStartValue = value4.As<double>();
			}
			if (info.TryGetProperty(PropertyName._previewTime, out var value5))
			{
				_previewTime = value5.As<double>();
			}
		}
	}

	private sealed class ProcessBoardCanvas : Control
	{
		public new class MethodName : Control.MethodName
		{
			public new static readonly StringName _GuiInput = "_GuiInput";

			public new static readonly StringName _Draw = "_Draw";

			public static readonly StringName DrawWaveFlow = "DrawWaveFlow";

			public static readonly StringName DrawVases = "DrawVases";

			public static readonly StringName DrawVase = "DrawVase";

			public static readonly StringName DrawFillPool = "DrawFillPool";

			public static readonly StringName DrawIZM = "DrawIZM";

			public static readonly StringName DrawQuiz = "DrawQuiz";

			public static readonly StringName GridRect = "GridRect";

			public static readonly StringName GridToPosition = "GridToPosition";

			public static readonly StringName PositionToGrid = "PositionToGrid";
		}

		public new class PropertyName : Control.PropertyName
		{
			public static readonly StringName Kind = "Kind";

			public static readonly StringName Config = "Config";

			public static readonly StringName Editable = "Editable";

			public static readonly StringName PreviewRatio = "PreviewRatio";

			public static readonly StringName _dragVaseIndex = "_dragVaseIndex";

			public static readonly StringName _dragVase = "_dragVase";
		}

		public new class SignalName : Control.SignalName
		{
		}

		private int _dragVaseIndex = -1;

		private TowerDefenseLevelVaseConfig _dragVase;

		public ProcessVisualKind Kind { get; set; }

		public Resource Config { get; set; }

		public XWVisualPropertyBinding Binding { get; set; }

		public bool Editable { get; set; }

		public float PreviewRatio { get; set; }

		public override void _GuiInput(InputEvent @event)
		{
			bool flag = !Editable;
			if (!flag)
			{
				ProcessVisualKind kind = Kind;
				bool flag2 = (uint)(kind - 1) <= 1u;
				flag = !flag2;
			}
			if (flag)
			{
				return;
			}
			if (@event is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
			{
				if (inputEventMouseButton.Pressed)
				{
					_dragVaseIndex = FindVase(inputEventMouseButton.Position, out _dragVase);
					if (GodotObject.IsInstanceValid(_dragVase))
					{
						Binding?.BeginEdit(_dragVase, "gridPos");
					}
				}
				else if (GodotObject.IsInstanceValid(_dragVase))
				{
					Binding?.CommitEdit(_dragVase, "gridPos", _dragVase.gridPos, "拖动花瓶格子");
					_dragVaseIndex = -1;
					_dragVase = null;
				}
				AcceptEvent();
			}
			else if (@event is InputEventMouseMotion inputEventMouseMotion && GodotObject.IsInstanceValid(_dragVase))
			{
				Vector2I vector2I = PositionToGrid(inputEventMouseMotion.Position);
				Binding?.PreviewValue(_dragVase, "gridPos", vector2I);
				QueueRedraw();
				AcceptEvent();
			}
		}

		public override void _Draw()
		{
			switch (Kind)
			{
			case ProcessVisualKind.Wave:
			case ProcessVisualKind.IZM2:
				DrawWaveFlow(Kind == ProcessVisualKind.IZM2);
				break;
			case ProcessVisualKind.Vase:
			case ProcessVisualKind.VaseItem:
				DrawVases();
				break;
			case ProcessVisualKind.VaseFill:
				DrawFillPool();
				break;
			case ProcessVisualKind.IZM:
				DrawIZM();
				break;
			case ProcessVisualKind.Quiz:
				DrawQuiz();
				break;
			case ProcessVisualKind.Empty:
				DrawString(ThemeDB.FallbackFont, new Vector2(Size.X * 0.34f, Size.Y * 0.5f), "空流程 · 自由布置", HorizontalAlignment.Center, Size.X * 0.32f, 24, new Color("f4f0c5"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
				break;
			}
		}

		private void DrawWaveFlow(bool izm)
		{
			float num = Mathf.Lerp(Size.X * 0.08f, Size.X * 0.68f, Mathf.Clamp(PreviewRatio * 2.2f, 0f, 1f));
			DrawRect(new Rect2(num, Size.Y * 0.12f, Size.X * 0.26f, Size.Y * 0.72f), new Color("fff6bd22"));
			DrawRect(new Rect2(num, Size.Y * 0.12f, Size.X * 0.26f, Size.Y * 0.72f), new Color("fff6bd"), filled: false, 3f);
			for (int i = 0; i < 5; i++)
			{
				float y = Size.Y * (0.23f + (float)i * 0.13f);
				float num2 = Size.X * (0.82f - (float)((i * 17 + 8) % 29) / 100f);
				DrawCircle(new Vector2(num2, y), 13f, izm ? new Color("a56cc1") : new Color("6d8b55"));
				DrawLine(new Vector2(num2 - 22f, y), new Vector2(num2 - 70f, y), new Color("d9e7c4"), 2f);
			}
			DrawString(ThemeDB.FallbackFont, new Vector2(num + 8f, Size.Y * 0.18f), "编辑器镜头", HorizontalAlignment.Left, -1f, 14, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}

		private void DrawVases()
		{
			int num = 0;
			if (Config is TowerDefenseLevelVaseManagerConfig { vaseList: var vaseList } towerDefenseLevelVaseManagerConfig)
			{
				num = Math.Min(vaseList?.Count ?? 0, 96);
				for (int i = 0; i < num; i++)
				{
					DrawVase(towerDefenseLevelVaseManagerConfig.vaseList[i], i == _dragVaseIndex);
				}
			}
			else if (Config is TowerDefenseLevelVaseConfig vase)
			{
				num = 1;
				DrawVase(vase, _dragVaseIndex == 0);
			}
			DrawString(ThemeDB.FallbackFont, new Vector2(14f, Size.Y - 18f), $"拖动花瓶到草坪格子 · {num} 个", HorizontalAlignment.Left, -1f, 14, new Color("f4f0c5"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}

		private void DrawVase(TowerDefenseLevelVaseConfig vase, bool selected)
		{
			if (GodotObject.IsInstanceValid(vase))
			{
				Vector2 vector = GridToPosition(vase.gridPos);
				string type = vase.type;
				Color color;
				if (type == "Plant")
				{
					color = new Color("6fbb62");
				}
				else
				{
					color = ((!(type == "Zombie")) ? new Color("c7a16a") : new Color("8d6aac"));
				}
				Color color2 = color;
				DrawRect(new Rect2(vector - new Vector2(13f, 18f), new Vector2(26f, 34f)), color2);
				DrawCircle(vector + new Vector2(0f, 13f), 14f, color2);
				DrawRect(new Rect2(vector - new Vector2(16f, 22f), new Vector2(32f, 7f)), selected ? Colors.White : color2.Lightened(0.18f));
			}
		}

		private void DrawFillPool()
		{
			for (int i = 0; i < 6; i++)
			{
				Rect2 rect = new Rect2(new Vector2(Size.X * (0.23f + (float)i * 0.1f), Size.Y * 0.34f), new Vector2(Size.X * 0.08f, Size.Y * 0.28f));
				DrawRect(rect, new Color("70593fcc"));
				DrawRect(rect, new Color("e9d09c"), filled: false, 2f);
			}
			DrawString(ThemeDB.FallbackFont, new Vector2(Size.X * 0.32f, Size.Y * 0.7f), "花瓶随机填充池", HorizontalAlignment.Center, Size.X * 0.36f, 20, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}

		private void DrawIZM()
		{
			DrawCircle(new Vector2(Size.X * 0.1f, Size.Y * 0.5f), 28f, new Color("f095b5"));
			DrawString(ThemeDB.FallbackFont, new Vector2(Size.X * 0.04f, Size.Y * 0.62f), "脑子", HorizontalAlignment.Center, Size.X * 0.12f, 14, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			for (int i = 0; i < 6; i++)
			{
				float num = Size.X * (0.3f + (float)i * 0.09f);
				DrawRect(new Rect2(num, Size.Y * 0.72f, Size.X * 0.075f, Size.Y * 0.2f), new Color("5f4a6e"));
				DrawCircle(new Vector2(num + Size.X * 0.038f, Size.Y * 0.38f), 14f, new Color("7c9662"));
				DrawLine(new Vector2(num, Size.Y * 0.38f), new Vector2(Size.X * 0.14f, Size.Y * 0.5f), new Color("d7b5e8aa"), 2f);
			}
			DrawString(ThemeDB.FallbackFont, new Vector2(Size.X * 0.3f, Size.Y * 0.68f), "僵尸选卡栏", HorizontalAlignment.Center, Size.X * 0.5f, 16, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}

		private void DrawQuiz()
		{
			int num = ((Config is TowerDefenseBattleProcessQuizConfig towerDefenseBattleProcessQuizConfig) ? Math.Clamp(towerDefenseBattleProcessQuizConfig.stripColumn, 1, 9) : 5);
			Rect2 rect = GridRect();
			float num2 = rect.Size.X / 9f;
			float num3 = rect.Size.Y / 5f;
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 9 && i * 9 + j < 96; j++)
				{
					Vector2 vector = rect.Position + new Vector2(((float)j + 0.5f) * num2, ((float)i + 0.5f) * num3);
					if (j < num)
					{
						DrawRect(new Rect2(vector - new Vector2(14f, 14f), new Vector2(28f, 28f)), new Color("e7b94f"));
						DrawLine(vector + new Vector2(-14f, -2f), vector + new Vector2(14f, -2f), new Color("fff0a8"), 2f);
					}
					else
					{
						DrawRect(new Rect2(vector - new Vector2(11f, 16f), new Vector2(22f, 29f)), new Color("8d6aac"));
						DrawCircle(vector + new Vector2(0f, 11f), 12f, new Color("8d6aac"));
					}
				}
			}
			float num4 = rect.Position.X + (float)num * num2;
			DrawLine(new Vector2(num4, rect.Position.Y), new Vector2(num4, rect.End.Y), new Color("ffdf64"), 4f);
			DrawString(ThemeDB.FallbackFont, new Vector2(rect.Position.X, rect.Position.Y - 10f), "礼盒区", HorizontalAlignment.Left, (float)num * num2, 16, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			DrawString(ThemeDB.FallbackFont, new Vector2(num4, rect.Position.Y - 10f), "僵尸罐区", HorizontalAlignment.Right, rect.End.X - num4, 16, Colors.White, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}

		private int FindVase(Vector2 position, out TowerDefenseLevelVaseConfig vase)
		{
			vase = null;
			if (Config is TowerDefenseLevelVaseConfig towerDefenseLevelVaseConfig && position.DistanceTo(GridToPosition(towerDefenseLevelVaseConfig.gridPos)) <= 28f)
			{
				vase = towerDefenseLevelVaseConfig;
				return 0;
			}
			if (!(Config is TowerDefenseLevelVaseManagerConfig { vaseList: var vaseList } towerDefenseLevelVaseManagerConfig))
			{
				return -1;
			}
			int num = Math.Min(vaseList?.Count ?? 0, 96);
			for (int i = 0; i < num; i++)
			{
				TowerDefenseLevelVaseConfig towerDefenseLevelVaseConfig2 = towerDefenseLevelVaseManagerConfig.vaseList[i];
				if (GodotObject.IsInstanceValid(towerDefenseLevelVaseConfig2) && position.DistanceTo(GridToPosition(towerDefenseLevelVaseConfig2.gridPos)) <= 28f)
				{
					vase = towerDefenseLevelVaseConfig2;
					return i;
				}
			}
			return -1;
		}

		private Rect2 GridRect()
		{
			return new Rect2(new Vector2(Size.X * 0.16f, Size.Y * 0.19f), new Vector2(Size.X * 0.72f, Size.Y * 0.65f));
		}

		private Vector2 GridToPosition(Vector2I grid)
		{
			Rect2 rect = GridRect();
			return rect.Position + new Vector2(((float)Math.Clamp(grid.X, 1, 9) - 0.5f) * rect.Size.X / 9f, ((float)Math.Clamp(grid.Y, 1, 5) - 0.5f) * rect.Size.Y / 5f);
		}

		private Vector2I PositionToGrid(Vector2 position)
		{
			Rect2 rect = GridRect();
			int x = Math.Clamp(Mathf.FloorToInt((position.X - rect.Position.X) / Math.Max(1f, rect.Size.X) * 9f) + 1, 1, 9);
			int y = Math.Clamp(Mathf.FloorToInt((position.Y - rect.Position.Y) / Math.Max(1f, rect.Size.Y) * 5f) + 1, 1, 5);
			return new Vector2I(x, y);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(11)
			{
				new MethodInfo(MethodName._GuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
				}, null),
				new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
				new MethodInfo(MethodName.DrawWaveFlow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Bool, "izm", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
				}, null),
				new MethodInfo(MethodName.DrawVases, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
				new MethodInfo(MethodName.DrawVase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Object, "vase", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
					new PropertyInfo(Variant.Type.Bool, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
				}, null),
				new MethodInfo(MethodName.DrawFillPool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
				new MethodInfo(MethodName.DrawIZM, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
				new MethodInfo(MethodName.DrawQuiz, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
				new MethodInfo(MethodName.GridRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
				new MethodInfo(MethodName.GridToPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Vector2I, "grid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
				}, null),
				new MethodInfo(MethodName.PositionToGrid, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
				}, null)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if (method == MethodName._GuiInput && args.Count == 1)
			{
				_GuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
				ret = default;
				return true;
			}
			if (method == MethodName._Draw && args.Count == 0)
			{
				_Draw();
				ret = default;
				return true;
			}
			if (method == MethodName.DrawWaveFlow && args.Count == 1)
			{
				DrawWaveFlow(VariantUtils.ConvertTo<bool>(in args[0]));
				ret = default;
				return true;
			}
			if (method == MethodName.DrawVases && args.Count == 0)
			{
				DrawVases();
				ret = default;
				return true;
			}
			if (method == MethodName.DrawVase && args.Count == 2)
			{
				DrawVase(VariantUtils.ConvertTo<TowerDefenseLevelVaseConfig>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
				ret = default;
				return true;
			}
			if (method == MethodName.DrawFillPool && args.Count == 0)
			{
				DrawFillPool();
				ret = default;
				return true;
			}
			if (method == MethodName.DrawIZM && args.Count == 0)
			{
				DrawIZM();
				ret = default;
				return true;
			}
			if (method == MethodName.DrawQuiz && args.Count == 0)
			{
				DrawQuiz();
				ret = default;
				return true;
			}
			if (method == MethodName.GridRect && args.Count == 0)
			{
				ret = VariantUtils.CreateFrom<Rect2>(GridRect());
				return true;
			}
			if (method == MethodName.GridToPosition && args.Count == 1)
			{
				ret = VariantUtils.CreateFrom<Vector2>(GridToPosition(VariantUtils.ConvertTo<Vector2I>(in args[0])));
				return true;
			}
			if (method == MethodName.PositionToGrid && args.Count == 1)
			{
				ret = VariantUtils.CreateFrom<Vector2I>(PositionToGrid(VariantUtils.ConvertTo<Vector2>(in args[0])));
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			if (method == MethodName._GuiInput)
			{
				return true;
			}
			if (method == MethodName._Draw)
			{
				return true;
			}
			if (method == MethodName.DrawWaveFlow)
			{
				return true;
			}
			if (method == MethodName.DrawVases)
			{
				return true;
			}
			if (method == MethodName.DrawVase)
			{
				return true;
			}
			if (method == MethodName.DrawFillPool)
			{
				return true;
			}
			if (method == MethodName.DrawIZM)
			{
				return true;
			}
			if (method == MethodName.DrawQuiz)
			{
				return true;
			}
			if (method == MethodName.GridRect)
			{
				return true;
			}
			if (method == MethodName.GridToPosition)
			{
				return true;
			}
			if (method == MethodName.PositionToGrid)
			{
				return true;
			}
			return base.HasGodotClassMethod(in method);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if (name == PropertyName.Kind)
			{
				Kind = VariantUtils.ConvertTo<ProcessVisualKind>(in value);
				return true;
			}
			if (name == PropertyName.Config)
			{
				Config = VariantUtils.ConvertTo<Resource>(in value);
				return true;
			}
			if (name == PropertyName.Editable)
			{
				Editable = VariantUtils.ConvertTo<bool>(in value);
				return true;
			}
			if (name == PropertyName.PreviewRatio)
			{
				PreviewRatio = VariantUtils.ConvertTo<float>(in value);
				return true;
			}
			if (name == PropertyName._dragVaseIndex)
			{
				_dragVaseIndex = VariantUtils.ConvertTo<int>(in value);
				return true;
			}
			if (name == PropertyName._dragVase)
			{
				_dragVase = VariantUtils.ConvertTo<TowerDefenseLevelVaseConfig>(in value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if (name == PropertyName.Kind)
			{
				value = VariantUtils.CreateFrom<ProcessVisualKind>(Kind);
				return true;
			}
			if (name == PropertyName.Config)
			{
				value = VariantUtils.CreateFrom<Resource>(Config);
				return true;
			}
			if (name == PropertyName.Editable)
			{
				value = VariantUtils.CreateFrom<bool>(Editable);
				return true;
			}
			if (name == PropertyName.PreviewRatio)
			{
				value = VariantUtils.CreateFrom<float>(PreviewRatio);
				return true;
			}
			if (name == PropertyName._dragVaseIndex)
			{
				value = VariantUtils.CreateFrom(in _dragVaseIndex);
				return true;
			}
			if (name == PropertyName._dragVase)
			{
				value = VariantUtils.CreateFrom(in _dragVase);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, PropertyName._dragVaseIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Object, PropertyName._dragVase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Int, PropertyName.Kind, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Object, PropertyName.Config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Bool, PropertyName.Editable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Float, PropertyName.PreviewRatio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(PropertyName.Kind, Variant.From<ProcessVisualKind>(Kind));
			info.AddProperty(PropertyName.Config, Variant.From<Resource>(Config));
			info.AddProperty(PropertyName.Editable, Variant.From<bool>(Editable));
			info.AddProperty(PropertyName.PreviewRatio, Variant.From<float>(PreviewRatio));
			info.AddProperty(PropertyName._dragVaseIndex, Variant.From(in _dragVaseIndex));
			info.AddProperty(PropertyName._dragVase, Variant.From(in _dragVase));
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			if (info.TryGetProperty(PropertyName.Kind, out var value))
			{
				Kind = value.As<ProcessVisualKind>();
			}
			if (info.TryGetProperty(PropertyName.Config, out var value2))
			{
				Config = value2.As<Resource>();
			}
			if (info.TryGetProperty(PropertyName.Editable, out var value3))
			{
				Editable = value3.As<bool>();
			}
			if (info.TryGetProperty(PropertyName.PreviewRatio, out var value4))
			{
				PreviewRatio = value4.As<float>();
			}
			if (info.TryGetProperty(PropertyName._dragVaseIndex, out var value5))
			{
				_dragVaseIndex = value5.As<int>();
			}
			if (info.TryGetProperty(PropertyName._dragVase, out var value6))
			{
				_dragVase = value6.As<TowerDefenseLevelVaseConfig>();
			}
		}
	}

	public const int MaximumTimelineSegments = 12;

	public const int MaximumBoardItems = 96;

	private XWGameplayLogicPresentationContext _context;

	private Resource _resource;

	private ProcessTimelineCanvas _timelineCanvas;

	private ProcessBoardCanvas _boardCanvas;

	private Timer _previewTimer;

	private Button _readyButton;

	private Button _battleButton;

	private Button _settleButton;

	private HSlider _scrubber;

	private Label _timeLabel;

	private Action _readyPressed;

	private Action _battlePressed;

	private Action _settlePressed;

	private Godot.Range.ValueChangedEventHandler _scrubberChanged;

	private bool _syncingTransport;

	private bool _runtimeProjection;

	private XWGameplayLogicPreviewSafety PreviewSafety => _context?.PreviewSafety;

	public double PreviewTime { get; private set; }

	public bool CanPresent(Resource resource)
	{
		if (resource is TowerDefenseBattleProcess || resource is TowerDefenseBattleProcessWaveEntryConfig || resource is TowerDefenseBattleProcessQuizConfig || resource is TowerDefenseLevelVaseManagerConfig || resource is TowerDefenseLevelVaseConfig || resource is TowerDefenseLevelVaseFillConfig || resource is TowerDefenseLevelIZMManagerConfig)
		{
			return true;
		}
		return false;
	}

	public void Mount(XWGameplayLogicPresentationContext context)
	{
		_context = context;
		_resource = context?.Resource;
	}

	public void Refresh()
	{
		if (GodotObject.IsInstanceValid(_resource))
		{
			DisconnectTransport();
			ClearChildren(_context?.StageRoot);
			ClearChildren(_context?.OverlayRoot);
			ClearChildren(_context?.HudRoot);
			ClearChildren(_context?.ShelfRoot);
			ClearChildren(_context?.TimelineRoot);
			PreviewTime = 0.0;
			_runtimeProjection = _resource is TowerDefenseBattleProcess;
			_timelineCanvas = new ProcessTimelineCanvas
			{
				CustomMinimumSize = new Vector2(320f, 52f),
				SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
				Binding = _context.PropertyBinding,
				Editable = !_runtimeProjection,
				PreviewChanged = SetPreviewTime
			};
			_context.TimelineRoot.AddChild(_timelineCanvas, forceReadableName: false, Node.InternalMode.Disabled);
			if (PreviewSafety != null)
			{
				PreviewSafety.TrackPreviewRoot(_timelineCanvas);
			}
			if (!((_resource is TowerDefenseBattleProcess process) ? MountRuntimeProcess(process) : MountConfigResource(_resource)))
			{
				MountEmpty("尚未识别的流程资源");
			}
			SetupTransport();
			SyncTimelineLimits();
			SetPreviewTime(0.0);
		}
	}

	public void Unmount()
	{
		DisconnectTransport();
		_timelineCanvas = null;
		_boardCanvas = null;
		_context = null;
		_resource = null;
	}

	private bool MountConfigResource(Resource resource)
	{
		if (!(resource is TowerDefenseBattleProcessWaveConfig config))
		{
			if (!(resource is TowerDefenseBattleProcessIZM2Config config2))
			{
				if (!(resource is TowerDefenseBattleProcessWaveEntryConfig config3))
				{
					if (!(resource is TowerDefenseBattleProcessQuizConfig config4))
					{
						if (!(resource is TowerDefenseLevelVaseManagerConfig config5))
						{
							if (!(resource is TowerDefenseLevelVaseConfig config6))
							{
								if (!(resource is TowerDefenseLevelVaseFillConfig config7))
								{
									if (resource is TowerDefenseLevelIZMManagerConfig config8)
									{
										return MountIZM(config8);
									}
									return false;
								}
								return MountVaseFill(config7);
							}
							return MountVaseItem(config6);
						}
						return MountVase(config5);
					}
					return MountQuiz(config4);
				}
				return MountWaveEntry(config3, "通用波次进入流程", ProcessVisualKind.Wave);
			}
			return MountIZM2(config2);
		}
		return MountWave(config);
	}

	private bool MountRuntimeProcess(TowerDefenseBattleProcess process)
	{
		AddProjectionNotice();
		if (!(process is TowerDefenseBattleProcessWave towerDefenseBattleProcessWave))
		{
			if (!(process is TowerDefenseBattleProcessVase towerDefenseBattleProcessVase))
			{
				if (!(process is TowerDefenseBattleProcessIZM towerDefenseBattleProcessIZM))
				{
					if (!(process is TowerDefenseBattleProcessIZM2 towerDefenseBattleProcessIZM2))
					{
						if (!(process is TowerDefenseBattleProcessQuiz towerDefenseBattleProcessQuiz))
						{
							if (process is TowerDefenseBattleProcessEmpty)
							{
								return MountEmpty("空流程不会推进胜负，仅保持种子栏可用。");
							}
							return false;
						}
						return MountQuiz(towerDefenseBattleProcessQuiz.config ?? CreateQuizConfig(towerDefenseBattleProcessQuiz.data));
					}
					return MountIZM2(towerDefenseBattleProcessIZM2.config ?? CreateIZM2Config(towerDefenseBattleProcessIZM2.data));
				}
				return MountIZM(towerDefenseBattleProcessIZM.config ?? CreateIZMConfig(towerDefenseBattleProcessIZM.data));
			}
			return MountVase(towerDefenseBattleProcessVase.config ?? CreateVaseConfig(towerDefenseBattleProcessVase.data));
		}
		return MountWave(towerDefenseBattleProcessWave.config ?? CreateWaveConfig(towerDefenseBattleProcessWave.data));
	}

	private bool MountWave(TowerDefenseBattleProcessWaveConfig config)
	{
		return MountWaveEntry(config, "标准波次流程", ProcessVisualKind.Wave);
	}

	private bool MountIZM2(TowerDefenseBattleProcessIZM2Config config)
	{
		return MountWaveEntry(config, "我是僵尸 2 · 波次流程", ProcessVisualKind.IZM2);
	}

	private bool MountWaveEntry(TowerDefenseBattleProcessWaveEntryConfig config, string title, ProcessVisualKind visualKind)
	{
		AddHeader(title, "拖动下方彩色阶段的分界线即可改变时长；播放只推进编辑器游标。", "Process");
		AddToggle(config, "mowerUse", "启用线路小推车");
		AddDuration(config, "entryBroadcastDuration", "僵尸来袭广播", 0.0, 15.0, new Color("c0504d"));
		AddDuration(config, "cameraTravelDuration", "镜头巡视", 0.0, 10.0, new Color("4f81bd"));
		AddDuration(config, "packetBankExitDelay", "选卡栏退出", 0.0, 5.0, new Color("9bbb59"));
		AddDuration(config, "entryLabelDuration", "关卡标签", 0.0, 10.0, new Color("8064a2"));
		AddDuration(config, "debugEnterHouseFadeDuration", "进屋淡出预演", 0.0, 5.0, new Color("f79646"));
		AddProcessBanner((visualKind == ProcessVisualKind.IZM2) ? "我是僵尸 · 准备进攻" : "更多的僵尸要来了！");
		AddBoardCanvas(visualKind, config);
		return true;
	}

	private bool MountVase(TowerDefenseLevelVaseManagerConfig config)
	{
		AddHeader("砸罐子流程", "花瓶按真实格子显示并可直接拖动；内容来源从左侧列表进入编辑。", "Vase");
		AddToggle(config, "shuffle", "随机打乱花瓶内容");
		AddToggle(config, "mowerUse", "启用线路小推车");
		AddEnum(config, "packetBankMethod", "选卡方式", Enum.GetNames<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(), storeString: false);
		AddDuration(config, "packetBankExitDelay", "选卡栏退出", 0.0, 5.0, new Color("9bbb59"));
		int num = Math.Min(config.vaseList?.Count ?? 0, 96);
		for (int i = 0; i < num; i++)
		{
			int index = i;
			TowerDefenseLevelVaseConfig vase = config.vaseList[i];
			if (GodotObject.IsInstanceValid(vase))
			{
				Button button = new Button
				{
					Text = $"花瓶 {i + 1} · {vase.gridPos} · {vase.packetName}"
				};
				button.Pressed += () =>
				{
					OpenNested(vase, config, "vaseList", index);
				};
				_context.ShelfRoot.AddChild(button, forceReadableName: false, Node.InternalMode.Disabled);
			}
		}
		int num2 = Math.Min(config.vaseFillList?.Count ?? 0, 96 - num);
		for (int num3 = 0; num3 < num2; num3++)
		{
			int index2 = num3;
			TowerDefenseLevelVaseFillConfig fill = config.vaseFillList[num3];
			if (GodotObject.IsInstanceValid(fill))
			{
				Button button2 = new Button
				{
					Text = "随机填充 · " + fill.packetName
				};
				button2.Pressed += () =>
				{
					OpenNested(fill, config, "vaseFillList", index2);
				};
				_context.ShelfRoot.AddChild(button2, forceReadableName: false, Node.InternalMode.Disabled);
			}
		}
		AddProcessBanner("砸开所有花瓶！");
		AddBoardCanvas(ProcessVisualKind.Vase, config);
		return true;
	}

	private bool MountVaseItem(TowerDefenseLevelVaseConfig config)
	{
		AddHeader("花瓶内容", "拖动草坪上的花瓶修改格子，内容卡片使用游戏资源选择器。", "Vase");
		AddResourceButton(config, "packetName", "花瓶内容", XWGameplayResourceKind.Card);
		AddEnum(config, "type", "花瓶外壳类型", new string[3] { "Normal", "Plant", "Zombie" }, storeString: true);
		AddBoardCanvas(ProcessVisualKind.VaseItem, config);
		return true;
	}

	private bool MountVaseFill(TowerDefenseLevelVaseFillConfig config)
	{
		AddHeader("花瓶随机填充来源", "候选卡片显示在草坪右侧填充池，不提前实例化战斗角色。", "Vase Fill");
		AddResourceButton(config, "packetName", "候选卡片", XWGameplayResourceKind.Card);
		AddBoardCanvas(ProcessVisualKind.VaseFill, config);
		return true;
	}

	private bool MountIZM(TowerDefenseLevelIZMManagerConfig config)
	{
		AddHeader("我是僵尸流程", "僵尸卡库、预生成、进屋判负与选卡退出在同一游戏画面中预演。", "IZM");
		AddToggle(config, "shuffle", "随机排列预生成单位");
		AddNumber(config, "preSpawnMaxRetryPasses", "预生成重试轮数", 1.0, 64.0, 1.0);
		AddNumber(config, "failureCheckIntervalFrames", "判负检查帧间隔", 1.0, 120.0, 1.0);
		AddDuration(config, "packetBankExitDelay", "僵尸选卡栏退出", 0.0, 5.0, new Color("9bbb59"));
		AddDuration(config, "enterHouseFadeDuration", "僵尸进屋淡出", 0.0, 5.0, new Color("c0504d"));
		AddResourceButton(config, "failureIgnoredZombieName", "忽略判负的僵尸", XWGameplayResourceKind.Character);
		AddProcessBanner("选择僵尸，吃掉脑子！");
		AddBoardCanvas(ProcessVisualKind.IZM, config);
		return true;
	}

	private bool MountQuiz(TowerDefenseBattleProcessQuizConfig config)
	{
		AddHeader("谁笑到最后 · 竞猜流程", "礼盒区和僵尸罐区按真实分界列绘制，结算金币阶段可拖动。", "Quiz");
		AddToggle(config, "autoStripColumn", "自动计算分界列");
		AddNumber(config, "stripColumn", "手动分界列", 1.0, 64.0, 1.0);
		AddResourceButton(config, "potPacketName", "花盆", XWGameplayResourceKind.Card);
		AddResourceButton(config, "lilyPadPacketName", "睡莲", XWGameplayResourceKind.Card);
		AddResourceButton(config, "presentBoxPacketName", "礼盒", XWGameplayResourceKind.Card);
		AddResourceButton(config, "zombieVasePacketName", "僵尸罐", XWGameplayResourceKind.Card);
		AddResourceButton(config, "presentBoxPacketBank", "礼盒内容池", XWGameplayResourceKind.PacketBank);
		AddResourceButton(config, "zombieVasePacketBank", "僵尸内容池", XWGameplayResourceKind.PacketBank);
		AddDuration(config, "settlementLineDelay", "逐行结算", 0.0, 10.0, new Color("4f81bd"));
		AddDuration(config, "settlementSummaryDelay", "结算摘要", 0.0, 10.0, new Color("8064a2"));
		AddDuration(config, "coinSpawnInterval", "金币生成", 0.0, 2.0, new Color("f2c94c"));
		AddDuration(config, "coinFlightDelay", "金币飞行", 0.0, 10.0, new Color("f79646"));
		AddDuration(config, "coinCollectDelay", "金币收集", 0.0, 10.0, new Color("9bbb59"));
		AddProcessBanner("选择一条线路，看看谁能笑到最后！");
		AddBoardCanvas(ProcessVisualKind.Quiz, config);
		return true;
	}

	private bool MountEmpty(string description)
	{
		AddHeader("空战斗流程", description, "Empty");
		AddProcessBanner("自由布置模式 · 不自动结算");
		AddBoardCanvas(ProcessVisualKind.Empty, null);
		return true;
	}

	private void AddHeader(string title, string hint, string badge)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		Label label = new Label
		{
			Text = title,
			SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
		};
		label.AddThemeFontSizeOverride("font_size", 18);
		hBoxContainer.AddChild(label, forceReadableName: false, Node.InternalMode.Disabled);
		hBoxContainer.AddChild(new Label
		{
			Text = " " + badge + " ",
			Modulate = new Color("9ccf7a")
		}, forceReadableName: false, Node.InternalMode.Disabled);
		_context.ShelfRoot.AddChild(hBoxContainer, forceReadableName: false, Node.InternalMode.Disabled);
		_context.ShelfRoot.AddChild(new Label
		{
			Text = hint,
			AutowrapMode = TextServer.AutowrapMode.WordSmart,
			Modulate = new Color("9aa894")
		}, forceReadableName: false, Node.InternalMode.Disabled);
	}

	private void AddProjectionNotice()
	{
		_context.ShelfRoot.AddChild(new Label
		{
			Text = "运行时 Process 为安全投影，只读预览；请进入其配置资源保存修改。",
			AutowrapMode = TextServer.AutowrapMode.WordSmart,
			Modulate = new Color("e9a95f")
		}, forceReadableName: false, Node.InternalMode.Disabled);
	}

	private void AddProcessBanner(string text)
	{
		PanelContainer panelContainer = new PanelContainer
		{
			AnchorLeft = 0.2f,
			AnchorTop = 0.025f,
			AnchorRight = 0.8f,
			AnchorBottom = 0.13f,
			ZIndex = 20,
			MouseFilter = Control.MouseFilterEnum.Ignore
		};
		panelContainer.AddChild(new Label
		{
			Text = text,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		}, forceReadableName: false, Node.InternalMode.Disabled);
		_context.HudRoot.AddChild(panelContainer, forceReadableName: false, Node.InternalMode.Disabled);
	}

	private void AddToggle(Resource owner, StringName property, string label)
	{
		CheckBox checkBox = new CheckBox
		{
			Text = label,
			Disabled = _runtimeProjection
		};
		_context.PropertyBinding.BindToggle(checkBox, owner, property, RedrawPreview);
		_context.ShelfRoot.AddChild(checkBox, forceReadableName: false, Node.InternalMode.Disabled);
	}

	private void AddText(Resource owner, StringName property, string label)
	{
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.AddChild(new Label
		{
			Text = label
		}, forceReadableName: false, Node.InternalMode.Disabled);
		LineEdit lineEdit = new LineEdit
		{
			Editable = !_runtimeProjection
		};
		_context.PropertyBinding.BindText(lineEdit, owner, property, RedrawPreview);
		vBoxContainer.AddChild(lineEdit, forceReadableName: false, Node.InternalMode.Disabled);
		_context.ShelfRoot.AddChild(vBoxContainer, forceReadableName: false, Node.InternalMode.Disabled);
	}

	private void AddEnum(Resource owner, StringName property, string label, string[] choices, bool storeString)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddChild(new Label
		{
			Text = label,
			SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
		}, forceReadableName: false, Node.InternalMode.Disabled);
		OptionButton optionButton = new OptionButton
		{
			Disabled = _runtimeProjection,
			CustomMinimumSize = new Vector2(128f, 0f)
		};
		string[] array = choices;
		foreach (string label2 in array)
		{
			optionButton.AddItem(label2);
		}
		int value = (storeString ? System.Array.IndexOf(choices, owner.Get(property).AsString()) : owner.Get(property).AsInt32());
		optionButton.Selected = Math.Clamp(value, 0, Math.Max(0, choices.Length - 1));
		optionButton.ItemSelected += (long index) =>
		{
			Variant value2 = (storeString ? Variant.From(in choices[(int)index]) : Variant.From(in index));
			_context.PropertyBinding.SetValue(owner, property, value2, $"修改 {property}");
			RedrawPreview();
		};
		hBoxContainer.AddChild(optionButton, forceReadableName: false, Node.InternalMode.Disabled);
		_context.ShelfRoot.AddChild(hBoxContainer, forceReadableName: false, Node.InternalMode.Disabled);
	}

	private void AddNumber(Resource owner, StringName property, string label, double minimum, double maximum, double step)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddChild(new Label
		{
			Text = label,
			SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
		}, forceReadableName: false, Node.InternalMode.Disabled);
		SpinBox spinBox = new SpinBox
		{
			MinValue = minimum,
			MaxValue = maximum,
			Step = step,
			CustomMinimumSize = new Vector2(104f, 0f),
			Editable = !_runtimeProjection
		};
		_context.PropertyBinding.BindNumber(spinBox, owner, property, RedrawPreview);
		hBoxContainer.AddChild(spinBox, forceReadableName: false, Node.InternalMode.Disabled);
		_context.ShelfRoot.AddChild(hBoxContainer, forceReadableName: false, Node.InternalMode.Disabled);
	}

	private void AddDuration(Resource owner, StringName property, string label, double minimum, double maximum, Color color)
	{
		AddNumber(owner, property, label, minimum, maximum, 0.05);
		if (_timelineCanvas.SegmentCount < 12)
		{
			_timelineCanvas.AddSegment(owner, property, label, color, minimum, maximum);
		}
	}

	private void AddResourceButton(Resource owner, StringName property, string label, XWGameplayResourceKind kind)
	{
		Button button = new Button
		{
			Text = label + " · " + owner.Get(property).AsString(),
			Disabled = _runtimeProjection
		};
		button.Pressed += () =>
		{
			OpenResourcePicker(button, owner, property, label, kind);
		};
		_context.ShelfRoot.AddChild(button, forceReadableName: false, Node.InternalMode.Disabled);
	}

	private void OpenResourcePicker(Button button, Resource owner, StringName property, string label, XWGameplayResourceKind kind)
	{
		_context.ResourcePicker?.Open(kind, owner.Get(property).AsString(), (XWGameplayResourceChoice choice) =>
		{
			_context.PropertyBinding.SetValue(owner, property, choice.Key, $"选择 {property}");
			if (GodotObject.IsInstanceValid(button))
			{
				button.Text = label + " · " + choice.Key;
			}
			RedrawPreview();
		});
	}

	private void AddBoardCanvas(ProcessVisualKind kind, Resource config)
	{
		_boardCanvas = new ProcessBoardCanvas
		{
			Kind = kind,
			Config = config,
			Binding = _context.PropertyBinding,
			Editable = !_runtimeProjection,
			MouseFilter = Control.MouseFilterEnum.Stop
		};
		_context.OverlayRoot.AddChild(_boardCanvas, forceReadableName: false, Node.InternalMode.Disabled);
		_boardCanvas.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect, Control.LayoutPresetMode.Minsize);
		if (PreviewSafety != null)
		{
			PreviewSafety.TrackPreviewRoot(_boardCanvas);
		}
	}

	private void OpenNested(Resource child, Resource owner, string property, int index)
	{
		string text = _context.EditContext?.OwnerPath;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = owner.ResourcePath;
		}
		XWResourceEditContext context = XWResourceEditContext.ForProperty(child, owner, child.ResourcePath, text, property, index, "gameplay_logic", _context.EditContext?.IsBuiltInSource ?? XWResourceEditContext.IsBuiltInPath(text));
		XWEditorInterface.Instance?.EditResource(child, context);
	}

	private void SetupTransport()
	{
		Node node = _context.TimelineRoot?.GetParent();
		_readyButton = node?.GetNodeOrNull<Button>("Transport/ReadyButton");
		_battleButton = node?.GetNodeOrNull<Button>("Transport/BattleButton");
		_settleButton = node?.GetNodeOrNull<Button>("Transport/SettleButton");
		_scrubber = node?.GetNodeOrNull<HSlider>("Transport/Scrubber");
		_timeLabel = node?.GetNodeOrNull<Label>("Header/TimeLabel");
		if (GodotObject.IsInstanceValid(_readyButton))
		{
			_readyButton.Text = "⏮ 准备";
			_readyPressed = () =>
			{
				SetPreviewTime(0.0);
			};
			_readyButton.Pressed += _readyPressed;
		}
		if (GodotObject.IsInstanceValid(_battleButton))
		{
			_battleButton.Text = "▶ 播放";
			_battlePressed = TogglePreviewPlayback;
			_battleButton.Pressed += _battlePressed;
		}
		if (GodotObject.IsInstanceValid(_settleButton))
		{
			_settleButton.Text = "■ 结算";
			_settlePressed = () =>
			{
				StopPreviewPlayback();
				SetPreviewTime(_timelineCanvas?.TotalDuration ?? 0.0);
			};
			_settleButton.Pressed += _settlePressed;
		}
		if (GodotObject.IsInstanceValid(_scrubber))
		{
			_scrubberChanged = (double value) =>
			{
				if (!_syncingTransport)
				{
					SetPreviewTime(value);
				}
			};
			_scrubber.ValueChanged += _scrubberChanged;
		}
		_previewTimer = new Timer
		{
			WaitTime = 0.05,
			OneShot = false
		};
		_previewTimer.Timeout += OnPreviewTick;
		_context.TimelineRoot.AddChild(_previewTimer, forceReadableName: false, Node.InternalMode.Disabled);
		if (PreviewSafety != null)
		{
			PreviewSafety.TrackTimer(_previewTimer);
		}
	}

	private void DisconnectTransport()
	{
		if (GodotObject.IsInstanceValid(_previewTimer))
		{
			_previewTimer.Stop();
			_previewTimer.Timeout -= OnPreviewTick;
		}
		if (GodotObject.IsInstanceValid(_readyButton) && _readyPressed != null)
		{
			_readyButton.Pressed -= _readyPressed;
		}
		if (GodotObject.IsInstanceValid(_battleButton) && _battlePressed != null)
		{
			_battleButton.Pressed -= _battlePressed;
		}
		if (GodotObject.IsInstanceValid(_settleButton) && _settlePressed != null)
		{
			_settleButton.Pressed -= _settlePressed;
		}
		if (GodotObject.IsInstanceValid(_scrubber) && _scrubberChanged != null)
		{
			_scrubber.ValueChanged -= _scrubberChanged;
		}
		_previewTimer = null;
		_readyButton = null;
		_battleButton = null;
		_settleButton = null;
		_scrubber = null;
		_timeLabel = null;
		_readyPressed = null;
		_battlePressed = null;
		_settlePressed = null;
		_scrubberChanged = null;
	}

	private void TogglePreviewPlayback()
	{
		if (!GodotObject.IsInstanceValid(_previewTimer))
		{
			return;
		}
		if (_previewTimer.IsStopped())
		{
			if (PreviewTime >= (_timelineCanvas?.TotalDuration ?? 0.0))
			{
				SetPreviewTime(0.0);
			}
			_previewTimer.Start();
			_battleButton.Text = "⏸ 暂停";
		}
		else
		{
			StopPreviewPlayback();
		}
	}

	private void StopPreviewPlayback()
	{
		_previewTimer?.Stop();
		if (GodotObject.IsInstanceValid(_battleButton))
		{
			_battleButton.Text = "▶ 播放";
		}
	}

	private void OnPreviewTick()
	{
		AdvancePreview(_previewTimer?.WaitTime ?? 0.05);
	}

	public void AdvancePreview(double delta)
	{
		double num = _timelineCanvas?.TotalDuration ?? 0.0;
		if (num <= 0.0)
		{
			StopPreviewPlayback();
			return;
		}
		double num2 = PreviewTime + Math.Max(0.0, delta);
		if (num2 >= num)
		{
			num2 = num;
			StopPreviewPlayback();
		}
		SetPreviewTime(num2);
	}

	private void SetPreviewTime(double value)
	{
		double num = _timelineCanvas?.TotalDuration ?? 0.0;
		PreviewTime = Math.Clamp(double.IsFinite(value) ? value : 0.0, 0.0, Math.Max(0.0, num));
		_timelineCanvas?.SetPreview(PreviewTime);
		if (GodotObject.IsInstanceValid(_boardCanvas))
		{
			_boardCanvas.PreviewRatio = ((num <= 0.0) ? 0f : ((float)(PreviewTime / num)));
			_boardCanvas.QueueRedraw();
		}
		_syncingTransport = true;
		if (GodotObject.IsInstanceValid(_scrubber))
		{
			_scrubber.MaxValue = Math.Max(0.01, num);
			_scrubber.Value = PreviewTime;
		}
		_syncingTransport = false;
		if (GodotObject.IsInstanceValid(_timeLabel))
		{
			_timeLabel.Text = FormatTime(PreviewTime) + " / " + FormatTime(num);
		}
	}

	private void SyncTimelineLimits()
	{
		_timelineCanvas?.QueueRedraw();
		SetPreviewTime(PreviewTime);
	}

	private void RedrawPreview()
	{
		_boardCanvas?.QueueRedraw();
		SyncTimelineLimits();
	}

	private static string FormatTime(double seconds)
	{
		int num = Math.Max(0, (int)Math.Round(seconds));
		return $"{num / 60:00}:{num % 60:00}";
	}

	private static TowerDefenseBattleProcessWaveConfig CreateWaveConfig(Dictionary data)
	{
		TowerDefenseBattleProcessWaveConfig towerDefenseBattleProcessWaveConfig = new TowerDefenseBattleProcessWaveConfig();
		towerDefenseBattleProcessWaveConfig.Init(data ?? new Dictionary());
		return towerDefenseBattleProcessWaveConfig;
	}

	private static TowerDefenseBattleProcessIZM2Config CreateIZM2Config(Dictionary data)
	{
		TowerDefenseBattleProcessIZM2Config towerDefenseBattleProcessIZM2Config = new TowerDefenseBattleProcessIZM2Config();
		towerDefenseBattleProcessIZM2Config.Init(data ?? new Dictionary());
		return towerDefenseBattleProcessIZM2Config;
	}

	private static TowerDefenseLevelVaseManagerConfig CreateVaseConfig(Dictionary data)
	{
		TowerDefenseLevelVaseManagerConfig towerDefenseLevelVaseManagerConfig = new TowerDefenseLevelVaseManagerConfig();
		towerDefenseLevelVaseManagerConfig.Init(data ?? new Dictionary());
		return towerDefenseLevelVaseManagerConfig;
	}

	private static TowerDefenseLevelIZMManagerConfig CreateIZMConfig(Dictionary data)
	{
		TowerDefenseLevelIZMManagerConfig towerDefenseLevelIZMManagerConfig = new TowerDefenseLevelIZMManagerConfig();
		towerDefenseLevelIZMManagerConfig.Init(data ?? new Dictionary());
		return towerDefenseLevelIZMManagerConfig;
	}

	private static TowerDefenseBattleProcessQuizConfig CreateQuizConfig(Dictionary data)
	{
		TowerDefenseBattleProcessQuizConfig towerDefenseBattleProcessQuizConfig = new TowerDefenseBattleProcessQuizConfig();
		towerDefenseBattleProcessQuizConfig.Init(data ?? new Dictionary());
		return towerDefenseBattleProcessQuizConfig;
	}

	private static void ClearChildren(Node root)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return;
		}
		foreach (Node child in root.GetChildren())
		{
			root.RemoveChild(child);
			child.QueueFree();
		}
	}
}
