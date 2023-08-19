from PyQt6.QtCore import QSize
from PyQt6.QtWidgets import QDialog, QApplication, QMessageBox
import getpass
import os

from WindowClass.GUI.SvnDetailWindowGUI import *
class SvnDetailWindow(QDialog, Ui_dialog):
    def __init__(self, x=None, y=None):
        super().__init__()
        self.setupUi(self)
        self.setFixedSize(QSize(400, 300))
        self.username = getpass.getuser()
        self.obj_path = "C:\\Users\\" + self.username + "\\AppData\\Roaming\\Subversion\\"
        self.label_2.setText(self.obj_path + " 경로의")

        self.quit.clicked.connect(self.hide)
        self.open_obj_dir.clicked.connect(self.open_dir)

        if x is not None and y is not None:
            self.move(x, y)


        self.show()

    # 버튼 기능 설정
    def open_dir(self):
        dir_to_open = os.path.realpath(self.obj_path)
        if os.path.isdir(self.obj_path):
            os.startfile(dir_to_open)
        else:
            button = QMessageBox.critical(self,
                                          "어라?",
                                          "해당 경로가 존재하지 않아 열 수 없습니다.\nSVN(TortosieSVN 등)을 설치하고 사용해 주세요.",
                                          buttons=QMessageBox.StandardButton.Ok)

            if button == QMessageBox.StandardButton.Yes:
                a = 1


if __name__ == "__main__":
    app = QApplication([])
    w = SvnDetailWindow()
    app.exec()
