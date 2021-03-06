﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example2
{
    interface DataAccessLayer
    {
        ContactDTO GetContactById(string id);
        string CreateContact(ContactDTO contact);
        bool UpdateContactById(string id, ContactDTO contact);
        bool DelecteContactById(string id);
        void changeLimit(int num);
        List<ContactDTO> GetContactsByName(string name);
        List<ContactDTO> GetContactsByPhone(string num);
        List<ContactDTO> GetContactsOnePage(int num);
        List<ContactDTO> GetAllContacts();
        List<ContactDTO> GetSortedContacts();
    }

    abstract class BaseContact
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Addr { get; set; }
    }

    class CreateContactCommand:BaseContact {
   
    }

    class ContactDTO : BaseContact
    {
        public string Id { get; set; }
    }

    class BLL
    {
        DataAccessLayer dal = default(DataAccessLayer);
        public BLL(DataAccessLayer dal)
        {
            this.dal = dal;
        }
        public ContactDTO GetContact(string id)
        {
            return dal.GetContactById(id);
        }
        public string CreateContact(CreateContactCommand contact)
        {
            ContactDTO contact1 = new ContactDTO();
            contact1.Id = Guid.NewGuid().ToString();
            contact1.Name = contact.Name;
            contact1.Addr = contact.Addr;
            contact1.Phone = contact.Phone;
            return dal.CreateContact(contact1);
        }

        public bool DelecteContact(string id)
        {
            return dal.DelecteContactById(id);
        }

        public bool UpdateContectById(string id, CreateContactCommand contact)
        {
            ContactDTO contact1 = new ContactDTO();
            //contact1.Id = Guid.NewGuid().ToString();
            contact1.Name = contact.Name;
            contact1.Addr = contact.Addr;
            contact1.Phone = contact.Phone;
            dal.UpdateContactById(id, contact1);
            return true;
        }

        public List<ContactDTO> GetContacts()
        {
            return dal.GetAllContacts();
        }

        public List<ContactDTO> GetSortedContacts()
        {
            return dal.GetSortedContacts();
        }

        public List<ContactDTO> GetContactsByName(string name)
        {
            return dal.GetContactsByName(name);
        }

        public List<ContactDTO> GetContactsByPhone(string num)
        {
            return dal.GetContactsByPhone(num);
        }

        public List<ContactDTO> GetContactsOnePage(int num)
        {
            return dal.GetContactsOnePage(num);
        }

        public void changeLimit(int num)
        {
            dal.changeLimit(num);
        }
    }
}
