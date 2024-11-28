namespace PetProject.Domain.Interfaces;

public interface ISoftDeletable
{
    public void Activate();

    public void Deactivate();
}