using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleTools.DialogueSystem {
	public class DialogueManager : MonoBehaviour {

		DialogueVertexAnimator dialogueVertexAnimator;

		Queue<string> sentences;
		Queue<bool> displayNames;
		Queue<string> characterNames;
		Queue<Sprite> characterImages;
		bool talking;

		public DialogueManagerItems dialogueItems;

		public static DialogueManager instance;
		void Awake() {
			instance = this;
			sentences = new Queue<string>();
			displayNames = new Queue<bool>();
			characterNames = new Queue<string>();
			characterImages = new Queue<Sprite>();

			dialogueVertexAnimator = new DialogueVertexAnimator(dialogueItems.textBox);
		}

		/// <summary>
		/// This is the main function to call to start a dialogue.
		/// </summary>
		/// <param name="dialogue">The dialogue to start.</param>
		/// <returns>A bool that is false if the dialogue has finished and true if it hasn't.</returns>
		public bool Dialogue(Dialogue dialogue) {
			return Dialogue(dialogue, string.Empty);
		}

		/// <summary>
		/// This is the main function to call to start a dialogue.
		/// </summary>
		/// <param name="dialogue">The dialogue to start.</param>
		/// <param name="sounds">The sounds from the AudioManager that will be played on character reveal.</param>
		/// <returns>A bool that is false if the dialogue has finished and true if it hasn't.</returns>
		public bool Dialogue(Dialogue dialogue, params string[] sounds) {
			dialogueVertexAnimator.SetAudioSourceGroup(sounds);

			if (!talking) {
				sentences.Clear();
				if (dialogue.sentences.Length != 0) {
					foreach (DialogueBox sentence in dialogue.sentences) {
						sentences.Enqueue(sentence.sentence);
						displayNames.Enqueue(sentence.displayName);
						characterNames.Enqueue(sentence.characterName);
						characterImages.Enqueue(sentence.characterImage);
					}
				} else {
					sentences.Enqueue("I am error. No text has been added");
				}
				talking = true;

				if (sentences.Count == 0) {
					if (dialogueVertexAnimator.IsMessageAnimating())
						return true;
					talking = false;
					return false;
				}

				string sentenceToShow = sentences.Peek();
				bool displayName = displayNames.Peek();
				string characterName = characterNames.Peek();
				Sprite characterImage = characterImages.Peek();
				if (PlayDialogue(sentenceToShow, displayName, characterName, characterImage)) {
					sentences.Dequeue();
					displayNames.Dequeue();
					characterNames.Dequeue();
					characterImages.Dequeue();
				}
				return true;
			} else {
				if (sentences.Count == 0) {
					if (dialogueVertexAnimator.IsMessageAnimating())
						return true;
					talking = false;
					return false;
				}

				string sentenceToShow = sentences.Peek();
				bool displayName = displayNames.Peek();
				string characterName = characterNames.Peek();
				Sprite characterImage = characterImages.Peek();
				if (PlayDialogue(sentenceToShow, displayName, characterName, characterImage)) {
					sentences.Dequeue();
					displayNames.Dequeue();
					characterNames.Dequeue();
					characterImages.Dequeue();
				}
				return true;
			}
		}

		private Coroutine typeRoutine = null;
		bool PlayDialogue(string message, bool displayName = false, string characterName = "", Sprite characterImage = null) {
			if (dialogueVertexAnimator.IsMessageAnimating()) {
				dialogueVertexAnimator.SkipToEndOfCurrentMessage();
				return false; //Next message hasn't been shown because the current one is still animating.
			}
			this.EnsureCoroutineStopped(ref typeRoutine);
			dialogueVertexAnimator.textAnimating = false;
			List<DialogueCommand> commands = DialogueUtility.ProcessInputString(message, out string totalTextMessage);
			typeRoutine = StartCoroutine(dialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, null));

			dialogueItems.characterImage.sprite = characterImage;
			dialogueItems.characterName.text = displayName ? characterName : "???";
			return true; //Next message shown successfully
		}
	}

	[System.Serializable]
	public struct DialogueManagerItems {
		public Image characterImage;
		public TMP_Text characterName;
		public TMP_Text textBox;
		public Canvas canvas;
	}
}