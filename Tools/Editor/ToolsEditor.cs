using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.SceneManagement;
using System;

public class ToolsEditor{
    
    [MenuItem("GameObject/Simple Tools/AudioManager", false, 10)]
    static void CreateAudioManager(){
        GameObject audioManager = new GameObject("AudioManager");
        audioManager.AddComponent<AudioManager>();
    }

    [MenuItem("GameObject/Simple Tools/Dialogue System", false, 10)]
    static void CreateDialogueSystem(){
        GameObject dialogueCanvas = new GameObject("DialogueCanvas");
        dialogueCanvas.AddComponent<RectTransform>();
        Canvas canvas = dialogueCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        dialogueCanvas.AddComponent<CanvasScaler>();
        dialogueCanvas.AddComponent<GraphicRaycaster>();

        GameObject text = new GameObject("TMP_Animated");
        text.transform.SetParent(dialogueCanvas.transform);
        text.AddComponent<TMP_Animated>().text = "New Text";
        text.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        GameObject name = new GameObject("NameText");
        name.transform.SetParent(dialogueCanvas.transform);
        name.AddComponent<TextMeshProUGUI>().text = "Name";
        name.GetComponent<RectTransform>().anchoredPosition = Vector2.up * 50f;

        GameObject image = new GameObject("Image");
        image.transform.SetParent(dialogueCanvas.transform);
        image.AddComponent<Image>();
        image.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150f, 25f);

        DialogueSystem dialogueSystem = dialogueCanvas.AddComponent<DialogueSystem>();
        dialogueSystem.nameText = name.GetComponent<TextMeshProUGUI>();
        dialogueSystem.dialogue = text.GetComponent<TMP_Animated>();
        dialogueSystem.faceImage = image.GetComponent<Image>();
        dialogueSystem.nameField = name;
    }

    [MenuItem("GameObject/Simple Tools/Camera Trigger/2D", false, 10)]
    static void CreateCameraTrigger2D(){
        GameObject cameraTrigger = new GameObject("CameraTrigger2D");
        cameraTrigger.AddComponent<BoxCollider2D>();
        cameraTrigger.AddComponent<CMCameraTrigger>();

        GameObject vCam = new GameObject("CM vcam1");
        vCam.transform.SetParent(cameraTrigger.transform);
        vCam.SetActive(false);
        CinemachineVirtualCamera cam = vCam.AddComponent<CinemachineVirtualCamera>();
        cam.m_Lens.Orthographic = true;
    }

    [MenuItem("GameObject/Simple Tools/Camera Trigger/3D", false, 10)]
    static void CreateCameraTrigger3D(){
        GameObject cameraTrigger = new GameObject("CameraTrigger3D");
        cameraTrigger.AddComponent<BoxCollider>();
        cameraTrigger.AddComponent<CMCameraTrigger>();

        GameObject vCam = new GameObject("CM vcam1");
        vCam.transform.SetParent(cameraTrigger.transform);
        vCam.SetActive(false);
        CinemachineVirtualCamera cam = vCam.AddComponent<CinemachineVirtualCamera>();
        cam.m_Lens.FieldOfView = 60f;
    }

#if CINEMACHINE_271_OR_NEWER
    [MenuItem("GameObject/Simple Tools/ScreenShake Camera/2D", false, 10)]
    static void CreateScreenShakeCamera2d(){
        GameObject screenShakeCamera = new GameObject("ScreenShakeCamera");
        CinemachineVirtualCamera vCam = screenShakeCamera.AddComponent<CinemachineVirtualCamera>();
        vCam.m_Lens.ModeOverride = LensSettings.OverrideModes.Orthographic;

        CinemachineBasicMultiChannelPerlin shake = vCam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        NoiseSettings noise = (NoiseSettings)AssetDatabase.LoadAssetAtPath("Packages/com.unity.cinemachine/Presets/Noise/6D Shake.asset", typeof(NoiseSettings));

        shake.m_NoiseProfile = noise;
        shake.m_AmplitudeGain = 0f;
        shake.m_FrequencyGain = 1f;
    }
    [MenuItem("GameObject/Simple Tools/ScreenShake Camera/3D", false, 10)]
    static void CreateScreenShakeCamera3d(){
        GameObject screenShakeCamera = new GameObject("ScreenShakeCamera");
        CinemachineVirtualCamera vCam = screenShakeCamera.AddComponent<CinemachineVirtualCamera>();
        vCam.m_Lens.ModeOverride = LensSettings.OverrideModes.Perspective;

        CinemachineBasicMultiChannelPerlin shake = vCam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        NoiseSettings noise = (NoiseSettings)AssetDatabase.LoadAssetAtPath("Packages/com.unity.cinemachine/Presets/Noise/6D Shake.asset", typeof(NoiseSettings));

        shake.m_NoiseProfile = noise;
        shake.m_AmplitudeGain = 0f;
        shake.m_FrequencyGain = 1f;
    }
#else
    [MenuItem("GameObject/Simple Tools/ScreenShake Camera", false, 10)]
    static void CreateScreenShakeCamera2d(){
        GameObject screenShakeCamera = new GameObject("ScreenShakeCamera");
        CinemachineVirtualCamera vCam = screenShakeCamera.AddComponent<CinemachineVirtualCamera>();

        CinemachineBasicMultiChannelPerlin shake = vCam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        NoiseSettings noise = (NoiseSettings)AssetDatabase.LoadAssetAtPath("Packages/com.unity.cinemachine/Presets/Noise/6D Shake.asset", typeof(NoiseSettings));

        shake.m_NoiseProfile = noise;
        shake.m_AmplitudeGain = 0f;
        shake.m_FrequencyGain = 1f;
    }
#endif

    [MenuItem("Assets/Create/Simple Tools/Create Loading Scene")]
    [MenuItem("Simple Tools/Create Loading Scene")]
    static void CreateLoadingScene(){
        EditorSceneManager.SaveOpenScenes();

        Scene loadingScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        loadingScene.name = "Loading";

        GameObject loaderCallback = new GameObject("LoaderCallback");
        loaderCallback.AddComponent<LoaderCallback>();

        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2Int(951, 535);
        canvasScaler.matchWidthOrHeight = 1f;
        
        canvasObj.AddComponent<GraphicRaycaster>();

        TextMeshProUGUI loadingText = new GameObject("LoadingText").AddComponent<TextMeshProUGUI>();
        loadingText.transform.SetParent(canvasObj.transform);
        RectTransform loadingTextTransform = loadingText.GetComponent<RectTransform>();
        loadingTextTransform.anchoredPosition = new Vector2Int(-333, -212);
        loadingTextTransform.sizeDelta = new Vector2Int(237, 52);
        loadingText.text = "LOADING...";

        Image bg = new GameObject("bg").AddComponent<Image>();
        bg.transform.SetParent(canvasObj.transform);
        RectTransform bgTransform = bg.GetComponent<RectTransform>();
        bgTransform.anchoredPosition = new Vector2Int(0, -235);
        bgTransform.sizeDelta = new Vector2Int(900, 20);
        bg.color = new Color(36f / 255f, 36f / 255f, 36f / 255f);

        Image progressBar = new GameObject("ProgressBar").AddComponent<Image>();
        progressBar.transform.SetParent(bg.transform);
        RectTransform progressBarTransform = progressBar.GetComponent<RectTransform>();
        progressBarTransform.anchoredPosition = Vector2.zero;
        progressBarTransform.sizeDelta = new Vector2Int(900, 20);

        progressBar.sprite = (Sprite)AssetDatabase.LoadAssetAtPath("Packages/com.geri.simpletools/Simple Tools/Editor/Square.png", typeof(Sprite));
        progressBar.type = Image.Type.Filled;
        progressBar.fillMethod = Image.FillMethod.Horizontal;
        progressBar.fillOrigin = (int)Image.OriginHorizontal.Left;
        progressBar.fillAmount = 1f;
        progressBar.gameObject.AddComponent<LoadingProgressBar>();
    }

#if UNITY_2019_3_OR_NEWER
    [MenuItem("Assets/Create/Simple Tools/Create Menu Scene")]
    [MenuItem("Simple Tools/Create Menu Scene")]
    static void CreateMenuScene(){
        EditorSceneManager.SaveOpenScenes();

        Scene menuScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        menuScene.name = "Menu";

        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CreateEventSystem(false, null);

        CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2Int(951, 535);
        canvasScaler.matchWidthOrHeight = 1f;

        canvasObj.AddComponent<GraphicRaycaster>();

        GameObject qualityDropdown = TMP_DefaultControls.CreateDropdown(GetStandardResources());
        qualityDropdown.transform.SetParent(canvasObj.transform);
        RectTransform qualityRectTransform = qualityDropdown.GetComponent<RectTransform>();
        qualityRectTransform.anchoredPosition = Vector2.up * 15f;
        qualityDropdown.name = "QualityDropdown";

        GameObject resolutionDropdown = TMP_DefaultControls.CreateDropdown(GetStandardResources());
        resolutionDropdown.transform.SetParent(canvasObj.transform);
        RectTransform resolutionRectTransform = resolutionDropdown.GetComponent<RectTransform>();
        resolutionRectTransform.anchoredPosition = Vector2.down * 15f;
        resolutionDropdown.name = "ResolutionDropdown";

        GameObject musicSlider;
        using (new FactorySwapToEditor())
            musicSlider = DefaultControls.CreateSlider(GetStandardUIResources());
        musicSlider.transform.SetParent(canvasObj.transform);
        RectTransform musicRectTransform = musicSlider.GetComponent<RectTransform>();
        musicRectTransform.anchoredPosition = Vector2.down * 40f;
        musicSlider.name = "MusicSlider";

        GameObject sfxSlider;
        using (new FactorySwapToEditor())
            sfxSlider = DefaultControls.CreateSlider(GetStandardUIResources());
        sfxSlider.transform.SetParent(canvasObj.transform);
        RectTransform sfxRectTransform = sfxSlider.GetComponent<RectTransform>();
        sfxRectTransform.anchoredPosition = Vector2.down * 60;
        sfxSlider.name = "MusicSlider";

        GameObject playButton = TMP_DefaultControls.CreateButton(GetStandardResources());
        playButton.transform.SetParent(canvasObj.transform);
        TMP_Text playTextComponent = playButton.GetComponentInChildren<TMP_Text>();
        playTextComponent.fontSize = 24;
        playTextComponent.text = "PLAY";
        RectTransform playRectTransform = playButton.GetComponent<RectTransform>();
        playRectTransform.anchoredPosition = Vector2.up * 45f;
        playButton.name = "PlayButton";

        GameObject quitButton = TMP_DefaultControls.CreateButton(GetStandardResources());
        quitButton.transform.SetParent(canvasObj.transform);
        TMP_Text quitTextComponent = quitButton.GetComponentInChildren<TMP_Text>();
        quitTextComponent.fontSize = 24;
        quitTextComponent.text = "QUIT";
        RectTransform quitRectTransform = quitButton.GetComponent<RectTransform>();
        quitRectTransform.anchoredPosition = Vector2.down * 85f;
        quitButton.name = "QuitButton";

        MenuController menuController = canvasObj.AddComponent<MenuController>();
        Slider sliderMusic = menuController.musicSlider = musicSlider.GetComponent<Slider>();
        Slider sliderSfx = menuController.sfxSlider = sfxSlider.GetComponent<Slider>();
        TMP_Dropdown dropdownQuality = menuController.qualityDropdown = qualityDropdown.GetComponent<TMP_Dropdown>();
        TMP_Dropdown dropdownResolution = menuController.resolutionDropdown = resolutionDropdown.GetComponent<TMP_Dropdown>();

        sliderMusic.onValueChanged.AddListener(menuController.SetMusicVolume);
        sliderSfx.onValueChanged.AddListener(menuController.SetSfxVolume);
        dropdownQuality.onValueChanged.AddListener(menuController.SetQuality);
        dropdownResolution.onValueChanged.AddListener(menuController.SetResolution);

        playButton.GetComponent<Button>().onClick.AddListener(menuController.Play);
        quitButton.GetComponent<Button>().onClick.AddListener(menuController.Quit);
    }

    #region CreateUISettings
    const string kUILayerName = "UI";

    const string kStandardSpritePath = "UI/Skin/UISprite.psd";
    const string kBackgroundSpritePath = "UI/Skin/Background.psd";
    const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
    const string kKnobPath = "UI/Skin/Knob.psd";
    const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
    const string kDropdownArrowPath = "UI/Skin/DropdownArrow.psd";
    const string kMaskPath = "UI/Skin/UIMask.psd";

    static TMP_DefaultControls.Resources s_StandardResources;

    static TMP_DefaultControls.Resources GetStandardResources(){
        if (s_StandardResources.standard == null){
            s_StandardResources.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
            s_StandardResources.background = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpritePath);
            s_StandardResources.inputField = AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath);
            s_StandardResources.knob = AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPath);
            s_StandardResources.checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath);
            s_StandardResources.dropdown = AssetDatabase.GetBuiltinExtraResource<Sprite>(kDropdownArrowPath);
            s_StandardResources.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>(kMaskPath);
        }
        return s_StandardResources;
    }
    static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform){
        // Find the best scene view
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null && SceneView.sceneViews.Count > 0)
            sceneView = SceneView.sceneViews[0] as SceneView;

        // Couldn't find a SceneView. Don't set position.
        if (sceneView == null || sceneView.camera == null)
            return;

        // Create world space Plane from canvas position.
        Camera camera = sceneView.camera;
        Vector3 position = Vector3.zero;
        Vector2 localPlanePosition;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition)){
            // Adjust for canvas pivot
            localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
            localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

            localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
            localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

            // Adjust for anchoring
            position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
            position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

            Vector3 minLocalPosition;
            minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
            minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

            Vector3 maxLocalPosition;
            maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
            maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

            position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
            position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
        }

        itemTransform.anchoredPosition = position;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.localScale = Vector3.one;
    }
    static GameObject CreateNewUI(){
        // Root for the UI
        var root = new GameObject("Canvas");
        root.layer = LayerMask.NameToLayer(kUILayerName);
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        root.AddComponent<CanvasScaler>();
        root.AddComponent<GraphicRaycaster>();

        // Works for all stages.
        StageUtility.PlaceGameObjectInCurrentStage(root);
        bool customScene = false;
        PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null){
            root.transform.SetParent(prefabStage.prefabContentsRoot.transform, false);
            customScene = true;
        }

        Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

        // If there is no event system add one...
        // No need to place event system in custom scene as these are temporary anyway.
        // It can be argued for or against placing it in the user scenes,
        // but let's not modify scene user is not currently looking at.
        if (!customScene)
            CreateEventSystem(false);
        return root;
    }
    static void CreateEventSystem(bool select){
        CreateEventSystem(select, null);
    }
    static void CreateEventSystem(bool select, GameObject parent){
        var esys = UnityEngine.Object.FindObjectOfType<EventSystem>();
        if (esys == null){
            var eventSystem = new GameObject("EventSystem");
            GameObjectUtility.SetParentAndAlign(eventSystem, parent);
            esys = eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();

            Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
        }

        if (select && esys != null){
            Selection.activeGameObject = esys.gameObject;
        }
    }
    // Helper function that returns a Canvas GameObject; preferably a parent of the selection, or other existing Canvas.
    static GameObject GetOrCreateCanvasGameObject(){
        GameObject selectedGo = Selection.activeGameObject;

        // Try to find a gameobject that is the selected GO or one if its parents.
        Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
        if (IsValidCanvas(canvas))
            return canvas.gameObject;

        // No canvas in selection or its parents? Then use any valid canvas.
        // We have to find all loaded Canvases, not just the ones in main scenes.
        Canvas[] canvasArray = StageUtility.GetCurrentStageHandle().FindComponentsOfType<Canvas>();
        for (int i = 0; i < canvasArray.Length; i++)
            if (IsValidCanvas(canvasArray[i]))
                return canvasArray[i].gameObject;

        // No canvas in the scene at all? Then create a new one.
        return CreateNewUI();
    }
    static bool IsValidCanvas(Canvas canvas){
        if (canvas == null || !canvas.gameObject.activeInHierarchy)
            return false;

        // It's important that the non-editable canvas from a prefab scene won't be rejected,
        // but canvases not visible in the Hierarchy at all do. Don't check for HideAndDontSave.
        if (EditorUtility.IsPersistent(canvas) || (canvas.hideFlags & HideFlags.HideInHierarchy) != 0)
            return false;

        if (StageUtility.GetStageHandle(canvas.gameObject) != StageUtility.GetCurrentStageHandle())
            return false;

        return true;
    }

    class FactorySwapToEditor : IDisposable{
        DefaultControls.IFactoryControls factory;

        public FactorySwapToEditor(){
            factory = DefaultControls.factory;
            DefaultControls.factory = DefaultEditorFactory.Default;
        }

        public void Dispose(){
            DefaultControls.factory = factory;
        }
    }

    const string kStandardSpritePathDefault = "UI/Skin/UISprite.psd";
    const string kBackgroundSpritePathDefault = "UI/Skin/Background.psd";
    const string kInputFieldBackgroundPathDefault = "UI/Skin/InputFieldBackground.psd";
    const string kKnobPathDefault = "UI/Skin/Knob.psd";
    const string kCheckmarkPathDefault = "UI/Skin/Checkmark.psd";
    const string kDropdownArrowPathDefault = "UI/Skin/DropdownArrow.psd";
    const string kMaskPathDefault = "UI/Skin/UIMask.psd";

    static DefaultControls.Resources s_StandardResourcesDefault;
    static DefaultControls.Resources GetStandardUIResources(){
        if (s_StandardResourcesDefault.standard == null){
            s_StandardResourcesDefault.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePathDefault);
            s_StandardResourcesDefault.background = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpritePathDefault);
            s_StandardResourcesDefault.inputField = AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPathDefault);
            s_StandardResourcesDefault.knob = AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPathDefault);
            s_StandardResourcesDefault.checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPathDefault);
            s_StandardResourcesDefault.dropdown = AssetDatabase.GetBuiltinExtraResource<Sprite>(kDropdownArrowPathDefault);
            s_StandardResourcesDefault.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>(kMaskPathDefault);
        }
        return s_StandardResourcesDefault;
    }
    static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand){
        GameObject parent = menuCommand.context as GameObject;
        bool explicitParentChoice = true;
        if (parent == null){
            parent = GetOrCreateCanvasGameObject();
            explicitParentChoice = false;

            // If in Prefab Mode, Canvas has to be part of Prefab contents,
            // otherwise use Prefab root instead.
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null && !prefabStage.IsPartOfPrefabContents(parent))
                parent = prefabStage.prefabContentsRoot;
        }
        if (parent.GetComponentsInParent<Canvas>(true).Length == 0){
            // Create canvas under context GameObject,
            // and make that be the parent which UI element is added under.
            GameObject canvas = CreateNewUIDefault();
            Undo.SetTransformParent(canvas.transform, parent.transform, "");
            parent = canvas;
        }

        GameObjectUtility.EnsureUniqueNameForSibling(element);

        SetParentAndAlign(element, parent);
        if (!explicitParentChoice) // not a context click, so center in sceneview
            SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

        // This call ensure any change made to created Objects after they where registered will be part of the Undo.
        Undo.RegisterFullObjectHierarchyUndo(parent == null ? element : parent, "");

        // We have to fix up the undo name since the name of the object was only known after reparenting it.
        Undo.SetCurrentGroupName("Create " + element.name);

        Selection.activeGameObject = element;
    }
    static GameObject CreateNewUIDefault(){
        // Root for the UI
        var root = ObjectFactory.CreateGameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        root.layer = LayerMask.NameToLayer(kUILayerName);
        Canvas canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Works for all stages.
        StageUtility.PlaceGameObjectInCurrentStage(root);
        bool customScene = false;
        PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null){
            Undo.SetTransformParent(root.transform, prefabStage.prefabContentsRoot.transform, "");
            customScene = true;
        }

        Undo.SetCurrentGroupName("Create " + root.name);

        // If there is no event system add one...
        // No need to place event system in custom scene as these are temporary anyway.
        // It can be argued for or against placing it in the user scenes,
        // but let's not modify scene user is not currently looking at.
        if (!customScene)
            CreateEventSystem(false);
        return root;
    }
    static void SetParentAndAlign(GameObject child, GameObject parent){
        if (parent == null)
            return;

        Undo.SetTransformParent(child.transform, parent.transform, "");

        RectTransform rectTransform = child.transform as RectTransform;
        if (rectTransform){
            rectTransform.anchoredPosition = Vector2.zero;
            Vector3 localPosition = rectTransform.localPosition;
            localPosition.z = 0;
            rectTransform.localPosition = localPosition;
        }else{
            child.transform.localPosition = Vector3.zero;
        }
        child.transform.localRotation = Quaternion.identity;
        child.transform.localScale = Vector3.one;

        SetLayerRecursively(child, parent.layer);
    }
    static void SetLayerRecursively(GameObject go, int layer){
        go.layer = layer;
        Transform t = go.transform;
        for (int i = 0; i < t.childCount; i++)
            SetLayerRecursively(t.GetChild(i).gameObject, layer);
    }
    class DefaultEditorFactory : DefaultControls.IFactoryControls{
        public static DefaultEditorFactory Default = new DefaultEditorFactory();

        public GameObject CreateGameObject(string name, params Type[] components){
            return ObjectFactory.CreateGameObject(name, components);
        }
    }
    #endregion
#endif
}
