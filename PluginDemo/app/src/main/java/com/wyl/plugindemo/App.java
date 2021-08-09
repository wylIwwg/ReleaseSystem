package com.wyl.plugindemo;

import android.content.Context;
import android.os.Environment;

import com.qihoo360.replugin.RePlugin;
import com.qihoo360.replugin.RePluginConfig;
import com.sjjd.wyl.baseandroid.base.BaseApp;

/**
 * Created by wyl on 2020/1/9.
 */
public class App extends BaseApp {

    @Override
    protected void attachBaseContext(Context base) {
        super.attachBaseContext(base);
        RePluginConfig mConfig = new RePluginConfig();
        mConfig.setMoveFileWhenInstalling(false);
        RePlugin.App.attachBaseContext(this, mConfig);
    }

    @Override
    public void onCreate() {
        super.onCreate();

        RePlugin.App.onCreate();
        initDebug(null);

        initOkGO();
        initTTs(Environment.getExternalStorageDirectory().getAbsolutePath() + "/sjjd/tts");

        initCrashRestart();

    }

}
