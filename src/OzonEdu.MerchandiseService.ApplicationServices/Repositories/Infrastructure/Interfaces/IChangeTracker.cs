using OzonEdu.MerchandiseService.Domain.Models;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.ApplicationServices.Repositories.Infrastructure.Interfaces
{
    /// <summary>
    /// Компонент, ответственный за хранение ссылок на сущности, которые были затронуты в рамках выполнения запроса.
    /// </summary>
    /// <remarks>
    /// Необходим для сбора доменных событий при сохранении.
    /// </remarks>
    public interface IChangeTracker
    {
        /// <summary>
        /// Коллекция всех сущностей, которые так или иначе были использованы в репозитории.
        /// </summary>
        IDictionary<int, Entity> TrackedEntities { get; }

        /// <summary>
        /// "Записать" сущность как подлежащую "использованию" в рамках выполнения запроса.
        /// </summary>
        /// <param name="entity"></param>
        public void Track(Entity entity);
    }
}