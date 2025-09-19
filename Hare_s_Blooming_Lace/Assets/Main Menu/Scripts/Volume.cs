using TMPro;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public TextMeshProUGUI MusicVolPercentage;
    public TextMeshProUGUI SoundsVolPercentage;

    public void Update()
    {
        if (MusicVolPercentage != null)
        {
            float volume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
            int percentage = Mathf.RoundToInt(volume * 100)*2;
            MusicVolPercentage.text = percentage.ToString() + "%";
        }
        if (SoundsVolPercentage != null)
        {
            float volume = PlayerPrefs.GetFloat("SoundVolume", 0.2f);
            int percentage = Mathf.RoundToInt(volume * 100)*5;
            SoundsVolPercentage.text = percentage.ToString() + "%";
        }

    }
}
