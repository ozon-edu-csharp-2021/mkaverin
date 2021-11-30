using OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate
{
    public class MerchType : Entity
    {
        public MerchTypeEnum Type { get; }

        private static IEnumerable<MerchTypeEnum> List() =>
             new[] { 
                 MerchTypeEnum.ConferenceListenerPack, 
                 MerchTypeEnum.ConferenceSpeakerPack, 
                 MerchTypeEnum.ProbationPeriodEndingPack,
                 MerchTypeEnum.VeteranPack,
                 MerchTypeEnum.WelcomePack
             };

        public MerchType(long id)
        {
            var merchType = List().SingleOrDefault(s => s.Id == id);

            if (merchType == null)
                throw new OrderStatusException($"Possible values for StatusType: {String.Join(",", List().Select(s => s.Name))}");

            Type = merchType;
            Id = Type.Id;
        }
    }
}
