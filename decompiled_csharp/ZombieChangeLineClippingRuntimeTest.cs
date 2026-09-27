using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ZombieChangeLineClippingRuntimeTest.cs")]
public class ZombieChangeLineClippingRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifyClip = "VerifyClip";

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

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager.currentControl;
		CharacterPositionRenderSyncControlStub control = new CharacterPositionRenderSyncControlStub();
		AddChild(control, forceReadableName: false, InternalMode.Disabled);
		control.characterNode = new Node2D();
		control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
		manager.currentControl = control;
		try
		{
			string[] array = new string[2] { "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Scene/Base/TowerDefenseZombieGargantuar.tscn" };
			foreach (string path in array)
			{
				TowerDefenseZombie zombie = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Reuse).Instantiate<TowerDefenseZombie>(PackedScene.GenEditState.Disabled);
				zombie.editorPreviewMode = true;
				zombie.inGame = false;
				zombie.Position = new Vector2(700f, 500f);
				control.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				try
				{
					for (int frame = 0; frame < 8; frame++)
					{
						await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
					}
					zombie.garlicComponent = zombie.componentManager.GetRuntime<GarlicComponent>();
					zombie.waterInteractionComponent = zombie.componentManager.GetRuntime<WaterInteractionComponent>();
					zombie.garlicComponent.changeLineDuration = 0.18f;
					bool[] array2 = new bool[2] { false, true };
					foreach (bool flag in array2)
					{
						zombie.inWater = flag;
						zombie.groundHeight = (flag ? (0.0 - zombie.waterHeight) : 0.0);
						float[] array3 = new float[2] { 1f, 1.5f };
						foreach (float num in array3)
						{
							zombie.transformPoint.Scale = Vector2.One * num;
							int[] array4 = new int[2] { -1, 1 };
							foreach (int num2 in array4)
							{
								zombie.gridPos = new Vector2I(5, 5);
								zombie.garlicComponent.moveDownChance = ((num2 > 0) ? 1 : 0);
								Task task = zombie.garlicComponent.ChangeLine();
								VerifyClip(zombie, "开始");
								while (!task.IsCompleted)
								{
									await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
									VerifyClip(zombie, "补间及完成");
								}
								await task;
								Check(!zombie.isChangeLine, "完成后退出换行状态");
								task = zombie.garlicComponent.ChangeLine();
								await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
								zombie.garlicComponent.CancelChangeLine();
								await task;
								VerifyClip(zombie, "取消");
								Check(!zombie.isChangeLine, "取消后退出换行状态");
							}
						}
					}
				}
				finally
				{
					zombie.Free();
				}
			}
		}
		catch (Exception ex)
		{
			_failures++;
			GD.PrintErr(ex);
		}
		finally
		{
			manager.currentControl = previousControl;
			control.QueueFree();
		}
		GD.Print($"ZOMBIE_CHANGE_LINE_CLIPPING_RESULT passed={_failures == 0} checks={_checks} failures={_failures}");
		GetTree().Quit((_failures != 0) ? 2 : 0);
	}

	private void VerifyClip(TowerDefenseZombie zombie, string stage)
	{
		float num = ((Dictionary<string, Variant>)typeof(ShaderEffectComponent).GetField("_visualParameters", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(zombie.shaderEffectComponent))["discardDownPos"].AsSingle();
		float num2 = 10000f;
		if (zombie.inWater)
		{
			Transform2D screenTransform = zombie.GetViewport().GetScreenTransform();
			screenTransform.Origin = Vector2.Zero;
			Vector2 logicalGlobalPosition = zombie.GetLogicalGlobalPosition(zombie.spriteGroup);
			logicalGlobalPosition.Y += zombie.waterInteractionComponent.discardOffsetIn * zombie.waterInteractionComponent.GetScaleRatioY() + (float)zombie.groundHeight;
			num2 = (screenTransform * logicalGlobalPosition).Y;
		}
		Check(Mathf.Abs(num - num2) < 0.01f, $"{zombie.config.name} {stage} water={zombie.inWater} actual={num} expected={num2}");
		Check(zombie.waterInteractionComponent.isInWater == zombie.inWater, "裁剪组件水陆状态与角色一致");
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PrintErr(message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(3)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifyClip, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.VerifyClip && args.Count == 2)
		{
			VerifyClip(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.VerifyClip)
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
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
