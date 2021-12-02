using OzonEdu.MerchandiseService.Domain.Exceptions.OrderAggregate;
using OzonEdu.MerchandiseService.Domain.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.OrderAggregate
{
    public sealed class NameUser : ValueObject
    {
        public string Value { get; }
        private NameUser(string nameString) => Value = nameString;
        public static NameUser Create(string nameString)
        {
            if (IsValidName(nameString))
            {
                return new NameUser(nameString);
            }
            throw new NameUserException($"User Name is invalid: {nameString}");
        }
        public override string ToString() => Value;

        private static bool IsValidName(string nameString)
            => nameString.Length < 150;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
