using OzonEdu.MerchandiseService.Domain.Contracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    /// <summary>
    /// Репозиторий для управления сущностью <see cref="Order"/>
    /// </summary>
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// Получить все заявки в определенном статусе
        /// </summary>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Список заявок</returns>
        Task<List<Order>> GetAllOrderInStatusAsync(Status status,
            CancellationToken cancellationToken);

        /// <summary>
        /// Получить все заявки по id Сотрудника 
        /// </summary>
        /// <param name="employeeId">Id Сотрудника</param>
        /// <param name="status">Статус заказа</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Список заявок</returns>
        Task<List<Order>> GetAllOrderByEmployeeIdAsync(long employeeId,
            CancellationToken cancellationToken);

        /// <summary>
        /// Получить заявку по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор заявки</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Объект заявки</returns>
        Task<Order> FindByIdAsync(long id,
            CancellationToken cancellationToken);
        Task<long> CreateAsync(Order order,
            CancellationToken cancellationToken);
        Task<Order> UpdateAsync(Order order,
            CancellationToken cancellationToken); 
    }
}
