namespace EmailFix
{
    public class Recipient
    {
        public string EmailTo { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string LargeItems { get; set; }
        public string MediumItems { get; set; }
        public string SmallItems { get; set; }
        public string BagsNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTextMMS { get; set; }
        public string NoContactPost { get; set; }
        public string NoContactTelephone { get; set; }
        public string Content { get; set; }
        public string VanCollectionId { get; set; }
        public string CreatedDate { get; set; }
        public string StoreEmail { get; set; }

        public static Recipient FromCsv(string csvLine)
        {
            string[] matrix = csvLine.Split(',');
            Recipient r = new Recipient
            {
                EmailTo = matrix[0],
                Title = matrix[1],
                Firstname = matrix[2],
                Surname = matrix[3],
                Phone = matrix[4],
                Address1 = matrix[5],
                City = matrix[6],
                County = matrix[7],
                Country = matrix[9],
                Postcode = matrix[8],
                LargeItems = matrix[10],
                MediumItems = matrix[11],
                SmallItems = matrix[12],
                BagsNumber = matrix[13],
                ContactEmail = matrix[14],
                ContactTextMMS = matrix[15],
                NoContactPost = matrix[16],
                NoContactTelephone = matrix[17],
                VanCollectionId = matrix[18],
                CreatedDate = matrix[19],
                StoreEmail = matrix[20].TrimEnd('\r', '\n')
            };
            return r;
        }
    }
}
