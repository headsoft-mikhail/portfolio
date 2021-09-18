#ifndef PROTO_VAR_H
#define PROTO_VAR_H

#include <QObject>

enum class CTRL_MSG_TYPE
{
    CTRL_MSG_TELEMETRY = 2,
    CTRL_MSG_WAYPOINT_ACTION = 4,
};

typedef uint16_t MsgType_t;

#define GCS_EXNG_PREAMBLE {UINT8_C(0xB1), UINT8_C(0x3F), UINT8_C(0xCE), UINT8_C(0x61)}
#define GCS_EXNG_PREAMBLE_SIZE 4
#define GCS_EXNG_MAX_DATA_SIZE UINT16_C(0xFFFF)
#define GCS_EXNG_HEADER_INIT {GCS_EXNG_PREAMBLE, UINT16_C(0), UINT16_C(0)}

#pragma pack(push, 1)
typedef struct GcsExngHeader {
    uint8_t preamble[GCS_EXNG_PREAMBLE_SIZE];
    MsgType_t type;                             // type of proto message
    uint16_t len;                               // len of proto data
} GcsExngHeader;
#pragma pack(pop)

#endif // PROTO_VAR_H

