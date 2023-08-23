namespace AquaBuildPresetter
{
    partial class Win_Boost166Detail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Win_Boost166Detail));
            label1 = new Label();
            label4 = new Label();
            boost166_ok = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(361, 68);
            label1.TabIndex = 0;
            label1.Text = "설정을 적용하면, VS2008 디폴트 기준 $(VSInstallDir) 경로인\r\nC:\\Program Files (x86)\\Microsoft Visual Studio 9.0\\ \r\n경로에 파일을 생성합니다.\r\n    - properties_win32.vsprops";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(12, 95);
            label4.Name = "label4";
            label4.Size = new Size(404, 34);
            label4.TabIndex = 4;
            label4.Text = "C:\\Program Files (x86)\\ 경로에 접근하기 위해서는 관리자 권한이 \r\n필요합니다.";
            // 
            // boost166_ok
            // 
            boost166_ok.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            boost166_ok.Location = new Point(172, 280);
            boost166_ok.Name = "boost166_ok";
            boost166_ok.Size = new Size(105, 29);
            boost166_ok.TabIndex = 5;
            boost166_ok.Text = "알겠어요";
            boost166_ok.UseVisualStyleBackColor = true;
            boost166_ok.Click += boost166_ok_Click;
            // 
            // Win_Boost166Detail
            // 
            AcceptButton = boost166_ok;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = boost166_ok;
            ClientSize = new Size(449, 321);
            Controls.Add(boost166_ok);
            Controls.Add(label4);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Win_Boost166Detail";
            RightToLeft = RightToLeft.No;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Boost 경로 매크로 설정(VS2008) - 자세히";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label4;
        private Button boost166_ok;
    }
}