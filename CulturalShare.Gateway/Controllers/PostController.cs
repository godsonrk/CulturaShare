using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using PostsReadProto;
using PostsWriteProto;

namespace CulturalShare.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly PostsRead.PostsReadClient _postReadClient;
    private readonly PostsWrite.PostsWriteClient _postWriteClient;

    public PostController(PostsRead.PostsReadClient postsClient, PostsWrite.PostsWriteClient postWriteClient)
    {
        _postReadClient = postsClient;
        _postWriteClient = postWriteClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetPostsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetPostsRequest()
            {
                UserId = 1,
            };
            var result = await _postReadClient.GetPostsAsync(request, cancellationToken: cancellationToken);

            return Ok(result);
        }
        catch (RpcException)
        {
            throw;
        }
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetPostByIdAsync([FromRoute] int Id, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetPostByIdRequest()
            {
                UserId = 1,
                Id = Id
            };
            var result = await _postReadClient.GetPostByIdAsync(request, cancellationToken: cancellationToken);

            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePostAsync(CreatePostRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _postWriteClient.CreatePostAsync(request, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePostAsync(UpdatePostRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _postWriteClient.UpdatePostAsync(request, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeletePostAsync([FromRoute] int Id, CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeletePostRequest()
            {
                UserId = 1,
                Id = Id
            };

            var result = await _postWriteClient.DeletePostAsync(request, cancellationToken: cancellationToken);
            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
