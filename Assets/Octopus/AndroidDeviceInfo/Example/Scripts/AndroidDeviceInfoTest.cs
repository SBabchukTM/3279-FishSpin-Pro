using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidDeviceInfoTest : MonoBehaviour
{
    [SerializeField] private Text _chipModel;
    [SerializeField] private Text _chipProvider;
    [SerializeField] private Text _supportedBinaryInterfaces;
    [SerializeField] private Text _deviceModel;
    [SerializeField] private Text _deviceBrand;
    [SerializeField] private Text _batteryCapacityLevelLow;
    [SerializeField] private Text _batteryCapacityLevelHigh;

    private void Start()
    {
        float batteryLevel = SystemInfo.batteryLevel; // 0.0–1.0, або -1 якщо недоступно
        BatteryStatus status = SystemInfo.batteryStatus;
        _chipModel.text = "Chip Model: " + AndroidDeviceInfo.GetChipModel();
        _chipProvider.text = "Chip Provider: " + AndroidDeviceInfo.GetChipProvider();
        _supportedBinaryInterfaces.text = "ABIs: " + AndroidDeviceInfo.GetSupportedBinaryInterfaces();
        _deviceModel.text = "Device Model: " + AndroidDeviceInfo.GetDeviceModel();
        _deviceBrand.text = "Device Brand: " + AndroidDeviceInfo.GetDeviceBrand();
        _batteryCapacityLevelLow.text = "Battery Low: " + AndroidDeviceInfo.GetBatteryCapacityLevelLow();
        //_batteryCapacityLevelHigh.text = "Battery High: " + AndroidDeviceInfo.GetBatteryCapacityLevelHigh();
        //_batteryCapacityLevelLow.text = "Battery Low: " + (SystemInfo.batteryStatus);
        _batteryCapacityLevelHigh.text = "Battery High: " + (SystemInfo.batteryLevel * 100);
    }

}
