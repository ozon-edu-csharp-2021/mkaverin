using OzonEdu.MerchandiseService.Domain.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    /// <summary>
    /// Репозиторий для управления сущностью <see cref="MerchPack"/>
    /// </summary>
    public interface IMerchPackRepository : IRepository<MerchPack>
    {
        /// <summary>
        /// Получить MerchPack по типу MerchPack
        /// </summary>
        /// <param name="merchType">Тип пакета мерча</param>
        /// <param name="cancellationToken">Токен для отмены операции. <see cref="CancellationToken"/></param>
        /// <returns>Список заявок</returns>
        Task<MerchPack> FindByTypeAsync(MerchType merchType,
            CancellationToken cancellationToken = default);
    }
}
