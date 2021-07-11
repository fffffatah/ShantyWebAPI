using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShantyWebAPI.CustomAttributes
{
    public class IgnorePropertyFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //if (context.ApiDescription == null || operation.Parameters == null)
            //    return;

            //if (!context.ApiDescription.ParameterDescriptions.Any())
            //    return;


            //context.ApiDescription.ParameterDescriptions.Where(p => p.Source.Equals(BindingSource.Form)
            //            && p.CustomAttributes().Any(p => p.GetType().Equals(typeof(JsonIgnoreAttribute))))
            //    .ForAll(p => operation.RequestBody.Content.Values.Single(v => v.Schema.Properties.Remove(p.Name)));



            //context.ApiDescription.ParameterDescriptions.Where(p => p.Source.Equals(BindingSource.Query)
            //              && p.CustomAttributes().Any(p => p.GetType().Equals(typeof(JsonIgnoreAttribute))))
            //    .ForAll(p => operation.Parameters.Remove(operation.Parameters.Single(w => w.Name.Equals(p.Name))));


            #region OldCode

            var excludedProperties = context.ApiDescription.ParameterDescriptions.Where(p =>
                p.Source.Equals(BindingSource.Form));

            if (excludedProperties.Any())
            {

                foreach (var excludedPropertie in excludedProperties)
                {
                    foreach (var customAttribute in excludedPropertie.CustomAttributes())
                    {
                        if (customAttribute.GetType() == typeof(JsonIgnoreAttribute))
                        {
                            for (int i = 0; i < operation.RequestBody.Content.Values.Count; i++)
                            {
                                for (int j = 0; j < operation.RequestBody.Content.Values.ElementAt(i).Encoding.Count; j++)
                                {
                                    if (operation.RequestBody.Content.Values.ElementAt(i).Encoding.ElementAt(j).Key ==
                                        excludedPropertie.Name)
                                    {
                                        operation.RequestBody.Content.Values.ElementAt(i).Encoding
                                            .Remove(operation.RequestBody.Content.Values.ElementAt(i).Encoding
                                                .ElementAt(j));
                                        operation.RequestBody.Content.Values.ElementAt(i).Schema.Properties.Remove(excludedPropertie.Name);


                                    }
                                }
                            }

                        }
                    }
                }

            }


            #endregion
        }
    }
}
