using System.Data;
using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Example2
{
    class ContactDB : DataAccessLayer,IDisposable
    {
        int limit = 5;

        SQLiteConnection con = default(SQLiteConnection);
        string cs = @"URI=file:test.db";
        public ContactDB()
        {
            con = new SQLiteConnection(cs);
            con.Open();
            PrepareDB();
        }

        public void Dispose()
        {
            con.Close();
        }

        private void ExecuteNonQuery(string commandText)
        {
            var cmd = new SQLiteCommand(con);
            cmd.CommandText = commandText;
            cmd.ExecuteNonQuery();
        }
        private void PrepareDB()
        {
            //SQLiteConnection.CreateFile("test.db");
            ExecuteNonQuery("DROP TABLE IF EXISTS contacts");
            ExecuteNonQuery("CREATE TABLE contacts(id STRING PRIMARY KEY, name TEXT, phone TEXT, address TEXT)");
        }

        public string CreateContact(ContactDTO contact)
        {
            string text = string.Format("INSERT INTO contacts(id, name, phone, address) VALUES('{0}', '{1}', '{2}', '{3}')"
                , contact.Id,
                contact.Name,
                contact.Phone,
                contact.Addr);

            ExecuteNonQuery(text);
            return contact.Id;
        }

        public bool DelecteContactById(string id)
        {
            string text = "DELETE FROM contacts WHERE id='" + id + "'";
            ExecuteNonQuery(text);
            return true;
        }

        public List<ContactDTO> GetAllContacts()
        {
            List<ContactDTO> res = new List<ContactDTO>();

            string selectSql = @"select * from contacts";
            using (SQLiteCommand command = new SQLiteCommand(selectSql, con))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                   var item = new ContactDTO
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        Phone = reader.GetString(2),
                        Addr = reader.GetString(3)
                    };

                    res.Add(item);
                }
            }
            return res;
        }

        public List<ContactDTO> GetSortedContacts()
        {
            List<ContactDTO> res = new List<ContactDTO>();

            string selectSql = @"select * from contacts order by name";
            using (SQLiteCommand command = new SQLiteCommand(selectSql, con))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = new ContactDTO
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        Phone = reader.GetString(2),
                        Addr = reader.GetString(3)
                    };

                    res.Add(item);
                }
            }
            return res;
        }

        public ContactDTO GetContactById(string id)
        {
            return null;
        }

        public List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public bool UpdateContactById(string id, ContactDTO contact)
        {
            string text = string.Format("UPDATE contacts SET name='{0}', phone='{1}', address='{2}' WHERE id == '{3}'"
                    ,contact.Name,
                    contact.Phone,
                    contact.Addr,
                    id);
            ExecuteNonQuery(text);
            return true;
        }

        public List<ContactDTO> GetContactsByName(string name)
        {
            List<ContactDTO> res = new List<ContactDTO>();

            string selectSql = "SELECT * FROM contacts WHERE name LIKE '" + name + "%'";
            using (SQLiteCommand command = new SQLiteCommand(selectSql, con))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = new ContactDTO
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        Phone = reader.GetString(2),
                        Addr = reader.GetString(3)
                    };

                    res.Add(item);
                }
            }
            return res;
        }

        public List<ContactDTO> GetContactsByPhone(string num)
        {
            List<ContactDTO> res = new List<ContactDTO>();

            string selectSql = "SELECT * FROM contacts WHERE phone LIKE '" + num + "%'";
            using (SQLiteCommand command = new SQLiteCommand(selectSql, con))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = new ContactDTO
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        Phone = reader.GetString(2),
                        Addr = reader.GetString(3)
                    };

                    res.Add(item);
                }
            }
            return res;
        }

        public List<ContactDTO> GetContactsOnePage(int num)
        {
            List<ContactDTO> res = new List<ContactDTO>();

            string selectSql = "SELECT * FROM contacts LIMIT " + (num-1)*this.limit + ", " + this.limit;
            using (SQLiteCommand command = new SQLiteCommand(selectSql, con))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = new ContactDTO
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        Phone = reader.GetString(2),
                        Addr = reader.GetString(3)
                    };

                    res.Add(item);
                }
            }
            return res;
        }

        public void changeLimit(int num)
        {
            this.limit = num;
        }
    }
}
