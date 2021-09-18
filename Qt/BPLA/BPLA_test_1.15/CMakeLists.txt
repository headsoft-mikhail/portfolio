cmake_minimum_required(VERSION 3.5)

project(vectorTestClient LANGUAGES CXX)

set(CMAKE_INCLUDE_CURRENT_DIR ON)

set(CMAKE_AUTOUIC ON)
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)

set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED ON)


#Generate PROTOBUF ProtoFiles
set(PROTOBUF_LIB_NAME "protobuf")

set(NAMES_ "lib${PROTOBUF_LIB_NAME}${CMAKE_STATIC_LIBRARY_SUFFIX}")

find_library(PROTOBUF_LIBRARY
	NAMES
	    ${NAMES_}
		protobuf
		libprotobuf
	PATHS ${CMAKE_SOURCE_DIR}/redist/protobuf/lib_mingw)

find_path(PROTOBUF_INCLUDE_DIR
	google/protobuf/service.h
	PATHS ${CMAKE_SOURCE_DIR}/redist/protobuf/src
)

find_program(PROTOBUF_COMPILER protoc
	    PATHS ${CMAKE_SOURCE_DIR}/redist/protobuf/)

set(protos_out "${CMAKE_SOURCE_DIR}/WaypointActionCtrl.pb.h" "${CMAKE_SOURCE_DIR}/WaypointActionCtrl.pb.cc"
	"${CMAKE_SOURCE_DIR}/TelemetryCtrl.pb.h" "${CMAKE_SOURCE_DIR}/TelemetryCtrl.pb.cc")

message("Command exe " ${PROTOBUF_COMPILER})
message("protos " ${protos_out})

add_custom_command(
	    COMMAND ${PROTOBUF_COMPILER}
		ARGS --proto_path="${CMAKE_SOURCE_DIR}/Proto" --cpp_out="${CMAKE_SOURCE_DIR}" "WaypointActionCtrl.proto" "TelemetryCtrl.proto"
		OUTPUT ${protos_out}
		DEPENDS "${CMAKE_SOURCE_DIR}/Proto/"
		#DEPENDS "${CMAKE_SOURCE_DIR}/Proto/WaypointActionCtrl.proto" "${CMAKE_SOURCE_DIR}/Proto/TelemetryCtrl.proto"
		COMMENT "========== generate protofiles ==========="
)

add_custom_target(proto_gen ALL DEPENDS ${protos_out})


#Build project
find_package(Qt5Core REQUIRED)
find_package(Qt5Widgets REQUIRED)
find_package(Qt5Network REQUIRED)

include_directories(${PROTOBUF_INCLUDE_DIR})

add_executable(vectorTestClient
        WIN32
	main.cpp
	mainwindow.cpp
	mainwindow.h
	mainwindow.ui
        uav.cpp
        uav.h
        waypoint.cpp
        waypoint.h
        cargo.cpp
        cargo.h
        trackloader.cpp
        trackloader.h
        tcpconnection.cpp
        tcpconnection.h
        protovar.h
	WaypointActionCtrl.pb.h
	WaypointActionCtrl.pb.cc
	TelemetryCtrl.pb.h
	TelemetryCtrl.pb.cc
)

target_link_libraries(vectorTestClient PRIVATE Qt5::Core Qt5::Widgets Qt5::Network ${PROTOBUF_LIBRARY})

