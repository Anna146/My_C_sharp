using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PhoneBook
{
    class Contact
    {
        public String Name;
        public String Surname;
        public String SecName;

        public DateTime birth;
        public String[] emails;
        public String[] phones;
        public Adress[] add;

        public class Adress
        {
            public String City;
            public String Street;
            public int house;

            public Adress(String s)
            {
                String[] fields = new String[3];
                fields = s.Split(',');
                City = fields[0];
                Street = fields[1];
                house = int.Parse(fields[2]);
            }
        }



        public Contact(String s)
        {
            String[] fields = new String[7];
            fields = s.Split(' ');
            Name = fields[0];
            Surname = fields[1];
            SecName = fields[2];
            birth = DateTime.Parse(fields[3]);

            emails = fields[4].Split(';');
            while (!validate(emails, "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}"))
            {
                Console.WriteLine("Your emails are incorrect. Try again");
                String tp = Console.ReadLine();
                emails = tp.Split(';');
            }

            phones = fields[5].Split(';');
            //checks if matches (999)9999999
            /*while (!validate(phones, "@^((\\(\\d{3}\\) ?)|(\\d{3}-))?\\d{3}-\\d{4}$"))
            {
                Console.WriteLine("Your telephones are incorrect. Try again");
                String tp = Console.ReadLine();
                phones = tp.Split(';');
            }*/

            String[] tmp = fields[6].Split(';');
            add = new Adress[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                add[i] = new Adress(tmp[i]);
            }
        }

        public bool validate(String[] strs, String ptr)
        {
            Regex rx = new Regex(ptr);

            foreach (String s in strs) 
            {
                if (!rx.IsMatch(s))
                    return false;
            }
            return true;
        }
    }
}
