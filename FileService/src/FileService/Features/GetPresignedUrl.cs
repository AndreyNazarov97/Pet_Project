using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;

namespace FileService.Features;

public static class GetPresignedUrl
{
    private record GetPresignedUrlRequest(string BucketName, string Prefix);
    
   public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("files/{prefix}/{key}/presigned", Handler);
        }
    }

    private static async Task<IResult> Handler(
        string prefix,
        string key,
        IAmazonS3 s3Client,
        CancellationToken cancellationToken)
    {
        try
        {
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = "files",
                Key = $"{prefix}/{key}",
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddHours(24),
                Protocol = Protocol.HTTP
            };
 
            var url = await s3Client.GetPreSignedURLAsync(presignedRequest);
            
            return Results.Ok(new
            {
                key,
                url
            });
        }
        catch (AmazonS3Exception ex)
        {
            return Results.BadRequest($"S3 error generating presigned URL: {ex.Message}");
        }
    }
}