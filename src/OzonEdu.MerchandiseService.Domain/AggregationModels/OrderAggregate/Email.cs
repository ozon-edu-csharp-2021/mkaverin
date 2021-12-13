using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public sealed class Email : ValueObject
    {
        public string Value { get; }
        private Email(string emailString) => Value = emailString;
        public static Email Create(string emailString)
        {
            if (IsValidEmail(emailString))
            {
                return new Email(emailString);
            }
            throw new EmailException($"Email is invalid: {emailString}");
        }
        public override string ToString() => Value;

        private static bool IsValidEmail(string emailString)
            => Regex.IsMatch(emailString, "^[a-zA-Z0-9_!#$%&'*+/=?`{|}~^.-]+@[a-zA-Z0-9.-]+$");

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
