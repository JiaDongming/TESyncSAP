using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;
using System.Data;
using SAPSyncdll.Models;
using System.Configuration;

namespace SAPSyncdll
{
    /// <summary>
    /// 调用SAP接口，传递数据
    /// </summary>
    public class SAPSync
    {
        private RfcDestination _rfcDestination = null;
        public DataTable dtr = new DataTable();

        public void RegisterDestination()  //注册客户端
        {
            try
            {
                if (_rfcDestination == null)
                {
                    //rfc配置
                    //RfcConfigParameters argsP = new RfcConfigParameters();
                    //argsP.Add(RfcConfigParameters.Name, ConfigurationManager.AppSettings["Name"].ToString());
                    //argsP.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["AppServerHost"].ToString());
                    //argsP.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SystemNumber"].ToString()); 
                    //argsP.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings["User"].ToString());
                    //argsP.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings["Password"].ToString());
                    //argsP.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings["Client"].ToString());
                    //argsP.Add(RfcConfigParameters.Language, ConfigurationManager.AppSettings["Language"].ToString());
                    //argsP.Add(RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["PoolSize"].ToString());
                    ////argsP.Add(RfcConfigParameters.LogonGroup, ConfigurationManager.AppSettings["GROUP"].ToString());
                    //argsP.Add(RfcConfigParameters.MaxPoolSize, ConfigurationManager.AppSettings["MaxPoolSize"].ToString());
                    //argsP.Add(RfcConfigParameters.IdleTimeout, ConfigurationManager.AppSettings["IdleTimeout"].ToString());

                    //_rfcDestination = RfcDestinationManager.GetDestination(argsP);
                   // 直接读取app.config中的节点中的数据
                    _rfcDestination = RfcDestinationManager.GetDestination(ConfigurationManager.AppSettings["Name"].ToString());

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取RfcDestination出错" + ex.Message);
            }
        }

        /// <summary>
        /// 调用SAP提供的RFC函数ZPSRFC03传递项目信息和wbs信息给SAP
        /// </summary>
        /// <param name="project">项目信息</param>
        /// <param name="wbsInfo">wbs信息</param>
        public List<ReturnMessage> InvokeRFCFunctionZPSRFC03(Project project, WBSInfo wbsInfo)
        {
            IRfcFunction function = null;
            List<ReturnMessage> returnMessage = new List<ReturnMessage>();
            try
            {
                try
                {
                    function = _rfcDestination.Repository.CreateFunction("ZPSRFC03");
                    //  function.Invoke(_rfcDestination);
                    // IRfcStructure itb = function.GetStructure("ZSPS_003");
                    IRfcTable projectTable = function.GetTable("LT_XMDY");
                    projectTable.Insert();

                    //IRfcStructure struSAP = projectTable.Metadata.LineType.CreateStructure();
                    //struSAP.SetValue("POSID", project.ProjectUniqueID);//传入项目编码
                    //struSAP.SetValue("POST1", project.ProjectName);//传入项目名称
                    //struSAP.SetValue("PROFL", project.ProjectTypeID);//传入项目编码
                    //struSAP.SetValue("VERNR", project.ProjectManager);//传入项目经理
                    //struSAP.SetValue("PXMDH", project.ProjectCodeName);//传入项目代号
                    //struSAP.SetValue("PXMLX", project.ProjectType);//传入项目类型
                    //struSAP.SetValue("PWTBM", project.DelegateDepartment);//传入委托部门
                    //struSAP.SetValue("PCDBM", project.UnderTaskDepartment);//传入承担部门
                    //struSAP.SetValue("FZDDJ", project.ProjectComplexLevel);//传入项目复杂度等级
                    //struSAP.SetValue("PXYXJ", project.ProjectPriority);//传入项目优先级
                    //struSAP.SetValue("ESTAT", project.ProjectStatus);//传入项目状态
                    //projectTable.Append(struSAP);

                    //function.SetValue("LT_XMDY", projectTable);
                    //function.Invoke(_rfcDestination);

                    projectTable.CurrentRow.SetValue("POSID", project.ProjectUniqueID);//传入项目编码
                    //projectTable.CurrentRow.SetValue("POSID", "B1909007");//传入项目编码
                    projectTable.CurrentRow.SetValue("POST1", project.ProjectName);//传入项目名称
                    projectTable.CurrentRow.SetValue("PROFL", project.ProjectTypeID);//传入项目编码
                    projectTable.CurrentRow.SetValue("VERNR", project.ProjectManager);//传入项目经理
                    //projectTable.CurrentRow.SetValue("VERNR", 00000002);//传入项目经理
                    projectTable.CurrentRow.SetValue("PXMDH", project.ProjectCodeName);//传入项目代号
                    projectTable.CurrentRow.SetValue("PXMLX", project.ProjectType);//传入项目类型
                    projectTable.CurrentRow.SetValue("PWTBM", project.DelegateDepartment);//传入委托部门
                    //projectTable.CurrentRow.SetValue("PWTBM", "10000010");//传入委托部门
                   projectTable.CurrentRow.SetValue("PCDBM", project.UnderTaskDepartment);//传入承担部门
                   //projectTable.CurrentRow.SetValue("PCDBM", "10000010");//传入承担部门
                    projectTable.CurrentRow.SetValue("FZDDJ", project.ProjectComplexLevel);//传入项目复杂度等级
                    projectTable.CurrentRow.SetValue("PXYXJ", project.ProjectPriority);//传入项目优先级
                    projectTable.CurrentRow.SetValue("ESTAT", project.ProjectStatus);//传入项目状态

                    // function = _rfcDestination.Repository.CreateFunction("ZPSRFC03");
                    function.SetValue("LT_XMDY", projectTable);
                    // function.Invoke(_rfcDestination);

                    IRfcTable wbsTable = function.GetTable("LT_WBSD");

                    //for (int i = 0; i < wbsInfo.WbsFolders.Count(); i++)
                    //{

                    //    IRfcStructure struSAP2 = wbsTable.Metadata.LineType.CreateStructure();
                    //    struSAP2.SetValue("ZPOSID", wbsInfo.WbsFolders[i].WBSUniqueNo);//传入wbs的DS 阶段 ID
                    //    struSAP2.SetValue("POSIDNAME", wbsInfo.WbsFolders[i].WBSName);//传入wbs的DS 阶段名称
                    //    struSAP2.SetValue("PSPID", wbsInfo.ProjectUniqueID);
                    //    wbsTable.Append(struSAP2);
                    //}

                    for (int i = 0; i < wbsInfo.WbsFolders.Count(); i++)
                    {
                        wbsTable.Insert();
                        wbsTable.CurrentRow.SetValue("ZPOSID", wbsInfo.WbsFolders[i].WBSUniqueNo);//传入wbs的DS 阶段 ID
                        wbsTable.CurrentRow.SetValue("POSIDNAME", wbsInfo.WbsFolders[i].WBSName);//传入wbs的DS 阶段名称
                        wbsTable.CurrentRow.SetValue("PSPID", wbsInfo.ProjectUniqueID);
                        //wbsTable.CurrentRow.SetValue("PSPID", "B1909007");
                    }


                    //  function = _rfcDestination.Repository.CreateFunction("ZPSRFC03");
                    function.SetValue("LT_WBSD", wbsTable);

                    // function.SetParameterActive(0, true);
                    function.Invoke(_rfcDestination);
                }
                catch (Exception ex)
                {

                    //LogHelper.WriteLog("连接SAP远程服务器出错：" + ex.Message);
                    LogHelper.Error("连接SAP远程服务器出错：" + ex.Message,ex);
                }
                // function.GetElementMetadata(2).ValueMetadataAsTableMetadata.LineType.Name.ElementAt.SetValue("POSID", project.ProjectUniqueID);//传入项目编码

                //function.SetValue("POSID", project.ProjectUniqueID);//传入项目编码
                //function.SetValue("POST1", project.ProjectName);//传入项目名称
                //function.SetValue("PROFL", project.ProjectTypeID);//传入项目编码
                //function.SetValue("VERNR", project.ProjectManager);//传入项目经理
                //function.SetValue("PXMDH", project.ProjectCodeName);//传入项目代号
                //function.SetValue("PXMLX", project.ProjectType);//传入项目类型
                //function.SetValue("PWTBM", project.DelegateDepartment);//传入委托部门
                //function.SetValue("PCDBM", project.UnderTaskDepartment);//传入承担部门
                //function.SetValue("FZDDJ", project.ProjectComplexLevel);//传入项目复杂度等级
                //function.SetValue("PXYXJ", project.ProjectPriority);//传入项目优先级
                //function.SetValue("ESTAT", project.ProjectStatus);//传入项目状态


                // var Nos = from c in wbsInfo.WbsFolders select c.WBSUniqueNo;
                // var Names = from n in wbsInfo.WbsFolders select n.WBSName;
                // string wbsUniqueNos = string.Join("、",Nos.ToArray());
                //string wbsNames = string.Join("、",Names.ToArray());

                //foreach (var item in wbsInfo.WbsFolders)
                //{
                //    function.SetValue("ZPOSID", item.WBSUniqueNo);//传入wbs的DS 阶段 ID
                //    function.SetValue("POSIDNAME", item.WBSName);//传入wbs的DS 阶段名称
                //}
                //function.SetValue("PSPID", wbsInfo.ProjectUniqueID);//传入项目编码

                //function.SetParameterActive(0,true);
                //function.Invoke(_rfcDestination);

                //获取调用SAP的ZPSRFC03返回值到DataTable
                IRfcTable myrfcTable = function.GetTable("LT_RETURN");//rfc server function 返回值table结构名称

                int liElement = 0;
                for (liElement = 0; liElement <= myrfcTable.ElementCount - 1; liElement++)
                {
                    RfcElementMetadata metadata = myrfcTable.GetElementMetadata(liElement);
                    dtr.Columns.Add(metadata.Name);//循环创建列
                }
                foreach (IRfcStructure dr in myrfcTable)//循环table结构表
                {
                    DataRow row = dtr.NewRow();//创建新行
                    for (liElement = 0; liElement <= myrfcTable.ElementCount - 1; liElement++)
                    {
                        RfcElementMetadata metadata = myrfcTable.GetElementMetadata(liElement);
                        row[metadata.Name] = dr.GetString(metadata.Name).Trim();
                    }
                    dtr.Rows.Add(row);
                }

                //输出调用返回日志
                for (int i = 0; i < dtr.Rows.Count; i++)
                {
                    returnMessage.Add(new ReturnMessage()
                    {
                        Type = dtr.Rows[i]["TYPE"].ToString(),
                        Message = dtr.Rows[i]["MESSAGE"].ToString(),
                    });
                    LogHelper.WriteLog("SAP返回结果：消息类型：" + dtr.Rows[i]["TYPE"].ToString() + "\t消息文本" + dtr.Rows[i]["MESSAGE"].ToString());
                }


            }
            catch (Exception ex)
            {

                // LogHelper.WriteLog("调用SAP函数ZPSRFC03出错：" + ex.Message);
                LogHelper.Error("调用SAP函数ZPSRFC03出错：" + ex.Message,ex);
            }
            return returnMessage;
        }
        public void InvokeRFCFunctionZPSRFC04(WBSInfo wbsInfo)
        {
            IRfcFunction function = null;

            try
            {
                try
                {
                    function = _rfcDestination.Repository.CreateFunction("ZPSRFC04");


                    IRfcTable wbsTable = function.GetTable("LT_GSDATA");



                    for (int i = 0; i < wbsInfo.WbsFolders.Count(); i++)
                    {
                        for (int j = 0; j < wbsInfo.WbsFolders[i].TimeCosts.Count(); j++)
                        {
                            wbsTable.Insert();
                            wbsTable.CurrentRow.SetValue("PSPID", wbsInfo.ProjectUniqueID);//项目定义编码
                            wbsTable.CurrentRow.SetValue("ZPOSID", wbsInfo.WbsFolders[i].WBSUniqueNo);//传入wbs的DS 阶段 ID
                            wbsTable.CurrentRow.SetValue("SYGID", wbsInfo.WbsFolders[i].TimeCosts[j].LoginName);//员工ID 00000002
                            wbsTable.CurrentRow.SetValue("MJAHR", wbsInfo.WbsFolders[i].TimeCosts[j].Yeart);//年
                            wbsTable.CurrentRow.SetValue("SGSYF", wbsInfo.WbsFolders[i].TimeCosts[j].Montht);//月
                            wbsTable.CurrentRow.SetValue("SJGSZ", wbsInfo.WbsFolders[i].TimeCosts[j].TotalTime);
                        }
                       
                    }

                    function.SetValue("LT_GSDATA", wbsTable);


                    function.Invoke(_rfcDestination);
                }
                catch (Exception ex)
                {

                    //LogHelper.WriteLog("连接SAP远程服务器出错：" + ex.Message);
                    LogHelper.Error("连接SAP远程服务器出错：" + ex.Message,ex);
                }


                //获取调用SAP的ZPSRFC04返回值到DataTable
                IRfcTable myrfcTable = function.GetTable("LT_RETURN");//rfc server function 返回值table结构名称

                int liElement = 0;
                for (liElement = 0; liElement <= myrfcTable.ElementCount - 1; liElement++)
                {
                    RfcElementMetadata metadata = myrfcTable.GetElementMetadata(liElement);
                    dtr.Columns.Add(metadata.Name);//循环创建列
                }
                foreach (IRfcStructure dr in myrfcTable)//循环table结构表
                {
                    DataRow row = dtr.NewRow();//创建新行
                    for (liElement = 0; liElement <= myrfcTable.ElementCount - 1; liElement++)
                    {
                        RfcElementMetadata metadata = myrfcTable.GetElementMetadata(liElement);
                        row[metadata.Name] = dr.GetString(metadata.Name).Trim();
                    }
                    dtr.Rows.Add(row);
                }

                //输出调用返回日志
                for (int i = 0; i < dtr.Rows.Count; i++)
                {

                    LogHelper.WriteLog("SAP返回结果：消息类型：" + dtr.Rows[i]["TYPE"].ToString() + "\t消息文本" + dtr.Rows[i]["MESSAGE"].ToString());
                }


            }
            catch (Exception ex)
            {

                // LogHelper.WriteLog("调用SAP函数ZPSRFC04出错：" + ex.Message);
                LogHelper.Error("调用SAP函数ZPSRFC04出错：" + ex.Message,ex);
            }
        }
    }
}
