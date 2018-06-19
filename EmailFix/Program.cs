using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

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

    class Program
    {

        private const string Sender = "donotreplyfe@bhf.org.uk";
        static void Main(string[] args)
        {
            var currDir = Directory.GetCurrentDirectory();
            var path = $"{currDir}\\data.csv";

            // parse CSV file into list of Recipient objects
            List<Recipient> recipients = 
                File.ReadAllLines(path)
                    .Skip(1) // ignore the headers
                    .Select(v => Recipient.FromCsv(v)) // take one line at a time
                    .ToList();

            BuildContent(ref recipients);

            foreach (var v in recipients)
            {
                Console.Write($"Sending to {v.StoreEmail} ... ");
                SendEmail(v.StoreEmail, Sender, v.Content);
            }
            Console.WriteLine(" ALL DONE ");

            Console.ReadKey();
        }

        private static void BuildContent(ref List<Recipient> recipients)
        {
            var template = "<table><tr><td>Date Booked:</td><td>{19}" +
                           "<tr><td>Title:</td><td>{0}" +
                           "</td></tr><tr><td>First Name:</td><td>{1}" +
                           "</td></tr><tr><td>Last Name:</td><td>{2}" +
                           "</td></tr><tr><td>Telephone:</td><td>{3}" +
                           "</td></tr><tr><td>Email:</td><td>{4}" +
                           "</td></tr><tr><td>Address 1:</td><td>{5}" +
                           "</td></tr><tr><td>City/Town:</td><td>{6}" +
                           "</td></tr><tr><td>County:</td><td>{7}" +
                           "</td></tr><tr><td>Postcode:</td><td>{8}" +
                           "</td></tr><tr><td>Country:</td><td>{9}" +
                           "</td></tr><tr><td>Large items:</td><td>{10}" +
                           "</td></tr><tr><td>Medium items:</td><td>{11}" +
                           "</td></tr><tr><td>Small items:</td><td>{12}" +
                           "</td></tr><tr><td>Number of donation bags:</td><td>{13}" +
                           "</td></tr><tr><td>Contact preferences:</td><td> Email?  #CanContactByEmail#" + //MINUS
                           "<br /></td></tr><tr><td>&nbsp;</td><td> Text/MMS?  #CanContactByText#" + // MINUS
                           "<br /></td></tr><tr><td>&nbsp;</td><td> Post?  #DoNotContactByPost#" +
                           "<br /></td></tr><tr><td>&nbsp;</td><td> Phone?  #DoNotContactByPhone#" +
                           "<br /></td></tr></table><br /><br /><a href='https://bhf.org.uk/f-and-e-accept?RequestId={18}'>Accept</a><br/><a href='https://bhf.org.uk/f-and-e-decline?RequestId={18}'>Decline</a>";


            foreach (var r in recipients)
            {
                r.Content = string.Format(template, 
                    r.Title, //1
                    r.Firstname, //2
                    r.Surname,
                    r.Phone,
                    r.EmailTo, //5
                    r.Address1,
                    r.City,
                    r.County,
                    r.Postcode,
                    r.Country, //10
                    r.LargeItems,
                    r.MediumItems,
                    r.SmallItems,   
                    r.BagsNumber,
                    r.ContactEmail, //15
                    r.ContactTextMMS,
                    r.NoContactPost,
                    r.NoContactTelephone,
                    r.VanCollectionId,
                    r.CreatedDate //20
                    );

                r.Content = r.Content.Replace("#CanContactByEmail#",
                    r.ContactEmail == "1" ? "Ok to contact by Email" : "Do not contact by email");

                r.Content = r.Content.Replace("#CanContactByText#",
                    r.ContactTextMMS == "1" ? "Ok to contact by text messageor MMS" : "Do not contact by text message or MMS");

                r.Content = r.Content.Replace("#DoNotContactByPost#",
                    r.NoContactPost == "1" ? "Do not contact by post" : "Ok to contact by post");

                r.Content = r.Content.Replace("#DoNotContactByPhone#",
                    r.NoContactTelephone == "1" ? "Do not contact by telephone" : "Ok to contact by telephone ");
            }
        }

        private static void Test()
        {
            var recipientAddress = "preston-smithm@bhf.org.uk";
            var senderAddress = "preston-smithm@bhf.org.uk";
            var content =
                "<table><tr><td>Title:</td><td>Mr</td></tr><tr><td>First Name:</td><td>mark</td></tr><tr><td>Last Name:</td><td>preston-smith</td></tr><tr><td>Telephone:</td><td>07856431301</td></tr><tr><td>Email:</td><td>mark@mcgabhann.org</td></tr><tr><td>Address 1:</td><td>44 Bemsted Road</td></tr><tr><td>City/Town:</td><td>London</td></tr><tr><td>County:</td><td>Waltham Forest</td></tr><tr><td>Postcode:</td><td>E17 5JZ</td></tr><tr><td>Country:</td><td>United Kingdom</td></tr><tr><td>Large items:</td><td>1</td></tr><tr><td>Medium items:</td><td>0</td></tr><tr><td>Small items:</td><td>0</td></tr><tr><td>Number of donation bags:</td><td>0</td></tr><tr><td>Contact preferences:</td><td>Do not contact by email <br /></td></tr><tr><td>&nbsp;</td><td>Do not contact by text message or MMS <br /></td></tr><tr><td>&nbsp;</td><td>Do not contact by post <br /></td></tr><tr><td>&nbsp;</td><td>Do not contact by telephone <br /></td></tr></table><br /><br /><a href='http://bhf.local/f-and-e-accept?RequestId=24004296'>Accept</a><br/><a href='http://bhf.local/f-and-e-decline?RequestId=24004296'>Decline</a>";

            if (SendEmail(recipientAddress, senderAddress, content))
                Console.WriteLine($"email sent to {recipientAddress}");

            Console.ReadKey();
        }

        private static bool SendEmail(string recipientAddress, string senderAddress, string content)
        {
            //return false;
            return EmailHelper.SendEmail("BHF", senderAddress,  "F&E Store", recipientAddress, "Recovered online booking: please prioritise", content
                );
        }
    }
}
