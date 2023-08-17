import ctypes
import getpass
import os


def svn_dir_checker():
    username = getpass.getuser()
    svn_dir_1 = "C:\\Users\\" + username + "\\AppData\\Roaming\\test\\"
    # svn_dir_1 = "C:\\Users\\" + username + "\\AppData\\Roaming\\Subversion\\"
    svn_dir_2 = "C:\\Documents and Settings\\" + username + "\\Application Data\\Subversion\\"
    if os.path.isdir(svn_dir_1):
        return True, svn_dir_1
    elif os.path.isdir(svn_dir_2):
        return True, svn_dir_2
    else:
        return False, ""


def file_checker(file_name):
    return os.path.isfile(file_name)


if __name__ == "__main__":
    # 입력받은 무시 패턴
    ignore_pattern = "*.o *.lo *.la *.al .libs *.so *.so.[0-9]* *.a *.pyc *.pyo __pycache__ *.rej *~ #*# .#* .*.swp .DS_Store Debug Release Debug-Static Release-Static Debug-Excel Release-Excel DebugPS ReleasePS lib Win32 x64 *.user build .vs *_i.c *_i.h *_p.c *.ncb *.suo *.sdf ipch *.aps"

    # 입력받은 무시 패턴에 "\n"이 있으면 없애 준다
    ignore_pattern = ignore_pattern.replace("\n", " ")

    # 입력받은 무시 패턴 분리
    pattern_list = ignore_pattern.split(" ")

    # 관리자 권한 체크
    if not ctypes.windll.shell32.IsUserAnAdmin():
        print('관리자 권한으로 실행 필요')
        #exit()

    # SVN 설치 확인(경로 존재 확인)
    res, svn_dir = svn_dir_checker()
    if res is False:
        print("svn부터 설치 필요")
        exit()

    config_file = svn_dir + "config"
    # SVN 경로 내 config 파일 존재 확인 (없을 리가 없음)
    if not file_checker(config_file):
        print("config 파일이 없음")
        exit()

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
                    switch_ig_block = False     # 블록 끝!
                    new_string = l

            # 이것도 저것도 아니면 그냥 원래의 l을 돌려준다
            else:
                new_string = l

            if new_string:
                new_config += new_string

    with open(config_file+"result", 'w', encoding='utf-8') as f:
        f.write(new_config)
