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
            if (IsValidEmail(nameString))
            {
                return new NameUser(nameString);
            }
            throw new NameUserException($"User Name is invalid: {nameString}");
        }
        public override string ToString() => Value;

        private static bool IsValidEmail(string emailString)
            => Regex.IsMatch(emailString, "^[а-яА-Я][а-яА-Я0-9-]+$");

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
