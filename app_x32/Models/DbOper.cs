using MySql.Data.EntityFramework;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Entity;

namespace Cps_x32.Models
{
    internal class DbOperation
    {
        MySqlConnection connection;

        private CommonFunc m_common = new CommonFunc();

        public DbOperation() { }

        /// <summary>
        /// Get the data of the lotnumber table
        /// 
        /// SQL: 
        /// SELECT `l`.`Id`, `l`.`coupler_number`, `l`.`status_id`, `l`.`weightStation_id`, `w`.`Id`, `w`.`Description`, `w`.`Name`
        /// FROM `lotnumber` AS `l`
        /// INNER JOIN `weightstation` AS `w` ON `l`.`weightStation_id` = `w`.`Id`
        /// WHERE `l`.`status_id` = 0
        /// 
        /// Obtain the station name and lot number from it
        /// 
        /// </summary>
        /// <param name="treeView"></param>
        public void GetLotData(TreeView treeView)
        {
            int i = 0;

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString", "bak");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                // Gets and prints all Lotnumbers in database
                var lotNumbers = context.LotNumbers
                  .Include(ls => ls.WeightStation)
                  .Where(m => m.LotnumStatusId == 0);


                foreach (var lotNumber in lotNumbers)
                {
                    var data = new StringBuilder();
                    //Console.WriteLine(data.ToString());
                    treeView.Nodes.Add(lotNumber.WeightStation.Name);
                    treeView.Nodes[i].Nodes.Add(lotNumber.CouplerNumber.ToString());
                    i++;
                }
            }

            connection.Close();
        }

        /// <summary>
        /// Get the data of the dispatchtemporary table
        /// 
        /// SQL:
        /// SELECT `d`.`car_number`, `d`.`lot_id`, `d`.`arrive_id`, `d`.`breed_id`, `d`.`car_model`, `d`.`carry_weight`, `d`.`Consist`, `d`.`is_obsoleted`, `d`.`past_date`, `d`.`past_time`, `d`.`pl_weight`, `d`.`self_weight`, `d`.`total_weight`, `d`.`Weight`
        /// FROM `dispatchtemporary` AS `d`
        /// WHERE(`d`.`lot_id` = @__iLotNumberId_0) AND(`d`.`car_number` = '4306258')
        /// 
        /// Get the arrival date and time from it
        /// 
        /// </summary>
        /// <param name="iLotNumberId">(integer) lotNumberId</param>
        /// <param name="carNumber"></param>
        /// <returns></returns>
        public String GetCarInfo(int iLotNumberId, String carNumber)
        {
            String strResult = "Info: ";

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var dispatchStores = context.DispatchStores.Where(lot => lot.LotNumberId == iLotNumberId)
                    .Where(cn => cn.CarNumber == carNumber);

                foreach (var dispatchStore in dispatchStores)
                {
                    strResult += dispatchStore.PastDate.ToShortDateString();
                    strResult += " " + dispatchStore.PastTime;
                }
            }

            connection.Close();

            return strResult;
        }

        /// <summary>
        /// Get the car number data
        ///
        /// </summary>
        /// <param name="iLotNumberId"></param>
        /// <param name="iType">0:description; 1:name</param>
        /// <returns></returns>
        public String[] GetCarsData(int iLotNumberId, int iType)
        {
            String[] result = null;
            String separator = ".";

            if (iType == 0 ) 
            {
                separator = "-";
            }

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var dispatchStores = context.DispatchStores
                    .Where(lot => lot.LotNumberId == iLotNumberId);

                foreach (var dispatchStore in dispatchStores)
                {
                    result = new String[] {
                        dispatchStore.TotalWeight.ToString(),
                        dispatchStore.Weight.ToString(),
                        dispatchStore.CarModel.ToString(),
                        dispatchStore.CarNumber.ToString(),
                        dispatchStore.CarryWeight.ToString(),
                        dispatchStore.SelfWeight.ToString(),
                        dispatchStore.PlWeight.ToString(),
                        //dispatchStore.BreedCoalId.ToString(),
                        GetBreedCoalInfo(dispatchStore.BreedCoalId, iType),
                        //dispatchStore.ArriveStationId.ToString(),
                        GetArriveStationInfo(dispatchStore.ArriveStationId, iType),
                        m_common.GetDateFormat(dispatchStore.PastDate, separator),
                        dispatchStore.PastTime.ToString()
                    };
                }

            }

            connection.Close();

            return result;
        }

        /// <summary>
        /// Get zero-multiple car numbers
        /// 
        /// Only car number field data.
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="iLotNumId"></param>
        /// <param name="bObsolete"></param>
        public void GetCarNums(TreeView treeView, int iLotNumId, bool bObsolete)
        {
            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var dispatchStores = context.DispatchStores
                    .Where(num => num.LotNumber.Id == iLotNumId)
                    .Where(o => o.IsObsoleted == bObsolete)
                    .Where(c => c.Consist != -1);


                foreach (var dispatchStore in dispatchStores)
                {
                    var data = new StringBuilder();
                    treeView.SelectedNode.Nodes.Add(dispatchStore.CarNumber);
                }
            }

            connection.Close();
        }

        /// <summary>
        /// Get lot number id from lotnumber table
        /// 
        /// </summary>
        /// <param name="iWsId">(integer) weight station id</param>
        /// <param name="iCouplerNumber">(integer) coupler number</param>
        /// <returns>lotnumber's id</returns>
        public int GetLotNumId(int iWsId, int iCouplerNumber)
        {
            int i = 0;

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var lotNums = context.LotNumbers
                    .Where(ws => ws.WeightStationId == iWsId)
                    .Where(cn => cn.CouplerNumber == iCouplerNumber);

                foreach (var lotNum in lotNums)
                {
                    i = lotNum.Id;
                    break;
                }
            }

            connection.Close();

            return i;
        }

        /// <summary>
        /// Get lot number id from dispatchtemporary table
        /// 
        /// </summary>
        /// <param name="carNumber"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <returns>lot_id</returns>
        public int GetLotNumId4complex(String carNumber, String date, String time)
        {
            int result = 0;

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var dispatchStores = context.DispatchStores
                    .Where(d => d.CarNumber == carNumber)
                    .Where(d => d.PastTime == time)
                    .Where(d => d.PastDate == DateTime.Parse(date));

                foreach (var dispatchStore in dispatchStores)
                {
                    result = dispatchStore.LotNumberId;
                }
            }

            connection.Close();

            return result;
        }

        /// <summary>
        /// Get Description or Name from ArriveStation table
        /// 
        /// </summary>
        /// <param name="iArrvieStationId"></param>
        /// <param name="iType">0:description; 1:name</param>
        /// <returns></returns>
        private String GetArriveStationInfo(int iArrvieStationId, int iType)
        {
            var result = "";

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var arrvieStations = context.ArriveStations
                    .Where(a => a.Id == iArrvieStationId);

                if (iType == 0) // Get Description
                {
                    foreach (var arrvieStation in arrvieStations)
                    {
                        result = arrvieStation.Description;
                    }
                }

                if (iType == 1) // Get Name
                {
                    foreach (var arrvieStation in arrvieStations)
                    {
                        result = arrvieStation.Name;
                    }
                }
            }

            connection.Close();

            return result;
        }

        /// <summary>
        /// Get Description or Name from BreedCoal table
        /// 
        /// </summary>
        /// <param name="iBreedCoalId"></param>
        /// <param name="iType">0:description; 1:name</param>
        /// <returns></returns>
        private String GetBreedCoalInfo(int iBreedCoalId, int iType)
        {
            var result = "";

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var breedCoals = context.BreedCoals.Where(b => b.Id == iBreedCoalId);

                if (iType == 0) // Get Description
                {
                    foreach (var breedCoal in breedCoals)
                    {
                        result = breedCoal.Description;
                    }
                }
                if (iType == 1) // Get Name
                {
                    foreach (var breedCoal in breedCoals)
                    {
                        result = breedCoal.Name;
                    }
                }
            }

            connection.Close();

            return result;
        }

        /// <summary>
        /// Get Id from WeightStation table
        /// 
        /// </summary>
        /// <param name="wsName">(string) weight station name</param>
        /// <returns>weightstation's id</returns>
        public int GetWeightStationId(String wsName)
        {
            int i = 0;

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var weightStations = context.WeightStations.Where(n => n.Name == wsName);

                foreach (var weightStation in weightStations)
                {
                    i = weightStation.Id;
                    break;
                }
            }

            connection.Close();

            return i;
        }

        /// <summary>
        /// Get Consist from the DispatchStore table
        /// using the max() of SQL
        /// </summary>
        /// <returns></returns>
        public int GetConsistMax()
        {
            int result = 0;

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                result = context.DispatchStores.Max(d => d.Consist);
            }

            connection.Close();

            return result;
        }

        /// <summary>
        /// Get Consist from the DispatchStore table
        /// using the count() of SQL
        /// </summary>
        /// <param name="nLotnum"></param>
        /// <param name="nConsist"></param>
        /// <returns></returns>
        public int GetCarCount(int nLotnum, int nConsist)
        {
            int result = 0;

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                if (nConsist > -1)
                    result = context.DispatchStores.Count(l => l.LotNumberId == nLotnum);
                else
                    result = context.DispatchStores.Count(
                        l => (l.LotNumberId == nLotnum) && (l.Consist == nConsist)
                    );
            }

            connection.Close();

            return result;
        }

        /// <summary>
        /// Set Consist Value in DispatchStore table
        /// 
        /// </summary>
        /// <param name="lotNum"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public int SetConsistValue(int lotNum, int val)
        {
            int result = 0;

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var dispatchTemps = context.DispatchStores.Where(d => d.LotNumberId == lotNum)
                    .First();

                dispatchTemps.Consist = val;

                result = context.SaveChanges();
            }

            connection.Close();

            return result;
        }

        /// <summary>
        /// Set Obsoleted Value in DispatchStore table
        /// 
        /// </summary>
        /// <param name="lotNum"></param>
        /// <param name="carnum"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public int SetObsoletedValue(int lotNum, String carnum, bool flag)
        {
            int result = 0;

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var dispatchTemps = context.DispatchStores.Where(d => d.LotNumberId == lotNum)
                    .Where(c => c.CarNumber == carnum)
                    .First();

                dispatchTemps.IsObsoleted = flag;

                result = context.SaveChanges();
            }

            connection.Close();

            return result;
        }

        /// <summary>
        /// Set Status Id Value in LotNumber table
        /// 
        /// </summary>
        /// <param name="lotnum"></param>
        /// <param name="nStatus"></param>
        /// <returns></returns>
        public int SetLotNumberStatus(int lotnum, int nStatus)
        {
            int result = 0;

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var lotnumbers = context.LotNumbers.Where(s => s.Id == lotnum).FirstOrDefault();

                lotnumbers.LotnumStatusId = nStatus;

                result = context.SaveChanges();
            }

            connection.Close();

            return result;
        }
    }
}
