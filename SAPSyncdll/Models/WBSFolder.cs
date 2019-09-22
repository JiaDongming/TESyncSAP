using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SAPSyncdll
{

    /// <summary>
    /// 保存需要同步到SAP的WBS类
    /// </summary>
    public class WBSInfo
    {
        public int DevProjectID { get; set; }//开发基础项目编号
        public int DevSpaceID { get; set; }//开发space id
        public string ProjectUniqueID { get; set; }//项目编码

        public string ProjectType { get; set; }//PPM项目类型-----

        public List<WBSFolder> WbsFolders { get; set; }

        //保存wbs名称和对应编码
       Dictionary<string, string> code4Wbs = new Dictionary<string, string>();

        public WBSInfo(ProjectBinder projectBinder)
        {
            //wbs编码赋值，初始化wbs编码字典，后续这里通过获取表的值，分析得到
            //code4Wbs = new Dictionary<string, string>()
            //    {
            //        { "TR1/TR2","01"},
            //        { "TR3","02"},
            //        { "TR4A","03"},
            //        { "TR5","04"},
            //        { "TR6","05"},
            //        { "概念与计划阶段","06"},
            //        { "开发阶段","07"},
            //        { "验证阶段","08"},
            //        { "结项阶段","09"},
            //        { "立项准备","10"}
            //    };


            using (ABZG_DSEntities dbcontext = new ABZG_DSEntities())
            {
              //  code4Wbs = (from code in dbcontext.CustomerFieldListValue where code.ProjectID == projectBinder.ProjectID && code.CustomFieldID == 508 select code).ToDictionary(k => k.ChoiceName.Split(' ')[0], v => v.ChoiceName.Split(' ')[1]);
                //立项任务编号
                // int? BugID = new ProjectHelper().GetProjectRequestTask(projectBinder);
                int? BugID = projectBinder.ProjectRequestID;
                //指定立项任务存储在表CustomerFieldTrackExt中的信息集合
                var customerFieldTrackExt = (from c in dbcontext.CustomerFieldTrackExt where c.ProjectID == projectBinder.ProjectID && c.BugID == BugID select c).SingleOrDefault<CustomerFieldTrackExt>();

                var spaceLink = (from link in dbcontext.SpaceLink where link.ProjectID == projectBinder.ProjectID && link.ProjectTypeID == 41 && link.SpaceID == projectBinder.SpaceID select link);

                //项目类型------
                ProjectType = customerFieldTrackExt.Desc_Custom_3 is null ? "" : customerFieldTrackExt.Desc_Custom_3;

                //项目编码
                ProjectUniqueID = customerFieldTrackExt.Desc_Custom_1;

                if (spaceLink.Where(s => s.ToProjectTypeID == 1).Count() > 0)
                {
                    DevProjectID = spaceLink.Where(s => s.ToProjectTypeID == 1).SingleOrDefault().ToProjectID;
                    DevSpaceID = spaceLink.Where(s => s.ToProjectTypeID == 1).SingleOrDefault().ToSpaceID;
                    //把sql的in查询用linq的嵌套查询来实现
                    //第一步：获取项目下的child folder  id列表
                    var releaseIDList = from f in dbcontext.SubProjectTree where f.ProjectID == DevProjectID && f.ParentID == DevSpaceID select f.ChildID;

                    //第二步：获取所有的sub folders
                    // var allreleaseFolders = from sub in dbcontext.SubProject where sub.ProjectID == DevProjectID && sub.SubProjectType == 99 select sub;
                    var allreleaseFolders = from sub in dbcontext.SubProject where sub.ProjectID == DevProjectID && sub.SubProjectType!=2002
                                            select sub;

                    //第三步：获取对应项目下的满足的folders
                    var releaseFolders = from s in allreleaseFolders where releaseIDList.Contains(s.SubProjectID) select new { s.Title, s.ProjectID, s.SubProjectID };

                    WbsFolders = new List<WBSFolder>();
                    if (releaseFolders.Count() > 0)
                    {
                        foreach (var item in releaseFolders)
                        {
                            WbsFolders.Add(new WBSFolder()
                            {
                                WBSFolderID = item.SubProjectID,
                                WBSName = item.Title,
                                // WBSUniqueNo = code4Wbs.ContainsKey(item.Title) ? code4Wbs[item.Title] : "NoneMatchCode"
                                WBSUniqueNo = item.SubProjectID.ToString()
                            });
                        }
                    }
                    else
                    {

                    }
                };
            }

        }

        //select ToProjectID,ToSpaceID from SpaceLink where projectid=502 and projecttypeid=41 and spaceid=19915 and ToProjectTypeID=1 --获取项目对应的开发空间
        //select SubProjectType,* from SubProject where projectid=563 and subprojectid in (select ChildID from SubProjectTree where ProjectID=563 and ParentID=19922) and SubProjectType=99
    }

    public class WBSFolder
    {
        public int WBSFolderID { get; set; }
        /// <summary>
        /// 本级WBS元素名称
        /// </summary>
        public string WBSName { get; set; }
        /// <summary>
        /// 本级WBS元素编码
        /// </summary>
        public string WBSUniqueNo { get; set; }
        /// <summary>
        /// 当前阶段（release中的）
        /// </summary>
        public List<MonthCostByOwner> TimeCosts { get; set; }
    }

    /// <summary>
    /// 实体类，保存每个release节点下每个人的时间花费
    /// </summary>
    public class MonthCostByOwner
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        public int PersonID { get; set; }
        /// <summary>
        /// 员工登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 年	月
        /// </summary>
        public DateTime Month { get; set; }
        public int Yeart { get; set; }//年
        public int Montht { get; set; }//月
        /// <summary>
        /// 实际工时总数（h)
        /// </summary>
        public double? TotalTime { get; set; }
    }

    public class Folder
    {
        public int ProjectID { get; set; }
        public int FolderID { get; set; }
    }
}
