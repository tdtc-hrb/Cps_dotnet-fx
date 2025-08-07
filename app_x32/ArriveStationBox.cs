using Cps_x32.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cps_x32
{
    public partial class ArriveStationBox : Form
    {
        public int m_nLotnum = 0;

        MySqlConnection connection;

        private CommonFunc m_common = new CommonFunc();

        public ArriveStationBox()
        {
            InitializeComponent();
        }

        private List<String> GetArriveStationNames()
        {
            var result = new List<String>();

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var arriveStations = context.ArriveStations.OrderBy(w => w.Id).ToList();

                foreach (var arriveStation in arriveStations)
                {
                    result.Add(arriveStation.Description);
                }
            }

            return result;
        }

        private int SetArriveId(int lotnum, int nId)
        {
            int result = 0;

            string strConn = m_common.getXmlValue("cpsConfig.xml", "ConnectionString");
            connection = new MySqlConnection(strConn);
            connection.Open();

            using (var context = new PubsDbContext(connection, false))
            {
                var dispatchs = context.DispatchStores
                    .Where(d => d.LotNumberId == lotnum);

                foreach (var dispatch in dispatchs)
                {
                    dispatch.ArriveStationId = nId;
                }

                result = context.SaveChanges();
            }

            return result;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int iArriveId = comboBox1.SelectedIndex + 1000;

            if (SetArriveId(m_nLotnum, iArriveId) != 0)
                this.Close();
        }

        private void ArriveStationBox_Shown(object sender, EventArgs e)
        {
            //
            comboBox1.Items.Clear();
            foreach (var item in GetArriveStationNames())
            {
                comboBox1.Items.Add(item);
            }
        }
    }
}
