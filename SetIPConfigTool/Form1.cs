using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.DirectoryServices;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Threading;

namespace SetIPConfigTool
{
    public partial class MainForm : Form
    {
        //存储获取网卡的数组
        List<NetworkInterface> adapters = new List<NetworkInterface>();
        NetworkInterface[] tmpadapters; 
        //存储获取的网卡的IP
        List<string> dbIPList = new List<string>();
        //存储获取的网卡的IP的子网掩码
        List<string> dbSubList = new List<string>();
        //存储删除的记录
        List<string> deletedIPList = new List<string>();
        List<string> deletedSubList = new List<string>();
        //存数增加的记录
        List<string> addedIPList = new List<string>();
        List<string> addedSubList = new List<string>();
        //最后配置的数组
        List<string> setIPList = new List<string>();
        List<string> setSubList = new List<string>();
        
        //记录选择的网卡
        static int indexOfAdapter;

        public MainForm()
        {
            InitializeComponent();
        }

        //窗体加载
        private void MainForm_Load(object sender, EventArgs e)
        {
            dataGridView.AutoSize = true;
            dataGridView.Location = new System.Drawing.Point(91, 63);
            //设置网卡列表提示信息
            try
            {
                //初始化DataGridView
                initDataGridView();
                clearCachesChangeList();
                clearCachesDBList();  
                getAdaptersList();
                if (adapters != null)
                {
                    indexOfAdapter = 0;
                    comboBox_NetCard.SelectedIndex = 0;
                    updateDataGridView();
                    textBox_NetMask.Text = "255.255.255.0";
                    btn_CopyIPInfo.Enabled = true;
                    btn_AddIP.Enabled = true;
                    btn_Set.Enabled = true;
                }
                else
                {
                    btn_CopyIPInfo.Enabled = false;
                    btn_AddIP.Enabled = false;
                    btn_Set.Enabled = false;
                }
                

            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        //退出按钮按下
        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            try {
                clearCachesChangeList();
                clearCachesDBList();
                getAdaptersList();
                if (adapters != null)
                {
                    indexOfAdapter = 0;
                    comboBox_NetCard.SelectedIndex = 0;
                    updateDataGridView();
                    textBox_NetMask.Text = "255.255.255.0";
                    btn_CopyIPInfo.Enabled = true;
                    btn_AddIP.Enabled = true;
                    btn_Set.Enabled = true;
                }
                else
                {
                    btn_CopyIPInfo.Enabled = false;
                    btn_AddIP.Enabled = false;
                    btn_Set.Enabled = false;
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }    
        }

        //选择一项网卡
        private void comboBox_NetCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (adapters != null)
            {
                
                indexOfAdapter = comboBox_NetCard.SelectedIndex;
                textBox_GateWay.Text = "未获得网关信息，请修改";
                //选择了新的网卡，更新缓存数组信息
                clearCachesDBList();
                clearCachesChangeList();
                //更新界面
                updateDataGridView();
                getNewIPOnGridView();
            }
            
        }
       

        //拷贝IP信息按钮
        private void btn_CopyIPInfo_Click(object sender, EventArgs e)
        {
            
            try {
                string tmp = "";
                for (int indexOfRow = 0; indexOfRow < dataGridView.Rows.Count-1; indexOfRow++)
                {
                    
                    for (int indexOfCol = 0; indexOfCol < dataGridView.Columns.Count-1; indexOfCol++)
                    {
                        tmp += dataGridView.Rows[indexOfRow].Cells[indexOfCol].Value.ToString() + "   ";
                    }
                    tmp += "\r\n";
                }
                Clipboard.SetDataObject(tmp);
            }catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            
        }

        //点击添加IP按钮
        private void btn_AddIP_Click(object sender, EventArgs e)
        {
            try
            {
                //去掉输入框的空格
                textBox_AddIP.Text.Trim();
                textBox_EndNet.Text.Trim();
                textBox_NetMask.Text.Trim();
                //判断输入内容的正误
                string strIP = textBox_AddIP.Text, strEndNet = textBox_EndNet.Text, strSubMaskNet = textBox_NetMask.Text;
                if (strIP == "")
                {
                    MessageBox.Show("错误：IP地址不能为空", "提示");
                    textBox_AddIP.Text = "";
                    textBox_EndNet.Text = "";
                    return;
                }
                else if (IsRightIP(strIP) == false)
                {
                    MessageBox.Show("错误：您填入的不是IP地址，请填入正确的IP地址！", "错误");
                    textBox_AddIP.Text = "";
                    return;
                }
                else if(strEndNet == "")//单个IP
                {
                    //插入增加数组
                    addToList(strIP, strSubMaskNet);
                    //去除删除数组里新增的IP
                    removeAddedIPInDeletedList(strIP, strSubMaskNet);
                    //更新DBList
                    dbIPList.Add(strIP);
                    dbSubList.Add(strSubMaskNet);
                    //更新界面
                    updateDataGridView();
                    clearInputTextBox();
                    textBox_NetMask.Text = "255.255.255.0";
                    textBox_AddIP.Focus();
                    
                }
                else if ((IsNUmberStr(strEndNet) == false) || int.Parse(strEndNet) < getLastIP(strIP) | int.Parse(strEndNet) < 1 || int.Parse(strEndNet) > 255)
                {
                    MessageBox.Show("主机号的范围为1-255", "错误");
                    textBox_EndNet.Text = "";
                    return;
                }
                else if (strSubMaskNet == "")
                {
                    MessageBox.Show("错误:子网掩码不能为空！", "错误");
                    return;
                }
                else if (IsRightIP(strSubMaskNet) == false)
                {
                    MessageBox.Show("错误：您填入的不是IP子网掩码，请填入正确的子网掩码！", "错误");
                    textBox_NetMask.Text = "";
                    return;
                }
                else //输入无误，存入addList,更新dbList数组
                {
                    if (getindexOfSectionNumber(strIP, 4).Equals(int.Parse(strEndNet)))//单个IP地址
                    {
                        //插入增加数组
                        addToList(strIP, strSubMaskNet);
                        //更新DBList
                        dbIPList.Add(strIP);
                        dbSubList.Add(strSubMaskNet);
                        //去除删除数组里新增的IP
                        removeAddedIPInDeletedList(strIP, strSubMaskNet);
                        //更新界面
                        updateDataGridView();
                        clearInputTextBox();
                        textBox_NetMask.Text = "255.255.255.0";
                    }
                    else//多个IP
                    {
                        strIP += "-" + strEndNet;
                        //插入增加数组
                        addToList(strIP, strSubMaskNet);
                        //去除删除数组里新增的IP
                        removeAddedIPInDeletedList(strIP, strSubMaskNet);
                        //更新DBList
                        dbIPList.Add(strIP);
                        dbSubList.Add(strSubMaskNet);
                        //更新界面
                        updateDataGridView();
                        clearInputTextBox();
                        textBox_NetMask.Text = "255.255.255.0";
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }        
        }

        //点击删除按钮
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try 
            {
                int indexOfRow = e.RowIndex;
                int indexOfColumn = e.ColumnIndex;
                int countOfRows = dataGridView.Rows.Count-1;//除去最下面的空行
                //只有点击参数按钮做出反应,并且dataGridView中有数据
                if (indexOfColumn == 2 && indexOfRow<countOfRows)
                {
                    
                    //更新数据数组
                    string strIP = dataGridView.Rows[indexOfRow].Cells[0].Value.ToString();
                    string strSubNetMask = dataGridView.Rows[indexOfRow].Cells[1].Value.ToString();
                    deleteToList(strIP, strSubNetMask);
                    //去除增加数组里新去除的IP
                    removeDeletedIPInAddedList(strIP, strSubNetMask);
                    dbIPList.Remove(strIP);
                    dbSubList.Remove(strSubNetMask);
                    //更新UI
                    updateDataGridView();

                }
            }
            catch(Exception error)
            {
                MessageBox.Show(error.ToString());
            }
            
        }
        //点击配置应用按钮
        private void btn_Set_Click(object sender, EventArgs e)
        {
            string gateWay = textBox_GateWay.Text.Trim();
            if (IsRightIP(gateWay) || gateWay=="")
            {
                if (IslimitAccess() == false)
                {
                    try
                    {
                        if (addedIPList.Count != 0 || deletedIPList.Count != 0)
                        {
                            string strTip = "";
                            if (deletedIPList.Count != 0)
                            {
                                strTip = "确定删除以下IP?\n";
                                for (int index = 0; index < deletedIPList.Count; index++)
                                {
                                    strTip += "IP: " + deletedIPList[index] + '\n' + "子网掩码： " + deletedSubList[index] + '\n';
                                }
                            }
                            if (addedIPList.Count != 0)
                            {
                                strTip += "确定添加以下IP?\n";
                                for (int index = 0; index < addedIPList.Count; index++)
                                {
                                    strTip += "IP: " + addedIPList[index] + '\n' + "子网掩码： " + addedSubList[index] + '\n';
                                }
                            }

                            DialogResult dr = MessageBox.Show(strTip, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr == DialogResult.OK)
                            {
                                getSettingList();
                                SetIPConfig(gateWay);
                                clearCachesDBList();
                                clearCachesChangeList();
                                writeSingleAddressToConToDB(getIPAddressInfoColletion(indexOfAdapter, 0), getIPAddressInfoColletion(indexOfAdapter, 1));
                                updateDataGridView();
                                //更新界面
                                textBox_AddIP.Text = "";
                                textBox_EndNet.Text = "";
                            }
                            else//不执行此次配置
                            {
                                clearCachesDBList();
                                clearCachesChangeList();
                                //得到当前网卡的IP信息
                                writeSingleAddressToConToDB(getIPAddressInfoColletion(indexOfAdapter, 0), getIPAddressInfoColletion(indexOfAdapter, 1));
                                updateDataGridView();
                            }
                        }
                        else
                        {
                            MessageBox.Show("您还没有进行任何更改：）", "提示");
                        }
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.ToString());
                    }
                }
                else//不具备操作权限
                {
                    clearCachesDBList();
                    clearCachesChangeList();
                    //得到当前网卡的IP信息
                    writeSingleAddressToConToDB(getIPAddressInfoColletion(indexOfAdapter, 0), getIPAddressInfoColletion(indexOfAdapter, 1));
                    updateDataGridView();
                }
            }
            else
            {
                MessageBox.Show("对不起，请输入正确的网关地址！","提示");
            }
            
        }

/******************************************网络操作方法************************************************/

        //获取Adapters列表
       public void getAdaptersList()
        {
            tmpadapters = null;
            tmpadapters = NetworkInterface.GetAllNetworkInterfaces();
            //获得网卡列表
            adapters.Clear();
           //判断网卡类型
            foreach (NetworkInterface nt in tmpadapters)
            {
                if ((nt.NetworkInterfaceType == NetworkInterfaceType.Ethernet) || (nt.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                {
                    adapters.Add(nt);
                }
            }
            //判断是否获得网卡
            if (adapters == null)
            {
                MessageBox.Show("未检测到网卡信息，请刷新！", "网卡检测结果");
            }
            else
            {
                int numOfAdapters = adapters.Count;
                int index = 0;
                comboBox_NetCard.Items.Clear();

                foreach (NetworkInterface adapter in adapters)
                {
                    index++;
                    //显示网络适配器描述  
                    comboBox_NetCard.Items.Add(adapter.Description);
                }
            }
        }

        //根据网卡获取当前网卡IP信息,返回的是单个IP
        public List<string> getIPAddressInfoColletion(int indexOfAdpaters,int IPOrSubNetMask)
        {
           //获得网卡列表
            adapters = new List<NetworkInterface>(NetworkInterface.GetAllNetworkInterfaces());
            List<string> addressList = new List<string>();
            List<string> subMaskList = new List<string>();
            //获取以太网网卡接口信息
            IPInterfaceProperties ipInterfaceProper = adapters[indexOfAdapter].GetIPProperties();
            //获取单播地址集
            UnicastIPAddressInformationCollection ipCollection = ipInterfaceProper.UnicastAddresses;
            foreach (UnicastIPAddressInformation ipAddress in ipCollection)
            {
                //判断是否为IPV4地址
                if (ipAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    addressList.Add(ipAddress.Address.ToString());
                    if (ipAddress.IPv4Mask != null)
                    {
                        subMaskList.Add(ipAddress.IPv4Mask.ToString());
                    }
                    else
                    {
                        subMaskList.Add("");
                    }

                }
            }
            if (IPOrSubNetMask == 0)//返回IP
            {
                return addressList;
            }else
            {
                return subMaskList;
            }
        }
      
       

       //获取网管信息
        public string getGateWayInfo()
        {
            //获取以太网网卡接口信息
            IPInterfaceProperties ipInterfaceProper = adapters[indexOfAdapter].GetIPProperties();
            string strIPGateWay = "";
            //获取网关信息
            GatewayIPAddressInformationCollection gateWays = ipInterfaceProper.GatewayAddresses;
            foreach (var gateWay in gateWays)
            {
                //如果能够Ping通网关
                if (IsPingIP(gateWay.Address.ToString()))
                {
                    //得到网关地址
                    strIPGateWay = gateWay.Address.ToString();
                    //跳出循环
                }
            }
            if ((strIPGateWay != "") && (gateWays != null))
            {
                return strIPGateWay;
            }
            else
            {
                return strIPGateWay = "获取网关失败";
            }  
        }

        public void SetIPConfig(string strGateWay)
        {
            ManagementClass findAdapters = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection adaptersCollections = findAdapters.GetInstances();
            foreach (ManagementObject adpater in adaptersCollections)
            {
                if ((string)adpater["Description"] == adapters[indexOfAdapter].Description && (bool)adpater["IPEnabled"]==true)
                {
                    if (setIPList.Count != 0 &&  setSubList.Count != 0)
                    {
                        //设置IP
                        string[] ip = new string[setIPList.Count];
                        string[] mask = new string[setSubList.Count];
                        setIPList.CopyTo(ip);
                        setSubList.CopyTo(mask);
                        try
                        {
                            ManagementBaseObject newIP = adpater.GetMethodParameters("EnableStatic");
                            newIP["IPAddress"] = ip;
                            newIP["SubnetMask"] = mask;
                            ManagementBaseObject setIP = adpater.InvokeMethod("EnableStatic",newIP,null);
                            UInt32 result = (UInt32)(setIP["returnValue"]);
                            returnWrongMessage(result);
                        }
                        catch (Exception error)
                        {
                            MessageBox.Show(error.ToString());
                        }
                        //设置网关地址
                        ManagementBaseObject newGateWay = adpater.GetMethodParameters("SetGateWays");
                        newGateWay["DefaultIPGateway"] = new string[] { strGateWay };
                        ManagementBaseObject setGateWay = adpater.InvokeMethod("SetGateways", newGateWay,null);
                        UInt32 ret = (UInt32)(setGateWay["returnValue"]);
                    }
                    else//如果IP为空则禁止此次配置
                    {
                        MessageBox.Show("该网卡IP地址不能为空，此次操作失败：）","提示");
                        updateDataGridView();
                    }                    
                }
            }
        }
        //判断权限
        public bool IslimitAccess()
        {
            //检查系统权限
            System.Security.Principal.WindowsIdentity wid = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal p = new System.Security.Principal.WindowsPrincipal(wid);
            bool isAdmin = (p.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator));
            if (isAdmin == false)
            {
                MessageBox.Show("您目前登录的账号不具有管理员权限，请确认后使用应用：）", "权限警告", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return true;
            }
            else
            {
                return false;
            }
        }
        //获得IP更新界面
        public void getNewIPOnGridView()
        {
            List<string> addressList = new List<string>();
            List<string> subMaskList = new List<string>();

            //获取当前网卡信息
            addressList = getIPAddressInfoColletion(indexOfAdapter, 0);
            subMaskList = getIPAddressInfoColletion(indexOfAdapter, 1);
            if (addressList.Count == 0)
            {
                MessageBox.Show("当前网卡无单播IPv地址或者您尚未配置IP地址，请重试！", "提示");
            }
            else//检测到IPv4地址正确信息，更新窗体
            {

                //将得到得IP存入DBList
                writeSingleAddressToConToDB(addressList, subMaskList);

                //更新DataridView
                updateDataGridView();

                //获取网关信息
                textBox_GateWay.Text = getGateWayInfo();
            }
            btn_AddIP.Enabled = true;
            btn_Set.Enabled = true;
        }
   /*------------------------------------------------UI方法-------------------------------------------*/
        //初始化dataGridView
        public void initDataGridView()
        {
            dataGridView.Columns.Clear();
            //添加IP列
            DataGridViewTextBoxColumn colIP = new DataGridViewTextBoxColumn();
            colIP.HeaderText = "IP";
            colIP.ReadOnly = true;
            dataGridView.Columns.Add(colIP);
            //添加子网掩码列
            DataGridViewTextBoxColumn colMask = new DataGridViewTextBoxColumn();
            colMask.HeaderText = "子网掩码";
            colMask.ReadOnly = true;
            dataGridView.Columns.Add(colMask);
            //添加操作栏
            DataGridViewButtonColumn colBtnDel = new DataGridViewButtonColumn();
            colBtnDel.HeaderText = "操作";
            dataGridView.Columns.Add(colBtnDel);
        }
        //更新dataGridView
        public void updateDataGridView()
        {
            try
            {
                dataGridView.Rows.Clear();
                //循环添加行
                for (int i = 0; i < dbIPList.Count; i++)
                {
                    int index = i + 1;
                    dataGridView.Rows.Add();
                    dataGridView.Rows[i].Cells[0].Value = dbIPList[i];
                    dataGridView.Rows[i].Cells[1].Value = dbSubList[i];
                    dataGridView.Rows[i].Cells[2].Value = "删除";
                    dataGridView.Rows[i].HeaderCell.Value = index.ToString();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
        //清理输入框
        public void clearInputTextBox()
        {
            textBox_AddIP.Text = "";
            textBox_EndNet.Text = "";
            textBox_NetMask.Text = "";
        }
        //设置界面得按钮属性
        public void setButtonsEnabled(bool updateBtn,bool copyIPBtn,bool addBtn,bool SetBtn)
        {
            btn_Update.Enabled = updateBtn;
            btn_CopyIPInfo.Enabled = copyIPBtn;
            btn_AddIP.Enabled =addBtn;
            btn_Set.Enabled = addBtn;
            btn_Exit.Enabled = true;

        }
        /*-----------------------------------------数组字符串方法---------------------------------------------*/
        
        //返回正确格式的16进制地址
        public string getRightMacAdress(string adapterMac)
        {
            if (adapterMac == "")
            {
                adapterMac = "无Mac地址";
            }
            else
            {
                try
                {
                    //将一串字符转换为十六进制Mac地址
                    string[] strArray = new string[6];
                    for (int i = 0; i < 6; i++)
                    {
                        strArray[i] = adapterMac.Substring(2 * i, 2);
                    }
                    adapterMac = "";
                    int index = 0;
                    while (index < 5)
                    {
                        adapterMac += strArray[index] + ":";
                        index++;
                    }
                    adapterMac += strArray[index];
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                }
            }
            return adapterMac;
        }
        
        //返回单个IPList
        private List<string> getSingleIPListWithList(List<string> List)
        {
            List<string> lstIPAddress = new List<string>();
            foreach (string strtmp in List)//遍历原Lsit
            {
                //判断是多个IP还是多个IP
                if (strtmp.Contains("-"))//多个IP
                {
                    string[] sArray = strtmp.Split(new char[5] { '.', '.', '.', '.', '-' });
                    int startNum = int.Parse(sArray[3]), endNum = int.Parse(sArray[4]);
                    string str = sArray[0] + "." + sArray[1] + "." + sArray[2] + ".";
                    for (int i = startNum; i <= endNum; i++)
                    {
                        lstIPAddress.Add(str + i.ToString());
                    }

                }
                else//单个IP
                {
                    lstIPAddress.Add(strtmp);
                }
            }
            return lstIPAddress;
        }
        //返回单个的子网掩码List
        private List<string> getSingleSubMaskListWithList(List<string> IPList, List<string> submastList)
        {
            List<string> lstSubList = new List<string>();
            for (int index = 0; index < IPList.Count; index++)
            {
                //判断是多个IP还是多个IP
                if (IPList[index].Contains("-"))//多个IP
                {
                    string[] sArray = IPList[index].Split(new char[5] { '.', '.', '.', '.', '-' });
                    int startNum = int.Parse(sArray[3]), endNum = int.Parse(sArray[4]);
                    for (int i = startNum; i <= endNum; i++)
                    {
                        lstSubList.Add(submastList[index]);
                    }

                }
                else//单个IP
                {
                    lstSubList.Add(submastList[index]);
                }
            }
            return lstSubList;
        }
        //根据字符串返回单个IP
         private List<string> getSingleIPListWithStr(string strtmp)
         {
             List<string> lstIPAddress = new List<string>();
             //判断是多个IP还是多个IP
                 if (strtmp.Contains("-"))//多个IP
                 {
                     string[] sArray = strtmp.Split(new char[5] { '.', '.', '.', '.', '-' });
                     int startNum = int.Parse(sArray[3]), endNum = int.Parse(sArray[4]);
                     string str = sArray[0] + "." + sArray[1] + "." + sArray[2] + ".";
                     for (int i = startNum; i <= endNum; i++)
                     {
                         lstIPAddress.Add(str + i.ToString());
                     }

                 }
                 else//单个IP
                 {
                     lstIPAddress.Add(strtmp);
                 }
             return lstIPAddress;
         }
         //根据字符串返回单个子网掩码
         private List<string> getSingleSubMaskWithStr(string IPList, string submastList)
         {
             List<string> lstSubList = new List<string>();
             //判断是多个IP还是多个IP
             if (IPList.Contains("-"))//多个IP
             {
                 string[] sArray = IPList.Split(new char[5] { '.', '.', '.', '.', '-' });
                 int startNum = int.Parse(sArray[3]), endNum = int.Parse(sArray[4]);
                 for (int i = startNum; i <= endNum; i++)
                 {
                     lstSubList.Add(submastList);
                 }

             }
             else//单个IP
             {
                 lstSubList.Add(submastList);
             }
             return lstSubList;
         }
        //合并多个连续IP
         public List<string > comSingleToCon(List<string> addressList, List<string> subMaskList,bool flag)
         {
             try
             {
                 List<IPInfo> IPInfoList = new List<IPInfo>();
                 //存数对象List，进行排序
                 
                 for (int i = 0; i < addressList.Count; i++)
                 {
                     IPInfo temIPInfo = new IPInfo();
                     temIPInfo.IPAdress = addressList[i];
                     temIPInfo.SubNetMaskAdress = subMaskList[i];
                     IPInfoList.Add(temIPInfo);
                 }
                 //排序:按IP地址排序
                 IPInfoList.Sort(delegate(IPInfo A, IPInfo B)
                 {
                     string IPA = A.IPAdress.ToString();
                     string IPB = B.IPAdress.ToString();
                     IPA = Regex.Replace(IPA, @"(\d+)", "00$1");//先给每个IP加00
                     IPB = Regex.Replace(IPB, @"(\d+)", "00$1");
                     IPA = Regex.Replace(IPA, @"0*(\d{3})", "$1");//将每个数字用它得后三位替换
                     IPB = Regex.Replace(IPB, @"0*(\d{3})", "$1");
                     IPA = IPA.Replace(".", "");
                     IPB = IPB.Replace(".", "");
                     return Int64.Parse(IPA).CompareTo(Int64.Parse(IPB));
                 });
                 
                 //得到排序后的数组
                 addressList.Clear();
                 subMaskList.Clear();
                 foreach(IPInfo tem in IPInfoList)
                 {
                     addressList.Add(tem.IPAdress);
                     subMaskList.Add(tem.SubNetMaskAdress);
                 }


             }catch(Exception err)
             {
                 MessageBox.Show(err.ToString());
             }
             
            
             List<string> conIPList = new List<string>();
             List<string> conSubNetList = new List<string>();
             try
             {
                 for (int index = 0; index < addressList.Count; index++)
                 {
                     string strTmp = addressList[index];

                     int cnt = index;
                     while ((cnt < addressList.Count - 1) && IsConIP(addressList[cnt], addressList[cnt + 1]))
                     {
                         cnt++;
                     }
                     if (cnt == index)
                     {
                         conIPList.Add(addressList[index]);
                         conSubNetList.Add(subMaskList[index]);
                     }
                     else
                     {
                         strTmp += "-" + getLastIP(addressList[cnt]).ToString();
                         index = cnt;
                         conIPList.Add(strTmp);
                         conSubNetList.Add(subMaskList[index]);
                     }

                 }
             }
             catch (Exception err)
             {
                 MessageBox.Show(err.ToString());
             }
             if (flag)
             {
                 return conIPList;
             }
             else
             {
                 return conSubNetList;
             }
         }
        //将单个IP地址写入DB
         public void writeSingleAddressToConToDB(List<string> addressList, List<string> subMaskList)
         {
             //排序
             try
             {
                 List<IPInfo> IPInfoList = new List<IPInfo>();
                 //存数对象List，进行排序
                 for (int i = 0; i < addressList.Count; i++)
                 {
                     IPInfo temIPInfo = new IPInfo();
                     temIPInfo.IPAdress = addressList[i];
                     temIPInfo.SubNetMaskAdress = subMaskList[i];
                     IPInfoList.Add(temIPInfo);
                 }
                 //排序:按IP地址排序
                 IPInfoList.Sort(delegate(IPInfo A, IPInfo B)
                 {
                     string IPA = A.IPAdress.ToString();
                     string IPB = B.IPAdress.ToString();
                     IPA = Regex.Replace(IPA,@"(\d+)","00$1");//先给每个IP加00
                     IPB = Regex.Replace(IPB, @"(\d+)", "00$1");
                     IPA = Regex.Replace(IPA, @"0*(\d{3})", "$1");//将每个数字用它得后三位替换
                     IPB = Regex.Replace(IPB, @"0*(\d{3})", "$1");
                     IPA = IPA.Replace(".","");
                     IPB = IPB.Replace(".", "");
                     return Int64.Parse(IPA).CompareTo(Int64.Parse(IPB));
                 });

                 //得到排序后的数组
                 addressList.Clear();
                 subMaskList.Clear();
                 foreach (IPInfo tem in IPInfoList)
                 {
                     addressList.Add(tem.IPAdress);
                     subMaskList.Add(tem.SubNetMaskAdress);
                 }


             }
             catch (Exception err)
             {
                 MessageBox.Show(err.ToString());
             }
             //将DBList缓存清空
             clearCachesDBList();
             //将得到得IP存入DBList
             try
             {
                 for (int index = 0; index < addressList.Count; index++)
                 {
                     string strTmp = addressList[index];

                     int cnt = index;
                     while ((cnt < addressList.Count - 1) && IsConIP(addressList[cnt], addressList[cnt + 1]))
                     {
                         cnt++;
                     }
                     if (cnt == index)
                     {
                         dbIPList.Add(addressList[index]);
                         dbSubList.Add(subMaskList[index]);
                     }
                     else
                     {
                         strTmp += "-" + getLastIP(addressList[cnt]).ToString();
                         index = cnt;
                         dbIPList.Add(strTmp);
                         dbSubList.Add(subMaskList[index]);
                     }

                 }
             }
             catch (Exception err)
             {
                 MessageBox.Show(err.ToString());
             }
         }
        //IP地址格式判断
        public static bool IsRightIP(string strIPaddr)
        {
            //正则表达式判断IP地址
            // return Regex.IsMatch(strIPaddr, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$)");
            Regex rx = new Regex(@"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))");
            if (rx.IsMatch(strIPaddr))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        //尝试Ping指定IP是否能够Ping通
        public static bool IsPingIP(string strIP)
        {
            try
            {
                //创建Ping对象
                Ping ping = new Ping();
                //接受Ping返回值
                PingReply reply = ping.Send(strIP, 1000);
                //Ping通
                return true;
            }
            catch
            {
                //Ping失败
                return false;
            }
        }



        //比较两个IP地址是否相连
        public bool IsConIP(string strIP1, string strIP2)
        {
            try
            {
                if (IsRightIP(strIP1) && IsRightIP(strIP2))
                {
                    string[] sArray1 = strIP1.Split(new char[4] { '.', '.', '.', '.' });
                    string[] sArray2 = strIP2.Split(new char[4] { '.', '.', '.', '.' });

                    if (sArray1[0].Equals(sArray2[0]) && sArray1[1].Equals(sArray2[1]) && sArray1[2].Equals(sArray2[2]) && (int.Parse(sArray2[3]) - int.Parse(sArray1[3]) == 1))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                return false;
            }
        }
        
        //得到IP地址第四区间的值
        public int getLastIP(string strIP)
        {
            string[] sArray = strIP.Split(new char[4] { '.', '.', '.', '.' });
            return int.Parse(sArray[3]);
        }
        //得到IP地址第N区间的值
        public int getindexOfSectionNumber(string strIP, int n)
        {
            //失败返回-1
            if (n > 0 && n < 5)
            {
                if (IsRightIP(strIP))
                {
                    string[] sArray = strIP.Split(new char[4] { '.', '.', '.', '.' });
                    return int.Parse(sArray[n - 1]);
                }
                else
                {
                    MessageBox.Show("错误：您输入的不是一个IP地址", "提示");
                    return -1;
                }

            }
            else
            {
                MessageBox.Show("错误：您输入的区间范围因为1-4！", "错误");
                return -1;
            }

        }

        //判断出入的字符串是否为纯数字的
        public bool IsNUmberStr(string str)
        {
            bool flag = true;
            if (str != "")
            {
                foreach (char ch in str)
                {
                    if (ch < '0' || ch > '9')
                    {
                        flag = false;
                        break;
                    }
                }
            }     
            return flag;
        }

        //清空缓存信息
        public void clearCachesDBList()
        {
            dbIPList.Clear();
            dbSubList.Clear();
        }

        //清空增加删除List
        public void clearCachesChangeList()
        {
            addedIPList.Clear();
            addedSubList.Clear();
            deletedIPList.Clear();
            deletedIPList.Clear();
        }

        //addUI数据
        public void addToList(string newIP,string newsubnetMask)
        {
            //先将add数组分解成单个IP;
            List<string> singleIPList = getSingleIPListWithList(addedIPList);
            List<string> singleSubNetList = getSingleSubMaskListWithList(addedIPList,addedSubList);
            //合并到数组
            if (newIP.Contains("-") == false)//单个IP
            {
                if (singleIPList.Contains(newIP) == false)//新IP不存在IP数组
                {
                    singleIPList.Add(newIP);
                    singleSubNetList.Add(newsubnetMask);
                }
                else if (singleSubNetList[singleIPList.IndexOf(newIP)] != newsubnetMask)//IP存在，但是子网掩码不同，也是新IP
                {
                    singleIPList.Add(newIP);
                    singleSubNetList.Add(newsubnetMask);
                }
            }
            else//处理多个IP
            {
                List<string> temIPList = getSingleIPListWithStr(newIP);
                List<string> temSubList = getSingleSubMaskWithStr(newIP,newsubnetMask);
                foreach (string tempStr in temIPList)
                {
                    if (singleIPList.Contains(tempStr) == false)//新IP不存在IP数组
                    {
                        singleIPList.Add(tempStr);
                        singleSubNetList.Add(temSubList[temIPList.IndexOf(tempStr)]);
                    }
                    else if (singleSubNetList[singleIPList.IndexOf(tempStr)] != temSubList[temIPList.IndexOf(tempStr)])//IP存在，但是子网掩码不同，也是新IP
                    {
                        singleIPList.Add(tempStr);
                        singleSubNetList.Add(temSubList[temIPList.IndexOf(tempStr)]);
                    }
                }

            }
            //将单个的数组组合
            addedIPList.Clear();
            addedSubList.Clear();
            addedIPList = comSingleToCon(singleIPList, singleSubNetList, true);
            addedSubList = comSingleToCon(singleIPList, singleSubNetList, false);
           
        }
        //deleteUI数据
        public void deleteToList(string newIP, string newsubnetMask)
        {
            //先将add数组分解成单个IP;
            List<string> singleIPList = getSingleIPListWithList(deletedIPList);
            List<string> singleSubNetList = getSingleSubMaskListWithList(deletedIPList, deletedSubList);
            //合并到数组
            if (newIP.Contains("-") == false)//单个IP
            {
                if (singleIPList.Contains(newIP) == false)//新IP不存在IP数组
                {
                    singleIPList.Add(newIP);
                    singleSubNetList.Add(newsubnetMask);
                }
                else if (singleSubNetList[singleIPList.IndexOf(newIP)] != newsubnetMask)//IP存在，但是子网掩码不同，也是新IP
                {
                    singleIPList.Add(newIP);
                    singleSubNetList.Add(newsubnetMask);
                }
            }
            else//处理多个IP
            {
                List<string> temIPList = getSingleIPListWithStr(newIP);
                List<string> temSubList = getSingleSubMaskWithStr(newIP, newsubnetMask);
                foreach (string tempStr in temIPList)
                {
                    if (singleIPList.Contains(tempStr) == false)//新IP不存在IP数组
                    {
                        singleIPList.Add(tempStr);
                        singleSubNetList.Add(temSubList[temIPList.IndexOf(tempStr)]);
                    }
                    else if (singleSubNetList[singleIPList.IndexOf(tempStr)] != temSubList[temIPList.IndexOf(tempStr)])//IP存在，但是子网掩码不同，也是新IP
                    {
                        singleIPList.Add(tempStr);
                        singleSubNetList.Add(temSubList[temIPList.IndexOf(tempStr)]);
                    }
                }

            }
            //将单个的数组组合
            deletedIPList.Clear();
            deletedSubList.Clear();
            deletedIPList = comSingleToCon(singleIPList, singleSubNetList, true);
            deletedSubList = comSingleToCon(singleIPList, singleSubNetList, false);

        }
        
        //得到最后配置的数组
        public void getSettingList()
        {
            setIPList.Clear();
            setSubList.Clear();
            List<string> dbSIPList = getSingleIPListWithList(dbIPList);
            List<string> dbSSubList = getSingleSubMaskListWithList(dbIPList,dbSubList);
            try 
            {
                //去除重复的db
                foreach (string tmp in dbSIPList)
                {
                    if (setIPList.Contains(tmp) == false)//不存在IP
                    {
                        setIPList.Add(tmp);
                        setSubList.Add(dbSSubList[dbSIPList.IndexOf(tmp)]);
                    }
                    else if (setSubList[setIPList.IndexOf(tmp)] != dbSSubList[dbSIPList.IndexOf(tmp)])//如果IP相同子网不同，也是新的IP
                    {
                        setIPList.Add(tmp);
                        setSubList.Add(dbSSubList[dbSIPList.IndexOf(tmp)]);
                    }
                }

                //添加Add中的数据
                List<string> aSinIPList = getSingleIPListWithList(addedIPList);
                List<string> aSinSubList = getSingleSubMaskListWithList(addedIPList, addedSubList);
                //去重
                foreach (string tmp in aSinIPList)
                {
                    if (setIPList.Contains(tmp) == false)//不存在IP
                    {
                        setIPList.Add(tmp);
                        setSubList.Add(aSinSubList[aSinIPList.IndexOf(tmp)]);
                    }
                    else if (setSubList[setIPList.IndexOf(tmp)] != aSinSubList[aSinIPList.IndexOf(tmp)])//如果IP相同子网不同，也是新的IP
                    {
                        setIPList.Add(tmp);
                        setSubList.Add(aSinSubList[aSinIPList.IndexOf(tmp)]);
                    }
                }

                //删除del中的数据
                List<string> dSinIPList = getSingleIPListWithList(deletedIPList);
                List<string> dSinSubList = getSingleSubMaskListWithList(deletedIPList, deletedSubList);
                foreach (string tmp in dSinIPList)
                {
                    if (setIPList.Contains(tmp) == true)
                    {
                        setIPList.Remove(tmp);
                        setSubList.Remove(dSinSubList[dSinIPList.IndexOf(tmp)]);
                    }
                }
            }catch(Exception error)
            {
                MessageBox.Show(error.ToString());
            }
            
        }
        //去除DeletList中增加的IP
        public void removeAddedIPInDeletedList(string addedIPStr,string addedSubStr)
        {
            List<string> aIPList = getSingleIPListWithStr(addedIPStr);
            List<string> aSubList = getSingleSubMaskWithStr(addedIPStr,addedSubStr);
            List<string> dList = getSingleIPListWithList(deletedIPList);
            List<string> dSubList = getSingleSubMaskListWithList(deletedIPList,deletedSubList);
            foreach(string tmp in aIPList)
            {
                if (dList.Contains(tmp))
                {
                    dSubList.RemoveAt(dList.IndexOf(tmp));
                    dList.Remove(tmp);
                    
                }
            }
            deletedIPList.Clear();
            deletedSubList.Clear();
            deletedIPList = comSingleToCon(dList, dSubList, true);
            deletedSubList = comSingleToCon(dList, dSubList, false);
        }
        //去除AddedList中删除的IP
        public void removeDeletedIPInAddedList(string deldStr, string delSubStr)
        {
            List<string> dIPList = getSingleIPListWithStr(deldStr);
            List<string> dSubList = getSingleSubMaskWithStr(deldStr, delSubStr);
            List<string> aList = getSingleIPListWithList(addedIPList);
            List<string> aSubList = getSingleSubMaskListWithList(addedIPList, addedSubList);
            foreach (string tmp in dIPList)
            {
                if (aList.Contains(tmp))
                {
                    aSubList.RemoveAt(aList.IndexOf(tmp));
                    aList.Remove(tmp);
                    
                }
            }
            addedIPList.Clear();
            addedSubList.Clear();
            addedIPList = comSingleToCon(aList, dSubList, true);
            addedSubList = comSingleToCon(aList, dSubList, false);
        }

        /**************************************错误提示**************************************/
        public void returnWrongMessage(UInt32 ret)
        {
            if (ret == 0)
            {
                MessageBox.Show("配置成功：）", "提示");
            }
            else if (ret == 1)
            {
                MessageBox.Show("配置成功，但需要重启才能生效", "提示");
            }
            else if(ret == 64)
            {
                MessageBox.Show("失败：此平台不支持这个工具:(", "提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ret == 65)
            {
                MessageBox.Show("失败：未知错误！:(", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(ret == 66)
            {
                MessageBox.Show("失败：无效的子网掩码！:(", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ret == 69)
            {
                MessageBox.Show("失败：指定了超过5个网关地址:(", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ret == 70)
            {
                MessageBox.Show("失败：无效的IP地址:(", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ret == 84)
            {
                MessageBox.Show("失败：此网卡没有启动IP:(", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ret == 88)
            {
                MessageBox.Show("失败：您指定的网络号有误！:(", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ret == 89)
            {
                MessageBox.Show("失败：网络号重复！:(", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ret == 92)
            {
                MessageBox.Show("失败：超出内存！:(", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ret == 93)
            {
                MessageBox.Show("失败：IP已经存在！:(", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                MessageBox.Show("非常规错误！错误代码："+ret.ToString()+"请登录http://msdn.microsoft.com/en-us/library/aa390383(v=vs.85).aspx 查阅错误原因！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /*------------------------------------备用方法--------------------------------------*/
        //使用CMD操作
        public void useCMDOperate(object strCMDCommand)
        {
            string strOutput = null;
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                //要执行的程序名称
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;//可能接受来自调用程序得输入信息
                process.StartInfo.RedirectStandardOutput = true;//有调用程序获取信息
                process.StartInfo.CreateNoWindow = true;//不显示程序窗口
                process.Start();//启动程序
                //向CMD窗口发送输入信息
                process.StandardInput.WriteLine(strCMDCommand);
                process.StandardInput.WriteLine("exit");
                //获取CMD窗口的输出信息
                strOutput = process.StandardOutput.ReadToEnd();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
            }

            /*
            if (strOutput != null)
            {
                MessageBox.Show(strOutput);
            }
             
             */

        }

        private void dataGridView_SizeChanged(object sender, EventArgs e)
        {
            //labe_GateWay.Location = new System.Drawing.Point(19, dataGridView.Location.Y - 10 + dataGridView.Size.Height);
            labe_GateWay.SetBounds(labe_GateWay.Location.X,dataGridView.Location.Y - 10 + dataGridView.Size.Height,labe_GateWay.Size.Width,labe_GateWay.Size.Height);
            textBox_GateWay.SetBounds(textBox_GateWay.Location.X, dataGridView.Location.Y - 10 + dataGridView.Size.Height, textBox_GateWay.Size.Width, textBox_GateWay.Size.Height);
            label_addIP.SetBounds(label_addIP.Location.X,labe_GateWay.Location.Y+labe_GateWay.Size.Height+20,label_addIP.Size.Width,label_addIP.Size.Height);
            labelunderline.SetBounds(labelunderline.Location.X, labe_GateWay.Location.Y + labe_GateWay.Size.Height + 20, labelunderline.Size.Width, labelunderline.Size.Height);
            label_NetMask.SetBounds(label_NetMask.Location.X,labe_GateWay.Location.Y + labe_GateWay.Size.Height + 20,label_NetMask.Size.Width,label_NetMask.Size.Height);
            textBox_AddIP.SetBounds(textBox_AddIP.Location.X, textBox_GateWay.Location.Y + textBox_GateWay.Size.Height + 5, textBox_AddIP.Size.Width, textBox_AddIP.Size.Height);
            textBox_EndNet.SetBounds(textBox_EndNet.Location.X, textBox_GateWay.Location.Y + textBox_GateWay.Size.Height + 5,textBox_EndNet.Size.Width,textBox_EndNet.Size.Height);
            textBox_NetMask.SetBounds(textBox_NetMask.Location.X, textBox_GateWay.Location.Y + textBox_GateWay.Size.Height + 5,textBox_NetMask.Size.Width,textBox_NetMask.Size.Height);
            btn_AddIP.SetBounds(btn_AddIP.Location.X, textBox_GateWay.Location.Y + textBox_GateWay.Size.Height + 5,btn_AddIP.Size.Width,btn_AddIP.Size.Height);
            btn_Set.SetBounds(btn_Set.Location.X,textBox_AddIP.Location.Y+textBox_AddIP.Size.Height+20,btn_Set.Size.Width,btn_Set.Size.Height);
            btn_Exit.SetBounds(btn_Exit.Location.X, textBox_AddIP.Location.Y + textBox_AddIP.Size.Height + 20,btn_Exit.Size.Width,btn_Exit.Size.Height);    
        }
    }
}
