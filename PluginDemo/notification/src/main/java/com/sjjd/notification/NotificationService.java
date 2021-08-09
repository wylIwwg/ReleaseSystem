package com.sjjd.notification;

import android.annotation.TargetApi;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.os.Handler;
import android.os.IBinder;
import android.os.Message;
import android.support.annotation.Nullable;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.NotificationManagerCompat;
import android.util.Log;


/**
 * Created by wyl on 2020/1/17.
 */
public class NotificationService extends Service {
    SocketManager mSocketManager;

    private NotificationManager manager;
    private Notification.Builder builder;
    private Context context;
    private Notification notification;
    private String TAG = "NotificationService";

    private int id = 1;

    Handler mHandler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            //  super.handleMessage(msg);
            switch (msg.what) {
                case SocketManager.MSG_SOCKET_RECEIVED:
                    //处理socket消息
                    String result = msg.obj == null ? "" : msg.obj.toString();
                    Log.e("socket  ", result);
                    setNotification("Notification", result);
                    break;
            }
        }
    };


    @TargetApi(26)
    public void setNotification(String title, String desc) {

        manager = (NotificationManager) context.getSystemService(context.NOTIFICATION_SERVICE);

       /* 点击通知栏要跳转到哪个地方
       Intent intent = new Intent(context, NotificationService.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        PendingIntent pendingIntent = PendingIntent.getActivity(context, 0, intent, 0);
*/
        builder = new Notification.Builder(context).setContentTitle(title)
                .setContentText(desc)
                // .setContentIntent(pendingIntent)
                .setWhen(System.currentTimeMillis())
                .setAutoCancel(true)
                .setPriority(Notification.PRIORITY_DEFAULT)
                .setVibrate(new long[]{0, 1000, 1000, 1000}) //通知栏消息震动
                .setLights(Color.GREEN, 1000, 2000) //通知栏消息闪灯(亮一秒间隔两秒再亮)
                .setDefaults(NotificationCompat.DEFAULT_ALL)
                .setSmallIcon(R.drawable.ic_launcher);
        NotificationManagerCompat new_nm = NotificationManagerCompat.from(context);
        new_nm.notify(id++, builder.build());  // 第一个参数1具体实现时 需要修改 用于显示不同消息。
    }


    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public void onCreate() {
        super.onCreate();
        context = getApplicationContext();
        mSocketManager = SocketManager.getInstance(this);
        mSocketManager.setHandler(mHandler);
        mSocketManager.startTcpConnection("192.168.2.188", "8282");
    }


}
