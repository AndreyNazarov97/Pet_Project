using PetProject.SharedKernel.Shared;

namespace PetProject.SharedKernel.Exceptions;

public class CanNotCreateRecordException : CustomException
{
    public CanNotCreateRecordException(Error error) : base(error) { }
}