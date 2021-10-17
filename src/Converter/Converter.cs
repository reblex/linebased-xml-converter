using System;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Converter
{
    class Converter
    {
        /// <summary>
        /// Convert LineBased stored information containing people and their families contact information to XML.
        /// </summary>
        /// <param name="inputFile">Input file.</param>
        /// <param name="outputFile">Output file.</param>
        public static void LineBasedToXML(string inputFile, string outputFile)
        {
            Console.WriteLine(inputFile + " => " + outputFile);
            
            // Load from file into objects
            List<Person> people = LoadLineBased(inputFile);

            // Add all together as XML
            XElement xml = new XElement("people");
            foreach (Person person in people)
            {
                XElement personXML = person.ToXML();
                xml.Add(personXML);
            }

            // Save to File
            xml.Save(outputFile);
        }

        /// <summary>
        /// Load from LineBased frile into Person() object structure.
        /// </summary>
        /// <param name="inputFile">Input file.</param>
        /// <returns>List of Person() objects</returns>
        private static List<Person> LoadLineBased(string inputFile)
        {
            char[] acceptedLineTypes = { 'P', 'T', 'A', 'F' };
            List<Person> people = new List<Person>();
            Person person = new Person();  
            int familyMemberIndex = -1;                         // -1 = No family members as of yet.
            int lineCount = 0;

            foreach (string line in System.IO.File.ReadLines(inputFile))
            {
                string[] segments = line.Split('|');            // "P|Simon|Wahlström" => [P, Simon, Wahlström]
                char lineType = segments[0].ToCharArray()[0];   // Type of line (P/T/A/F) as defined by first char on line

                // Ensure lineType is OK
                if (Array.IndexOf(acceptedLineTypes, lineType) == -1)
                {
                    Console.WriteLine("Incorrect file structure \"" + lineType + "\" at: ", line);
                    Environment.Exit(1);
                }
   
                // Extract data from line into array, ignoring line type
                string[] data = new ArraySegment<string>(segments, 1, segments.Length-1).ToArray();

                // Move data into Person() object
                switch (lineType)
                {
                    case 'P':
                        if (lineCount > 0)
                        {
                            people.Add(person);
                            person = new Person();
                            familyMemberIndex = -1;
                        }
                        
                        person.AddNames(data);
                        break;
                    
                    case 'T':
                        person.AddPhone(data, familyMemberIndex);
                        break;

                    case 'A':
                        person.AddAddress(data, familyMemberIndex);
                        break;
                    
                    case 'F':
                        familyMemberIndex++;
                        
                        person.AddFamilyMember(data);
                        break;                    
                }

                lineCount++;
            }

            // Add last person
            people.Add(person);

            return people;
        }
    }
}