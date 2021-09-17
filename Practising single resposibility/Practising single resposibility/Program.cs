using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Practising_single_resposibility
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\josephb\OneDrive - UCAS\Documents\C# Programming\Practising single resposibility\Practising single resposibility\Transactions2014.csv";
            BankTransactions bankTransactions = new BankTransactions();
            List<Person> people = bankTransactions.RetrieveTransactions(path);
            string input = null;
            string holderName = null;
            while (input == null)
            {
                Console.WriteLine("Enter what you want to do: ");
                Console.WriteLine("1.) List All\n2.) List [account holder]");
                input = Console.ReadLine();
                if (input == "1")
                {
                    foreach (Person person in people)
                    {
                        HoldOutput(person);
                        Console.WriteLine();
                    }
                    Console.Read();
                }
                else if (input == "2")
                {
                    Console.WriteLine("PLease enter the name of the account holder: ");
                    holderName = Console.ReadLine();
                    foreach (Person person in people)
                    {
                        if (person.Name == holderName)
                        {
                            HoldOutput(person);
                            Console.Read();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect Input. Please input either '1' or '2'");
                    input = null;
                }
            }
        }

        static void HoldOutput(Person person)
        {
            Console.WriteLine("\nName: {0}, Balance: {1}", person.Name, person.Balance);
            foreach (var transaction in person.userTans)
            {
                Console.WriteLine("Date: {0}, From: {1}, To: {2}, Comment{3}, Amount: {4}", transaction.date, transaction.fromName, transaction.toName, transaction.narrative, transaction.amount);
            }
        }
    }

    
    // This class is used to setr the variables for the transactions to be printed in main
    class Transaction
    {
        public DateTime date;
        public string fromName;
        public string toName;
        public string narrative;
        public double amount;

        public Transaction(DateTime theDate, String nameFrom, String nameTo, String theNarrative, double theAmount)
        {
            this.date = theDate;
            this.fromName = nameFrom;
            this.toName = nameTo;
            this.narrative = theNarrative;
            this.amount = theAmount;
        }


    }


    // Person class is used to create the list of peoples names.
    class Person
    {
        public string Name;
        public double Balance;

        public List<Transaction> userTans = new List<Transaction>();



        public Person(string namePerson)
        {
            this.Name = namePerson;
            this.Balance = 0;
        }
    }


    //BankTransactions is used to populate the different lists and reutrn all of that to the Main function through people
    class BankTransactions
    {

        //Below is the constructor
        public BankTransactions() { }


        //Below is the Function that retrieves the transactions from the csv file
        public List<Person> RetrieveTransactions(string path)
        {
            List<Person> people = new List<Person>();

            var logArray = File.ReadAllLines(path).Skip(1);
            List<Transaction> tranList = new List<Transaction>();
            List<string> names = new List<string>();

            foreach (string log in logArray)
            {
                string[] logEntry = log.Split(',');
                if (!names.Contains(logEntry[1]))
                {
                    names.Add(logEntry[1]);
                }

                Transaction newLog = new Transaction(DateTime.Parse(logEntry[0]), logEntry[1], logEntry[2], logEntry[3], double.Parse(logEntry[4]));
                tranList.Add(newLog);
            }

            foreach (string name in names)
            {
                people.Add(new Person(name));
            }

            foreach (Transaction trans in tranList)
            {
                Person sender;
                Person reciever;

                foreach (Person person in people)
                {
                    if (person.Name == trans.fromName)
                    {
                        sender = person;
                        sender.Balance -= trans.amount;
                        sender.userTans.Add(trans);
                    }

                    if (person.Name == trans.toName)
                    {
                        reciever = person;
                        reciever.Balance += trans.amount;
                        reciever.userTans.Add(trans);
                    }
                }
            }

            return people;
        }
    }
}