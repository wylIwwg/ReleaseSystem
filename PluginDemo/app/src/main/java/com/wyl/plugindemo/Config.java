package com.wyl.plugindemo;

import android.os.Environment;

/**
 * Created by wyl on 2019/3/8.
 */
public interface Config extends com.sjjd.wyl.baseandroid.utils.IConfigs {
    String ACTION_CHANGED = "ACTION_CHANGED";
    String APK_PATH = Environment.getExternalStorageDirectory() + "/sjjd/apk";
    String PROGRAM_PATH = Environment.getExternalStorageDirectory() + "/sjjd/program";
    String ZIP_PATH = Environment.getExternalStorageDirectory() + "/sjjd/zip";

    // String HOST = "http://%1$s:%2$s";
    //String HOST = "http://SjjdDocLine/depart/departAlls";//39.100.88.253
    String HOST = "http:/%1$s:8080";//39.100.88.253


    int MSG_MEDIA_INIT = 17853;


    int MSG_SETTING_CHANGED = 434;
    int MSG_GET_APK = 41241;
    int MSG_REQUEST_APK = 54545;
    int MSG_COMMUNICATION=9001;

    int MSG_REBOOT_LISTENER = 1221;

    int REGISTER_FORBIDDEN = 0;//禁止注册
    int REGISTER_FOREVER = -1;//永久注册
    int REGISTER_LIMIT = 1;//注册时间剩余

    String SP_VOICE_SWICH = "flag";
    String SP_DEPART = "depart";
    String SP_CLINIC = "clinic";
    String SP_SETTING_SCROLL_TIME = "scroll";
    String SP_SETTING_DELAY_TIME = "delay";
    String SP_SETTING_BACK_TIME = "back";
    String SP_SETTING_TRY_TIME = "try";
    String SP_SETTING_START_TIME = "start_time";
    String SP_SETTING_END_TIME = "end_time";

    String SP_FORCED_URL = "forcedurl";
    String SP_FORCED_STATE = "forcedstate";
    String SP_PROGRAM_ID = "proid";
    String SP_APK_ID = "apkid";
    String SP_TARGET_APP = "target";
    String SP_APK_VERSION_CODE = "code";
    String SP_CLIENT_ID = "client_id";
    String SP_PATH_DATA = "source";
    String SP_PATH_DATA_BACKUP = "source_backup";

    String SP_CHECK_IGNORE = "check_ignore";



    String SP_SYNTHESIS_TYPE_B = "synthesis_type";


    String URL_UPLOAD_IMAGE = "/SjjdStaLine/facility/screenShot";


    String URL_ADD_TERMINAL = "/SjjdStaLine/facility/addfacility";
    String URL_GET_REGION = "/SjjdStaLine/depart/departAlls";//获取科室诊室
    String URL_UPDATE_VOICE = "/SjjdStaLine/queue/callsucess?pid=%1$s&queNum=%2$s";//语音播报完成接口  pid 病人id

    String URL_ADD_MSG = "/SjjdMei/upMsg/addUpMsg?upMacid=%1$s&upFotaid=%2$s&upMessage=%3$s";

    String URL_INIT = "/SjjdStaLine/facility/facInit";//初始化


    String URL_GET_REGION_B = "/SjjdStaLine/depart/departAllsB";//获取科室诊室
    String URL_UPDATE_VOICE_B = "/SjjdStaLine/Bqueue/callBsucess?pid=%1$s&queNum=%2$s";//语音播报完成接口  pid 病人id
    String URL_INIT_B = "/SjjdStaLine/Bqueue/facBInit";//初始化


    String URL_GET_PROGRAM = "/SjjdStaLine/template/downloadTem?macid=";

    String URL_ADD_PUSH = "/SjjdStaLine/Push/addPush";
    String URL_REQUEST_APK = "/SjjdDocLine/upMsg/downloadFota?macid=";
    String URL_ADD_APK = "/SjjdDocLine/upMsg/addUpMsg";
    String URL_FLOW = "/SjjdDocLine/facility/flowinfor";//强制推流更新

}
