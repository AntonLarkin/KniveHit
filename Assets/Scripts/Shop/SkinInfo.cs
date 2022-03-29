using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Skin", menuName = "Create New Skin")]
public class SkinInfo : ScriptableObject
{
    [SerializeField] private int skinId;

    [SerializeField] private Sprite skinSprite;

    [SerializeField] private int skinPrice;

    public int SkinId => skinId;
    public Sprite SkinSprite => skinSprite;
    public int SkinPrice => skinPrice;
}
