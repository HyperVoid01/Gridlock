using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats")]
public class Stats : ScriptableObject
{
    public int highestRound;
    public float musicVolume;
    public float sfxVolume;
    public bool doneTutorial;
}
