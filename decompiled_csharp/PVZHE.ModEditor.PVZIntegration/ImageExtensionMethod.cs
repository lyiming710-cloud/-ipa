using System.ComponentModel;
using Godot.Bridge;
using PVZHE.ModEditor.FileSystem;

namespace PVZHE.ModEditor.PVZIntegration;

public class ImageExtensionMethod : XWFileSystemExtensionImageMethod
{
	public new class MethodName : XWFileSystemExtensionImageMethod.MethodName
	{
	}

	public new class PropertyName : XWFileSystemExtensionImageMethod.PropertyName
	{
	}

	public new class SignalName : XWFileSystemExtensionImageMethod.SignalName
	{
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
