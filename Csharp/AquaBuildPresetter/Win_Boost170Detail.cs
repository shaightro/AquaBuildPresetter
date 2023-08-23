using Microsoft.VisualBasic.ApplicationServices;
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
    public partial class Win_Boost170Detail : Form
    {
        public Win_Boost170Detail()
        {
            InitializeComponent();
            string username = Environment.UserName;
            string new_string;

            new_string = "설정을 적용하면, 디폴트 $(UserRootDir)경로인\r\n";
            new_string += "C:\\Users\\" + username + "\\AppData\\Local\\Microsoft\\MSBuild\\v4.0\\";
            new_string += "\r\n경로를 만들고, 이 경로에 두 개의 파일을 생성합니다.";
            new_string += "\r\n    - Microsoft.Cpp.Win32.user.props";
            new_string += "\r\n    - Microsoft.Cpp.x64.user.props";
            label1.Text = new_string;
            boost170_ok.TabIndex = 0;

        }


        private void boost170_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
