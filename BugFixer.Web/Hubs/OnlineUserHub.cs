using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.Admin.OnlineUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BugFixer.Web.Hubs
{
    [Authorize]
    public class OnlineUserHub : Hub
    {
        #region constractor

        private readonly IUserService _userService;

        public OnlineUserHub(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        private static readonly Dictionary<long, OnlineUsersViewModel> OnlineUsersList = new Dictionary<long, OnlineUsersViewModel>();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.GetUserId();

            if (userId == null) return;

            if (OnlineUsersList.ContainsKey(userId.Value)) return;

            var user = await _userService.GetUserById(userId.Value);
            if (user == null) return;

            var onlineUserViewModel = new OnlineUsersViewModel
            {
                ConnectedDate = $"{DateTime.Now.ToShamsiDate()} - {DateTime.Now:HH:mm:ss}",
                DisplayName = user.GetUserDisplayName(),
                UserId = userId.Value.ToString()
            };

            OnlineUsersList.Add(userId.Value, onlineUserViewModel);

            await Clients.All.SendAsync("NewUserConnected", onlineUserViewModel);


            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.GetUserId();

            if (userId == null) return;

            if (!OnlineUsersList.ContainsKey(userId.Value)) return;

            OnlineUsersList.Remove(userId.Value);

            await Clients.All.SendAsync("NewUserDisConnected", userId.Value.ToString());

            await base.OnDisconnectedAsync(exception);
        }
    }
}
