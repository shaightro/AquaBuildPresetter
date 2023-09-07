using AquaBuildPresetter;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace AquaBuildPresetterApp
{
    public partial class Win_AquaBuildPresetter : Form
    {
        private Win_SvnDetail? svn_subForm;
        private Win_Boost170Detail? boost170_subForm;
        private Win_Boost166Detail? boost166_subForm;
        private Form? currentSubForm;

        public Win_AquaBuildPresetter()
        {
            InitializeComponent();
            svn_subForm = null;
            boost170_subForm = null;
            boost166_subForm = null;
            currentSubForm = null;
            tabControl1.TabIndex = 0;
            svn_backup_config.TabIndex = 1;
            svn_ignore_pattern.TabIndex = 2;
            svn_apply_button.TabIndex = 3;
            svn_restore_backup_button.TabIndex = 4;
            svn_restore_default_button.TabIndex = 5;
            svn_detail_button.TabIndex = 6;
            boost170_include_path.TabIndex = 7;
            boost170_32lib_path.TabIndex = 8;
            boost170_64lib_path.TabIndex = 9;
            boost170_apply_button.TabIndex = 10;
            boost170_delete_button.TabIndex = 11;
            boost170_detail_button.TabIndex = 12;
            boost166_include_path.TabIndex = 13;
            boost166_32lib_path.TabIndex = 14;
            vs2008_installdir.TabIndex = 15;
            boost166_apply_button.TabIndex = 16;
            boost166_delete_button.TabIndex = 17;
            boost166_detail_button.TabIndex = 18;

        }

        private void svn_apply_button_Click(object sender, EventArgs e)
        {
            try
            {
                string ignore_pattern = svn_ignore_pattern.Text.Trim(); // �Է¹��� ���� ����
                bool backup_flag = svn_backup_config.Checked;
                bool backup_done;

                // �Է¹��� ���� ���Ͽ� \r\n�� ������ ���� �ش�
                ignore_pattern = ignore_pattern.Replace("\r\n", " ");
                // ���ӵ� ���� �ϳ��� ����
                string pattern = "\\s+";
                ignore_pattern = Regex.Replace(ignore_pattern, pattern, " ");

                // �Է¹��� ���� ���� �и�
                string[] pattern_list = ignore_pattern.Split(new char[] { ' ', '\n', '\r' },
                    StringSplitOptions.RemoveEmptyEntries);

                // SVN ��ġ Ȯ��(��� ���� Ȯ��)
                string username = Environment.UserName;
                string svn_dir = "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\";
                if (!(Directory.Exists(svn_dir)))
                {
                    svn_dir = "C:\\Documents and Settings\\" + username + "\\ApplicationData\\Subversion\\";
                    if (!(Directory.Exists(svn_dir)))
                    {
                        // ������� ���� ��� ��ü�� �����Ƿ�, SVN ��ġ�� �� �� ������ ����
                        MessageBox.Show(this, "SVN ��ΰ� �������� �ʽ��ϴ�.\r\nSVN(TortoiseSVN ��)�� ���� ��ġ�� �ּ���.",
                            "��?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string config_file = svn_dir + "config";
                // SVN ��� �� config ���� ���� Ȯ��
                if (!(File.Exists(config_file)))
                {
                    MessageBox.Show(this, "SVN ��δ� �ִµ� config ������ �����ϴ�.\r\n�Ϲ������δ� �Ͼ�� �ʴ� ��Ȳ�� �� �����ϴ�.",
                            "��?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ��� �÷��� = true�̸�, ����Ѵ�
                if (backup_flag)
                {
                    string backup_config_file = svn_dir + "config_backup_byABP";
                    if (File.Exists(backup_config_file))
                    {
                        if (MessageBox.Show(this, "��� ������ �̹� �����ϴ� �� �����ϴ�.\r\n���� ����Ͻðڽ��ϱ�?\r\n���� ����� ����� �˴ϴ�.",
                            "��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            File.Copy(config_file, backup_config_file, true);
                            backup_done = true;
                        }
                        else
                        {
                            backup_done = false;
                        }
                    }
                    else
                    {
                        File.Copy(config_file, backup_config_file, true);
                        backup_done = true;
                    }
                }
                else
                {
                    backup_done = false;
                }

                // config ������ �а� �����Ѵ�
                string new_config = "";
                string new_string = "";
                bool switch_ig_block = false;

                using (StreamReader reader = new StreamReader(config_file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // �ϴ� line�� global-ignores, = �� ���ԵǾ� �ִ��� Ȯ��
                        if (line.Contains("global-ignores") && line.Contains('='))
                        {
                            switch_ig_block = true;
                            // global-ignores�� ������, �� �տ� �Է¹��� ���� ������ �߰��Ѵ�
                            new_string = "global-ignores = " + ignore_pattern + "\r\n";
                        }
                        // global-ignores�� ������ switch-ig-block�� True�� ���, ����� ���� �������� �ּ� ó���� ���ش�
                        // block�� ������ �� ó���� #�� �� �ϳ� Ȥ�� 0������ ������. ���� �̰� �ٲ� ���� ������
                        // �׸��� �Է¹��� ���� ���Ͽ� ���Ե��� ���� �༮�鸸 �߰��� �ش�
                        else if (switch_ig_block)
                        {
                            if ((line.StartsWith("#") && !(line.StartsWith("###"))) || line.StartsWith(" "))
                            {
                                line = " " + line.Replace("\r\n", " ").TrimStart('#');
                                string[] now_list = line.Split(new char[] { ' ', '\n', '\r' },
                                    StringSplitOptions.RemoveEmptyEntries);
                                if (now_list.Length > 0)
                                {
                                    var difference = now_list.Except(pattern_list);

                                    if (difference.Any())
                                    {
                                        new_string = "    ";
                                        foreach (var item in difference)
                                        {
                                            new_string += item + " ";
                                        }
                                        new_string = new_string.TrimEnd() + "\r\n";
                                        if (new_string.Replace("\r\n", "").Trim() == "")
                                        {
                                            new_string = "";
                                        }
                                    }
                                    else
                                    {
                                        new_string = "";
                                    }
                                }
                                else
                                {
                                    new_string = "";
                                }
                            }
                            else
                            {
                                switch_ig_block = false;    // ��� ��!
                                new_string = line + "\r\n";
                            }
                        }
                        else
                        {
                            // �̰͵� ���͵� �ƴϸ� �׳� ������ line�� �����ش�
                            new_string = line + "\r\n";
                        }

                        if (new_string.Length > 0)
                        {
                            new_config += new_string;
                        }
                    }
                }

                using (StreamWriter writer = new StreamWriter(config_file))
                {
                    writer.WriteLine(new_config);
                }

                string success_msg = "������ �Ϸ��Ͽ����ϴ�.";
                if (backup_done)
                {
                    success_msg += "\r\n��� ���ϵ� ����Ǿ����ϴ�.";
                }
                else
                {
                    success_msg += "\r\n����� ���� �ʾҽ��ϴ�.";
                }

                MessageBox.Show(this, success_msg, "����!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show(this,
                    "�������� ���� ������ �߻��߽��ϴ�.",
                    "��!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void svn_restore_backup_button_Click(object sender, EventArgs e)
        {
            try
            {
                // ������� ����
                string username = Environment.UserName;
                string svn_dir = "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\";
                if (!(Directory.Exists(svn_dir)))
                {
                    svn_dir = "C:\\Documents and Settings\\" + username + "\\ApplicationData\\Subversion\\";
                    if (!(Directory.Exists(svn_dir)))
                    {
                        // ������� ���� ��� ��ü�� �����Ƿ�, SVN ��ġ�� �� �� ������ ����
                        MessageBox.Show(this, "SVN ��ΰ� �������� �ʽ��ϴ�.\r\nSVN(TortoiseSVN ��)�� ���� ��ġ�� �ּ���.",
                            "��?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string config_file = svn_dir + "config";
                string backup_config_file = svn_dir + "config_backup_byABP";
                // SVN ��� �� config ���� ���� Ȯ��
                if (!(File.Exists(backup_config_file)))
                {
                    MessageBox.Show(this, "��� ������ �������� �ʾ� ����� �� �����ϴ�.\n��� ����Ʈ���� ����� �õ��� ������.",
                            "��?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // ����
                File.Copy(backup_config_file, config_file, true);

                MessageBox.Show(this, "������� ������ �Ϸ��߽��ϴ�.",
                            "����!", MessageBoxButtons.OK, MessageBoxIcon.Information); ;

            }
            catch
            {
                MessageBox.Show(this,
                    "�������� ���� ������ �߻��߽��ϴ�.",
                    "��!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void svn_restore_default_button_Click(object sender, EventArgs e)
        {
            // ����Ʈ���� ����
            try
            {
                string username = Environment.UserName;
                string svn_dir = "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\";
                if (!(Directory.Exists(svn_dir)))
                {
                    svn_dir = "C:\\Documents and Settings\\" + username + "\\ApplicationData\\Subversion\\";
                    if (!(Directory.Exists(svn_dir)))
                    {
                        // ������� ���� ��� ��ü�� �����Ƿ�, SVN ��ġ�� �� �� ������ ����
                        MessageBox.Show(this, "SVN ��ΰ� �������� �ʽ��ϴ�.\r\nSVN(TortoiseSVN ��)�� ���� ��ġ�� �ּ���.",
                            "��?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string config_file = svn_dir + "config";

                string file_contents = DefaultSvnIgnore.Get();

                using (StreamWriter writer = new StreamWriter(config_file))
                {
                    writer.WriteLine(file_contents);
                }
                MessageBox.Show(this,
                    "����Ʈ���� ������ �Ϸ��߽��ϴ�.",
                    "����!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(this,
                    "�������� ���� ������ �߻��߽��ϴ�.",
                    "��!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void svn_detail_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentSubForm != null && !currentSubForm.IsDisposed)
                {
                    currentSubForm.Close();
                }
                svn_subForm = new Win_SvnDetail();
                int x = this.Location.X;
                int y = this.Location.Y;
                svn_subForm.Location = new Point(x + 605, y);
                svn_subForm.Show();
                currentSubForm = svn_subForm;
            }
            catch
            {
                MessageBox.Show(this,
                    "�������� ���� ������ �߻��߽��ϴ�.",
                    "��!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }



        private void boost170_apply_button_Click(object sender, EventArgs e)
        {
            try
            {
                string include_path = boost170_include_path.Text.Trim();
                string lib32_path = boost170_32lib_path.Text.Trim();
                string lib64_path = boost170_64lib_path.Text.Trim();

                // ��� �˻� include_path
                if (include_path.Length < 2)
                {
                    MessageBox.Show(this,
                        "Boost 1.70 ��� \"" + include_path + "\" �� ���������� �ʽ��ϴ�.\r\n�ʹ� ª���ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (include_path[1] != ':')
                {
                    MessageBox.Show(this,
                        "Boost 1.70 ��� \"" + include_path + "\" �� ���������� �ʽ��ϴ�.\r\nC:, D: ������ �����ؾ� �մϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (include_path[include_path.Length - 1] != '\\')
                {
                    include_path += "\\";
                }
                if (!(Directory.Exists(include_path)))
                {
                    MessageBox.Show(this,
                        "Boost 1.70 ��� \"" + include_path + "\" �� �������� �ʽ��ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (!(Directory.Exists(include_path + "boost\\")))
                {
                    MessageBox.Show(this,
                        "Boost 1.70 ���  \"" + include_path + "\" �ȿ� boost ���丮�� �����ϴ�.\r\nBoost ��ΰ� �ƴ� �� �����ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // ��� �˻� lib32_path
                if (lib32_path.Length < 2)
                {
                    MessageBox.Show(this,
                        "Boost 1.70 32bit Library ��� \"" + lib32_path + "\" �� ���������� �ʽ��ϴ�.\r\n�ʹ� ª���ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (lib32_path[1] != ':')
                {
                    MessageBox.Show(this,
                        "Boost 1.70 32bit Library ��� \"" + lib32_path + "\" �� ���������� �ʽ��ϴ�.\r\nC:, D: ������ �����ؾ� �մϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (lib32_path[lib32_path.Length - 1] != '\\')
                {
                    lib32_path += "\\";
                }
                if (!(Directory.Exists(lib32_path)))
                {
                    MessageBox.Show(this,
                        "Boost 1.70 32bit Library ��� \"" + lib32_path + "\" �� �������� �ʽ��ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                string[] libfiles = Directory.GetFiles(lib32_path);
                bool libexist = false;
                for (int i = 0; i < libfiles.Length; i++)
                {
                    if (libfiles[i].EndsWith(".lib", StringComparison.OrdinalIgnoreCase))
                    {
                        libexist = true;
                        break;
                    }
                }
                if (libexist == false)
                {
                    MessageBox.Show(this,
                        "Boost 1.70 32bit Library ��� \"" + lib32_path + "\"  �ȿ� boost lib ������ ���� �� �����ϴ�.\r\nBoost Library��ΰ� �ƴ� �� �����ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // ��� �˻� lib64_path
                if (lib64_path.Length < 2)
                {
                    MessageBox.Show(this,
                        "Boost 1.70 64bit Library ��� \"" + lib64_path + "\" �� ���������� �ʽ��ϴ�.\r\n�ʹ� ª���ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (lib64_path[1] != ':')
                {
                    MessageBox.Show(this,
                        "Boost 1.70 64bit Library ��� \"" + lib64_path + "\" �� ���������� �ʽ��ϴ�.\r\nC:, D: ������ �����ؾ� �մϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (lib64_path[lib64_path.Length - 1] != '\\')
                {
                    lib64_path += "\\";
                }
                if (!(Directory.Exists(lib64_path)))
                {
                    MessageBox.Show(this,
                        "Boost 1.70 64bit Library ��� \"" + lib64_path + "\" �� �������� �ʽ��ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                string[] libfiles2 = Directory.GetFiles(lib64_path);
                bool libexist2 = false;
                for (int i = 0; i < libfiles2.Length; i++)
                {
                    if (libfiles2[i].EndsWith(".lib", StringComparison.OrdinalIgnoreCase))
                    {
                        libexist2 = true;
                        break;
                    }
                }
                if (libexist2 == false)
                {
                    MessageBox.Show(this,
                        "Boost 1.70 64bit Library ��� \"" + lib64_path + "\"  �ȿ� boost lib ������ ���� �� �����ϴ�.\r\nBoost Library��ΰ� �ƴ� �� �����ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // C:\Users\(����� id)\AppData\Local\Microsoft\MSBuild\v4.0 �� �����
                string username = Environment.UserName;
                string props_dir = "C:\\Users\\" + username + "\\AppData\\Local\\Microsoft\\MSBuild\\v4.0\\";

                if (!(Directory.Exists(props_dir)))
                {
                    Directory.CreateDirectory(props_dir);
                }

                // Ȥ�ó� ������ �ִ��� üũ
                string props_32 = props_dir + "Microsoft.Cpp.Win32.user.props";
                string props_64 = props_dir + "Microsoft.Cpp.x64.user.props";
                if (File.Exists(props_32) || File.Exists(props_64))
                {
                    // ����� ���� Ȯ��
                    if (MessageBox.Show(this, "���� ������ �̹� �����մϴ�.\r\n����ðڽ��ϱ�?",
                        "��..", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        MessageBox.Show(this,
                            "���� ������ �������� �ɰ� �����߽��ϴ�.",
                            "��..",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }

                string file_contents_32 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <ImportGroup Label=""PropertySheets"" />
  <PropertyGroup Label=""UserMacros"">
    <boost_1_70>" + include_path + @"</boost_1_70>
    <boost_1_70_lib>" + lib32_path + @"</boost_1_70_lib>
  </PropertyGroup>
  <PropertyGroup />
  <ItemDefinitionGroup />
  <ItemGroup>
    <BuildMacro Include=""boost_1_70"">
      <Value>$(boost_1_70)</Value>
    </BuildMacro>
    <BuildMacro Include=""boost_1_70_lib"">
      <Value>$(boost_1_70_lib)</Value>
    </BuildMacro>
  </ItemGroup>
</Project>";

                string file_contents_64 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <ImportGroup Label=""PropertySheets"" />
  <PropertyGroup Label=""UserMacros"">
    <boost_1_70>" + include_path + @"</boost_1_70>
    <boost_1_70_lib>" + lib64_path + @"</boost_1_70_lib>
  </PropertyGroup>
  <PropertyGroup />
  <ItemDefinitionGroup />
  <ItemGroup>
    <BuildMacro Include=""boost_1_70"">
      <Value>$(boost_1_70)</Value>
    </BuildMacro>
    <BuildMacro Include=""boost_1_70_lib"">
      <Value>$(boost_1_70_lib)</Value>
    </BuildMacro>
  </ItemGroup>
</Project>";

                using (StreamWriter writer = new StreamWriter(props_32))
                {
                    writer.WriteLine(file_contents_32);
                }
                using (StreamWriter writer = new StreamWriter(props_64))
                {
                    writer.WriteLine(file_contents_64);
                }

                MessageBox.Show(this, props_dir + " ��ο� ���� ������ �����߽��ϴ�", "����!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show(this,
                    "�������� ���� ������ �߻��߽��ϴ�.",
                    "��!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void boost170_delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                string username = Environment.UserName;
                string props_dir = "C:\\Users\\" + username + "\\AppData\\Local\\Microsoft\\MSBuild\\v4.0\\";
                string props_parent_dir = "C:\\Users\\" + username + "\\AppData\\Local\\Microsoft\\MSBuild\\";
                string props_32 = props_dir + "Microsoft.Cpp.Win32.user.props";
                string props_64 = props_dir + "Microsoft.Cpp.x64.user.props";

                if (!(File.Exists(props_32)) && !(File.Exists(props_64)))
                {
                    MessageBox.Show(this,
                    "���� ������ �������� �ʽ��ϴ�.",
                    "��..",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show(this, "���� ������ ���� �����Ͻðڽ��ϱ�?\r\n����?",
                    "����?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // �����ϴ� �κ�
                    if (File.Exists(props_32))
                    {
                        File.Delete(props_32);
                    }
                    if (File.Exists(props_64))
                    {
                        File.Delete(props_64);
                    }
                    if (Directory.GetFiles(props_dir).Length == 0)
                    {
                        Directory.Delete(props_dir);
                    }
                    if (Directory.GetFiles(props_parent_dir).Length == 0)
                    {
                        Directory.Delete(props_parent_dir);
                    }
                    MessageBox.Show(this, "���� ������ �����߽��ϴ�.", "����!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "���� ������ �������� �ʾҽ��ϴ�.", "����!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch
            {
                MessageBox.Show(this,
                    "�������� ���� ������ �߻��߽��ϴ�.",
                    "��!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void boost170_detail_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentSubForm != null && !currentSubForm.IsDisposed)
                {
                    currentSubForm.Close();
                }
                boost170_subForm = new Win_Boost170Detail();
                int x = this.Location.X;
                int y = this.Location.Y;
                boost170_subForm.Location = new Point(x + 605, y);
                boost170_subForm.Show();
                currentSubForm = boost170_subForm;
            }
            catch
            {
                MessageBox.Show(this,
                    "�������� ���� ������ �߻��߽��ϴ�.",
                    "��!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void boost166_apply_button_Click(object sender, EventArgs e)
        {
            try
            {
                string include_path = boost166_include_path.Text.Trim();
                string lib32_path = boost166_32lib_path.Text.Trim();
                string vs2008_install_dir = vs2008_installdir.Text.Trim();

                // ��� �˻� include_path
                if (include_path.Length < 2)
                {
                    MessageBox.Show(this,
                        "Boost 1.66 ��� \"" + include_path + "\" �� ���������� �ʽ��ϴ�.\r\n�ʹ� ª���ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (include_path[1] != ':')
                {
                    MessageBox.Show(this,
                        "Boost 1.66 ��� \"" + include_path + "\" �� ���������� �ʽ��ϴ�.\r\nC:, D: ������ �����ؾ� �մϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (include_path[include_path.Length - 1] != '\\')
                {
                    include_path += "\\";
                }
                if (!(Directory.Exists(include_path)))
                {
                    MessageBox.Show(this,
                        "Boost 1.66 ��� \"" + include_path + "\" �� �������� �ʽ��ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (!(Directory.Exists(include_path + "boost\\")))
                {
                    MessageBox.Show(this,
                        "Boost 1.66 ��� \"" + include_path + "\"  �ȿ� boost ���丮�� �����ϴ�.\r\nBoost ��ΰ� �ƴ� �� �����ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // ��� �˻� lib32_path
                if (lib32_path.Length < 2)
                {
                    MessageBox.Show(this,
                        "Boost 1.66 32bit Library ��� \"" + lib32_path + "\" �� ���������� �ʽ��ϴ�.\r\n�ʹ� ª���ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (lib32_path[1] != ':')
                {
                    MessageBox.Show(this,
                        "Boost 1.66 32bit Library ��� \"" + lib32_path + "\" �� ���������� �ʽ��ϴ�.\r\nC:, D: ������ �����ؾ� �մϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (lib32_path[lib32_path.Length - 1] != '\\')
                {
                    lib32_path += "\\";
                }
                if (!(Directory.Exists(lib32_path)))
                {
                    MessageBox.Show(this,
                        "Boost 1.66 32bit Library ��� \"" + lib32_path + "\" �� �������� �ʽ��ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                string[] libfiles = Directory.GetFiles(lib32_path);
                bool libexist = false;
                for (int i = 0; i < libfiles.Length; i++)
                {
                    if (libfiles[i].EndsWith(".lib", StringComparison.OrdinalIgnoreCase))
                    {
                        libexist = true;
                        break;
                    }
                }
                if (libexist == false)
                {
                    MessageBox.Show(this,
                        "Boost 1.66 32bit Library ��� \"" + lib32_path + "\"  �ȿ� boost lib ������ ���� �� �����ϴ�.\r\nBoost Library��ΰ� �ƴ� �� �����ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // ��� �˻� - vs2008_install_dir
                if (vs2008_install_dir.Length < 2)
                {
                    MessageBox.Show(this,
                        "Visual Studio 2008 ��ġ ��� \"" + vs2008_install_dir + "\" �� ���������� �ʽ��ϴ�.\r\n�ʹ� ª���ϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (vs2008_install_dir[1] != ':')
                {
                    MessageBox.Show(this,
                        "Visual Studil 2008 ��ġ ��� \"" + vs2008_install_dir + "\" �� ���������� �ʽ��ϴ�.\r\nC:, D: ������ �����ؾ� �մϴ�.",
                        "��?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (vs2008_install_dir[vs2008_install_dir.Length - 1] != '\\')
                {
                    vs2008_install_dir += "\\";
                }

                // ������ ���� üũ
                WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                WindowsPrincipal currentPrincipal = new WindowsPrincipal(currentIdentity);

                bool isAdmin = currentPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
                if (vs2008_install_dir.StartsWith("C:\\Program Files", StringComparison.OrdinalIgnoreCase) && !isAdmin)
                {
                    MessageBox.Show(this,
                        "������ ������ �ʿ��մϴ�.\r\n������ �������� �ٽ� ������ �ּ���.",
                        "��..",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // vs2008_install_dir ��θ� �����
                if (!(Directory.Exists(vs2008_install_dir)))
                {
                    Directory.CreateDirectory(vs2008_install_dir);
                }

                // Ȥ�ó� ������ �ִ��� üũ
                string props_32 = vs2008_install_dir + "properties_win32.vsprops";
                string props_64 = vs2008_install_dir + "properties_x64.vsprops";
                if (File.Exists(props_32) || File.Exists(props_64))
                {
                    // ����� ���� Ȯ��
                    if (MessageBox.Show(this, "���� ������ �̹� �����մϴ�.\r\n����ðڽ��ϱ�?",
                        "��..", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        MessageBox.Show(this,
                            "���� ������ �������� �ɰ� �����߽��ϴ�.",
                            "��..",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                }

                string file_contents = @"<?xml version=""1.0"" encoding=""ks_c_5601-1987""?>
<VisualStudioPropertySheet
	ProjectType=""Visual C++""
	Version=""8.00""
	Name=""properties_win32""
	>
	<UserMacro
		Name=""boost_1_66""
		Value=""" + include_path + @"""
	/>
	<UserMacro
		Name=""boost_1_66_lib""
		Value=""" + lib32_path + @"""
	/>
</VisualStudioPropertySheet>";

                string file_contents_64 = @"<?xml version=""1.0"" encoding=""ks_c_5601-1987""?>
<VisualStudioPropertySheet
	ProjectType=""Visual C++""
	Version=""8.00""
	Name=""properties_x64""
	>
	<UserMacro
		Name=""boost_1_66""
		Value=""" + include_path + @"""
	/>
	<UserMacro
		Name=""boost_1_66_lib""
		Value=""" + lib32_path + @"""
	/>
</VisualStudioPropertySheet>";

                using (StreamWriter writer = new StreamWriter(props_32))
                {
                    writer.WriteLine(file_contents);
                }
                using (StreamWriter writer = new StreamWriter(props_64))
                {
                    writer.WriteLine(file_contents_64);
                }

                MessageBox.Show(this, vs2008_install_dir + " ��ο� ���� ������ �����߽��ϴ�", "����!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch
            {
                MessageBox.Show(this,
                    "�������� ���� ������ �߻��߽��ϴ�.",
                    "��!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void boost166_delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                string vs2008_install_dir = vs2008_installdir.Text.Trim();

                // ��� �˻�
                if (vs2008_install_dir.Length < 2)
                {
                    MessageBox.Show(this,
                    "Visual Studio 2008 ��ġ ��� \"" + vs2008_install_dir + "\" �� ���������� �ʽ��ϴ�.\r\n�ʹ� ª���ϴ�.",
                    "��?",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }
                if (vs2008_install_dir[1] != ':')
                {
                    MessageBox.Show(this,
                    "Visual Studil 2008 ��ġ ��� \"" + vs2008_install_dir + "\" �� ���������� �ʽ��ϴ�.\r\nC:, D: ������ �����ؾ� �մϴ�.",
                    "��?",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }
                if (vs2008_install_dir[vs2008_install_dir.Length - 1] != '\\')
                {
                    vs2008_install_dir += "\\";
                }

                // ������ ���� üũ
                WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                WindowsPrincipal currentPrincipal = new WindowsPrincipal(currentIdentity);

                bool isAdmin = currentPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
                if (vs2008_install_dir.StartsWith("C:\\Program Files", StringComparison.OrdinalIgnoreCase) && !isAdmin)
                {
                    MessageBox.Show(this,
                    "������ ������ �ʿ��մϴ�.\r\n������ �������� �ٽ� ������ �ּ���.",
                    "��..",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }

                string props_32 = vs2008_install_dir + "properties_win32.vsprops";
                string props_64 = vs2008_install_dir + "properties_x64.vsprops";

                if (!(File.Exists(props_32)) && !(File.Exists(props_64)))
                {
                    MessageBox.Show(this,
                    "���� ������ �������� �ʽ��ϴ�.",
                    "��..",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show(this, "���� ������ ���� �����Ͻðڽ��ϱ�?\r\n����?",
                    "����?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // �����ϴ� �κ�
                    if (File.Exists(props_32))
                    {
                        File.Delete(props_32);
                    }
                    if (File.Exists(props_64))
                    {
                        File.Delete(props_64);
                    }
                    if (Directory.GetFiles(vs2008_install_dir).Length == 0)
                    {
                        Directory.Delete(vs2008_install_dir);
                    }
                    MessageBox.Show(this, "���� ������ �����߽��ϴ�.", "����!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "���� ������ �������� �ʾҽ��ϴ�.", "����!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
            catch
            {
                MessageBox.Show(this,
                    "�������� ���� ������ �߻��߽��ϴ�.",
                    "��!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void boost166_detail_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentSubForm != null && !currentSubForm.IsDisposed)
                {
                    currentSubForm.Close();
                }
                boost166_subForm = new Win_Boost166Detail();
                int x = this.Location.X;
                int y = this.Location.Y;
                boost166_subForm.Location = new Point(x + 605, y);
                boost166_subForm.Show();
                currentSubForm = boost166_subForm;
            }
            catch
            {
                MessageBox.Show(this,
                    "�������� ���� ������ �߻��߽��ϴ�.",
                    "��!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void subFormResize(object sender, EventArgs e)
        {
            if (currentSubForm == null)
            {
                return;
            }
            if (WindowState == FormWindowState.Minimized)
            {
                currentSubForm.WindowState = FormWindowState.Minimized;
            }
            else
            {
                currentSubForm.WindowState = FormWindowState.Normal;
            }
        }

        private void Win_AquaBuildPresetter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}