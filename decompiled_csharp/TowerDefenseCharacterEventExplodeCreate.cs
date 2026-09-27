using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventExplodeCreate.cs")]
public class TowerDefenseCharacterEventExplodeCreate : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public static readonly StringName Run = "Run";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName effectScene = "effectScene";

		public static readonly StringName effectAudio = "effectAudio";

		public static readonly StringName explodeSize = "explodeSize";

		public static readonly StringName onlyLastPenetrateHit = "onlyLastPenetrateHit";

		public static readonly StringName explodeEvents = "explodeEvents";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public PackedScene effectScene;

	[Export(PropertyHint.None, "")]
	public string effectAudio = "";

	[Export(PropertyHint.None, "")]
	public Vector2 explodeSize = new Vector2(0.5f, 0.5f);

	[Export(PropertyHint.None, "")]
	public bool onlyLastPenetrateHit = true;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> explodeEvents;

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(pos, target);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(pos, target);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		if (!onlyLastPenetrateHit || projectile.penetrateNum <= 0)
		{
			Run(projectile.GlobalPosition, target, projectile.camp, projectile.collisionFlags);
		}
	}

	public void Run(Vector2 pos, TowerDefenseCharacter target, TowerDefenseEnum.CHARACTER_CAMP camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT, int collisionFlags = 0)
	{
		if (effectScene != null)
		{
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(effectScene, target.gridPos, "Idle");
			if (GodotObject.IsInstanceValid(towerDefenseEffectSpriteOnce))
			{
				Node2D characterNode = TowerDefenseManager.GetCharacterNode();
				if (GodotObject.IsInstanceValid(characterNode))
				{
					characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, Node.InternalMode.Disabled);
					towerDefenseEffectSpriteOnce.gridPos = TowerDefenseManager.Instance.GetMapGridPos(pos);
					towerDefenseEffectSpriteOnce.GlobalPosition = pos;
				}
				else
				{
					towerDefenseEffectSpriteOnce.QueueFree();
				}
			}
		}
		if (!string.IsNullOrEmpty(effectAudio))
		{
			AudioManager.Instance.AudioPlay(effectAudio);
		}
		if (explodeEvents != null && explodeEvents.Count > 0)
		{
			TowerDefenseExplode.CreateExplode(target.GetLogicalGlobalPosition(), explodeSize, explodeEvents, null, camp, collisionFlags);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteDps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteProject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Execute && args.Count == 2)
		{
			Execute(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteDps && args.Count == 3)
		{
			ExecuteDps(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteProject && args.Count == 2)
		{
			ExecuteProject(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Run && args.Count == 4)
		{
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.ExecuteDps)
		{
			return true;
		}
		if (method == MethodName.ExecuteProject)
		{
			return true;
		}
		if (method == MethodName.Run)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.effectScene)
		{
			effectScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.effectAudio)
		{
			effectAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.explodeSize)
		{
			explodeSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.onlyLastPenetrateHit)
		{
			onlyLastPenetrateHit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.explodeEvents)
		{
			explodeEvents = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.effectScene)
		{
			value = VariantUtils.CreateFrom(in effectScene);
			return true;
		}
		if (name == PropertyName.effectAudio)
		{
			value = VariantUtils.CreateFrom(in effectAudio);
			return true;
		}
		if (name == PropertyName.explodeSize)
		{
			value = VariantUtils.CreateFrom(in explodeSize);
			return true;
		}
		if (name == PropertyName.onlyLastPenetrateHit)
		{
			value = VariantUtils.CreateFrom(in onlyLastPenetrateHit);
			return true;
		}
		if (name == PropertyName.explodeEvents)
		{
			value = VariantUtils.CreateFromArray(explodeEvents);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.effectScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.effectAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.explodeSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.onlyLastPenetrateHit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.explodeEvents, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.effectScene, Variant.From(in effectScene));
		info.AddProperty(PropertyName.effectAudio, Variant.From(in effectAudio));
		info.AddProperty(PropertyName.explodeSize, Variant.From(in explodeSize));
		info.AddProperty(PropertyName.onlyLastPenetrateHit, Variant.From(in onlyLastPenetrateHit));
		info.AddProperty(PropertyName.explodeEvents, Variant.CreateFrom(explodeEvents));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.effectScene, out var value))
		{
			effectScene = value.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.effectAudio, out var value2))
		{
			effectAudio = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.explodeSize, out var value3))
		{
			explodeSize = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.onlyLastPenetrateHit, out var value4))
		{
			onlyLastPenetrateHit = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.explodeEvents, out var value5))
		{
			explodeEvents = value5.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
	}
}
