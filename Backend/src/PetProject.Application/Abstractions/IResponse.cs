namespace PetProject.Application.Abstractions;

public interface IResponse<T>
{
    T CreateResponse();
}