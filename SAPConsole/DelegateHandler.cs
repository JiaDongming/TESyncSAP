using SAPSyncdll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TESyncSAP
{
    //1. 声明委托
 public delegate void DelegateSyncSAP(ProjectBinder projectBinder);

    
    public  class DelegateHandler
    {
        #region 新建项目时的执行逻辑
        //新建项目
        public void SyncProjectInfo(ProjectBinder projectBinder)
        {
            Project project = null;
            WBSInfo wbs = null;
            try
            {
               
                //1. 获取立项生成项目后触发的ProjectID 和 SubProjectID
                ProjectHelper projectHelper = new ProjectHelper();

                //2. 根据生成的项目，获取项目的信息，封装到项目信息实体类中
                project = projectHelper.GetProjectInfo(projectBinder);//项目信息
                if (project==null)
                {
                    //  LogHelper.WriteLog("Init Project error, please check it");
                    LogHelper.Error("项目信息初始化异常", new Exception("获取项目的基础信息出错"));
                    return;
                }
                 wbs = projectHelper.GetWBS(projectBinder);//wbs结构
                                                                  //ProjectBudget projectBudget = projectHelper.GetProjectBudget(projectBinder);//项目预算
                                                                  //2.1 根据。。名称获取。。编号

                LogHelper.WriteLog($"当前项目基础信息为以下：--------------------");
                LogHelper.WriteLog($"项目名称：{ project.ProjectName}");
                LogHelper.WriteLog($"项目编码：{ project.ProjectUniqueID}");
                LogHelper.WriteLog($"项目类型：{ project.ProjectType}");
                LogHelper.WriteLog($"项目类型对应的编码：{ project.ProjectTypeID}");
                LogHelper.WriteLog($"项目经理：{ project.ProjectManager}");
                LogHelper.WriteLog($"项目代号：{ project.ProjectCodeName}");
                LogHelper.WriteLog($"委托部门：{ project.DelegateDepartment}");
                LogHelper.WriteLog($"承担部门：{ project.UnderTaskDepartment}");
                LogHelper.WriteLog($"项目复杂度等级：{ project.ProjectComplexLevel}");
                LogHelper.WriteLog($"项目优先级：{ project.ProjectPriority}");
                LogHelper.WriteLog($"项目状态：{ project.ProjectStatus}");
                LogHelper.WriteLog($"当前项目基础信息截止：--------------------");
                LogHelper.WriteLog("                        ");
                LogHelper.WriteLog($"当前项目wbs信息为以下：--------------------");
                LogHelper.WriteLog($"项目编码：{ wbs.ProjectUniqueID}");
                foreach (var item in wbs.WbsFolders)
                {
                    LogHelper.WriteLog($"WBS元素名称：{ item.WBSName}");
                    LogHelper.WriteLog($"WBS元素编码：{ item.WBSUniqueNo}");
                }

                LogHelper.WriteLog($"当前项目wbs信息截止：--------------------");
                LogHelper.WriteLog("                        ");



                LogHelper.WriteLog($"当前里程碑的子目录信息为以下：--------------------");

              
            }
            catch (Exception ex)
            {

                //LogHelper.WriteLog("获取项目信息和wbs信息出错："+ex.Message); ;
                LogHelper.Error("获取项目信息和wbs信息出错：" + ex.Message,ex); ;
            }

            //调用SAP RFC函数执行。。。
            SAPSync sapSync = new SAPSync();
            sapSync.RegisterDestination();
            sapSync.InvokeRFCFunctionZPSRFC03(project, wbs);
        }

        #endregion

        //同步状态
        public void SyncProjectStatus(ProjectBinder projectBinder)
        {
        }

        //同步工时
        public void SyncProjectEstimate(ProjectBinder projectBinder)
        {

            WBSInfo wbs = null;
            try
            {
                //1. 获取立项生成项目后触发的ProjectID 和 SubProjectID
                ProjectHelper projectHelper = new ProjectHelper();

                //2. 获取wbs信息

                wbs = projectHelper.GetWBS(projectBinder);//wbs结构
                                                          //ProjectBudget projectBudget = projectHelper.GetProjectBudget(projectBinder);//项目预算
                                                          //2.1 根据。。名称获取。。编号


               

                TimeHelper timeHelper = new TimeHelper(wbs);

                LogHelper.WriteLog($"当前项目wbs信息为以下：--------------------");
                LogHelper.WriteLog($"项目编码：{ wbs.ProjectUniqueID}");
                foreach (var item in wbs.WbsFolders)
                {
                    LogHelper.WriteLog($"WBS元素名称：{ item.WBSName}");
                    LogHelper.WriteLog($"WBS元素编码：{ item.WBSUniqueNo}");
                }

                LogHelper.WriteLog($"当前项目wbs信息截止：--------------------");
                LogHelper.WriteLog("                        ");

                //调用获取子目录方法，返回每个wbs元素对应下的所有子目录
                Dictionary<string, List<int>> subFolders = timeHelper.GetAllChildFolderIds(wbs);

                //输出每个wbs元素信息，以及对应的子目录节点id，以及工时统计，按人员分组
                foreach (var item in wbs.WbsFolders)
                {
                    LogHelper.WriteLog($"当前wbs元素编码的信息为：--------------------");
                    LogHelper.WriteLog($"WBS元素编码：{ item.WBSUniqueNo}");
                    //LogHelper.WriteLog($"WBS所有子目录为：");
                    //foreach (var item4 in subFolders[item.WBSUniqueNo])
                    //{
                    //    LogHelper.WriteLog($"当前WBS元素的子目录为：{item4.ToString()}");
                    //}
                    LogHelper.WriteLog($"-------------当前wbs元素工时信息为：--------------------");
                    foreach (var item2 in item.TimeCosts)
                    {
                        LogHelper.WriteLog($"-----------------当前人员编号为：{item2.PersonID}");
                        LogHelper.WriteLog($"-----------------当前人员登录名为：{item2.LoginName}");
                        LogHelper.WriteLog($"-----------------统计的月份为：{item2.Month}");
                        LogHelper.WriteLog($"-----------------统计的年为：{item2.Yeart}");
                        LogHelper.WriteLog($"-----------------统计的月为：{item2.Montht}");
                        LogHelper.WriteLog($"-----------------总计工时为：{item2.TotalTime}");
                    }
                }


            }
            catch (Exception ex)
            {

                // LogHelper.WriteLog("获取wbs信息出错：" + ex.Message); 
                LogHelper.Error("获取wbs信息出错：" + ex.Message,ex);
            }

            //调用SAP RFC函数执行。。。
            SAPSync sapSync = new SAPSync();
            sapSync.RegisterDestination();
            sapSync.InvokeRFCFunctionZPSRFC04(wbs);

        }
    }
}
