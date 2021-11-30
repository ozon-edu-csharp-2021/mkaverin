using Dapper;
using Npgsql;
using OzonEdu.MerchandiseService.ApplicationServices.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Repositories.Implementation
{
    public class MerchPackRepository : IMerchPackRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private const int Timeout = 5;

        public MerchPackRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }
        public async Task<MerchPack> FindByTypeAsync(MerchType merchType, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT id,merch_type_id,merch_items
                FROM merch_pack
                WHERE merch_type_id = @MerchTypeId;";

            var parameters = new
            {
                MerchTypeId = merchType.Id,
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var merchPack = await connection.QueryAsync<MerchPack>(commandDefinition);

            var stockItem = merchPack.AsList()[0];
            _changeTracker.Track(stockItem);
            return stockItem;
        }
    }
}
