using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dm;
using DMAccess;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using DmDemo.Utility;
using System.Data;


namespace DmDemo.Controllers
{
    public class AuditSettingController : Controller
    {
        // GET: AuditSetting
        public ActionResult Index()
        {
            return View();
        }

        // GET: AuditSetting/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuditSetting/Create
        public ActionResult CreateStatement()
        {
            string[] arrStatement = {"ALL","USER","ROLE","TABLESPACE","SCHEMA","TABLE","VIEW","INDEX",
                    "PROCEDURE","TRIGGER","SEQUENCE","CONTEXT","SYNONYM","GRANT",
                    "REVOKE","AUDIT","NOAUDIT","INSERT TABLE","UPDATE TABLE",
                    "DELETE TABLE","SELECT TABLE","EXECUTE","PROCEDURE","PACKAGE",
                    "PACKAGE BODY","MAC POLICY","MAC LEVEL","MAC COMPARTMENT",
                    "MAC GROUP","MAC LABEL","MAC USER","MAC TABLE","MAC SESSION",
                    "CHECKPOINT","SAVEPOINT","EXPLAIN","NOT EXIST","DATABASE",
                    "CONNECT","COMMIT","ROLLBACK","SET TRANSACTION"};
            ViewBag.STMT = arrStatement;

            using (var dal = new DmDAL())
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT NAME FROM sysobjects WHERE TYPE$='UR' AND SUBTYPE$='USER'");                
                
                var dt = dal.ExecuteQuery(sbSql.ToString());

                List<string> users = new List<string>();
                foreach(DataRow row in dt.Rows)
                {
                    users.Add(row[0].ToString());
                }
                ViewBag.Users = users;

            }
            return View();
        }

        // POST: AuditSetting/Create
        [HttpPost]
        public ActionResult CreateStatement(string type, string username, string whenever)
        {
            JsonResult ret = new JsonResult();
            try
            {
                using (var dal = new DmDAL())
                {

                    StringBuilder sbSql = new StringBuilder();
                    //sbSql.Append("SP_AUDIT_STMT");
                    //dal.ExecuteProcedureNonQuery(sbSql.ToString(),
                    //    new DmParameter("TYPE", type),
                    //    new DmParameter("USERNAME", username),
                    //    new DmParameter("WHENEVER", whenever));
                    sbSql.AppendFormat("SP_AUDIT_STMT('{0}', '{1}', '{2}')",
                        type, username, whenever);
                    dal.ExecuteNonQuery(sbSql.ToString());
                }
                ret.Data = JsonConvert.SerializeObject(new
                {
                    status = 0,
                    message = ""
                });
            }
            catch(Exception ex)
            {
                ret.Data= JsonConvert.SerializeObject(new {
                    status = 1,
                    message = "发生异常：" + ex.Message
                });
            }
            return ret;
        }

        public ActionResult CreateObject()
        {
            string[] arrStatement = { "ALL", "INSERT", "UPDATE", "DELETE", "SELECT", "EXECUTE", "MERGE INTO", "EXECUTE TRIGGER", "LOCK TABLE" };
            ViewBag.Operation = arrStatement;

            using (var dal = new DmDAL())
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT NAME FROM sysobjects WHERE TYPE$='UR' AND SUBTYPE$='USER'");

                var dt = dal.ExecuteQuery(sbSql.ToString());

                List<string> users = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    users.Add(row[0].ToString());
                }
                ViewBag.Users = users;

            }
            return View();
        }
        [HttpPost]
        public ActionResult CreateObject(   string type,
                                            string username,
                                            string tvname,
                                            string colname,
                                            string whenever)
        {
            JsonResult ret = new JsonResult();
            try
            {
                using (var dal = new DmDAL())
                {

                    StringBuilder sbSql = new StringBuilder();
                    sbSql.AppendFormat("SP_AUDIT_OBJECT('{0}', '{1}', '{2}', '{3}'",
                        type, username, username, tvname);
                    if (!string.IsNullOrWhiteSpace(colname))
                    {
                        sbSql.AppendFormat(",'{0}'", colname);
                    }
                    sbSql.AppendFormat(",'{0}')", whenever);
                    dal.ExecuteNonQuery(sbSql.ToString());
                }
                ret.Data = JsonConvert.SerializeObject(new
                {
                    status = 0,
                    message = ""
                });
            }
            catch (Exception ex)
            {
                ret.Data = JsonConvert.SerializeObject(new
                {
                    status = 1,
                    message = "发生异常：" + ex.Message
                });
            }
            return ret;
        }

        // GET: AuditSetting/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuditSetting/Edit/5
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

        // GET: AuditSetting/Delete/5
        public ActionResult DeleteStatement(int id)
        {
            return View();
        }

        // POST: AuditSetting/Delete/5
        [HttpPost]
        public ActionResult DeleteStatement(string type, string username, string whenever)
        {
            JsonResult ret = new JsonResult();
            try
            {
                using (var dal = new DmDAL())
                {

                    StringBuilder sbSql = new StringBuilder();
                    sbSql.AppendFormat("SP_NOAUDIT_STMT('{0}', '{1}', '{2}')",
                        type, string.IsNullOrWhiteSpace(username) ? "NULL" : username, whenever);
                    dal.ExecuteNonQuery(sbSql.ToString());
                }
                ret.Data = JsonConvert.SerializeObject(new
                {
                    status = 0,
                    message = ""
                });
            }
            catch (Exception ex)
            {
                ret.Data = JsonConvert.SerializeObject(new
                {
                    status = 1,
                    message = "发生异常：" + ex.Message
                });
            }
            return ret;
        }
        [HttpPost]
        public ActionResult DeleteObject(string type,
                                            string username,
                                            string tvname,
                                            string colname,
                                            string whenever)
        {
            JsonResult ret = new JsonResult();
            try
            {
                using (var dal = new DmDAL())
                {

                    StringBuilder sbSql = new StringBuilder();
                    sbSql.AppendFormat("SP_NOAUDIT_OBJECT('{0}', '{1}', '{2}', '{3}'",
                        type, username, username, tvname);
                    if (!string.IsNullOrWhiteSpace(colname))
                    {
                        sbSql.AppendFormat(",'{0}'", colname);
                    }
                    sbSql.AppendFormat(",'{0}')", whenever);
                    dal.ExecuteNonQuery(sbSql.ToString());
                }
                ret.Data = JsonConvert.SerializeObject(new
                {
                    status = 0,
                    message = ""
                });
            }
            catch (Exception ex)
            {
                ret.Data = JsonConvert.SerializeObject(new
                {
                    status = 1,
                    message = "发生异常：" + ex.Message
                });
            }
            return ret;
        }
        public ActionResult List(int page= 1, int limit = 30, string level="")
        {
            JsonResult ret = new JsonResult();
            ret.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            try
            {
                using (var dal = new DmDAL())
                {
                    StringBuilder sbSql = new StringBuilder();
                    StringBuilder sbCount = new StringBuilder();

                    sbCount.Append("SELECT COUNT(*) FROM SYSAUDITOR.SYSAUDIT");
                    sbSql.Append(@"SELECT ad.*,
                                    obj1.NAME AS USERNAME,
                                    obj2.NAME AS OBJECTNAME,
                                    (SELECT NAME FROM SYSCOLUMNS WHERE ID = ad.TVPID AND COLID = ad.COLID) AS COLNAME,
                                    (CASE LEVEL WHEN 1 THEN '语句级' WHEN 2 THEN '对象级' ELSE '' END) AS SLEVEL,
                                    (CASE TYPE 	WHEN 0 THEN 'ALL'
	                                    WHEN 12 THEN 'USER'
	                                    WHEN 13 THEN 'ROLE'
	                                    WHEN 9 THEN 'TABLESPACE'
	                                    WHEN 14 THEN 'SCHEMA'
	                                    WHEN 15 THEN 'TABLE'
	                                    WHEN 16 THEN 'VIEW'
	                                    WHEN 17 THEN 'INDEX'
	                                    WHEN 18 THEN 'PROCEDURE'
	                                    WHEN 19 THEN 'TRIGGER'
	                                    WHEN 20 THEN 'SEQUENCE'
	                                    WHEN 21 THEN 'CONTEXT'
	                                    WHEN 26 THEN 'SYNONYM'
	                                    WHEN 22 THEN 'GRANT'
	                                    WHEN 23 THEN 'REVOKE'
	                                    WHEN 24 THEN 'AUDIT'
	                                    WHEN 25 THEN 'NOAUDIT'
	                                    WHEN 30 THEN 'INSERT TABLE'
	                                    WHEN 33 THEN 'UPDATE TABLE'
	                                    WHEN 32 THEN 'DELETE TABLE'
	                                    WHEN 31 THEN 'SELECT TABLE'
	                                    WHEN 18 THEN 'PROCEDURE'
	                                    WHEN 44 THEN 'PACKAGE'
	                                    WHEN 45 THEN 'PACKAGE BODY'
	                                    WHEN 34 THEN 'MAC POLICY'
	                                    WHEN 35 THEN 'MAC LEVEL'
	                                    WHEN 36 THEN 'MAC COMPARTMENT'
	                                    WHEN 37 THEN 'MAC GROUP'
	                                    WHEN 38 THEN 'MAC LABEL'
	                                    WHEN 40 THEN 'MAC USER'
	                                    WHEN 41 THEN 'MAC TABLE'
	                                    WHEN 39 THEN 'MAC SESSION'
	                                    WHEN 28 THEN 'CHECKPOINT'
	                                    WHEN 75 THEN 'SAVEPOINT'
	                                    WHEN 76 THEN 'EXPLAIN'
	                                    WHEN 77 THEN 'NOT EXIST'
	                                    WHEN 70 THEN 'DATABASE'
	                                    WHEN 74 THEN 'CONNECT'
	                                    WHEN 72 THEN 'COMMIT'
	                                    WHEN 73 THEN 'ROLLBACK'
	                                    WHEN 43 THEN 'SET TRANSACTION'
                                        WHEN 50 THEN 'INSERT'
	                                    WHEN 53 THEN 'UPDATE'
	                                    WHEN 52 THEN 'DELETE'
	                                    WHEN 51 THEN 'SELECT'
	                                    WHEN 54 THEN 'EXECUTE'
	                                    WHEN 56 THEN 'MERGE INTO'
	                                    WHEN 55 THEN 'EXECUTE TRIGGER'
	                                    WHEN 57 THEN 'LOCK TABLE'
	                                    ELSE '' END) AS STYPE,
                                    '' AS SWHENEVER
                                    FROM SYSAUDITOR.SYSAUDIT ad
                                    LEFT OUTER JOIN sysobjects obj1 
                                    ON obj1.ID = ad.UID
                                    LEFT OUTER JOIN sysobjects obj2
                                    ON obj2.ID = ad.TVPID");
                    if (!string.IsNullOrWhiteSpace(level))
                    {
                        int iLevel = 0;
                        if ("语句级".Contains(level)) iLevel = 1;
                        else if ("对象级".Contains(level)) iLevel = 2;
                        sbSql.AddCondition(string.Format("LEVEL = {0}", iLevel));
                        sbCount.AddCondition(string.Format("LEVEL = {0}", iLevel));
                    }
                    sbSql.AppendFormat(" LIMIT {0} OFFSET {1}",
                        limit, (page - 1) * limit);


                    int count = Convert.ToInt32(dal.ExecuteScalar(sbCount.ToString()));
                    var dt = dal.ExecuteQuery(sbSql.ToString());
                    foreach(DataRow dr in dt.Rows)
                    {
                        var when = Convert.ToInt32(dr["WHENEVER"]);
                        string sWhen = "";
                        switch (when)
                        {
                            case 1:
                                sWhen = "SUCCESSFUL";
                                break;
                            case 2:
                                sWhen = "FAIL";
                                break;
                            case 3:
                                sWhen = "ALL";
                                break;
                        }
                        dr["SWHENEVER"] = sWhen;
                    }
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
