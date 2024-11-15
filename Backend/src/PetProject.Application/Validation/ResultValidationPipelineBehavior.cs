using System.Reflection;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.Validation
{
    /// <summary>
    /// Валидирует только TRequest с возвращаемым типом Result&lt;T, Error&gt;
    /// </summary>
    /// <typeparam name="TResponse">Должен быть <see cref="Result{T, Error}"/> (Result с типом T и Error из Domain.Shared)</typeparam>
    public class ResultValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ResultValidationPipelineBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Проверяем, что TResponse является обобщённым типом Result<,> с Error в качестве второго параметра
            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<,>) &&
                typeof(TResponse).GetGenericArguments()[1] == typeof(Error))
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var error = validationResult.Errors
                        .Select(x =>
                            Error.Deserialize(x.ErrorMessage))
                        .First();
                    
                    var successType = typeof(TResponse).GetGenericArguments()[0];
                  
                    var resultType =
                        typeof(Result<,>).MakeGenericType(successType, typeof(Error)); // Используем типы для T и E
                    var constructor = resultType.GetConstructor(
                        BindingFlags.NonPublic | BindingFlags.Instance, // Ищем нестандартные (не публичные) экземплярные конструкторы
                        null, // Не указываем модификатор типа
                        new[] { typeof(bool), typeof(Error), successType }, // Параметры конструктора
                        null)!
                        .Invoke(new object[] { true, error, null });// Вызываем конструктор

                    return (TResponse)constructor;
                }
            }
            
            return await next.Invoke();
        }
    }
}