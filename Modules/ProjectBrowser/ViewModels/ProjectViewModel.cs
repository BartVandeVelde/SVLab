using SVLab.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectBrowser.ViewModels
{
    public class ProjectViewModel : TreeViewItemViewModel 
    {
        readonly Project project;

        /// <summary>
        /// Builds a <see cref="ProjectViewModel"/> without parent based on the data from a <see cref="Project"/>
        /// </summary>
        /// <param name="project">Entity which holds the data for this viewmodel.</param>
        public ProjectViewModel(Project project) 
            : base(null, true)
        {
            this.project = project;
        }

        /// <summary>
        /// Builds a <see cref="ProjectViewModel"/> with parent based on the data from a <see cref="Project"/>
        /// </summary>
        /// <param name="project">Entity which holds the data for this viewmodel.</param>
        /// <param name="parentProject">Entity which is the parent of the <see cref="Project"/>.</param>
        public ProjectViewModel(Project project, ProjectViewModel parentProject)
            : base(parentProject, true)
        {
            this.project = project;
        }

        /// <summary>
        /// Returns the name of the project
        /// </summary>
        public string Name
        {
            get { return this.project.Name; }
        }

        /// <summary>
        /// Loads all the children from the model into the viewmodel
        /// </summary>
        protected override void LoadChildren()
        {
            if (this.project.SubProjects != null)
            {
                foreach (Project p in this.project.SubProjects)
                {
                    base.Children.Add(new ProjectViewModel(p, this));
                }
            }

            if (this.project.SubCampaigns != null)
            {
                foreach (Campaign c in this.project.SubCampaigns)
                {
                    base.Children.Add(new CampaignViewModel(c, this));
                }
            }
        }
    }
}
