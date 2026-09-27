using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class ExplosionDispatchCountingEvent : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public static readonly StringName Reset = "Reset";

		public new static readonly StringName Execute = "Execute";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	public static readonly Dictionary<ulong, int> HitsByPhysicsFrame = new Dictionary<ulong, int>();

	public static ExplosionDispatchCountingEvent ExpectedInstance;

	public static int TotalHits;

	public static bool PreservedReference = true;

	public static void Reset(ExplosionDispatchCountingEvent expected)
	{
		HitsByPhysicsFrame.Clear();
		ExpectedInstance = expected;
		TotalHits = 0;
		PreservedReference = true;
	}

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		ulong physicsFrames = Engine.GetPhysicsFrames();
		HitsByPhysicsFrame.TryGetValue(physicsFrames, out var value);
		HitsByPhysicsFrame[physicsFrames] = value + 1;
		TotalHits++;
		PreservedReference &= this == ExpectedInstance;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Reset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "expected", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Reset && args.Count == 1)
		{
			Reset(VariantUtils.ConvertTo<ExplosionDispatchCountingEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Execute && args.Count == 2)
		{
			Execute(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Reset && args.Count == 1)
		{
			Reset(VariantUtils.ConvertTo<ExplosionDispatchCountingEvent>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Reset)
		{
			return true;
		}
		if (method == MethodName.Execute)
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
