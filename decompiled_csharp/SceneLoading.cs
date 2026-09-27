using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/SceneManager/SceneLoadeing/SceneLoading.cs")]
public class SceneLoading : CanvasLayer
{
	public delegate void EnterEventHandler();

	public new class MethodName : CanvasLayer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetStatus = "SetStatus";

		public static readonly StringName EnterScene = "EnterScene";

		public static readonly StringName Exit = "Exit";

		public static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName Timeout = "Timeout";
	}

	public new class PropertyName : CanvasLayer.PropertyName
	{
		public static readonly StringName background = "background";

		public static readonly StringName stinky = "stinky";

		public static readonly StringName label = "label";

		public static readonly StringName statusLabel = "statusLabel";

		public static readonly StringName pointNum = "pointNum";

		public static readonly StringName _statusText = "_statusText";
	}

	public new class SignalName : CanvasLayer.SignalName
	{
	}

	public Control background;

	public AdobeAnimateSpriteBase stinky;

	public Label label;

	public Label statusLabel;

	public int pointNum;

	private string _statusText = "正在加载场景";

	public event EnterEventHandler OnEnter;

	public override void _Ready()
	{
		background = GetNode<Control>("%Background");
		stinky = GetNode<AdobeAnimateSpriteBase>("%Stinky");
		Control node = GetNode<Control>("%StinkyRenderMount");
		stinky.forceLocalRender = true;
		stinky.SetRenderClipControl(node);
		stinky.OnAnimeCompleted += AnimeCompleted;
		label = GetNode<Label>("%Label");
		statusLabel = GetNode<Label>("%StatusLabel");
		statusLabel.Text = _statusText;
		EnterScene();
		stinky.SetAnimation("Out", loop: false);
		GetNode<Timer>("Timer").Timeout += Timeout;
	}

	public void SetStatus(string status)
	{
		if (!string.IsNullOrWhiteSpace(status))
		{
			_statusText = status;
			if (GodotObject.IsInstanceValid(statusLabel))
			{
				statusLabel.Text = status;
			}
		}
	}

	public async void EnterScene()
	{
		Tween tween = CreateTween();
		tween.SetParallel();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Back);
		tween.TweenProperty(stinky, "scale", new Vector2(1f, 1f), 0.5).From(new Vector2(2f, 0f));
		tween.TweenProperty(background, "modulate:a", 1.0, 0.5).From(0.0);
		await ToSignal(tween, Tween.SignalName.Finished);
		OnEnter?.Invoke();
	}

	public async void Exit()
	{
		Tween tween = CreateTween();
		tween.SetParallel();
		tween.SetEase(Tween.EaseType.In);
		tween.SetTrans(Tween.TransitionType.Back);
		tween.TweenProperty(stinky, "scale", new Vector2(0f, 0f), 1.0).From(new Vector2(1f, 1f));
		tween.TweenProperty(background, "modulate:a", 0.0, 0.75).From(1.0);
		await ToSignal(tween, Tween.SignalName.Finished);
		QueueFree();
	}

	public void AnimeCompleted(string clip)
	{
		switch (clip)
		{
		case "In":
			stinky.SetAnimation("Out", loop: false);
			break;
		case "Out":
			stinky.SetAnimation("Crawl", loop: false);
			break;
		case "Crawl":
			stinky.SetAnimation("In", loop: false);
			break;
		}
	}

	public void Timeout()
	{
		pointNum = (pointNum + 1) % 7;
		label.Text = string.Concat("加载中", string.Concat(Enumerable.Repeat(" .", pointNum)));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "status", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnterScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Exit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Timeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetStatus && args.Count == 1)
		{
			SetStatus(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnterScene && args.Count == 0)
		{
			EnterScene();
			ret = default;
			return true;
		}
		if (method == MethodName.Exit && args.Count == 0)
		{
			Exit();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Timeout && args.Count == 0)
		{
			Timeout();
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
		if (method == MethodName.SetStatus)
		{
			return true;
		}
		if (method == MethodName.EnterScene)
		{
			return true;
		}
		if (method == MethodName.Exit)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.Timeout)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.background)
		{
			background = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.stinky)
		{
			stinky = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName.label)
		{
			label = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.statusLabel)
		{
			statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.pointNum)
		{
			pointNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._statusText)
		{
			_statusText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.background)
		{
			value = VariantUtils.CreateFrom(in background);
			return true;
		}
		if (name == PropertyName.stinky)
		{
			value = VariantUtils.CreateFrom(in stinky);
			return true;
		}
		if (name == PropertyName.label)
		{
			value = VariantUtils.CreateFrom(in label);
			return true;
		}
		if (name == PropertyName.statusLabel)
		{
			value = VariantUtils.CreateFrom(in statusLabel);
			return true;
		}
		if (name == PropertyName.pointNum)
		{
			value = VariantUtils.CreateFrom(in pointNum);
			return true;
		}
		if (name == PropertyName._statusText)
		{
			value = VariantUtils.CreateFrom(in _statusText);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.background, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.stinky, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.label, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.pointNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._statusText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.background, Variant.From(in background));
		info.AddProperty(PropertyName.stinky, Variant.From(in stinky));
		info.AddProperty(PropertyName.label, Variant.From(in label));
		info.AddProperty(PropertyName.statusLabel, Variant.From(in statusLabel));
		info.AddProperty(PropertyName.pointNum, Variant.From(in pointNum));
		info.AddProperty(PropertyName._statusText, Variant.From(in _statusText));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.background, out var value))
		{
			background = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.stinky, out var value2))
		{
			stinky = value2.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName.label, out var value3))
		{
			label = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.statusLabel, out var value4))
		{
			statusLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.pointNum, out var value5))
		{
			pointNum = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._statusText, out var value6))
		{
			_statusText = value6.As<string>();
		}
	}
}
