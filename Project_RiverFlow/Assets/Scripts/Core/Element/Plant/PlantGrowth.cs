using System;
using UnityEngine;

[Serializable]
public struct PlantGrowth
{
    public Sprite deadSprite;
    public Sprite agonySprite;
    public Sprite babySprite;
    public Sprite youngSprite;
    public Sprite adultSprite;
    public Sprite seniorSprite;
    public Sprite fruitSprite;
    [Space(10)]
    public Sprite[] flowers;
    public Sprite StateSprite(PlantState state)
    {
        switch (state)
        {
            case PlantState.Dead:
                return deadSprite;
            case PlantState.Agony:
                return agonySprite;
            case PlantState.Baby:
                return babySprite;
            case PlantState.Young:
                return youngSprite;
            case PlantState.Adult:
                return adultSprite;
            case PlantState.Senior:
                return seniorSprite;
            case PlantState.FruitTree:
                return fruitSprite;
            default:
                return null;
        }
    }

}
