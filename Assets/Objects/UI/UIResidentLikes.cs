using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResidentLikes : MonoBehaviour
{
    public static UIResidentLikes instance;
    [SerializeField] private RaceImages[] backgrounds;
    [SerializeField] private Color happyColor;
    [SerializeField] private Color angryColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color neutralColor;
    Race currentRace;

    private void Start()
    {
        instance = this;
        Grid3DManager.Instance.OnCubeChange += OnResidentChanged;
    }

    public void ShowGain(Race race , int value)
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].race == race)
            {
                backgrounds[i].ValueAdded.text = value.ToString();
                if (value > 0)
                    backgrounds[i].ValueAdded.color = happyColor;
                else if (value < -1)
                    backgrounds[i].ValueAdded.color = angryColor;
                else if (value == -1)
                    backgrounds[i].ValueAdded.color = neutralColor;
            }
            else
            {
                backgrounds[i].ValueAdded.text = "";
            }
        }
    }

    public void HideGain()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].ValueAdded.text = "";
        }
    }

    public void OnResidentChanged(PieceSO piece)
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].race == piece.resident.race)
            {
                backgrounds[i].image.color = selectedColor;
                continue;
            }
            int like = piece.resident.CheckLikes(backgrounds[i].race);
            if (like > 0)
            {
                backgrounds[i].image.color = happyColor;
            }else if (like < 0)
            {
                backgrounds[i].image.color = angryColor;
            }
            else
            {
                backgrounds[i].image.color = neutralColor;
            }
        }
        currentRace = piece.resident.race;
    }
}

[Serializable]
struct RaceImages
{
    public Image image;
    public Race race;
    public TextMeshProUGUI ValueAdded;
}
