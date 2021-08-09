package com.wyl.plugindemo;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.view.Gravity;
import android.view.View;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.blankj.utilcode.util.ImageUtils;
import com.bumptech.glide.Glide;
import com.lzy.okgo.OkGo;
import com.lzy.okgo.callback.FileCallback;
import com.lzy.okgo.model.Response;
import com.sjjd.wyl.baseandroid.utils.LogUtils;
import com.wyl.plugindemo.bean.ControlAttribute;
import com.wyl.plugindemo.bean.Controls;

import java.io.File;

/**
 * Created by wyl on 2020/1/17.
 */
public class ControlsCore {
    private static final String TAG = " ControlsCore ";

    public static View getView(Context context, Controls control) {
        View mView = null;
        switch (control.getControl()) {
            case "TextView":
                mView = getTextView(context, control);
                break;
            case "ImageView":
                mView = getImageView(context, control);
                break;
        }
        return mView;
    }

    public static TextView getTextView(Context context, Controls control) {
        TextView mTextView = new TextView(context);
        mTextView.setBackgroundColor(context.getResources().getColor(R.color.white));
        RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams((int) control.getWidth(), (int) control.getHeight());
        lp.setMargins((int) control.getX(), (int) control.getY(), 0, 0);
        mTextView.setLayoutParams(lp);
        ControlAttribute mAttribute = control.getAttribute();
        mTextView.setGravity(mAttribute.getGravity() == 1 ? Gravity.CENTER : Gravity.CENTER_VERTICAL);
        mTextView.setBackgroundColor(Color.parseColor(mAttribute.getBackgroundColor()));
        mTextView.setText(mAttribute.getText());
        mTextView.setTextColor(Color.parseColor(mAttribute.getTextColor()));
        mTextView.setTextSize(mAttribute.getTextSize() > 0 ? mAttribute.getTextSize() : 20);

        return mTextView;
    }

    public static ImageView getImageView(Context context, Controls control) {
        ImageView mImageView = new ImageView(context);
        RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams((int) control.getWidth(), (int) control.getHeight());
        lp.setMargins((int) control.getX(), (int) control.getY(), 0, 0);
        mImageView.setLayoutParams(lp);
        ControlAttribute mAttribute = control.getAttribute();
        //先找本地是否有
        File mFile = new File(Config.PROGRAM_PATH, mAttribute.getBackgroundImage().substring(mAttribute.getBackgroundImage().lastIndexOf("/")));
        LogUtils.e(TAG, "getImageView: " + mAttribute.getBackgroundImage());
        if (mFile.exists()) {
            Glide.with(context).load(mAttribute.getBackgroundImage()).centerCrop().into(mImageView);
        } else {
            //否则网络下载 保存
            OkGo.<File>get(mAttribute.getBackgroundImage())
                    .execute(new FileCallback() {
                        @Override
                        public void onSuccess(Response<File> response) {
                            Glide.with(context).load(response.body()).into(mImageView);
                            new Thread(new Runnable() {
                                @Override
                                public void run() {
                                    ImageUtils.save(ImageUtils.getBitmap(response.body()), mFile, Bitmap.CompressFormat.PNG);
                                }
                            }).start();

                        }
                    });
        }
        mImageView.setScaleType(ImageView.ScaleType.FIT_START);

        return mImageView;
    }
}
