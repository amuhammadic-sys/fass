using UnityEngine;
public enum VibrationType
{
    Light,
    Medium,
    Heavy
}

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;

    public bool Enabled => PlayerPrefs.GetInt("vibration_enabled", 1) == 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Vibrate(VibrationType type)
    {
        if (!Enabled) return;

#if UNITY_ANDROID || UNITY_IOS
        switch (type)
        {
            case VibrationType.Light:
                Handheld.Vibrate();
                break;

            case VibrationType.Medium:
                Handheld.Vibrate();
                Invoke(nameof(DelayedVibrate), 0.05f);
                break;

            case VibrationType.Heavy:
                Handheld.Vibrate();
                Invoke(nameof(DelayedVibrate), 0.05f);
                Invoke(nameof(DelayedVibrate), 0.1f);
                break;
        }
#endif
    }

    private void DelayedVibrate()
    {
#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }
}
