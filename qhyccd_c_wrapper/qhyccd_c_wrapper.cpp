// qhyccd_c.cpp: 与预编译标头对应的源文件

#include "framework.h"
#include "qhyccd_c_wrapper.h"
#include "qhyccd.h"

// 当使用预编译的头时，需要使用此源文件，编译才能成功。

uint32_t C_InitQHYCCDResource()
{
	return InitQHYCCDResource();
}

QHYCCD_API uint32_t C_ReleaseQHYCCDResource()
{
	return ReleaseQHYCCDResource();
}

QHYCCD_API uint32_t C_ScanQHYCCD()
{
	return ScanQHYCCD();
}

QHYCCD_API uint32_t C_GetQHYCCDId(uint32_t index, char* id)
{
	return GetQHYCCDId(index, id);
}

QHYCCD_API qhyccd_handle* C_OpenQHYCCD(char* id)
{
	return OpenQHYCCD(id);
}

QHYCCD_API uint32_t C_CloseQHYCCD(qhyccd_handle* handle)
{
	return CloseQHYCCD(handle);
}

QHYCCD_API uint32_t C_GetQHYCCDReadModeNumber(qhyccd_handle* handle, uint32_t* number)
{
	return GetQHYCCDNumberOfReadModes(handle, number);
}

QHYCCD_API uint32_t C_GetQHYCCDReadModeName(qhyccd_handle* handle, uint32_t index, char* name)
{
	return GetQHYCCDReadModeName(handle, index, name);
}

QHYCCD_API uint32_t C_SetQHYCCDReadMode(qhyccd_handle* handle, uint32_t modeIndex)
{
	return SetQHYCCDReadMode(handle, modeIndex);
}

QHYCCD_API uint32_t C_SetQHYCCDStreamMode(qhyccd_handle* handle, uint32_t readmode)
{
	return SetQHYCCDStreamMode(handle, readmode);
}

QHYCCD_API uint32_t C_InitQHYCCD(qhyccd_handle* handle)
{
	return InitQHYCCD(handle);
}

QHYCCD_API uint32_t C_SetQHYCCDDebayerOnOff(qhyccd_handle* handle, bool onoff)
{
	return SetQHYCCDDebayerOnOff(handle, onoff);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_Bits(qhyccd_handle* handle, double bits)
{
	return SetQHYCCDParam(handle, CONTROL_TRANSFERBIT, bits);
}

QHYCCD_API uint32_t C_SetQHYCCDBinMode(qhyccd_handle* handle, uint32_t wbin, uint32_t hbin)
{
	return SetQHYCCDBinMode(handle, wbin, hbin);
}

QHYCCD_API uint32_t C_SetQHYCCDResolution(qhyccd_handle* handle, uint32_t x, uint32_t y, uint32_t xsize, uint32_t ysize)
{
	return SetQHYCCDResolution(handle, x, y, xsize, ysize);
}

QHYCCD_API uint32_t C_GetQHYCCDChipInfo(qhyccd_handle* handle, double* chipw, double* chiph, uint32_t* imagew, uint32_t* imageh, double* pixelw, double* pixelh, uint32_t* bpp)
{
	return GetQHYCCDChipInfo(handle, chipw, chiph, imagew, imageh, pixelw, pixelh, bpp);
}

QHYCCD_API uint32_t C_GetQHYCCDEffectiveArea(qhyccd_handle* handle, uint32_t* startX, uint32_t* startY, uint32_t* sizeX, uint32_t* sizeY)
{
	return GetQHYCCDEffectiveArea(handle, startX, startY, sizeX, sizeY);
}

QHYCCD_API uint32_t C_GetQHYCCDOverScanArea(qhyccd_handle* handle, uint32_t* startX, uint32_t* startY, uint32_t* sizeX, uint32_t* sizeY)
{
	return GetQHYCCDOverScanArea(handle, startX, startY, sizeX, sizeY);
}

QHYCCD_API uint32_t C_GetQHYCCDMemLength(qhyccd_handle* handle)
{
	return GetQHYCCDMemLength(handle);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_Exposure(qhyccd_handle* handle, double time)
{
	return SetQHYCCDParam(handle, CONTROL_EXPOSURE, time);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_Gain(qhyccd_handle* handle, double gain)
{
	return SetQHYCCDParam(handle, CONTROL_GAIN, gain);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_Offset(qhyccd_handle* handle, double offset)
{
	return SetQHYCCDParam(handle, CONTROL_OFFSET, offset);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_Traffic(qhyccd_handle* handle, double traffic)
{
	return SetQHYCCDParam(handle, CONTROL_USBTRAFFIC, traffic);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_WBR(qhyccd_handle* handle, double wbr)
{
	return SetQHYCCDParam(handle, CONTROL_WBR, wbr);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_WBG(qhyccd_handle* handle, double wbg)
{
	return SetQHYCCDParam(handle, CONTROL_WBB, wbg);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_WBB(qhyccd_handle* handle, double wbb)
{
	return SetQHYCCDParam(handle, CONTROL_WBB, wbb);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_Brightness(qhyccd_handle* handle, double brightness)
{
	return SetQHYCCDParam(handle, CONTROL_BRIGHTNESS, brightness);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_Contrast(qhyccd_handle* handle, double contrast)
{
	return SetQHYCCDParam(handle, CONTROL_CONTRAST, contrast);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_Gamma(qhyccd_handle* handle, double gamma)
{
	return SetQHYCCDParam(handle, CONTROL_GAMMA, gamma);
}

QHYCCD_API uint32_t C_ExpQHYCCDSingleFrame(qhyccd_handle* handle)
{
	return ExpQHYCCDSingleFrame(handle);
}

QHYCCD_API uint32_t C_GetQHYCCDSingleFrame(qhyccd_handle* handle, uint32_t* w, uint32_t* h, uint32_t* bpp, uint32_t* channels, uint8_t* imgdata)
{
	return GetQHYCCDSingleFrame(handle, w, h, bpp, channels, imgdata);
}

QHYCCD_API uint32_t C_CancelQHYCCDExposingAndReadout(qhyccd_handle* handle)
{
	return CancelQHYCCDExposingAndReadout(handle);
}

QHYCCD_API uint32_t C_BeginQHYCCDLive(qhyccd_handle* handle)
{
	return BeginQHYCCDLive(handle);
}

QHYCCD_API uint32_t C_GetQHYCCDLiveFrame(qhyccd_handle* handle, uint32_t* w, uint32_t* h, uint32_t* bpp, uint32_t* channels, uint8_t* imgdata)
{
	return GetQHYCCDLiveFrame(handle, w, h, bpp, channels, imgdata);
}

QHYCCD_API uint32_t C_StopQHYCCDLive(qhyccd_handle* handle)
{
	return StopQHYCCDLive(handle);
}

QHYCCD_API uint32_t C_GetQHYCCDTrigerInterfaceNumber(qhyccd_handle* handle, uint32_t* interfaceNumber)
{
	return GetQHYCCDTrigerInterfaceNumber(handle, interfaceNumber);
}

QHYCCD_API uint32_t C_GetQHYCCDTrigerInterfaceName(qhyccd_handle* handle, uint32_t interfaceIndex, char* name)
{
	return GetQHYCCDTrigerInterfaceName(handle, interfaceIndex, name);
}

QHYCCD_API uint32_t C_SetQHYCCDTrigerInterface(qhyccd_handle* handle, uint32_t interfaceIndex)
{
	return SetQHYCCDTrigerInterface(handle, interfaceIndex);
}

QHYCCD_API uint32_t C_SetQHYCCDTrigerFunction(qhyccd_handle* handle, bool onoff)
{
	return SetQHYCCDTrigerFunction(handle, onoff);
}

QHYCCD_API uint32_t C_EnableQHYCCDTrigerOut(qhyccd_handle* handle)
{
	return EnableQHYCCDTrigerOut(handle);
}

QHYCCD_API uint32_t C_EnableQHYCCDBurstMode(qhyccd_handle* handle, bool onoff)
{
	return EnableQHYCCDBurstMode(handle, onoff);
}

QHYCCD_API uint32_t C_SetQHYCCDBurstModeStartEnd(qhyccd_handle* handle, uint32_t start, uint32_t end)
{
	return SetQHYCCDBurstModeStartEnd(handle, start, end);
}

QHYCCD_API uint32_t C_SetQHYCCDBurstModePatchNumber(qhyccd_handle* handle, uint32_t number)
{
	return SetQHYCCDBurstModePatchNumber(handle, number);
}

QHYCCD_API uint32_t C_SetQHYCCDBurstIDLE(qhyccd_handle* handle)
{
	return SetQHYCCDBurstIDLE(handle);
}

QHYCCD_API uint32_t C_ReleaseQHYCCDBurstIDLE(qhyccd_handle* handle)
{
	return ReleaseQHYCCDBurstIDLE(handle);
}

QHYCCD_API double C_GetQHYCCDParam_CurTempture(qhyccd_handle* handle)
{
	return GetQHYCCDParam(handle, CONTROL_CURTEMP);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_TargetTemperature(qhyccd_handle* handle, double temp)
{
	return SetQHYCCDParam(handle, CONTROL_COOLER, temp);
}

QHYCCD_API uint32_t C_SetQHYCCDParam_CoolerPWM(qhyccd_handle* handle, double pwm)
{
	return SetQHYCCDParam(handle, CONTROL_MANULPWM, pwm);
}
