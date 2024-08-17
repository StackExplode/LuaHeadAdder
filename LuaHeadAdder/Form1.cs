using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuaHeadAdder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var directoryDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "Select Folder",
                DefaultDirectory = @"H:\World of Warcraft\_retail_\Interface\AddOns"
            };
            var rst = directoryDialog.ShowDialog();
            if(rst == CommonFileDialogResult.Ok)
            {
                textBox1.Text = directoryDialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button4_Click(null,null);

            //Scan all file by an extension filter including sub folder into an array
            string[] files = System.IO.Directory.GetFiles(textBox1.Text, "*.lua", System.IO.SearchOption.AllDirectories);

            string code = "--Begin Jennings Add \r\n";
            code += textBox2.Text + "\r\n";
            code += "--End Jennings Add \r\n";

            foreach (var file in files)
            {
                //Append string to file's head and not overwrite the original file
                System.IO.File.WriteAllText(file, code + System.IO.File.ReadAllText(file),Encoding.UTF8);

            }
            MessageBox.Show("Add all patch to lua files success!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] files = System.IO.Directory.GetFiles(textBox1.Text, "*.lua", System.IO.SearchOption.AllDirectories);
            foreach (var file in files)
            {
                StreamReader sr = new StreamReader(file,Encoding.UTF8);
                MemoryStream ms = new MemoryStream();
                StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
                bool canread = true;
                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if(line.Contains("--Begin Jennings Add"))
                    {
                        canread = false;
                        continue;
                    }
                    else if(line.Contains("--End Jennings Add"))
                    {
                        canread = true;
                        continue;
                    }
                    if(canread)
                        sw.WriteLine(line);
                }
                sw.Close();
                ms.Close();
                sr.Close();
                System.IO.File.WriteAllBytes(file, ms.ToArray());
            }
            if(sender != null)
                MessageBox.Show("Remove all patch from lua files success!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = "Wait...";
            string url = "https://ghproxy.net/https://raw.githubusercontent.com/StackExplode/WowLuaAPIAdaptiveHead/master/template.lua";
            //Download string from url
            string code = new System.Net.WebClient().DownloadString(url);
            //Change code newline to windows style
            code = code.Replace("\n", "\r\n");
            textBox2.Text = code;
            button3.Text = "&Sync Code";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/StackExplode/WowLuaAPIAdaptiveHead");
        }
    }
}
