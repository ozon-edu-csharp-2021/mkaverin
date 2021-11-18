﻿using MediatR;

namespace OzonEdu.MerchandiseService.ApplicationServices.Queries.OrderAggregate
{
    public class CheckGiveOutMerchByEmployeeIdQuery : IRequest<bool>
    {
        public long EmployeeId { get; init; }
        public long MerchType { get; set; }
    }
}