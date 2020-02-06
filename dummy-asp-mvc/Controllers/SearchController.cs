using dummy_asp_mvc.App_Start;
using dummy_asp_mvc.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dummy_asp_mvc.Controllers
{
    public class SearchController : Controller
    {
        MongoContext mongoContext;
        public SearchController()
        {
            mongoContext = new MongoContext();
        }

        // GET: Search
        public ActionResult Index()
        {
            var collection = mongoContext._database.GetCollection<Dummy>("dummies");

            var dummies = collection.AsQueryable();

            return View("Index", dummies.AsEnumerable());
        }

        public ActionResult Create()
        {
            return PartialView("Create");
        }

        [HttpPost]
        public ActionResult CreateDummy(Dummy dummy)
        {
            var collection = mongoContext._database.GetCollection<Dummy>("dummies");

            var builder = Builders<Dummy>.Filter;
            var filterByNameAndLastName = builder.And(builder.Eq(x => x.Name,dummy.Name), 
                builder.Eq(x => x.Name, dummy.Name));

            var count = collection.CountDocuments(filterByNameAndLastName);

            if (count == 0)
            {
                collection.InsertOne(dummy);
                
            }
            else
            {
                dummy = collection.Find<Dummy>(filterByNameAndLastName).FirstOrDefault();                
            }

            return PartialView("Details", dummy);
        }

        public ActionResult List()
        {
            var collection = mongoContext._database.GetCollection<Dummy>("dummies");

            var dummies = collection.AsQueryable();           

            return PartialView("List", dummies.AsEnumerable());
        }

        public ActionResult Details(string id)
        {
            var collection = mongoContext._database.GetCollection<Dummy>("dummies");

            var builder = Builders<Dummy>.Filter;
            var filterById = builder.Eq(x => x.Id, id);

            var dummy = collection.Find(filterById).FirstOrDefault();

            return PartialView("Details", dummy);
        }

        public ActionResult Edit(string id)
        {
            var collection = mongoContext._database.GetCollection<Dummy>("dummies");

            var builder = Builders<Dummy>.Filter;
            var filterById = builder.Eq(x => x.Id, id);

            var dummy = collection.Find(filterById).FirstOrDefault();

            return PartialView("Edit", dummy);
        }

        [HttpPost]
        public ActionResult UpdateDummy(Dummy dummy)
        {
            var collection = mongoContext._database.GetCollection<Dummy>("dummies");

            var builder = Builders<Dummy>.Filter;
            var filterById = builder.Eq(x => x.Id, dummy.Id);
            var count = collection.CountDocuments(filterById);
            
            var findOneAndReplaceOptions = new FindOneAndReplaceOptions<Dummy>() {
                IsUpsert=true,
                ReturnDocument=ReturnDocument.After
            };

            dummy = collection.FindOneAndReplace<Dummy>(filterById,dummy, findOneAndReplaceOptions);
            

            return PartialView("Details", dummy);
        }
    }
}