using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Core;
using NesivosHakodesh.Core.Config;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace NesivosHakodesh.Providers.Solr
{
    public class SolrSyncProvider
    {
        public static ProviderResponse SyncSolr()
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                List<SolrRecord> solrRecords = GetSolrRecords();
                SendToSolr(solrRecords);

                SolrDelete solrDelete = GetSolrDelete();
                SendToSolr(solrDelete);

            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        private static List<SolrRecord> GetSolrRecords()
        {
            return AppProvider.GetDBContext().Maamarim
                        .Where(x => x.Status != MaamarimStatus.Deleted)
                            .Select(x => new SolrRecord
                            {
                                id = $"Maamar-{x.MaamarID}",
                                internalId = x.MaamarID,
                                //Type = "Torahs"

                                Title = x.Title,
                                //Content = string.Join("\n", x.MaamarParagraphs.Select(p => p.Text)),
                                Content = x.Content,

                                Topic = x.Topic.Name,
                                Source = x.Source.FullName,
                                MaamarType = x.TypeValue,
                                Parsha = x.Parsha,
                                Year = x.Year,
                                CreatedTime = x.CreatedTime,

                                OtherDetails = x.OtherDetails,
                                Date = x.Date,
                                Status = x.StatusValue,
                                SubTopics = x.SubTopics.Select(x => x.Topic.Name).ToList(),
                            })
                            .ToList();

            /*solrRecords.AddRange(AppProvider.GetDBContext().Torahs
                                                 .Select(x => new SolrRecord
                                                 {
                                                     id = $"Torah-{x.TorahID}",
                                                     internalId = x.TorahID,
                                                     //Type = "Torahs",
                                                     
                                                     Title = x.Title,
                                                   // Need to fix ===  Content = x.Text,

                                                     //Parsha = x.Parsha,
                                                     CreatedTime = x.CreatedTime,
                                                 })
                                                 .ToList());*/
        }

        private static SolrDelete GetSolrDelete()
        {
            List<SolrRecord> solrRecords = AppProvider.GetDBContext().Maamarim
                                                .Where(x => x.Status == MaamarimStatus.Deleted)
                                                .Select(x => new SolrRecord
                                                {
                                                    id = $"Maamarim-{x.MaamarID}",

                                                })
                                                .IgnoreQueryFilters()
                                                .ToList();

            SolrDelete solrDelete = new SolrDelete
            {
                delete = solrRecords
            };

            return solrDelete;
        }

        private static void SendToSolr(object value)
        {
            string json = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            StringContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            string url = $"{AppSettingsProvider.GetOtherSettings().SolrBaseUrl}/update?commit=true";

            using HttpClient client = new HttpClient();
                HttpResponseMessage res = client.PostAsync(url, httpContent).Result;
        }
    }
}
