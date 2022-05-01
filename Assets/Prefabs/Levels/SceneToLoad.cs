using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/SceneToLoad", order = 1)]
public class SceneToLoad : ScriptableObject
{
    public GameObject mapPrefab;
    public int targetMoves;
    public string hint;
}