using SAPSyncdll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAPSyncdll
{

    /// <summary>
    /// 按照SAP所需同步的信息，包括项目基础信息，wbs结构信息，预算信息
    /// </summary>
    public class ProjectHelper
    {
        #region 获取项目信息
        /// <summary>
        /// 根据传入的项目，获取详细的项目信息，封装成项目实体类返回
        /// </summary>
        /// <param name="projectBinder"></param>
        /// <returns></returns>
        public Project GetProjectInfo(ProjectBinder projectBinder)
        {
            Project project = null;
            //根据传入的获取项目信息
            try
            {
               
                project = new Project(projectBinder);

                //获取项目的基础信息，也要通过app.config记录的匹配信息，获取指定字段的匹配信息
            }
            catch (Exception ex)
            {

                //  LogHelper.WriteLog($"项目初始化发生异常：" + ex.Message);
                LogHelper.Error($"项目初始化发生异常：" + ex.Message,ex);
            }
            return project;
        }
        #endregion

        #region 获取立项编号
        /// <summary>
        /// 根据ProjectID，SpaceID获取立项编号
        /// </summary>
        /// <param name="projectBinder"></param>
        /// <returns></returns>
        public int? GetProjectRequestTask(ProjectBinder projectBinder)
        {
            using (ABZG_DSEntities dbcontext=new ABZG_DSEntities() )
            {
                LogHelper.WriteLog("ready to get linked specid");
                int? linkedSpecID = (from b in dbcontext.Bug where b.ProjectID == projectBinder.ProjectID && b.SubProjectID == -(1500000001 + projectBinder.SpaceID) select b).SingleOrDefault<Bug>().LinkedSpecID;

                projectBinder.ProjectRequestID = -linkedSpecID;
                LogHelper.WriteLog("linked specid"+ projectBinder.ProjectRequestID);
                return projectBinder.ProjectRequestID;
            }
        }
        #endregion

        #region 获取项目的wbs信息
        /// <summary>
        /// 根据传入的项目，获取项目的wbs结构，封装成wbs泛型实体类
        /// </summary>
        /// <param name="projectBinder"></param>
        /// <returns></returns>
        public WBSInfo GetWBS(ProjectBinder projectBinder)
        {
            //获取项目下的链接的开发空间中的space下一层目录
            //根据app.config记录的名称和编号的匹配关系，封装成wbs
            WBSInfo wbs = null;
            //根据传入的获取项目信息
            try
            {
                wbs = new WBSInfo(projectBinder);

                //获取项目的基础信息，也要通过app.config记录的匹配信息，获取指定字段的匹配信息
            }
            catch (Exception ex)
            {

                //LogHelper.WriteLog($"wbs初始化发生异常：" + ex.Message);
                LogHelper.Error($"wbs初始化发生异常：" + ex.Message,ex);
            }
            return wbs;
        }
        #endregion

        /// <summary>
        /// 根据传入的项目，获取项目的预算信息，封装成预算实体类返回
        /// </summary>
        /// <param name="projectBinder"></param>
        /// <returns></returns>
        public ProjectBudget GetProjectBudget(ProjectBinder projectBinder)
        {
            //解析项目下指定类型任务中指定字段的值，把table格式解析成json
            return null;
        }
    }
}
