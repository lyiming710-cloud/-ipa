using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Override/TowerDefenseCharacterOverride.cs")]
public class TowerDefenseCharacterOverride : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public static readonly StringName ExecuteCharacter = "ExecuteCharacter";

		public static readonly StringName ExecuteCharacterWhenReady = "ExecuteCharacterWhenReady";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName invisible = "invisible";

		public static readonly StringName scale = "scale";

		public static readonly StringName hitpointScale = "hitpointScale";

		public static readonly StringName walkSpeedScale = "walkSpeedScale";

		public static readonly StringName animeSpeedScale = "animeSpeedScale";

		public static readonly StringName armor = "armor";

		public static readonly StringName propertyChange = "propertyChange";

		public static readonly StringName spawnEvent = "spawnEvent";

		public static readonly StringName dieEvent = "dieEvent";

		public static readonly StringName canMowerMove = "canMowerMove";

		public static readonly StringName hypnoses = "hypnoses";

		public static readonly StringName spawnFromLeft = "spawnFromLeft";

		public static readonly StringName _hasCanMowerMoveOverride = "_hasCanMowerMoveOverride";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool invisible;

	[Export(PropertyHint.None, "")]
	public double scale = -1.0;

	[Export(PropertyHint.None, "")]
	public double hitpointScale = -1.0;

	[Export(PropertyHint.None, "")]
	public Vector2 walkSpeedScale = new Vector2(-1f, -1f);

	[Export(PropertyHint.None, "")]
	public Vector2 animeSpeedScale = new Vector2(-1f, -1f);

	[Export(PropertyHint.None, "")]
	public Array armor = new Array();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterPropertyChangeConfig> propertyChange = new Array<TowerDefenseCharacterPropertyChangeConfig>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> spawnEvent = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> dieEvent = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public bool canMowerMove;

	[Export(PropertyHint.None, "")]
	public bool hypnoses;

	[Export(PropertyHint.None, "")]
	public bool spawnFromLeft;

	private bool _hasCanMowerMoveOverride;

	public void Init(Dictionary data)
	{
		propertyChange.Clear();
		spawnEvent.Clear();
		dieEvent.Clear();
		invisible = data.GetValueOrDefault("Invisible", false).AsBool();
		scale = data.GetValueOrDefault("Scale", -1).AsDouble();
		hitpointScale = data.GetValueOrDefault("HitpointScale", -1).AsDouble();
		Variant valueOrDefault = data.GetValueOrDefault("WalkSpeedScale", new Array { -1, -1 });
		if (valueOrDefault.VariantType == Variant.Type.Array)
		{
			Array array = valueOrDefault.AsGodotArray();
			if (array.Count == 2)
			{
				walkSpeedScale = new Vector2((float)array[0].AsDouble(), (float)array[1].AsDouble());
			}
			else if (array.Count == 1)
			{
				walkSpeedScale = new Vector2((float)array[0].AsDouble(), (float)array[0].AsDouble());
			}
		}
		if (valueOrDefault.VariantType == Variant.Type.Float)
		{
			walkSpeedScale = Vector2.One * (float)valueOrDefault.AsDouble();
		}
		Variant valueOrDefault2 = data.GetValueOrDefault("AnimeSpeedScale", new Array { -1, -1 });
		if (valueOrDefault2.VariantType == Variant.Type.Array)
		{
			Array array2 = valueOrDefault2.AsGodotArray();
			if (array2.Count == 2)
			{
				animeSpeedScale = new Vector2((float)array2[0].AsDouble(), (float)array2[1].AsDouble());
			}
			else if (array2.Count == 1)
			{
				animeSpeedScale = new Vector2((float)array2[0].AsDouble(), (float)array2[0].AsDouble());
			}
		}
		if (valueOrDefault2.VariantType == Variant.Type.Float)
		{
			animeSpeedScale = Vector2.One * (float)valueOrDefault2.AsDouble();
		}
		armor = data.GetValueOrDefault("Armor", new Array()).AsGodotArray();
		Array array3 = data.GetValueOrDefault("PropertyChange", new Array()).AsGodotArray();
		if (array3 != null && array3.Count != 0)
		{
			foreach (Variant item in array3)
			{
				Dictionary data2 = item.AsGodotDictionary();
				TowerDefenseCharacterPropertyChangeConfig towerDefenseCharacterPropertyChangeConfig = new TowerDefenseCharacterPropertyChangeConfig();
				towerDefenseCharacterPropertyChangeConfig.Init(data2);
				propertyChange.Add(towerDefenseCharacterPropertyChangeConfig);
			}
		}
		Array array4 = data.GetValueOrDefault("SpawnEvent", new Array()).AsGodotArray();
		if (array4 != null && array4.Count != 0)
		{
			foreach (Variant item2 in array4)
			{
				Dictionary dictionary = item2.AsGodotDictionary();
				string text = dictionary.GetValueOrDefault("EventName", "").AsString();
				if (!string.IsNullOrEmpty(text))
				{
					TowerDefenseCharacterEventBase towerDefenseCharacterEventBase = TowerDefenseCharacterEventMathine.EventGet(text);
					Dictionary valueDictionary = dictionary.GetValueOrDefault("Value", new Dictionary()).AsGodotDictionary();
					towerDefenseCharacterEventBase.Init(valueDictionary);
					spawnEvent.Add(towerDefenseCharacterEventBase);
				}
			}
		}
		Array array5 = data.GetValueOrDefault("DieEvent", new Array()).AsGodotArray();
		if (array5 != null && array5.Count != 0)
		{
			foreach (Variant item3 in array5)
			{
				Dictionary dictionary2 = item3.AsGodotDictionary();
				string text2 = dictionary2.GetValueOrDefault("EventName", "").AsString();
				if (!string.IsNullOrEmpty(text2))
				{
					TowerDefenseCharacterEventBase towerDefenseCharacterEventBase2 = TowerDefenseCharacterEventMathine.EventGet(text2);
					Dictionary valueDictionary2 = dictionary2.GetValueOrDefault("Value", new Dictionary()).AsGodotDictionary();
					towerDefenseCharacterEventBase2.Init(valueDictionary2);
					dieEvent.Add(towerDefenseCharacterEventBase2);
				}
			}
		}
		_hasCanMowerMoveOverride = data.ContainsKey("CanMowerMove");
		canMowerMove = _hasCanMowerMoveOverride && data["CanMowerMove"].AsBool();
		hypnoses = data.GetValueOrDefault("Hypnoses", false).AsBool();
		spawnFromLeft = data.GetValueOrDefault("SpawnFromLeft", false).AsBool();
	}

	public Dictionary Export()
	{
		if (armor == null)
		{
			armor = new Array();
		}
		if (propertyChange == null)
		{
			propertyChange = new Array<TowerDefenseCharacterPropertyChangeConfig>();
		}
		if (spawnEvent == null)
		{
			spawnEvent = new Array<TowerDefenseCharacterEventBase>();
		}
		if (dieEvent == null)
		{
			dieEvent = new Array<TowerDefenseCharacterEventBase>();
		}
		Dictionary dictionary = new Dictionary
		{
			["Invisible"] = invisible,
			["Scale"] = scale,
			["HitpointScale"] = hitpointScale,
			["WalkSpeedScale"] = new Array { walkSpeedScale.X, walkSpeedScale.Y },
			["AnimeSpeedScale"] = new Array { animeSpeedScale.X, animeSpeedScale.Y },
			["Armor"] = armor,
			["PropertyChange"] = new Array(),
			["SpawnEvent"] = new Array(),
			["DieEvent"] = new Array()
		};
		if (_hasCanMowerMoveOverride || canMowerMove)
		{
			dictionary["CanMowerMove"] = canMowerMove;
		}
		if (hypnoses)
		{
			dictionary["Hypnoses"] = true;
		}
		if (spawnFromLeft)
		{
			dictionary["SpawnFromLeft"] = true;
		}
		foreach (TowerDefenseCharacterPropertyChangeConfig item in propertyChange)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				((Array)dictionary["PropertyChange"]).Add(item.Export());
			}
		}
		foreach (TowerDefenseCharacterEventBase item2 in spawnEvent)
		{
			if (GodotObject.IsInstanceValid(item2))
			{
				((Array)dictionary["SpawnEvent"]).Add(item2.Export());
			}
		}
		foreach (TowerDefenseCharacterEventBase item3 in dieEvent)
		{
			if (GodotObject.IsInstanceValid(item3))
			{
				((Array)dictionary["DieEvent"]).Add(item3.Export());
			}
		}
		return dictionary;
	}

	public void ExecuteCharacter(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return;
		}
		if (!character.IsNodeReady())
		{
			ExecuteCharacterWhenReady(character);
			return;
		}
		if (!character.invisible)
		{
			character.invisible = invisible;
		}
		if (scale != -1.0)
		{
			character.transformPoint.Scale = (float)scale * Vector2.One;
		}
		if (hitpointScale != -1.0)
		{
			character.instance.hitpointScale = hitpointScale;
		}
		if (character is TowerDefenseZombie towerDefenseZombie && walkSpeedScale != new Vector2(-1f, -1f))
		{
			towerDefenseZombie.walkSpeedScale *= (float)GD.RandRange(walkSpeedScale.X, walkSpeedScale.Y);
		}
		if (hypnoses)
		{
			character.Hypnoses();
		}
		if (spawnFromLeft && character is TowerDefenseZombie && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
			logicalGlobalPosition.X = (float)TowerDefenseManager.Instance.GetMapGroundLeft() - 40f;
			character.SetLogicalGlobalPosition(logicalGlobalPosition);
		}
		if (animeSpeedScale != new Vector2(-1f, -1f))
		{
			character.timeScale *= (float)GD.RandRange(animeSpeedScale.X, animeSpeedScale.Y);
		}
		if (armor != null && armor.Count != 0)
		{
			foreach (Variant item in armor)
			{
				character.instance.ArmorAdd(item.AsString());
			}
		}
		if (propertyChange != null && propertyChange.Count != 0)
		{
			foreach (TowerDefenseCharacterPropertyChangeConfig item2 in propertyChange)
			{
				item2.Execute(character);
			}
		}
		character.dieEvent.AddRange(dieEvent);
		foreach (TowerDefenseCharacterEventBase item3 in spawnEvent)
		{
			item3.Execute(character.GetLogicalGlobalPosition(), character);
		}
		if (_hasCanMowerMoveOverride || canMowerMove)
		{
			character.canMowerMove = canMowerMove;
		}
	}

	private async void ExecuteCharacterWhenReady(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			await ToSignal(character, Node.SignalName.Ready);
			if (GodotObject.IsInstanceValid(character))
			{
				ExecuteCharacter(character);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExecuteCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteCharacterWhenReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.ExecuteCharacter && args.Count == 1)
		{
			ExecuteCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteCharacterWhenReady && args.Count == 1)
		{
			ExecuteCharacterWhenReady(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.ExecuteCharacter)
		{
			return true;
		}
		if (method == MethodName.ExecuteCharacterWhenReady)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.invisible)
		{
			invisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.scale)
		{
			scale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitpointScale)
		{
			hitpointScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.walkSpeedScale)
		{
			walkSpeedScale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.animeSpeedScale)
		{
			animeSpeedScale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.armor)
		{
			armor = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.propertyChange)
		{
			propertyChange = VariantUtils.ConvertToArray<TowerDefenseCharacterPropertyChangeConfig>(in value);
			return true;
		}
		if (name == PropertyName.spawnEvent)
		{
			spawnEvent = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.dieEvent)
		{
			dieEvent = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.canMowerMove)
		{
			canMowerMove = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hypnoses)
		{
			hypnoses = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.spawnFromLeft)
		{
			spawnFromLeft = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasCanMowerMoveOverride)
		{
			_hasCanMowerMoveOverride = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.invisible)
		{
			value = VariantUtils.CreateFrom(in invisible);
			return true;
		}
		if (name == PropertyName.scale)
		{
			value = VariantUtils.CreateFrom(in scale);
			return true;
		}
		if (name == PropertyName.hitpointScale)
		{
			value = VariantUtils.CreateFrom(in hitpointScale);
			return true;
		}
		if (name == PropertyName.walkSpeedScale)
		{
			value = VariantUtils.CreateFrom(in walkSpeedScale);
			return true;
		}
		if (name == PropertyName.animeSpeedScale)
		{
			value = VariantUtils.CreateFrom(in animeSpeedScale);
			return true;
		}
		if (name == PropertyName.armor)
		{
			value = VariantUtils.CreateFrom(in armor);
			return true;
		}
		if (name == PropertyName.propertyChange)
		{
			value = VariantUtils.CreateFromArray(propertyChange);
			return true;
		}
		if (name == PropertyName.spawnEvent)
		{
			value = VariantUtils.CreateFromArray(spawnEvent);
			return true;
		}
		if (name == PropertyName.dieEvent)
		{
			value = VariantUtils.CreateFromArray(dieEvent);
			return true;
		}
		if (name == PropertyName.canMowerMove)
		{
			value = VariantUtils.CreateFrom(in canMowerMove);
			return true;
		}
		if (name == PropertyName.hypnoses)
		{
			value = VariantUtils.CreateFrom(in hypnoses);
			return true;
		}
		if (name == PropertyName.spawnFromLeft)
		{
			value = VariantUtils.CreateFrom(in spawnFromLeft);
			return true;
		}
		if (name == PropertyName._hasCanMowerMoveOverride)
		{
			value = VariantUtils.CreateFrom(in _hasCanMowerMoveOverride);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.invisible, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.scale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpointScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.walkSpeedScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.animeSpeedScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.armor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.propertyChange, PropertyHint.TypeString, "24/17:TowerDefenseCharacterPropertyChangeConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.spawnEvent, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.dieEvent, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canMowerMove, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hypnoses, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.spawnFromLeft, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasCanMowerMoveOverride, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.invisible, Variant.From(in invisible));
		info.AddProperty(PropertyName.scale, Variant.From(in scale));
		info.AddProperty(PropertyName.hitpointScale, Variant.From(in hitpointScale));
		info.AddProperty(PropertyName.walkSpeedScale, Variant.From(in walkSpeedScale));
		info.AddProperty(PropertyName.animeSpeedScale, Variant.From(in animeSpeedScale));
		info.AddProperty(PropertyName.armor, Variant.From(in armor));
		info.AddProperty(PropertyName.propertyChange, Variant.CreateFrom(propertyChange));
		info.AddProperty(PropertyName.spawnEvent, Variant.CreateFrom(spawnEvent));
		info.AddProperty(PropertyName.dieEvent, Variant.CreateFrom(dieEvent));
		info.AddProperty(PropertyName.canMowerMove, Variant.From(in canMowerMove));
		info.AddProperty(PropertyName.hypnoses, Variant.From(in hypnoses));
		info.AddProperty(PropertyName.spawnFromLeft, Variant.From(in spawnFromLeft));
		info.AddProperty(PropertyName._hasCanMowerMoveOverride, Variant.From(in _hasCanMowerMoveOverride));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.invisible, out var value))
		{
			invisible = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.scale, out var value2))
		{
			scale = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitpointScale, out var value3))
		{
			hitpointScale = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.walkSpeedScale, out var value4))
		{
			walkSpeedScale = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.animeSpeedScale, out var value5))
		{
			animeSpeedScale = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.armor, out var value6))
		{
			armor = value6.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.propertyChange, out var value7))
		{
			propertyChange = value7.AsGodotArray<TowerDefenseCharacterPropertyChangeConfig>();
		}
		if (info.TryGetProperty(PropertyName.spawnEvent, out var value8))
		{
			spawnEvent = value8.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.dieEvent, out var value9))
		{
			dieEvent = value9.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.canMowerMove, out var value10))
		{
			canMowerMove = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hypnoses, out var value11))
		{
			hypnoses = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.spawnFromLeft, out var value12))
		{
			spawnFromLeft = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasCanMowerMoveOverride, out var value13))
		{
			_hasCanMowerMoveOverride = value13.As<bool>();
		}
	}
}
