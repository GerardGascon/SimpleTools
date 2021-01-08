using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Tools/Character", order = 0)]
public class Dialogue : ScriptableObject{

    public bool displayName;
    public string characterName;
    
    [Space]
    public Sprite characterImage;
    [TextArea] public string[] sentences;
}
