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
    
    public partial class TimeSheetEntry
    {
        public int TSProjectID { get; set; }
        public int TimeSheetID { get; set; }
        public int TimeEntryID { get; set; }
        public string TimeEntryTitle { get; set; }
        public Nullable<int> TimeEntryType { get; set; }
        public Nullable<int> WorkProjectID { get; set; }
        public Nullable<int> SubProjectID { get; set; }
        public Nullable<int> StandTimeEntryID { get; set; }
        public Nullable<int> WorkTypeID { get; set; }
        public Nullable<int> TimeOffID { get; set; }
        public Nullable<int> IssueID { get; set; }
        public string IssueTitle { get; set; }
        public string ProjectName { get; set; }
        public string SubProjectName { get; set; }
        public Nullable<int> TSEntryGroupOption { get; set; }
        public Nullable<int> TSEntryOption { get; set; }
    }
}
