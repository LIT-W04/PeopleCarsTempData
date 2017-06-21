using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCarsTempData.Data
{
    public class CarDb
    {
        private string _connectionString;

        public CarDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Car> GetCars(int personId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Cars where PersonId = @personId";
                cmd.Parameters.AddWithValue("@personId", personId);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Car> results = new List<Car>();
                while (reader.Read())
                {
                    Car car = new Car();
                    car.Id = (int)reader["Id"];
                    car.Make = (string)reader["Make"];
                    car.Model = (string)reader["Model"];
                    car.Year = (int)reader["Year"];
                    car.PersonId = (int)reader["PersonId"];
                    results.Add(car);
                }

                return results;
            }
        }

        public void AddCar(Car car)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Cars (Make, Model, Year, PersonId) " +
                                      "VALUES (@make, @model, @year, @personId)";
                command.Parameters.AddWithValue("@make", car.Make);
                command.Parameters.AddWithValue("@model", car.Model);
                command.Parameters.AddWithValue("@year", car.Year);
                command.Parameters.AddWithValue("@personId", car.PersonId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int carId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Cars WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", carId);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCarsForPerson(int personId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Cars WHERE PersonId = @id";
                cmd.Parameters.AddWithValue("@id", personId);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
