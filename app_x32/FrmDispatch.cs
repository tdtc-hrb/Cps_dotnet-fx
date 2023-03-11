using Cps_x32.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cps_x32
{
    public partial class FrmDispatch : Form
    {
        /// <summary>
        /// lot number id
        /// </summary>
        private int m_lotnum = 0;

        /// <summary>
        /// ListView is sn
        /// </summary>
        private int m_sn = 1;

        /// <summary>
        /// carnumber string
        /// </summary>
        private String m_carnumber = "1";

        /// <summary>
        /// station is id
        /// </summary>
        private int m_stationId = 1;

        /// <summary>
        /// coupler number
        /// </summary>
        private int m_couplerNumber = 1;

        /// <summary>
        /// DB oper class
        /// </summary>
        private DbOperation m_dbOper = new DbOperation();

        public FrmDispatch()
        {
            InitializeComponent();
        }

        private void FrmDispatch_Shown(object sender, EventArgs e)
        {
            treeView1.BeginUpdate();
            // Clear the TreeView each time the method is called.
            treeView1.Nodes.Clear();
            m_dbOper.GetLotData(treeView1);
            treeView1.EndUpdate();
        }

        /// <summary>
        /// Operate window controls
        /// Export data from ListView
        /// 
        /// </summary>
        /// <param name="listView"></param>
        /// <returns></returns>
        private StringBuilder ExportData(ListView listView)
        {
            var result = new StringBuilder();
            int iCountItems = listView.Items.Count;

            for (int j = 0; j < iCountItems; j++)
            {
                int iCount = listView.Items[j].SubItems.Count;
                int i = 0;

                foreach (var item in listView.Items[j].SubItems)
                {
                    String str = listView.Items[j].SubItems[i].Text;

                    // Determine whether the character is ASCII
                    // IsASCII(str);

                    result.Append(str);
                    if (iCount > 1)
                    {
                        result.Append(',');
                    }
                    i++;
                    iCount--;
                }

                if (iCountItems - j > 1)
                {
                    //result.AppendLine();
                    result.Append(Environment.NewLine);
                }

            }

            return result;
        }

        /// <summary>
        /// Operate window controls
        /// The double-click event is at the level of the TreeView's Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var iLevel = e.Node.Level;
            var count = e.Node.Nodes.Count;

            if (count == 0 && iLevel == 1)
            {
                var strChild = e.Node.Text;
                var strParent = e.Node.Parent.Text;
                m_couplerNumber = int.Parse(strChild);

                m_stationId = m_dbOper.GetWeightStationId(strParent);
                m_lotnum = m_dbOper.GetLotNumId(m_stationId, m_couplerNumber);
                m_dbOper.GetCarNums(treeView1, m_lotnum, false);
            }

            if (iLevel == 2)
            {
                var strParent = e.Node.Parent.Text;
                var strGrandParent = e.Node.Parent.Parent.Text;
                m_carnumber = e.Node.Text;
                m_couplerNumber = int.Parse(strParent);

                m_stationId = m_dbOper.GetWeightStationId(strGrandParent);
                m_lotnum = m_dbOper.GetLotNumId(m_stationId, m_couplerNumber);
                infoLabel.Text = m_dbOper.GetCarInfo(m_lotnum, m_carnumber);
            }

        }

        /// <summary>
        /// Operate window controls
        /// Right click event at the level of TreeView's Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var iLevel = e.Node.Level;

            if (e.Button == MouseButtons.Right && iLevel == 2)
            {
                var strGrandParent = e.Node.Parent.Parent.Text;
                var strParent = e.Node.Parent.Text;

                m_couplerNumber = int.Parse(strParent);
                m_stationId = m_dbOper.GetWeightStationId(strGrandParent);
                m_lotnum = m_dbOper.GetLotNumId(m_stationId, m_couplerNumber);
                m_carnumber = e.Node.Text;

                contextMenuStrip2.Show(Cursor.Position);
            }

            if (e.Button == MouseButtons.Right && iLevel == 1)
            {
                m_couplerNumber = int.Parse(e.Node.Text);
                m_stationId = m_dbOper.GetWeightStationId(e.Node.Parent.Text);
                m_lotnum = m_dbOper.GetLotNumId(m_stationId, m_couplerNumber);

                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        /// <summary>
        /// Operate window controls
        /// change arrival
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ArriveStationBox arriveBox = new ArriveStationBox();
            arriveBox.m_nLotnum = m_lotnum;
            arriveBox.ShowDialog();
        }

        /// <summary>
        /// Operate window controls
        /// add new marshalling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int iCount = 0;
            // update
            m_dbOper.SetConsistValue(m_lotnum, -1);
            // show in ListView
            var data = new List<string>();
            // export data in ListView
            var expData = new List<string>();

            data.Add(m_sn.ToString());
            expData.Add(m_sn.ToString());

            foreach (var item in m_dbOper.GetCarsData(m_lotnum, 0))
            {
                data.Add((string)item);
            }

            foreach (var item in m_dbOper.GetCarsData(m_lotnum, 1))
            {
                expData.Add((string)item);
            }

            /// print
            listView1.Items.Add(new ListViewItem(data.ToArray()));

            /// export data
            expListView.Items.Add(new ListViewItem(expData.ToArray()));

            m_sn++;

            iCount = m_dbOper.GetCarCount(m_lotnum, 0) - m_dbOper.GetCarCount(m_lotnum, -1);

            if (iCount == 0)
            {
                m_dbOper.SetLotNumberStatus(m_lotnum, 1);
            }

            if (treeView1.SelectedNode != null)
            {
                TreeNode tn = treeView1.SelectedNode;
                treeView1.Nodes.Remove(tn);
            }
        }

        /// <summary>
        /// Data reporting function:
        /// Divided into two parts:
        /// First, generate a csv file;
        /// Second, insert the current data into the TotalTable data table.
        /// 
        /// Note: the current version only implements the first part!
        /// </summary>
        /// <seealso cref="https://github.com/tdtc-hrb/zcc2_cps/blob/main/Udispatch.pas#L386"/>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCsv_Click(object sender, EventArgs e)
        {
            String exportFile = "C:/send/myTest.txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(exportFile))
            {
                toolStripStatusLabel2.Text = "Please wait...";
                String strData = ExportData(expListView).ToString();
                toolStripStatusLabel1.Text = "Exporting data...";
                file.Write(strData);
                toolStripStatusLabel1.Text = "Export data done";
            }

            /// invoke https://li_guibin@bitbucket.org/li_guibin/commsrv2
            /// git clone git@bitbucket.org:li_guibin/commsrv2.git
            String strArgs = "-Dfile.encoding=UTF8";
            strArgs += " -classpath \"C:\\commsrv2;C:\\commsrv2\\lib\\log4j-1.2.17.jar;C:\\commsrv2\\lib\\xom-1.1.jar\"";
            strArgs += " Server";
            strArgs += " C:\\receive\\configQ.xml";
            ProcessStartInfo startInfo = new ProcessStartInfo("java.exe", strArgs);
            toolStripStatusLabel2.Text = "Running CMD...";
            Process.Start(startInfo);
        }

        /// <summary>
        /// Operate window controls
        /// Delete the data in the row from the ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeVehicle_Click(object sender, EventArgs e)
        {
            String strVehicleNumber = "";
            String strDate = "";
            String strNewDate = "";
            String strTime = "";

            toolStripStatusLabel1.Text = "Removing data...";
            // remove listView item
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Selected)
                {
                    strVehicleNumber = listView1.Items[i].SubItems[4].Text;
                    strDate = listView1.Items[i].SubItems[10].Text;
                    strTime = listView1.Items[i].SubItems[11].Text;

                    toolStripStatusLabel2.Text = strVehicleNumber;
                    toolStripStatusLabel3.Text = strDate + " " + strTime;

                    listView1.Items[i].Remove();
                }
            }

            strNewDate = DateTime.Parse(strDate).ToString("yyyy-MM-dd");

            toolStripStatusLabel1.Text = "Remove data done";

            // get lotnum
            m_lotnum = m_dbOper.GetLotNumId4complex(strVehicleNumber, strNewDate, strTime);

            // update consist
            m_dbOper.SetConsistValue(m_lotnum, 0);

            // update lotnumber status
            m_dbOper.SetLotNumberStatus(m_lotnum, 0);
        }

        /// <summary>
        /// set Obsoleted flag and remove TreeView current's Node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void abandonedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // set Obsoleted flag
            m_dbOper.SetObsoletedValue(m_lotnum, m_carnumber, true);
            // remove TreeView'Node
            if (treeView1.SelectedNode != null)
            {
                TreeNode tn = treeView1.SelectedNode;
                treeView1.Nodes.Remove(tn);
            }
        }

        /// <summary>
        /// print preview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            //
            listView1.Title = "print preview";
            listView1.PageSetup();
            listView1.PrintPreview();
        }

    }
}
