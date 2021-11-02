using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderMerchAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchandiseRequestAggregate
{
    /// <summary>
    /// Репозиторий для управления сущностью <see cref="MerchandiseRequest"/>
    /// </summary>
    public interface IMerchandiseRequestRepository : IRepository<MerchandiseRequest>
    {
        /// <summary>
        /// Получить все заявки в Статусе InQueue
        /// </summary>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Список заявок</returns>
        Task<List<MerchandiseRequest>> GetAllMerchandiseRequestInStatusInQueueAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить все заявки по id Сотрудника
        /// </summary>
        /// <param name="employeeId">Id Сотрудника</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Список заявок</returns>
        Task<List<MerchandiseRequest>> GetAllMerchandiseRequestByEmployeeIdAsync(EmployeeId employeeId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Получить заявку по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор заявки</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Объект заявки</returns>
        Task<MerchandiseRequest> FindByIdAsync(int id,
            CancellationToken cancellationToken = default);
    }
}
