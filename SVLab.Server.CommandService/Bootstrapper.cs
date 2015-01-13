using Microsoft.Practices.Unity;
using Ncqrs.Bus;
using Ncqrs.Bus.NServiceBus;
using Ncqrs.CommandHandling.AutoMapping;
using Ncqrs.CommandHandling;
using Ncqrs.CommandHandling.Dispatching;
using Ncqrs.Domain;
using Ncqrs.Domain.Bus;
using Ncqrs.EventSourcing.EntityFramework;
using Ncqrs.EventSourcing.Storage;
using Ncqrs.Infrastructure;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SVLab.Server.CommandService
{
    public class Bootstrapper
    {
        public void Start()
        {
            SetupDependencies();

            InitializeNServiceBus();
        }

        private static void SetupDependencies()
        {
            var container = ServiceLocator.Current;

            var dispatcher = new InProcessCommandHandlerDispatcher();

            //dispatcher.RegisterExecutorForAllCommandsIn(Assembly.GetAssembly(typeof(AddNewRecipeCommand)), new AutoMappingCommandExecutor());

            container.RegisterType<IUniqueIdentifierGenerator, BasicGuidGenerator>(new ContainerControlledLifetimeManager());
            container.RegisterInstance<ICommandHandler>(dispatcher, new ContainerControlledLifetimeManager());
            container.RegisterInstance<IPropertyBagConverter>(new PropertyBagConverter());
            container.RegisterType<IEventStore, EntityFrameworkEventStore>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEventBus, DistributedEventBus>();
            container.RegisterType<IUnitOfWorkFactory, ThreadBasedUnitOfWorkFactory>();
        }

        private static void InitializeNServiceBus()
        {
            BusConfiguration configuration = new BusConfiguration();

            configuration.PurgeOnStartup(false);
            configuration.RijndaelEncryptionService();
            configuration.UseTransport<MsmqTransport>();
            configuration.UseSerialization<XmlSerializer>();

            // For production use, please select a durable persistence.
            // To use RavenDB, install-package NServiceBus.RavenDB and then use configuration.UsePersistence<RavenDBPersistence>();
            // To use SQLServer, install-package NServiceBus.NHibernate and then use configuration.UsePersistence<NHibernatePersistence>();
            if (Debugger.IsAttached)
            {
                configuration.UsePersistence<InMemoryPersistence>();
            }

            configuration.Conventions()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.StartsWith("SVLab.Server.CommandService") && t.Namespace.EndsWith("Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("SVLab.Server.CommandService") && t.Namespace.EndsWith("Events"));
            //.DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("VideoStore") && t.Namespace.EndsWith("RequestResponse"))
            //.DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));

            // In Production, make sure the necessary queues for this endpoint are installed before running the website
            if (Debugger.IsAttached)
            {
                // While calling this code will create the necessary queues required, this step should
                // ideally be done just one time as opposed to every execution of this endpoint.
                configuration.EnableInstallers();
            }

            IBus bus = Bus.Create(configuration).Start();

            ServiceLocator.Current.RegisterInstance(new DistributedEventBus(bus));
        }
    }
}