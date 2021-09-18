QT        += core gui printsupport serialport network

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets opengl

CONFIG += c++11 console

# The following define makes your compiler emit warnings if you use
# any Qt feature that has been marked deprecated (the exact warnings
# depend on your compiler). Please consult the documentation of the
# deprecated API in order to know how to port your code away from it.
DEFINES += QT_DEPRECATED_WARNINGS
DEFINES += QCUSTOMPLOT_USE_OPENGL

# You can also make your code fail to compile if it uses deprecated APIs.
# In order to do so, uncomment the following line.
# You can also select to disable deprecated APIs only up to a certain version of Qt.
#DEFINES += QT_DISABLE_DEPRECATED_BEFORE=0x060000    # disables all the APIs deprecated before Qt 6.0.0

SOURCES += \
    ethconncontrol.cpp \
    ethconndatagram.cpp \
    graphics.cpp \
    json.cpp \
    locator.cpp \
    main.cpp \
    mainwindow.cpp \
    qcustomplot.cpp \
    rli.cpp \
    scanner.cpp \
    tcpconnection.cpp

HEADERS += \
    ethconncontrol.h \
    ethconndatagram.h \
    graphics.h \
    json.h \
    locator.h \
    mainwindow.h \
    qcustomplot.h \
    rli.h \
    scanner.h \
    tcpconnection.h

FORMS += \
    graphics.ui \
    mainwindow.ui

DESTDIR = ./_LAUNCH

# Default rules for deployment.
qnx: target.path = /tmp/$${TARGET}/bin
else: unix:!android: target.path = /opt/$${TARGET}/bin
!isEmpty(target.path): INSTALLS += target

LIBS += -lws2_32

DISTFILES += \
    blackDot.png \
    greenDot.png \
    antenna.png \
    icon.ico

RC_FILE += resources.rc
VERSION = 1.0.14
TARGET = RLI_$${VERSION}
DEFINES += VERSION_STRING=\\\"$${VERSION}\\\"
