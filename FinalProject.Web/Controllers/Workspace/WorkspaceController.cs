using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.AddFavoriteWorkspace;
using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.DeleteFavoriteWorkspaceById;
using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.GetFavoriteWorkspacesForAuthUser;
using FinalProject.Services.DTOs.Requests.Workspace.GetAllWorkspaces;
using FinalProject.Services.Interfaces.FavoriteWorkspace;
using FinalProject.Services.Interfaces.Workspace;
using FinalProject.Web.Controllers.Base;
using FinalProject.Web.Models.ViewModels.Error;
using FinalProject.Web.Models.ViewModels.Workspace;
using FinalProject.Web.Models.ViewModels.Workspace.Index;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Web.Controllers.Workspace
{
    [Route("[controller]")]
    public class WorkspaceController : BaseController
    {
        private readonly IWorkspaceService _workspaceService;
        private readonly IFavoriteWorkspaceService _favoriteWorkspaceService;

        public WorkspaceController(IWorkspaceService workspaceService, IFavoriteWorkspaceService favoriteWorkspaceService)
        {
            _workspaceService = workspaceService;
            _favoriteWorkspaceService = favoriteWorkspaceService;
        }

        public async Task<IActionResult> Index()
        {
            if (GetUserId() == 0)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var workspacesRequest = new GetAllWorkspacesRequest()
            {
                User = GetUserSessionData()
            };

            var workspacesResponse = await _workspaceService.GetAllWorkspacesAsync(workspacesRequest);

            if (workspacesResponse.Status == false)
            {
                return View("Error", new ErrorViewModel() { RequestId = workspacesResponse.Message });
            }

            var favoriteWorkspacesRequest = new GetFavoriteWorkspacesForAuthUserRequest()
            {
                User = GetUserSessionData()
            };

            var favoriteWorkspacesResponse = await _favoriteWorkspaceService.GetFavoriteWorkspacesForAuthUserAsync(favoriteWorkspacesRequest);

            if (favoriteWorkspacesResponse.Status == false)
            {
                return View("Error", new ErrorViewModel() { RequestId = favoriteWorkspacesResponse.Message });
            }

            var workspaces = workspacesResponse.Data.ToList().Select(workspace => new WorkspaceViewModel()
            {
                Id = workspace.Id,
                Name = workspace.Name,
                Floor = workspace.Floor,
                Zone = workspace.Zone,
                HasMonitor = workspace.HasMonitor,
                HasDockingStation = workspace.HasDockingStation,
                IsNearWindow = workspace.IsNearWindow,
                IsNearPrinter = workspace.IsNearPrinter,
                ReservedDates = workspace.ReservedDates
            }).ToList();

            var favoriteWorkspaces = favoriteWorkspacesResponse.Data.ToList().Select(favoriteWorkspace => new SimpleFavoriteWorkspaceViewModel()
            {
                Id = favoriteWorkspace.Id,
                WorkspaceId = favoriteWorkspace.WorkspaceId,
                UserId = favoriteWorkspace.UserId
            }).ToList();

            var indexWorkspaceViewModel = new IndexWorkspaceViewModel()
            {
                Workspaces = workspaces,
                FavoriteWorkspaces = favoriteWorkspaces
            };

            System.Console.WriteLine("Favorite Workspace: ");
            foreach (var favoriteWorkspace in favoriteWorkspaces)
            {
                System.Console.WriteLine(favoriteWorkspace.WorkspaceId);
            }

            return View(indexWorkspaceViewModel);
        }

        [HttpPost]
        [Route("toggle-favorite")]
        public async Task<IActionResult> ToggleFavorite(int workspaceId, bool isFavorite, int? favoriteWorkspaceId)
        {
            if (GetUserId() == 0)
            {
                return RedirectToAction("Login", "Authentication");
            }

            System.Console.WriteLine("Workspace ID: " + workspaceId);
            System.Console.WriteLine("Is Favorite: " + isFavorite);
            System.Console.WriteLine("Favorite Workspace ID: " + favoriteWorkspaceId);

            if (!isFavorite)
            {
                var favoriteWorkspaceRequest = new AddFavoriteWorkspaceRequest()
                {
                    User = GetUserSessionData(),
                    Data = new AddFavoriteWorkspaceDto()
                    {
                        WorkspaceId = workspaceId
                    }
                };

                var favoriteWorkspaceResponse = await _favoriteWorkspaceService.AddFavoriteWorkspaceAsync(favoriteWorkspaceRequest);

                if (favoriteWorkspaceResponse.Status == false)
                {
                    return View("Error", new ErrorViewModel() { RequestId = favoriteWorkspaceResponse.Message });
                }
            }
            else
            {
                if (favoriteWorkspaceId == null)
                {
                    return View("Error", new ErrorViewModel() { RequestId = "Favorite workspace not found" });
                }

                var favoriteWorkspaceRequest = new DeleteFavoriteWorkspaceByIdRequest()
                {
                    User = GetUserSessionData(),
                    Data = new DeleteFavoriteWorkspaceByIdDto()
                    {
                        Id = favoriteWorkspaceId.Value
                    }
                };

                var favoriteWorkspaceResponse = await _favoriteWorkspaceService.DeleteFavoriteWorkspaceByIdAsync(favoriteWorkspaceRequest);

                if (favoriteWorkspaceResponse.Status == false)
                {
                    return View("Error", new ErrorViewModel() { RequestId = favoriteWorkspaceResponse.Message });
                }
            }

            return RedirectToAction("Index", "Workspace");
        }
    }
}