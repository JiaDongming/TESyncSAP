using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SAPSyncdll
{
  public  class Project
    {

        public int BaseProjectID { get; set; }//PPM基础项目编号
        public int ProjectSpaceID { get; set; }//PPM SpaceID 
        public string ProjectUniqueID { get; set; }//项目编码
        public string ProjectName { get; set; }//项目名称 

        public string ProjectTypeID { get; set; }//项目类型对应的编码
        public string ProjectType { get; set; }//项目类型
       //  public string ProjectManager { get; set; }//项目经理
        public int ProjectManager { get; set; }//项目经理
        public string ProjectCodeName { get; set; }//项目代号

        public string DelegateDepartment { get; set; }//委托部门
        public string UnderTaskDepartment { get; set; }//承担部门
        public string ProjectComplexLevel { get; set; }//项目复杂度等级
        public string ProjectPriority { get; set; }//项目优先级
        public string ProjectStatus { get; set; }//项目状态

        //保存项目类型名称和对应编码
        Dictionary<string, string> code4ProjectType = new Dictionary<string, string>();
        //保存部门名称和对应编码
        Dictionary<string, string> code4Depart = new Dictionary<string, string>();

        //构造函数实现按照指定的项目SpaceID获取具体的项目信息
        public Project(ProjectBinder projectBinder)
        {
            using (ABZG_DSEntities dbcontext = new ABZG_DSEntities())
            {
                
                //立项任务编号
                //int? BugID = new ProjectHelper().GetProjectRequestTask(projectBinder);
                int? BugID = projectBinder.ProjectRequestID;

               
                 //指定立项任务存储在表CustomerFieldTrackExt中的信息集合
                 var customerFieldTrackExt = (from c in dbcontext.CustomerFieldTrackExt where c.ProjectID == projectBinder.ProjectID && c.BugID == BugID select c).SingleOrDefault<CustomerFieldTrackExt>();
                //指定SpaceID所在的表Subproject的单条数据
                var subproject = (from p in dbcontext.SubProject where p.ProjectID == projectBinder.ProjectID && p.SubProjectID == projectBinder.SpaceID select p).SingleOrDefault<SubProject>();

                //项目类型和编码的字典集合
                code4ProjectType = (from code in dbcontext.CustomerFieldListValue where code.ProjectID == projectBinder.ProjectID && code.CustomFieldID == 505 select code).ToDictionary(k => k.ChoiceName.Split(' ')[0], v => v.ChoiceName.Split(' ')[1]);
                //部门类型和编码的字典集合
                code4Depart = (from code in dbcontext.CustomerFieldListValue where code.ProjectID == projectBinder.ProjectID && code.CustomFieldID == 503 select code).ToDictionary(k => k.ChoiceName.Split(' ')[0], v => v.ChoiceName.Split(' ')[1]);

                //用户表集合
                var login = (from l in dbcontext.LogIn select l);

                //获取项目经理对应的personID
                var bugSelection = (from b in dbcontext.BugSelectionInfo select b);
                int loginID = (from c in bugSelection where c.ProjectID == projectBinder.ProjectID && c.BugID == BugID && c.FieldID == 7 select c.FieldSelectionID).SingleOrDefault();

                //-------------以下是给项目属性赋值---------------------------
                //PPM基础项目编号
                BaseProjectID = projectBinder.ProjectID;
                ProjectSpaceID = projectBinder.SpaceID;
                //项目编码
                ProjectUniqueID = customerFieldTrackExt.Desc_Custom_1;
                //项目名称 
                ProjectName = subproject.Title;
                //项目类型
                ProjectType = customerFieldTrackExt.Desc_Custom_3 is null?"": customerFieldTrackExt.Desc_Custom_3;
                //项目类型对应的编码
                if (code4ProjectType.ContainsKey(ProjectType))
                {
                    ProjectTypeID = code4ProjectType[ProjectType];
                };

                //项目经理 need improve it..
                // ProjectManager = Convert.ToInt32( (from c in login where c.PersonID == loginID select c.Login1).SingleOrDefault());
                ProjectManager = 00000002;
                //项目代号
                ProjectCodeName = subproject.Title;

                //委托部门名称
                var code4DepartName = customerFieldTrackExt.Custom_4 == null ? "" : customerFieldTrackExt.Custom_4;

                //委托部门
                if (!(code4DepartName ==""))
                {
                    if (code4Depart.ContainsKey(customerFieldTrackExt.Custom_4))
                    {
                        DelegateDepartment = code4Depart[customerFieldTrackExt.Custom_4];
                    };
                }
                else
                    DelegateDepartment = "";

                //承担部门
                if (code4Depart.ContainsKey(customerFieldTrackExt.Custom_6))
                {
                    UnderTaskDepartment = code4Depart[customerFieldTrackExt.Custom_6];
                };
                //项目复杂度等级
                ProjectComplexLevel = customerFieldTrackExt.Custom_10;
                //项目优先级
                ProjectPriority = customerFieldTrackExt.Custom_11;
                //项目状态
                switch (projectBinder.MyFlag)
                {
                    case Flag.insert:
                        ProjectStatus = "下达";
                        break;
                    case Flag.delay:
                        ProjectStatus = "暂停";
                        break;
                    case Flag.close:
                        ProjectStatus = "关闭";
                        break;
                    default:
                        ProjectStatus = "";
                        break;
                }

            }

        }


    }
}

