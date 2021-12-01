using CSharpCourse.Core.Lib.Models;
using MediatR;
using System.Collections.Generic;

namespace OzonEdu.MerchandiseService.ApplicationServices.Commands
{
    public class NewMerchandiseAppearedCommand : IRequest
    {
        public IReadOnlyCollection<StockReplenishedItem> Items { get; set; }
    }
}