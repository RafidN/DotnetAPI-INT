namespace DotNetAPI
{
    public partial class UserJobInfo
    {
        public int UserID { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }

        public UserJobInfo()
        {
            if (JobTitle == null)
            {
                JobTitle = "";
            }
            if (Department == null)
            {
                Department = "";
            }
        }
    }
}