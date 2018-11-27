using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMAccess;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using DmDemo.Utility;

namespace DmDemo.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult List(int page = 1, int limit = 30, string name="")
        {
            JsonResult ret = new JsonResult();
            ret.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            using (var dal = new DmDAL())
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT * FROM DMHR.EMPLOYEE");
                if (!string.IsNullOrWhiteSpace(name))
                {
                    sbSql.AddCondition(string.Format("EMPLOYEE_NAME LIKE '%{0}%'", name));
                }
                sbSql.AppendFormat(" LIMIT {0} OFFSET {1}",
                    limit, (page - 1) * limit);


                int count = Convert.ToInt32(dal.ExecuteScalar("SELECT COUNT(1) FROM DMHR.EMPLOYEE"));
                var dt = dal.ExecuteQuery(sbSql.ToString());
                ret.Data = JsonConvert.SerializeObject(new
                {
                    status = 0,
                    message = "",
                    total = count,
                    data = dt
                });
            }
            return ret;
        }
    }
}
