using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/CoinBank/CoinBank.cs")]
public class CoinBank : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadFontAfterFirstDraw = "LoadFontAfterFirstDraw";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName ShowCoinBank = "ShowCoinBank";

		public static readonly StringName StartHide = "StartHide";

		public static readonly StringName AddNum = "AddNum";

		public static readonly StringName UseCoin = "UseCoin";

		public static readonly StringName SetNum = "SetNum";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName num = "num";

		public static readonly StringName numShow = "numShow";

		public static readonly StringName coinBankTexture = "coinBankTexture";

		public static readonly StringName coinNumLabel = "coinNumLabel";

		public static readonly StringName _num = "_num";

		public static readonly StringName _numShow = "_numShow";

		public static readonly StringName _displayedNum = "_displayedNum";

		public static readonly StringName timer = "timer";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const string CoinBankFontPath = "res://Asset/Font/fzcq.ttf";

	private const long DebugMaximum = 999999999L;

	private const float HiddenAlphaThreshold = 0.001f;

	public NinePatchRect coinBankTexture;

	public Label coinNumLabel;

	private long _num;

	private double _numShow;

	private long _displayedNum = -9223372036854775808L;

	public double timer;

	[Export(PropertyHint.None, "")]
	public long num
	{
		get
		{
			return _num;
		}
		set
		{
			if (_num != value)
			{
				_num = value;
				numShow = value;
			}
		}
	}

	public double numShow
	{
		get
		{
			return _numShow;
		}
		set
		{
			_numShow = value;
			long num = (long)Mathf.Round(_numShow);
			if (coinNumLabel != null && num != _displayedNum)
			{
				_displayedNum = num;
				coinNumLabel.Text = num.ToString();
			}
		}
	}

	public override void _Ready()
	{
		coinBankTexture = GetNode<NinePatchRect>("%CoinBankTexture");
		coinNumLabel = GetNode<Label>("%CoinNumLabel");
		Modulate = new Color(Modulate, 0f);
		numShow = _num;
		CommandManager instance = CommandManager.Instance;
		if (instance != null && instance.debugCoinMax)
		{
			num = 999999999L;
		}
		SetPhysicsProcess(enable: false);
		LoadFontAfterFirstDraw();
	}

	private async void LoadFontAfterFirstDraw()
	{
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		if (IsInsideTree() && GodotObject.IsInstanceValid(coinNumLabel))
		{
			StartupLoadDiagnostics.Mark("deferred.coin_font.begin");
			FontFile fontFile = GD.Load<FontFile>("res://Asset/Font/fzcq.ttf");
			if (GodotObject.IsInstanceValid(fontFile))
			{
				coinNumLabel.AddThemeFontOverride("font", fontFile);
			}
			StartupLoadDiagnostics.Mark("deferred.coin_font.end");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (timer > 0.0)
		{
			timer = Mathf.Max(0.0, timer - delta);
			return;
		}
		Color modulate = Modulate;
		if (modulate.A <= 0.001f)
		{
			if (modulate.A > 0f)
			{
				Modulate = new Color(modulate, 0f);
			}
			SetPhysicsProcess(enable: false);
			return;
		}
		float num = Mathf.Lerp(modulate.A, 0f, (float)(5.0 * delta));
		if (num <= 0.001f)
		{
			num = 0f;
		}
		Modulate = new Color(modulate, num);
		if (num <= 0f)
		{
			SetPhysicsProcess(enable: false);
		}
	}

	public void ShowCoinBank(Vector2 pos = default(Vector2), bool still = false)
	{
		if (pos == default(Vector2))
		{
			pos = new Vector2(86f, 557f);
		}
		Position = pos;
		Modulate = new Color(Modulate);
		if (!still)
		{
			timer = 5.0;
			SetPhysicsProcess(enable: true);
		}
		else
		{
			timer = 10000000000.0;
			SetPhysicsProcess(enable: false);
		}
	}

	public void StartHide()
	{
		timer = 0.0;
		SetPhysicsProcess(enable: true);
	}

	public void AddNum(long _num)
	{
		num += _num;
	}

	public void UseCoin(long _num)
	{
		num -= _num;
	}

	public void SetNum(long _num)
	{
		num = _num;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFontAfterFirstDraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowCoinBank, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "still", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartHide, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddNum, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UseCoin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetNum, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.LoadFontAfterFirstDraw && args.Count == 0)
		{
			LoadFontAfterFirstDraw();
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCoinBank && args.Count == 2)
		{
			ShowCoinBank(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartHide && args.Count == 0)
		{
			StartHide();
			ret = default;
			return true;
		}
		if (method == MethodName.AddNum && args.Count == 1)
		{
			AddNum(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UseCoin && args.Count == 1)
		{
			UseCoin(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetNum && args.Count == 1)
		{
			SetNum(VariantUtils.ConvertTo<long>(in args[0]));
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
		if (method == MethodName.LoadFontAfterFirstDraw)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.ShowCoinBank)
		{
			return true;
		}
		if (method == MethodName.StartHide)
		{
			return true;
		}
		if (method == MethodName.AddNum)
		{
			return true;
		}
		if (method == MethodName.UseCoin)
		{
			return true;
		}
		if (method == MethodName.SetNum)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.numShow)
		{
			numShow = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.coinBankTexture)
		{
			coinBankTexture = VariantUtils.ConvertTo<NinePatchRect>(in value);
			return true;
		}
		if (name == PropertyName.coinNumLabel)
		{
			coinNumLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._num)
		{
			_num = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._numShow)
		{
			_numShow = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._displayedNum)
		{
			_displayedNum = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.num)
		{
			value = VariantUtils.CreateFrom<long>(num);
			return true;
		}
		if (name == PropertyName.numShow)
		{
			value = VariantUtils.CreateFrom<double>(numShow);
			return true;
		}
		if (name == PropertyName.coinBankTexture)
		{
			value = VariantUtils.CreateFrom(in coinBankTexture);
			return true;
		}
		if (name == PropertyName.coinNumLabel)
		{
			value = VariantUtils.CreateFrom(in coinNumLabel);
			return true;
		}
		if (name == PropertyName._num)
		{
			value = VariantUtils.CreateFrom(in _num);
			return true;
		}
		if (name == PropertyName._numShow)
		{
			value = VariantUtils.CreateFrom(in _numShow);
			return true;
		}
		if (name == PropertyName._displayedNum)
		{
			value = VariantUtils.CreateFrom(in _displayedNum);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.coinBankTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.coinNumLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._num, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._numShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._displayedNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.numShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.num, Variant.From<long>(num));
		info.AddProperty(PropertyName.numShow, Variant.From<double>(numShow));
		info.AddProperty(PropertyName.coinBankTexture, Variant.From(in coinBankTexture));
		info.AddProperty(PropertyName.coinNumLabel, Variant.From(in coinNumLabel));
		info.AddProperty(PropertyName._num, Variant.From(in _num));
		info.AddProperty(PropertyName._numShow, Variant.From(in _numShow));
		info.AddProperty(PropertyName._displayedNum, Variant.From(in _displayedNum));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.num, out var value))
		{
			num = value.As<long>();
		}
		if (info.TryGetProperty(PropertyName.numShow, out var value2))
		{
			numShow = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.coinBankTexture, out var value3))
		{
			coinBankTexture = value3.As<NinePatchRect>();
		}
		if (info.TryGetProperty(PropertyName.coinNumLabel, out var value4))
		{
			coinNumLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._num, out var value5))
		{
			_num = value5.As<long>();
		}
		if (info.TryGetProperty(PropertyName._numShow, out var value6))
		{
			_numShow = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._displayedNum, out var value7))
		{
			_displayedNum = value7.As<long>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value8))
		{
			timer = value8.As<double>();
		}
	}
}
