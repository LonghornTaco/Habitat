using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ProjectOxford.Face.Contract;
using Sitecore.Feature.CognitiveServices.Models.Repository;
using System.Net.Http;
using System.Net.Http.Headers;
using Sitecore.Feature.CognitiveServices.Configuration;
using Newtonsoft.Json;

namespace Sitecore.Feature.CognitiveServices.Services
{
   public class MicrosoftFaceApiService : IFaceApiService
   {
      private readonly ICognitiveServicesConfiguration _configuration;

      public MicrosoftFaceApiService(ICognitiveServicesConfiguration configuration)
      {
         _configuration = configuration;
      }

      public Face[] Detect(string base64Image)
      {
         if (string.IsNullOrWhiteSpace(base64Image)) throw new ArgumentException("base64Image cannot be null");

         var faces = DetectFaces(base64Image);

         return faces;
      }

      [Obsolete]
      public string CreatePerson()
      {
         throw new NotImplementedException();
      }

      public string CreatePerson(string name, string base64Image)
      {
         if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name cannot be null");
         if (string.IsNullOrWhiteSpace(base64Image)) throw new ArgumentException("base64Image cannot be null");

         byte[] imageByteData = System.Convert.FromBase64String(base64Image);

         CreatePersonResult createPersonResult;
         using (var client = new HttpClient())
         {
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _configuration.ApiKey);
            var uri = $"{_configuration.ApiRoot}/{_configuration.PersonGroupApi}/{_configuration.PersonGroupId}/{_configuration.PersonApi}/";

            var data = JsonConvert.SerializeObject(new { name = name });
            var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = client.PostAsync(uri, content).Result;
            var result = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
               createPersonResult = JsonConvert.DeserializeObject<CreatePersonResult>(result);

               if (createPersonResult != null)
               {
                  uri = $"{uri}/{createPersonResult.PersonId.ToString()}/persistedFaces";

                  using (var imageContent = new ByteArrayContent(imageByteData))
                  {
                     imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                     var imageResponse = client.PostAsync(uri, imageContent).Result;
                     var imageResult = imageResponse.Content.ReadAsStringAsync().Result;

                     if (!imageResponse.IsSuccessStatusCode)
                     {
                        var error = JsonConvert.DeserializeObject<Error>(imageResult);
                        throw new InvalidOperationException($"There was an error saving an image to personId {createPersonResult.PersonId.ToString()}: {error.Code} - {error.Message}");
                     }
                  }
               }
            }
            else
            {
               var error = JsonConvert.DeserializeObject<Error>(result);
               throw new InvalidOperationException($"There was an error creating a person for {name}: {error.Code} - {error.Message}");
            }


         }
         return createPersonResult?.PersonId.ToString() ?? string.Empty;
      }

      [Obsolete]
      public void AddPhotoToPerson(string personId, string base64ImageString)
      {
         throw new NotImplementedException();
      }

      public string VerifyPerson(string base64Image)
      {
         var personIdToReturn = string.Empty;

         if (string.IsNullOrWhiteSpace(base64Image)) throw new ArgumentException("base64Image cannot be null");

         var faces = DetectFaces(base64Image);

         if (faces.Length == 1)
         {
            var data = JsonConvert.SerializeObject(new
            {
               personGroupId = _configuration.PersonGroupId,
               faceIds = new[] { faces[0].FaceId },
               maxNumOfCandidatesReturned = 1,
               confidenceThreshold = 0.9
            });
            using (var client = new HttpClient())
            {
               client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _configuration.ApiKey);

               var uri = $"{_configuration.ApiRoot}/{_configuration.IdentifyApi}";
               var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
               var response = client.PostAsync(uri, content).Result;
               var result = response.Content.ReadAsStringAsync().Result;

               if (response.IsSuccessStatusCode)
               {
                  var identifyResponse = JsonConvert.DeserializeObject<List<IdentifyResponse>>(result);

                  var candidates = identifyResponse.FirstOrDefault(c => c.FaceId == faces[0].FaceId.ToString());
                  var candidate = candidates?.Candidates.FirstOrDefault(p => p.Confidence > 0.9);

                  personIdToReturn = candidate?.PersonId.ToString() ?? string.Empty;
               }
               else
               {
                  var error = JsonConvert.DeserializeObject<Error>(result);
                  throw new InvalidOperationException($"There was an error identifying the user: {error.Code} - {error.Message}");
               }
            }
         }
         else
         {
            throw new InvalidOperationException($"More than one face was detected in the image submitted. Please submit an image with only one face.");
         }

         return personIdToReturn;
      }

      private Face[] DetectFaces(string base64Image)
      {
         byte[] byteData = System.Convert.FromBase64String(base64Image);

         Face[] faces;
         using (var client = new HttpClient())
         {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _configuration.ApiKey);
            queryString["returnFaceId"] = "true";
            queryString["returnFaceLandmarks"] = "false";
            var uri = $"{_configuration.ApiRoot}/{_configuration.DetectApi}?" + queryString;

            using (var content = new ByteArrayContent(byteData))
            {
               content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
               var response = client.PostAsync(uri, content).Result;

               var result = response.Content.ReadAsStringAsync().Result;

               faces = JsonConvert.DeserializeObject<Face[]>(result);
            }
         }
         return faces;
      }

      private List<Person> GetPeople()
      {
         var list = new List<Person>();

         using (var client = new HttpClient())
         {
            var uri = $"{_configuration.ApiRoot}/{_configuration.PersonGroupApi}/{_configuration.PersonGroupId}/persons";
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _configuration.ApiKey);
            var response = client.GetAsync(uri).Result;
            var result = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
               list = JsonConvert.DeserializeObject<List<Person>>(result);
            }
         }

         return list;
      }
   }
}