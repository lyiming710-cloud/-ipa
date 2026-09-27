using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateAtlasBackgroundRefreshRuntimeProbe.cs")]
public class AdobeAnimateAtlasBackgroundRefreshRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "ADOBE_ANIMATE_ATLAS_BACKGROUND_REFRESH_RESULT";

	public override async void _Ready()
	{
		bool projectVisual = false;
		bool bootstrapVisual = false;
		bool projectPose = false;
		bool bootstrapPose = false;
		int visualArrayLayers = 0;
		string failure = string.Empty;
		try
		{
			await Task.Run(() =>
			{
				projectVisual = AdobeAnimateGlobalAtlasCache.RefreshProjectAtlas("res://Asset/Anime", bakePoseTextures: false);
				visualArrayLayers = (projectVisual ? AdobeAnimateGlobalAtlasCache.RefreshSharedVisualTextureArray() : 0);
				bootstrapVisual = projectVisual && visualArrayLayers > 0 && AdobeAnimateGlobalAtlasCache.RefreshBootstrapAtlas(bakePoseTextures: false);
			});
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if ((projectVisual && visualArrayLayers > 0) & bootstrapVisual)
			{
				await Task.Run(() =>
				{
					projectPose = AdobeAnimateGlobalAtlasCache.RefreshProjectPoseData();
					bootstrapPose = projectPose && AdobeAnimateGlobalAtlasCache.RefreshBootstrapPoseData();
				});
			}
		}
		catch (Exception ex)
		{
			failure = ex.ToString();
		}
		bool flag = ((projectVisual && visualArrayLayers > 0) & bootstrapVisual & projectPose & bootstrapPose) && string.IsNullOrEmpty(failure);
		GD.Print($"{"ADOBE_ANIMATE_ATLAS_BACKGROUND_REFRESH_RESULT"} passed={flag} projectVisual={projectVisual} visualArrayLayers={visualArrayLayers} bootstrapVisual={bootstrapVisual} projectPose={projectPose} bootstrapPose={bootstrapPose}");
		if (!string.IsNullOrEmpty(failure))
		{
			GD.PushError("ADOBE_ANIMATE_ATLAS_BACKGROUND_REFRESH_RESULT failure:\n" + failure);
		}
		GetTree().Quit((!flag) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
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
