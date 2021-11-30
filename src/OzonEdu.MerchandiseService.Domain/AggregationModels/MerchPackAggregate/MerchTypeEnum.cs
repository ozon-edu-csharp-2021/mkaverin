using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public class MerchTypeEnum : Enumeration
    {
        public static MerchTypeEnum WelcomePack = new(10, nameof(WelcomePack).ToLowerInvariant());
        public static MerchTypeEnum ConferenceListenerPack = new(20, nameof(ConferenceListenerPack).ToLowerInvariant());
        public static MerchTypeEnum ConferenceSpeakerPack = new(30, nameof(ConferenceSpeakerPack).ToLowerInvariant());
        public static MerchTypeEnum ProbationPeriodEndingPack = new(40, nameof(ProbationPeriodEndingPack).ToLowerInvariant());
        public static MerchTypeEnum VeteranPack = new(50, nameof(VeteranPack).ToLowerInvariant());

        protected MerchTypeEnum(int id, string name) : base(id, name)
        {

        }
    }
}