using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Simple Tools/Character", order = 11)]
public class Dialogue : ScriptableObject{

    public bool displayName;
    public string characterName;
    
    [Space]
    public Sprite characterImage;
    [TextArea] public string[] sentences;
}
