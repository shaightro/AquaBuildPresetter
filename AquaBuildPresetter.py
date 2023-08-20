from WindowClass.MainWindowClass import *
import sys

if __name__ == "__main__":
    app = QApplication([])
    w = MainWindow()
    sys.exit(app.exec())