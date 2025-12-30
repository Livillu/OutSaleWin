using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System;

namespace WTools.warehouse
{
    internal class WarehouseTools
    {
        public string Mid { get; set; }
        public string Userid { get; set; }
        public string UserType { get; set; }
        public int Upid { get; set; }
        public int Itemid { get; set; }

        public int Quty { get; set; }
        public WarehouseTools() { }
        private void SetWarehouse()
        {
            //UserType=0.進貨 1.進貨退回 2.銷貨 3.銷退 4.領料 5.領料退回
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand("", conn1);
            cmd1.Connection.Open();
            SqlTransaction sqlTransaction = null;
            sqlTransaction = conn1.BeginTransaction();
            cmd1.Transaction = sqlTransaction;
            int InOut = 1;
            string subline = "+";
            if (UserType.ToString().Contains("124"))
            {
                InOut = -1;
                subline = "-";
            }
            //新增交易
            string sql = "INSERT INTO [PDList]([userid],[MB001],[Quty],[InOut],UserType) VALUES";
            sql += $"('{Userid}','{Mid}',{Quty},{InOut},'{UserType}')";
            cmd1.CommandText = sql;
            cmd1.ExecuteNonQuery();
            //變更商品數量
            sql = $"UPDATE [Products] SET [MB064] = [MB064]{subline}{Quty},UDate=getdate() WHERE [MB001]='{Mid}'";
            cmd1.CommandText = sql;
            cmd1.ExecuteNonQuery();
            //變更儲位數量批次
            sql = $"if(select count(*) FROM [PtLocation] where [Upid]=1 and [Itemid]=2 and [MB001]='YAF003') >0 UPDATE [PtLocation] SET [Quty] =[Quty]{subline}{Quty} WHERE [Upid] = {Upid} and [Itemid] = {Itemid} and [MB001] ='{Mid}' ";
            sql += $"else INSERT INTO [PtLocation]([Upid],[Itemid],[MB001],[Quty]) VALUES({Upid},{Itemid},'{Mid}',{Quty})";
            cmd1.CommandText = sql;
            cmd1.ExecuteNonQuery();

            try
            {
                sqlTransaction.Commit();
                MessageBox.Show("存檔完成....");
            }
            catch
            {
                sqlTransaction.Rollback();
                MessageBox.Show("存檔失敗!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
