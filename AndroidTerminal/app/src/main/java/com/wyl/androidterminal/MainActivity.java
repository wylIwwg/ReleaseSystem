package com.wyl.androidterminal;

import android.os.Bundle;
import android.os.Message;
import android.view.Gravity;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONObject;
import com.sjjd.wyl.baseandroid.base.BaseActivity;
import com.sjjd.wyl.baseandroid.socket.SocketManager;
import com.sjjd.wyl.baseandroid.socket.UDPSocket;
import com.sjjd.wyl.baseandroid.utils.IConfigs;
import com.sjjd.wyl.baseandroid.utils.LogUtils;

import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;

public class MainActivity extends BaseActivity {


    SocketManager mSocketManager;
    @BindView(R.id.rlRoot)
    RelativeLayout mRlRoot;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        ButterKnife.bind(this);

        initData();

    }

    private UDPSocket socket;

    @Override
    public void initData() {
        super.initData();
       /* mSocketManager = SocketManager.getInstance(mContext).setHandler(mDataHandler);
        mSocketManager.startTcpConnection("192.168.2.188", "8282");
        mSocketManager.getTcpSocket().setPING("ping");*/
        socket = new UDPSocket(this);
        socket.startUDPSocket();
        socket.sendMessage("Android 123123");

    }

    @Override
    public void userHandler(Message msg) {
        super.userHandler(msg);
        LogUtils.e(TAG, "userHandler: " + (msg.obj == null ? "" : msg.obj.toString()));
        switch (msg.what) {
            case IConfigs.MSG_SOCKET_RECEIVED:
                String message = (String) msg.obj;
                JSONObject jo = JSON.parseObject(message, JSONObject.class);

                LogUtils.e(TAG, "userHandler2: " + message);
                List<Controls> mControls = JSON.parseArray(jo.get("data").toString(), Controls.class);
                mRlRoot.removeAllViews();
                for (Controls c : mControls) {
                    TextView mTextView = new TextView(mContext);
                    mTextView.setBackgroundColor(getResources().getColor(R.color.white));
                    RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams((int) c.getWidth(), (int) c.getHeight());
                    lp.setMargins((int) c.getX(), (int) c.getY(), 0, 0);
                    mTextView.setLayoutParams(lp);
                    ;
                    mTextView.setGravity(Gravity.CENTER);
                    mTextView.setText(c.getText());
                    mRlRoot.addView(mTextView);
                }
                break;
        }

    }
}
