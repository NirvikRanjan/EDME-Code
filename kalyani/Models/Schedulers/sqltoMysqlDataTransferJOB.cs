using MySql.Data.MySqlClient;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AutoSherpa_project.Models.Schedulers
{
    public class sqltoMysqlDataTransferJOB : IJob
    {
        Logger logger = LogManager.GetLogger("apkRegLogger");
        DataTable dt = new DataTable();
        public async Task Execute(IJobExecutionContext context)
        {
            long? lastidinserted = 0;
            logger.Info("\n\n -------- Pulling data from mssql to autosherpa -------------AT  : " + DateTime.Now);
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE trans_bitly_karix");
                  
                }
                long totalCount = counttotalRecords(lastidinserted);

                logger.Info("\n\n -------- Total Records in trans_bitly_karix{0} /  Server Last Record {1}-------------|" , totalCount,lastidinserted);

                for (int i = 0; i <= totalCount; i = i + 50000)
                {
                    try
                    {
                        //logger.Info("\n\n -------- Loop Started {0} - Total Count {1} ----------",i);
                        using (var db = new AutoSherDBContext())
                        {
                            lastidinserted = db.Database.SqlQuery<int>("SELECT id FROM trans_bitly_karix order by id desc limit 1;").FirstOrDefault();
                        }
                        logger.Info("\n\n -------- Last Inserted Id ----------{0}", lastidinserted);

                        dt = new DataTable();
                        fetchSqlRead(0, 50000, lastidinserted);
                        insrtMysql();
                        //logger.Info("\n\n -------- Loop Ended {0} - Total Count {1} ----------", i);

                    }
                    catch (Exception ex)
                    {
                        string exception = "";
                        if (ex.Message.Contains("inner exception"))
                        {
                            exception = ex.InnerException.Message;
                        }
                        else
                        {
                            exception = ex.Message;
                        }
                        logger.Info("\n\n --------  Extraction Error Inside Loop{0}  Breaking loop-------------: ",exception);
                       // goto skipfor;
                    }
                }
                //skipfor: logger.Info("\n\n --------  Done-------------");
                 logger.Info("\n\n --------  Done-------------");
                batchENDinsert(0, 0);
                using (var db = new AutoSherDBContext())
                {
                    db.Database.ExecuteSqlCommandAsync("call HERO_LOAD_RATECARDTABLE();");
                }
            }
            catch (Exception ex)
            {
                string exception = "";
                if (ex.Message.Contains("inner exception"))
                {
                    exception = ex.InnerException.Message;
                }
                else
                {
                    exception = ex.Message;
                }
                logger.Info("\n\n --------  Extraction Error Outer Exeption{0} -------------: ", exception);
            }
            logger.Info("\n\n -------- Pulling data from mssql to autosherpa Ended -------------AT  : " + DateTime.Now);

        }


        public long counttotalRecords(long ? lastidinserted)
        {
            long totalCount = 0;
            //string constring = @"Data Source=CHETANPOOJARY1\SQLEXPRESS;Initial Catalog=Test;uid='sa';password='sa@12345';Connection Timeout=600;MultipleActiveResultSets=true";
            string constring = @"Data Source=10.200.1.74;Initial Catalog=herocorp;uid='autosherpa';password='P@ssw0rd@123';Connection Timeout=6000;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("select count(*) from trans_bitly_karix  TK(nolock)  where  TK.request_type = 'heroninja' and IS_Renewal=0 and  Previous_Policy_Expiry >= dateadd( day,-95, CONVERT(date,GETDATE()))", con))
                {
                    con.Open();
                    totalCount = (int)cmd.ExecuteScalar();
                    con.Close();
                }
            }
            return totalCount;
        }

        public DataTable fetchSqlRead(int fromlimit, int tolimit,long? lastidinserted)
        {
            logger.Info("\n\n -------- Data Extraction Started from MSSQl  From Limit-{0} - To Limit {1}-------------  : " ,fromlimit,tolimit);

            string constring = @"Data Source=10.200.1.74;Initial Catalog=herocorp;uid='autosherpa';password='P@ssw0rd@123';Connection Timeout=6000;MultipleActiveResultSets=true";
            //string constring = @"Data Source=CHETANPOOJARY1\SQLEXPRESS;Initial Catalog=Test;uid='sa';password='sa@12345';Connection Timeout=600;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM trans_bitly_karix TK(nolock)  where TK.id >"+ lastidinserted + " and TK.request_type = 'heroninja' and IS_Renewal=0 and  Previous_Policy_Expiry >= dateadd( day,-95, CONVERT(date,GETDATE())) order by id asc OFFSET " + fromlimit + " ROWS FETCH NEXT " + tolimit+ " ROWS ONLY", con))
                {
                    cmd.CommandTimeout = 300;
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
            logger.Info("\n\n -------- Data Extraction Ended from MSSQl  From Limit-{0} - To Limit {1}-------------  : ", fromlimit, tolimit);

            return dt;
        }
        public void insrtMysql()
        {
            string firstCellValue = dt.Rows[0][0].ToString();
            string lastCellValue = dt.Rows[dt.Rows.Count - 1][0].ToString();
           // long batchId = batchStartinsert(Convert.ToInt64(firstCellValue));

            logger.Info("\n\n -------- Inserting to MYSQl  From Index {0}  To Index  {1} -------------: ",firstCellValue,lastCellValue);


            DataColumn newCol = new DataColumn("batchId", typeof(long));
            newCol.AllowDBNull = true;
            dt.Columns.Add(newCol);

            //if (dt.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        dr["batchId"] = batchId;
            //    }
            //}
            //string rows = BulkInsert(ref dt, "trans_bitly_karix_Test");
            string rows = BulkInsert(ref dt, "trans_bitly_karix");
            //logger.Info("\n\n -------- Inserting to MYSQl data{0} -------------|AT: ", rows);

            string conStr = ConfigurationManager.ConnectionStrings["AutoSherContext"].ConnectionString;
            //string conStr = ConfigurationManager.ConnectionStrings[AutoSherDBContext.getContextName()].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(conStr))
            {
                connection.Open();
                MySqlCommand myCmd = new MySqlCommand(rows, connection);
                myCmd.ExecuteNonQuery();

                connection.Close();
            }
            //batchENDinsert(batchId,Convert.ToInt64(lastCellValue));
            logger.Info("\n\n -------- Done Inserting to MYSQl  From Index {0} To Index {1}-------------: ",  firstCellValue, lastCellValue);

        }
        public long batchStartinsert(long fromlimit)
        {
            long batchId = 0;
            using (var db = new AutoSherDBContext())
            {
                batchstart startbatch = new batchstart();
                startbatch.startedDate = DateTime.Now.ToString("dd/MM/yyyy");
                startbatch.startedTime = DateTime.Now.ToString("hh:mm");
                startbatch.fromlimit = fromlimit;
                db.Batchstarts.Add(startbatch);
                db.SaveChanges();
                batchId = startbatch.id;
            }
            return batchId;
        }
        public long batchENDinsert(long startbatchId,long tolimit)
        {
            long batchId = 0;
            using (var db = new AutoSherDBContext())
            {
                batchend endbatch = new batchend();
                endbatch.endDate = DateTime.Now.ToString("dd/MM/yyyy");
                endbatch.endTime = DateTime.Now.ToString("hh:mm");
               // endbatch.startbatchId =Convert.ToInt32(startbatchId);
               // endbatch.tolimit =tolimit;
                db.Batchends.Add(endbatch);
                db.SaveChanges();
            }
            return batchId;
        }
        public String BulkInsert(ref DataTable table, String table_name)
        {
            try
            {
                StringBuilder queryBuilder = new StringBuilder();
                DateTime dt;

                queryBuilder.AppendFormat("INSERT INTO `{0}` (", table_name);

                // more than 1 column required and 1 or more rows
                if (table.Columns.Count > 1 && table.Rows.Count > 0)
                {
                    // build all columns
                    queryBuilder.AppendFormat("`{0}`", table.Columns[0].ColumnName);

                    if (table.Columns.Count > 1)
                    {
                        for (int i = 1; i < table.Columns.Count; i++)
                        {
                            queryBuilder.AppendFormat(", `{0}` ", table.Columns[i].ColumnName);
                        }
                    }

                    queryBuilder.AppendFormat(") VALUES (");

                    // build all values for the first row
                    // escape String & Datetime values!
                    if (table.Columns[0].DataType == typeof(String))
                    {
                        queryBuilder.AppendFormat("'{0}'", MySqlHelper.EscapeString(table.Rows[0][table.Columns[0].ColumnName].ToString()));
                    }
                    else if (table.Columns[0].DataType == typeof(DateTime))
                    {
                        if (table.Rows[0][table.Columns[0].ColumnName].ToString() != "" && table.Rows[0][table.Columns[0].ColumnName] != DBNull.Value)
                        {
                            dt = (DateTime)table.Rows[0][table.Columns[0].ColumnName];
                            queryBuilder.AppendFormat("'{0}'", dt.ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            queryBuilder.AppendFormat("null");

                        }
                    }
                    else if (table.Columns[0].DataType == typeof(Int32) )
                    {
                        queryBuilder.AppendFormat("{0}", table.Rows[0].Field<Int32?> (table.Columns[0].ColumnName) ?? 0);
                    }
                    else if (table.Columns[0].DataType == typeof(Int64))
                    {
                        queryBuilder.AppendFormat("{0}", table.Rows[0].Field<Int64?> (table.Columns[0].ColumnName) ?? 0);
                    }
                    else if (table.Columns[0].DataType == typeof(decimal))
                    {
                        queryBuilder.AppendFormat("{0}", table.Rows[0].Field<decimal?> (table.Columns[0].ColumnName) ?? 0);
                    }
                    else
                    {
                        queryBuilder.AppendFormat("{0}", table.Rows[0][table.Columns[0].ColumnName].ToString());
                    }

                    for (int i = 1; i < table.Columns.Count; i++)
                    {
                        // escape String & Datetime values!
                        if (table.Columns[i].DataType == typeof(String))
                        {
                            queryBuilder.AppendFormat(", '{0}'", MySqlHelper.EscapeString(table.Rows[0][table.Columns[i].ColumnName].ToString()));
                        }
                        else if (table.Columns[i].DataType == typeof(DateTime))
                        {
                            if (table.Rows[0][table.Columns[i].ColumnName].ToString() != "" && table.Rows[0][table.Columns[i].ColumnName] != DBNull.Value)
                            {
                                dt = (DateTime)table.Rows[0][table.Columns[i].ColumnName];
                                queryBuilder.AppendFormat(", '{0}'", dt.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                queryBuilder.AppendFormat(", null");
                            }

                        }
                        else if (table.Columns[i].DataType == typeof(Int64))
                        {
                            queryBuilder.AppendFormat(", {0}", table.Rows[0].Field<Int64?>(table.Columns[i].ColumnName) ?? 0);
                        }
                        else if (table.Columns[i].DataType == typeof(Int32))
                        {
                            queryBuilder.AppendFormat(", {0}", table.Rows[0].Field<Int32?>(table.Columns[i].ColumnName) ?? 0);
                        }
                        else if (table.Columns[i].DataType == typeof(decimal))
                        {
                            queryBuilder.AppendFormat(", {0}", table.Rows[0].Field<decimal?>(table.Columns[i].ColumnName) ?? 0);
                        }
                        else
                        {
                            queryBuilder.AppendFormat(", {0}", table.Rows[0][table.Columns[i].ColumnName].ToString());
                        }
                    }

                    queryBuilder.Append(")");
                    queryBuilder.AppendLine();

                    // build all values all remaining rows
                    if (table.Rows.Count > 1)
                    {
                        // iterate over the rows
                        for (int row = 1; row < table.Rows.Count; row++)
                        {
                            // open value block
                            queryBuilder.Append(", (");

                            // escape String & Datetime values!
                            if (table.Columns[0].DataType == typeof(String))
                            {
                                queryBuilder.AppendFormat("'{0}'", MySqlHelper.EscapeString(table.Rows[row][table.Columns[0].ColumnName].ToString()));
                            }
                            else if (table.Columns[0].DataType == typeof(DateTime))
                            {
                                if (table.Rows[row][table.Columns[0].ColumnName].ToString() != "" && table.Rows[row][table.Columns[0].ColumnName] != DBNull.Value)
                                {
                                    dt = (DateTime)table.Rows[row][table.Columns[0].ColumnName];
                                    queryBuilder.AppendFormat("'{0}'", dt.ToString("yyyy-MM-dd"));
                                }
                                else
                                {
                                    queryBuilder.AppendFormat(", null");
                                }
                            }
                            else if (table.Columns[0].DataType == typeof(Int32))
                            {
                                queryBuilder.AppendFormat("{0}", table.Rows[row].Field<Int32?>(table.Columns[0].ColumnName) ?? 0);
                            }
                            else if (table.Columns[0].DataType == typeof(Int64))
                            {
                                queryBuilder.AppendFormat("{0}", table.Rows[row].Field<Int64?>(table.Columns[0].ColumnName) ?? 0);
                            }
                            else if (table.Columns[0].DataType == typeof(decimal))
                            {
                                queryBuilder.AppendFormat("{0}", table.Rows[row].Field<decimal?>(table.Columns[0].ColumnName) ?? 0);
                            }
                            else
                            {
                                queryBuilder.AppendFormat(", {0}", table.Rows[row][table.Columns[0].ColumnName].ToString());
                            }

                            for (int col = 1; col < table.Columns.Count; col++)
                            {
                                // escape String & Datetime values!
                                if (table.Columns[col].DataType == typeof(String))
                                {
                                    queryBuilder.AppendFormat(", '{0}'", MySqlHelper.EscapeString(table.Rows[row][table.Columns[col].ColumnName].ToString()));
                                }
                                else if (table.Columns[col].DataType == typeof(DateTime))
                                {
                                    if (table.Rows[row][table.Columns[col].ColumnName].ToString() != "" && table.Rows[row][table.Columns[col].ColumnName] != DBNull.Value)
                                    {
                                        dt = (DateTime)table.Rows[row][table.Columns[col].ColumnName];
                                        queryBuilder.AppendFormat(", '{0}'", dt.ToString("yyyy-MM-dd"));
                                    }
                                    else
                                    {
                                        queryBuilder.AppendFormat(", null");

                                    }
                                }
                                else if (table.Columns[col].DataType == typeof(Int32))
                                {
                                    queryBuilder.AppendFormat(", {0}", table.Rows[row].Field<Int32?>(table.Columns[col].ColumnName) ?? 0);
                                }
                                else if (table.Columns[col].DataType == typeof(Int64))
                                {
                                    queryBuilder.AppendFormat(", {0}", table.Rows[row].Field<Int64?>(table.Columns[col].ColumnName) ?? 0);
                                }
                                else if (table.Columns[col].DataType == typeof(decimal))
                                {
                                    queryBuilder.AppendFormat(", {0}", table.Rows[row].Field<decimal?>(table.Columns[col].ColumnName) ?? 0);
                                }
                                else
                                {
                                    queryBuilder.AppendFormat(", {0}", table.Rows[row][table.Columns[col].ColumnName].ToString());
                                }
                            } // end for (int i = 1; i < table.Columns.Count; i++)

                            // close value block
                            queryBuilder.Append(")");
                            queryBuilder.AppendLine();

                        } // end for (int r = 1; r < table.Rows.Count; r++)

                        // sql delimiter =)
                        queryBuilder.Append(";");

                    } // end if (table.Rows.Count > 1)

                    return queryBuilder.ToString();
                }
                else
                {
                    return "";
                } // end if(table.Columns.Count > 1 && table.Rows.Count > 0)
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

    }
}