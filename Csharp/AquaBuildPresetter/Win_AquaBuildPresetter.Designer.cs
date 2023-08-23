using AquaBuildPresetter;
using System.Windows.Forms;

namespace AquaBuildPresetterApp
{
    partial class Win_AquaBuildPresetter
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Win_AquaBuildPresetter));
            label1 = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            svn_detail_button = new Button();
            svn_restore_default_button = new Button();
            svn_restore_backup_button = new Button();
            svn_apply_button = new Button();
            svn_ignore_pattern = new TextBox();
            svn_backup_config = new CheckBox();
            label2 = new Label();
            tabPage2 = new TabPage();
            boost170_delete_button = new Button();
            boost170_detail_button = new Button();
            boost170_apply_button = new Button();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            boost170_64lib_path = new TextBox();
            boost170_32lib_path = new TextBox();
            boost170_include_path = new TextBox();
            label3 = new Label();
            tabPage3 = new TabPage();
            boost166_delete_button = new Button();
            label11 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            vs2008_installdir = new TextBox();
            boost166_32lib_path = new TextBox();
            boost166_include_path = new TextBox();
            label7 = new Label();
            boost166_detail_button = new Button();
            boost166_apply_button = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(16, 15);
            label1.Name = "label1";
            label1.Size = new Size(363, 51);
            label1.TabIndex = 0;
            label1.Text = "SVN의 전역 무시 패턴(Global Ignore Pattern)을 설정합니다.\r\nTortoiseSVN이 설치된 상태에서 사용해야 합니다.\r\n이 기능은 TortoiseSVN의 무시 패턴 설정보다 우선합니다.";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(569, 288);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(svn_detail_button);
            tabPage1.Controls.Add(svn_restore_default_button);
            tabPage1.Controls.Add(svn_restore_backup_button);
            tabPage1.Controls.Add(svn_apply_button);
            tabPage1.Controls.Add(svn_ignore_pattern);
            tabPage1.Controls.Add(svn_backup_config);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(561, 258);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "SVN 무시 패턴 설정";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // svn_detail_button
            // 
            svn_detail_button.Location = new Point(445, 208);
            svn_detail_button.Name = "svn_detail_button";
            svn_detail_button.Size = new Size(97, 48);
            svn_detail_button.TabIndex = 7;
            svn_detail_button.Text = "자세히...";
            svn_detail_button.UseVisualStyleBackColor = true;
            svn_detail_button.Click += svn_detail_button_Click;
            // 
            // svn_restore_default_button
            // 
            svn_restore_default_button.Location = new Point(250, 208);
            svn_restore_default_button.Name = "svn_restore_default_button";
            svn_restore_default_button.Size = new Size(63, 48);
            svn_restore_default_button.TabIndex = 6;
            svn_restore_default_button.Text = "복원\r\n(디폴트)";
            svn_restore_default_button.UseVisualStyleBackColor = true;
            svn_restore_default_button.Click += svn_restore_default_button_Click;
            // 
            // svn_restore_backup_button
            // 
            svn_restore_backup_button.Location = new Point(172, 208);
            svn_restore_backup_button.Name = "svn_restore_backup_button";
            svn_restore_backup_button.Size = new Size(63, 48);
            svn_restore_backup_button.TabIndex = 5;
            svn_restore_backup_button.Text = "복원\r\n(백업)";
            svn_restore_backup_button.UseVisualStyleBackColor = true;
            svn_restore_backup_button.Click += svn_restore_backup_button_Click;
            // 
            // svn_apply_button
            // 
            svn_apply_button.Location = new Point(16, 208);
            svn_apply_button.Name = "svn_apply_button";
            svn_apply_button.Size = new Size(97, 48);
            svn_apply_button.TabIndex = 4;
            svn_apply_button.Text = "설정 적용";
            svn_apply_button.UseVisualStyleBackColor = true;
            svn_apply_button.Click += svn_apply_button_Click;
            // 
            // svn_ignore_pattern
            // 
            svn_ignore_pattern.Location = new Point(16, 108);
            svn_ignore_pattern.Multiline = true;
            svn_ignore_pattern.Name = "svn_ignore_pattern";
            svn_ignore_pattern.Size = new Size(526, 87);
            svn_ignore_pattern.TabIndex = 3;
            svn_ignore_pattern.Text = resources.GetString("svn_ignore_pattern.Text");
            // 
            // svn_backup_config
            // 
            svn_backup_config.AutoSize = true;
            svn_backup_config.Checked = true;
            svn_backup_config.CheckState = CheckState.Checked;
            svn_backup_config.Location = new Point(385, 87);
            svn_backup_config.Name = "svn_backup_config";
            svn_backup_config.Size = new Size(157, 21);
            svn_backup_config.TabIndex = 2;
            svn_backup_config.Text = "기존 config 파일 백업";
            svn_backup_config.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(16, 88);
            label2.Name = "label2";
            label2.Size = new Size(179, 17);
            label2.TabIndex = 1;
            label2.Text = "무시 패턴 입력 (기본값 권장)";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(boost170_delete_button);
            tabPage2.Controls.Add(boost170_detail_button);
            tabPage2.Controls.Add(boost170_apply_button);
            tabPage2.Controls.Add(label6);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(boost170_64lib_path);
            tabPage2.Controls.Add(boost170_32lib_path);
            tabPage2.Controls.Add(boost170_include_path);
            tabPage2.Controls.Add(label3);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(561, 258);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Boost 경로 매크로 설정 (VS2019)";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // boost170_delete_button
            // 
            boost170_delete_button.Location = new Point(172, 208);
            boost170_delete_button.Name = "boost170_delete_button";
            boost170_delete_button.Size = new Size(63, 48);
            boost170_delete_button.TabIndex = 10;
            boost170_delete_button.Text = "삭제";
            boost170_delete_button.UseVisualStyleBackColor = true;
            boost170_delete_button.Click += boost170_delete_button_Click;
            // 
            // boost170_detail_button
            // 
            boost170_detail_button.Location = new Point(445, 208);
            boost170_detail_button.Name = "boost170_detail_button";
            boost170_detail_button.Size = new Size(97, 48);
            boost170_detail_button.TabIndex = 9;
            boost170_detail_button.Text = "자세히...";
            boost170_detail_button.UseVisualStyleBackColor = true;
            boost170_detail_button.Click += boost170_detail_button_Click;
            // 
            // boost170_apply_button
            // 
            boost170_apply_button.Location = new Point(16, 208);
            boost170_apply_button.Name = "boost170_apply_button";
            boost170_apply_button.Size = new Size(97, 48);
            boost170_apply_button.TabIndex = 8;
            boost170_apply_button.Text = "설정 적용";
            boost170_apply_button.UseVisualStyleBackColor = true;
            boost170_apply_button.Click += boost170_apply_button_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 135);
            label6.Name = "label6";
            label6.Size = new Size(181, 17);
            label6.TabIndex = 7;
            label6.Text = "Boost 1.70 64bit Library 경로";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 104);
            label5.Name = "label5";
            label5.Size = new Size(181, 17);
            label5.TabIndex = 6;
            label5.Text = "Boost 1.70 32bit Library 경로";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(95, 73);
            label4.Name = "label4";
            label4.Size = new Size(102, 17);
            label4.TabIndex = 5;
            label4.Text = "Boost 1.70 경로";
            // 
            // boost170_64lib_path
            // 
            boost170_64lib_path.Location = new Point(203, 132);
            boost170_64lib_path.Name = "boost170_64lib_path";
            boost170_64lib_path.Size = new Size(339, 25);
            boost170_64lib_path.TabIndex = 4;
            // 
            // boost170_32lib_path
            // 
            boost170_32lib_path.Location = new Point(203, 101);
            boost170_32lib_path.Name = "boost170_32lib_path";
            boost170_32lib_path.Size = new Size(339, 25);
            boost170_32lib_path.TabIndex = 3;
            // 
            // boost170_include_path
            // 
            boost170_include_path.Location = new Point(203, 70);
            boost170_include_path.Name = "boost170_include_path";
            boost170_include_path.Size = new Size(339, 25);
            boost170_include_path.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(16, 15);
            label3.Name = "label3";
            label3.Size = new Size(534, 34);
            label3.TabIndex = 1;
            label3.Text = "Visual Studio 2019의 Boost 1.70버전 포함 및 라이브러리 경로 설정에 사용하는 매크로를\r\n설정합니다. 그러면 매크로 $(boost_1_70), $(boost_1_70_lib)이 생성됩니다.";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(boost166_delete_button);
            tabPage3.Controls.Add(label11);
            tabPage3.Controls.Add(label8);
            tabPage3.Controls.Add(label9);
            tabPage3.Controls.Add(label10);
            tabPage3.Controls.Add(vs2008_installdir);
            tabPage3.Controls.Add(boost166_32lib_path);
            tabPage3.Controls.Add(boost166_include_path);
            tabPage3.Controls.Add(label7);
            tabPage3.Controls.Add(boost166_detail_button);
            tabPage3.Controls.Add(boost166_apply_button);
            tabPage3.Location = new Point(4, 26);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(561, 258);
            tabPage3.TabIndex = 1;
            tabPage3.Text = "Boost 경로 매크로 설정 (VS2008)";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // boost166_delete_button
            // 
            boost166_delete_button.Location = new Point(172, 208);
            boost166_delete_button.Name = "boost166_delete_button";
            boost166_delete_button.Size = new Size(63, 48);
            boost166_delete_button.TabIndex = 17;
            boost166_delete_button.Text = "삭제";
            boost166_delete_button.UseVisualStyleBackColor = true;
            boost166_delete_button.Click += boost166_delete_button_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(16, 169);
            label11.Name = "label11";
            label11.Size = new Size(500, 17);
            label11.TabIndex = 16;
            label11.Text = "※ 설치 경로가 C:\\Program Files (x86)\\ 내에 있는 경우, 관리자 권한이 필요합니다.";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(16, 135);
            label8.Name = "label8";
            label8.Size = new Size(181, 17);
            label8.TabIndex = 15;
            label8.Text = "Visual Studio 2008 설치 경로";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(16, 104);
            label9.Name = "label9";
            label9.Size = new Size(181, 17);
            label9.TabIndex = 14;
            label9.Text = "Boost 1.66 32bit Library 경로";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(95, 73);
            label10.Name = "label10";
            label10.Size = new Size(102, 17);
            label10.TabIndex = 13;
            label10.Text = "Boost 1.66 경로";
            // 
            // vs2008_installdir
            // 
            vs2008_installdir.Location = new Point(203, 132);
            vs2008_installdir.Name = "vs2008_installdir";
            vs2008_installdir.Size = new Size(339, 25);
            vs2008_installdir.TabIndex = 12;
            vs2008_installdir.Text = "C:\\Program Files (x86)\\Microsoft Visual Studio 9.0\\";
            // 
            // boost166_32lib_path
            // 
            boost166_32lib_path.Location = new Point(203, 101);
            boost166_32lib_path.Name = "boost166_32lib_path";
            boost166_32lib_path.Size = new Size(339, 25);
            boost166_32lib_path.TabIndex = 11;
            // 
            // boost166_include_path
            // 
            boost166_include_path.Location = new Point(203, 70);
            boost166_include_path.Name = "boost166_include_path";
            boost166_include_path.Size = new Size(339, 25);
            boost166_include_path.TabIndex = 10;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(16, 15);
            label7.Name = "label7";
            label7.Size = new Size(534, 34);
            label7.TabIndex = 9;
            label7.Text = "Visual Studio 2019의 Boost 1.70버전 포함 및 라이브러리 경로 설정에 사용하는 매크로를\r\n설정합니다. 그러면 매크로 $(boost_1_70), $(boost_1_70_lib)이 생성됩니다.";
            // 
            // boost166_detail_button
            // 
            boost166_detail_button.Location = new Point(445, 208);
            boost166_detail_button.Name = "boost166_detail_button";
            boost166_detail_button.Size = new Size(97, 48);
            boost166_detail_button.TabIndex = 8;
            boost166_detail_button.Text = "자세히...";
            boost166_detail_button.UseVisualStyleBackColor = true;
            boost166_detail_button.Click += boost166_detail_button_Click;
            // 
            // boost166_apply_button
            // 
            boost166_apply_button.Location = new Point(16, 208);
            boost166_apply_button.Name = "boost166_apply_button";
            boost166_apply_button.Size = new Size(97, 48);
            boost166_apply_button.TabIndex = 5;
            boost166_apply_button.Text = "설정 적용";
            boost166_apply_button.UseVisualStyleBackColor = true;
            boost166_apply_button.Click += boost166_apply_button_Click;
            // 
            // Win_AquaBuildPresetter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(593, 312);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            Name = "Win_AquaBuildPresetter";
            Text = "Aqua Build Presetter";
            KeyDown += Win_AquaBuildPresetter_KeyDown;
            Resize += subFormResize;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private Label label2;
        private CheckBox svn_backup_config;
        private TextBox svn_ignore_pattern;
        private Button svn_apply_button;
        private Button svn_detail_button;
        private Button svn_restore_default_button;
        private Button svn_restore_backup_button;
        private Label label3;
        private TextBox boost170_include_path;
        private Button boost170_detail_button;
        private Button boost170_apply_button;
        private Label label6;
        private Label label5;
        private Label label4;
        private TextBox boost170_64lib_path;
        private TextBox boost170_32lib_path;
        private Label label11;
        private Label label8;
        private Label label9;
        private Label label10;
        private TextBox vs2008_installdir;
        private TextBox boost166_32lib_path;
        private TextBox boost166_include_path;
        private Label label7;
        private Button boost166_detail_button;
        private Button boost166_apply_button;
        private Button boost170_delete_button;
        private Button boost166_delete_button;
    }
}