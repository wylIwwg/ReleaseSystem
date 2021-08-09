package com.wyl.plugindemo;

import android.os.Message;
import android.os.RemoteException;

import com.sjjd.wyl.baseandroid.base.BaseDataHandler;
import com.sjjd.wyl.baseandroid.utils.LogUtils;

/**
 * Created by wyl on 2020/1/10.
 */
public class HostBinder extends IDemoAidl.Stub {

    private static final String TAG = " HostBinder  ";
    private BaseDataHandler mHandler;

    public HostBinder(BaseDataHandler handler) {
        mHandler = handler;
    }

    @Override
    public void Communication(String json) throws RemoteException {
        if (mHandler != null) {
            LogUtils.e(TAG, "Communication: "+json );
            Message mMessage = mHandler.obtainMessage();
            mMessage.obj = json;
            mMessage.what = Config.MSG_COMMUNICATION;
            mHandler.sendMessage(mMessage);
        }
    }
}