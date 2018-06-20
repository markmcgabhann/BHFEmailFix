using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EmailFix
{
    class Program
    {
        private const string Sender = "donotreplyfe@bhf.org.uk";

        static void Main(string[] args)
        {
            try
            {
                var currDir = Directory.GetCurrentDirectory();
                var path = $"{currDir}\\csv\\data.csv";

                if (!File.Exists(path))
                {
                    Console.WriteLine($"File doesn't exist, check {path}");
                    Console.ReadKey();
                    return;
                }

                // parse CSV file into list of Recipient objects
                List<Recipient> recipients =
                    File.ReadAllLines(path)
                        .Skip(1) // ignore the headers
                        .Select(Recipient.FromCsv) // take one line at a time
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }

        private static void BuildContent(ref List<Recipient> recipients)
        {
            try
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

                if (recipients.Count == 0)
                {
                    Console.WriteLine("data.csv contains no rows");
                    Console.ReadKey();
                }
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
           
        }

        private static bool SendEmail(string recipientAddress, string senderAddress, string content)
        {
            return EmailHelper.SendEmail("BHF", senderAddress,  "F&E Store", recipientAddress, "Recovered online booking: please prioritise", content);
        }
    }
}
