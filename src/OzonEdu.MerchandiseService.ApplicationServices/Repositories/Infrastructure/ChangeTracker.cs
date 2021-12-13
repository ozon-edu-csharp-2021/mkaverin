using OzonEdu.MerchandiseService.ApplicationServices.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseService.Domain.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.ApplicationServices.Repositories.Infrastructure
{
    public class ChangeTracker : IChangeTracker
    {
        public IDictionary<int, Entity> TrackedEntities => _usedEntitiesBackingField;

        private readonly ConcurrentDictionary<int, Entity> _usedEntitiesBackingField;

        public ChangeTracker()
        {
            _usedEntitiesBackingField = new ConcurrentDictionary<int, Entity>();
        }

        public void Track(Entity entity)
        {
            _usedEntitiesBackingField.TryAdd(entity.GetHashCode(), entity);
        }
    }
}