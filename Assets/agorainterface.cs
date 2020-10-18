using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;

public class agorainterface : MonoBehaviour
{
    private static string appid = "fce15e90dba8416cad27d3cf6d20055f";
    public IRtcEngine mRtcEngine;
    public uint mRemotePeer;
    public void loadEngine () {
        Debug.Log("starting engine");
        if (mRtcEngine != null) {
            Debug.Log("engine exists");
            return;

        }
        mRtcEngine = IRtcEngine.getEngine(appid);
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG);

    }
    public void joinChannel(string channelname) {
        Debug.Log("joining channel " + channelname);

        if (mRtcEngine== null) {
            Debug.Log("engine needs to be initialized");
            return;

        }
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccess;
        mRtcEngine.OnUserJoined = OnUserJoined;
        mRtcEngine.OnUserOffline = OnUserOffline;


        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.JoinChannel(channelname, null, 0);

    }

    public void leaveChannel() {
        Debug.Log("leaving channel");
        if (mRtcEngine == null) {
            Debug.Log("missing engine");
            return;
        }
        mRtcEngine.LeaveChannel();
        mRtcEngine.DisableVideoObserver();

    }
    public void unloadEngine() {
        Debug.Log("unloading engine");
        if (mRtcEngine != null ) {
            IRtcEngine.Destroy();
            mRtcEngine = null;
        }
    }

   /* public void getsdkversion() {
        Debug.Log("getting version");
        GameObject versiontext = GameObject.Find("")
    }*/

    private void OnJoinChannelSuccess (string channelname, uint uid, int elapsed) {
        Debug.Log("success join channel " + channelname + " " + uid);

    } 
    private void OnUserJoined (uint uid, int elapsed) {
        Debug.Log("new person join channel " + " " + uid);
        GameObject go;
        go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        go.name = uid.ToString();
        VideoSurface o = go.AddComponent<VideoSurface>();
        o.SetForUser(uid);
        o.mAdjustTransfrom += OnTransformDelegate;
        o.SetEnable(true);
        o.transform.Rotate(-90.0f, 0.0f, 0.0f);
        float r = Random.Range(-5.0f, 5.0f);
        o.transform.position = new Vector3(0f, r, 0f);
        o.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
        mRemotePeer = uid;

    }

    private void OnUserOffline (uint uid, USER_OFFLINE_REASON reason) {
        Debug.Log("user off line with id " + uid);
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null)) {
            Destroy(go);
        }


    }

    private void OnTransformDelegate (uint uid, string objname, ref Transform transform) {
        if (uid == 0) {
            transform.position = new Vector3(0f, 2f, 0f);
            transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
            transform.Rotate(0f, 1f, 0f);

        } else {
            transform.Rotate(0.0f, 1.0f, 0.0f);
        }
    }
    public void OnChatSceneLoaded() {
        GameObject go = GameObject.Find("Sphere");
        if (ReferenceEquals(go, null)) {
            Debug.Log("cant fiund sphere");
            return;
        }

        VideoSurface o =  go.GetComponent<VideoSurface>();
        o.mAdjustTransfrom += OnTransformDelegate;
    }
}
