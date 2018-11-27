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
    public class AuditRecordController : Controller
    {
        // GET: AuditRecord
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int page = 1, int limit = 20, string username = "")
        {
            JsonResult ret = new JsonResult();
            ret.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            try
            {
                using (var dal = new DmDAL())
                {
                    //dal.ConnectionString = ConfigurationManager.ConnectionStrings["DmConnection"].ConnectionString;
                    //dal.Open();

                    StringBuilder sbSql = new StringBuilder();
                    StringBuilder sbCount = new StringBuilder();

                    sbCount.Append("SELECT COUNT(*) FROM SYSAUDITOR.V$AUDITRECORDS");
                    sbSql.Append("SELECT * FROM SYSAUDITOR.V$AUDITRECORDS");
                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        sbSql.AddCondition(string.Format("USERNAME LIKE '%{0}%'", username.ToUpper()));
                        sbCount.AddCondition(string.Format("USERNAME LIKE '%{0}%'", username.ToUpper()));
                    }
                    sbSql.AppendFormat(" LIMIT {0} OFFSET {1}",
                        limit, (page - 1) * limit);


                    int count = Convert.ToInt32(dal.ExecuteScalar(sbCount.ToString()));
                    var dt = dal.ExecuteQuery(sbSql.ToString());
                    ret.Data = JsonConvert.SerializeObject(new
                    {
                        status = 0,
                        message = "",
                        total = count,
                        data = dt
                    });
                }
            }
            catch (Exception ex)
            {
                ret.Data = JsonConvert.SerializeObject(new
                {
                    status = 1,
                    message = "发生异常：" + ex.Message,
                    total = 0,
                    data = ""
                });
            }
            return ret;
        }
    }
}