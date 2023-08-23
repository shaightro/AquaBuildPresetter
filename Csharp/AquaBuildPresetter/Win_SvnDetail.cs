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

namespace AquaBuildPresetter
{
    public partial class Win_SvnDetail : Form
    {
        public Win_SvnDetail()
        {
            InitializeComponent();
            string username = Environment.UserName;
            string new_string;
            new_string = "설정을 적용하면,\r\n";
            new_string += "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\";
            new_string += "\r\n경로의 config 파일을 수정합니다.";
            label1.Text = new_string;
            svn_ok.TabIndex = 0;
            open_svn_dir.TabIndex = 1;
        }

        private void open_dir_button_Click(object sender, EventArgs e)
        {
            string username = Environment.UserName;
            string svn_dir = "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\";
            if (Directory.Exists(svn_dir))
            {
                Process.Start("explorer.exe", svn_dir);
            }
            else
            {
                MessageBox.Show(this, "SVN 경로가 존재하지 않아 열 수 없습니다.\r\nSVN(TortoiseSVN 등)을 설치하고 사용해 주세요.",
                    "어라?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void svn_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
