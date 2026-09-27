using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Resource/TowerDefenseBattleProcess.cs")]
public class TowerDefenseBattleProcess : TowerDefenseBattleComponentBase
{
	public new class MethodName : TowerDefenseBattleComponentBase.MethodName
	{
		public static readonly StringName InitManager = "InitManager";

		public static readonly StringName CheckFinal = "CheckFinal";

		public static readonly StringName CheckFail = "CheckFail";

		public static readonly StringName Finish = "Finish";

		public static readonly StringName CanFinish = "CanFinish";

		public static readonly StringName TryFinish = "TryFinish";

		public static readonly StringName PhysicsProcess = "PhysicsProcess";

		public static readonly StringName InputProcess = "InputProcess";

		public static readonly StringName ViewMap = "ViewMap";

		public new static readonly StringName GameFail = "GameFail";

		public static readonly StringName SaveProcess = "SaveProcess";

		public static readonly StringName LoadProcess = "LoadProcess";
	}

	public new class PropertyName : TowerDefenseBattleComponentBase.PropertyName
	{
	}

	public new class SignalName : TowerDefenseBattleComponentBase.SignalName
	{
	}

	public virtual void InitManager()
	{
	}

	public virtual bool CheckFinal()
	{
		return false;
	}

	public virtual bool CheckFail()
	{
		return false;
	}

	public virtual void Finish()
	{
	}

	public virtual bool CanFinish()
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(control))
		{
			return !control.HasPendingBattleOperations;
		}
		return false;
	}

	public bool TryFinish()
	{
		if (!CanFinish())
		{
			return false;
		}
		Finish();
		return true;
	}

	public virtual void PhysicsProcess(double delta)
	{
	}

	public virtual void InputProcess(InputEvent event_)
	{
	}

	public virtual void ViewMap()
	{
	}

	public virtual void GameFail(TowerDefenseCharacter enterCharacter)
	{
	}

	public virtual Dictionary SaveProcess()
	{
		return new Dictionary();
	}

	public virtual void LoadProcess(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.InitManager, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckFinal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckFail, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanFinish, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryFinish, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InputProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.ViewMap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "enterCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SaveProcess, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.InitManager && args.Count == 0)
		{
			InitManager();
			ret = default;
			return true;
		}
		if (method == MethodName.CheckFinal && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckFinal());
			return true;
		}
		if (method == MethodName.CheckFail && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckFail());
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		if (method == MethodName.CanFinish && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanFinish());
			return true;
		}
		if (method == MethodName.TryFinish && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryFinish());
			return true;
		}
		if (method == MethodName.PhysicsProcess && args.Count == 1)
		{
			PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InputProcess && args.Count == 1)
		{
			InputProcess(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ViewMap && args.Count == 0)
		{
			ViewMap();
			ret = default;
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 1)
		{
			GameFail(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveProcess && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveProcess());
			return true;
		}
		if (method == MethodName.LoadProcess && args.Count == 2)
		{
			LoadProcess(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.InitManager)
		{
			return true;
		}
		if (method == MethodName.CheckFinal)
		{
			return true;
		}
		if (method == MethodName.CheckFail)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		if (method == MethodName.CanFinish)
		{
			return true;
		}
		if (method == MethodName.TryFinish)
		{
			return true;
		}
		if (method == MethodName.PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.InputProcess)
		{
			return true;
		}
		if (method == MethodName.ViewMap)
		{
			return true;
		}
		if (method == MethodName.GameFail)
		{
			return true;
		}
		if (method == MethodName.SaveProcess)
		{
			return true;
		}
		if (method == MethodName.LoadProcess)
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
