using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace searh_mail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string[] SearchDirectory(string patch)
        {
            
            //находим все папки в по указанному пути
            try { 
            string[] ReultSearch = Directory.GetDirectories(patch);
            //возвращаем список директорий
            return ReultSearch;
            }
            catch { }
            string[] er= { };
            return er;
        }
        static string[] SearchFile(string patch, string pattern)
        {
            /*флаг SearchOption.AllDirectories означает искать во всех вложенных папках*/
            string[] ReultSearch = Directory.GetFiles(patch, pattern, SearchOption.AllDirectories);
            //возвращаем список найденных файлов соответствующих условию поиска 
            return ReultSearch;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //получаем переменную Windows с адресом текущего пользователя
            string[] pattches = { @"e:\thunderbird\", @"c:\Users\"};
            //string PatchProfile = Environment.GetEnvironmentVariable("USERPROFILE");
            string PatchProfile = "";
            string ListPatch = "найденные файлы \n"; //заголовок для строк
            //ищем все вложенные папки 
            for (int i = 0; i < pattches.Length; i++)
            {
                PatchProfile = pattches[i];
                string[] S = SearchDirectory(PatchProfile);
                //создаем строку в которой соберем все пути

                foreach (string folderPatch in S)
                {
                    //добавляем новую строку в список
                    // ListPatch += folderPatch + "\n";
                    try
                    {
                        //пытаемся найти данные в папке 
                        //string[] F = SearchFile(folderPatch, "*.png");
                        string[] F = SearchFile(folderPatch, "prefs.js");
                        // string[] F = SearchFile(@"e:\", "prefs.js");
                        foreach (string FF in F)
                        {
                            //добавляем файл в список 
                            ListPatch += FF + "\n";
                        }
                      
                    }
                    catch
                    {
                    }
                }}
           
            //выводим список на экран 
            MessageBox.Show(ListPatch); 
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
        {
            string st="";
            //using (var reader = new StreamReader(@"E:\The Bat!\Data\Mail\udp404@i.ua\Account.CFN", Encoding.ASCII))
            using (var reader = new StreamReader(@"G:\THEBAT!\Mail\asomson-ukrnet\Account.CFN", Encoding.ASCII))
                //"mailto:%FromAddr"
                
            //
            {
                var builder = new StringBuilder((int)reader.BaseStream.Length + 1);
                while (!reader.EndOfStream)
                {
                    var chr = (char)reader.Read();
                    if (chr < 32)
                    {// chr = LowNames[chr];
                    }
                    if (chr == 0)  chr = '_'; 
                    else
                    if (chr >127) chr = '_'; else st+=chr;
                    //builder.Clear();
                    builder.Append(chr);
                }

                var result = builder.ToString();
                st = FixString(st);
                st=st.Trim();
                textBox1.Text = st;
                //  string[] flags = { "[%Name]", " mailto:%FromAddr" };
                string[] flags = { "Name", " mailtoFromAddr" };
                string flag = "";
                for(int i=0;i<flags.Length;i++)
                { flag = flags[i];
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
                            {
                                
                                string aa_name = aa.Substring(0, aa_sobaka);
                             //   textBox2.Text += aa+"_"+aa_name.ToString()+"_" + Environment.NewLine;
                                
                                int aa_addres = aa.IndexOf(aa_name,aa_sobaka);
                                //int aa_addres = aa.IndexOf("lex");
                                //textBox2.Text += aa_addres.ToString() + Environment.NewLine;
                              if (aa_addres>0)
                                  textBox2.Text += aa.Substring(0,aa_addres) + Environment.NewLine;
                                //textBox2.Text += st.Substring(a + flag.Length, aa_addres) + Environment.NewLine;
                              else textBox2.Text += aa + Environment.NewLine;
                            }
                          //  textBox2.Text += aa.Substring(0,10) + Environment.NewLine;
                        }
                        
                      //  textBox2.Text += st.Substring(a + flag.Length, 40) + Environment.NewLine;
                        st = st.Remove(a, 25);
                }
                }
                st = Regex.Replace(st, "[ ]+", " ");
                
                
//MessageBox.Show(result);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] dir_s = { @"thunderbird\", @"Users\", @"mail\", @"почта\", @"пошта\", @"Bat\", @"The Bat\", @"TheBat\", @"TheBat!\", @"The Bat!\" };
            string[] Drives = Environment.GetLogicalDrives();
            string[] pattches= new string[100];
            int i = 0;
           // string ss = "";
            foreach (string s in Drives)
            {
                foreach (string sd in dir_s)
                    pattches[i++] = s + sd;
                    //ss +=s+sd;
            }
            //foreach (string a in pathes) ss += a+"; ";
            //MessageBox.Show(ss);
            string PatchProfile = "";
            string ListPatch = "найденные файлы \n"; //заголовок для строк
            //ищем все вложенные папки 
            for (i = 0; i < pattches.Length; i++)
            {
                PatchProfile = pattches[i];
                string[] S = SearchDirectory(PatchProfile);
                //создаем строку в которой соберем все пути

                foreach (string folderPatch in S)
                {
                    //добавляем новую строку в список
                    // ListPatch += folderPatch + "\n";
                    try
                    {
                        //пытаемся найти данные в папке 
                        //string[] F = SearchFile(folderPatch, "*.png");
                        string[] F = SearchFile(folderPatch, "prefs.js");
                        // string[] F = SearchFile(@"e:\", "prefs.js");
                        foreach (string FF in F)
                        {
                            //добавляем файл в список 
                            ListPatch += FF + "\n";
                        }
                        F = SearchFile(folderPatch, "Account.CFN");
                        // string[] F = SearchFile(@"e:\", "prefs.js");
                        foreach (string FF in F)
                        {
                            //добавляем файл в список 
                            ListPatch += FF + "\n";
                        }
                    }
                    catch
                    {
                    }
                }
            }

            //выводим список на экран 
            MessageBox.Show(ListPatch);
        }

        private void button4_Click(object sender, EventArgs e)
        {MessageBox.Show(mail_thunderbird(@"E:\thunderbird\6yjr55bj.default\prefs.js")); }

        private static string mail_thunderbird(string path)
        {   string outs = "";
            if (File.Exists(path))
            {   string[] readText = File.ReadAllLines(path);
                string ss;
                foreach (string s in readText)
                {if (s.IndexOf("useremail") > -1)
                    {   ss = s;
                        ss = ss.Replace("user_pref(\"mail.identity.id", "");
                        ss = ss.Replace(".useremail\", ", "");
                        ss = ss.Replace("\");", "");
                        ss = ss.Substring(ss.IndexOf("\"") + 1);
                        outs += ss + Environment.NewLine; }}}
            else outs = "error open profile thunderbird";
            return outs;
        }
    }
}
