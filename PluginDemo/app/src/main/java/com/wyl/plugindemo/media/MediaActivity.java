package com.wyl.plugindemo.media;

import android.content.Context;
import android.os.Bundle;
import android.widget.TextView;

import com.sjjd.wyl.baseandroid.adapter.CommonAdapter;
import com.sjjd.wyl.baseandroid.base.BaseActivity;
import com.sjjd.wyl.baseandroid.bean.Banner;
import com.sjjd.wyl.baseandroid.socket.SocketManager;
import com.sjjd.wyl.baseandroid.thread.TimeThread;
import com.wyl.plugindemo.R;

import java.io.File;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;
import cn.jzvd.Jzvd;

public class MediaActivity extends BaseActivity {
    Context mContext;
    List<Banner> mBanners = new ArrayList<>();
    @BindView(R.id.banner)
    MediaBanner mMediaBanner;
    @BindView(R.id.tvCover)
    TextView mTvCover;

    TimeThread mTimeThread;
    private SimpleDateFormat mDateFormat;
    private SimpleDateFormat mTimeFormat;
    private SimpleDateFormat mWeekFormat;

    File dir;
    String PATH_DATA;
    CommonAdapter<Banner> mCommonAdapter;
    boolean isInit = false;
    SocketManager mSocketManager;
    String sessionId = "";
    boolean isUping = false;
    boolean stop = false;


    public int scrollTime;//图片滚动时间
    public int delayTime;//间隔多少时间滚动
    public String ip = "";
    public String mac = "";
    public String port = "";
    public String socketport = "";
    int cloudVersionCode;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_media);
        ButterKnife.bind(this);
        Jzvd.WIFI_TIP_DIALOG_SHOWED = true;
        mContext = this;


    }

}
