using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro{
    [System.Serializable] public class TextRevealEvent : UnityEvent<char> { }
    [System.Serializable] public class DialogueEvent : UnityEvent { }

    public class TMP_Animated : TextMeshProUGUI{

        float speed;

        public TextRevealEvent onTextReveal;
        public DialogueEvent onDialogueFinish;

        public void ReadText(string newText){
            text = string.Empty;

            string[] subTexts = newText.Split('<', '>');

            string displayText = "";
            for (int i = 0; i < subTexts.Length; i++){
                if (i % 2 == 0)
                    displayText += subTexts[i];
                else if (!isCustomTag(subTexts[i].Replace(" ", "")))
                    displayText += $"<{subTexts[i]}>";
            }

            bool isCustomTag(string tag){
                return tag.StartsWith("speed=") || tag.StartsWith("pause=");
            }

            text = displayText;
            maxVisibleCharacters = 0;
            StartCoroutine(Read());

            IEnumerator Read(){
                int subCounter = 0;
                int visibleCounter = 0;
                while(subCounter < subTexts.Length){
                    if(subCounter % 2 == 1){
                        yield return EvaluateTag(subTexts[subCounter].Replace(" ", ""));
                    }else{
                        while(visibleCounter < subTexts[subCounter].Length){
                            onTextReveal.Invoke(subTexts[subCounter][visibleCounter]);
                            visibleCounter++;
                            maxVisibleCharacters++;
                            yield return new WaitForSeconds(1f / speed);
                        }
                        visibleCounter = 0;
                    }
                    subCounter++;
                }
                yield return null;

                WaitForSeconds EvaluateTag(string tag){
                    if (tag.Length > 0){
                        if (tag.StartsWith("speed=")){
                            speed = float.Parse(tag.Split('=')[1]);
                        }else if (tag.StartsWith("pause=")){
                            return new WaitForSeconds(float.Parse(tag.Split('=')[1]));
                        }
                    }
                    return null;
                }

                onDialogueFinish.Invoke();
            }
        }
    }
}
