# Form implementation generated from reading ui file 'svn_ignore_detail_window.ui'
#
# Created by: PyQt6 UI code generator 6.4.2
#
# WARNING: Any manual changes made to this file will be lost when pyuic6 is
# run again.  Do not edit this file unless you know what you are doing.


from PyQt6 import QtCore, QtGui, QtWidgets


class Ui_dialog(object):
    def setupUi(self, dialog):
        dialog.setObjectName("dialog")
        dialog.setWindowModality(QtCore.Qt.WindowModality.NonModal)
        dialog.resize(400, 300)
        self.label = QtWidgets.QLabel(parent=dialog)
        self.label.setGeometry(QtCore.QRect(10, 10, 381, 16))
        self.label.setObjectName("label")
        self.label_2 = QtWidgets.QLabel(parent=dialog)
        self.label_2.setGeometry(QtCore.QRect(10, 30, 381, 16))
        self.label_2.setObjectName("label_2")
        self.label_3 = QtWidgets.QLabel(parent=dialog)
        self.label_3.setGeometry(QtCore.QRect(10, 50, 381, 16))
        self.label_3.setObjectName("label_3")
        self.label_4 = QtWidgets.QLabel(parent=dialog)
        self.label_4.setGeometry(QtCore.QRect(10, 80, 381, 16))
        self.label_4.setObjectName("label_4")
        self.label_5 = QtWidgets.QLabel(parent=dialog)
        self.label_5.setGeometry(QtCore.QRect(10, 100, 381, 16))
        self.label_5.setObjectName("label_5")
        self.label_6 = QtWidgets.QLabel(parent=dialog)
        self.label_6.setGeometry(QtCore.QRect(10, 130, 381, 16))
        self.label_6.setObjectName("label_6")
        self.label_7 = QtWidgets.QLabel(parent=dialog)
        self.label_7.setGeometry(QtCore.QRect(10, 150, 381, 16))
        self.label_7.setObjectName("label_7")
        self.label_8 = QtWidgets.QLabel(parent=dialog)
        self.label_8.setGeometry(QtCore.QRect(10, 180, 381, 16))
        self.label_8.setObjectName("label_8")
        self.label_9 = QtWidgets.QLabel(parent=dialog)
        self.label_9.setGeometry(QtCore.QRect(10, 200, 381, 16))
        self.label_9.setObjectName("label_9")
        self.label_10 = QtWidgets.QLabel(parent=dialog)
        self.label_10.setGeometry(QtCore.QRect(10, 220, 381, 16))
        self.label_10.setObjectName("label_10")
        self.quit = QtWidgets.QPushButton(parent=dialog)
        self.quit.setGeometry(QtCore.QRect(170, 260, 61, 24))
        self.quit.setObjectName("quit")
        self.open_obj_dir = QtWidgets.QPushButton(parent=dialog)
        self.open_obj_dir.setGeometry(QtCore.QRect(160, 50, 75, 21))
        self.open_obj_dir.setObjectName("open_obj_dir")

        self.retranslateUi(dialog)
        QtCore.QMetaObject.connectSlotsByName(dialog)
        dialog.setTabOrder(self.open_obj_dir, self.quit)

    def retranslateUi(self, dialog):
        _translate = QtCore.QCoreApplication.translate
        dialog.setWindowTitle(_translate("dialog", "SVN 무시 패턴 설정 - 자세히"))
        self.label.setText(_translate("dialog", "설정을 적용하면, "))
        self.label_2.setText(_translate("dialog", "C:\\Users\\username\\AppData\\Roaming\\Subversion\\ 경로의"))
        self.label_3.setText(_translate("dialog", "config 파일을 수정합니다."))
        self.label_4.setText(_translate("dialog", "global-ignores가 주석 처리되어 있으면 해제하고, 입력된 무시 패턴을"))
        self.label_5.setText(_translate("dialog", "해당 변수에 추가해 줍니다."))
        self.label_6.setText(_translate("dialog", "config 파일의 global-ignores 변수가 살아 있으면, TortoiseSVN에서"))
        self.label_7.setText(_translate("dialog", "설정한 global ignore 패턴은 무시됩니다."))
        self.label_8.setText(_translate("dialog", "\'기존 config 파일 백업\' 옵션을 사용하면, 기존의 config 파일을 "))
        self.label_9.setText(_translate("dialog", "config_backup_byABP 이름으로 복사해 둡니다. 만약 나중에 수동으로"))
        self.label_10.setText(_translate("dialog", "설정을 변경하려고 할 때 사용할 수 있습니다. 굳이?"))
        self.quit.setText(_translate("dialog", "알겠어요"))
        self.open_obj_dir.setText(_translate("dialog", "경로 열기"))
