namespace AquaBuildPresetter
{
    partial class Win_SvnDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Win_SvnDetail));
            label1 = new Label();
            open_svn_dir = new Button();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            svn_ok = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(361, 51);
            label1.TabIndex = 0;
            label1.Text = "설정을 적용하면,\r\nC:\\Users\\(사용자 계정)\\AppData\\Roaming\\Subversion\\\r\n경로의 config 파일을 수정합니다.";
            // 
            // open_svn_dir
            // 
            open_svn_dir.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            open_svn_dir.Location = new Point(356, 43);
            open_svn_dir.Name = "open_svn_dir";
            open_svn_dir.Size = new Size(75, 23);
            open_svn_dir.TabIndex = 1;
            open_svn_dir.Text = "경로 열기";
            open_svn_dir.UseVisualStyleBackColor = true;
            open_svn_dir.Click += open_dir_button_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(12, 76);
            label2.Name = "label2";
            label2.Size = new Size(419, 34);
            label2.TabIndex = 2;
            label2.Text = "global-ignores가 주석 처리되어 있으면 해제하고, 입력된 무시 패턴을\r\n해당 변수에 추가해 줍니다.";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(12, 129);
            label3.Name = "label3";
            label3.Size = new Size(407, 34);
            label3.TabIndex = 3;
            label3.Text = "config 파일의 global-ignores 변수가 살아 있으면, TortoiseSVN에서\r\n설정한 global ignores 패턴은 무시됩니다.";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(12, 182);
            label4.Name = "label4";
            label4.Size = new Size(419, 51);
            label4.TabIndex = 4;
            label4.Text = "'기존 config 파일 백업' 옵션을 사용하면, 기존의 config 파일을\r\nconfig_backup_byABP 이름으로 복사해 둡니다. 만약 나중에 수동으로\r\n설정을 변경하려고 할 때 사용할 수 있습니다. 굳이?";
            // 
            // svn_ok
            // 
            svn_ok.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            svn_ok.Location = new Point(172, 280);
            svn_ok.Name = "svn_ok";
            svn_ok.Size = new Size(105, 29);
            svn_ok.TabIndex = 5;
            svn_ok.Text = "알겠어요";
            svn_ok.UseVisualStyleBackColor = true;
            svn_ok.Click += svn_ok_Click;
            // 
            // Win_SvnDetail
            // 
            AcceptButton = svn_ok;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = svn_ok;
            ClientSize = new Size(449, 321);
            Controls.Add(svn_ok);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(open_svn_dir);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Win_SvnDetail";
            RightToLeft = RightToLeft.No;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "SVN 무시 패턴 설정 - 자세히";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button open_svn_dir;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button svn_ok;
    }
}