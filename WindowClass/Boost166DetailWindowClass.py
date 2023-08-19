from PyQt6.QtCore import QSize
from PyQt6.QtWidgets import QDialog, QApplication

from WindowClass.GUI.Boost166DetailWindowGUI import *
class Boost166DetailWindow(QDialog, Ui_dialog):
    def __init__(self, x=None, y=None):
        super().__init__()
        self.setupUi(self)
        self.setFixedSize(QSize(400, 300))

        self.quit.clicked.connect(self.hide)

        if x is not None and y is not None:
            self.move(x, y)

        self.show()



if __name__ == "__main__":
    app = QApplication([])
    w = Boost166DetailWindow()
    app.exec()
