namespace AquaBuildPresetter
{
    partial class Win_Boost170Detail
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Win_Boost170Detail));
            label1 = new Label();
            label3 = new Label();
            label4 = new Label();
            boost170_ok = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(420, 85);
            label1.TabIndex = 0;
            label1.Text = "설정을 적용하면, 디폴트 $(UserRootDir) 경로인\r\nC:\\Users\\(사용자 계정)\\AppData\\Local\\Microsoft\\MSBuild\\v4.0\\\r\n경로를 만들고, 이 경로에 두 개의 파일을 생성합니다.\r\n    - Microsoft.Cpp.Win32.user.props\r\n    - Microsoft.Cpp.Win64.user.props";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(12, 105);
            label3.Name = "label3";
            label3.Size = new Size(395, 68);
            label3.TabIndex = 3;
            label3.Text = "각 파일은 Aqua 엔진에서 자동으로 읽어들이고, 이 파일에 설정된\r\nBoost 관련 경로에 대한 매크로가 생성됩니다.\r\n그래서 빌드 설정에서 Boost 경로를 엔진마다 각각 입력할 필요가\r\n없게 되는 것입니다.";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(12, 185);
            label4.Name = "label4";
            label4.Size = new Size(405, 34);
            label4.TabIndex = 4;
            label4.Text = "각 파일의 내용은 Visual Studio에서 '속성 관리자'를 통해 확인할 수\r\n있습니다.";
            // 
            // boost170_ok
            // 
            boost170_ok.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            boost170_ok.Location = new Point(172, 280);
            boost170_ok.Name = "boost170_ok";
            boost170_ok.Size = new Size(105, 29);
            boost170_ok.TabIndex = 5;
            boost170_ok.Text = "알겠어요";
            boost170_ok.UseVisualStyleBackColor = true;
            boost170_ok.Click += boost170_ok_Click;
            // 
            // Win_Boost170Detail
            // 
            AcceptButton = boost170_ok;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = boost170_ok;
            ClientSize = new Size(449, 321);
            Controls.Add(boost170_ok);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Win_Boost170Detail";
            RightToLeft = RightToLeft.No;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Boost 경로 매크로 설정(VS2019) - 자세히";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label3;
        private Label label4;
        private Button boost170_ok;
    }
}