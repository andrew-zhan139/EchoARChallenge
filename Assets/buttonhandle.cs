using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if(UNITY_2018_3_OR_NEWER)
using UnityEngine.Android;
#endif
 
public class buttonhandle : MonoBehaviour
{
    static agorainterface app = null; 
    // Start is called before the first frame update
    void Start()
    {
#if (UNITY_2018_3_OR_NEWER)     
        if (Permission.HasUserAuthorizedPermission(Permission.Microphone)) {

        } else {
            Permission.RequestUserPermission(Permission.Microphone);
        }

        if (Permission.HasUserAuthorizedPermission(Permission.Camera)) {

        } else {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick (){
        Debug.Log("Button Clicked: " + name);
        if (name.CompareTo("joinbutton") == 0) {

        } else if (name.CompareTo("leavebutton") == 0) {

        }
    }

    private void OnJoinButtonClicked () {
        Debug.Log("join clicked");

        GameObject go = GameObject.Find("channelname");
        InputField input = go.GetComponent<InputField>();

        if (ReferenceEquals(app, null)) {
            app = new agorainterface();
            app.loadEngine();
        }

        app.joinChannel(input.text);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        SceneManager.LoadScene("callscreen", LoadSceneMode.Single);
        
    }
    private void OnLeaveButtonClicked () {
        Debug.Log("leave clicked");
        if (ReferenceEquals(app, null)) {
            app.leaveChannel();
            app.unloadEngine();
            app = null;
            SceneManager.LoadScene("JoinScreen", LoadSceneMode.Single);

        }
    }

    public void OnSceneFinishedLoading (Scene scene, LoadSceneMode mode) {
        if (scene.name.CompareTo("callscreen") == 0) {
            if (!ReferenceEquals(app, null)) {
                app.OnChatSceneLoaded();
            }
            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }
    }
    
}
