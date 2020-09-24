using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Practical.Extentions
{
    public static class ModelStateExtensions
    {
        public static IList<string> GetErrorMessages(this ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Values
                            .Where(v => v.Errors != null && v.Errors.Count > 0)
                            .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                            .ToList();
        }
    }
}
