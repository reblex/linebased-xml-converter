using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace Converter
{
    /// <summary>
    /// Person Class used as middle ground for converting between stored data types. 
    /// Currently LineBased to XML.
    /// </summary>
    public class Person
    {
        //
        // Locales
        //
        private string firstname;
        private string surname;
        private Phone phone;
        private Address address;
        private List<FamilyMemeber> familyMembers;
        
        /// <summary>
        /// Person() Constructor
        /// </summary>
        public Person()
        {
            phone = new Phone();
            address = new Address();
            familyMembers = new List<FamilyMemeber>();
        }

        //
        // Internal Data Structures
        //
        private class Phone
        {
            public string mobile;
            public string home;
        }

        private class Address
        {
            public string street;
            public string city;
            public string zip;
        }

        private class FamilyMemeber
        {
            public string name;
            public string born;
            public Address address;
            public Phone phone;
        }

        /*
        * Methods
        */
        
        
        /// <summary>
        /// Add first- and surname member variables.
        /// </summary>
        /// <param name="data">First- and surname as array[2]</param>
        public void AddNames(string[] data)
        {
            firstname = data.ElementAtOrDefault(0);
            surname = data.ElementAtOrDefault(1);
        }

        /// <summary>
        /// Add phone numbers to Person or related family member object.
        /// </summary>
        /// <param name="data">Mobile and Home numbers as array[2]</param>
        /// <param name="familyIndex">Optional: If given, indexes of 0 or higher adds phone numbers to indexed family member</param>
        public void AddPhone(string[] data, int familyIndex = -1)
        {            
            Phone phone = familyIndex != -1 ? familyMembers[familyIndex].phone : this.phone;
            phone.mobile = data.ElementAtOrDefault(0);
            phone.home = data.ElementAtOrDefault(1);
        }

        /// <summary>
        /// Add address to Person or related family member object.
        /// </summary>
        /// <param name="data">Street, City, and Zip, as array[3]</param>
        /// <param name="familyIndex">Optional: If given, indexes of 0 or higher adds address information to indexed family member</param>
        public void AddAddress(string[] data, int familyIndex = -1)
        {
            Address address = familyIndex != -1 ? familyMembers[familyIndex].address : this.address;
            address.street = data.ElementAtOrDefault(0);
            address.city = data.ElementAtOrDefault(1);
            address.zip = data.ElementAtOrDefault(2);
        }
        

        /// <summary>
        /// Create a family member object associated with this person.
        /// </summary>
        /// <param name="data">Name and birth year, as array[2]</param>
        public void AddFamilyMember(string[] data)
        {
            FamilyMemeber family = new FamilyMemeber();
            family.phone = new Phone();
            family.address = new Address();
            family.name = data.ElementAtOrDefault(0);
            family.born = data.ElementAtOrDefault(1);

            familyMembers.Add(family);
        }

        /// <summary>
        /// Generate XML structure from Person()-object including family member info.
        /// </summary>
        /// <returns>XElement object</returns>
        public XElement ToXML()
        {
            XElement person = new XElement("person",
                new XElement("firstname", firstname),
                new XElement("lastname", surname),
                new XElement("address",
                    new XElement("street", address.street),
                    new XElement("city", address.city),
                    new XElement("zip", address.zip)),
                new XElement("phone",
                    new XElement("mobile", phone.mobile),
                    new XElement("home", phone.home)),
                familyMembers.Select(family =>
                    new XElement("family",
                        new XElement("name", family.name),
                        new XElement("born", family.born),
                        new XElement("address",
                            new XElement("street", family.address.street),
                            new XElement("city", family.address.city),
                            new XElement("zip", family.address.zip)),
                        new XElement("phone",
                            new XElement("mobile", family.phone.mobile),
                            new XElement("home", family.phone.home)))));

            return person;
        }
    }
}