//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAPSyncdll
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bug
    {
        public int ProjectID { get; set; }
        public int BugID { get; set; }
        public string ProblemID { get; set; }
        public string BugTitle { get; set; }
        public Nullable<int> CreatedTypeID { get; set; }
        public Nullable<int> PrevBugIDIfReopen { get; set; }
        public Nullable<int> CreatedByPerson { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> CurrentOwner { get; set; }
        public Nullable<int> ProgressStatusID { get; set; }
        public Nullable<int> AssignedByPersonID { get; set; }
        public Nullable<System.DateTime> DateAssigned { get; set; }
        public Nullable<int> ClosedByPerson { get; set; }
        public Nullable<int> CloseStatusID { get; set; }
        public Nullable<System.DateTime> DateClosed { get; set; }
        public Nullable<int> CrntPriorityID { get; set; }
        public Nullable<short> IsActive { get; set; }
        public Nullable<short> IfClosed { get; set; }
        public Nullable<int> CrntOriginTypeID { get; set; }
        public Nullable<int> CrntBugTypeID { get; set; }
        public Nullable<int> NoOfHistories { get; set; }
        public string LockOwner { get; set; }
        public Nullable<int> CrntPlatformID { get; set; }
        public string CrntVersion { get; set; }
        public Nullable<int> CrntVersionID { get; set; }
        public Nullable<int> TargetReleaseID { get; set; }
        public Nullable<System.DateTime> TargetDate { get; set; }
        public Nullable<int> EstimatedHours { get; set; }
        public Nullable<int> ActualHours { get; set; }
        public Nullable<System.DateTime> DateFixed { get; set; }
        public Nullable<int> CrntForwardTypeID { get; set; }
        public bool IsModified { get; set; }
        public bool IsSharedIssue { get; set; }
        public Nullable<int> CrntGroupID { get; set; }
        public Nullable<System.DateTime> PlanedStartDate { get; set; }
        public Nullable<System.DateTime> PlanedEndDate { get; set; }
        public Nullable<int> CrntFolderID { get; set; }
        public Nullable<int> LastUpdateNo { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> NoOfResponses { get; set; }
        public Nullable<int> AvailableToCustomer { get; set; }
        public Nullable<int> ContactID { get; set; }
        public Nullable<int> PublishedKnowledgeID { get; set; }
        public Nullable<int> EmailStatusBits { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<System.DateTime> DateLastOpenMail { get; set; }
        public Nullable<System.DateTime> DateLastUntouchMail { get; set; }
        public Nullable<int> AccessCustomerMethod { get; set; }
        public Nullable<int> ServicePlanID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public Nullable<int> Ready4Billing { get; set; }
        public Nullable<int> IfClosedByCustomer { get; set; }
        public Nullable<System.DateTime> DateLastDueDateMail { get; set; }
        public Nullable<double> EstimatedCost { get; set; }
        public Nullable<int> EnableLiveSupport { get; set; }
        public Nullable<int> IfFinishDateLinked { get; set; }
        public Nullable<double> TotalSale { get; set; }
        public Nullable<int> SalesOrderTemplateID { get; set; }
        public Nullable<int> SalesPossibility { get; set; }
        public Nullable<int> SalesStatusID { get; set; }
        public Nullable<System.DateTime> ExpectedPODate { get; set; }
        public Nullable<int> PrimaryQuoteID { get; set; }
        public Nullable<System.DateTime> PODate { get; set; }
        public Nullable<int> SubProjectID { get; set; }
        public Nullable<int> PriorityValue { get; set; }
        public Nullable<int> PriorityOrder { get; set; }
        public Nullable<int> LinkedMSProjectTaskID { get; set; }
        public Nullable<System.DateTime> LastStatusDate { get; set; }
        public Nullable<System.DateTime> SalesLostDate { get; set; }
        public Nullable<int> InstallSiteID { get; set; }
        public Nullable<int> PrimaryEventID { get; set; }
        public Nullable<int> AttachmentID { get; set; }
        public Nullable<int> AttachmentTypeID { get; set; }
        public Nullable<int> IfBillable { get; set; }
        public Nullable<int> PrimaryContactID { get; set; }
        public string ExternalBugID { get; set; }
        public Nullable<int> SalesPersonID { get; set; }
        public Nullable<double> TotalNetSales { get; set; }
        public Nullable<int> IfAssetLink4SalesProcessed { get; set; }
        public Nullable<System.DateTime> ActiveStartDate { get; set; }
        public Nullable<System.DateTime> ActiveEndDate { get; set; }
        public Nullable<int> IfNewSales { get; set; }
        public Nullable<int> IfResponseTimeLinked { get; set; }
        public Nullable<int> PlanResponseTimeID { get; set; }
        public Nullable<System.DateTime> TimeResponded { get; set; }
        public Nullable<System.DateTime> TimeResolved { get; set; }
        public Nullable<int> LostToCompetitorID { get; set; }
        public Nullable<int> LinkedQATestKnowledgeID { get; set; }
        public Nullable<int> LinkedQAIssueTemplateID { get; set; }
        public Nullable<int> IssueType { get; set; }
        public Nullable<int> CompetitorSurveyTmpltID { get; set; }
        public Nullable<int> CreatedFromWebClickID { get; set; }
        public Nullable<int> LinkedFormTemplateID { get; set; }
        public Nullable<int> ParentIfParent { get; set; }
        public Nullable<int> ParentIssueID { get; set; }
        public Nullable<int> LinkedShortCutIssueID { get; set; }
        public Nullable<double> LinkedSalesTotal { get; set; }
        public string SubmittedFromEmail { get; set; }
        public Nullable<int> DynamicOwnerFolderID { get; set; }
        public Nullable<System.DateTime> RequiredResponseTime { get; set; }
        public Nullable<System.DateTime> RequiredResolveTime { get; set; }
        public Nullable<System.DateTime> ActualResponseTime { get; set; }
        public Nullable<System.DateTime> ActualResolveTime { get; set; }
        public Nullable<int> ActualResponseMinutes { get; set; }
        public Nullable<int> ActualResolveMinutes { get; set; }
        public Nullable<System.DateTime> SLAStartTime { get; set; }
        public Nullable<int> IfSLAException { get; set; }
        public Nullable<int> LastTransitionID { get; set; }
        public Nullable<int> IfNewRequest { get; set; }
        public Nullable<System.DateTime> RequestTime { get; set; }
        public Nullable<int> IsLocked { get; set; }
        public Nullable<int> LockedByPersonID { get; set; }
        public Nullable<int> IsNotAcknowledged { get; set; }
        public Nullable<int> WorkflowID { get; set; }
        public Nullable<int> IssueTypeID { get; set; }
        public Nullable<int> ReleaseBuildID { get; set; }
        public Nullable<int> TimeSpent { get; set; }
        public Nullable<int> TimeRemain { get; set; }
        public Nullable<System.DateTime> IssueFinishDate { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public Nullable<double> CurrencyRate { get; set; }
        public string ProblemDescriptionUcd { get; set; }
        public string CloseDescriptionUcd { get; set; }
        public Nullable<int> LinkedSpecID { get; set; }
        public Nullable<int> DevSpecSubProjectID { get; set; }
        public Nullable<int> LinkedModuleID { get; set; }
        public Nullable<int> BacklogSubProjectID { get; set; }
        public Nullable<int> LinkedDefectSpecID { get; set; }
        public Nullable<int> BacklogItemPriorityValue { get; set; }
        public Nullable<int> BacklogItemOrder { get; set; }
        public Nullable<int> SpecificationPoint { get; set; }
        public string CloseDescription { get; set; }
        public string ProblemDescription { get; set; }
        public string ContactMemo { get; set; }
        public string AnswerToCustomer { get; set; }
        public Nullable<System.DateTime> TaskPlannedStartDate { get; set; }
        public Nullable<System.DateTime> TaskPlannedFinishDate { get; set; }
        public Nullable<int> LinkedImplementationCopyID { get; set; }
        public Nullable<int> IfRestoreTimeLinked { get; set; }
        public Nullable<System.DateTime> RequiredRestoreTime { get; set; }
        public Nullable<System.DateTime> ActualRestoreTime { get; set; }
        public Nullable<int> ActualRestoreMinutes { get; set; }
        public Nullable<System.DateTime> PlannedRestoreTime { get; set; }
        public string CWPSourceIP { get; set; }
        public string CWPSourceName { get; set; }
        public Nullable<int> PrimaryResource { get; set; }
        public Nullable<int> EstTotal { get; set; }
        public Nullable<int> IfCommitted { get; set; }
        public Nullable<int> ParentStoryItemID { get; set; }
        public Nullable<System.DateTime> IssueActualStartDate { get; set; }
        public Nullable<int> EmployeeOnBehalfOf { get; set; }
        public Nullable<int> LastModifiedByPerson { get; set; }
        public Nullable<int> ProductVersionOption { get; set; }
        public Nullable<int> LinkedSvcAssetID { get; set; }
        public Nullable<int> IsArchived { get; set; }
        public Nullable<int> EmployeeManager { get; set; }
        public Nullable<int> EvaluationValueID { get; set; }
        public string EvaluationComments { get; set; }
        public Nullable<int> LinkedKWSolutionID { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<int> KeyTask { get; set; }
    }
}
