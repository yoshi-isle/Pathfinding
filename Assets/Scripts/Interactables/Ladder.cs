public class Ladder : Interactable
{
    public override void Interact()
    {
        print($"Interacting with {InteractName} ({InteractDescription})");
    }
}