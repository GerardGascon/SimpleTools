using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour{

    public static DialogueSystem instance;

    public GameObject nameField = default;
    public TextMeshProUGUI nameText = default;
    public TMP_Animated dialogue = default;
    public Image faceImage = default;

    Queue<string> sentences;
    bool talking;
    Animator anim;

    void Awake(){
        instance = this;
        sentences = new Queue<string>();
        anim = GetComponent<Animator>();
    }

    /// <summary>Start or continue the dialogue
    /// <para>This function returns false if the dialogue has ended.</para>
    /// </summary>
    public bool Dialogue(Dialogue dialogue){
        if(!talking){
            if (dialogue.displayName){
                nameText.text = dialogue.characterName;
                nameField.SetActive(true);
                nameText.gameObject.SetActive(true);
            }else{
                nameField.SetActive(false);
                nameText.gameObject.SetActive(false);
            }

            if (dialogue.characterImage)
                faceImage.sprite = dialogue.characterImage;
            else
                faceImage.sprite = null;

            sentences.Clear();
            if(dialogue.sentences.Length != 0){
                foreach (string sentence in dialogue.sentences){
                    sentences.Enqueue(sentence);
                }
            }else{
                sentences.Enqueue("I am error. No text has been added");
            }
            talking = true;

            if(sentences.Count == 0){
                talking = false;
                return false;
            }

            string sentenceToShow = sentences.Dequeue();
            this.dialogue.ReadText(sentenceToShow);
            anim.SetBool("Talking", true);
            return true;
        }else{
            if (sentences.Count == 0){
                talking = false;
                anim.SetBool("Talking", false);
                return false;
            }

            string sentenceToShow = sentences.Dequeue();
            this.dialogue.ReadText(sentenceToShow);
            return true;
        }
    }
}
