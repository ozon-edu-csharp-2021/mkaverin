using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate;
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
        /// Получить все заявки в Статусе InQueue
        /// </summary>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Список заявок</returns>
        Task<List<Order>> GetAllOrderInStatusInQueueAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить все заявки по id Сотрудника
        /// </summary>
        /// <param name="employeeId">Id Сотрудника</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Список заявок</returns>
        Task<List<Order>> GetAllOrderByEmployeeIdAsync(EmployeeId employeeId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Получить заявку по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор заявки</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Объект заявки</returns>
        Task<Order> FindByIdAsync(int id,
            CancellationToken cancellationToken = default);
    }
}
