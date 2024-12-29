using PetProject.Discussions.Application.DiscussionsManagement.Commands.CloseDiscussion;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.DeleteMessage;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.SendMessage;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.UpdateMessage;
using PetProject.Discussions.Contracts.Requests;

namespace PetProject.Discussions.Application.Mappers;

public static class RequestMappers
{
    public static CloseDiscussionCommand ToCommand(this CloseDiscussionRequest request)
    {
        return new CloseDiscussionCommand
        {
            DiscussionId = request.DiscussionId,
            UserId = request.UserId
        };
    }

    public static SendMessageCommand ToCommand(this SendMessageRequest request)
    {
        return new SendMessageCommand
        {
            DiscussionId = request.DiscussionId,
            Text = request.Text,
            UserId = request.UserId
        };
    }
    
    public static UpdateMessageCommand ToCommand(this UpdateMessageRequest request)
    {
        return new UpdateMessageCommand
        {
            DiscussionId = request.DiscussionId,
            MessageId = request.MessageId,
            Text = request.Text,
            UserId = request.UserId
        };
    }
    
    public static DeleteMessageCommand ToCommand(this DeleteMessageRequest request)
    {
        return new DeleteMessageCommand
        {
            DiscussionId = request.DiscussionId,
            MessageId = request.MessageId,
            UserId = request.UserId
        };
    }
}