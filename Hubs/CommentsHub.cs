using Microsoft.AspNetCore.SignalR;


namespace finalproject.Hubs
{
    public class CommentsHub : Hub
    {
        public async Task SendComment(string templateId, string userName, string commentText)
        {
            await Clients.Group(templateId).SendAsync("ReceiveComment", userName, commentText);
        }

        public async Task JoinTemplateGroup(string templateId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, templateId);
        }
    }
}
