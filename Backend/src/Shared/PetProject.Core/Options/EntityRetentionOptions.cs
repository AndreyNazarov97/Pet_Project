namespace PetProject.Core.Options;

public class EntityRetentionOptions
{
    public const string EntityRetention = nameof(EntityRetention);
    
    public int Days { get; set; }
}