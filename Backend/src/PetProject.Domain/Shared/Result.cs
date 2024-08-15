﻿namespace PetProject.Domain.Result;

public class Result
{
    public Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null ||
            !isSuccess && error == null )
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    
    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);
    public static implicit operator Result(Error error) => new(false, error);
}

public class Result<TValue> : Result
{
    public Result(TValue value, bool isSuccess, Error? error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    private readonly TValue _value;

    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static Result<TValue> Success(TValue value) => new(value, true, null);
    public new static Result<TValue> Failure(Error error) => new(default!, false, error);
    
    public static implicit operator Result<TValue>(TValue value) => new(value, true, null);
    public static implicit operator Result<TValue>(Error error) => new(default!, false, error);
}