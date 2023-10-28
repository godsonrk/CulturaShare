using CulturalShare.PostRead.Services.Services.Base;
using Grpc.Core;
using PostsReadProto;

namespace CulturalShare.PostRead.API.Services;

public class PostsReadService : PostsRead.PostsReadBase
{
    private readonly IPostService _postService;
    public PostsReadService(IPostService postService)
    {
        _postService = postService;
    }

    public override Task<PostReply> GetPostById(GetPostByIdRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.Unavailable, "Not implemented"));
    }

    public override Task<PostsList> GetPosts(GetPostsRequest request, ServerCallContext context)
    {
        throw new RpcException(new Status(StatusCode.Unavailable, "Not implemented"));
    }
}
