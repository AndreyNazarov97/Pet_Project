using PetProject.Domain.Consts;
using PetProject.Domain.Results;
using PetProject.Domain.Results.Errors;

namespace PetProject.Domain.Entities.ValueObjects;

public record FullName
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? Patronymic { get; private set; }

    private FullName() { }
    private FullName(string firstName, string lastName, string? patronymic)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
    }

    public static Result<FullName> Create(string firstName, string lastName, string? patronymic)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result<FullName>.Failure(FullNameErrors.FirstNameRequired);
        if (firstName.Length > FullNameConsts.FirstNameMaxLength)
            return Result<FullName>.Failure(FullNameErrors.FirstNameTooLong);
        if (firstName.Length < FullNameConsts.FirstNameMinLength)
            return Result<FullName>.Failure(FullNameErrors.FirstNameTooShort);
        if (string.IsNullOrWhiteSpace(lastName))
            return Result<FullName>.Failure(FullNameErrors.LastNameRequired);
        if (lastName.Length > FullNameConsts.LastNameMaxLength)
            return Result<FullName>.Failure(FullNameErrors.LastNameTooLong);
        if (lastName.Length < FullNameConsts.LastNameMinLength)
            return Result<FullName>.Failure(FullNameErrors.LastNameTooShort);
        if (patronymic != null && patronymic.Length > FullNameConsts.PatronymicMaxLength)
            return Result<FullName>.Failure(FullNameErrors.PatronymicTooLong);
        if (patronymic != null && patronymic.Length < FullNameConsts.PatronymicMinLength)
            return Result<FullName>.Failure(FullNameErrors.PatronymicTooShort);

        return new FullName(firstName, lastName, patronymic);
    }
}