using System;
using System.Collections.Generic;

using Ncqrs.Domain.Storage;

namespace Ncqrs.Domain
{
    /// <summary>
    ///   A context from within domain object can be changed.
    ///   <example>
    ///     using (var work = new UnitOfWork(repository))
    ///     {
    ///     // Create the new customer.
    ///     Customer newCustomer = new Customer();
    ///     newCustomer.Name = "Pieter Joost van de Sande";
    ///     
    ///     // Accept the work that has been done in the context.
    ///     work.Accept();
    ///     }
    ///   </example>
    /// </summary>
    internal sealed class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        ///   The <see cref = "UnitOfWork" /> that is associated with the current thread.
        /// </summary>
        [ThreadStatic] private static UnitOfWork _threadInstance;

        /// <summary>
        ///   A queue that holds a reference to all instances that have themself registered as a dirty instance during the lifespan of this unit of work instance.
        /// </summary>
        private readonly Queue<AggregateRoot> _dirtyInstances;

        /// <summary>
        ///   A reference to the repository that is asociated with this instance.
        /// </summary>
        private readonly IDomainRepository _repository;

        /// <summary>
        ///   Gets the <see cref = "UnitOfWork" /> associated with the current thread context.
        /// </summary>
        /// <value>The current.</value>
        public static UnitOfWork Current
        {
            get { return _threadInstance; }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        ///   Gets the domain repository.
        /// </summary>
        /// <value>The domain repository.</value>
        public IDomainRepository Repository
        {
            get { return _repository; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "UnitOfWork" /> class.
        /// </summary>
        /// <param name = "domainRepository">The domain repository to use in this unit of work.</param>
        public UnitOfWork(IDomainRepository domainRepository)
        {
            _repository = domainRepository;
            _dirtyInstances = new Queue<AggregateRoot>();
            _threadInstance = this;
            IsDisposed = false;
        }

        /// <summary>
        ///   Releases unmanaged resources and performs other cleanup operations before the
        ///   <see cref = "UnitOfWork" /> is reclaimed by garbage collection.
        /// </summary>
        ~UnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name = "disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    _threadInstance = null;
                }

                IsDisposed = true;
            }
        }

        /// <summary>
        ///   Registers the dirty.
        /// </summary>
        /// <param name = "dirtyInstance">The dirty instance.</param>
        internal void RegisterDirtyInstance(AggregateRoot dirtyInstance)
        {
            if (!_dirtyInstances.Contains(dirtyInstance))
            {
                _dirtyInstances.Enqueue(dirtyInstance);
            }
        }

        /// <summary>
        ///   Accepts the unit of work and persist the changes.
        /// </summary>
        public void Accept()
        {
            while (_dirtyInstances.Count > 0)
            {
                var dirtyInstance = _dirtyInstances.Dequeue();

                _repository.Add(dirtyInstance);
            }
        }
    }
}