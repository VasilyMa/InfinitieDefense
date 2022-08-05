using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biom
{
    public List<int> BiomLevels; // ���������� ������� �����
    public int StartBiomLevel; // ��������� ������� �����
    public BiomType BiomType; // ��� �����
    public Sprite BiomSprite; // �������� ������������ ����
    public Sprite NextBiomSprite;
    //public Sprite BlurSprite; // ����������� �������� ��� ��������
}

public enum BiomType
{
    Iceland = 1, Oasis = 2, Wasteland = 3
}
