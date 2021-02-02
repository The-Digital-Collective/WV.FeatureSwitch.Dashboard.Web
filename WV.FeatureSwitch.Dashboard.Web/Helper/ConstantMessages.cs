using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WV.FeatureSwitch.Dashboard.Web.Helper
{
    public static class ConstantMessages
    {
        public static string Load = "{event} Loaded Successfully .";
        public static string Create = "{event} created Successfully";
        public static string Update = "{event} updated Successfully.";
        public static string Delete = "{event} deleted Successfully .";
        public static string Duplicate = "{event} details Already Exists.";
        public static string ResetAll = "All Feature Reset To Default Successfully .";
        public static string Error = "Error Occurred in While processing your requst.";
        public static string SignUpExists = "{event} already exists in Singup.";
        public static string LoadFailure = "Failed to retrieve the {event}-{0}.";
        public static string CreateFailure = "Failed to create {event} -{0}";
        public static string UpdateFailure = "Failed to update {event}-{0}";
        public static string DeleteFailure = "Failed to delete {event}-{0}";
        public static string ExistsFailure = "{event} not exists -{0}";
        public static string NoRecordsFound = "No records found in {event}";
        public static string SelfDelete = "You cannot delete yourself";
        public static string SignUpNotExists = "{event} not exists in SignUp";
        public static string UploadChildIds = "Child Ids Uploaded Successfully. ";
        public static string UploadChildIdsFailure = "Failed to Upload Child Ids. ";
    }
}
