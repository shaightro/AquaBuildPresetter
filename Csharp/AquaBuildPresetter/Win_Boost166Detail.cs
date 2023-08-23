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
    public partial class Win_Boost166Detail : Form
    {

        public Win_Boost166Detail()
        {
            InitializeComponent();
            boost166_ok.TabIndex = 0;
        }



        private void boost166_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
