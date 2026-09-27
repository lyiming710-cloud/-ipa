using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/MowerHitVisualOwnershipRuntimeTest.cs")]
public class MowerHitVisualOwnershipRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string MowerHitScenePath = "res://Prefab/TowerDefense/Mower/MowerHit.tscn";

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		MowerHit mowerHit = null;
		MowerHitVisualTargetStub mowerHitVisualTargetStub = null;
		try
		{
			PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Prefab/TowerDefense/Mower/MowerHit.tscn", null, ResourceLoader.CacheMode.Ignore);
			Check(GodotObject.IsInstanceValid(packedScene), "The real mower-hit scene must load.");
			mowerHit = packedScene?.Instantiate<MowerHit>(PackedScene.GenEditState.Disabled);
			Check(GodotObject.IsInstanceValid(mowerHit), "The real mower-hit scene must instantiate.");
			if (GodotObject.IsInstanceValid(mowerHit))
			{
				AddChild(mowerHit, forceReadableName: false, InternalMode.Disabled);
				mowerHitVisualTargetStub = new MowerHitVisualTargetStub
				{
					Name = "MowerHitVisualTarget",
					Position = new Vector2(180f, 90f),
					Scale = new Vector2(1.25f, 0.8f)
				};
				AddChild(mowerHitVisualTargetStub, forceReadableName: false, InternalMode.Disabled);
				ulong instanceId = mowerHitVisualTargetStub.GetInstanceId();
				bool condition = mowerHit.Init(mowerHitVisualTargetStub);
				Check(condition, "The real mower-hit effect must accept a live target.");
				Check(mowerHitVisualTargetStub.GetInstanceId() == instanceId, "The squash effect must retain the original character instance.");
				Check(mowerHitVisualTargetStub.mowerDeathVisualOwned, "The target must publish mower visual ownership immediately.");
				Check(mowerHitVisualTargetStub.GetParent() is AdobeAnimateSlot, "The target must be mounted into the animation slot synchronously.");
				Check(mowerHitVisualTargetStub.Position.IsEqualApprox(Vector2.Zero), "The mounted target must be centered on the mower-hit slot.");
				AdobeAnimateSlot adobeAnimateSlot = mowerHitVisualTargetStub.GetParent() as AdobeAnimateSlot;
				Check(GodotObject.IsInstanceValid(adobeAnimateSlot) && adobeAnimateSlot.followSlotId == 1 && adobeAnimateSlot.useScale && adobeAnimateSlot.GetParent() is AdobeAnimateSprite, "The target must retain the authored scale-following squash slot.");
				Check(mowerHit.character == mowerHitVisualTargetStub, "The effect must retain exactly the original target reference.");
				Check(!mowerHitVisualTargetStub.IsQueuedForDeletion(), "The target must remain alive for the authored squash animation.");
			}
		}
		catch (Exception ex)
		{
			GD.PushError(ex.ToString());
			_failures++;
		}
		finally
		{
			GD.Print($"MOWER_HIT_VISUAL_OWNERSHIP_RESULT passed={_failures == 0} checks={_checks} failures={_failures}");
			GetTree().Quit((_failures != 0) ? 1 : 0);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError(message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
