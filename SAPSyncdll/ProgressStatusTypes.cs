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
    
    public partial class ProgressStatusTypes
    {
        public int ProjectID { get; set; }
        public int ProgressStatusID { get; set; }
        public string ProgressStatusName { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<int> CanBeInitialState { get; set; }
        public Nullable<int> CanBeCloseState { get; set; }
        public Nullable<int> CanBeReopenState { get; set; }
        public Nullable<int> DefaultOwnerID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> CustomerAccess { get; set; }
        public Nullable<int> IfResolved { get; set; }
        public Nullable<int> IfResponded { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<int> IfSetSLAStartTime { get; set; }
        public Nullable<int> IfClosed { get; set; }
        public Nullable<int> NeedIdentityConfirmation { get; set; }
        public Nullable<int> IfDevTimeDefaultNew { get; set; }
        public Nullable<int> SetTimeToZero { get; set; }
        public Nullable<int> AddTimeRemain { get; set; }
        public Nullable<int> HSCompanyID { get; set; }
        public Nullable<int> DefaultOwnerID4Unassigned { get; set; }
        public Nullable<int> DefaultOwnerID4NonAppl { get; set; }
        public Nullable<int> DefaultOwner4Appl { get; set; }
        public Nullable<int> DefaultOwner4NonAppl { get; set; }
        public Nullable<int> ApplicableToRespond { get; set; }
        public Nullable<int> ApplicableToResolve { get; set; }
        public string Comments { get; set; }
        public Nullable<int> IfRestored { get; set; }
        public Nullable<int> ApplicableToRestore { get; set; }
        public Nullable<int> SetResolved4WebConversation { get; set; }
        public string ConfirmationMsg { get; set; }
        public string IdentityConfirmMsg { get; set; }
        public Nullable<int> GoToPrevStateOption { get; set; }
        public Nullable<int> IfEnableUserEvaluation { get; set; }
        public Nullable<int> OwnerMustBeAssigned { get; set; }
        public Nullable<int> WorkflowTypeOption { get; set; }
        public Nullable<int> IfDefaultOwner4SamePrevious { get; set; }
    }
}
