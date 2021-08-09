package com.sjjd.notification;

import android.app.Application;
import android.content.Intent;


/**
 * Created by wyl on 2020/1/17.
 */
public class App extends Application {
    @Override
    public void onCreate() {
        super.onCreate();
        Intent mService = new Intent(this, NotificationService.class);
        startService(mService);
    }
}

