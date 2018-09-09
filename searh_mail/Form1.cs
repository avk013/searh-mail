using System;
using System.Diagnostics;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.IO;
//using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Net;

namespace searh_mail
{
    public partial class Form1 : Form
    {
        const string server = "172.16.36.207", name = "inventar", pass = "pa100slow";
        const string db = "inventar";
        string dir_aida = @"e:\!invent\";
        string dir_templ = "1.rpf", num_kab="404";
        string[] data2db = new string[1];
        string folder_path = @"e:\!invent\";
        string filename;
        private MySqlConnection connection;

        private void init()
        {
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            db + ";" + "UID=" + name + ";" + "PASSWORD=" + pass + ";";
            connection = new MySqlConnection(connectionString);
        }
        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            //   Button buttonCancel = new Button();
            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;
            buttonOk.Text = "OK";
            //   buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            //   buttonCancel.DialogResult = DialogResult.Cancel;
            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            // buttonCancel.SetBounds(309, 72, 75, 23);
            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            //            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            //form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            //   form.CancelButton = buttonCancel;
            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
        static string[] SearchDirectory(string patch)
        { //находим все папки в по указанному пути
            try {
                    string[] ReultSearch = Directory.GetDirectories(patch);
                    return ReultSearch; //возвращаем список директорий
                }
            catch { } string[] er = { }; return er;//если ошибка
        }
        static string[] SearchFile(string patch, string pattern)
        {
            /*флаг SearchOption.AllDirectories означает искать во всех вложенных папках*/
            string[] ReultSearch = Directory.GetFiles(patch, pattern, SearchOption.AllDirectories);
            //возвращаем список найденных файлов соответствующих условию поиска 
            //  MessageBox.Show(patch+pattern);
            return ReultSearch;
        }

        static readonly string[] LowNames =
         {
     "NUL", "SOH", "STX", "ETX", "EOT", "ENQ", "ACK", "BEL",
     "BS", "HT", "LF", "VT", "FF", "CR", "SO", "SI",
     "DLE", "DC1", "DC2", "DC3", "DC4", "NAK", "SYN", "ETB",
     "CAN", "EM", "SUB", "ESC", "FS", "GS", "RS", "US"
 };
        private string FixString(string str)
        {
            return string.IsNullOrEmpty(str) ? str : Regex.Replace(str, @"[^a-zA-Z0-9@. ]", "");
        }

        private void button2_Click(object sender, EventArgs e)
        { }
        private string mail_bats(string path)
        {
            string st = "";
            string outs = "";
            using (var reader = new StreamReader(@"e:\The Bat!\Data\Mail\udp404@i.ua\Account.CFN", Encoding.ASCII))
            //using (var reader = new StreamReader(@"G:\THEBAT!\Mail\asom-mailru\Account.CFN"))

            {
                var builder = new StringBuilder((int)reader.BaseStream.Length + 1);
                while (!reader.EndOfStream)
                {
                    var chr = (char)reader.Read();
                    if (chr < 32)
                    {// chr = LowNames[chr];
                    }
                    if (chr == 0) chr = '_';
                    else
                    if (chr > 127) chr = '_'; else st += chr;
                    //builder.Clear();
                    builder.Append(chr);
                }

                var result = builder.ToString();
                st = FixString(st);
                st = st.Trim();
                textBox1.Text = st;
                string[] flags = { " mailtoFromAddr", "Name" };
                string flag = "";
                for (int i = 0; i < flags.Length; i++)
                {
                    flag = flags[i];
                    ///
                    while (st.IndexOf(flag) != -1)
                    {
                        int a = st.IndexOf(flag);
                        if (st.Substring(a + flag.Length, 1) != " ")
                        {
                            string aa = st.Substring(a + flag.Length, 50);
                            // textBox2.Text += "_"+aa;
                            int aa_sobaka = aa.IndexOf("@");
                            if (aa_sobaka > 0)
                            {//узнаем место собаки и находи имя
                                string aa_name = aa.Substring(0, aa_sobaka);
                                //   textBox2.Text += aa+"_"+aa_name.ToString()+"_" + Environment.NewLine;
                                //  ищем адрес имени после собаки   
                                int aa_addres = aa.IndexOf(aa_name, aa_sobaka);
                                // если имя найдено отрезаем и узнаем адрес
                                int points = aa.LastIndexOf(".");

                                if (aa.IndexOf(" ") < aa.LastIndexOf("."))
                                    aa = aa.Substring(0, aa.IndexOf(" "));
                                else if (aa.LastIndexOf(".") != -1) aa = aa.Substring(0, aa.LastIndexOf(".") + 3);
                                if (aa.LastIndexOf(".") != -1)
                                    outs += aa + Environment.NewLine;
                                //textBox2.Text += aa + Environment.NewLine;
                            }
                        }
                        st = st.Remove(a, 25);
                    }
                }
                st = Regex.Replace(st, "[ ]+", " ");
                //MessageBox.Show(st);
            }
            return outs;
        }



        private void button4_Click(object sender, EventArgs e)
        { MessageBox.Show(mail_thunderbird(@"E:\thunderbird\6yjr55bj.default\prefs.js")); }
        private string mail_bat(string path)
        {
            string st = "", outs = ""; ;
            using (var reader = new StreamReader(path, Encoding.ASCII))
            {
                var builder = new StringBuilder((int)reader.BaseStream.Length + 1);
                while (!reader.EndOfStream)
                {
                    var chr = (char)reader.Read();
                    if (chr < 32)
                    {// chr = LowNames[chr];
                    }
                    if (chr == 0) chr = '_';
                    else
                    if (chr > 127) chr = '_'; else st += chr;
                    //builder.Clear();
                    builder.Append(chr);
                }

                var result = builder.ToString();
                st = FixString(st);
                st = st.Trim();
                textBox1.Text = st;
                //  string[] flags = { "[%Name]", " mailto:%FromAddr" };
                string[] flags = { "Name", " mailtoFromAddr" };
                string flag = "";
                for (int i = 0; i < flags.Length; i++)
                {
                    flag = flags[i];
                    ///
                    while (st.IndexOf(flag) != -1)
                    {
                        int a = st.IndexOf(flag);
                        if (st.Substring(a + flag.Length, 1) != " ")
                        {
                            string aa = st.Substring(a + flag.Length, 90);
                            // textBox2.Text += "_"+aa;
                            int aa_sobaka = aa.IndexOf("@");
                            if (aa_sobaka > 0)
                                if (aa_sobaka > 0)
                                {

                                    string aa_name = aa.Substring(0, aa_sobaka);
                                    //   textBox2.Text += aa+"_"+aa_name.ToString()+"_" + Environment.NewLine;

                                    int aa_addres = aa.IndexOf(aa_name, aa_sobaka);
                                    //int aa_addres = aa.IndexOf("lex");
                                    //textBox2.Text += aa_addres.ToString() + Environment.NewLine;
                                    if (aa_addres > 0)
                                        outs += aa.Substring(0, aa_addres) + Environment.NewLine;
                                    //textBox2.Text += st.Substring(a + flag.Length, aa_addres) + Environment.NewLine;
                                    else outs += aa + Environment.NewLine;
                                }
                            //  textBox2.Text += aa.Substring(0,10) + Environment.NewLine;
                        }

                        //  textBox2.Text += st.Substring(a + flag.Length, 40) + Environment.NewLine;
                        st = st.Remove(a, 25);
                    }
                }
                st = Regex.Replace(st, "[ ]+", " ");
                return outs;

                //MessageBox.Show(result);
            }
        }

        private static string mail_microsoft(string path)
        {
            string outs = "";
            if (File.Exists(path))
            {
                string[] readText = File.ReadAllLines(path);
                string ss;
                foreach (string s in readText)
                {
                    if (s.IndexOf("SMTP_Email_Address") > -1)
                    {
                        ss = s;
                        ss = ss.Replace("<SMTP_Email_Address type=\"SZ\">", "");
                        ss = ss.Replace("</SMTP_Email_Address>", "");
                        ss = ss.Replace(" ", "");
                        //ss = ss.Substring(ss.IndexOf("\"") + 1);
                        outs += ss + Environment.NewLine;
                    }
                }
            }
            else outs = "error open profile win mail";
            return outs;
        }
        private static string mail_thunderbird(string path)
        {
            string outs = "";
            if (File.Exists(path))
            {
                string[] readText = File.ReadAllLines(path);
                string ss;
                foreach (string s in readText)
                {
                    if (s.IndexOf("useremail") > -1)
                    {
                        ss = s;
                        ss = ss.Replace("user_pref(\"mail.identity.id", "");
                        ss = ss.Replace(".useremail\", ", "");
                        ss = ss.Replace("\");", "");
                        ss = ss.Substring(ss.IndexOf("\"") + 1);
                        outs += ss + Environment.NewLine;
                    }
                }
            }
            else outs = "error open profile thunderbird";
            return outs;
        }
        private string  out_mail()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            // appdata = appdata.Substring(3, appdata.Length-"Roaming".Length-3);
            appdata = appdata.Substring(3, appdata.Length - 3);
            string[] dir_s = { appdata,@"thunderbird\", @"mail\", @"почта\", @"пошта\", @"Bat\", @"The Bat\", @"TheBat\", @"TheBat!\", @"The Bat!\" };
            label2.Text= appdata;
            string[] Drives = Environment.GetLogicalDrives();
            string[] pattches = new string[100];
            int i = 0;
            foreach (string s in Drives) //ищем диски и помещаем список в массив
            {
                foreach (string sd in dir_s)
                    pattches[i++] = s + sd;
            }
            string PatchProfile = "";
            string ListPatch = ""; //заголовок для строк
            //ищем все вложенные папки 
            for (i = 0; i < pattches.Length; i++)
            {
                PatchProfile = pattches[i];
                string[] S = SearchDirectory(PatchProfile);
                //создаем строку в которой соберем все пути

                foreach (string folderPatch in S)
                {
                    try
                    {
                        //пытаемся найти данные в папке 
                        //string[] F = SearchFile(folderPatch, "*.png");
                        string[] F = SearchFile(folderPatch, "prefs.js");
                        // string[] F = SearchFile(@"e:\", "prefs.js");
                        foreach (string FF in F)
                        {
                            //добавляем файл в список 
                            //     ListPatch += FF + "\n";
                            if (mail_thunderbird(FF) != "") ListPatch +=mail_thunderbird(FF);
                        }
                        F = SearchFile(folderPatch, "Account.CFN");
                        // string[] F = SearchFile(@"e:\", "prefs.js");
                        foreach (string FF in F)
                        {
                            //добавляем файл в список 
                            // MessageBox.Show(FF + "_"+mail_bat(FF)+Environment.NewLine);
                           ListPatch += mail_batss(FF);
                            // ListPatch += FF + "\n";
                        }
                        F = SearchFile(folderPatch, "*.oeaccount");
                        foreach (string FF in F)
                        {   //добавляем файл в список 
                            if (mail_microsoft(FF) != "") ListPatch += mail_microsoft(FF);
                        } }
                    catch
                    { }
                }
            }

            //выводим список на экран 
            textBox2.Text = ListPatch;
            return ListPatch;
            //MessageBox.Show(ListPatch);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string mail=out_mail();
            string sql = @"INSERT INTO `mail` (`komp_name`, `kabinet`, `mail`, `notes`)
VALUES ('404', '404', '"+mail+"', '');";
            init();
            if (this.OpenConnection() == true)
            { //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {  //aida /r filename /text /langru /safe /hw /html /custom C:\1.rpf
            folder_path= folder_path.Substring(0, folder_path.LastIndexOf(@"\"));
            Process.Start(folder_path+@"\aida\aida64", @"/r "+folder_path+ @"\out\"+filename+" /text /langru /safe /hw /html /custom " + dir_templ);
            }

        private void button8_Click(object sender, EventArgs e)
        {    //.oeaccount
            //   string[] dir_s = { @"Users\upi\AppData\Local\Microsoft\Windows Live Mail\" };
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            // appdata = appdata.Substring(3, appdata.Length-"Roaming".Length-3);
            appdata = appdata.Substring(3, appdata.Length -3);            
            string[] dir_s = {appdata};
            textBox1.Text += Environment.NewLine+ dir_s[0];
            string[] Drives = Environment.GetLogicalDrives();
            string[] pattches = new string[100];
            int i = 0;
            foreach (string s in Drives) //ищем диски и помещаем список в массив
            {
                foreach (string sd in dir_s)
                    pattches[i++] = s + sd;
            }
            string PatchProfile = "";
            string ListPatch = ""; //заголовок для строк
            //ищем все вложенные папки 
            for (i = 0; i < pattches.Length; i++)
            {
                PatchProfile = pattches[i];
                string[] S = SearchDirectory(PatchProfile);
                //создаем строку в которой соберем все пути
                int iii = 0;
                foreach (string folderPatch in S)
                {
                    try
                    {
                        //пытаемся найти данные в папке 
                        string[] F = SearchFile(folderPatch, "*.oeaccount");
                        foreach (string FF in F)
                        {   //добавляем файл в список 
                           if(mail_microsoft(FF)!="") ListPatch += (++iii).ToString() + ">." + mail_microsoft(FF) + Environment.NewLine;
                        }
                    }
                    catch { }
                }
            }
            // MessageBox.Show(ListPatch);
            textBox2.Text = ListPatch;
        }

        private void Form1_Load(object sender, EventArgs e)
        {//привязываемся к папке в которой программа
            folder_path= System.Reflection.Assembly.GetEntryAssembly().Location;
            label1.Text = folder_path;
            dir_templ = dir_aida + dir_templ;
            but_dir_aida.Text = dir_aida;
            but_dir_templ.Text = dir_templ;
            //combobox from file
            string[] lineOfContents = File.ReadAllLines("brend.csv");
            foreach (var line in lineOfContents)
            {
                string[] tokens = line.Split(',');
                comboBox1.Items.Add(tokens[1]);
            }
            //
            // Ping's the local machine.
            Ping pingSender = new Ping();
            // IPAddress address = IPAddress.Loopback;
            IPAddress address = System.Net.IPAddress.Parse(server);
            PingReply reply = pingSender.Send(address);

            if (reply.Status == IPStatus.Success)
            {               
            }
            else
            {
                radioButton2.Checked = true;
                radioButton1.Text = "удаленно (вероятно недоступен!!!)";
                radioButton1.ForeColor = Color.Red;
            }
            ///


            InputBox("кабинет", "укажите кабинет", ref num_kab);
            filename = num_kab + "~" + Environment.MachineName + "~" + Environment.UserName;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.ShowNewFolderButton = false;
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                folder_path = FBD.SelectedPath; label1.Text = folder_path;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int flag = 0;
            if (radioButton2.Checked == true) flag = 2; else flag = 1;
            string mail = out_mail();
            string sql = @"INSERT INTO `mail` (`komp_name`, `kabinet`, `mail`, `notes`)
VALUES ('404', '404', '" + mail + "', '');";
            if (flag==1) 
                {
                label10.Text = sql;
            init();
            if (this.OpenConnection() == true)
            { //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                //close connection
                this.CloseConnection();
            }
            }
            else
            {//сохраняем данные в файл
                label10.Text = "ok";
                folder_path = folder_path.Substring(0, folder_path.LastIndexOf(@"\"));
                using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(folder_path+ @"/"+filename+".sql", true))
                {
                    file.WriteLine(sql);
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true) label10.Text = "2";else label10.Text = "1";
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {        
        }

        private void tabControl1_SizeChanged(object sender, EventArgs e)
        {

        }
        private string mail_batss(string path)
        {
            string st = "";
            string outs = "";
            using (var reader = new StreamReader(path, Encoding.ASCII))
            //using (var reader = new StreamReader(@"e:\The Bat!\Data\Mail\udp404@i.ua\Account.CFN", Encoding.ASCII))
            //using (var reader = new StreamReader(@"G:\THEBAT!\Mail\asom-mailru\Account.CFN"))

            {
                var builder = new StringBuilder((int)reader.BaseStream.Length + 1);
                while (!reader.EndOfStream)
                {
                    var chr = (char)reader.Read();
                    if (chr < 32)
                    { chr = (char)0;
                    }
                    //if (chr == 0) chr = (char)32;
                    //if (chr == 0) chr = (char)32;
                    //  else
                    //if (chr > 126) chr = '_'; else st += chr;
                    if (chr > 126) chr = (char)0;// else st += chr;
                    st += chr;
                    //builder.Clear();

                    builder.Append(chr);
                }

                var result = builder.ToString();
                st = FixString(st);
                st = st.Trim();
                st = st.Replace(Environment.NewLine, " ");
                textBox1.Text = st;
                string[] flags = { " mailtoFromAddr", "xtName" };
                string flag = "";
                for (int i = 0; i < flags.Length; i++)
                {
                    flag = flags[i];
                    ///
                    while (st.IndexOf(flag) != -1)
                    {
                        int a = st.IndexOf(flag);
                        if (st.Substring(a + flag.Length, 1) != " ")
                        {
                            string aa = st.Substring(a + flag.Length, 50);
                            // textBox2.Text += "_"+aa;
                            int aa_sobaka = aa.IndexOf("@");
                            if (aa_sobaka > 0)
                            {//узнаем место собаки и находи имя
                                string aa_name = aa.Substring(0, aa_sobaka);
                                //   textBox2.Text += aa+"_"+aa_name.ToString()+"_" + Environment.NewLine;
                                //  ищем адрес имени после собаки   
                                int aa_addres = aa.IndexOf(aa_name, aa_sobaka);
                                // если имя найдено отрезаем и узнаем адрес
                                int points = aa.LastIndexOf(".");
                                //?? а вдруг в емаиде есть названия протоколов или релай????
                                //string aaa = aa.Substring(aa.IndexOf("@"), aa.Length - aa.IndexOf("@"));                              
                               if (aa.LastIndexOf("pop.") != -1)
                                    aa = aa.Substring(0, aa.LastIndexOf("pop."));
                               if (aa.LastIndexOf("pop3.") != -1)
                                    aa = aa.Substring(0, aa.LastIndexOf("pop3."));
                                 if (aa.LastIndexOf("imap.") != -1)
                                    aa = aa.Substring(0, aa.LastIndexOf("imap."));
                                if (aa.LastIndexOf("smtp.") != -1)
                                    aa = aa.Substring(0, aa.LastIndexOf("smtp."));
                                if (aa.LastIndexOf("relay.") != -1)
                                    aa = aa.Substring(0, aa.LastIndexOf("relay."));
                                //     if (aa.LastIndexOf(aa.Substring(0,3)) != -1)
                                //       aa = aa.Substring(0, aa.Length-aa.LastIndexOf(aa.Substring(0, 3)));

                                if (aa.IndexOf(" ") < aa.LastIndexOf(".")&& aa.IndexOf(" ")!=-1)
                                    aa = aa.Substring(0, aa.IndexOf(" "));
                               // else if (aa.LastIndexOf(".") != -1) aa = aa.Substring(0, aa.LastIndexOf(".") + 3);
                                if (aa.LastIndexOf(".") != -1 && aa.Length>5)
                                    outs += aa + Environment.NewLine;
                                //textBox2.Text += aa + Environment.NewLine;
                            }
                        }
                        st = st.Remove(a, 60);
                    }
                }
                st = Regex.Replace(st, "[ ]+", " ");
                //MessageBox.Show(st);
            }
            return outs;
        }
    }
}