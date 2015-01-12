namespace SVLab.UI.Infrastructure.Interfaces
{
    public interface IView
    {
        string Caption
        {
            get;
        }

        string RegionName
        {
            get;
        }

        string ViewName
        {
            get;
        }

        object DataContext
        {
            get;
        }
    }
}
