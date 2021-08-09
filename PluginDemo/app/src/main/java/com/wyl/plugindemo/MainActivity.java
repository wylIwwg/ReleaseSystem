/*
 * Copyright (C) 2005-2017 Qihoo 360 Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed To in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */

package com.wyl.plugindemo;

import android.content.Context;
import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.os.Message;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.RelativeLayout;

import com.alibaba.fastjson.JSON;
import com.alibaba.fastjson.JSONObject;
import com.blankj.utilcode.util.FileIOUtils;
import com.bumptech.glide.Glide;
import com.lzy.okgo.OkGo;
import com.lzy.okgo.model.Progress;
import com.lzy.okgo.request.GetRequest;
import com.lzy.okserver.OkDownload;
import com.lzy.okserver.download.DownloadListener;
import com.qihoo360.replugin.RePlugin;
import com.qihoo360.replugin.model.PluginInfo;
import com.sjjd.wyl.baseandroid.adapter.CommonAdapter;
import com.sjjd.wyl.baseandroid.adapter.ViewHolder;
import com.sjjd.wyl.baseandroid.base.BaseActivity;
import com.sjjd.wyl.baseandroid.bean.Banner;
import com.sjjd.wyl.baseandroid.socket.SocketManager;
import com.sjjd.wyl.baseandroid.tts.TTSManager;
import com.sjjd.wyl.baseandroid.utils.AppUtils;
import com.sjjd.wyl.baseandroid.utils.IConfigs;
import com.sjjd.wyl.baseandroid.utils.LogUtils;
import com.sjjd.wyl.baseandroid.utils.SPUtils;
import com.sjjd.wyl.baseandroid.view.ItemScrollLayoutManager;
import com.unisound.client.SpeechConstants;
import com.unisound.client.SpeechSynthesizer;
import com.unisound.client.SpeechSynthesizerListener;
import com.wyl.plugindemo.bean.BannerData;
import com.wyl.plugindemo.bean.Controls;
import com.wyl.plugindemo.media.MediaBanner;
import com.wyl.plugindemo.media.player.SimpleJZPlayer;
import com.yanzhenjie.permission.runtime.Permission;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;
import cn.jzvd.Jzvd;

/**
 * 1、启动socket连接 ×连接失败 处理提示   连接成功
 * 2、处理socket消息
 */
public class MainActivity extends BaseActivity {
    public String TAG = this.getClass().getSimpleName();

    Context pluginContext;

    String ip, port, scport;

    SocketManager mSocketManager;

    @BindView(R.id.rlRoot)
    RelativeLayout mRlRoot;

    //banner轮播
    List<Banner> mBanners;
    CommonAdapter mCommonAdapter;
    MediaBanner mBanner;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        ButterKnife.bind(this);

        PERMISSIONS = new String[]{Permission.WRITE_EXTERNAL_STORAGE, Permission.READ_EXTERNAL_STORAGE, Permission.READ_PHONE_STATE};

        if (Build.VERSION.SDK_INT >= 23) {
            hasPermission();
        } else {
            initData();
        }

    }


    @Override
    public void initData() {
        super.initData();


        ip = SPUtils.init(mContext).getDIYString(Config.SP_IP);
        port = SPUtils.init(mContext).getDIYString(Config.SP_PORT);
        scport = SPUtils.init(mContext).getDIYString(Config.SP_PORT2);

        mSocketManager = SocketManager.getInstance(mContext);
        mSocketManager.setHandler(mDataHandler);
        mSocketManager.startTcpConnection(ip, scport, "{\"type\":\"ping\"}");

        RePlugin.registerGlobalBinder("host", new HostBinder(mDataHandler));

        checkPlugin();

    }


    @Override
    public void userHandler(Message msg) {
        super.userHandler(msg);

        switch (msg.what) {
            case IConfigs.MSG_CREATE_TCP_ERROR:
                showError("服务器连接失败！");

                break;
            case IConfigs.MSG_PING_TCP_TIMEOUT:
                showError("服务器连接超时！");

                break;

            case Config.MSG_COMMUNICATION://插件通信
                LogUtils.e(TAG, "userHandler: 这是来自插件的消息");

                break;
            case Config.MSG_SOCKET_RECEIVED://socket消息处理
                if (msg.obj == null)
                    return;
                String obj = msg.obj.toString();
                LogUtils.e(TAG, "userHandler2: " + obj);
                JSONObject jo = JSON.parseObject(obj, JSONObject.class);
                String mType = jo.getString("type");
                if (mType == null)
                    return;
                switch (mType) {
                    case "plugin"://下发插件
                        String pluginLink = jo.getString("url");
                        downloadPlugin(pluginLink);

                        break;
                    case "custom"://自定义的展示
                        List<Controls> mControls = JSON.parseArray(jo.get("data").toString(), Controls.class);
                        mRlRoot.removeAllViews();
                        for (Controls c : mControls) {
                            mRlRoot.addView(ControlsCore.getView(mContext, c));
                        }
                        File mFile1 = new File(Config.PROGRAM_PATH, "data.txt");
                        FileIOUtils.writeFileFromString(mFile1, obj);
                        break;
                    case "banner"://banner轮播图
                        BannerData mData = JSON.parseObject(jo.getString("data"), BannerData.class);
                        mRlRoot.removeAllViews();
                        initBanner(mData);
                        RelativeLayout.LayoutParams l = new RelativeLayout.LayoutParams(-1, -1);
                        mBanner.setLayoutParams(l);
                        mRlRoot.addView(mBanner);
                        File mFile = new File(Config.PROGRAM_PATH, "data.txt");
                        FileIOUtils.writeFileFromString(mFile, obj);
                        break;
                }
                break;


        }
    }

    /**
     * 下载安装插件
     *
     * @param pluginLink
     */
    private void downloadPlugin(String pluginLink) {
        GetRequest<File> mRequest = OkGo.<File>get(pluginLink);
        OkDownload.getInstance().removeAll(true);
        OkDownload.request(pluginLink, mRequest)
                .folder(Config.APK_PATH)//adui/a3
                .fileName(pluginLink.substring(pluginLink.lastIndexOf("/")))
                .save()
                .register(new DownloadListener("DownloadListener") {
                    @Override
                    public void onStart(Progress progress) {

                    }

                    @Override
                    public void onProgress(Progress progress) {

                    }

                    @Override
                    public void onError(Progress progress) {
                        showError(progress.exception.getMessage());
                    }

                    @Override
                    public void onFinish(File file, Progress progress) {
                        new Thread(new Runnable() {
                            @Override
                            public void run() {
                                PluginInfo info = null;
                                LogUtils.e(TAG, "run: " + file.getAbsolutePath());
                                //关闭进程
                                PluginInfo mPlugin = RePlugin.getPluginInfo(SPUtils.init(mContext).getDIYString("plugin"));
                                RePlugin.isPluginRunning(SPUtils.init(mContext).getDIYString("plugin"));
                                if (file.exists()) {
                                    info = RePlugin.install(file.getAbsolutePath());
                                    if (info != null) {
                                        SPUtils.init(mContext).putDIYString("plugin", info.getName());
                                        if (RePlugin.preload(info))
                                            //checkPlugin();
                                            mDataHandler.postDelayed(new Runnable() {
                                                @Override
                                                public void run() {
                                                    AppUtils.restartApp(mContext);
                                                }
                                            }, 3000);
                                    }
                                }
                            }
                        }).start();
                    }

                    @Override
                    public void onRemove(Progress progress) {

                    }
                })//
                .start();
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        close();
    }

    private void initBanner(BannerData data) {
        mBanner = new MediaBanner(mContext);
        mBanners = new ArrayList<>();
        List<String> mUrls = data.getUrls();

        mCommonAdapter = new CommonAdapter<Banner>(mContext, R.layout.item_video, mBanners) {
            @Override
            protected void convert(ViewHolder holder, Banner banner, int position) {
                SimpleJZPlayer mPlayer = holder.getView(R.id.videoplayer);
                LogUtils.e(TAG, "convert: " + banner.getUrl());
                if ("0".equals(banner.getType())) {
                    Glide.with(mContext).load(banner.getUrl()).into(mPlayer.thumbImageView);

                } else {
                    mPlayer.setUp(banner.getUrl(), "", Jzvd.SCREEN_FULLSCREEN);
                }
            }
        };

        ItemScrollLayoutManager mLayoutManager = new ItemScrollLayoutManager(mContext, LinearLayoutManager.HORIZONTAL);
        mLayoutManager.setScrollTime(200);
        mBanner.setLayoutManager(mLayoutManager);
        mBanner.setAdapter(mCommonAdapter);
        // mBanner.setTypeTime(MediaBanner.ROLL_PIXEL,30);
        mBanner.setTypeTime(MediaBanner.ROLL_ITEM, data.getScrollDelay() * 1000);
        mBanner.addOnScrollListener(new RecyclerView.OnScrollListener() {
            @Override
            public void onScrollStateChanged(RecyclerView recyclerView, int newState) {
                super.onScrollStateChanged(recyclerView, newState);

                LinearLayoutManager manager = (LinearLayoutManager) recyclerView.getLayoutManager();
                // 当不滚动时
                if (newState == RecyclerView.SCROLL_STATE_IDLE) {
                    //获取最后一个完全显示的ItemPosition
                    int mVisibleItemPosition = manager.findLastCompletelyVisibleItemPosition();
                    LogUtils.e(TAG, "" + mVisibleItemPosition);
                    int index = mVisibleItemPosition % mBanners.size();
                    if (index < mBanners.size() && index > 0) {
                        Banner b = mBanners.get(index);
                        if ("1".equals(b.getType())) {
                            View mView = mBanner.getChildAt(0);
                            if (mView != null) {
                                SimpleJZPlayer mPlayer = mView.findViewById(R.id.videoplayer);
                                mPlayer.startButton.performClick();
                                mBanner.stop();
                                mPlayer.setOnCompleteListener(new SimpleJZPlayer.CompleteListener() {
                                    @Override
                                    public void complete(SimpleJZPlayer player, String url, int screen) {
                                        Jzvd.releaseAllVideos();
                                        mBanner.start();
                                    }
                                });
                            }
                        }
                    }

                }
            }

        });
        OkDownload.getInstance().removeAll(true);
        for (String url : mUrls) {
            //url  http://192.168.2.xx/aaa.png;
            LogUtils.e(TAG, "initBanner: " + url);
            String name = url.substring(url.lastIndexOf("/"));
            File mFile = new File(Config.PROGRAM_PATH, name);
            if (mFile.exists()) {
                String mPath = mFile.getAbsolutePath().toLowerCase();
                Banner b = new Banner();
                b.setUrl(mPath);
                if (mPath.endsWith("jpg") || mPath.endsWith("jpeg") || mPath.endsWith("png")) {
                    b.setType("0");
                    mBanners.add(b);
                } else if (mPath.endsWith("mp4") || mPath.endsWith("avi") || mPath.endsWith("flv")) {
                    b.setType("1");
                    mBanners.add(b);
                }
            } else {

                GetRequest<File> mRequest = OkGo.<File>get(url);
                OkDownload.request(url, mRequest)//
                        //.extra1(file)
                        // .extra2(file.getIcon())
                        .folder(Config.PROGRAM_PATH)//adui/a3
                        .fileName(name)
                        .save()//
                        .register(new DownloadListener("DownloadListener") {
                            @Override
                            public void onStart(Progress progress) {

                            }

                            @Override
                            public void onProgress(Progress progress) {

                            }

                            @Override
                            public void onError(Progress progress) {

                            }

                            @Override
                            public void onFinish(File file, Progress progress) {
                                String mPath = file.getAbsolutePath().toLowerCase();
                                LogUtils.e(TAG, "onFinish: " + mPath);
                                Banner b = new Banner();
                                b.setUrl(mPath);
                                if (mPath.endsWith("jpg") || mPath.endsWith("jpeg") || mPath.endsWith("png")) {
                                    b.setType("0");
                                    mBanners.add(b);
                                } else if (mPath.endsWith("mp4") || mPath.endsWith("avi") || mPath.endsWith("flv")) {
                                    b.setType("1");
                                    mBanners.add(b);

                                }
                                LogUtils.e(TAG, "onFinish: " + mBanners.size() + "  " + mUrls.size());
                                if (mBanners.size() == mUrls.size()) {
                                    mCommonAdapter.setLoop(true);
                                    mCommonAdapter.notifyDataSetChanged();
                                    mBanner.start();
                                }
                            }

                            @Override
                            public void onRemove(Progress progress) {

                            }
                        })//
                        .start();

            }
        }

        LogUtils.e(TAG, "  : " + mBanners.size() + "  " + mUrls.size());
        if (mBanners.size() == mUrls.size()) {
            mCommonAdapter.setLoop(true);
            mCommonAdapter.notifyDataSetChanged();
            mBanner.start();
        }
    }

    //判断是否安装插件
    public void checkPlugin() {

        new Thread(new Runnable() {
            @Override
            public void run() {
                String mPlugin = SPUtils.init(mContext).getDIYString("plugin");
                if (RePlugin.isPluginInstalled(mPlugin)) {
                    pluginContext = RePlugin.fetchContext(mPlugin);
                    Intent mDemo = RePlugin.createIntent(mPlugin, RePlugin.getPluginInfo(mPlugin).getPackageName() + ".MainActivity");
                    RePlugin.startActivity(MainActivity.this, mDemo);
                } else {
                    //Toasty.error(mContext, "插件未安装!", Toast.LENGTH_LONG, true).show();
                    File mFile = new File(Config.PROGRAM_PATH, "data.txt");
                    if (mFile.exists()) {
                        mDataHandler.postDelayed(new Runnable() {
                            @Override
                            public void run() {
                                Message mObtain = Message.obtain();
                                mObtain.what = Config.MSG_SOCKET_RECEIVED;
                                mObtain.obj = com.sjjd.wyl.baseandroid.utils.FileUtils.readJson(mFile.getAbsolutePath());
                                mDataHandler.sendMessage(mObtain);
                            }
                        }, 200);
                    }
                }
            }
        }).start();
    }


    @Override
    public void initListener() {
        super.initListener();
        initTtsListener();

    }

    /***语音呼叫模块*/
    SpeechSynthesizer mTTSPlayer;//呼叫播放
    boolean isSpeeking = false;

    private void initTtsListener() {

        if (mTTSPlayer == null) {
            mTTSPlayer = TTSManager.getInstance(mContext).getTTSPlayer();
            if (mTTSPlayer == null) {
                TTSManager.getInstance(mContext).initTts(mContext);
                mTTSPlayer = TTSManager.getInstance(mContext).getTTSPlayer();
            }
        }
        mTTSPlayer.setTTSListener(new SpeechSynthesizerListener() {
            @Override
            public void onEvent(int type) {
                switch (type) {
                    case SpeechConstants.TTS_EVENT_PLAYING_START:
                        isSpeeking = true;
                        break;
                    case SpeechConstants.TTS_EVENT_PLAYING_END:
                        LogUtils.e(TAG, "onEvent: TTS_EVENT_PLAYING_END");
                        isSpeeking = false;

                        Intent mIntent = new Intent("host_receiver");
                        mIntent.putExtra("type", " HOST");
                        sendBroadcast(mIntent);
                        break;
                }
            }

            @Override
            public void onError(int i, String s) {
            }
        });
    }

    private synchronized void ttsSpeak(String txt) {
        if (txt != null && mTTSPlayer != null) {
            isSpeeking = true;
            mTTSPlayer.playText(txt);
            LogUtils.e(TAG, "TTSspeak: " + txt);
        }

    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
    }


    /**语音模块*/

    /**扫描模块*/

    /**打印*/
}
