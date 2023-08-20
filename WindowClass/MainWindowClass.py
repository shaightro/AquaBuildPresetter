from PyQt6.QtCore import QCoreApplication
from PyQt6.QtWidgets import QMainWindow

from WindowClass.GUI import MainWindowGUI
from WindowClass.SvnDetailWindowClass import *
from WindowClass.Boost170DetailWindowClass import *
from WindowClass.Boost166DetailWindowClass import *

from datetime import datetime
import shutil

import svn_ignore_default
import ctypes



class MainWindow(QMainWindow, MainWindowGUI.Ui_MainWindow):
    def __init__(self):
        super().__init__()
        self.setupUi(self)
        self.setFixedSize(QSize(543, 263))

        self.svn_ignore_detail_button.clicked.connect(self.svn_detail_button_clicked)
        self.svn_ignore_apply_button.clicked.connect(self.svn_apply_button_clicked)
        self.svn_ignore_restore_button.clicked.connect(self.svn_restore_button_clicked)
        self.svn_ignore_restore_button_2.clicked.connect(self.svn_restore_button_2_clicked)

        self.boost_170_detail_button.clicked.connect(self.boost_170_detail_button_clicked)
        self.boost_170_apply_button.clicked.connect(self.boost_170_apply_button_clicked)
        self.boost_170_delete_button.clicked.connect(self.boost_170_delete_button_clicked)

        self.boost_166_detail_button.clicked.connect(self.boost_166_detail_button_clicked)
        self.boost_166_apply_button.clicked.connect(self.boost_166_apply_button_clicked)
        self.boost_166_delete_button.clicked.connect(self.boost_166_delete_button_clicked)

        self.sub_w = None

        self.show()

    def closeEvent(self, event):
        QCoreApplication.instance().quit()
        event.accept()

    def get_geometry(self):
        widget = self.geometry()
        self.window_x = widget.x()
        self.window_y = widget.y()
        self.window_width = widget.width()
        self.window_height = widget.height()
        #print(self.window_x, self.window_y, self.window_width, self.window_height)
        return self.window_x, self.window_y, self.window_width, self.window_height

    def subwindow_location(self):
        x, y, width, height = self.get_geometry()
        return x+width+15, y-30

    # 1. SVN 무시 패턴 설정 버튼
    def svn_detail_button_clicked(self):
        try:
            if self.sub_w is not None:
                self.sub_w.close()
                self.sub_w = None
            x, y = self.subwindow_location()
            self.sub_w = SvnDetailWindow(parent=self, x=x, y=y)
        except:
            QMessageBox.critical(self, "헉!", "예상하지 못한 오류가 발생했습니다.")


    def svn_apply_button_clicked(self):
        try:
            ignore_pattern = self.svn_ignore_list.toPlainText()        # 입력받은 무시 패턴
            backup_flag = self.svn_config_backup.isChecked()        # 입력받은 백업 여부
            backup_done = False

            # 입력받은 무시 패턴에 "\n"이 있으면 없애 준다
            ignore_pattern = ignore_pattern.replace("\n", " ")

            # 입력받은 무시 패턴 분리
            pattern_list = ignore_pattern.split(" ")

            # SVN 설치 확인(경로 존재 확인)
            username = getpass.getuser()
            svn_dir = "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\"
            if not os.path.isdir(svn_dir):
                svn_dir = "C:\\Documents and Settings\\" + username + "\\Application Data\\Subversion\\"
                if not os.path.isdir(svn_dir):
                    # 여기까지 오면 경로 자체가 없으므로, svn 설치가 안 된 것으로 간주
                    QMessageBox.critical(
                        self, "엥?", "SVN 경로가 존재하지 않습니다. \nSVN(TortoiseSVN 등)을 먼저 설치해 주세요."
                    )
                    return

            config_file = svn_dir + "config"
            # SVN 경로 내 config 파일 존재 확인 (없을 리가 없음)
            if not os.path.isfile(config_file):
                QMessageBox.critical(
                    self, "엥?", "SVN 경로는 있는데 config 파일이 없습니다.\n일반적으로는 일어나지 않는 상황인 것 같습니다."
                )
                return

            # 백업 플래그 = true이면, 백업한다
            now_time = datetime.now().strftime('%Y-%m-%d_%H-%M-%S')
            if backup_flag:
                backup_config_file = svn_dir + "config_backup_byABP"
                if os.path.isfile(backup_config_file):
                    button = QMessageBox.question(
                        self, "음..", "백업 파일이 이미 존재하는 것 같습니다.\n새로 백업하시겠습니까?\n이전 백업에 덮어쓰게 됩니다."
                    )
                    if button == QMessageBox.StandardButton.Yes:
                        shutil.copyfile(config_file, backup_config_file)
                        backup_done = True
                    else:
                        backup_done = False
                else:
                    shutil.copyfile(config_file, backup_config_file)
                    backup_done = True
            else:
                backup_done = False

            # Config 파일을 읽고 수정한다
            new_config = ""
            new_string = ""
            switch_ig_block = False
            with open(config_file, 'r', encoding='utf-8') as f:
                lines = f.readlines()
                for i, l in enumerate(lines):
                    # 일단 l에 global-ignores, = 가 포함되어 있는지 확인
                    now_index = l.find("global-ignores")
                    if now_index >= 0 and l.find("=") >= 0:
                        switch_ig_block = True
                        # global-ignores가 있으면, 맨 앞에 입력받은 무시 패턴을 추가한다
                        new_string = "global-ignores = " + ignore_pattern + "\n"

                    # global-ignores는 없지만 switch_ig_block 이 True인 경우, 블록이 끝날 때까지는  주석 처리를 없앤다
                    # block의 기준은 맨 처음이 #가 딱 하나 혹은 0개인지 따진다. 설마 이걸 바꾼 넘은 없겠지
                    # 그리고 입력받은 무시 패턴에 포함되지 않은 녀석들만 추가해 준다
                    elif switch_ig_block is True:
                        if (l.startswith("#") and not l.startswith("###")) or l.startswith(" "):
                            l = l.replace("\n", "")
                            now_list = l[1:].split(" ")
                            if now_list:
                                now_list = list(set(now_list) - set(pattern_list))
                                if now_list:
                                    new_string = "    "
                                    for item in now_list:
                                        new_string += item + " "
                                    new_string = new_string[:-1] + "\n"
                                    if new_string.replace("\n", "").strip() == "":
                                        new_string = ""
                                else:
                                    new_string = ""
                            else:
                                new_string = ""
                        else:
                            switch_ig_block = False  # 블록 끝!
                            new_string = l

                    # 이것도 저것도 아니면 그냥 원래의 l을 돌려준다
                    else:
                        new_string = l

                    if new_string:
                        new_config += new_string

            with open(config_file, 'w', encoding='utf-8') as f:
                f.write(new_config)

            success_msg = "설정을 완료하였습니다."
            if backup_done:
                success_msg += "\n백업 파일도 저장되었습니다."
            else:
                success_msg += "\n백업은 하지 않았습니다."
            QMessageBox.information(
                self, "만세!", success_msg
            )
        except:
            QMessageBox.critical(self, "헉!", "예상하지 못한 오류가 발생했습니다.")

    def svn_restore_button_clicked(self):
        try:
            # 백업에서 복원
            # SVN 설치 확인(경로 존재 확인)
            username = getpass.getuser()
            svn_dir = "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\"
            if not os.path.isdir(svn_dir):
                svn_dir = "C:\\Documents and Settings\\" + username + "\\Application Data\\Subversion\\"
                if not os.path.isdir(svn_dir):
                    # 여기까지 오면 경로 자체가 없으므로, svn 설치가 안 된 것으로 간주
                    QMessageBox.critical(
                        self, "엥?", "SVN 경로가 존재하지 않습니다. \nSVN(TortoiseSVN 등)을 먼저 설치해 주세요."
                    )
                    return

            config_file = svn_dir + "config"
            backup_config_file = svn_dir + "config_backup_byABP"
            # SVN 경로 내 config 파일 존재 확인 (없을 리가 없음)
            if not os.path.isfile(backup_config_file):
                QMessageBox.critical(
                    self, "엥?", "백업 파일이 존재하지 않아 백업할 수 없습니다.\n대신 디폴트에서 백업을 시도해 보세요."
                )
                return

            # 복원
            shutil.copyfile(backup_config_file, config_file)

            QMessageBox.information(
                self, "만세!", "백업에서 복원을 완료했습니다."
            )
        except:
            QMessageBox.critical(self, "헉!", "예상하지 못한 오류가 발생했습니다.")

    def svn_restore_button_2_clicked(self):
        try:
            # 디폴트에서 복원
            # SVN 설치 확인(경로 존재 확인)
            username = getpass.getuser()
            svn_dir = "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\"
            if not os.path.isdir(svn_dir):
                svn_dir = "C:\\Documents and Settings\\" + username + "\\Application Data\\Subversion\\"
                if not os.path.isdir(svn_dir):
                    # 여기까지 오면 경로 자체가 없으므로, svn 설치가 안 된 것으로 간주
                    QMessageBox.critical(
                        self, "엥?", "SVN 경로가 존재하지 않습니다. \nSVN(TortoiseSVN 등)을 먼저 설치해 주세요."
                    )
                    return

            config_file = svn_dir + "config"

            with open(config_file, 'w', encoding='utf-8') as f:
                f.write(svn_ignore_default.svn_ignore_default)


            QMessageBox.information(
                self, "만세!", "디폴트에서 복원을 완료했습니다."
            )
        except:
            QMessageBox.critical(self, "헉!", "예상하지 못한 오류가 발생했습니다.")

    # 2. Boost 경로 매크로 설정(VS2019) 버튼
    def boost_170_detail_button_clicked(self):
        try:
            if self.sub_w is not None:
                self.sub_w.close()
                self.sub_w = None
            x, y = self.subwindow_location()
            self.sub_w = Boost170DetailWindow(parent=self, x=x, y=y)
        except:
            QMessageBox.critical(self, "헉!", "예상하지 못한 오류가 발생했습니다.")

    def boost_170_apply_button_clicked(self):
        try:
            # 경로 입력칸에 적힌 내용들을 읽는다
            boost_170_dir = self.boost_170_dir.text().strip()
            boost_170_lib_32 = self.boost_170_lib_32.text().strip()
            boost_170_lib_64 = self.boost_170_lib_64.text().strip()

            # 유효한 경로인지 확인한다.
            # 먼저 실제 경로가 맞긴 한지.. 일단 정상적인 절대경로라면 길이는 2 이상에 1번째가 :이어야 한다
            if len(boost_170_dir) < 2:
                QMessageBox.critical(self, "엥?", "Boost 1.70 경로가 정상적이지 않습니다.\n너무 짧습니다.")
                return
            if not boost_170_dir[1] == ":":
                QMessageBox.critical(self, "엥?", "Boost 1.70 경로가 정상적이지 않습니다.\nC:, D: 등으로 시작해야 합니다.")
                return
            if not boost_170_dir[-1] == "\\":
                boost_170_dir += "\\"
            if not os.path.isdir(boost_170_dir):
                QMessageBox.critical(self, "엥?", "Boost 1.70 경로가 존재하지 않습니다.")
                return
            if not os.path.isdir(boost_170_dir + "boost\\"):
                QMessageBox.critical(self, "엥?", "Boost 1.70 경로 안에 boost 디렉토리가 없습니다.")
                return

            if len(boost_170_lib_32) < 2:
                QMessageBox.critical(self, "엥?", "Boost 1.70 32bit Library 경로가 정상적이지 않습니다.\n너무 짧습니다.")
                return
            if not boost_170_lib_32[1] == ":":
                QMessageBox.critical(self, "엥?", "Boost 1.70 32bit Library 경로가 정상적이지 않습니다.\nC:, D: 등으로 시작해야 합니다.")
                return
            if not boost_170_lib_32[-1] == "\\":
                boost_170_lib_32 += "\\"
            if not os.path.isdir(boost_170_lib_32):
                QMessageBox.critical(self, "엥?", "Boost 1.70 32bit Library 경로가 존재하지 않습니다.")
                return
            file_list = [file for file in os.listdir(boost_170_lib_32)
                         if file.startswith("libboost_") and file.endswith(".lib")]
            if len(file_list) <= 0:
                QMessageBox.critical(self, "엥?", "Boost 1.70 32bit Library 경로 안에 boost lib 파일이 없는 것 같습니다.\n")
                return

            if len(boost_170_lib_64) < 2:
                QMessageBox.critical(self, "엥?", "Boost 1.70 64bit Library 경로가 정상적이지 않습니다.\n너무 짧습니다.")
                return
            if not boost_170_lib_64[1] == ":":
                QMessageBox.critical(self, "엥?", "Boost 1.70 64bit Library 경로가 정상적이지 않습니다.\nC:, D: 등으로 시작해야 합니다.")
                return
            if not boost_170_lib_64[-1] == "\\":
                boost_170_lib_64 += "\\"
            if not os.path.isdir(boost_170_lib_64):
                QMessageBox.critical(self, "엥?", "Boost 1.70 64bit Library 경로가 존재하지 않습니다.")
                return
            file_list = [file for file in os.listdir(boost_170_lib_64)
                         if file.startswith("libboost_") and file.endswith(".lib")]
            if len(file_list) <= 0:
                QMessageBox.critical(self, "엥?", "Boost 1.70 64bit Library 경로 안에 boost lib 파일이 없는 것 같습니다.\n")
                return

            # C:\Users(사용자 id)\AppData\Local\Microsoft\MSBuild\v4.0 를 만든다
            username = getpass.getuser()
            props_dir = 'C:\\Users\\' + username + '\\AppData\\Local\\Microsoft\\MSBuild\\v4.0\\'
            if not os.path.isdir(props_dir):
                os.makedirs(props_dir)

            # 혹시나 파일이 있는지 체크
            props_32 = props_dir + 'Microsoft.Cpp.Win32.user.props'
            props_64 = props_dir + 'Microsoft.Cpp.x64.user.props'
            if os.path.isfile(props_32) or os.path.isfile(props_64):
                # 덮어쓸 지 말 지 확인
                button = QMessageBox.question(self, "음..", "설정 파일이 이미 존재합니다.\n덮어쓰시겠습니까?")
                if not button == QMessageBox.StandardButton.Yes:
                    QMessageBox.critical(self, "흠..", "설정 파일을 저장하지 앉고 종료했습니다.")
                    return

            with open(props_32, 'w', encoding='utf-8') as f:
                f.write('''<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <ImportGroup Label="PropertySheets" />
  <PropertyGroup Label="UserMacros">
    <boost_1_70>''' + boost_170_dir + '''</boost_1_70>
    <boost_1_70_lib>''' + boost_170_lib_32 + '''</boost_1_70_lib>
  </PropertyGroup>
  <PropertyGroup />
  <ItemDefinitionGroup />
  <ItemGroup>
    <BuildMacro Include="boost_1_70">
      <Value>$(boost_1_70)</Value>
    </BuildMacro>
    <BuildMacro Include="boost_1_70_lib">
      <Value>$(boost_1_70_lib)</Value>
    </BuildMacro>
  </ItemGroup>
</Project>''')

            with open(props_64, 'w', encoding='utf-8') as f:
                f.write('''<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <ImportGroup Label="PropertySheets" />
  <PropertyGroup Label="UserMacros">
    <boost_1_70>''' + boost_170_dir + '''</boost_1_70>
    <boost_1_70_lib>''' + boost_170_lib_64 + '''</boost_1_70_lib>
  </PropertyGroup>
  <PropertyGroup />
  <ItemDefinitionGroup />
  <ItemGroup>
    <BuildMacro Include="boost_1_70">
      <Value>$(boost_1_70)</Value>
    </BuildMacro>
    <BuildMacro Include="boost_1_70_lib">
      <Value>$(boost_1_70_lib)</Value>
    </BuildMacro>
  </ItemGroup>
</Project>''')

            QMessageBox.information(self, "만세!", props_dir + " 경로에 설정 파일을 저장했습니다.")
        except:
            QMessageBox.critical(self, "헉!", "예상하지 못한 오류가 발생했습니다.")

    def boost_170_delete_button_clicked(self):
        try:
            username = getpass.getuser()
            props_dir = 'C:\\Users\\' + username + '\\AppData\\Local\\Microsoft\\MSBuild\\v4.0\\'
            props_parent_dir = 'C:\\Users\\' + username + '\\AppData\\Local\\Microsoft\\MSBuild\\'
            props_32 = props_dir + 'Microsoft.Cpp.Win32.user.props'
            props_64 = props_dir + 'Microsoft.Cpp.x64.user.props'

            if not os.path.isfile(props_32) and not os.path.isfile(props_64):
                QMessageBox.information(self, "응?", "설정 파일이 존재하지 않습니다.")
                return

            button = QMessageBox.question(self, "정말?", "설정 파일을 정말 삭제하시겠습니까?\n굳이?")
            if not button == QMessageBox.StandardButton.Yes:
                QMessageBox.information(self, "만세!", "설정 파일을 삭제하지 않았습니다.")
                return

            if os.path.isfile(props_32):
                os.remove(props_32)
            if os.path.isfile(props_64):
                os.remove(props_64)
            if not os.listdir(props_dir):
                os.rmdir(props_dir)
            if not os.listdir(props_parent_dir):
                os.rmdir(props_parent_dir)
            QMessageBox.information(self, "만세!", "설정 파일을 삭제했습니다.")
        except:
            QMessageBox.critical(self, "헉!", "예상하지 못한 오류가 발생했습니다.")

    # 3. Boost 경로 매크로 설정(VS2008) 버튼
    def boost_166_detail_button_clicked(self):
        try:
            if self.sub_w is not None:
                self.sub_w.close()
                self.sub_w = None
            x, y = self.subwindow_location()
            self.sub_w = Boost166DetailWindow(parent=self, x=x, y=y)
        except:
            QMessageBox.critical(self, "헉!", "예상하지 못한 오류가 발생했습니다.")

    def boost_166_apply_button_clicked(self):
        try:
            # 경로 입력칸에 적힌 내용들을 읽는다
            boost_166_dir = self.boost_166_dir.text().strip()
            boost_166_lib_32 = self.boost_166_lib_32.text().strip()
            vs2008_install_dir = self.vs2008_install_dir.text().strip()

            # 유효한 경로인지 확인한다.
            # 먼저 실제 경로가 맞긴 한지.. 일단 정상적인 절대경로라면 길이는 2 이상에 1번째가 :이어야 한다
            if len(boost_166_dir) < 2:
                QMessageBox.critical(self, "엥?", "Boost 1.66 경로가 정상적이지 않습니다.\n너무 짧습니다.")
                return
            if not boost_166_dir[1] == ":":
                QMessageBox.critical(self, "엥?", "Boost 1.66 경로가 정상적이지 않습니다.\nC:, D: 등으로 시작해야 합니다.")
                return
            if not boost_166_dir[-1] == "\\":
                boost_166_dir += "\\"
            if not os.path.isdir(boost_166_dir):
                QMessageBox.critical(self, "엥?", "Boost 1.66 경로가 존재하지 않습니다.")
                return
            if not os.path.isdir(boost_166_dir + "boost\\"):
                QMessageBox.critical(self, "엥?", "Boost 1.66 경로 안에 boost 디렉토리가 없습니다.")
                return

            if len(boost_166_lib_32) < 2:
                QMessageBox.critical(self, "엥?", "Boost 1.66 32bit Library 경로가 정상적이지 않습니다.\n너무 짧습니다.")
                return
            if not boost_166_lib_32[1] == ":":
                QMessageBox.critical(self, "엥?", "Boost 1.66 32bit Library 경로가 정상적이지 않습니다.\nC:, D: 등으로 시작해야 합니다.")
                return
            if not boost_166_lib_32[-1] == "\\":
                boost_166_lib_32 += "\\"
            if not os.path.isdir(boost_166_lib_32):
                QMessageBox.critical(self, "엥?", "Boost 1.66 32bit Library 경로가 존재하지 않습니다.")
                return
            file_list = [file for file in os.listdir(boost_166_lib_32)
                         if file.startswith("libboost_") and file.endswith(".lib")]
            if len(file_list) <= 0:
                QMessageBox.critical(self, "엥?", "Boost 1.66 32bit Library 경로 안에 boost lib 파일이 없는 것 같습니다.\n")
                return

            if len(vs2008_install_dir) < 2:
                QMessageBox.critical(self, "엥?", "Visual Studio 2008 설치 경로가 정상적이지 않습니다.\n너무 짧습니다.")
                return
            if not vs2008_install_dir[1] == ":":
                QMessageBox.critical(self, "엥?", "Visual Studil 2008 설치 경로가 정상적이지 않습니다.\nC:, D: 등으로 시작해야 합니다.")
                return
            if not vs2008_install_dir[-1] == "\\":
                vs2008_install_dir += "\\"
            # 관리자 권한 체크
            if vs2008_install_dir.startswith("C:\\Program Files") and not ctypes.windll.shell32.IsUserAnAdmin():
                QMessageBox.critical(self, "아..", "관리자 권한이 필요합니다.\n관리자 권한으로 다시 실행해 주세요.")
                return

            # vs2008_install_dir 경로를 만든다
            if not os.path.isdir(vs2008_install_dir):
                os.makedirs(vs2008_install_dir)

            # 혹시나 파일이 있는지 체크
            props_32 = vs2008_install_dir + 'properties_win32.props'
            if os.path.isfile(props_32):
                # 덮어쓸 지 말 지 확인
                button = QMessageBox.question(self, "음..", "설정 파일이 이미 존재합니다.\n덮어쓰시겠습니까?")
                if not button == QMessageBox.StandardButton.Yes:
                    QMessageBox.critical(self, "흠..", "설정 파일을 저장하지 앉고 종료했습니다.")
                    return

            with open(props_32, 'w', encoding='utf-8') as f:
                f.write('''<?xml version="1.0" encoding="ks_c_5601-1987"?>
<VisualStudioPropertySheet
	ProjectType="Visual C++"
	Version="8.00"
	Name="properties_win32"
	>
	<UserMacro
		Name="boost_1_66"
		Value="''' + boost_166_dir + '''"
	/>
	<UserMacro
		Name="boost_1_66_lib"
		Value="''' + boost_166_lib_32 + '''"
	/>
</VisualStudioPropertySheet>''')

            QMessageBox.information(self, "만세!", vs2008_install_dir + " 경로에 설정 파일을 저장했습니다.")
        except:
            QMessageBox.critical(self, "헉!", "예상하지 못한 오류가 발생했습니다.")

    def boost_166_delete_button_clicked(self):
        try:
            vs2008_install_dir = self.vs2008_install_dir.text().strip()

            if len(vs2008_install_dir) < 2:
                QMessageBox.critical(self, "엥?", "Visual Studio 2008 설치 경로가 정상적이지 않습니다.\n너무 짧습니다.")
                return
            if not vs2008_install_dir[1] == ":":
                QMessageBox.critical(self, "엥?", "Visual Studil 2008 설치 경로가 정상적이지 않습니다.\nC:, D: 등으로 시작해야 합니다.")
                return
            if not vs2008_install_dir[-1] == "\\":
                vs2008_install_dir += "\\"
            # 관리자 권한 체크
            if vs2008_install_dir.startswith("C:\\Program Files") and not ctypes.windll.shell32.IsUserAnAdmin():
                QMessageBox.critical(self, "아..", "관리자 권한이 필요합니다.\n관리자 권한으로 다시 실행해 주세요.")
                return

            props_32 = vs2008_install_dir + 'properties_win32.props'

            if not os.path.isfile(props_32):
                QMessageBox.information(self, "응?", "설정 파일이 존재하지 않습니다.")
                return

            button = QMessageBox.question(self, "정말?", "설정 파일을 정말 삭제하시겠습니까?\n굳이?")
            if not button == QMessageBox.StandardButton.Yes:
                QMessageBox.information(self, "만세!", "설정 파일을 삭제하지 않았습니다.")
                return

            if os.path.isfile(props_32):
                os.remove(props_32)
            if not os.listdir(vs2008_install_dir):
                os.rmdir(vs2008_install_dir)
            QMessageBox.information(self, "만세!", "설정 파일을 삭제했습니다.")
        except:
            QMessageBox.critical(self, "헉!", "예상하지 못한 오류가 발생했습니다.")


if __name__ == "__main__":
    app = QApplication([])
    w = MainWindow()
    app.exec()