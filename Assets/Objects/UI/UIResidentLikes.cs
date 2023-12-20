using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResidentLikes : MonoBehaviour
{
    [SerializeField] private RaceImages[] backgrounds;
    [SerializeField] private Color happyColor;
    [SerializeField] private Color angryColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color neutralColor;

    private void Start()
    {
        Grid3DManager.Instance.OnCubeChange += OnResidentChanged;
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
    }
}

[Serializable]
struct RaceImages
{
    public Image image;
    public Race race;
}
