using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Extends/AdobeAnimateSprite/AdobeAnimateSpriteBase.cs")]
public class AdobeAnimateSpriteBase : AdobeAnimateSprite
{
	public new class MethodName : AdobeAnimateSprite.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName RunBatchedProcessExtension = "RunBatchedProcessExtension";

		public static readonly StringName BatchPhysicsUpdate = "BatchPhysicsUpdate";

		public static readonly StringName ChangeFrameRate = "ChangeFrameRate";
	}

	public new class PropertyName : AdobeAnimateSprite.PropertyName
	{
		public new static readonly StringName RequiresDisplayFrameBatchedProcessExtension = "RequiresDisplayFrameBatchedProcessExtension";

		public static readonly StringName RequiresDisplayFrameBatchPhysicsUpdate = "RequiresDisplayFrameBatchPhysicsUpdate";
	}

	public new class SignalName : AdobeAnimateSprite.SignalName
	{
	}

	protected override bool RequiresDisplayFrameBatchedProcessExtension => RequiresDisplayFrameBatchPhysicsUpdate;

	protected virtual bool RequiresDisplayFrameBatchPhysicsUpdate => false;

	public override void _Ready()
	{
		base._Ready();
		if (!AdobeAnimateSprite.IsEditorContext && Global.Instance != null)
		{
			trueFrameRate = Global.Instance.trueAnimeFrameRate;
			Global.Instance.OnAnimeFrameRateChange += ChangeFrameRate;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (!AdobeAnimateSprite.IsEditorContext && Global.Instance != null)
		{
			Global.Instance.OnAnimeFrameRateChange -= ChangeFrameRate;
		}
	}

	protected override void RunBatchedProcessExtension(double delta)
	{
		BatchPhysicsUpdate(delta);
	}

	public virtual void BatchPhysicsUpdate(double delta)
	{
	}

	public void ChangeFrameRate()
	{
		if (Global.Instance != null)
		{
			int num = ((Engine.MaxFps > 0) ? Engine.MaxFps : ((int)ProjectSettings.GetSetting("application/run/max_fps")));
			refreshEveryFlame = (double)num < Global.Instance.trueAnimeFrameRate;
			trueFrameRate = Global.Instance.trueAnimeFrameRate;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunBatchedProcessExtension, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchPhysicsUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChangeFrameRate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.RunBatchedProcessExtension && args.Count == 1)
		{
			RunBatchedProcessExtension(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchPhysicsUpdate && args.Count == 1)
		{
			BatchPhysicsUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChangeFrameRate && args.Count == 0)
		{
			ChangeFrameRate();
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
		if (method == MethodName.RunBatchedProcessExtension)
		{
			return true;
		}
		if (method == MethodName.BatchPhysicsUpdate)
		{
			return true;
		}
		if (method == MethodName.ChangeFrameRate)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.RequiresDisplayFrameBatchedProcessExtension)
		{
			from = RequiresDisplayFrameBatchedProcessExtension;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RequiresDisplayFrameBatchPhysicsUpdate)
		{
			from = RequiresDisplayFrameBatchPhysicsUpdate;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.RequiresDisplayFrameBatchedProcessExtension, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RequiresDisplayFrameBatchPhysicsUpdate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
