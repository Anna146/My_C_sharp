using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyPinkTelephoneBook
{
    public class Contact
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
            
            phones = fields[5].Split(';');

            String[] tmp = fields[6].Split(';');
            add = new Adress[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                add[i] = new Adress(tmp[i]);
            }
        }

        
    }
}
