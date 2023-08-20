using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Core;
using NesivosHakodesh.Core.Config;
using NesivosHakodesh.Core.DB;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Utils;
using NesivosHakodesh.Providers.Utils.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Topics
{
    public class TopicsProvider
    {
        internal static ProviderResponse GetAllTopics(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            ListResData<Topic> listRes = new ListResData<Topic>(search);
            response.Data = listRes;

            try
            {
                var TopicSearch = AppProvider.GetDBContext().Topics
                      .Where(x => string.IsNullOrEmpty(search.SearchTerm) ||
                      x.Name.Contains(search.SearchTerm));

                if (search.CategoryId != 0)
                {
                    TopicSearch = TopicSearch.Where(x => x.Category.Id == search.CategoryId)
                         .Where(x => x.ParentTopic == null);
                }

                listRes.TotalCount = TopicSearch.Count();
                if (search.SortBy != null)
                {
                    if (search.SortDirection == SortDirection.Ascending)
                    {

                        TopicSearch = TopicSearch.OrderBy(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)

                    }
                    else
                    {

                        TopicSearch = TopicSearch.OrderByDescending(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)

                    }
                }
               
                TopicSearch = TopicSearch.Skip(search.PageStartIndex).Take(search.ItemsPerPage);
                listRes.List = TopicSearch.Include(x => x.SubTopices).Include(x => x.Category).ToList();
                response.Data = listRes;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse GetTopic(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                Topic topic = AppProvider.GetDBContext().Topics.Where(x => x.TopicID == id).Include(x => x.ParentTopic).Include(x => x.SubTopices).FirstOrDefault();

                if (topic != null)
                {
                    response.Data = topic;
                }
                else
                {
                    response.Messages.Add("ענין לא חוקי");
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse AddTopic(Topic Topic)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();


              
                    response.Messages.AddRange(ValidateTopic(null, Topic, false, db));
                    if (response.Success)
                    {
                    
                        Topic topic = new Topic
                        {
                            Category = db.Categories.Find(Topic.Category?.Id),
                            Name = Topic.Name,
                           // ParentTopic = db.Topics.Find(Topic.ParentTopic?.TopicID),
                            Status = TopicsStatus.Active,
                            SubTopices = Topic.SubTopices.Where(x => x.Name != null).ToList()

                        };

                        db.Topics.Add(topic);

                        
                    }
                

                db.SaveChanges();
                response.Data = Topic;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse UpdateTopic(Topic Topic)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                Topic currTopic = db.Topics.Where(x => x.TopicID == Topic.TopicID).FirstOrDefault();

                response.Messages.AddRange(ValidateTopic(currTopic, Topic, true, db));
                if (response.Success)
                {
                    //var currTopiceTest = db.Topics.Where(x => x.TopicID == Topic.TopicID).FirstOrDefault();
                    var UpdateSub = Topic.SubTopices.Where(x => !string.IsNullOrEmpty(x.Name)).ToList();
                    //currTopic.ParentTopic = Topic.ParentTopic;
                    currTopic.Name = Topic.Name;
                    currTopic.Category = db.Categories.Find(Topic.Category?.Id);
                    List<Topic> SubTopices = new List<Topic>();
                    foreach (var Sub in UpdateSub)
                    {
                        var CurrSubTopice = currTopic.SubTopices.Where(x => x.TopicID == Sub.TopicID).FirstOrDefault();
                        if (CurrSubTopice != null)
                        {
                            CurrSubTopice = Sub;
                        }
                        else
                        {
                            SubTopices.Add(Sub);

                        }
                    }
                    currTopic.SubTopices.AddRange(SubTopices);
                }

              


                db.SaveChanges();
                response.Data = currTopic;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        internal static ProviderResponse DeleteTopic(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                var currTopic = db.Topics.Find(id);

                if (currTopic == null)
                {
                    response.Messages.Add("ענין לא חוקי");
                }
                else
                {
                    var maamarTopice = db.Maamarim.Where(x => x.Topic.TopicID == currTopic.TopicID).ToList();

                    if (maamarTopice.Count == 0)
                    {
                        currTopic.Status = TopicsStatus.Deleted;
                        db.SaveChanges();
                    }
                    else
                    {
                        response.Messages.Add("אינך יכול למחוק נושא זה  משום שהקשור למאמר");
                    }
                    
                }
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }
            return response;
        }

        public static ProviderResponse GetSeforimTopics()
        {
            ProviderResponse response = new ProviderResponse();
           

            try
            {

                int SeforimCategoryId = 8;
                var TopicSearch = AppProvider.GetDBContext().Topics
                      .Where(x => x.Category.Id == SeforimCategoryId)
                      .Where(x => x.ParentTopic == null).ToList();


                  response.Data = TopicSearch;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }

        public static ProviderResponse GetTopicsForDropDown()
        {
            ProviderResponse response = new ProviderResponse();

            try
            {

                var TopicSearch = AppProvider.GetDBContext().Topics.ToList();

                response.Data = TopicSearch;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }
        private static List<string> ValidateTopic(Topic currTopic, Topic Topic, bool update, AppDBContext db)
        {
            List<string> errors = new List<string>();

            //if not topice
            if (Topic == null)
            {
                errors.Add("נושא לא חוקי");
            }
            else if (update && currTopic == null)
            {
                errors.Add("מזהה נושא מובהק");
            }
            else if (Topic.Category == null)
            {
                errors.Add("אתה צריך להוסיף קטגוריה");
            }
            else
            {
                Topic topiceName = new Topic();
                // topiceName = db.Topics.Where(x => x.Name == Topic.Name).Include(x => x.SubTopices).FirstOrDefault();

                // if add New topice try to find in the same category if theere is the same name,
                topiceName = db.Topics.Where(x => x.Name == Topic.Name).Where(x => x.Category.Id == Topic.Category.Id).Include(x => x.SubTopices).FirstOrDefault();
               /* if (currTopic == null)
                 {
                     topiceName = db.Topics.Where(x => x.Name == Topic.Name).Where(x => x.Category.Id == Topic.Category.Id).Include(x => x.SubTopices).FirstOrDefault();
                 }

                 else
                 {
                     topiceName = currTopic;
                 }*/

                if (currTopic != null && currTopic.Name == Topic.Name)
                {

                }
                else
                {
                   

                    if (topiceName !=  null)
                    {
                        errors.Add("אתה לא יכול להוסיף שם ענין שכבר קיים");
                    }
                }
                
               
              /*  if (Topic.ParentTopic != null)
                {
                    Topic.ParentTopic = db.Topics.Find(Topic.ParentTopic.TopicID);
                    if (Topic.ParentTopic == null)
                    {
                        errors.Add("הפניה בנושא לא חוקי");
                    }

                }*/
              // if there is sub topices
                if (Topic.SubTopices.Count != 0)
                {
                    foreach (Topic SubTopice in Topic.SubTopices.Where(x => x.TopicID == 0))
                    {
                        // chack in curr sub topices if this topice name exists
                        Topic CurrSubTopice = topiceName?.SubTopices?.Where(x => x.Name == SubTopice.Name).FirstOrDefault();
                   
                        if (CurrSubTopice != null )
                        {
                            // if the topice name exists in the category and if he add the name 2 times in sub we need to remove it,
                            var NewSubs = Topic.SubTopices.Where(x => x.Name == SubTopice.Name && x.TopicID == 0).ToList();
                            foreach (var Sub in NewSubs)
                            {
                                Sub.Name = null;
                            }
                            
                           // errors.Add("אתה לא יכול להוסיף שם תת  קטוגורי  שכבר קיים");
                        }

                        // remove name if add it 2 time in sub list
                        var NewSubTopices = Topic.SubTopices.Where(x => x.Name == SubTopice.Name).ToList();
                        if ( NewSubTopices.Count > 1)
                        {
                           List<Topic> NewSubTopices1 = NewSubTopices.Where(x => x.Name != null).AsEnumerable().Reverse().Skip(1).ToList();
                           
                            foreach (Topic NewSub in NewSubTopices1)
                            {
                                NewSub.Name = null;
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(Topic.Name))
                {
                    errors.Add("חסר שם");
                }
            }

            return errors;
        }

    }
}
