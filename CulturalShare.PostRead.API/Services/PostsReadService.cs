using CulturalShare.PostRead.Services.Services.Base;
using Grpc.Core;
using PostsReadProto;
using System.Data;

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
        throw new NotImplementedException();
    }

    public override Task<PostsList> GetPosts(GetPostsRequest request, ServerCallContext context)
    {
        throw new RowNotInTableException("There is no data");
    }
}
