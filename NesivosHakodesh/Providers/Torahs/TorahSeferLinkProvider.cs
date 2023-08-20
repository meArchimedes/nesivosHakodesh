using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Core;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Providers.Torahs
{
    public class TorahSeferLinkProvider
    {
        public static ProviderResponse AddTorahSeferLink(List<TorahSeferLink> torahSefers)
        {
            var response = new ProviderResponse();

            try
            {
                var db = AppProvider.GetDBContext();

                var count = 0;

                foreach (var torahSefer in torahSefers)
                {
                    //check if its Linked 
                    var existingLinks = db.TorahSeferLinks.Where(x => x.Torah.TorahID == torahSefer.Torah.TorahID)
                        .Where(x => x.Sefer.SeferID == torahSefer.Sefer.SeferID).FirstOrDefault();

                    if (existingLinks == null)
                    {
                        var sefer = torahSefer.Sefer;

                        if(sefer.SeferID > 0)
                        {
                            sefer = db.Sefurim.Find(sefer.SeferID);
                        }
                        //else
                        //{
                        //    torah.TorahID = 0;
                        //    torah.Sefer = db.Sefurim.Find(torah.Sefer.SeferID);
                        //}

                        var torahSeferLink = new TorahSeferLink
                        {
                            Torah = db.Torahs.Find(torahSefer.Torah.TorahID),
                            Sefer = sefer,
                        };

                        db.TorahSeferLinks.Add(torahSeferLink);
                    }
                    else
                    {
                        count++;
                    }                 
                }

                db.SaveChanges();

                var error = string.Empty;
                if(count > 0)
                {
                    error = $"{count} כבר מקושר";
                }

                response.Data = error;               
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse GetTorahSeferLink(int seferId)
        {
            var response = new ProviderResponse();

            try
            {
                var torahSeferLink = AppProvider.GetDBContext().TorahSeferLinks.Where(x => x.Sefer.SeferID == seferId).Include(x => x.Torah).ToList();

                if (torahSeferLink.Count != 0)
                {
                    response.Data = torahSeferLink;
                }
                else
                {
                    response.Messages.Add("קישור לא חוקי");
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        public static ProviderResponse DeleteTorahSeferLink(int torahSeferId)
        {
            var response = new ProviderResponse();

            try
            {
                var db = AppProvider.GetDBContext();

                var currentTorahSefer = db.TorahSeferLinks.Find(torahSeferId);

                if (currentTorahSefer == null)
                {
                    response.Messages.Add("מאאמר קושר לא חוקי");
                }
                else
                {
                    currentTorahSefer.IsDeleted = true;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }
    }
}
