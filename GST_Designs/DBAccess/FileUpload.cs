using GST_Designs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace GST_Designs.DBAccess
{

    public class FileUpload
    {
        private List<Card> cardLst = new List<Card>();
        private List<FileDetail> fileLst = new List<FileDetail>();
        private SqlConnection conn = new SqlConnection();

        public FileUpload()
        {
            this.conn = DatabaseConnection.GetConnection();
        }
        public List<Card> GetCardData()
        {
            SqlDataAdapter ad = new SqlDataAdapter("sp_GST_DesignsGetCard", conn);
            ad.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            ad.Fill(ds);
            conn.Close();

            cardLst = ds.Tables[0].AsEnumerable().Select(
                    DataRow => new Card
                    {
                        CardId = DataRow.Field<int>("CardId"),
                        Title = DataRow.Field<string>("Title"),
                        Description = DataRow.Field<string>("Description")
                    }).ToList();
            return cardLst;
        }

        public List<FileDetail> GetFileData()
        {
            SqlDataAdapter adFile = new SqlDataAdapter("sp_GST_DesignsGetFileDetails", conn);
            adFile.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet dsFile = new DataSet();
            adFile.Fill(dsFile);
            conn.Close();

            fileLst = dsFile.Tables[0].AsEnumerable().Select(
                    DataRow => new FileDetail
                    {
                        Id = DataRow.Field<Guid>("GuidId"),
                        FileName = DataRow.Field<string>("FileName"),
                        Extension = DataRow.Field<string>("Extension"),
                        CardId = DataRow.Field<int>("CardId")
                    }).ToList();

            return fileLst;
        }

        public void SaveFileDetails(Card model)
        {
            SqlCommand cmd = new SqlCommand("sp_GST_DesignsAddCard", conn);
            //cmd.CommandText = "Execute sp_GST_DesignsAddCard @Title,@Description";
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter outVal = new SqlParameter("@newid", SqlDbType.Int);
            outVal.Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Title", SqlDbType.VarChar, 200).Value = model.Title;
            cmd.Parameters.Add("@Description", SqlDbType.VarChar, 500).Value = model.Description;
            cmd.Parameters.Add(outVal);

            cmd.ExecuteNonQuery();

            int cardId;
            if (outVal.Value != DBNull.Value)
            {
                SqlCommand cmdFile = new SqlCommand("sp_GST_DesignsAddFileDetails", conn);
                cmdFile.CommandType = CommandType.StoredProcedure;
                cardId = Convert.ToInt32(outVal.Value);
                foreach (var item in model.FileDetails)
                {
                    cmdFile.Parameters.Add("@GuidId", SqlDbType.UniqueIdentifier).Value = item.Id;
                    cmdFile.Parameters.Add("@FileName", SqlDbType.VarChar, 200).Value = item.FileName;
                    cmdFile.Parameters.Add("@Extension", SqlDbType.VarChar, 100).Value = item.Extension;
                    cmdFile.Parameters.Add("@newid", SqlDbType.Int).Value = cardId;
                    cmdFile.ExecuteNonQuery();
                }
            }


            conn.Close();
        }

        public int DeleteRecord(Card support)
        {
            if (!String.IsNullOrEmpty(support.Description))
            {
                string d = HostingEnvironment.MapPath(Path.Combine("/Content/assets/img/admin/", support.Description));
                //string fullpath = Path.Combine("/Content/assets/img/admin/", "53.jpg");
                if (File.Exists(d))
                {
                    File.Delete(d);
                }
            }

            SqlCommand cmd = new SqlCommand("sp_GST_DesignsDeleteCard", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CardId", SqlDbType.Int).Value = support.CardId;
            int result = cmd.ExecuteNonQuery();
            conn.Close();



            return result;
        }

    }
}