using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCarsTempData.Data
{
    public class PersonDb
    {
        private string _connectionString;

        public PersonDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<PersonWithCarCount> GetPeople()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT p.*, COUNT(c.Id) as CarCount
                                    FROM People p
                                    LEFT JOIN Cars c
                                    ON p.Id = c.PersonId
                                    GROUP BY p.Id, p.FirstName, p.LastName, p.Age";
                connection.Open();
                List<PersonWithCarCount> result = new List<PersonWithCarCount>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PersonWithCarCount person = new PersonWithCarCount
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Age = (int)reader["Age"],
                        CarCount = (int)reader["CarCount"]
                    };
                    result.Add(person);
                }

                return result;
            }
        }

        public void AddPerson(Person person)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO People (FirstName, LastName, Age) " +
                                  "VALUES (@firstName, @lastName, @age); SELECT @@Identity";
                cmd.Parameters.AddWithValue("@firstName", person.FirstName);
                cmd.Parameters.AddWithValue("@lastName", person.LastName);
                cmd.Parameters.AddWithValue("@age", person.Age);
                connection.Open();
                person.Id = (int)(decimal)cmd.ExecuteScalar();
            }
        }

        public void Delete(int personId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM People WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", personId);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Person GetPerson(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM People WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }

                Person person = new Person
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    Age = (int)reader["Age"]
                };

                return person;
            }
        }
    }
}
