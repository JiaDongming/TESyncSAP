using SAPSyncdll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPSyncdll.Models;

namespace SAPSyncdll
{
    

    public class TimeHelper
    {

        //调用内部获取每个wbs元素对应的所有子目录字典
        Dictionary<string, List<int>> ReleaseFolders = new Dictionary<string, List<int>>();

        /// <summary>
        /// 工时统计构造方法，实现调用后，返回带有工时信息的wbs信息
        /// </summary>
        /// <param name="wbsInfo"></param>
        public TimeHelper(WBSInfo wbsInfo)
        {
         
            ReleaseFolders = GetAllChildFolderIds(wbsInfo);

            //用来保存哪些部门项目 项目名称，开发项目编号
            Dictionary<string, int> InteralDevProject = new Dictionary<string, int>();

            //写死特殊需要处理的部门项目
            InteralDevProject.Add("通用产品事业部部门项目", 21493);//。。。继续写死其他开发部门项目

            //GetCostByOwner(wbsInfo);//调用获取工时方法，实现补全wbs信息
            //GetDevTimeTaskCostByOwner(wbsInfo);//获取devtime表单中已审核过任务的工时
       
            if (InteralDevProject.ContainsValue(wbsInfo.DevSpaceID))
            {
                GetBuMenTaskCostByOwner(wbsInfo);
                GetDevTimeCostByOwner(wbsInfo);//获取DevTime工单审批后的公用时间数据插入到部门项目信息中
            }
            else
                GetDevTimeTaskCostByOwner(wbsInfo);//获取devtime表单中已审核过任务的工时

        }

        private void GetBuMenTaskCostByOwner(WBSInfo wbsInfo)
        {

            using (ABZG_DSEntities dbcontent = new ABZG_DSEntities())
            {
                // 定位时间段
                //取上月第一天
                DateTime checkMonthFrom = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01 00:00:00.000")).AddMonths(-1);
                //上月最后一天是当月第一天-1天
                DateTime checkMonthTo = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01 23:59:59.000")).AddDays(-1);

                var month = DateTime.Parse(checkMonthFrom.ToString("yyyy-MM"));

                //2. 获取所有审过的表单
                var confirmSheets = from t in dbcontent.TimeSheet
                                    join status in dbcontent.ProgressStatusTypes
                                    on t.StatusID equals status.ProgressStatusID
                                    where t.TSProjectID == 191 && status.ProjectID == 191 && status.ProgressStatusName == "已审核"
                                    select t;

                //3. 获取审核过表单内涉及到的人员
                var users = confirmSheets.Select(p => p.TimeSheetUserID).Distinct().ToList();

                List<int> folders = new List<int>();//保存部门项目下的所有目录
                foreach (WBSFolder item in wbsInfo.WbsFolders)
                {
                    folders.AddRange(ReleaseFolders[item.WBSUniqueNo]);
                }

             
                    //4. 获取对应的子目录Id
                  
                    List<MonthCostByOwner> monthCostsByOwner = new List<MonthCostByOwner>();
                    foreach (var user in users)//根据用户遍历
                    {
                        int currentUser = Convert.ToInt32(user);
                        //5. 获取每个用户对应的已审核的sheets
                        var mySheets = from mysheet in confirmSheets
                                       where mysheet.TimeSheetUserID == currentUser
                                       select mysheet;

                        //6. 从这些sheets中过滤出添加的任务的所属的timeentryid
                        var mySheetEntrys = from s in dbcontent.TimeSheetEntry
                                            join sheet in mySheets
                                            on s.TimeSheetID equals sheet.TimeSheetID
                                            join f in folders
                                            on s.SubProjectID equals f
                                            where s.IssueID > 0
                                            select s;

                        //7. 从timesheethour中获取对应的工时记录
                        var mysheetHours = from s in dbcontent.TimeSheetHour
                                           join entry in mySheetEntrys
                                           on s.TimeEntryID equals entry.TimeEntryID
                                           where s.TSProjectID == 191 && s.TimeEntryDate > checkMonthFrom
                                           && s.TimeEntryDate < checkMonthTo
                                           select s;

                        //8. 获取当前用户的统计工时
                        var TotalTime = mysheetHours.Sum(s => s.TimeEntryHours);
                        if (TotalTime > 0)
                        {
                            //遍历。。添加工时
                            monthCostsByOwner.Add(new MonthCostByOwner()
                            {
                                PersonID = currentUser,
                                LoginName = (from s in dbcontent.LogIn where s.PersonID == currentUser select s.Login1).SingleOrDefault<string>(),
                                TotalTime = TotalTime,
                                Month = month,
                                Yeart = month.Year,
                                Montht = month.Month
                            });
                        }
                }

                wbsInfo.WbsFolders = null;

                List<WBSFolder> wbsFolders = new List<WBSFolder>() {
                        new WBSFolder(){ WBSFolderID=01,WBSName="部门虚拟wbs节点",WBSUniqueNo="01",TimeCosts=monthCostsByOwner }
                    };

                wbsInfo.WbsFolders = wbsFolders;


            }
        }

        #region 获取表单里审核过的任务的工时
        /// <summary>
        /// 获取devtime表单中已审核过任务的工时
        /// </summary>
        /// <param name="wbsInfo">传入的wbsinfo</param>
        private void GetDevTimeTaskCostByOwner(WBSInfo wbsInfo)
        {
            using (ABZG_DSEntities dbcontent = new ABZG_DSEntities())
            {
                // 定位时间段
                //取上月第一天
                DateTime checkMonthFrom = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01 00:00:00.000")).AddMonths(-1);
                //上月最后一天是当月第一天-1天
                DateTime checkMonthTo = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01 23:59:59.000")).AddDays(-1);

                var month = DateTime.Parse(checkMonthFrom.ToString("yyyy-MM"));

                //2. 获取所有审过的表单
                var confirmSheets = from t in dbcontent.TimeSheet
                                    join status in dbcontent.ProgressStatusTypes
                                    on t.StatusID equals status.ProgressStatusID
                                    where t.TSProjectID == 191 && status.ProjectID == 191 && status.ProgressStatusName == "已审核"
                                    select t;

                //3. 获取审核过表单内涉及到的人员
                var users = confirmSheets.Select(p => p.TimeSheetUserID).Distinct().ToList();

                foreach (WBSFolder item in wbsInfo.WbsFolders)
                {
                    //4. 获取对应的子目录Id
                    List<int> folders = ReleaseFolders[item.WBSUniqueNo];
                    List<MonthCostByOwner> monthCostsByOwner = new List<MonthCostByOwner>();
                    foreach (var user in users)//根据用户遍历
                    {
                        int currentUser = Convert.ToInt32(user);
                        //5. 获取每个用户对应的已审核的sheets
                        var mySheets = from mysheet in confirmSheets
                                       where mysheet.TimeSheetUserID == currentUser
                                       select mysheet;

                        //6. 从这些sheets中过滤出添加的任务的所属的timeentryid
                        var mySheetEntrys = from s in dbcontent.TimeSheetEntry
                                            join sheet in mySheets
                                            on s.TimeSheetID equals sheet.TimeSheetID
                                            join f in folders
                                            on s.SubProjectID equals f
                                            where s.IssueID>0
                                            select s;

                        //7. 从timesheethour中获取对应的工时记录
                        var mysheetHours = from s in dbcontent.TimeSheetHour
                                           join entry in mySheetEntrys
                                           on s.TimeEntryID equals entry.TimeEntryID
                                           where s.TSProjectID == 191 && s.TimeEntryDate > checkMonthFrom
                                           && s.TimeEntryDate < checkMonthTo
                                           select s;

                        //8. 获取当前用户的统计工时
                        var TotalTime = mysheetHours.Sum(s => s.TimeEntryHours);
                        if (TotalTime>0)
                        {
                            //遍历。。添加工时
                            monthCostsByOwner.Add(new MonthCostByOwner()
                            {
                                PersonID = currentUser,
                                LoginName = (from s in dbcontent.LogIn where s.PersonID == currentUser select s.Login1).SingleOrDefault<string>(),
                                TotalTime = TotalTime,
                                Month = month,
                                Yeart = month.Year,
                                Montht = month.Month
                            });
                        }
                      
                      

                    }
                    item.TimeCosts = monthCostsByOwner;
                }

            }
        }

        #endregion

        #region 获取wbs节点下的工时统计，并按人员分组
        /// <summary>
        /// 获取每个wbs节点的工时统计并按人员分组
        /// </summary>
        /// <param name="wbsInfo">参数为wbs信息</param>
        /// <returns>返回带有工时统计的wbs信息</returns>
        private WBSInfo GetCostByOwner(WBSInfo wbsInfo)
        {
            using (ABZG_DSEntities dbcontent = new ABZG_DSEntities())
            {
                //                对应的sql语句
                //                -- - 获取对应目录中的所有任务
                //select BugID from Bug where projectid = 563 and subprojectid in (19929, 19930, 19931, 19950, 19924)

                //  --获取对应任务的所有工时
                //select* from IssueTimeTracking where ProjectID = 563 and issueid in (
                // select BugID from Bug where projectid = 563 and subprojectid in (19929, 19930, 19931, 19950, 19924))

                //--找出满足时间的所有工时

                //select* from IssueTimeTracking where ProjectID = 563 and issueid in (
                // select BugID from Bug where projectid = 563 and subprojectid in (19929, 19930, 19931, 19950, 19924)) and DateAdded > '2019-08-01 00:00:00.000' and DateAdded< '2019-08-31 11:59:59.000'

                //     --分组

                //     select TimeItemOwnerID,l.Login,SUM(TimeHours) as 'total time','2019-08' as 'Month'  from IssueTimeTracking time join LogIn l on time.TimeItemOwnerID = l.PersonID where ProjectID = 563 and issueid in (
                //        select BugID from Bug where projectid = 563 and subprojectid in (19929, 19930, 19931, 19950, 19924)) and DateAdded > '2019-08-01 00:00:00.000' and DateAdded< '2019-08-31 11:59:59.000'
                //group by TimeItemOwnerID,l.Login

                foreach (WBSFolder item in wbsInfo.WbsFolders)
                {
                    //1. 获取对应的子目录Id
                    List<int> folders = ReleaseFolders[item.WBSUniqueNo];

                    //2. 根据所有的子目录，获取该wbs节点中所有的任务
                    var allTasks = from t in dbcontent.Bug
                                   join id in folders
                                   on t.SubProjectID equals id
                                   where t.ProjectID == wbsInfo.DevProjectID
                                   select t;


                    //3. 根据任务获取指定时间段内所有的工时记录
                    //取上月第一天
                    DateTime checkMonthFrom = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01 00:00:00.000")).AddMonths(-1);
                    //上月最后一天是当月第一天-1天
                    DateTime checkMonthTo = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01 23:59:59.000")).AddDays(-1);

                    ////测试数据，取当月
                    //DateTime checkMonthFrom = DateTime.Parse(DateTime.Now.ToString("2019-08-01 00:00:00.000"));
                    ////测试数据，取当月
                    //DateTime checkMonthTo = DateTime.Parse(DateTime.Now.ToString("2019-08-31 23:59:59.000"));

                    var alltimetracking = from track in dbcontent.IssueTimeTracking
                                          join tasks in allTasks
                                          on track.IssueID equals tasks.BugID
                                          where track.ProjectID == wbsInfo.DevProjectID
                                          // && track.DateAdded > checkMonthFrom && track.DateAdded < checkMonthTo
                                          && track.TimeItemFromDate > checkMonthFrom && track.TimeItemFromDate < checkMonthTo
                                          select track;

                    // 4.工时统计，并按人员分组，显示每位员工花费的总工时，月份以及统计的月份
                    var month = DateTime.Parse(checkMonthFrom.ToString("yyyy-MM"));


                    var groupbytimetracking = (from track in alltimetracking
                                               join lg in dbcontent.LogIn
                                                on track.TimeItemOwnerID equals lg.PersonID
                                               where track.ProjectID == wbsInfo.DevProjectID
                                               group new { track.TimeItemOwnerID, lg.Login1, track.TimeHours, Month = month }
                                               by new { track.TimeItemOwnerID, lg.Login1 } into g
                                               select new
                                               {
                                                   PersonID = g.Key.TimeItemOwnerID,
                                                   LoginName = g.Key.Login1,
                                                   TotalTime = g.Sum(c => c.TimeHours),
                                                   CheckMonth = month
                                               });


                    //遍历。。添加工时
                    List<MonthCostByOwner> monthCostsByOwner = new List<MonthCostByOwner>();
                    foreach (var item3 in groupbytimetracking)
                    {
                        monthCostsByOwner.Add(new MonthCostByOwner()
                        {
                            PersonID = item3.PersonID,
                            LoginName = item3.LoginName,
                            TotalTime = item3.TotalTime,
                            Month = item3.CheckMonth,
                            Yeart = item3.CheckMonth.Year,
                            Montht = item3.CheckMonth.Month
                        }) ;
                    }


                    item.TimeCosts = monthCostsByOwner;

                }
            }

            return wbsInfo;//待实现
        }
        #endregion

        #region 获取每个wbs元素对应下的子目录，并把他存入字典中
        /// <summary>
        /// 获取所有wbs元素对应的子目录
        /// </summary>
        /// <param name="wbsInfo"></param>
        /// <returns></returns>
        public Dictionary<string, List<int>> GetAllChildFolderIds(WBSInfo wbsInfo)
        {
            Dictionary<string, List<int>> ReleaseFolders = new Dictionary<string, List<int>>();
            using (ABZG_DSEntities dbcontext = new ABZG_DSEntities())
            {
                foreach (var item in wbsInfo.WbsFolders)
                {
                    List<int> folders = GetSonID(wbsInfo.DevProjectID, item.WBSFolderID).ToList();
                    folders.Add(item.WBSFolderID);
                    if (ReleaseFolders.ContainsKey(item.WBSUniqueNo))
                    {
                        ReleaseFolders[item.WBSUniqueNo] = ReleaseFolders[item.WBSUniqueNo].Union(folders).ToList<int>();
                    }
                    else
                    ReleaseFolders.Add(item.WBSUniqueNo, folders);
                }

            }
            return ReleaseFolders;

        }

        #region 获取所有下级
        public IEnumerable<int> GetSonID(int projectID, int p_id)
        {
            using (ABZG_DSEntities dbcontext = new ABZG_DSEntities())
            {
                var query = from c in dbcontext.SubProjectTree
                            where c.ProjectID == projectID
&& c.ParentID == p_id
                            select c.ChildID;

                return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(projectID, t)));
            }

        }
        #endregion

        #endregion

        #region 获取DevTime中的通用工时，并归入首个"TR1/TR2" 节点下
        private WBSInfo GetDevTimeCostByOwner(WBSInfo wbsInfo)
        {
            //定义存储时间的泛型列表，统计每个人当月在DevTime中花费的总工时
            List<MonthCostByOwner> monthCostByOwners = new List<MonthCostByOwner>();

            //取上月第一天
            DateTime checkMonthFrom = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01 00:00:00.000")).AddMonths(-1);
            //上月最后一天是当月第一天-1天
            DateTime checkMonthTo = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01 23:59:59.000")).AddDays(-1);

            var month = DateTime.Parse(checkMonthFrom.ToString("yyyy-MM"));
            //直接获取该项目中指定人员在上月中在已审批的时间表单中花费的工时
            using (ABZG_DSEntities dbcontent = new ABZG_DSEntities())
            {
                //获取参与部门项目开发空间的人员编号
                var subProjectOwners = from owners in dbcontent.SubProjectOwners
                                       where owners.ProjectID == wbsInfo.DevProjectID && owners.SubProjectID == wbsInfo.DevSpaceID
                                       select owners;

                if (subProjectOwners.Count()>0)//代表当前开发空间有参与人员，需要统计devtime中工时
                {
                    foreach (var item in subProjectOwners)
                    {
                        //获取当前遍历用户所属的已审批过的时间表单
                        var timeSheets = from timesheet in dbcontent.TimeSheet
                                         join status in dbcontent.ProgressStatusTypes
                                         on timesheet.StatusID equals status.ProgressStatusID
                                         where status.ProgressStatusName == "已审核" && status.ProjectID == 191//devtime编号
                                         && timesheet.TimeSheetUserID == item.TeamMemberID && timesheet.TSProjectID == 191
                                         select timesheet;

                        if (timeSheets.Count()>0)//如果有表单数据
                        {
                            //找到所属这些时间表单中的属于devtime公共部分的数据
                            var timeSheetEntry = from entry in dbcontent.TimeSheetEntry
                                                 join sheets in timeSheets
                                                 on new { entry.TSProjectID, entry.TimeSheetID } equals new { sheets.TSProjectID, sheets.TimeSheetID }
                                                 where entry.TSProjectID == 191 && entry.TimeEntryType == 3 && entry.StandTimeEntryID > 3
                                                 select entry;

                             if (timeSheetEntry.Count()>0)//如果有需要检查的表单中的行数据
                            {
                                var timesheetHours = from hours in dbcontent.TimeSheetHour
                                                     join entrys in timeSheetEntry
                                                     on new { hours.TSProjectID, hours.TimeSheetID, hours.TimeEntryID } equals new { entrys.TSProjectID, entrys.TimeSheetID, entrys.TimeEntryID }
                                                     where hours.TimeEntryDate > checkMonthFrom && hours.TimeEntryDate < checkMonthTo
                                                     select hours;

                                monthCostByOwners.Add(new MonthCostByOwner()
                                {
                                    PersonID = item.TeamMemberID,
                                    LoginName = (from l in dbcontent.LogIn
                                                 where l.PersonID == item.TeamMemberID
                                                 select l.Login1).SingleOrDefault(),
                                    Month = month,
                                    Yeart = month.Year,
                                    Montht = month.Month,
                                    TotalTime = (from c in timesheetHours
                                                 select c.TimeEntryHours).Sum()
                                }); 
                            }
                                                
                        }

                    }
                }

                //判断部门的虚拟的节点的时间
                var tr1times = (from currentr1 in wbsInfo.WbsFolders
                          where currentr1.WBSUniqueNo == "01"
                          select currentr1).SingleOrDefault().TimeCosts;

                tr1times.AddRange(monthCostByOwners);
               // List<MonthCostByOwner> newmonthCostByOwners = new List<MonthCostByOwner>();
               var newmonthCostByOwners = from costs in tr1times
                                       group costs
                                       by new { costs.PersonID, costs.LoginName } into g
                                       select new MonthCostByOwner
                                       {
                                           PersonID = g.Key.PersonID,
                                           LoginName = g.Key.LoginName,
                                           Month = month,
                                           Yeart = month.Year,
                                           Montht = month.Month,
                                           TotalTime = g.Sum(x => x.TotalTime),
                                       };
                List<MonthCostByOwner> monthCostByOwners2 = new List<MonthCostByOwner>();
                 monthCostByOwners2 = newmonthCostByOwners.ToList();
                foreach (var item in wbsInfo.WbsFolders)
                {
                    if (item.WBSUniqueNo== "01")
                    {
                        item.TimeCosts = null;
                        item.TimeCosts = new List<MonthCostByOwner>();
                        item.TimeCosts.AddRange(monthCostByOwners2);
                    }
                }

            }

                return wbsInfo;
        }

        #endregion
    }
}
