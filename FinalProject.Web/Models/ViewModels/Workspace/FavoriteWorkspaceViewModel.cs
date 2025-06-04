using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Web.Models.ViewModels.Workspace
{
    public class FavoriteWorkspaceViewModel
    {
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public WorkspaceViewModel Workspace { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}