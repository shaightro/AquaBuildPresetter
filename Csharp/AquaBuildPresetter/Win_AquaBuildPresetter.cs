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
                string ignore_pattern = svn_ignore_pattern.Text.Trim(); // 입력받은 무시 패턴
                bool backup_flag = svn_backup_config.Checked;
                bool backup_done;

                // 입력받은 무시 패턴에 \r\n이 있으면 없애 준다
                ignore_pattern = ignore_pattern.Replace("\r\n", " ");
                // 연속된 공백 하나로 줄임
                string pattern = "\\s+";
                ignore_pattern = Regex.Replace(ignore_pattern, pattern, " ");

                // 입력받은 무시 패턴 분리
                string[] pattern_list = ignore_pattern.Split(new char[] { ' ', '\n', '\r' },
                    StringSplitOptions.RemoveEmptyEntries);

                // SVN 설치 확인(경로 존재 확인)
                string username = Environment.UserName;
                string svn_dir = "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\";
                if (!(Directory.Exists(svn_dir)))
                {
                    svn_dir = "C:\\Documents and Settings\\" + username + "\\ApplicationData\\Subversion\\";
                    if (!(Directory.Exists(svn_dir)))
                    {
                        // 여기까지 오면 경로 자체가 없으므로, SVN 설치가 안 된 것으로 간주
                        MessageBox.Show(this, "SVN 경로가 존재하지 않습니다.\r\nSVN(TortoiseSVN 등)을 먼저 설치해 주세요.",
                            "엥?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string config_file = svn_dir + "config";
                // SVN 경로 내 config 파일 존재 확인
                if (!(File.Exists(config_file)))
                {
                    MessageBox.Show(this, "SVN 경로는 있는데 config 파일이 없습니다.\r\n일반적으로는 일어나지 않는 상황인 것 같습니다.",
                            "엥?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 백업 플래그 = true이면, 백업한다
                if (backup_flag)
                {
                    string backup_config_file = svn_dir + "config_backup_byABP";
                    if (File.Exists(backup_config_file))
                    {
                        if (MessageBox.Show(this, "백업 파일이 이미 존재하는 것 같습니다.\r\n새로 백업하시겠습니까?\r\n이전 백업에 덮어쓰게 됩니다.",
                            "음", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

                // config 파일을 읽고 수정한다
                string new_config = "";
                string new_string = "";
                bool switch_ig_block = false;

                using (StreamReader reader = new StreamReader(config_file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 일단 line에 global-ignores, = 가 포함되어 있는지 확인
                        if (line.Contains("global-ignores") && line.Contains('='))
                        {
                            switch_ig_block = true;
                            // global-ignores가 있으면, 맨 앞에 입력받은 무시 패턴을 추가한다
                            new_string = "global-ignores = " + ignore_pattern + "\r\n";
                        }
                        // global-ignores는 없지만 switch-ig-block이 True인 경우, 블록이 끝날 때까지는 주석 처리를 없앤다
                        // block의 기준은 맨 처음이 #가 딱 하나 혹은 0개인지 따진다. 설마 이걸 바꾼 넘은 없겠지
                        // 그리고 입력받은 무시 패턴에 포함되지 않은 녀석들만 추가해 준다
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
                                switch_ig_block = false;    // 블록 끝!
                                new_string = line + "\r\n";
                            }
                        }
                        else
                        {
                            // 이것도 저것도 아니면 그냥 원래의 line을 돌려준다
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

                string success_msg = "설정을 완료하였습니다.";
                if (backup_done)
                {
                    success_msg += "\r\n백업 파일도 저장되었습니다.";
                }
                else
                {
                    success_msg += "\r\n백업은 하지 않았습니다.";
                }

                MessageBox.Show(this, success_msg, "만세!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show(this,
                    "예상하지 못한 오류가 발생했습니다.",
                    "헉!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void svn_restore_backup_button_Click(object sender, EventArgs e)
        {
            try
            {
                // 백업에서 복원
                string username = Environment.UserName;
                string svn_dir = "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\";
                if (!(Directory.Exists(svn_dir)))
                {
                    svn_dir = "C:\\Documents and Settings\\" + username + "\\ApplicationData\\Subversion\\";
                    if (!(Directory.Exists(svn_dir)))
                    {
                        // 여기까지 오면 경로 자체가 없으므로, SVN 설치가 안 된 것으로 간주
                        MessageBox.Show(this, "SVN 경로가 존재하지 않습니다.\r\nSVN(TortoiseSVN 등)을 먼저 설치해 주세요.",
                            "엥?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string config_file = svn_dir + "config";
                string backup_config_file = svn_dir + "config_backup_byABP";
                // SVN 경로 내 config 파일 존재 확인
                if (!(File.Exists(backup_config_file)))
                {
                    MessageBox.Show(this, "백업 파일이 존재하지 않아 백업할 수 없습니다.\n대신 디폴트에서 백업을 시도해 보세요.",
                            "엥?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // 복원
                File.Copy(backup_config_file, config_file, true);

                MessageBox.Show(this, "백업에서 복원을 완료했습니다.",
                            "만세!", MessageBoxButtons.OK, MessageBoxIcon.Information); ;

            }
            catch
            {
                MessageBox.Show(this,
                    "예상하지 못한 오류가 발생했습니다.",
                    "헉!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void svn_restore_default_button_Click(object sender, EventArgs e)
        {
            // 디폴트에서 복원
            try
            {
                string username = Environment.UserName;
                string svn_dir = "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\";
                if (!(Directory.Exists(svn_dir)))
                {
                    svn_dir = "C:\\Documents and Settings\\" + username + "\\ApplicationData\\Subversion\\";
                    if (!(Directory.Exists(svn_dir)))
                    {
                        // 여기까지 오면 경로 자체가 없으므로, SVN 설치가 안 된 것으로 간주
                        MessageBox.Show(this, "SVN 경로가 존재하지 않습니다.\r\nSVN(TortoiseSVN 등)을 먼저 설치해 주세요.",
                            "엥?", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    "디폴트에서 복원을 완료했습니다.",
                    "만세!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(this,
                    "예상하지 못한 오류가 발생했습니다.",
                    "헉!",
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
                    "예상하지 못한 오류가 발생했습니다.",
                    "헉!",
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

                // 경로 검사 include_path
                if (include_path.Length < 2)
                {
                    MessageBox.Show(this,
                        "Boost 1.70 경로 \"" + include_path + "\" 가 정상적이지 않습니다.\r\n너무 짧습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (include_path[1] != ':')
                {
                    MessageBox.Show(this,
                        "Boost 1.70 경로 \"" + include_path + "\" 가 정상적이지 않습니다.\r\nC:, D: 등으로 시작해야 합니다.",
                        "엥?",
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
                        "Boost 1.70 경로 \"" + include_path + "\" 가 존재하지 않습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (!(Directory.Exists(include_path + "boost\\")))
                {
                    MessageBox.Show(this,
                        "Boost 1.70 경로  \"" + include_path + "\" 안에 boost 디렉토리가 없습니다.\r\nBoost 경로가 아닌 것 같습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // 경로 검사 lib32_path
                if (lib32_path.Length < 2)
                {
                    MessageBox.Show(this,
                        "Boost 1.70 32bit Library 경로 \"" + lib32_path + "\" 가 정상적이지 않습니다.\r\n너무 짧습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (lib32_path[1] != ':')
                {
                    MessageBox.Show(this,
                        "Boost 1.70 32bit Library 경로 \"" + lib32_path + "\" 가 정상적이지 않습니다.\r\nC:, D: 등으로 시작해야 합니다.",
                        "엥?",
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
                        "Boost 1.70 32bit Library 경로 \"" + lib32_path + "\" 가 존재하지 않습니다.",
                        "엥?",
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
                        "Boost 1.70 32bit Library 경로 \"" + lib32_path + "\"  안에 boost lib 파일이 없는 것 같습니다.\r\nBoost Library경로가 아닌 것 같습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // 경로 검사 lib64_path
                if (lib64_path.Length < 2)
                {
                    MessageBox.Show(this,
                        "Boost 1.70 64bit Library 경로 \"" + lib64_path + "\" 가 정상적이지 않습니다.\r\n너무 짧습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (lib64_path[1] != ':')
                {
                    MessageBox.Show(this,
                        "Boost 1.70 64bit Library 경로 \"" + lib64_path + "\" 가 정상적이지 않습니다.\r\nC:, D: 등으로 시작해야 합니다.",
                        "엥?",
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
                        "Boost 1.70 64bit Library 경로 \"" + lib64_path + "\" 가 존재하지 않습니다.",
                        "엥?",
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
                        "Boost 1.70 64bit Library 경로 \"" + lib64_path + "\"  안에 boost lib 파일이 없는 것 같습니다.\r\nBoost Library경로가 아닌 것 같습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // C:\Users\(사용자 id)\AppData\Local\Microsoft\MSBuild\v4.0 을 만든다
                string username = Environment.UserName;
                string props_dir = "C:\\Users\\" + username + "\\AppData\\Local\\Microsoft\\MSBuild\\v4.0\\";

                if (!(Directory.Exists(props_dir)))
                {
                    Directory.CreateDirectory(props_dir);
                }

                // 혹시나 파일이 있는지 체크
                string props_32 = props_dir + "Microsoft.Cpp.Win32.user.props";
                string props_64 = props_dir + "Microsoft.Cpp.x64.user.props";
                if (File.Exists(props_32) || File.Exists(props_64))
                {
                    // 덮어쓰기 여부 확인
                    if (MessageBox.Show(this, "설정 파일이 이미 존재합니다.\r\n덮어쓰시겠습니까?",
                        "음..", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        MessageBox.Show(this,
                            "설정 파일을 저장하지 앉고 종료했습니다.",
                            "흠..",
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

                MessageBox.Show(this, props_dir + " 경로에 설정 파일을 저장했습니다", "만세!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
                MessageBox.Show(this,
                    "예상하지 못한 오류가 발생했습니다.",
                    "헉!",
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
                    "설정 파일이 존재하지 않습니다.",
                    "아..",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show(this, "설정 파일을 정말 삭제하시겠습니까?\r\n굳이?",
                    "정말?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // 삭제하는 부분
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
                    MessageBox.Show(this, "설정 파일을 삭제했습니다.", "만세!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "설정 파일을 삭제하지 않았습니다.", "만세!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch
            {
                MessageBox.Show(this,
                    "예상하지 못한 오류가 발생했습니다.",
                    "헉!",
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
                    "예상하지 못한 오류가 발생했습니다.",
                    "헉!",
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

                // 경로 검사 include_path
                if (include_path.Length < 2)
                {
                    MessageBox.Show(this,
                        "Boost 1.66 경로 \"" + include_path + "\" 가 정상적이지 않습니다.\r\n너무 짧습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (include_path[1] != ':')
                {
                    MessageBox.Show(this,
                        "Boost 1.66 경로 \"" + include_path + "\" 가 정상적이지 않습니다.\r\nC:, D: 등으로 시작해야 합니다.",
                        "엥?",
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
                        "Boost 1.66 경로 \"" + include_path + "\" 가 존재하지 않습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (!(Directory.Exists(include_path + "boost\\")))
                {
                    MessageBox.Show(this,
                        "Boost 1.66 경로 \"" + include_path + "\"  안에 boost 디렉토리가 없습니다.\r\nBoost 경로가 아닌 것 같습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // 경로 검사 lib32_path
                if (lib32_path.Length < 2)
                {
                    MessageBox.Show(this,
                        "Boost 1.66 32bit Library 경로 \"" + lib32_path + "\" 가 정상적이지 않습니다.\r\n너무 짧습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (lib32_path[1] != ':')
                {
                    MessageBox.Show(this,
                        "Boost 1.66 32bit Library 경로 \"" + lib32_path + "\" 가 정상적이지 않습니다.\r\nC:, D: 등으로 시작해야 합니다.",
                        "엥?",
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
                        "Boost 1.66 32bit Library 경로 \"" + lib32_path + "\" 가 존재하지 않습니다.",
                        "엥?",
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
                        "Boost 1.66 32bit Library 경로 \"" + lib32_path + "\"  안에 boost lib 파일이 없는 것 같습니다.\r\nBoost Library경로가 아닌 것 같습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // 경로 검사 - vs2008_install_dir
                if (vs2008_install_dir.Length < 2)
                {
                    MessageBox.Show(this,
                        "Visual Studio 2008 설치 경로 \"" + vs2008_install_dir + "\" 가 정상적이지 않습니다.\r\n너무 짧습니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (vs2008_install_dir[1] != ':')
                {
                    MessageBox.Show(this,
                        "Visual Studil 2008 설치 경로 \"" + vs2008_install_dir + "\" 가 정상적이지 않습니다.\r\nC:, D: 등으로 시작해야 합니다.",
                        "엥?",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (vs2008_install_dir[vs2008_install_dir.Length - 1] != '\\')
                {
                    vs2008_install_dir += "\\";
                }

                // 관리자 권한 체크
                WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                WindowsPrincipal currentPrincipal = new WindowsPrincipal(currentIdentity);

                bool isAdmin = currentPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
                if (vs2008_install_dir.StartsWith("C:\\Program Files", StringComparison.OrdinalIgnoreCase) && !isAdmin)
                {
                    MessageBox.Show(this,
                        "관리자 권한이 필요합니다.\r\n관리자 권한으로 다시 실행해 주세요.",
                        "아..",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // vs2008_install_dir 경로를 만든다
                if (!(Directory.Exists(vs2008_install_dir)))
                {
                    Directory.CreateDirectory(vs2008_install_dir);
                }

                // 혹시나 파일이 있는지 체크
                string props_32 = vs2008_install_dir + "properties_win32.vsprops";
                string props_64 = vs2008_install_dir + "properties_x64.vsprops";
                if (File.Exists(props_32) || File.Exists(props_64))
                {
                    // 덮어쓰기 여부 확인
                    if (MessageBox.Show(this, "설정 파일이 이미 존재합니다.\r\n덮어쓰시겠습니까?",
                        "음..", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        MessageBox.Show(this,
                            "설정 파일을 저장하지 앉고 종료했습니다.",
                            "흠..",
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

                MessageBox.Show(this, vs2008_install_dir + " 경로에 설정 파일을 저장했습니다", "만세!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            catch
            {
                MessageBox.Show(this,
                    "예상하지 못한 오류가 발생했습니다.",
                    "헉!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void boost166_delete_button_Click(object sender, EventArgs e)
        {
            try
            {
                string vs2008_install_dir = vs2008_installdir.Text.Trim();

                // 경로 검사
                if (vs2008_install_dir.Length < 2)
                {
                    MessageBox.Show(this,
                    "Visual Studio 2008 설치 경로 \"" + vs2008_install_dir + "\" 가 정상적이지 않습니다.\r\n너무 짧습니다.",
                    "엥?",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }
                if (vs2008_install_dir[1] != ':')
                {
                    MessageBox.Show(this,
                    "Visual Studil 2008 설치 경로 \"" + vs2008_install_dir + "\" 가 정상적이지 않습니다.\r\nC:, D: 등으로 시작해야 합니다.",
                    "엥?",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }
                if (vs2008_install_dir[vs2008_install_dir.Length - 1] != '\\')
                {
                    vs2008_install_dir += "\\";
                }

                // 관리자 권한 체크
                WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                WindowsPrincipal currentPrincipal = new WindowsPrincipal(currentIdentity);

                bool isAdmin = currentPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
                if (vs2008_install_dir.StartsWith("C:\\Program Files", StringComparison.OrdinalIgnoreCase) && !isAdmin)
                {
                    MessageBox.Show(this,
                    "관리자 권한이 필요합니다.\r\n관리자 권한으로 다시 실행해 주세요.",
                    "아..",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }

                string props_32 = vs2008_install_dir + "properties_win32.vsprops";
                string props_64 = vs2008_install_dir + "properties_x64.vsprops";

                if (!(File.Exists(props_32)) && !(File.Exists(props_64)))
                {
                    MessageBox.Show(this,
                    "설정 파일이 존재하지 않습니다.",
                    "아..",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show(this, "설정 파일을 정말 삭제하시겠습니까?\r\n굳이?",
                    "정말?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // 삭제하는 부분
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
                    MessageBox.Show(this, "설정 파일을 삭제했습니다.", "만세!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "설정 파일을 삭제하지 않았습니다.", "만세!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
            catch
            {
                MessageBox.Show(this,
                    "예상하지 못한 오류가 발생했습니다.",
                    "헉!",
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
                    "예상하지 못한 오류가 발생했습니다.",
                    "헉!",
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