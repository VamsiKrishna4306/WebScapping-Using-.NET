using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.Core;
using System.Configuration;
using MongoDBCRUDOperations.App_Start;
using MongoDB.Driver;
using MongoDBCRUDOperations.Models;

namespace MongoDBCRUDOperations.Controllers
{
    public class JobPostingsController : Controller
    {
        private MongoDBContext dbcontext;
        private IMongoCollection<JobPostingsModel> jobPostingsCollection;

        public JobPostingsController()
        {
            dbcontext = new MongoDBContext();
            jobPostingsCollection=dbcontext.database.GetCollection<JobPostingsModel>("Assignment_Test3");

        }
        // GET: JobPostings
        public ActionResult Index()
        {
            List<JobPostingsModel> JobPostings = jobPostingsCollection.AsQueryable<JobPostingsModel>().ToList();
            return View(JobPostings);
        }

        // GET: JobPostings/Details/5
        public ActionResult Details(String id)
        {
            var JobPostId = new ObjectId(id);
            var JobPost = jobPostingsCollection.AsQueryable<JobPostingsModel>().SingleOrDefault(x => x.Id == JobPostId);
            return View(JobPost);
        }

        // GET: JobPostings/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: JobPostings/Create
        [HttpPost]
        public ActionResult Create(JobPostingsModel JobPost)
        {
            try
            {
                // TODO: Add insert logic here
                jobPostingsCollection.InsertOne(JobPost);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: JobPostings/Edit/5
        public ActionResult Edit(String id)
        {
            var JobPostId = new ObjectId(id);
            var JobPost = jobPostingsCollection.AsQueryable<JobPostingsModel>().SingleOrDefault(x => x.Id == JobPostId);
            return View(JobPost);
        }

        // POST: JobPostings/Edit/5
        [HttpPost]
        public ActionResult Edit(String id, JobPostingsModel JobPost)
        {
            try
            {
                // TODO: Add update logic here
                var filter = Builders<JobPostingsModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<JobPostingsModel>.Update
                                .Set("JobTitle", JobPost.JobTitle)
                                .Set("Company", JobPost.Company)
                                .Set("PostedSalary", JobPost.PostedSalary)
                                .Set("PostDate", JobPost.PostDate)
                                .Set("JOblink", JobPost.JOblink);
                var result = jobPostingsCollection.UpdateOne(filter, update);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: JobPostings/Delete/5
        public ActionResult Delete(String id)
        {
            var JobPostId = new ObjectId(id);
            var JobPost = jobPostingsCollection.AsQueryable<JobPostingsModel>().SingleOrDefault(x => x.Id == JobPostId);
            return View(JobPost);
        }

        // POST: JobPostings/Delete/5
        [HttpPost]
        public ActionResult Delete(String id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                jobPostingsCollection.DeleteOne(Builders<JobPostingsModel>.Filter.Eq("_id", ObjectId.Parse(id)));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
