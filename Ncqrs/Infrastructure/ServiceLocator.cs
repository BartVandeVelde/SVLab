using Microsoft.Practices.Unity;

namespace Ncqrs.Infrastructure
{
    public class ServiceLocator
    {
        private static readonly IUnityContainer serviceLocator;

        static ServiceLocator()
        {
            serviceLocator = new UnityContainer();
        }

        public static IUnityContainer Current
        {
            get { return serviceLocator; }
        }
    }
}
