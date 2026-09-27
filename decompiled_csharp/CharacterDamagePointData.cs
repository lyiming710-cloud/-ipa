using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/General/Character/DamagePoint/CharacterDamagePointData.cs")]
public class CharacterDamagePointData : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName SplitFliter = "SplitFliter";

		public static readonly StringName ClearDamagePointFliters = "ClearDamagePointFliters";

		public static readonly StringName ClearDamagePointAll = "ClearDamagePointAll";

		public static readonly StringName SetDamagePointFliters = "SetDamagePointFliters";

		public static readonly StringName CreateEffect = "CreateEffect";

		public static readonly StringName ResolveLogicalSpritePosition = "ResolveLogicalSpritePosition";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName damagePointList = "damagePointList";

		public static readonly StringName _damagePointList = "_damagePointList";

		public static readonly StringName damagePointDictionary = "damagePointDictionary";

		public static readonly StringName fliterOpenAll = "fliterOpenAll";

		public static readonly StringName fliterCloseAll = "fliterCloseAll";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private Array<CharacterDamagePointConfig> _damagePointList = new Array<CharacterDamagePointConfig>();

	[Export(PropertyHint.None, "")]
	public Dictionary damagePointDictionary = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Array<string> fliterOpenAll = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<string> fliterCloseAll = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<CharacterDamagePointConfig> damagePointList
	{
		get
		{
			return _damagePointList;
		}
		set
		{
			_damagePointList = value ?? new Array<CharacterDamagePointConfig>();
			EmitChanged();
			Refresh();
		}
	}

	public void Refresh()
	{
		fliterOpenAll.Clear();
		fliterCloseAll.Clear();
		damagePointDictionary.Clear();
		List<CharacterDamagePointConfig> list = _damagePointList.Where((CharacterDamagePointConfig config) => config != null).ToList();
		list.Sort((CharacterDamagePointConfig a, CharacterDamagePointConfig b) => b.damagePersontage.CompareTo(a.damagePersontage));
		foreach (CharacterDamagePointConfig item in list)
		{
			Array<string> array = SplitFliter(item.animeFliterOpen);
			Array<string> array2 = SplitFliter(item.animeFliterClose);
			damagePointDictionary[item.damagePointName] = new Dictionary
			{
				["Config"] = item,
				["Open"] = array,
				["Close"] = array2
			};
			if (array.Count > 0)
			{
				foreach (string item2 in array)
				{
					fliterOpenAll.Add(item2);
				}
			}
			if (array2.Count <= 0)
			{
				continue;
			}
			foreach (string item3 in array2)
			{
				fliterCloseAll.Add(item3);
			}
		}
	}

	private Array<string> SplitFliter(string fliter)
	{
		Array<string> array = new Array<string>();
		if (string.IsNullOrEmpty(fliter))
		{
			return array;
		}
		string[] array2 = fliter.Split("&", StringSplitOptions.RemoveEmptyEntries);
		foreach (string item in array2)
		{
			array.Add(item);
		}
		return array;
	}

	public void ClearDamagePointFliters(AdobeAnimateSprite sprite)
	{
		sprite.SetFlitersRecursive((Godot.Collections.Array?)fliterCloseAll, open: true);
		sprite.SetFlitersRecursive((Godot.Collections.Array?)fliterOpenAll, open: false);
	}

	public void ClearDamagePointAll(AdobeAnimateSprite sprite)
	{
		ClearDamagePointFliters(sprite);
		foreach (CharacterDamagePointConfig damagePoint in damagePointList)
		{
			if (damagePoint != null && !damagePoint.replaceMediaName.IsEmpty)
			{
				sprite.SetAtlasReplace(damagePoint.replaceMediaName, string.Empty);
			}
		}
	}

	public void SetDamagePointFliters(AdobeAnimateSprite sprite, string damagePointName)
	{
		if (!damagePointDictionary.ContainsKey(damagePointName))
		{
			return;
		}
		Dictionary dictionary = (Dictionary)damagePointDictionary[damagePointName];
		if (dictionary.ContainsKey("Config"))
		{
			CharacterDamagePointConfig characterDamagePointConfig = (CharacterDamagePointConfig)(GodotObject)dictionary["Config"];
			sprite.SetFlitersRecursive((Godot.Collections.Array)dictionary["Close"], open: false);
			sprite.SetFlitersRecursive((Godot.Collections.Array)dictionary["Open"], open: true);
			if (!characterDamagePointConfig.replaceMediaName.IsEmpty)
			{
				sprite.SetAtlasReplace(characterDamagePointConfig.replaceMediaName, characterDamagePointConfig.replaceMediaTexturePath);
			}
		}
	}

	public void CreateEffect(AdobeAnimateSprite sprite, string damagePointName, Vector2I gridPos = default(Vector2I))
	{
		if (!damagePointDictionary.ContainsKey(damagePointName))
		{
			return;
		}
		Dictionary dictionary = (Dictionary)damagePointDictionary[damagePointName];
		if (dictionary.ContainsKey("Config"))
		{
			CharacterDamagePointConfig characterDamagePointConfig = (CharacterDamagePointConfig)(GodotObject)dictionary["Config"];
			if (characterDamagePointConfig.animeEffect != null)
			{
				TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(characterDamagePointConfig.animeEffect, gridPos);
				TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, Node.InternalMode.Disabled);
				towerDefenseEffectParticlesOnce.GlobalPosition = ResolveLogicalSpritePosition(sprite) + characterDamagePointConfig.animeEffectOffset;
			}
		}
	}

	private static Vector2 ResolveLogicalSpritePosition(AdobeAnimateSprite sprite)
	{
		Node node = sprite;
		while (GodotObject.IsInstanceValid(node))
		{
			if (node is TowerDefenseCharacter towerDefenseCharacter)
			{
				return towerDefenseCharacter.GetLogicalGlobalPosition(sprite);
			}
			node = node.GetParent();
		}
		if (!GodotObject.IsInstanceValid(sprite))
		{
			return Vector2.Zero;
		}
		return sprite.GlobalPosition;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SplitFliter, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearDamagePointFliters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearDamagePointAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetDamagePointFliters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveLogicalSpritePosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.SplitFliter && args.Count == 1)
		{
			Array<string> array = SplitFliter(VariantUtils.ConvertTo<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.ClearDamagePointFliters && args.Count == 1)
		{
			ClearDamagePointFliters(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearDamagePointAll && args.Count == 1)
		{
			ClearDamagePointAll(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDamagePointFliters && args.Count == 2)
		{
			SetDamagePointFliters(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateEffect && args.Count == 3)
		{
			CreateEffect(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveLogicalSpritePosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ResolveLogicalSpritePosition(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResolveLogicalSpritePosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ResolveLogicalSpritePosition(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.SplitFliter)
		{
			return true;
		}
		if (method == MethodName.ClearDamagePointFliters)
		{
			return true;
		}
		if (method == MethodName.ClearDamagePointAll)
		{
			return true;
		}
		if (method == MethodName.SetDamagePointFliters)
		{
			return true;
		}
		if (method == MethodName.CreateEffect)
		{
			return true;
		}
		if (method == MethodName.ResolveLogicalSpritePosition)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.damagePointList)
		{
			damagePointList = VariantUtils.ConvertToArray<CharacterDamagePointConfig>(in value);
			return true;
		}
		if (name == PropertyName._damagePointList)
		{
			_damagePointList = VariantUtils.ConvertToArray<CharacterDamagePointConfig>(in value);
			return true;
		}
		if (name == PropertyName.damagePointDictionary)
		{
			damagePointDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.fliterOpenAll)
		{
			fliterOpenAll = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.fliterCloseAll)
		{
			fliterCloseAll = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.damagePointList)
		{
			value = VariantUtils.CreateFromArray(damagePointList);
			return true;
		}
		if (name == PropertyName._damagePointList)
		{
			value = VariantUtils.CreateFromArray(_damagePointList);
			return true;
		}
		if (name == PropertyName.damagePointDictionary)
		{
			value = VariantUtils.CreateFrom(in damagePointDictionary);
			return true;
		}
		if (name == PropertyName.fliterOpenAll)
		{
			value = VariantUtils.CreateFromArray(fliterOpenAll);
			return true;
		}
		if (name == PropertyName.fliterCloseAll)
		{
			value = VariantUtils.CreateFromArray(fliterCloseAll);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._damagePointList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.damagePointList, PropertyHint.TypeString, "24/17:CharacterDamagePointConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.damagePointDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.fliterOpenAll, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.fliterCloseAll, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.damagePointList, Variant.CreateFrom(damagePointList));
		info.AddProperty(PropertyName._damagePointList, Variant.CreateFrom(_damagePointList));
		info.AddProperty(PropertyName.damagePointDictionary, Variant.From(in damagePointDictionary));
		info.AddProperty(PropertyName.fliterOpenAll, Variant.CreateFrom(fliterOpenAll));
		info.AddProperty(PropertyName.fliterCloseAll, Variant.CreateFrom(fliterCloseAll));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.damagePointList, out var value))
		{
			damagePointList = value.AsGodotArray<CharacterDamagePointConfig>();
		}
		if (info.TryGetProperty(PropertyName._damagePointList, out var value2))
		{
			_damagePointList = value2.AsGodotArray<CharacterDamagePointConfig>();
		}
		if (info.TryGetProperty(PropertyName.damagePointDictionary, out var value3))
		{
			damagePointDictionary = value3.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.fliterOpenAll, out var value4))
		{
			fliterOpenAll = value4.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.fliterCloseAll, out var value5))
		{
			fliterCloseAll = value5.AsGodotArray<string>();
		}
	}
}
