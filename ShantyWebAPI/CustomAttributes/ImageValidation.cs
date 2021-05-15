using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using MongoDB.Bson;
using System.IO;
using Newtonsoft.Json;

namespace ShantyWebAPI.CustomAttributes
{
    public class ImageValidation:ValidationAttribute
    {
        internal class Adult
        {
            [JsonProperty("isAdultContent")]
            public bool IsAdultContent { get; set; }

            [JsonProperty("isRacyContent")]
            public bool IsRacyContent { get; set; }

            [JsonProperty("isGoryContent")]
            public bool IsGoryContent { get; set; }

            [JsonProperty("adultScore")]
            public double AdultScore { get; set; }

            [JsonProperty("racyScore")]
            public double RacyScore { get; set; }

            [JsonProperty("goreScore")]
            public double GoreScore { get; set; }
        }
        internal class Root
        {
            [JsonProperty("adult")]
            public Adult Adult { get; set; }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile file;
            if (value != null)
            {
                file = (IFormFile)value;
                if (!(file.ContentType == "image/jpeg" || file.ContentType == "image/png"))
                {
                    return new ValidationResult(ErrorMessage = "Invalid Image Type");
                }
            }
            else
            {
                return new ValidationResult(ErrorMessage = "Profile Image/Icon Required");
            }
            //SEND REQUEST TO COGNITIVE VISION
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddHeader("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("COGNITIVE_VISION_KEY"));
            
            long length = file.Length;

            using var fileStream = file.OpenReadStream();
            byte[] fileBytes = new byte[length];
            fileStream.Read(fileBytes, 0, (int)file.Length);

            request.AddParameter("* /*", fileBytes, ParameterType.RequestBody);

            var client = new RestClient(new Uri(Environment.GetEnvironmentVariable("COGNITIVE_VISION_API")));

            var response = client.Execute(request);
            Root adult = JsonConvert.DeserializeObject<Root>(response.Content);
            if (adult.Adult.IsAdultContent && adult.Adult.AdultScore > 0.85)
            {
                return new ValidationResult(ErrorMessage = "Image Contains Adult Content");
            }
            else if (adult.Adult.IsGoryContent && adult.Adult.GoreScore > 0.85)
            {
                return new ValidationResult(ErrorMessage = "Image Contains Gore Content");
            }
            return ValidationResult.Success;
        }
    }
}
