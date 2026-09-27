public sealed class CharacterComponentProbeRuntime : CharacterComponentRuntime
{
	public string ActiveStateId => StateMachine?.CurrentStateHandle?.StableId ?? string.Empty;
}
