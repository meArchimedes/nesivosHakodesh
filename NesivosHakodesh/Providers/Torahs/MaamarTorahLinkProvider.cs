using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Core;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;

namespace NesivosHakodesh.Providers.Torahs
{
    public class MaamarTorahLinkProvider
    {
        public static ProviderResponse AddMaamarTorahLink(List<MaamarTorahLink> MaamarTorahs)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                int count = 0;

                foreach (MaamarTorahLink MaamarTorah in MaamarTorahs)
                {
                    //chack if its Linked 
                    var link = db.MaamarTorahLinks.Where(x => x.Maamar.MaamarID == MaamarTorah.Maamar.MaamarID)
                            .Where(x => x.Torah.TorahID == MaamarTorah.Torah.TorahID).FirstOrDefault();

                    if (link == null)
                    {
                        Torah torah = MaamarTorah.Torah;

                        if(torah.TorahID > 0)
                        {
                            torah = db.Torahs.Find(torah.TorahID);
                        }
                        else
                        {
                            torah.TorahID = 0;
                            torah.Sefer = db.Sefurim.Find(torah.Sefer.SeferID);
                        }

                        MaamarTorahLink maamarTorahLink = new MaamarTorahLink
                        {
                            Maamar = db.Maamarim.Find(MaamarTorah.Maamar.MaamarID),
                            Torah = torah,
                        };


                        db.MaamarTorahLinks.Add(maamarTorahLink);
                    }
                    else
                    {
                        count++;
                    }
                   
                }

              

                    db.SaveChanges();

                string error = "";
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

        internal static ProviderResponse GetMaamarTorahLink(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                List<MaamarTorahLink> MaamarTorahLink = AppProvider.GetDBContext().MaamarTorahLinks.Where(x => x.Torah.TorahID == id).Include(x => x.Maamar).ToList();

                if (MaamarTorahLink.Count != 0)
                {
                    response.Data = MaamarTorahLink;
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

        public static ProviderResponse DeleteMaamarTorahLink(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                MaamarTorahLink currMaamarTorah = db.MaamarTorahLinks.Find(id);

                if (currMaamarTorah == null)
                {
                    response.Messages.Add("מאאמר קושר לא חוקי");
                }
                else
                {

                    currMaamarTorah.IsDeleted = true;
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
