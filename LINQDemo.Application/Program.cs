using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace LINQDemo.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var people = ReadPeopleFromJsonFile();
            Console.WriteLine(people.First());
            Console.WriteLine(people.FirstOrDefault());//Recomended
            Console.WriteLine(people.Last());
            Console.WriteLine($"First person with Firstname start with a {people.FirstOrDefault(f => f.FirstName.ToLower().StartsWith("a"))}");
            Console.WriteLine($"Maximum salary: {people.Max(p => p.Salary)}");
            Console.WriteLine($"Average salary: {people.Average(p => p.Salary)}");
            Console.WriteLine($"Minimum salary: {people.Min(p => p.Salary)}");
            if(people.Any())
                Console.WriteLine("there are people here");
            else
                Console.WriteLine("no people here");
            Console.WriteLine("People by Age");
            var orderByBirthday = people.OrderByDescending(a => a.BirthDate)
                                        .ThenBy(p=>p.FirstName)
                                        .ToArray();
            //foreach (var item in orderByBirthday)
            //{
            //    Console.WriteLine(item);
            //}

            //Paggination
            int pageSize = 10;
            int pageNumber = 3;

            //var pagging = people.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            //foreach (var item in pagging)
            //{
            //    Console.WriteLine(item);
            //}

            //Filter
            Console.WriteLine("------------Filtering using where standard query operator");
            var p = people.Where(p => (p.BirthDate > new DateTime(1990, 1, 1)) && p.Salary > 3500)
                         .OrderBy(p => p.FirstName);
            foreach (var item in p)
            {
                Console.WriteLine(item);
            }

            //Projection
            Console.WriteLine("------------Filtering using where standard query operator");
            var pro = people.Where(p => (p.BirthDate > new DateTime(1990, 1, 1)) && p.Salary > 3500)
                         .OrderBy(p => p.FirstName)
                         .Select(p => new 
                         {
                             SocialNumber = p.Id,
                             FullName = p.FirstName + "" + p.LastName,
                             Bonus = p.Salary + ((decimal)0.1 * p.Salary),
                             p.Salary
                         });
            foreach (var item in pro)
            {
                Console.WriteLine($"Bonus:{item.Bonus} : Salary:{item.Salary}");
            }


            //Query-Syntax
            var peoplewithQuerySyna = from s in people
                                      where s.Salary > 5000 && s.City.ToLower().Equals("cairo")
                                      orderby s.Id descending
                                      //select s
                                      select new 
                                      {
                                          SocialNumber = s.Id,
                                          FullName = s.FirstName + "" + s.LastName,
                                          Bonus = s.Salary + ((decimal)0.1 * s.Salary),
                                          s.Salary

                                      };
                                     

            //Grouping  remeber the colunm u r grouping must have multilpe same values e.g City
            var peopleKey = people.GroupBy(p => p.City);
            Console.WriteLine("Group-------------------------");
            foreach (var cityGroup in peopleKey)
            {
                Console.WriteLine(cityGroup.Key);
                foreach (var item in cityGroup)
                {
                    Console.WriteLine(item);
                }
            }
        }


        static Person[] ReadPeopleFromJsonFile()
        {
            using (var reader=new StreamReader("people.json"))
            {
                string jsonFile = reader.ReadToEnd();
                var people = JsonConvert.DeserializeObject<Person[]>(jsonFile);
                return people;
            }
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public decimal Salary { get; set; }
        public DateTime BirthDate { get; set; }


        public override string ToString()
        {
            return $"Id:{Id} | Name:{FirstName} - {LastName} | City:{City} | BithDate:{BirthDate.ToShortDateString()} | Salary:{Salary,5:C2}";
        }
    }

   
}
