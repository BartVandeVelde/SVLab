using SVLab.Shared.Entities;
using System.Collections.Generic;

namespace SVLab.UI.Infrastructure.Interfaces
{
    public interface IEntityService
    {
        IList<Project> GetRootProjects();
    }
}