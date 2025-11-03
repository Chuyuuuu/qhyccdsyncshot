// qhyccd_c_wrapper.h: 这是预编译标头文件。
// 下方列出的文件仅编译一次，提高了将来生成的生成性能。
// 这还将影响 IntelliSense 性能，包括代码完成和许多代码浏览功能。
// 但是，如果此处列出的文件中的任何一个在生成之间有更新，它们全部都将被重新编译。
// 请勿在此处添加要频繁更新的文件，这将使得性能优势无效。

#ifndef QHYCCD_C_WRAPPER_H
#define QHYCCD_C_WRAPPER_H

// 添加要在此处预编译的标头
#include <stdint.h>

#pragma once

#ifdef _WIN32
#ifdef QHYCCD_C_WRAPPER_EXPORTS
#define QHYCCD_API __declspec(dllexport)
#else
#define QHYCCD_API __declspec(dllimport)
#endif
#else
#define QHYCCD_API
#endif

typedef void qhyccd_handle;
//typedef int int32_t;
//typedef unsigned int uint32_t;

#ifdef __cplusplus
extern "C" {
#endif

    //SDK resource functions
    QHYCCD_API uint32_t C_InitQHYCCDResource();

    QHYCCD_API uint32_t C_ReleaseQHYCCDResource();

    //scan and open camera functions
    QHYCCD_API uint32_t C_ScanQHYCCD();

    QHYCCD_API uint32_t C_GetQHYCCDId(uint32_t index, char* id);

    QHYCCD_API qhyccd_handle * C_OpenQHYCCD(char* id);

    QHYCCD_API uint32_t C_CloseQHYCCD(qhyccd_handle* handle);

    //read mode
    QHYCCD_API uint32_t C_GetQHYCCDReadModeNumber(qhyccd_handle* handle, uint32_t* number);

    QHYCCD_API uint32_t C_GetQHYCCDReadModeName(qhyccd_handle* handle, uint32_t modeIndex, char* name);

    QHYCCD_API uint32_t C_SetQHYCCDReadMode(qhyccd_handle* handle, uint32_t modeIndex);

    //stream mode
    QHYCCD_API uint32_t C_SetQHYCCDStreamMode(qhyccd_handle* handle, uint32_t readmode);

    //init camera
    QHYCCD_API uint32_t C_InitQHYCCD(qhyccd_handle *handle);

    //necessary setting
    QHYCCD_API uint32_t C_SetQHYCCDDebayerOnOff(qhyccd_handle* handle, bool onoff);

	QHYCCD_API uint32_t C_SetQHYCCDParam_Bits(qhyccd_handle* handle, double bits);

    QHYCCD_API uint32_t C_SetQHYCCDBinMode(qhyccd_handle* handle, uint32_t wbin, uint32_t hbin);

	QHYCCD_API uint32_t C_SetQHYCCDResolution(qhyccd_handle* handle, uint32_t x, uint32_t y, uint32_t xsize, uint32_t ysize);

    //get camera information
    QHYCCD_API uint32_t C_GetQHYCCDChipInfo(qhyccd_handle* handle, double* chipw, double* chiph, uint32_t* imagew, uint32_t* imageh, double* pixelw, double* pixelh, uint32_t* bpp);

    QHYCCD_API uint32_t C_GetQHYCCDEffectiveArea(qhyccd_handle* handle, uint32_t* startX, uint32_t* startY, uint32_t* sizeX, uint32_t* sizeY);

    QHYCCD_API uint32_t C_GetQHYCCDOverScanArea(qhyccd_handle* handle, uint32_t* startX, uint32_t* startY, uint32_t* sizeX, uint32_t* sizeY);

    QHYCCD_API uint32_t C_GetQHYCCDMemLength(qhyccd_handle* handle);

    //optional setting
    QHYCCD_API uint32_t C_SetQHYCCDParam_Exposure(qhyccd_handle* handle, double time);

    QHYCCD_API uint32_t C_SetQHYCCDParam_Gain(qhyccd_handle* handle, double gain);

    QHYCCD_API uint32_t C_SetQHYCCDParam_Offset(qhyccd_handle* handle, double offset);

    QHYCCD_API uint32_t C_SetQHYCCDParam_Traffic(qhyccd_handle* handle, double traffic);

    QHYCCD_API uint32_t C_SetQHYCCDParam_WBR(qhyccd_handle* handle, double wbr);

    QHYCCD_API uint32_t C_SetQHYCCDParam_WBG(qhyccd_handle* handle, double wbg);

    QHYCCD_API uint32_t C_SetQHYCCDParam_WBB(qhyccd_handle* handle, double wbb);

    QHYCCD_API uint32_t C_SetQHYCCDParam_Brightness(qhyccd_handle* handle, double brightness);

    QHYCCD_API uint32_t C_SetQHYCCDParam_Contrast(qhyccd_handle* handle, double contrast);

    QHYCCD_API uint32_t C_SetQHYCCDParam_Gamma(qhyccd_handle* handle, double gamma);

    //single capture functions
    QHYCCD_API uint32_t C_ExpQHYCCDSingleFrame(qhyccd_handle* handle);

    QHYCCD_API uint32_t C_GetQHYCCDSingleFrame(qhyccd_handle* handle, uint32_t* w, uint32_t* h, uint32_t* bpp, uint32_t* channels, uint8_t* imgdata);

    QHYCCD_API uint32_t C_CancelQHYCCDExposingAndReadout(qhyccd_handle* handle);

    //live capture functions
    QHYCCD_API uint32_t C_BeginQHYCCDLive(qhyccd_handle* handle);

    QHYCCD_API uint32_t C_GetQHYCCDLiveFrame(qhyccd_handle* handle, uint32_t* w, uint32_t* h, uint32_t* bpp, uint32_t* channels, uint8_t* imgdata);

    QHYCCD_API uint32_t C_StopQHYCCDLive(qhyccd_handle* handle);

    //trigger functions
    QHYCCD_API uint32_t C_GetQHYCCDTrigerInterfaceNumber(qhyccd_handle* handle, uint32_t* interfaceNumber);

    QHYCCD_API uint32_t C_GetQHYCCDTrigerInterfaceName(qhyccd_handle* handle, uint32_t interfaceIndex, char* name);

    QHYCCD_API uint32_t C_SetQHYCCDTrigerInterface(qhyccd_handle* handle, uint32_t interfaceIndex);

    QHYCCD_API uint32_t C_SetQHYCCDTrigerFunction(qhyccd_handle* handle, bool onoff);

    QHYCCD_API uint32_t C_EnableQHYCCDTrigerOut(qhyccd_handle* handle);

    //burst mode functions
    QHYCCD_API uint32_t C_EnableQHYCCDBurstMode(qhyccd_handle* handle, bool onoff);

    QHYCCD_API uint32_t C_SetQHYCCDBurstModeStartEnd(qhyccd_handle* handle, uint32_t start, uint32_t end);

    QHYCCD_API uint32_t C_SetQHYCCDBurstModePatchNumber(qhyccd_handle* handle, uint32_t number);

    QHYCCD_API uint32_t C_SetQHYCCDBurstIDLE(qhyccd_handle* handle);

    QHYCCD_API uint32_t C_ReleaseQHYCCDBurstIDLE(qhyccd_handle* handle);

    //temperature control functions
    QHYCCD_API double C_GetQHYCCDParam_CurTempture(qhyccd_handle* handle);

    QHYCCD_API uint32_t C_SetQHYCCDParam_TargetTemperature(qhyccd_handle* handle, double temp);
    
    QHYCCD_API uint32_t C_SetQHYCCDParam_CoolerPWM(qhyccd_handle* handle, double pwm);

#ifdef __cplusplus
}
#endif

#endif //QHYCCD_C_WRAPPER_H
