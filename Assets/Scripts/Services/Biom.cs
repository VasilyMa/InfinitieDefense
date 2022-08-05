using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biom
{
    public List<int> BiomLevels; // количество уровней биома
    public int StartBiomLevel; // стартовый уровень биома
    public BiomType BiomType; // тип биома
    public Sprite BiomSprite; // картинка отображающая биом
    public Sprite NextBiomSprite;
    //public Sprite BlurSprite; // заблюренная картинка для магазина
}

public enum BiomType
{
    Iceland = 1, Oasis = 2, Wasteland = 3
}
