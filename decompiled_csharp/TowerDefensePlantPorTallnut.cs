using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/PorTallnut/Scene/TowerDefensePlantPorTallnut.cs")]
public class TowerDefensePlantPorTallnut : TowerDefensePlant, IProjectileZone
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName OpenEntered = "OpenEntered";

		public static readonly StringName OpenProcessing = "OpenProcessing";

		public static readonly StringName OpenExited = "OpenExited";

		public static readonly StringName OpenIdleEntered = "OpenIdleEntered";

		public static readonly StringName OpenIdleProcessing = "OpenIdleProcessing";

		public static readonly StringName OpenIdleExited = "OpenIdleExited";

		public static readonly StringName CloseEntered = "CloseEntered";

		public static readonly StringName CloseProcessing = "CloseProcessing";

		public static readonly StringName CloseExited = "CloseExited";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName ProcessHitBoxOverlaps = "ProcessHitBoxOverlaps";

		public static readonly StringName HandleOverlap = "HandleOverlap";

		public static readonly StringName UpdateRect = "UpdateRect";

		public static readonly StringName OnProjectileIntersect = "OnProjectileIntersect";

		public static readonly StringName ToggleOpen = "ToggleOpen";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName WorldRect = "WorldRect";

		public static readonly StringName GridY = "GridY";

		public static readonly StringName RowSpan = "RowSpan";

		public static readonly StringName _zoneRect = "_zoneRect";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName _portalTransportActive = "_portalTransportActive";

		public static readonly StringName open = "open";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string POR_TALLNUT_BODY = "uid://bs7ofo8ns0gja";

	private const string POR_TALLNUT_CRACKED1 = "uid://co7m11ber6yxy";

	private const string POR_TALLNUT_CRACKED2 = "uid://b0n2gnnrjp7nn";

	private const string POR_TALLNUT_LEFT1 = "uid://gmk2eahfgc02";

	private const string POR_TALLNUT_LEFT2 = "uid://bg8metd818nty";

	private const string POR_TALLNUT_LEFT3 = "uid://dbnfbqj81lk5n";

	private const string POR_TALLNUT_MID1 = "uid://llr1lytnmv6p";

	private const string POR_TALLNUT_MID2 = "uid://dquduwep24k4x";

	private const string POR_TALLNUT_MID3 = "uid://d3r08nkknpepr";

	private const string POR_TALLNUT_RIGHT1 = "uid://c1cblsxkffip8";

	private const string POR_TALLNUT_RIGHT2 = "uid://dajcpk5gab5i8";

	private const string POR_TALLNUT_RIGHT3 = "uid://c3qtg8ng54gxx";

	private AttackComponent _attackComponent;

	private MousePressComponent _mousePressComponent;

	private Rect2 _zoneRect;

	private StateHandle _openState;

	private StateHandle _openIdleState;

	private StateHandle _closeState;

	private bool _roleStateSignalsConnected;

	private bool _portalTransportActive;

	public bool open = true;

	public Rect2 WorldRect => _zoneRect;

	public int GridY => gridPos.Y;

	public int RowSpan => 0;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			_mousePressComponent = componentManager.GetRuntime<MousePressComponent>();
			_mousePressComponent.OnPressed += ToggleOpen;
			AddToGroup("PorTallnut");
			open = true;
			_openState = StateMachine?.GetStateById("plant.por_tallnut.open");
			_openIdleState = StateMachine?.GetStateById("plant.por_tallnut.open_idle");
			_closeState = StateMachine?.GetStateById("plant.por_tallnut.close");
			ConnectRoleStateSignals();
			if (BulletField.Instance != null)
			{
				BulletField.Instance.RegisterZone(this);
			}
		}
	}

	public override void _ExitTree()
	{
		if (!Engine.IsEditorHint() && BulletField.Instance != null)
		{
			BulletField.Instance.UnregisterZone(this);
		}
		if (_mousePressComponent != null)
		{
			_mousePressComponent.OnPressed -= ToggleOpen;
		}
		base._ExitTree();
	}

	private void ConnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			return;
		}
		StateHandle openState = _openState;
		if (openState == null || !openState.IsValid)
		{
			return;
		}
		StateHandle openIdleState = _openIdleState;
		if (openIdleState != null && openIdleState.IsValid)
		{
			StateHandle closeState = _closeState;
			if (closeState != null && closeState.IsValid)
			{
				_openState.Entered += OpenEntered;
				_openState.Exited += OpenExited;
				_openState.PhysicsProcessing += OpenProcessing;
				_openIdleState.Entered += OpenIdleEntered;
				_openIdleState.Exited += OpenIdleExited;
				_openIdleState.PhysicsProcessing += OpenIdleProcessing;
				_closeState.Entered += CloseEntered;
				_closeState.Exited += CloseExited;
				_closeState.PhysicsProcessing += CloseProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			_openState.Entered -= OpenEntered;
			_openState.Exited -= OpenExited;
			_openState.PhysicsProcessing -= OpenProcessing;
			_openIdleState.Entered -= OpenIdleEntered;
			_openIdleState.Exited -= OpenIdleExited;
			_openIdleState.PhysicsProcessing -= OpenIdleProcessing;
			_closeState.Entered -= CloseEntered;
			_closeState.Exited -= CloseExited;
			_closeState.PhysicsProcessing -= CloseProcessing;
			_openState = null;
			_openIdleState = null;
			_closeState = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		switch (damagePointName)
		{
		case "Damage0":
			sprite.SetAtlasReplace("PorTallnut_body.png", "uid://bs7ofo8ns0gja");
			sprite.SetAtlasReplace("PorTallnut_left1.png", "uid://gmk2eahfgc02");
			sprite.SetAtlasReplace("PorTallnut_mid1.png", "uid://llr1lytnmv6p");
			sprite.SetAtlasReplace("PorTallnut_right1.png", "uid://c1cblsxkffip8");
			break;
		case "Damage1":
			sprite.SetAtlasReplace("PorTallnut_body.png", "uid://co7m11ber6yxy");
			sprite.SetAtlasReplace("PorTallnut_left1.png", "uid://bg8metd818nty");
			sprite.SetAtlasReplace("PorTallnut_mid1.png", "uid://dquduwep24k4x");
			sprite.SetAtlasReplace("PorTallnut_right1.png", "uid://dajcpk5gab5i8");
			break;
		case "Damage2":
			sprite.SetAtlasReplace("PorTallnut_body.png", "uid://b0n2gnnrjp7nn");
			sprite.SetAtlasReplace("PorTallnut_left1.png", "uid://dbnfbqj81lk5n");
			sprite.SetAtlasReplace("PorTallnut_mid1.png", "uid://d3r08nkknpepr");
			sprite.SetAtlasReplace("PorTallnut_right1.png", "uid://c3qtg8ng54gxx");
			break;
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (open && GetTree().GetNodeCountInGroup("PorTallnut") >= 2)
		{
			SendStateEvent("ToOpen");
		}
	}

	public virtual void OpenEntered()
	{
		AudioManager.Instance.AudioPlay("Portal");
		sprite.SetAnimation("Open", loop: false, 0.2);
	}

	public virtual void OpenProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public virtual void OpenExited()
	{
	}

	public virtual void OpenIdleEntered()
	{
		_portalTransportActive = true;
		instance.canBeCollection = true;
		sprite.SetAnimation("OpenIdle", loop: true, 0.2);
	}

	public virtual void OpenIdleProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		if (!open || GetTree().GetNodeCountInGroup("PorTallnut") < 2)
		{
			SendStateEvent("ToClose");
		}
		ProcessHitBoxOverlaps();
	}

	public virtual void OpenIdleExited()
	{
		_portalTransportActive = false;
		instance.canBeCollection = true;
	}

	public virtual void CloseEntered()
	{
		AudioManager.Instance.AudioPlay("Portal");
		sprite.SetAnimation("Close", loop: true, 0.2);
	}

	public virtual void CloseProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public virtual void CloseExited()
	{
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!(clip == "Open"))
		{
			if (clip == "Close")
			{
				Idle();
			}
		}
		else
		{
			SendStateEvent("ToOpenIdle");
		}
	}

	private void ProcessHitBoxOverlaps()
	{
		if (TryGetActiveWorldHitRect(out var rect))
		{
			List<TowerDefenseCharacter> charactersIntersectingRectListExcludingCamp = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectListExcludingCamp(rect, camp);
			for (int i = 0; i < charactersIntersectingRectListExcludingCamp.Count; i++)
			{
				HandleOverlap(charactersIntersectingRectListExcludingCamp[i]);
			}
		}
	}

	private void HandleOverlap(Node character)
	{
		if (!open)
		{
			return;
		}
		if (character is TowerDefenseProjectile towerDefenseProjectile)
		{
			if (TryGetProjectileDestination(towerDefenseProjectile.GlobalPosition, towerDefenseProjectile.velocity, towerDefenseProjectile.camp, out var destination))
			{
				towerDefenseProjectile.GlobalPosition = destination.GetLogicalGlobalPosition() + new Vector2(instance.hypnoses ? 11 : (-11), 0f);
				towerDefenseProjectile.gridPos = destination.gridPos;
			}
		}
		else
		{
			if (!(character is TowerDefenseGroundItemBase) || (character is TowerDefensePlant && !(character is TowerDefensePlantBowlingBase)) || (character is TowerDefenseItem && !(character is TowerDefenseMower)) || character is TowerDefenseCrater || character is TowerDefenseGravestone || !(character is TowerDefenseCharacter towerDefenseCharacter))
			{
				return;
			}
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			Vector2 logicalGlobalPosition2 = towerDefenseCharacter.GetLogicalGlobalPosition();
			if (character is TowerDefenseZombie)
			{
				if (((TowerDefenseZombie)character).instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
				{
					return;
				}
				if (((TowerDefenseZombie)character).isChangeLine)
				{
					Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
					if (Mathf.Abs(logicalGlobalPosition2.Y - logicalGlobalPosition.Y) > mapGridSize.Y * 0.5f)
					{
						return;
					}
				}
				else if (((TowerDefenseZombie)character).gridPos.Y != gridPos.Y)
				{
					return;
				}
			}
			if (logicalGlobalPosition2.X < logicalGlobalPosition.X - 10f || !towerDefenseCharacter.instance.canBeCollection || !CanTarget(towerDefenseCharacter))
			{
				return;
			}
			Array<Node> nodesInGroup = GetTree().GetNodesInGroup("PorTallnut");
			nodesInGroup.Remove(this);
			if (nodesInGroup.Count <= 0)
			{
				return;
			}
			TowerDefensePlantPorTallnut towerDefensePlantPorTallnut = nodesInGroup.PickRandom() as TowerDefensePlantPorTallnut;
			if (GodotObject.IsInstanceValid(towerDefensePlantPorTallnut))
			{
				if (character is TowerDefenseZombie { isChangeLine: not false, garlicComponent: { IsReleased: false } } towerDefenseZombie)
				{
					towerDefenseZombie.garlicComponent.CancelChangeLine();
				}
				Vector2 logicalGlobalPosition3 = towerDefensePlantPorTallnut.GetLogicalGlobalPosition();
				towerDefenseCharacter.shadowComponent.saveShadowPosition.Y += logicalGlobalPosition3.Y - logicalGlobalPosition2.Y;
				towerDefenseCharacter.SetLogicalGlobalPosition(logicalGlobalPosition3 - new Vector2(11f, 0f));
				towerDefenseCharacter.gridPos = towerDefensePlantPorTallnut.gridPos;
			}
		}
	}

	public void UpdateRect()
	{
		if (open && _portalTransportActive && TryGetActiveWorldHitRect(out var rect))
		{
			_zoneRect = rect;
		}
		else
		{
			_zoneRect = new Rect2(Vector2.Zero, Vector2.Zero);
		}
	}

	public void OnBulletIntersect(ref BulletData bullet, int index)
	{
		if (bullet.active && !bullet.over && bullet.config != null && TryGetProjectileDestination(bullet.pos, bullet.vel, bullet.camp, out var destination) && GodotObject.IsInstanceValid(BulletField.Instance))
		{
			Vector2 newPos = destination.GetLogicalGlobalPosition() + new Vector2(instance.hypnoses ? 11 : (-11), 0f);
			BulletField.Instance.TeleportBullet(index, newPos, destination.gridPos.Y);
			bullet.gridPos = destination.gridPos;
		}
	}

	public void OnProjectileIntersect(TowerDefenseProjectile projectile)
	{
		HandleOverlap(projectile);
	}

	private bool TryGetProjectileDestination(Vector2 projectilePosition, Vector2 projectileVelocity, TowerDefenseEnum.CHARACTER_CAMP projectileCamp, out TowerDefensePlantPorTallnut destination)
	{
		destination = null;
		if (!open || !_portalTransportActive || projectileCamp != camp)
		{
			return false;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		if (!instance.hypnoses)
		{
			if (projectilePosition.X < logicalGlobalPosition.X - 10f || projectileVelocity.X > 0f)
			{
				return false;
			}
		}
		else if (projectilePosition.X > logicalGlobalPosition.X + 10f || projectileVelocity.X < 0f)
		{
			return false;
		}
		Array<Node> nodesInGroup = GetTree().GetNodesInGroup("PorTallnut");
		nodesInGroup.Remove(this);
		if (nodesInGroup.Count == 0)
		{
			return false;
		}
		destination = nodesInGroup.PickRandom() as TowerDefensePlantPorTallnut;
		return GodotObject.IsInstanceValid(destination);
	}

	public void ToggleOpen(Vector2 pos)
	{
		open = !open;
		if (open)
		{
			AddToGroup("PorTallnut");
		}
		else
		{
			RemoveFromGroup("PorTallnut");
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "open", open } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		open = data.GetValueOrDefault("open", Variant.From<bool>(true)).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(23)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenIdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenIdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenIdleExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloseEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloseProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessHitBoxOverlaps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleOverlap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateRect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnProjectileIntersect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ToggleOpen, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectRoleStateSignals && args.Count == 0)
		{
			ConnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals && args.Count == 0)
		{
			DisconnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenEntered && args.Count == 0)
		{
			OpenEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenProcessing && args.Count == 1)
		{
			OpenProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenExited && args.Count == 0)
		{
			OpenExited();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenIdleEntered && args.Count == 0)
		{
			OpenIdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenIdleProcessing && args.Count == 1)
		{
			OpenIdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenIdleExited && args.Count == 0)
		{
			OpenIdleExited();
			ret = default;
			return true;
		}
		if (method == MethodName.CloseEntered && args.Count == 0)
		{
			CloseEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.CloseProcessing && args.Count == 1)
		{
			CloseProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloseExited && args.Count == 0)
		{
			CloseExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessHitBoxOverlaps && args.Count == 0)
		{
			ProcessHitBoxOverlaps();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleOverlap && args.Count == 1)
		{
			HandleOverlap(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateRect && args.Count == 0)
		{
			UpdateRect();
			ret = default;
			return true;
		}
		if (method == MethodName.OnProjectileIntersect && args.Count == 1)
		{
			OnProjectileIntersect(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleOpen && args.Count == 1)
		{
			ToggleOpen(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName.ConnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.OpenEntered)
		{
			return true;
		}
		if (method == MethodName.OpenProcessing)
		{
			return true;
		}
		if (method == MethodName.OpenExited)
		{
			return true;
		}
		if (method == MethodName.OpenIdleEntered)
		{
			return true;
		}
		if (method == MethodName.OpenIdleProcessing)
		{
			return true;
		}
		if (method == MethodName.OpenIdleExited)
		{
			return true;
		}
		if (method == MethodName.CloseEntered)
		{
			return true;
		}
		if (method == MethodName.CloseProcessing)
		{
			return true;
		}
		if (method == MethodName.CloseExited)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.ProcessHitBoxOverlaps)
		{
			return true;
		}
		if (method == MethodName.HandleOverlap)
		{
			return true;
		}
		if (method == MethodName.UpdateRect)
		{
			return true;
		}
		if (method == MethodName.OnProjectileIntersect)
		{
			return true;
		}
		if (method == MethodName.ToggleOpen)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._zoneRect)
		{
			_zoneRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._portalTransportActive)
		{
			_portalTransportActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.open)
		{
			open = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.WorldRect)
		{
			value = VariantUtils.CreateFrom<Rect2>(WorldRect);
			return true;
		}
		int from;
		if (name == PropertyName.GridY)
		{
			from = GridY;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RowSpan)
		{
			from = RowSpan;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._zoneRect)
		{
			value = VariantUtils.CreateFrom(in _zoneRect);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName._portalTransportActive)
		{
			value = VariantUtils.CreateFrom(in _portalTransportActive);
			return true;
		}
		if (name == PropertyName.open)
		{
			value = VariantUtils.CreateFrom(in open);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Rect2, PropertyName._zoneRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._portalTransportActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.open, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.WorldRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.GridY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RowSpan, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._zoneRect, Variant.From(in _zoneRect));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName._portalTransportActive, Variant.From(in _portalTransportActive));
		info.AddProperty(PropertyName.open, Variant.From(in open));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._zoneRect, out var value))
		{
			_zoneRect = value.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value2))
		{
			_roleStateSignalsConnected = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._portalTransportActive, out var value3))
		{
			_portalTransportActive = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.open, out var value4))
		{
			open = value4.As<bool>();
		}
	}
}
