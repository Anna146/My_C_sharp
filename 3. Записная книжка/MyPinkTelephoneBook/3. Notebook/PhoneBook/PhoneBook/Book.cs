using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PhoneBook
{
    class Book
    {
        List<Contact> cons;



        public Book() 
        {
            cons = new List<Contact>();   
        }



        public void addContactConsole() 
        {
            String str;
            Console.WriteLine("Insert contact:");
            //date mm.dd.yyyy
            str = Console.ReadLine();
            cons.Add(new Contact(str));
            Console.WriteLine();
        }

        public void addContactString(String s)
        {
            cons.Add(new Contact(s));
        }




        public Contact searchFIO(String n1, String n2, String n3)
        {
            var c1 = from elem in cons
                     where elem.Name.Equals(n1)
                     where elem.Surname.Equals(n2)
                     where elem.SecName.Equals(n3)
                     select elem;
            return (Contact)c1;
        }

        public void delete(String n1, String n2, String n3)
        {
            cons.Remove(searchFIO(n1, n2, n3));
        }


        public Contact[] searchPhone(String p)
        {
            var c1 = from el in cons
                     where el.phones.Any(tel => tel.Contains(p))
                     select el;
            List<Contact> tmp = new List<Contact>();
            foreach (var c2 in c1)
                tmp.Add((Contact)c2);
            return tmp.ToArray();
        }



        public List<Contact> alphabetic()
        {
            var c1 = from el in cons
                     orderby el.Surname + el.Name + el.SecName
                     select el;
            return c1.ToList();
        }




        public Contact[] searchLetter(Char ch)
        {
            var c1 = from el in cons
                     where el.Surname[0] == ch
                     select el;
            List<Contact> tmp = new List<Contact>();
            foreach (var c2 in c1)
                tmp.Add((Contact)c2);
            return tmp.ToArray();
        }


        //all birthdays in current month 
        public Contact[] birthDay()
        {
            DateTime td = DateTime.Now;
            var c1 = from el in cons
                     where el.birth.Month == td.Month
                     select el;
            List<Contact> tmp = new List<Contact>();
            foreach (var c2 in c1)
                tmp.Add((Contact)c2);
            return tmp.ToArray();
        }



        public void toXML(String fileName) 
        {
            XDocument doc = new XDocument();
            XElement conBook = new XElement("telephoneBook");
            doc.Add(conBook);

            
            
            foreach (var elem in cons) {

                XElement person = new XElement("conact");
                conBook.Add(person);
                XElement name = new XElement("name");
                name.Value = elem.Name;
                person.Add(name);

                XElement surname = new XElement("surname");
                surname.Value = elem.Surname;
                person.Add(surname);

                XElement secname = new XElement("secname");
                secname.Value = elem.SecName;
                person.Add(secname);

                XElement bd = new XElement("birthday");
                bd.Value = elem.birth.ToString().Substring(0,10);
                person.Add(bd);

                XElement em = new XElement("emails");
                person.Add(em);
                foreach (var item in elem.emails)
                {
                    XElement mail = new XElement("email");
                    mail.Value = item;
                    em.Add(mail);
                }

                XElement tel = new XElement("telephones");
                person.Add(tel);
                foreach (var item in elem.phones)
                {
                    XElement t = new XElement("telephone");
                    t.Value = item;
                    tel.Add(t);
                }

                XElement ad = new XElement("adresses");
                person.Add(ad);
                for (int i = 0; i < elem.add.Length; i++ )
                {
                    XElement adr = new XElement("adress");
                    ad.Add(adr);
                    XElement cit = new XElement("city");
                    cit.Value = elem.add[i].City;
                    adr.Add(cit);
                    XElement str = new XElement("street");
                    str.Value = elem.add[i].Street;
                    adr.Add(str);
                    XElement hous = new XElement("house");
                    hous.Value = elem.add[i].house.ToString();
                    adr.Add(hous);
                }
            }

            doc.Save(fileName);
        }

        public void fromXML(String fileName)
        {
            XDocument doc = XDocument.Load(fileName);
            //проходим по каждому элементу в найшей library
            //(этот элемент сразу доступен через свойство doc.Root)

            
            foreach (XElement el in doc.Root.Elements())
            {
                IEnumerable<XElement> els = el.Elements();

                String tmp = els.ElementAt(0).Value + " " + els.ElementAt(1).Value + " " + els.ElementAt(2).Value + " " + els.ElementAt(3).Value + " ";

                foreach (XElement mail in els.ElementAt(4).Elements())
                {
                    tmp += mail.Value;
                    if (mail.NextNode != null)
                        tmp += ";";
                }
                tmp += " ";

                foreach (XElement tel in els.ElementAt(5).Elements())
                {
                    tmp += tel.Value;
                    if (tel.NextNode != null)
                        tmp += ";";
                }
                tmp += " ";

                foreach (XElement adr in els.ElementAt(6).Elements())
                {
                    foreach (XElement e in adr.Elements())
                    {
                        tmp += e.Value;
                        if (e.NextNode != null)
                            tmp += ",";
                    }
                    if (adr.NextNode != null)
                            tmp += ";";
                }

                Contact ct = new Contact(tmp);
                cons.Add(ct);
            }
        }

        public static void Main()
        {
            Book phbk = new Book();
            //phbk.fromXML("C:/Users/Hp/Desktop/1.txt");
            phbk.addContactString("Anna Tigunova Sergeevna 09.02.1993 belpku001@mail.ru;belpku001@gmail.com 89519256652;89151617729 Dolgoprudny,Pervomayskaya,32");
            phbk.addContactString("Tonya Parshina Olegovna 05.12.1993 tobn@mail.ru 89028313893 Dolgoprudny,Pervomayskaya,32");
            phbk.toXML("C:/Users/Hp/Desktop/1.txt");
            Contact[] f = phbk.searchPhone("89");
            int a = 10;
        }
    }

}
