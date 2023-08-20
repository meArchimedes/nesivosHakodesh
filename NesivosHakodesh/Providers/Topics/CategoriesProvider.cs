using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Comman;
using NesivosHakodesh.Core;
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
    public class CategoriesProvider
    {
        public static ProviderResponse GetAllCategories(SearchCriteria search)
        {
            ProviderResponse response = new ProviderResponse();
            ListResData<Category> listRes = new ListResData<Category>(search);
            response.Data = listRes;

            try
            {
                var CategoriesSearch = AppProvider.GetDBContext().Categories.Where(x => string.IsNullOrEmpty(search.SearchTerm) || x.CategoryName.Contains(search.SearchTerm));

                listRes.TotalCount = CategoriesSearch.Count();
                if (search.SortBy != null)
                {
                    if (search.SortDirection == SortDirection.Ascending)
                    {

                        CategoriesSearch = CategoriesSearch.OrderBy(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)

                    }
                    else
                    {

                        CategoriesSearch = CategoriesSearch.OrderByDescending(x => EF.Property<object>(x, search.SortBy));  //.ThenBy(x => x.HebrewFirstName)

                    }
                }

                CategoriesSearch = CategoriesSearch.Skip(search.PageStartIndex).Take(search.ItemsPerPage);
                listRes.List = CategoriesSearch.ToList();
                response.Data = listRes;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }



        public static ProviderResponse AddCategories(Category Categories)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();

                response.Messages.AddRange(ValidateCategory(Categories));
                if (response.Success)
                {
                    var currCatigory = db.Categories.Where(x => x.CategoryName == Categories.CategoryName).FirstOrDefault();
                    if (currCatigory != null)
                    {
                        response.Messages.Add("אתה לא יכול להוסיף שם קטוגורי שכבר קיים");
                    }
                    else
                    {
                        Category categories = new Category
                        {
                            CategoryName = Categories.CategoryName,
                            Status = CategoryStatus.Active,
                        };
                        db.Categories.Add(categories);

                        db.SaveChanges();
                        response.Data = categories;
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



        public static ProviderResponse UpdateCategories(Category Categories)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                Category CurrCategory = db.Categories.Find(Categories.Id);

                response.Messages.AddRange(ValidateCategory(Categories));
                if (response.Success)
                {
                    CurrCategory.CategoryName = Categories.CategoryName;
                }

                db.SaveChanges();
                response.Data = CurrCategory;
            }
            catch (Exception e)
            {
                Logger.Log(e.ToString());
                response.Messages.Add("התרחשה שגיאה");
            }

            return response;
        }


        public static ProviderResponse DeleteCategories(int id)
        {
            ProviderResponse response = new ProviderResponse();

            try
            {
                AppDBContext db = AppProvider.GetDBContext();
                Category CurrCategory = db.Categories.Where(x => x.Id == id).Include(x => x.Topics).FirstOrDefault();

                if (CurrCategory == null)
                {
                    response.Messages.Add("קטוגרי לא חוקי");
                }
                else
                {
                    if (CurrCategory.Topics.Count == 0)
                    {
                        CurrCategory.Status = CategoryStatus.Deleted;
                        db.SaveChanges();
                    }
                    else
                    {
                        response.Messages.Add("אינך יכול למחוק קטוגרי זה  משום שהקשור לעניו");
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


        private static List<string> ValidateCategory(Category category)
        {
            List<string> errors = new List<string>();

            if (category == null)
            {
                errors.Add("קטגוריה לא חוקי");
            }

            else
            {
                if (category.CategoryName == null)
                {
                    errors.Add("צריך שם קטגוריה");
                }
            }


            return errors;
        }
    }
}
