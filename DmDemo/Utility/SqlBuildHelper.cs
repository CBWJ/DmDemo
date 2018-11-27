﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace DmDemo.Utility
{
    public static class SqlBuildHelper
    {
        public static void AddConditionPrefix(this StringBuilder sbSql)
        {
            //排除子查询
            var sql = sbSql.ToString();
            sql = Regex.Replace(sql, @"\([\s\S]+WHERE[\s\S]+\)", "");

            if (sql.Contains("WHERE"))
                sbSql.Append(" AND");
            else
                sbSql.Append(" WHERE");
        }

        public static void AddCondition(this StringBuilder sbSql, string conditon)
        {
            sbSql.AddConditionPrefix();
            sbSql.Append(" ");
            sbSql.Append(conditon);
        }
    }
}