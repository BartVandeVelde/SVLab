using SVLab.Shared.Entities;
using SVLab.UI.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProjectBrowser.Services
{
    public class EntityService : IEntityService
    {
        public IList<Project> GetRootProjects()
        {
            IList<Project> projects = new List<Project>();

            Project p2013 = new Project() { Name = "2013" };
            Project p2013_DCB = new Project() { Name = "DCB" };
            Project p2013_PandG = new Project() { Name = "P&G" };
            Project p2013_DCB_Oudenaarde = new Project() { Name = "Oudenaarde" };
            Project p2013_DCB_Waregem = new Project() { Name = "Waregem" };
            Campaign p2013_DCB_Waregem_ContactgeluidChape = new Campaign() { Name = "Contactgeluid (chape)" };
            Campaign p2013_DCB_Waregem_ContactgeluidVloer = new Campaign() { Name = "Contactgeluid (vloer)" };
            Campaign p2013_DCB_Waregem_Luchtgeluid = new Campaign() { Name = "Luchtgeluid" };
            Measurement p2013_DCB_Waregem_Contactgeluid_Meting1 = new Measurement() { Name = "Meting 1" };
            MeasurementFile p2013_DCB_Waregem_Contactgeluid_Meting1_File1 = new MeasurementFile() { Path = "C:\\test.svn" };

            p2013_DCB_Waregem_Contactgeluid_Meting1.SubMeasurementFiles = new Collection<MeasurementFile>();
            p2013_DCB_Waregem_Contactgeluid_Meting1.SubMeasurementFiles.Add(p2013_DCB_Waregem_Contactgeluid_Meting1_File1);

            p2013_DCB_Waregem_ContactgeluidChape.SubMeasurements = new Collection<Measurement>();
            p2013_DCB_Waregem_ContactgeluidChape.SubMeasurements.Add(p2013_DCB_Waregem_Contactgeluid_Meting1);

            p2013_DCB_Waregem.SubCampaigns = new Collection<Campaign>();
            p2013_DCB_Waregem.SubCampaigns.Add(p2013_DCB_Waregem_ContactgeluidChape);
            p2013_DCB_Waregem.SubCampaigns.Add(p2013_DCB_Waregem_ContactgeluidVloer);
            p2013_DCB_Waregem.SubCampaigns.Add(p2013_DCB_Waregem_Luchtgeluid);

            p2013_DCB.SubProjects = new Collection<Project>();
            p2013_DCB.SubProjects.Add(p2013_DCB_Oudenaarde);
            p2013_DCB.SubProjects.Add(p2013_DCB_Waregem);

            p2013.SubProjects = new Collection<Project>();
            p2013.SubProjects.Add(p2013_DCB);
            p2013.SubProjects.Add(p2013_PandG);

            Project p2014 = new Project() { Name = "2014" };
            Project p2014_Dokter_Dheere = new Project() { Name = "Dokter Dheere" };

            p2014.SubProjects = new Collection<Project>();
            p2014.SubProjects.Add(p2014_Dokter_Dheere);
            
            Project p2015 = new Project() { Name = "2015" };

            projects.Add(p2013);
            projects.Add(p2014);
            projects.Add(p2015);

            return projects;
        }
    }
}