using SAPSyncdll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TESyncSAP;

namespace SAPConsole
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">参数数组，接收ProjectID,SpaceID</param>
        static void Main(string[] args)
        {
            DelegateHandler delegateHandler = new DelegateHandler();
            try
            {
                //接收参数，封装成实体类
                ProjectBinder projectBinder = new ProjectBinder()
                {
                    ProjectID = Convert.ToInt32(args[0]),
                    SpaceID = Convert.ToInt32(args[1]),
                    MyFlag = (Flag)Convert.ToInt32(args[2]),
                    ProjectRequestID= Convert.ToInt32(args[3])
                };

                LogHelper.WriteLog("当前接收到的的ProjectID：" + projectBinder.ProjectID);
                LogHelper.WriteLog("当前接收到的的SpaceID：" + projectBinder.SpaceID);
                LogHelper.WriteLog("当前接收到的的更新标志是：" + projectBinder.MyFlag.ToString());
                LogHelper.WriteLog("当前接收到的的立项任务是：" + projectBinder.ProjectRequestID.ToString());

                if ((int)projectBinder.MyFlag == 0)//新建项目
                {
                    SyncProject2SAP(projectBinder, delegateHandler.SyncProjectInfo);
                }
                else if ((int)projectBinder.MyFlag == 1)//项目状态流转 技术性延迟
                {
                    SyncProject2SAP(projectBinder, delegateHandler.SyncProjectInfo);
                }
                else if ((int)projectBinder.MyFlag == 2)//项目状态流转时 关闭
                {
                    SyncProject2SAP(projectBinder, delegateHandler.SyncProjectInfo);
                }
                else if ((int)projectBinder.MyFlag == 4)//工时
                {
                    //3.委托绑定
                    SyncProject2SAP(projectBinder, delegateHandler.SyncProjectEstimate);
                }




                ////1. 获取立项生成项目后触发的ProjectID 和 SubProjectID
                //ProjectHelper projectHelper = new ProjectHelper();

                ////2. 根据生成的项目，获取项目的信息，封装到项目信息实体类中
                //Project project = projectHelper.GetProjectInfo(projectBinder);//项目信息
                //WBSInfo wbs = projectHelper.GetWBS(projectBinder);//wbs结构
                //                                                  //ProjectBudget projectBudget = projectHelper.GetProjectBudget(projectBinder);//项目预算
                //                                                  //2.1 根据。。名称获取。。编号

                //try
                //{
                //    LogHelper.WriteLog($"当前项目基础信息为以下：--------------------");
                //    LogHelper.WriteLog($"项目名称：{ project.ProjectName}");
                //    LogHelper.WriteLog($"项目编码：{ project.ProjectUniqueID}");
                //    LogHelper.WriteLog($"项目类型：{ project.ProjectType}");
                //    LogHelper.WriteLog($"项目类型对应的编码：{ project.ProjectTypeID}");
                //    LogHelper.WriteLog($"项目经理：{ project.ProjectManager}");
                //    LogHelper.WriteLog($"项目代号：{ project.ProjectCodeName}");
                //    LogHelper.WriteLog($"委托部门：{ project.DelegateDepartment}");
                //    LogHelper.WriteLog($"承担部门：{ project.UnderTaskDepartment}");
                //    LogHelper.WriteLog($"项目复杂度等级：{ project.ProjectComplexLevel}");
                //    LogHelper.WriteLog($"项目优先级：{ project.ProjectPriority}");
                //    LogHelper.WriteLog($"项目状态：{ project.ProjectStatus}");
                //    LogHelper.WriteLog($"当前项目基础信息截止：--------------------");
                //    LogHelper.WriteLog("                        ");
                //    LogHelper.WriteLog($"当前项目wbs信息为以下：--------------------");
                //    LogHelper.WriteLog($"项目编码：{ wbs.ProjectUniqueID}");
                //    foreach (var item in wbs.WbsFolders)
                //    {
                //        LogHelper.WriteLog($"WBS元素名称：{ item.WBSName}");
                //        LogHelper.WriteLog($"WBS元素编码：{ item.WBSUniqueNo}");
                //    }

                //    LogHelper.WriteLog($"当前项目wbs信息截止：--------------------");
                //    LogHelper.WriteLog("                        ");



                //    LogHelper.WriteLog($"当前里程碑的子目录信息为以下：--------------------");
                //    //调用工时帮助类实现返回带有工时信息的wbs元素信息
                //    TimeHelper timeHelper = new TimeHelper(wbs);

                //    //调用获取子目录方法，返回每个wbs元素对应下的所有子目录
                //    Dictionary<string, List<int>> subFolders = timeHelper.GetAllChildFolderIds(wbs);

                //    //输出每个wbs元素信息，以及对应的子目录节点id，以及工时统计，按人员分组
                //    foreach (var item in wbs.WbsFolders)
                //    {
                //        LogHelper.WriteLog($"当前wbs元素编码的信息为：--------------------");
                //        LogHelper.WriteLog($"WBS元素编码：{ item.WBSUniqueNo}");
                //        LogHelper.WriteLog($"WBS所有子目录为：");
                //        foreach (var item4 in subFolders[item.WBSUniqueNo])
                //        {
                //            LogHelper.WriteLog($"当前WBS元素的子目录为：{item4.ToString()}");
                //        }
                //        LogHelper.WriteLog($"-------------当前wbs元素工时信息为：--------------------");
                //        foreach (var item2 in item.TimeCosts)
                //        {
                //            LogHelper.WriteLog($"-----------------当前人员编号为：{item2.PersonID}");
                //            LogHelper.WriteLog($"-----------------当前人员登录名为：{item2.LoginName}");
                //            LogHelper.WriteLog($"-----------------统计的月份为：{item2.Month}");
                //            LogHelper.WriteLog($"-----------------总计工时为：{item2.TotalTime}");
                //        }
                //    }
                //    LogHelper.WriteLog($"当前里程碑的子目录信息截止：--------------------");

                //    //调用SAP RFC函数执行。。。
                //    SAPSync sapSync = new SAPSync();
                //    sapSync.RegisterDestination();
                //    sapSync.InvokeRFCFunctionZPSRFC03(project,wbs);

                //}
                //catch (Exception ex)
                //{

                //    LogHelper.WriteLog("同步出现异常：" + ex.Message);
                //}

            }
            catch (Exception ex)
            {
                // LogHelper.WriteLog("参数出错，执行报错：" + ex.Message);
                LogHelper.Error("参数出错，执行报错："+ex.Message ,ex);
            }


        }

        //2. 委托变量
        static void SyncProject2SAP(ProjectBinder projectBinder, DelegateSyncSAP delegateSyncSAP)
        {
            delegateSyncSAP(projectBinder);//4. 委托调用
        }
    }
}
