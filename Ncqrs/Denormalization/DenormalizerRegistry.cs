using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Ncqrs.Domain;

namespace Ncqrs.Denormalization
{
    public class DenormalizerRegistry
    {
        readonly Dictionary<Type, List<object>> denormalizers = new Dictionary<Type, List<object>>();

        /// <summary>
        /// Finds all classes implementing one or more versions of the <see cref="IDenormalizes{TEvent}"/> interfaces
        /// in a specific assembly and registers them using <see cref="RegisterDenormalizer{TEvent}"/>.
        /// </summary>
        public void RegisterDenormalizersFromAssembly(Assembly assembly)
        {
            foreach (var denormalizerType in FindDenormalizersInAssembly(assembly))
            {
                EnsureTypeHasDefaultConstructor(denormalizerType);

                object denormalizer = Activator.CreateInstance(denormalizerType);

                foreach (Type eventType in GetSupportedEvents(denormalizerType))
                {
                    RegisterDenormalizer(eventType, denormalizer);
                }  
            }
        }

        private static IEnumerable<Type> FindDenormalizersInAssembly(Assembly assembly)
        {
            return from t in assembly.GetTypes()
                   where t.GetInterfaces().Any(IsDenormalizer)
                   select t;
        }

        private static IEnumerable<Type> GetSupportedEvents(Type denormalizerType)
        {
            return denormalizerType.GetInterfaces().Where(IsDenormalizer).Select(t => t.GetGenericArguments().Single());
        }

        private static bool IsDenormalizer(Type interfaceType)
        {
            return interfaceType.IsGenericType && (interfaceType.GetGenericTypeDefinition() == typeof (IDenormalizes<>));
        }

        private static void EnsureTypeHasDefaultConstructor(Type denormalizerType)
        {
            var defaultCtor = denormalizerType.GetConstructor(Type.EmptyTypes);
            if (defaultCtor == null)
            {
                throw new InvalidOperationException("Denormalizer " + denormalizerType + " must have a default constructor");
            }
        }

        /// <summary>
        /// Registers an implementation of <see cref="IDenormalizes{TEvent}"/> as one of the denormalizers 
        /// for a specified event type.
        /// </summary>
        public void RegisterDenormalizer(Type eventType, object denormalizer)
        {
            if (!denormalizers.ContainsKey(eventType))
            {
                denormalizers[eventType] = new List<object>();
            }

            denormalizers[eventType].Add(denormalizer);
        }

        /// <summary>
        /// Publishes the event to all denormalizer classes that implemented the <see cref="IDenormalizes{TEvent}"/>
        /// interface and have been registered.
        /// </summary>
        public void PublishEvent(IEvent evt)
        {
            var eventType = evt.GetType();
            
            if (denormalizers.ContainsKey(eventType))
            {
                foreach (object untypedDenormalizer in denormalizers[eventType])
                {
                    InvokeDenormalizeEvent(untypedDenormalizer, evt);
                }
            }
        }

        private static void InvokeDenormalizeEvent(object untypedDenormalizer, IEvent evt)
        {
            Type typedDenormalizerInterface = typeof (IDenormalizes<>).MakeGenericType(evt.GetType());
            MethodInfo method = typedDenormalizerInterface.GetMethod("DenormalizeEvent");
                    
            method.Invoke(untypedDenormalizer, new object[] {evt});
        }
    }
}
