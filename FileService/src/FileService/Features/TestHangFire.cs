using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;
using FileService.Jobs;
using Hangfire;

namespace FileService.Features;

public static class TestHangFire
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("hangfire-test ", Handler);
        }
    }

    private static async Task<IResult> Handler(
        CancellationToken cancellationToken)
    {
        var jobId = BackgroundJob.Schedule<ConsistencyConfirmJob>(j =>
            j.Execute(Guid.NewGuid(), "key"), TimeSpan.FromSeconds(5));


        return Results.Ok(jobId);
    }
}