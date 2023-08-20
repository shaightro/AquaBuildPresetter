from PyQt6.QtCore import QSize
from PyQt6.QtWidgets import QDialog, QApplication

from WindowClass.GUI.Boost166DetailWindowGUI import *


class Boost166DetailWindow(QDialog, Ui_boost166dialog):
    def __init__(self, parent=None, x=None, y=None):
        super().__init__(parent)
        self.setupUi(self)
        self.setFixedSize(QSize(400, 300))

        self.quit.clicked.connect(self.hide)

        if x is not None and y is not None:
            self.move(x, y)

        self.show()




if __name__ == "__main__":
    app = QApplication([])
    w = Boost166DetailWindow(None)
    app.exec()
