using Confluent.Kafka;

namespace OzonEdu.MerchandiseService.ApplicationServices.MessageBroker
{
    public interface IProducerBuilderWrapper
    {
        /// <summary>
        /// Producer instance
        /// </summary>
        IProducer<string, string> Producer { get; set; }

        /// <summary>
        /// Топик для отправки email
        /// </summary>
        string EmployeeNotificationTopic { get; set; }
    }
}
