from PyQt6.QtCore import QSize
from PyQt6.QtWidgets import QDialog, QApplication
import getpass

from WindowClass.GUI.Boost170DetailWindowGUI import *
class Boost170DetailWindow(QDialog, Ui_dialog):
    def __init__(self, x=None, y=None):
        super().__init__()
        self.setupUi(self)
        self.setFixedSize(QSize(400, 300))
        self.username = getpass.getuser()
        self.obj_path = "C:\\Users\\" + self.username + "\\AppData\\Local\\Microsoft\\MSBuild\\v4.0\\"
        self.label_2.setText(self.obj_path)

        self.quit.clicked.connect(self.hide)

        if x is not None and y is not None:
            self.move(x, y)

        self.show()



if __name__ == "__main__":
    app = QApplication([])
    w = Boost170DetailWindow()
    app.exec()
