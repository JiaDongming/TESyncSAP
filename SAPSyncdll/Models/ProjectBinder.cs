using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAPSyncdll
{

    public class ProjectBinder
    {
        public int ProjectID { get; set; }
        public int SpaceID { get; set; }

        public int? ProjectRequestID { get; set; }

        public Flag MyFlag { get; set; }

    }

    public enum Flag//标志位，用于标志当前是插入，还是更新 项目
    {
        insert = 0, //项目生成时
        delay = 1,//项目状态流转时
        close = 2,//项目状态流转时
        time=4//工时



    }

}
