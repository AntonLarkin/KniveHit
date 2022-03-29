using UnityEngine;

[CreateAssetMenu(menuName = "Appearbale Objects", fileName = "Appearable Object")]
public class AppearableObjectData : ScriptableObject
{
    [SerializeField] private float chanceToAppear;

    [SerializeField] private int minNumberToAppear;
    [SerializeField] private int maxNumberToAppear;

    public int NumberToAppear
    {
        get
        {
            return UnityEngine.Random.Range(minNumberToAppear, maxNumberToAppear+1);
        }
    }

    public bool IsAppeared()
    {

        if (UnityEngine.Random.Range(0, 100) < chanceToAppear)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
