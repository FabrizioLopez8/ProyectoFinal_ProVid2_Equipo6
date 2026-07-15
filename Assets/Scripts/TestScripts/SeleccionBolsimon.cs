using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class SeleccionBolsimon : MonoBehaviour
{
    public Image image;
    public static List<Color> bolsimon = new List<Color>
    {
        Color.red,
        Color.blue,
        Color.green
    };

    void Start()
    {
        SetImageColor(bolsimon[0]);
        PlayersData.bolsimonP1 = Bolsimon.GetElementTypeOnColor(bolsimon[ColorChangeHelper.indexImage1]);
        PlayersData.bolsimonP2 = Bolsimon.GetElementTypeOnColor(bolsimon[ColorChangeHelper.indexImage2]);
        PlayersData.bolsimonP3 = Bolsimon.GetElementTypeOnColor(bolsimon[ColorChangeHelper.indexImage3]);
    }
    public void NextColor(int image)
    {
        
        switch (image)
        {
            case 1:
                ColorChangeHelper.indexImage1 += 1;
                if (ColorChangeHelper.indexImage1 >= bolsimon.Count) ColorChangeHelper.indexImage1 = 0;
                SetImageColor(bolsimon[ColorChangeHelper.indexImage1]);
                PlayersData.bolsimonP1 = Bolsimon.GetElementTypeOnColor(bolsimon[ColorChangeHelper.indexImage1]);
                break;
            case 2:
                ColorChangeHelper.indexImage2 += 1;
                if (ColorChangeHelper.indexImage2 >= bolsimon.Count) ColorChangeHelper.indexImage2 = 0;
                SetImageColor(bolsimon[ColorChangeHelper.indexImage2]);
                PlayersData.bolsimonP2 = Bolsimon.GetElementTypeOnColor(bolsimon[ColorChangeHelper.indexImage2]);
                break;
            case 3:
                ColorChangeHelper.indexImage3 += 1;
                if (ColorChangeHelper.indexImage3 >= bolsimon.Count) ColorChangeHelper.indexImage3 = 0;
                SetImageColor(bolsimon[ColorChangeHelper.indexImage3]);
                PlayersData.bolsimonP3 = Bolsimon.GetElementTypeOnColor(bolsimon[ColorChangeHelper.indexImage3]);
                break;
        }  
    }

    public void PreviousColor(int image)
    {
        switch (image)
        {
            case 1:
                ColorChangeHelper.indexImage1 -= 1;
                if (ColorChangeHelper.indexImage1 < 0) ColorChangeHelper.indexImage1 = bolsimon.Count - 1;
                SetImageColor(bolsimon[ColorChangeHelper.indexImage1]);
                PlayersData.bolsimonP1 = Bolsimon.GetElementTypeOnColor(bolsimon[ColorChangeHelper.indexImage1]);
                break;
            case 2:
                ColorChangeHelper.indexImage2 -= 1;
                if (ColorChangeHelper.indexImage2 < 0) ColorChangeHelper.indexImage2 = bolsimon.Count - 1;
                SetImageColor(bolsimon[ColorChangeHelper.indexImage2]);
                PlayersData.bolsimonP2 = Bolsimon.GetElementTypeOnColor(bolsimon[ColorChangeHelper.indexImage2]);
                break;
            case 3:
                ColorChangeHelper.indexImage3 -= 1;
                if (ColorChangeHelper.indexImage3 < 0) ColorChangeHelper.indexImage3 = bolsimon.Count - 1;
                SetImageColor(bolsimon[ColorChangeHelper.indexImage3]);
                PlayersData.bolsimonP3 = Bolsimon.GetElementTypeOnColor(bolsimon[ColorChangeHelper.indexImage3]);
                break;
        }
    }

    void SetImageColor(Color color)
    {
        image.color = color;
    }
}
