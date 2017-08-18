using Library.Entities.Enums;
using Library.ViewModels.BookViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataADONet.Repositories
{
    public class BookRepository
    {
        private string _dbconnectionString = "Data Source=globist.cytgchfgepr2.us-east-1.rds.amazonaws.com;Initial Catalog=GlobistProduction;User Id=Administrator;Password=od087*uasldf";
        public void AddBook(AddBookViewModel model)
        {
            if (model == null)
            {
                return;
            }
            var publicationId = InsertPublication(model);
            if (string.IsNullOrEmpty(publicationId))
            {
                return;
            }
            InsertBook(model, publicationId);

        }
        //private bool DetermineNeedAddingPublicationInPublisihngHouse(AddBookViewModel model)
        //{
        //    string[] subStrings = model.PublishingHousesIds.Split(',');
        //    foreach (var subString in subStrings)
        //    {
        //        if (subString == Errors.Error.ToString())
        //        {
        //            continue;
        //        }
        //        var checkExistPublicationInPublisihngHouse =  CheckExistPublicationInPublisihngHouse(publishingHouseId);
        //    }
        //}

        public string InsertPublication(AddBookViewModel model)
        {
            string queryString = "INSERT INTO Publications (Id, CreationDate, Name, Type)";
            queryString += " VALUES (@Id, @CreationDate, @Name, @Type)";

            using (SqlConnection connection = new SqlConnection(_dbconnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    var creationDate = DateTime.Now;
                    var id = Guid.NewGuid().ToString("N");
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@CreationDate", creationDate);
                    command.Parameters.AddWithValue("@Name", model.PublicationName == null ? string.Empty : model.PublicationName);
                    command.Parameters.AddWithValue("@Type", PublicationType.Book);
                    connection.Open();
                    command.ExecuteNonQuery();
                    return id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return string.Empty;

                }
            }
        }

        public void InsertBook(AddBookViewModel model, string publicationId)
        {
            string queryString = "INSERT INTO Books (Id, CreationDate, Author, NumberPages, PublishingYear, Publication_Id)";
            queryString += " VALUES (@Id, @CreationDate, @Author, @NumberPages, @PublishingYear, @Publication_Id)";

            using (SqlConnection connection = new SqlConnection(_dbconnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    var creationDate = DateTime.Now;
                    var id = Guid.NewGuid().ToString("N");
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@CreationDate", creationDate);
                    command.Parameters.AddWithValue("@Author", model.Author == null ? string.Empty : model.Author);
                    command.Parameters.AddWithValue("@NumberPages", model.NumberPages == null ? string.Empty : model.NumberPages);
                    command.Parameters.AddWithValue("@PublishingYear", model.PublishingYear == null ? DateTime.Now : model.PublishingYear);
                    command.Parameters.AddWithValue("@Publication_Id", publicationId);
                    command.Parameters.AddWithValue("@Type", PublicationType.Book);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }
        }


        public bool CheckExistPublicationInPublisihngHouse(string publishingHouseId)
        {
            string queryString = "SELECT COUNT(*) FROM [Table] WHERE ([user] = @user)";
            using (SqlConnection connection = new SqlConnection(_dbconnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    var creationDate = DateTime.Now;
                    command.Parameters.AddWithValue("@NumberPages", publishingHouseId);
                    connection.Open();
                    int UserExist = (int)command.ExecuteScalar();
                    if (UserExist > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }


    }
}
