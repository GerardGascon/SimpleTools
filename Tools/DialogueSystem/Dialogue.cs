using UnityEngine;

namespace SimpleTools.DialogueSystem {
	[CreateAssetMenu(fileName = "New Dialogue", menuName = "Simple Tools/Dialogue", order = 11)]
	public class Dialogue : ScriptableObject {
		public DialogueBox[] sentences;
	}

	[System.Serializable]
	public class DialogueBox {
		public bool displayName;
		public string characterName;
		public Sprite characterImage;
		[TextArea(5, 10)] public string sentence;
	}
}