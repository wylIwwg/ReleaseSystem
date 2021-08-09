package com.sjjd.notification;

import android.content.Context;
import android.util.Log;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.Arrays;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * Created by wyl on 2020/1/17.
 */
public class TcpSocket {
    private static final String TAG = " TCPSocket ";
    private static final String LABEL = "\n";
    private ExecutorService mThreadPool;
    private Socket mSocket;
    private BufferedReader br;
    private PrintWriter pw;
    private long lastReceiveTime = 0;
    private Context mContext;
    private final Object mObject = new Object();
    private SocketManager.OnConnectionStateListener mListener;
    private SocketManager.OnMessageReceiveListener mMessageListener;
    private long TIME_OUT = 15 * 1000;//心跳超时时间
    private long HEARTBEAT_RATE = 5 * 1000;//心跳间隔
    private static final long HEARTBEAT_MESSAGE_DURATION = 2 * 1000;//心跳反应时间
    private boolean alive = false;


    public void setTIME_OUT(long TIME_OUT) {
        this.TIME_OUT = TIME_OUT;
    }

    public void setHEARTBEAT_RATE(long HEARTBEAT_RATE) {
        this.HEARTBEAT_RATE = HEARTBEAT_RATE;
    }

    private MsgThread mMsgThread;

    public TcpSocket(Context context) {
        mContext = context;
        int cpuNumbers = Runtime.getRuntime().availableProcessors();
        // 根据CPU数目初始化线程池
        mThreadPool = Executors.newFixedThreadPool(cpuNumbers * 20);
        // 记录创建对象时的时间
        lastReceiveTime = System.currentTimeMillis();
        mMsgThread = new MsgThread();

    }


    public void startTcpSocket(final String ip, final String port) {
        mThreadPool.execute(new Runnable() {
            @Override
            public void run() {
                if (startTcpConnection(ip, Integer.valueOf(port))) {// 尝试建立 TCP 连接
                    if (mListener != null) {
                        mListener.onSuccess();
                    }
                    alive = true;
                    startReceiveTcpThread();//TCP创建成功 开启数据接收线程
                    //  startHeartbeatTimer();//发送心跳
                } else {
                    if (mListener != null) {
                        alive = false;
                        stopTcpConnection();//创建失败 关闭资源
                        // mListener.onFailed(IConfigs.MSG_CREATE_TCP_ERROR);
                    }
                }
            }
        });
    }

    public void setOnConnectionStateListener(SocketManager.OnConnectionStateListener listener) {
        this.mListener = listener;
    }

    public void addOnMessageReceiveListener(SocketManager.OnMessageReceiveListener listener) {
        mMessageListener = listener;
    }

    /**
     * 创建接收线程
     */
    class MsgThread implements Runnable {

        @Override
        public void run() {
            while (alive) {
                String line = "";
                try {
                    Thread.sleep(1000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
                byte[] buffer = new byte[1024 * 1024];
                int length = 0;
                try {
                    if (mSocket == null) {
                        continue;
                    }
                    String message = "";
                    if (mSocket.isClosed()) {
                        continue;
                    }
                    InputStream is = mSocket.getInputStream();
                    synchronized (mObject) {
                        while (alive && mSocket != null && mSocket.isConnected() && !mSocket.isClosed()) {
                            if (alive && mSocket != null && mSocket.isConnected() && !mSocket.isClosed() && ((length = is.read(buffer)) != -1)) {
                                if (length > 0) {
                                    message = new String(Arrays.copyOf(buffer,
                                            length));
                                    handleReceiveTcpMessage(message);
                                }
                            }

                        }
                    }

                } catch (IOException e) {
                    //e.printStackTrace();
                }

            }
        }
    }

    private void startReceiveTcpThread() {

        if (mMsgThread != null)
            mThreadPool.execute(mMsgThread);
    }

    /**
     * 处理 tcp 收到的消息
     *
     * @param line
     */
    String result = "";
    String remainder = "";

    private void handleReceiveTcpMessage(String line) {
        lastReceiveTime = System.currentTimeMillis();

        if (line.contains(LABEL)) {
            int mIndex = line.indexOf(LABEL);//获取标识符索引
            result += line.replace(LABEL, "");

            if (result.startsWith("{") && result.endsWith("}")) {
                if (mMessageListener != null) {
                    Log.e(TAG, "消息" + result);
                    mMessageListener.onMessageReceived(result);
                    result = "";
                }
            }

        } else {
            result += line;
        }


    }

    public void sendTcpMessage(String json) {
        if (pw != null)
            pw.println(json);
    }


    /**
     * 尝试建立tcp连接
     *
     * @param ip
     * @param port
     */
    public boolean startTcpConnection(final String ip, final int port) {
        try {
            if (mSocket == null) {
                mSocket = new Socket(ip, port);
                mSocket.setKeepAlive(true);
                mSocket.setTcpNoDelay(true);
                mSocket.setReuseAddress(true);
            }
            InputStream is = mSocket.getInputStream();
            br = new BufferedReader(new InputStreamReader(is));
            OutputStream os = mSocket.getOutputStream();
            pw = new PrintWriter(new BufferedWriter(new OutputStreamWriter(os)), true);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    public void stopTcpConnection() {
        try {
            alive = false;
            if (br != null) {
                br.close();
            }
            if (pw != null) {
                pw.close();
            }
            if (mThreadPool != null) {
                mThreadPool.shutdown();
                mThreadPool = null;
            }

            if (mSocket != null) {
                mSocket.close();
                mSocket = null;
            }

            if (mMsgThread != null) {
                mMsgThread = null;
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
