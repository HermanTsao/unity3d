#region Using
using System;
using UnityEngine;

#endregion

/// <summary>
/// SDK�ӿڻص���
/// </summary>
public class RiseSdkListener : MonoBehaviour {
    /// <summary>
    /// ��ʾ��Ƶ���Ľ���ص��¼�
    /// </summary>
    public static event Action<bool, int> OnRewardAdEvent;

    /// <summary>
    /// ֧���Ľ���ص��¼�
    /// </summary>
    public static event Action<int, int> OnPaymentEvent;

    /// <summary>
    /// facebook��ز����Ľ���ص��¼�
    /// </summary>
    public static event Action<bool, int, int> OnSNSEvent;

    /// <summary>
    /// �����ļ��Ľ���ص��¼�
    /// </summary>
    public static event Action<bool, int, string> OnCacheUrlResult;

    /// <submit or load, success, leader board id, extra data>
    public static event Action<bool, bool, string, string> OnLeaderBoardEvent;

    public static event Action<int, bool, string> OnReceiveServerResult;

    /// <summary>
    /// ��ȡ��̨�Զ���json���ݵĽ���ص��¼�
    /// </summary>
    public static event Action<string> OnReceiveServerExtra;

    /// <summary>
    /// ��ȡ��̨֪ͨ����Ϣ�Ľ���ص��¼�
    /// </summary>
    public static event Action<string> OnReceiveNotificationData;

    private static event Action<RiseSdk.AdEventType> OnAdEvent;

    private static RiseSdkListener _instance;
    private static RiseSdk riseSdk;

    /// <summary>
    /// ��������
    /// </summary>
    public static RiseSdkListener Instance {
        get {
            if (!_instance) {
                // check if there is a IceTimer instance already available in the scene graph
                _instance = FindObjectOfType (typeof (RiseSdkListener)) as RiseSdkListener;

                // nope, create a new one
                if (!_instance) {
                    var obj = new GameObject ("RiseSdkListener");
                    riseSdk = RiseSdk.Instance;
                    _instance = obj.AddComponent<RiseSdkListener> ();
                    DontDestroyOnLoad (obj);
                }
            }

            return _instance;
        }
    }

    void OnApplicationPause (bool pauseStatus) {
        if (pauseStatus) {
            riseSdk.OnPause ();
        }
    }

    void OnApplicationFocus (bool focusStatus) {
        if (focusStatus) {
            riseSdk.OnResume ();
        }
    }

    void OnApplicationQuit () {
        riseSdk.OnStop ();
        riseSdk.OnDestroy ();
    }

    void Awake () {
        riseSdk.OnStart ();
    }

    /// <summary>
    /// ��ʾ��Ƶ���Ľ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���صĽ������</param>
    public void onReceiveReward (string data) {
        string [] results = data.Split ('|');
        bool success = int.Parse (results [0]) == 0;
        int id = int.Parse (results [1]);
        if (OnRewardAdEvent != null && OnRewardAdEvent.GetInvocationList ().Length > 0) {
            OnRewardAdEvent (success, id);
        }
    }

    /// <summary>
    /// ֧���ɹ�����ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="billId">�Ʒѵ�Id</param>
    public void onPaymentSuccess (string billId) {
        int id = int.Parse (billId);
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PAYMENT_RESULT_SUCCESS, id);
        }
    }

    /// <summary>
    /// ֧��ʧ�ܽ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="billId">�Ʒѵ�Id</param>
    public void onPaymentFail (string billId) {
        int id = int.Parse (billId);
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PAYMENT_RESULT_FAILS, id);
        }
    }

    /// <summary>
    /// ֧��ȡ������ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="billId">�Ʒѵ�Id</param>
    public void onPaymentCanceled (string billId) {
        int id = int.Parse (billId);
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PAYMENT_RESULT_CANCEL, id);
        }
    }

    /// <summary>
    /// ����֧��ϵͳ״̬��SDK�Զ����á�
    /// </summary>
    /// <param name="dummy"></param>
    public void onPaymentSystemValid (string dummy) {
        riseSdk.SetPaymentSystemValid (true);
    }

    /// <summary>
    /// ��½faceboook�˻��Ľ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="result">���صĽ������</param>
    public void onReceiveLoginResult (string result) {
        int success = int.Parse (result);
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_LOGIN, 0);
        }
    }

    /// <summary>
    /// ����faceboook���ѵĽ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="result">���صĽ������</param>
    public void onReceiveInviteResult (string result) {
        int success = int.Parse (result);
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_INVITE, 0);
        }
    }

    /// <summary>
    /// faceboook���޵Ľ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="result">���صĽ������</param>
    public void onReceiveLikeResult (string result) {
        int success = int.Parse (result);
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (success == 0, RiseSdk.SNS_EVENT_LIKE, 0);
        }
    }

    /// <summary>
    /// faceboook������ս�Ľ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="result">���صĽ������</param>
    public void onReceiveChallengeResult (string result) {
        int count = int.Parse (result);
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (count > 0, RiseSdk.SNS_EVENT_CHALLENGE, count);
        }
    }

    public void onSubmitSuccess (string leaderBoardTag) {
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (true, true, leaderBoardTag, "");
        }
    }

    public void onSubmitFailure (string leaderBoardTag) {
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (true, false, leaderBoardTag, "");
        }
    }

    public void onLoadSuccess (string data) {
        string [] results = data.Split ('|');
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (false, true, results [0], results [1]);
        }
    }

    public void onLoadFailure (string leaderBoardTag) {
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (false, false, leaderBoardTag, "");
        }
    }

    public void onServerResult (string data) {
        string [] results = data.Split ('|');
        int resultCode = int.Parse (results [0]);
        bool success = int.Parse (results [1]) == 0;
        if (OnReceiveServerResult != null && OnReceiveServerResult.GetInvocationList ().Length > 0) {
            OnReceiveServerResult (resultCode, success, results [2]);
        }
    }

    /// <summary>
    /// �����ļ�����ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onCacheUrlResult (string data) {
        //tag,success,name
        string [] results = data.Split ('|');
        int tag = int.Parse (results [0]);
        bool success = int.Parse (results [1]) == 0;
        if (OnCacheUrlResult != null && OnCacheUrlResult.GetInvocationList ().Length > 0) {
            if (success) {
                OnCacheUrlResult (true, tag, results [2]);
            } else {
                OnCacheUrlResult (false, tag, "");
            }
        }
    }

    /// <summary>
    /// ��ȡ��̨���õ��Զ���json���ݵĻص�����SDK��ʼ����ɣ���һ��ȡ�����ݺ���Զ����ø÷����������Ҫ������ǰ��Ӽ�����
    /// </summary>
    /// <param name="data">���غ�̨���õ��Զ���json���ݣ��磺{"x":"x", "x":8, "x":{x}, "x":[x]}</param>
    public void onReceiveServerExtra (string data) {
        if (OnReceiveServerExtra != null && OnReceiveServerExtra.GetInvocationList ().Length > 0) {
            OnReceiveServerExtra (data);
        }
    }

    /// <summary>
    /// ��ȡ��֪ͨ����Ϣ���ݵĻص�����SDK��ʼ����ɣ���һ��ȡ�����ݺ���Զ����ø÷����������Ҫ������ǰ��Ӽ�����
    /// </summary>
    /// <param name="data">��̨���õ�����</param>
    public void onReceiveNotificationData (string data) {
        if (OnReceiveNotificationData != null && OnReceiveNotificationData.GetInvocationList ().Length > 0) {
            OnReceiveNotificationData (data);
        }
    }

    /// <summary>
    /// ������汻�رյĻص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onFullAdClosed (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            OnAdEvent (RiseSdk.AdEventType.FullAdClosed);
        }
    }

    /// <summary>
    /// ������汻����Ļص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onFullAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            OnAdEvent (RiseSdk.AdEventType.FullAdClicked);
        }
    }

    /// <summary>
    /// ��Ƶ��汻�رյĻص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onVideoAdClosed (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            OnAdEvent (RiseSdk.AdEventType.VideoAdClosed);
        }
    }

    /// <summary>
    /// banner��汻����Ļص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onBannerAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            OnAdEvent (RiseSdk.AdEventType.BannerAdClicked);
        }
    }

    /// <summary>
    /// �����ƹ��汻����Ļص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onCrossAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            OnAdEvent (RiseSdk.AdEventType.CrossAdClicked);
        }
    }
}
