using Grpc.Core;
using PostsReadProto;
using PostsWriteProto;

namespace CulturalShare.PostWrite.API.Services;

public class PostsWriteService : PostsWrite.PostsWriteBase
{
    public override Task<PostReply> CreatePost(CreatePostRequest request, ServerCallContext context)
    {
        return base.CreatePost(request, context); 
    }

    public override Task<DeletePostReply> DeletePost(DeletePostRequest request, ServerCallContext context)
    {
        return base.DeletePost(request, context);
    }

    public override Task<PostReply> UpdatePost(UpdatePostRequest request, ServerCallContext context)
    {
        return base.UpdatePost(request, context);
    }
}
