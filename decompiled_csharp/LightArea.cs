using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Buff/LightArea/LightArea.cs")]
public class LightArea : AabbArea2D
{
	public new class MethodName : AabbArea2D.MethodName
	{
		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName RevealCharacter = "RevealCharacter";
	}

	public new class PropertyName : AabbArea2D.PropertyName
	{
	}

	public new class SignalName : AabbArea2D.SignalName
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!AabbShapeUtil.TryComputeAreaWorldRect(this, out var rect))
		{
			return;
		}
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(instance) && instance.characterRegistry != null)
		{
			List<TowerDefenseCharacter> charactersIntersectingRectList = instance.characterRegistry.GetCharactersIntersectingRectList(rect);
			for (int i = 0; i < charactersIntersectingRectList.Count; i++)
			{
				RevealCharacter(charactersIntersectingRectList[i]);
			}
		}
	}

	private void RevealCharacter(TowerDefenseCharacter tdChar)
	{
		if (GodotObject.IsInstanceValid(tdChar) && GodotObject.IsInstanceValid(tdChar.sprite) && tdChar.sprite.invisible)
		{
			tdChar.sprite.invisible = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RevealCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tdChar", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RevealCharacter && args.Count == 1)
		{
			RevealCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.RevealCharacter)
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
