package com.sjjd.notification;

import android.content.Context;
import android.os.Handler;
import android.os.Message;
import android.text.TextUtils;
import android.util.Log;

import org.json.JSONException;
import org.json.JSONObject;

/**
 * Created by wyl on 2020/1/17.
 */
public class SocketManager {
    public static final int MSG_SOCKET_RECEIVED = 20000;
    private static volatile SocketManager instance = null;
    private static String TAG = "SocketManager";
    private TcpSocket tcpSocket;
    private Context mContext;
    private Handler mHandler;
    private String IP;
    private String PORT;
    private String PING = "";
    private int delayRequest = 5000;


    public void setPING(String ping) {
        this.PING = ping;
    }

    public String getPING() {
        return PING == null ? "" : PING;
    }

    public void setDelayRequest(int delayRequest) {
        this.delayRequest = delayRequest;
    }

    private SocketManager(Context context) {
        mContext = context.getApplicationContext();
    }

    public static SocketManager getInstance(Context context) {
        if (instance == null) {
            synchronized (SocketManager.class) {
                if (instance == null) {
                    instance = new SocketManager(context);
                }
            }
        }
        return instance;
    }

    public SocketManager setHandler(Handler handler) {
        mHandler = handler;
        return this;
    }


    public TcpSocket getTcpSocket() {
        if (tcpSocket == null) {
            tcpSocket = new TcpSocket(mContext);
        }
        return tcpSocket;
    }

    /**
     * 处理 udp 收到的消息
     *
     * @param message
     */
    private void handleUdpMessage(String message) {
        try {
            JSONObject jsonObject = new JSONObject(message);
            String ip = jsonObject.optString(IP);
            String port = jsonObject.optString(PORT);
            if (!TextUtils.isEmpty(ip) && !TextUtils.isEmpty(port)) {
                startTcpConnection(ip, port, PING);
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public synchronized void startTcpConnection(String ip, String port) {
        startTcpConnection(ip, port, "");
    }

    /**
     * 开始 TCP 连接
     *
     * @param ip
     * @param port
     */
    public synchronized void startTcpConnection(String ip, String port, String ping) {
        IP = ip;
        PORT = port;
        PING = ping;
        tcpSocket = new TcpSocket(mContext);
        if (tcpSocket != null) {
            //先设置监听
            tcpSocket.setOnConnectionStateListener(new OnConnectionStateListener() {
                @Override
                public void onSuccess() {// tcp 创建成功
                    //udpSocket.stopHeartbeatTimer();
                    Log.e(TAG, "连接成功");
                }

                @Override
                public void onFailed(int errorCode) {// tcp 异常处理

                   /* switch (errorCode) {
                        case IConfigs.MSG_CREATE_TCP_ERROR:
                            tcpSocket = null;
                            if (mHandler != null) {
                                mHandler.sendEmptyMessage(IConfigs.MSG_CREATE_TCP_ERROR);
                                //延迟时间去连接
                                mHandler.postDelayed(new Runnable() {
                                    @Override
                                    public void run() {
                                        startTcpConnection(IP, PORT, PING);
                                    }
                                }, delayRequest);
                            }
                            break;
                        case IConfigs.MSG_PING_TCP_TIMEOUT:
                            tcpSocket = null;
                            if (mHandler != null) {
                                mHandler.sendEmptyMessage(IConfigs.MSG_PING_TCP_TIMEOUT);
                                mHandler.postDelayed(new Runnable() {
                                    @Override
                                    public void run() {
                                        startTcpConnection(IP, PORT, PING);
                                    }
                                }, delayRequest);
                            }
                            break;
                    }*/
                }
            });
            tcpSocket.addOnMessageReceiveListener(new OnMessageReceiveListener() {
                @Override
                public void onMessageReceived(String message) {
                    if (mHandler != null) {
                        Message msg = Message.obtain();
                        msg.what = MSG_SOCKET_RECEIVED;
                        msg.obj = message;
                        mHandler.sendMessage(msg);
                        Log.e("onMessageReceived", message);
                    } else {
                        //LogUtils.e(TAG, "onMessageReceived: " + message);
                    }
                }
            });
            //再连接
            tcpSocket.startTcpSocket(ip, port);
        }


    }

    public void destroy() {
        stopSocket();
        instance = null;
    }

    public void stopSocket() {

        if (tcpSocket != null) {
            tcpSocket.stopTcpConnection();
            tcpSocket = null;
        }
    }

    public interface OnMessageReceiveListener {
        void onMessageReceived(String message);
    }


    public interface OnConnectionStateListener {
        void onSuccess();

        void onFailed(int error);
    }
}
