using TMPro;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public TextMeshProUGUI MusicVolPercentage;

    public void Update()
    {
        if (MusicVolPercentage != null)
        {
            float volume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
            int percentage = Mathf.RoundToInt(volume * 100)*2;
            MusicVolPercentage.text = percentage.ToString() + "%";
        }

    }
}
