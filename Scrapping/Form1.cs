using System;
//using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Text.RegularExpressions;
using System.Windows.Forms;
//using System.Net;

using System.Threading;

namespace WinEmailExtract
{   
    public partial class Form1 : Form
    {
        string _Separator = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }                
       
        private void btnExtract_Click(object sender, EventArgs e)
        {
            try
            {
                _Separator = tbSeparator.Text.Trim();

                Extract Extract = new Extract { URL = this.tbURL.Text };

                Extract.OnCompletion += new Extract.Completion(Extract_OnCompletion);

                Thread thread = new Thread(new ThreadStart(Extract.Start));

                thread.Start();

                btnExtract.Enabled = false;              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        void Extract_OnCompletion(object o, EventArgs e)
        {
            //WriteToTextBox("Extracting Email addresses!");

            List<string> Emails = ((Extract)o).Emails;

            Emails.RemoveDuplicates((x, y) => x.Equals(y) == true ? 0 : 1);

            string EmailList="";
            //string EmailList = string.Empty;

            //Emails.ForEach(email => EmailList = EmailList + "\n" + email + ";");
            Emails.ForEach(email => EmailList = EmailList + "\n" + email + _Separator);

            WriteToTextBox(EmailList);
        }        

        private void WriteToTextBox(string Text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate() { rtbEmails.Text = rtbEmails.Text + Text; btnExtract.Enabled = true; }));
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbEmails.Text = string.Empty;
        }
    }
}
