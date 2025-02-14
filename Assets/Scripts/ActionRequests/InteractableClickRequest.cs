public class InteractableClickRequest : Request
{
    public Interactable clickedInteractable;

    public InteractableClickRequest(Interactable clickedInteractable)
    {
        this.clickedInteractable = clickedInteractable;
    }
}