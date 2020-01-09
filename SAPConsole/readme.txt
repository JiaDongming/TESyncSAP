调试数据：502 21018 0 5408
该项目用于DevSuite 和SAP同步，其中引用了
1. EF框架和Linq用于和数据库交互
2. Log4net引用实现日志的记录
3. 调用sap dll来调用sap rfc
4. 生成的SAPConsole.exe，用触发器调用来实现新建项目或者其他时机的时候来获取ds数据同步到sap