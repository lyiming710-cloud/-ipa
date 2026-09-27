using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/RefCounted/XWResourcePreview.cs")]
public class XWResourcePreview : Node
{
	[Signal]
	public delegate void PreviewReadyEventHandler(string path, Texture2D previewTexture, Texture2D previewSmall);

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName QueuePreviewResource = "QueuePreviewResource";

		public static readonly StringName ProcessQueue = "ProcessQueue";

		public static readonly StringName GeneratePreview = "GeneratePreview";

		public static readonly StringName ClearCache = "ClearCache";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _timer = "_timer";
	}

	public new class SignalName : Node.SignalName
	{
		public static readonly StringName PreviewReady = "PreviewReady";
	}

	private readonly HashSet<string> _queuedPaths = new HashSet<string>();

	private readonly Dictionary<string, Texture2D> _cache = new Dictionary<string, Texture2D>();

	private Timer _timer;

	private PreviewReadyEventHandler backing_PreviewReady;

	public event PreviewReadyEventHandler PreviewReady
	{
		add
		{
			backing_PreviewReady = (PreviewReadyEventHandler)Delegate.Combine(backing_PreviewReady, value);
		}
		remove
		{
			backing_PreviewReady = (PreviewReadyEventHandler)Delegate.Remove(backing_PreviewReady, value);
		}
	}

	public override void _Ready()
	{
		_timer = new Timer
		{
			WaitTime = 0.1,
			Autostart = true
		};
		_timer.Timeout += ProcessQueue;
		AddChild(_timer, forceReadableName: false, InternalMode.Disabled);
	}

	public void QueuePreviewResource(string path)
	{
		if (!string.IsNullOrEmpty(path) && !_cache.ContainsKey(path) && !_queuedPaths.Contains(path))
		{
			_queuedPaths.Add(path);
		}
	}

	private void ProcessQueue()
	{
		if (_queuedPaths.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>(_queuedPaths);
		_queuedPaths.Clear();
		foreach (string item in list)
		{
			if (!_cache.ContainsKey(item))
			{
				Texture2D texture2D = GeneratePreview(item);
				if (texture2D != null)
				{
					_cache[item] = texture2D;
					EmitSignal(SignalName.PreviewReady, item, texture2D, texture2D);
				}
			}
		}
	}

	private static Texture2D GeneratePreview(string path)
	{
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		if (ResourceLoader.Load(path, "", ResourceLoader.CacheMode.Reuse) is Texture2D texture2D && XWTextureSafety.CanPreview(texture2D))
		{
			return texture2D;
		}
		return null;
	}

	public void ClearCache()
	{
		_cache.Clear();
		_queuedPaths.Clear();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueuePreviewResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessQueue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GeneratePreview, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.QueuePreviewResource && args.Count == 1)
		{
			QueuePreviewResource(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessQueue && args.Count == 0)
		{
			ProcessQueue();
			ret = default;
			return true;
		}
		if (method == MethodName.GeneratePreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GeneratePreview(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearCache && args.Count == 0)
		{
			ClearCache();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GeneratePreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GeneratePreview(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.QueuePreviewResource)
		{
			return true;
		}
		if (method == MethodName.ProcessQueue)
		{
			return true;
		}
		if (method == MethodName.GeneratePreview)
		{
			return true;
		}
		if (method == MethodName.ClearCache)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._timer)
		{
			_timer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._timer)
		{
			value = VariantUtils.CreateFrom(in _timer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._timer, Variant.From(in _timer));
		info.AddSignalEventDelegate(SignalName.PreviewReady, backing_PreviewReady);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._timer, out var value))
		{
			_timer = value.As<Timer>();
		}
		if (info.TryGetSignalEventDelegate<PreviewReadyEventHandler>(SignalName.PreviewReady, out var value2))
		{
			backing_PreviewReady = value2;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.PreviewReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "previewTexture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "previewSmall", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null)
		};
	}

	protected void EmitSignalPreviewReady(string path, Texture2D previewTexture, Texture2D previewSmall)
	{
		StringName previewReady = SignalName.PreviewReady;
		_003C_003Ey__InlineArray3<Variant> buffer = default;
		buffer[0] = path;
		buffer[1] = previewTexture;
		buffer[2] = previewSmall;
		EmitSignal(previewReady, buffer);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.PreviewReady && args.Count == 3)
		{
			backing_PreviewReady?.Invoke(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1]), VariantUtils.ConvertTo<Texture2D>(in args[2]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.PreviewReady)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
