using System.Text.Json.Serialization;

namespace Booking.Ivao.DTO
{
#pragma warning disable CS8618
#pragma warning disable IDE1006
    public class UserInfo
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string centerId { get; set; }
        public string countryId { get; set; }
        public DateTime createdAt { get; set; }
        public string divisionId { get; set; }
        public bool isStaff { get; set; }
        public string languageId { get; set; }
        public string email { get; set; }
        public Rating rating { get; set; }
        public string[] gcas { get; set; }
        public Hour[] hours { get; set; }
        public Userstaffposition[] userStaffPositions { get; set; }
        public Userstaffdetails userStaffDetails { get; set; }
        public object prCreator { get; set; }
        public object[] ownedVirtualAirlines { get; set; }
        public int sub { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string nickname { get; set; }
        public string profile { get; set; }
        public string publicNickname { get; set; }

        public class Rating
        {
            public bool isPilot { get; set; }
            public bool isAtc { get; set; }
            public Pilotrating pilotRating { get; set; }
            public Atcrating atcRating { get; set; }
            public Networkrating networkRating { get; set; }
        }

        public class Pilotrating
        {
            public int id { get; set; }
            public string name { get; set; }
            public string shortName { get; set; }
            public string description { get; set; }
        }

        public class Atcrating
        {
            public int id { get; set; }
            public string name { get; set; }
            public string shortName { get; set; }
            public string description { get; set; }
        }

        public class Networkrating
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class Userstaffdetails
        {
            public string email { get; set; }
            public object note { get; set; }
            public object description { get; set; }
            public object remark { get; set; }
        }

        public class Hour
        {
            public string type { get; set; }
            public int hours { get; set; }
        }

        public class Userstaffposition
        {
            public string id { get; set; }
            public string staffPositionId { get; set; }
            public string divisionId { get; set; }
            public object centerId { get; set; }
            public string connectAs { get; set; }
            public bool onTrial { get; set; }
            public object description { get; set; }
            public Staffposition staffPosition { get; set; }
        }

        public class Staffposition
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public Departmentteam departmentTeam { get; set; }
        }

        public class Departmentteam
        {
            public string id { get; set; }
            public string name { get; set; }
            public Department department { get; set; }
        }

        public class Department
        {
            public string id { get; set; }
            public string name { get; set; }
        }
    }
#pragma warning restore IDE1006
#pragma warning restore CS8618
}
