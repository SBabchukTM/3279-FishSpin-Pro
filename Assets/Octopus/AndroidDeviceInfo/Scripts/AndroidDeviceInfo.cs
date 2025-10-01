using UnityEngine;

public static class AndroidDeviceInfo
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaClass _deviceInfoClass;
    private static AndroidJavaObject _context;

    static AndroidDeviceInfo()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            _context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

        _deviceInfoClass = new AndroidJavaClass("com.androiddeviceinfo.module.DeviceInfo");
    }
#endif

    public static string GetChipModel()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return _deviceInfoClass.CallStatic<string>("getChipModel");
#else
        return "Editor_ChipModel";
#endif
    }

    public static string GetChipProvider()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return _deviceInfoClass.CallStatic<string>("getChipProvider");
#else
        return "Editor_ChipProvider";
#endif
    }

    public static string GetSupportedBinaryInterfaces()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return _deviceInfoClass.CallStatic<string>("getSupportedBinaryInterfaces");
#else
        return "Editor_ABIs";
#endif
    }

    public static string GetDeviceModel()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return _deviceInfoClass.CallStatic<string>("getDeviceModel");
#else
        return SystemInfo.deviceModel;
#endif
    }

    public static string GetDeviceBrand()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return _deviceInfoClass.CallStatic<string>("getDeviceBrand");
#else
        return "Editor_Brand";
#endif
    }

    public static string GetBatteryCapacityLevelLow()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return _deviceInfoClass.CallStatic<string>("getBatteryCapacityLevelLow", _context);
#else
        return ((int)(SystemInfo.batteryLevel * 100)).ToString();
#endif
    }

    public static string GetBatteryCapacityLevelHigh()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return _deviceInfoClass.CallStatic<string>("getBatteryCapacityLevelHigh", _context);
#else
        return ((int)(SystemInfo.batteryLevel * 100)).ToString();
#endif
    }
}
