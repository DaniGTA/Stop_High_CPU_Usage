using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;
using System.Security.Permissions;
using System.Security.Principal;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("psapi.dll")]
        static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]



        static extern bool CloseHandle(IntPtr hObject);
        public int path_list_int;
        List<string> path_list = new List<string>();
        public int name_list_int;
        List<string> name_list = new List<string>();
        public static string Selected_row_proc;
        public int Selected_row_proc_int;
        public string slider_cache;
        public bool rewrited;
        public bool rewrited_list_async = true;
        public string text = "";
        public string cache;
        public string old_rewrited_string;
        public string return_value;
        public int track_value;
        public string test;
        public string path2;
        public string list_value_check;
        public static string thread_name;
        public string current_checked_path;
        public static bool file_changes = false;
        public string text_list_cache;
        public static string path;
        public static int value;
        public static bool restart_bool;
        public bool start_up_window_change = false;
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        TaskService ts;
        //LANG MANAGER
        private string lang_basic_01;
        private string lang_basic_02;
        private string lang_basic_03;
        private string lang_basic_04;
        private string lang_basic_05;
        private string lang_basic_06;
        private string lang_basic_07;
        private string lang_basic_08;
        private string lang_basic_09;
        private string lang_basic_10;
        private string lang_error_01;
        private string lang_error_02;
        private string lang_error_03;
        private string lang_error_04;
        private string lang_error_05;
        private string lang_error_06;
        private string lang_error_07;
        private string lang_error_08;
        private string lang_error_09;
        private string lang_error_10;

        public Form1()
        {

            InitializeComponent();
            notifyIcon2.Text = "";

            System.Threading.Tasks.Task.Run(Check_USAGE_CPU);
            try
            {
                lang_manager(System.Globalization.CultureInfo.InstalledUICulture.ThreeLetterISOLanguageName.ToString());
            }catch(Exception)
            {

            }
            comboBox1.Text = "🌐";
            comboBox1.Items.Add("DEU| Deutsch   🌐");
            comboBox1.Items.Add("ENG| English   🌐");
            button1.Text = lang_basic_09;
            label1.Hide();
            label2.Text = "";
            checkBox1.Hide();
            checkBox3.Text = lang_basic_01;
            trackBar1.Hide();
            trackBar1.ResetText();
            checkBox2.Checked = false;
            List_Box();

            if (CheckAdmin())
            {
                button2.Hide();
            }
            else
            {
                button2.Show();
            }
            try {
                using (ts = new TaskService())
                {
                    try
                    {
                        Microsoft.Win32.TaskScheduler.Task get_task = ts.GetTask("Stop_High_CPU_usage");
                        if (get_task.Enabled)
                        {
                            checkBox3.Checked = true;
                            checkBox2.Checked = true;
                        }
                    }
                    catch (Exception)
                    {
                        checkBox3.Checked = false;
                    }
                }

                checkBox2.Text = lang_basic_02;
                if (rkApp.GetValue("Stop_High_CPU_usage") == null)
                {
                    if (checkBox2.Checked == true)
                    {

                    }
                    else
                    {
                        checkBox2.Checked = false;
                    }
                }
                else
                {
                    if (checkBox3.Checked == true)
                    {
                        rkApp.DeleteValue("Stop_High_CPU_usage");
                    }
                    else
                    {
                        if (rkApp.GetValue("Stop_High_CPU_usage").ToString() != Application.ExecutablePath.ToString()) ;
                        {
                            rkApp.SetValue("Stop_High_CPU_usage", Application.ExecutablePath);
                        }
                    }
                    checkBox2.Checked = true;
                }
            }catch(Exception)
            {

            }
            }

        public static string Get_Process(int pid)
        {
            var processHandle = OpenProcess(0x0400 | 0x0010, false, pid);

            if (processHandle == IntPtr.Zero)
            {
                return null;
            }

            const int lengthSb = 4000;

            var sb = new StringBuilder(lengthSb);

            string result = null;

            if (GetModuleFileNameEx(processHandle, IntPtr.Zero, sb, lengthSb) > 0)
            {
                result = sb.ToString();
            }

            CloseHandle(processHandle);

            return result;
        }
        public void List_Box()
        {
            if (rewrited_list_async == true)
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Process_list_config.cfg");
                if (File.Exists(path))
                {
                    text = File.ReadAllText(path);
                }
                else
                {
                    FileStream fileStream = File.Create(path);
                    fileStream.Close();
                }
                text_list_cache = text;
                rewrited_list_async = false;
            }
            bool add_to_list_allowed = false;
            listView1.Items.Clear();
            path_list.Clear();
            name_list.Clear();
            int x = 0;
            Process[] processlist = Process.GetProcesses();
            foreach (Process theprocess in processlist)
            {
                try
                {
                    x++;
                    add_to_list_allowed = false;
                    string path = "";
                    ListViewItem list = new ListViewItem(theprocess.ProcessName);
                    list.SubItems.Add(theprocess.Id.ToString());
                    list.Name = theprocess.ProcessName;
                    list.SubItems.Add(theprocess.MainWindowTitle);
                    Process process = Process.GetProcessById(theprocess.Id);

                    try
                    {
                        path = Get_Process(theprocess.Id);
                    }
                    catch
                    {

                    }
                    if (path == null)
                    {
                        name_list_int = 0;
                        if (name_list != null)
                        {
                            string[] name_list_array = name_list.ToArray();
                            foreach (string name_list_string in name_list_array)
                            {
                                if (name_list_string == theprocess.ProcessName)
                                {
                                    name_list_int = 1;
                                }
                            }
                        }
                        else
                        {
                            name_list_int = 0;
                        }
                        if (name_list_int == 0)
                        {
                            list.ForeColor = Color.FromArgb(100, 200, 85, 0);
                            path = lang_error_01;
                            list.SubItems.Add(path);
                            try
                            {
                                name_list.Add(theprocess.ProcessName);
                            }
                            catch
                            {

                            }
                            add_to_list_allowed = true;
                        }
                        else
                        {
                            add_to_list_allowed = false;
                        }

                    }
                    else
                    {
                        if (path_list != null)
                        {
                            path_list_int = 0;
                            string[] path_list_array = path_list.ToArray();
                            foreach (string path_list_string in path_list_array)
                            {
                                if (path_list_string == path)
                                {
                                    path_list_int = 1;
                                }
                            }
                        }
                        else
                        {
                            path_list_int = 0;
                        }
                        if (path_list_int == 0)
                        {
                            try
                            {
                                path_list.Add(path);
                            }
                            catch
                            {

                            }
                            list.SubItems.Add(path);
                            add_to_list_allowed = true;

                            list.SubItems.Add(path);
                            string[] tokens;
                            tokens = text_list_cache.Split('\n');
                            foreach (string token in tokens)
                            {
                                int x_int = 0;
                                string[] list_token = token.Split(',');
                                try
                                {
                                    foreach (string list_p in list_token)
                                    {

                                        if (x_int == 0)
                                        {
                                            thread_name = list_p;
                                        }
                                        if (x_int == 1)
                                        {
                                            path2 = list_p;
                                        }
                                        if (x_int == 2)
                                        {
                                            Int32.TryParse(list_p, out value);
                                        }
                                        if (x_int == 3)
                                        {
                                            if (path == path2)
                                            {
                                                if (value != 0)
                                                {
                                                    list.BackColor = Color.FromArgb(100, 0, 189, 0);
                                                }
                                            }
                                        }

                                        x_int++;
                                    }

                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        else
                        {
                            add_to_list_allowed = false;
                        }
                    }
                    if (add_to_list_allowed == true)
                    {
                        listView1.Items.Add(list);
                    }
                }
                catch (Exception)
                {

                }

            }
        }
        public static void ExecuteAsAdmin(string fileName, bool admin)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            if (admin == true)
            {
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.Verb = "runas";
            }
            proc.Start();
            proc.Close();
        }
        public void Check(string pfad, string thread_name)
        {
            while (true)
            {
                using (PerformanceCounter process_cpu = new PerformanceCounter("Process", "% Processor Time", thread_name))
                {
                    var process_cpu_usage_start = process_cpu.NextValue();
                    System.Threading.Thread.Sleep(1000);
                    var process_cpu_usage = process_cpu.NextValue();
                    var process_cpu_usage_new = process_cpu.NextValue();
                    if (process_cpu_usage > 1000)
                    {
                        try
                        {
                            foreach (Process proc in Process.GetProcessesByName(thread_name))
                            {
                                proc.Kill();
                            }
                        }
                        catch (Exception)
                        {

                        }


                        bool admin = true;
                        ExecuteAsAdmin(pfad, admin);
                    }

                }
            }
        }
        private void notifyIcon2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }
        private void ImportStatusForm_Resize(object sender, EventArgs e)
        {
            //Debug.WriteLine("mini" + this.WindowState);
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (ShowInTaskbar == true)
                {
                    ShowInTaskbar = false;
                }
            }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            trackBar1.Value = 0;
            foreach (ListViewItem test_debug in listView1.SelectedItems)
            {

               // Debug.WriteLine(test_debug.Name);
                string index = test_debug.Name;

                try
                {
                    bool error = true;
                // istViewItem list = new ListViewItem("Debug");
                //list.SubItems.Add(index);
                //listView2.Items.Add(list);
                foreach (Process proc in Process.GetProcessesByName(index))
                {
                        label2.Show();
                        error = false;
                    checkBox1.Enabled = true;
                    checkBox1.Show();
                    checkBox1.Text = lang_basic_04;
                    label1.Show();
                    label1.ResetText();
                    label2.ResetText();
                    label2.Text = lang_basic_05;
                    if (proc.MainWindowTitle != "")
                    {
                        label1.Text = lang_basic_03 + proc.ProcessName + " (" + proc.MainWindowTitle + ")";
                    }
                    else
                    {
                        label1.Text = lang_basic_03 + proc.ProcessName;
                    }
                    Selected_row_proc_int = proc.Id;
                    checkBox1.Checked = false;
                    return_value = null;
                    current_checked_path = Get_Process(proc.Id);
                    if (current_checked_path == null)
                    {
                        checkBox1.Enabled = false;
                    }
                    else
                    {
                        //Set string return_value
                        get_file(current_checked_path);
                    }
                    trackBar1.Show();

                    trackBar1.Enabled = true;
                    trackBar1.Value = 0;
                    if (return_value != null)
                    {
                        string[] return_array = return_value.Split(',');
                        int x = 0;
                        foreach (string return_string in return_array)
                        {
                            if (x == 3)
                            {
                                checkBox1.Checked = Convert.ToBoolean(return_string);
                                test = return_string;
                            }
                            if (x == 2)
                            {
                                int track_value = 0;
                                Int32.TryParse(return_string, out track_value);
                                trackBar1.Value = track_value;
                                if (track_value != 0)
                                {
                                    label2.Text = Process.GetProcessById(Selected_row_proc_int).ProcessName + lang_basic_06 + track_value + lang_basic_07;
                                }
                            }

                            x++;
                        }
                    }

                }
                if(error == true)
                    {
                        label1.Text = lang_basic_03 + index;
                        checkBox1.Show();
                        checkBox1.Text = lang_basic_04;
                        checkBox1.Enabled = false;
                        label1.Show();
                        label1.ResetText();
                        label2.ResetText();
                        label2.Text = lang_error_02;
                    }

                }
                catch (Exception)
                {
                    label1.Text = lang_basic_03 + index;
                    checkBox1.Show();
                    checkBox1.Text = lang_basic_04;
                    checkBox1.Enabled = false;
                    label1.Show();
                    label2.Text = lang_error_02;
                }
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int a = trackBar1.Value;
            if (a == 0)
            {
                label2.Text = lang_basic_05;
            }
            else
            {
                try
                {
                    if (checkBox1.Checked)
                    {
                        label2.Text = Process.GetProcessById(Selected_row_proc_int).ProcessName + lang_basic_06 + a.ToString() + lang_basic_07;
                    }
                    else
                    {
                        label2.Text = Process.GetProcessById(Selected_row_proc_int).ProcessName + lang_basic_06 + a.ToString() + lang_basic_08;
                    }
                }
                catch (Exception)
                {
                    label2.Text = lang_error_02;
                }

            }
            if (Get_Process(Selected_row_proc_int) != null)
            {
                save(Selected_row_proc_int, a, checkBox1.Checked);
            }
            else
            {
                trackBar1.Enabled = false;
                checkBox1.Enabled = false;
                label2.Text = lang_error_03;

            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        public void save(int proc_id, int value_slider, bool restart)
        {
            if (Get_Process(proc_id) != Application.ExecutablePath)
            {
                try
                {

                    if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Process_list_config.cfg")))
                    {
                    }
                    else
                    {

                    }
                    bool rewrite = false;
                    rewrited = false;
                    text = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Process_list_config.cfg"));
                    string[] tokens = text.Split('\n');
                    foreach (string token in tokens)
                    {
                        rewrite = false;
                        int x_int = 0;
                        string[] list_token = token.Split(',');
                        foreach (string list_p in list_token)
                        {
                            if (x_int == 0)
                            {
                                cache = list_p;

                            }
                            if (x_int == 1)
                            {
                                if (list_p == Get_Process(proc_id))
                                {
                                    cache = cache + "," + Get_Process(proc_id);
                                    rewrite = true;
                                }

                            }
                            if (x_int == 2)
                            {
                                slider_cache = list_p;
                            }
                            if (x_int == 3)
                            {
                                if (rewrite == true)
                                {
                                    old_rewrited_string = cache + "," + slider_cache + "," + list_p;
                                    text = text.Replace(old_rewrited_string, Process.GetProcessById(proc_id).ProcessName + "," + Get_Process(proc_id) + "," + value_slider.ToString() + "," + restart.ToString());
                                    rewrited = true;

                                }
                            }

                            x_int++;
                        }

                    }
                    //DEBUG
                    //ListViewItem list1 = new ListViewItem("Rewrite_old");
                    //list1.SubItems.Add(old_rewrited_string);
                    //listView2.Items.Add(list1);
                    //ListViewItem list = new ListViewItem("Rewrite");
                    //list.SubItems.Add(Process.GetProcessById(proc_id).ProcessName + "," + Process.GetProcessById(proc_id).MainModule.FileName + "," + value_slider.ToString() + "," + restart.ToString());
                    //listView2.Items.Add(list);

                    if (rewrited == false)
                    {
                        text = text + "\n" + Process.GetProcessById(proc_id).ProcessName + "," + Get_Process(proc_id) + "," + value_slider.ToString() + "," + restart;
                    }

                    File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Process_list_config.cfg"), text);
                    file_changes = true;
                    rewrited_list_async = true;
                }
                catch (Exception)
                {

                }
            }
        }
        public void get_file(string path)
        {
            if (path != Application.ExecutablePath)
            {
                text = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Process_list_config.cfg"));
                string[] tokens = text.Split('\n');
                bool rewrite;
                foreach (string token in tokens)
                {
                    rewrite = false;
                    int x_int = 0;
                    string[] list_token = token.Split(',');
                    foreach (string list_p in list_token)
                    {
                        if (x_int == 0)
                        {
                            cache = list_p;

                        }
                        if (x_int == 1)
                        {
                            if (list_p == path)
                            {
                                cache = cache + "," + path;
                                rewrite = true;
                            }

                        }
                        if (x_int == 2)
                        {
                            slider_cache = list_p;
                        }
                        if (x_int == 3)
                        {
                            if (rewrite == true)
                            {
                                return_value = cache + "," + slider_cache + "," + list_p;
                                return;

                            }
                        }

                        x_int++;
                    }

                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (sender != null)
            {
                return_value = null;
                get_file(current_checked_path);
                if (return_value != null)
                {
                    string[] return_array = return_value.Split(',');
                    int x = 0;
                    foreach (string return_string in return_array)
                    {
                        if (x == 2)
                        {
                            int track_value = 0;
                            Int32.TryParse(return_string, out track_value);
                            try
                            {
                                if (checkBox1.Checked)
                                {
                                    label2.Text = Process.GetProcessById(Selected_row_proc_int).ProcessName + lang_basic_06 + track_value + lang_basic_07;
                                }
                                else
                                {
                                    label2.Text = Process.GetProcessById(Selected_row_proc_int).ProcessName + lang_basic_06 + track_value + lang_basic_08;
                                }
                            }
                            catch
                            {
                                trackBar1.Enabled = false;
                                checkBox1.Enabled = false;
                                label2.Text = lang_error_02;
                            }
                            if (Get_Process(Selected_row_proc_int) != null)
                            {
                                save(Selected_row_proc_int, track_value, checkBox1.Checked);
                            }
                            else
                            {
                                trackBar1.Enabled = false;
                                checkBox1.Enabled = false;
                                label2.Text = lang_error_03;
                            }

                        }

                        x++;
                    }
                }
                else
                {
                    if (Get_Process(Selected_row_proc_int) != null)
                    {
                        save(Selected_row_proc_int, track_value, checkBox1.Checked);
                    }
                    else
                    {
                        trackBar1.Enabled = false;
                        checkBox1.Enabled = false;
                        label2.Text = lang_error_03;
                    }
                }
            }

        }
        static async System.Threading.Tasks.Task Check_USAGE_CPU()
        {

                string text = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Process_list_config.cfg"));


            file_changes = false;
            string[] tokens;
            while (true)
            {
                try
                {
                    if (Form1.file_changes == true)
                    {
                        text = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Process_list_config.cfg"));

                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    tokens = text.Split('\n');
                    foreach (string token in tokens)
                    {
                        int x_int = 0;
                        string[] list_token = token.Split(',');
                        foreach (string list_p in list_token)
                        {

                            if (x_int == 0)
                            {
                                thread_name = list_p;
                            }
                            if (x_int == 1)
                            {
                                path = list_p;
                            }
                            if (x_int == 2)
                            {
                                Int32.TryParse(list_p, out value);
                            }
                            if (x_int == 3)
                            {
                                if (value != 0)
                                {
                                    restart_bool = Convert.ToBoolean(list_p);
                                    using (PerformanceCounter process_cpu = new PerformanceCounter("Process", "% Processor Time", thread_name))
                                    {
                                        //Debug.WriteLine(thread_name + " wird geprüft");
                                        var process_cpu_usage_start = process_cpu.NextValue();
                                        System.Threading.Thread.Sleep(1000);
                                        var process_cpu_usage = process_cpu.NextValue() / Environment.ProcessorCount;
                                        //Debug.WriteLine(thread_name + " hat eine Auslastung von " + process_cpu_usage + "% Core " + Environment.ProcessorCount);
                                        var process_cpu_usage_new = process_cpu.NextValue();
                                        if (process_cpu_usage > value)
                                        {
                                            try
                                            {
                                                foreach (Process proc in Process.GetProcessesByName(thread_name))
                                                {
                                                    proc.Kill();
                                                    System.Threading.Thread.Sleep(1000);
                                                }
                                            }
                                            catch (Exception)
                                            {

                                            }
                                            if (restart_bool)
                                            {
                                                ExecuteAsAdmin(Form1.path, true);
                                            }
                                        }

                                    }
                                }
                            }

                            x_int++;
                        }


                    }
                }
                catch (Exception)
                {

                }

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Lädt...";
            List_Box();
            button1.Text = lang_basic_09;

        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                if (checkBox3.Checked == false)
                {
                    rkApp.SetValue("Stop_High_CPU_usage", Application.ExecutablePath);
                }
                else
                {

                }
            }
            else
            {
                if (checkBox3.Checked)
                {
                    using (ts = new TaskService())
                    {
                        try
                        {
                            ts.RootFolder.DeleteTask("Stop_High_CPU_usage");

                            try
                            {
                                Microsoft.Win32.TaskScheduler.Task get_task = ts.GetTask("Stop_High_CPU_usage");

                                if (get_task.Enabled)
                                {
                                    checkBox3.Checked = true;
                                    checkBox2.Checked = true;
                                }
                            }
                            catch (Exception)
                            {
                                checkBox3.Checked = false;
                            }
                            }
                        catch (Exception)
                        {
                            checkBox3.Checked = true;
                            checkBox2.Checked = true;
                            checkBox3.Enabled = false;
                            checkBox2.Enabled = false;
                            checkBox3.Text = lang_error_04;
                            checkBox3.BackColor = Color.FromArgb(10, 218, 142, 61);
                            checkBox2.BackColor = Color.FromArgb(10, 218, 142, 61);
                        }
                    }
                }
                rkApp.DeleteValue("Stop_High_CPU_usage", false);
            }
        }
        private void checkBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox3.Checked == false)
            {
                rkApp.SetValue("Stop_High_CPU_usage", Application.ExecutablePath);
                try
                {
                    ts.RootFolder.DeleteTask("Stop_High_CPU_usage");

                    try
                    {
                        Microsoft.Win32.TaskScheduler.Task get_task = ts.GetTask("Stop_High_CPU_usage");

                        if (get_task.Enabled)
                        {
                            checkBox3.Checked = true;
                            checkBox2.Checked = true;
                        }
                    }
                    catch (Exception)
                    {
                        checkBox3.Checked = false;

                    }
                }
                catch
                {
                    checkBox3.Checked = true;
                    checkBox2.Checked = true;
                    checkBox3.Enabled = false;
                    checkBox2.Enabled = false;
                    checkBox3.Text = lang_error_04;
                    checkBox3.BackColor = Color.FromArgb(10, 218, 142, 61);
                    checkBox2.BackColor = Color.FromArgb(10, 218, 142, 61);
                }
            }
            else
            {
                try
                {
                    Microsoft.Win32.TaskScheduler.Task get_task = ts.GetTask("Stop_High_CPU_usage");

                    if (get_task.Enabled)
                    {
                        checkBox3.Checked = true;
                        checkBox2.Checked = true;
                    }
                }
                catch (Exception)
                {
                    using (ts = new TaskService())
                    {
                        // Create a new task definition and assign properties
                        TaskDefinition td = ts.NewTask();
                        td.RegistrationInfo.Description = "Autostart for Stop_High_CPU_usage";

                        // Create a trigger that will fire the task at this time every other day
                        td.Triggers.Add(new LogonTrigger { });
                        td.Principal.RunLevel = TaskRunLevel.Highest;
                        // Create an action that will launch Notepad whenever the trigger fires
                        td.Actions.Add(new ExecAction(Application.ExecutablePath, "", null));

                        // Register the task in the root folder
                        try
                        {
                            ts.RootFolder.RegisterTaskDefinition("Stop_High_CPU_usage", td);
                            rkApp.DeleteValue("Stop_High_CPU_usage", false);
                            checkBox3.Checked = true;
                            checkBox2.Checked = true;
                        }
                        catch (System.UnauthorizedAccessException)
                        {
                            checkBox3.Enabled = false;
                            checkBox3.Checked = false;
                            checkBox3.Text = lang_error_04;
                            checkBox3.BackColor = Color.FromArgb(10, 150, 62, 0);
                        }
                        // Remove the task we just created
                        //ts.RootFolder.("Test");
                    }
                }

            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lang_manager(comboBox1.SelectedItem.ToString());
        }
        private void lang_manager(string lang)
        {


            string[] lang_array = lang.Split('|');
            lang=lang_array[0].ToLower();
            //Debug.WriteLine(lang);

            if(lang=="deu")
            {
                listView1.Select();
                lang_basic_01 = "Programm Automatisch mit erhöhten rechten Starten.";
                lang_basic_02 = "Dieses Programm Automatisch mit Windows Starten";
                lang_basic_03 = "Einstellungen für ";
                lang_basic_04 = "Neustarten statt Schließen";
                lang_basic_05 = "AUS";
                lang_basic_06 = " wird ab einer CPU Auslastung von ";
                lang_basic_07 = "% Neugestartet";
                lang_basic_08 = "% Geschlossen";
                lang_basic_09 = "Liste Neuladen";
                lang_basic_10 = "Das Programm höhere Berechtigung erteilen";
                lang_error_01 = "⚠ | Zu wenig Rechte.";
                lang_error_02 = "⚠ | Fehler, das Programm scheint nicht mehr geöffnet zu sein.";
                lang_error_03 = "⚠ | Fehler, das Programm konnte den Pfad nicht ermitteln wahrscheinlich zu wenig Rechte.";
                lang_error_04 = "Programm Automatisch mit erhöhten rechten Starten ⚠ | Zu wenig Rechte um diese Einstellung zu Ändern!";
                lang_error_05 = "";
                lang_error_06 = "";
                lang_error_07 = "";
                lang_error_08 = "";
                lang_error_09 = "";
                lang_error_10 = "";
            }
            else
            {
                listView1.Select();
                lang_basic_01 = "Start this program with higher privileges.";
                lang_basic_02 = "Start this program automatically with Windows.";
                lang_basic_03 = "Options for ";
                lang_basic_04 = "Restart instead of closing.";
                lang_basic_05 = "OFF";
                lang_basic_06 = " will at a heigher CPU utilization of ";
                lang_basic_07 = "% be closed.";
                lang_basic_08 = "% be restarted.";
                lang_basic_09 = "Reload list";
                lang_basic_10 = "Obtain administrative rights";
                lang_error_01 = "⚠ | Insufficient rights.";
                lang_error_02 = "⚠ | Error, the program does not seem to be open any more."; ;
                lang_error_03 = "⚠ | Error, cant get the path of the program maybe insufficient rights for it.";
                lang_error_04 = "Start this program with higher privileges. ⚠ | Insufficient rights too change this Option!";
                lang_error_05 = "";
                lang_error_06 = "";
                lang_error_07 = "";
                lang_error_08 = "";
                lang_error_09 = "";
                lang_error_10 = "";

            }
            // ThreeLetterISOLanguageName
            button1.Text = lang_basic_09;
            checkBox2.Text = lang_basic_02;
            checkBox3.Text = lang_basic_01;
            checkBox1.Hide();
            checkBox1.Text = lang_basic_04;
            label1.Hide();
            label2.Hide();
            label2.Text = lang_basic_05;
            trackBar1.Hide();
            List_Box();
            button2.Text = lang_basic_10;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Process own_proc = System.Diagnostics.Process.GetCurrentProcess();
            Process proc = new Process();
            proc.StartInfo.FileName = Application.ExecutablePath;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
            proc.Close();
            own_proc.Kill();
        }
        public static bool CheckAdmin()
        {
            try
            {
                WindowsIdentity w_id = WindowsIdentity.GetCurrent();
                WindowsPrincipal w_principal = new WindowsPrincipal(w_id);
                return w_principal.IsInRole(WindowsBuiltInRole.Administrator);
            }catch {
                return false;
            }
        }
    }
}

