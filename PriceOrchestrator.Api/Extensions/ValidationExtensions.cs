using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace PriceOrchestrator.Api.Extensions
{
    public static class ValidationExtensions
    {
        public static ValidationProblemDetails? ValidateModel(this object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(model, context, results, validateAllProperties: true);

            if (!results.Any())
                return null;

            var dict = new Dictionary<string, string[]>();

            foreach (var r in results)
            {
                var memberNames = r.MemberNames.Any() ? r.MemberNames : new[] { string.Empty };
                foreach (var name in memberNames)
                {
                    if (!dict.TryGetValue(name, out var list))
                    {
                        list = new List<string>().ToArray();
                        dict[name] = list;
                    }

                    var current = dict[name].ToList();
                    current.Add(r.ErrorMessage ?? "");
                    dict[name] = current.ToArray();
                }
            }

            return new ValidationProblemDetails(dict);
        }
    }
}
