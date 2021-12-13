using CSharpCourse.Core.Lib.Enums;
using Dapper;
using Npgsql;
using OpenTracing;
using OzonEdu.MerchandiseService.ApplicationServices.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.ApplicationServices.Repositories.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private readonly ITracer _tracer;
        private const int Timeout = 5;
        public OrderRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory, ITracer tracer, IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _tracer = tracer;
            _changeTracker = changeTracker;
        }
        public async Task<long> CreateAsync(Order itemToCreate, CancellationToken cancellationToken = default)
        {
            using var span = _tracer.BuildSpan("OrderRepository.CreateAsync").StartActive();
            const string sql = @"
                INSERT INTO merchandise_order (creation_date, employee_email,employee_name,manager_email,manager_name,clothing_size, merch_pack_id,source_id,status_id)
                VALUES (@CreationDate, @EmployeeEmail, @EmployeeName,@ManagerEmail, @ManagerName, @ClothingSize, @MerchPackId, @SourceId, @StatusId) RETURNING id;";

            var parameters = new
            {
                CreationDate = itemToCreate.CreationDate.Value,
                EmployeeEmail = itemToCreate.EmployeeEmail.Value,
                EmployeeName = itemToCreate.EmployeeName.Value,
                ManagerEmail = itemToCreate.ManagerEmail.Value,
                ManagerName = itemToCreate.ManagerName.Value,
                ClothingSize = (int)itemToCreate.ClothingSize,
                MerchPackId = itemToCreate.MerchPack.Id,
                SourceId = itemToCreate.Source.Id,
                StatusId = itemToCreate.Status.Id
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var result = await connection.ExecuteScalarAsync(commandDefinition);

            if (result is null)
                throw new InvalidOperationException(nameof(result));
            _changeTracker.Track(itemToCreate);
            return (long)result;
        }
        public async Task<Order> FindByIdAsync(long id, CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan("OrderRepository.FindByIdAsync").StartActive();
            const string sql = @"
                SELECT  merchandise_order.id, merchandise_order.creation_date, 
                        merchandise_order.employee_email,merchandise_order.employee_name,merchandise_order.manager_email,merchandise_order.manager_name,
                        merchandise_order.clothing_size, merchandise_order.merch_pack_id, merchandise_order.source_id, 
                        merchandise_order.status_id, merchandise_order.delivery_date,merch_pack.id, merch_pack.merch_type_id, merch_pack.merch_items
                FROM merchandise_order
                INNER JOIN merch_pack on merch_pack.id = merchandise_order.merch_pack_id
                WHERE merchandise_order.id = @Id;";

            var parameters = new
            {
                Id = id,
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var merchPack = await connection.QueryAsync<Repositories.Models.Order, Repositories.Models.MerchPack, Order>(commandDefinition,
                (merchandiseOrder, merchPack) => new Order(
                    merchandiseOrder.id,
                    new(merchandiseOrder.creation_date),
                    Email.Create(merchandiseOrder.employee_email),
                    NameUser.Create(merchandiseOrder.employee_name),
                    Email.Create(merchandiseOrder.manager_email),
                    NameUser.Create(merchandiseOrder.manager_name),
                    (ClothingSize)merchandiseOrder.clothing_size,
                    new(merchPack.id, merchPack.merch_type_id, merchPack.merch_items),
                    new(merchandiseOrder.source_id),
                    new(merchandiseOrder.status_id),
                    DeliveryDate.Create(merchandiseOrder.delivery_date)
                ));
            var stockItem = merchPack.AsList()[0];
            return stockItem;
        }

        public async Task<List<Order>> GetAllOrderByEmployeeAsync(string employeeEmail, CancellationToken cancellationToken = default)
        {
            using var span = _tracer.BuildSpan("OrderRepository.GetAllOrderByEmployeeAsync").StartActive();
            string sql = @"
                SELECT  merchandise_order.id, merchandise_order.creation_date, 
                        merchandise_order.employee_email,merchandise_order.employee_name,merchandise_order.manager_email,merchandise_order.manager_name,
                        merchandise_order.clothing_size, merchandise_order.merch_pack_id, merchandise_order.source_id, 
                        merchandise_order.status_id, merchandise_order.delivery_date,merch_pack.id, merch_pack.merch_type_id, merch_pack.merch_items
                FROM merchandise_order
                INNER JOIN merch_pack on merch_pack.id = merchandise_order.merch_pack_id
                WHERE merchandise_order.employee_email = @EmployeeEmail;";

            var parameters = new
            {
                EmployeeEmail = employeeEmail,
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            var pack = await connection.QueryAsync<Repositories.Models.Order, Repositories.Models.MerchPack, Order>(commandDefinition,
                   (merchandiseOrder, merchPack) => new Order(
                       merchandiseOrder.id,
                       new(merchandiseOrder.creation_date),
                    Email.Create(merchandiseOrder.employee_email),
                    NameUser.Create(merchandiseOrder.employee_name),
                    Email.Create(merchandiseOrder.manager_email),
                    NameUser.Create(merchandiseOrder.manager_name),
                    (ClothingSize)merchandiseOrder.clothing_size,
                    new(merchPack.id, merchPack.merch_type_id, merchPack.merch_items),
                    new(merchandiseOrder.source_id),
                    new(merchandiseOrder.status_id),
                    DeliveryDate.Create(merchandiseOrder.delivery_date)

                 ));
            var stockItem = pack.AsList();
            return stockItem;
        }

        public async Task<List<Order>> GetAllOrderInStatusAsync(Status status, CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan("OrderRepository.GetAllOrderInStatusAsync").StartActive();
            string sql = @"
                SELECT  merchandise_order.id, merchandise_order.creation_date, 
                        merchandise_order.employee_email,merchandise_order.employee_name,merchandise_order.manager_email,merchandise_order.manager_name,
                        merchandise_order.clothing_size, merchandise_order.merch_pack_id, merchandise_order.source_id, 
                        merchandise_order.status_id, merchandise_order.delivery_date,merch_pack.id, merch_pack.merch_type_id, merch_pack.merch_items
                FROM merchandise_order
                INNER JOIN merch_pack on merch_pack.id = merchandise_order.merch_pack_id
                WHERE status_id = @Status
                ORDER BY  merchandise_order.source_id DESC, merchandise_order.creation_date;";

            var parameters = new
            {
                Status = status.Id,
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);

            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);

            var merchPack = await connection.QueryAsync<Repositories.Models.Order, Repositories.Models.MerchPack, Order>(commandDefinition,
               (merchandiseOrder, merchPack) => new Order(
                   merchandiseOrder.id,
                   new(merchandiseOrder.creation_date),
                Email.Create(merchandiseOrder.employee_email),
                NameUser.Create(merchandiseOrder.employee_name),
                Email.Create(merchandiseOrder.manager_email),
                NameUser.Create(merchandiseOrder.manager_name),
                (ClothingSize)merchandiseOrder.clothing_size,
                new(merchPack.id, merchPack.merch_type_id, merchPack.merch_items),
                new(merchandiseOrder.source_id),
                new(merchandiseOrder.status_id),
                DeliveryDate.Create(merchandiseOrder.delivery_date)

              ));

            var stockItem = merchPack.AsList();
            return stockItem;
        }

        public async Task<Order> UpdateAsync(Order itemToUpdate, CancellationToken cancellationToken)
        {
            using var span = _tracer.BuildSpan("OrderRepository.UpdateAsync").StartActive();
            const string sql = @"
                UPDATE merchandise_order
                SET creation_date = @CreationDate, 
                    employee_email = @EmployeeEmail, 
                    employee_name = @EmployeeName, 
                    manager_email = @ManagerEmail, 
                    manager_name = @ManagerName, 
                    clothing_size = @ClothingSize, 
                    merch_pack_id = @MerchPackId, 
                    source_id = @SourceId,
                    status_id = @StatusId,
                    delivery_date = @DeliveryDate
                WHERE id = @Id;";

            var parameters = new
            {
                Id = itemToUpdate.Id,
                CreationDate = itemToUpdate.CreationDate.Value,
                EmployeeEmail = itemToUpdate.EmployeeEmail.Value,
                EmployeeName = itemToUpdate.EmployeeName.Value,
                ManagerEmail = itemToUpdate.ManagerEmail.Value,
                ManagerName = itemToUpdate.ManagerName.Value,
                ClothingSize = (int)itemToUpdate.ClothingSize,
                MerchPackId = itemToUpdate.MerchPack.Id,
                SourceId = itemToUpdate.Source.Id,
                StatusId = itemToUpdate.Status.Id,
                DeliveryDate = itemToUpdate.DeliveryDate?.Value,
            };
            var commandDefinition = new CommandDefinition(
                sql,
                parameters: parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(itemToUpdate);
            return itemToUpdate;
        }
    }
}
