using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAPSyncdll.Models
{
    /// <summary>
    /// 部门项目类，用于记录当时部门项目时，需要去DevTime统计项目人员工时
    /// </summary>
   public class InternalProject
    {
        public int DevProjectID { get; set; }
        public int DevSpaceID { get; set; }
    }
}
