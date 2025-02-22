using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class U002_RewardDailyReset: MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button claimButton;

    private static readonly string LastClaimKey = "LastClaimDate";

    private void Start()
    {
        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(ClaimReward);
        
        int lastClaimDate = PlayerPrefs.GetInt("LastClaimDate", 0);
        Debug.Log("📌 LastClaimDate: " + lastClaimDate);

   

    }

    private bool CanClaimReward()
    {
        int lastClaimDate = PlayerPrefs.GetInt(LastClaimKey, 0);
        int currentDate = GetCurrentDate();

        return lastClaimDate < currentDate; // Nếu ngày nhận trước nhỏ hơn hôm nay → Được nhận
    }

    private void ClaimReward()
    {
        if (CanClaimReward())
        {
            PlayerPrefs.SetInt(LastClaimKey, GetCurrentDate());
            PlayerPrefs.Save();

            Debug.Log("🎉 Đã nhận quà ngày hôm nay!");
            UpdateUI();
        }
        else
        {
            Debug.Log("⏳ Bạn đã nhận quà hôm nay rồi!");
        }
    }

    private void UpdateUI()
    {
        if (CanClaimReward())
        {
            statusText.text = "🎁 Sẵn sàng nhận quà!";
            claimButton.interactable = true;
        }
        else
        {
            statusText.text = "⏳ Đã nhận hôm nay, quay lại sau!";
            claimButton.interactable = false;
        }
    }

    private int GetCurrentDate()
    {
        TimeZoneInfo vietnamZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamZone);
        return vietnamTime.Year * 10000 + vietnamTime.Month * 100 + vietnamTime.Day;
    }

}
