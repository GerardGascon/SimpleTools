using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour{

    Image image;

    // Start is called before the first frame update
    void Awake(){
        image = transform.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update(){
        image.fillAmount = Loader.GetLoadingProgress();
    }
}
