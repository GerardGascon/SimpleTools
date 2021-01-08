using UnityEngine;

public class LoaderCallback : MonoBehaviour{

    bool isFirstUpdate = true;

    // Update is called once per frame
    void Update(){
        if (isFirstUpdate){
            isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
