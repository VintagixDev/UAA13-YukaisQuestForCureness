using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public Slider volumeSlider;
    public TextMeshProUGUI volumePercentage;
    public Button[] keybindButtons;


    private Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>()
    {
        { "MoveLeft", KeyCode.Q },
        { "MoveUp", KeyCode.Z },
        { "MoveDown", KeyCode.S },
        { "MoveRight", KeyCode.D },

        { "ShootLeft", KeyCode.LeftArrow },
        { "ShootUp", KeyCode.UpArrow },
        { "ShootDown", KeyCode.DownArrow },
        { "ShootRight", KeyCode.RightArrow }

    };


    public void Start()
    {
        volumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        RefreshSliderValue();

        // Keybinds
        
        foreach(var key in keybinds)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }

    }
    public void ValueChangeCheck()
    {
        RefreshSliderValue();
    }


    public void RefreshSliderValue()
    {
        float value = Mathf.Round(volumeSlider.value * 100);
        volumePercentage.text = value.ToString() + "%";
    }


    public void Update()
    {
        
    }

    public void ChangeKey(Button button)
    {
        Transform child = button.transform.GetChild(0);
        TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
        text.text = "...";
        /*while (text.text == "...")
        {
            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keycode))
                {
                    text.text = keycode.ToString();
                    PlayerPrefs.SetString(button.transform.parent.name, keycode.ToString());
                    PlayerPrefs.Save();

                    KeyCode key = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(button.transform.parent.name));
                    Debug.Log(key);
                    Debug.Log(button.transform.parent.name);
                    Debug.Log(PlayerPrefs.GetString(button.transform.parent.name));
                }
            }
        }*/
    }
}
