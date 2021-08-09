package com.wyl.plugindemo;

import android.animation.ObjectAnimator;
import android.animation.PropertyValuesHolder;
import android.animation.ValueAnimator;
import android.content.Intent;
import android.os.Bundle;
import android.text.TextUtils;
import android.view.View;
import android.view.animation.AlphaAnimation;
import android.view.animation.Animation;
import android.view.animation.AnimationSet;
import android.view.animation.LinearInterpolator;
import android.view.animation.TranslateAnimation;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.sjjd.wyl.baseandroid.base.BaseActivity;
import com.sjjd.wyl.baseandroid.register.RegisterUtils;
import com.sjjd.wyl.baseandroid.utils.DisplayUtil;
import com.sjjd.wyl.baseandroid.utils.LogUtils;
import com.sjjd.wyl.baseandroid.utils.SPUtils;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import es.dmoral.toasty.Toasty;

public class LoginActivity extends BaseActivity {

    private static final String TAG = "  LoginActivity  ";
    @BindView(R.id.imgLogo)
    ImageView mImgLogo;
    @BindView(R.id.tvAppName)
    TextView mTvAppName;
    @BindView(R.id.imgLoginUser)
    ImageView mImgLoginUser;
    @BindView(R.id.etUserName)
    EditText mEtUserName;
    @BindView(R.id.imgDeleteName)
    ImageView mImgDeleteName;
    @BindView(R.id.imgLoginPsw)
    ImageView mImgLoginPsw;
    @BindView(R.id.etUserPsw)
    EditText mEtUserPsw;
    @BindView(R.id.imgDeletePsw)
    ImageView mImgDeletePsw;
    @BindView(R.id.btnLogin)
    Button mBtnLogin;
    @BindView(R.id.llInput)
    LinearLayout mLlInput;
    @BindView(R.id.progressBar2)
    ProgressBar mProgressBar2;
    @BindView(R.id.llprogress)
    LinearLayout mLlprogress;
    @BindView(R.id.btnRegister)
    Button mBtnRegister;


    int waitTime = 2000;
    long startTime = 0;
    String nick, psw;
    @BindView(R.id.imgIp)
    ImageView mImgIp;
    @BindView(R.id.btnGetArea)
    Button mBtnGetArea;
    @BindView(R.id.btnGo)
    Button mBtnGo;
    @BindView(R.id.llSetting)
    LinearLayout mLlSetting;
    @BindView(R.id.view)
    View mView;
    @BindView(R.id.view1)
    View mView1;
    @BindView(R.id.etIp)
    EditText mEtIp;
    @BindView(R.id.etPort)
    EditText mEtPort;
    @BindView(R.id.etSocketPort)
    EditText mEtSocketPort;
    @BindView(R.id.rlInputContainer)
    RelativeLayout mRlInputContainer;
    @BindView(R.id.llBtnContainer)
    LinearLayout mLlBtnContainer;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        ButterKnife.bind(this);
        initUI();
    }

    private void initUI() {
        isStart = false;

        mLlprogress.setVisibility(View.INVISIBLE);

        mLlInput.setAlpha(1.0f);
        mLlInput.setVisibility(View.VISIBLE);

        mImgDeleteName.setVisibility(View.VISIBLE);
        mImgDeletePsw.setVisibility(View.VISIBLE);
        mImgLoginUser.setVisibility(View.VISIBLE);
        mImgLoginPsw.setVisibility(View.VISIBLE);
        mEtUserName.setVisibility(View.VISIBLE);
        mEtUserPsw.setVisibility(View.VISIBLE);

        if (SPUtils.init(mContext).getDIYBoolean(Config.SP_CHECK_IGNORE)) {
            Intent mIntent = new Intent(mContext, MainActivity.class);
            startActivity(mIntent);
            close();
        }

        RegisterUtils.getInstance(mContext).isDeviceRegistered();
    }

    /**
     * 检测ip是否正确
     *
     * @param ip
     * @return
     */
    public boolean checkIP(String ip) {
        if (ip.length() < 7 || ip.length() > 15) {
            LogUtils.e(TAG, "checkIP: 长度" + ip);
            return false;
        }
        String[] mSplit = ip.split("\\.");
        if (mSplit.length != 4) {
            LogUtils.e(TAG, "checkIP: 格式 " + ip + " " + mSplit.length);
            return false;
        }
        for (String str : mSplit) {
            if (Integer.parseInt(str) < 1 || Integer.parseInt(str) > 255) {
                LogUtils.e(TAG, "checkIP: 大小 " + str);
                return false;
            }
        }
        return true;
    }

    /**
     * 检测配置
     */
    public boolean checkSettingLegal() {
        if (TextUtils.isEmpty(mEtIp.getText().toString().trim())
                || TextUtils.isEmpty(mEtPort.getText().toString().trim())
                || TextUtils.isEmpty(mEtSocketPort.getText().toString().trim()))
            return false;
        boolean mIP = checkIP(mEtIp.getText().toString());
        if (!mIP) {
            return false;
        }
        String port = mEtPort.getText().toString().trim();
        if (Integer.parseInt(port) < 1 || Integer.parseInt(port) > 65535) {
            return false;
        }
        port = mEtSocketPort.getText().toString().trim();
        if (Integer.parseInt(port) < 1 || Integer.parseInt(port) > 65535) {
            return false;
        }
        return true;

    }

    @OnClick({R.id.imgDeleteName, R.id.imgDeletePsw, R.id.btnLogin, R.id.btnGo})
    public void onViewClicked(View view) {
        switch (view.getId()) {
            case R.id.btnGo://直接进入
                //先检查设置
                if (checkSettingLegal()) {
                    //进入主界面
                    SPUtils.init(mContext).putDIYString(Config.SP_IP, mEtIp.getText().toString().trim());
                    SPUtils.init(mContext).putDIYString(Config.SP_PORT, mEtPort.getText().toString().trim());
                    SPUtils.init(mContext).putDIYString(Config.SP_PORT2, mEtSocketPort.getText().toString().trim());

                    Intent mIntent = new Intent(mContext, MainActivity.class);
                    startActivity(mIntent);
                    close();

                } else {
                    Toasty.info(mContext, "请正确设置ip和端口！", Toast.LENGTH_SHORT, true).show();
                }
                break;
            case R.id.imgDeleteName:
                mEtUserName.setText("");
                break;
            case R.id.imgDeletePsw:
                mEtUserPsw.setText("");
                break;
            case R.id.btnLogin:
                // 计算出控件的高与宽
                login();
                break;
            case R.id.btnRegister:
                break;
        }
    }

    private void login() {
        final String nick = mEtUserName.getText().toString();
        String psw = mEtUserPsw.getText().toString();

        if (TextUtils.isEmpty(nick)) {
            mEtUserName.setError("请输入用户名！");
            return;
        }
        if (TextUtils.isEmpty(psw)) {
            mEtUserPsw.setError("请输入密码！");
            return;
        }

        mBtnLogin.setText("验证中...");
        mBtnLogin.setEnabled(false);
        mBtnRegister.setEnabled(false);

        startTime = System.currentTimeMillis();
        this.nick = nick;
        this.psw = psw;
        isStart = false;
        startAnimate(mLlInput, false);
        mDataHandler.postDelayed(new Runnable() {
            @Override
            public void run() {
                Toasty.success(mContext, "验证成功！", Toast.LENGTH_SHORT, true).show();
                SPUtils.init(mContext).putDIYBoolean(Config.SP_CHECK_IGNORE, true);
                mEtUserName.setText("");
                mEtUserPsw.setText("");

                mRlInputContainer.setVisibility(View.GONE);
                mLlBtnContainer.setVisibility(View.GONE);

                mLlSetting.setVisibility(View.VISIBLE);

                mEtIp.setText(SPUtils.init(mContext).getDIYString(Config.SP_IP));
                mEtPort.setText(SPUtils.init(mContext).getDIYString(Config.SP_PORT));
                mEtSocketPort.setText(SPUtils.init(mContext).getDIYString(Config.SP_PORT2));
                animAlphaTranslate(mLlSetting, 0, 1, -DisplayUtil.dip2px(mContext, 200), 0);

               /* Intent mIntent = new Intent(mContext, MainActivity.class);
                startActivity(mIntent);
                close();*/

              /*  mLlInput.setVisibility(View.VISIBLE);
                mLlprogress.setVisibility(View.INVISIBLE);
                startAnimate(mLlInput, true);
                mBtnLogin.setEnabled(true);
                mBtnRegister.setEnabled(true);*/
                //   }
            }
        }, 2000);


    }

    @Override
    public void showError(final String error) {
        super.showError(error);
        long t = System.currentTimeMillis() - startTime;
        if (t < waitTime) {
            mDataHandler.postDelayed(new Runnable() {
                @Override
                public void run() {
                    Toast.makeText(mContext, "" + error, Toast.LENGTH_SHORT).show();
                    mLlInput.setVisibility(View.VISIBLE);
                    mLlprogress.setVisibility(View.INVISIBLE);
                    startAnimate(mLlInput, true);
                }
            }, waitTime - t);
        } else {
            Toast.makeText(mContext, "" + error, Toast.LENGTH_SHORT).show();
            mLlInput.setVisibility(View.VISIBLE);
            mLlprogress.setVisibility(View.INVISIBLE);
            startAnimate(mLlInput, true);

        }

        mBtnLogin.setEnabled(true);
        mBtnRegister.setEnabled(true);
    }


    /**
     * 输入框的动画效果
     *
     * @param view 控件
     * @param w    宽
     * @param h    高
     */
    boolean isStart;

    private void animAlphaTranslate(View view, float a1, float a2, float y1, float y2) {
        AlphaAnimation alphaAni = new AlphaAnimation(a1, a2);
        //设置动画执行的时间，单位是毫秒
        alphaAni.setDuration(500);
        TranslateAnimation translateAni = new TranslateAnimation(
                Animation.ABSOLUTE, 0, Animation.ABSOLUTE,
                0, Animation.ABSOLUTE, y1,
                Animation.ABSOLUTE, y2);

        // 设置动画执行的时间，单位是毫秒
        translateAni.setDuration(500);
        // 将缩放动画和旋转动画放到动画插值器
        AnimationSet as = new AnimationSet(false);
        as.addAnimation(alphaAni);
        as.addAnimation(translateAni);
        as.setFillAfter(true);
        // 启动动画
        view.startAnimation(as);
    }

    private void startAnimate(final View view, boolean maxed) {
        PropertyValuesHolder animator;
        PropertyValuesHolder animator2;
        if (maxed) {
            animator = PropertyValuesHolder.ofFloat("scaleX", 0f, 1f);
            animator2 = PropertyValuesHolder.ofFloat("scaleY", 0f, 1f);
        } else {
            animator = PropertyValuesHolder.ofFloat("scaleX", 1f, 0f);
            animator2 = PropertyValuesHolder.ofFloat("scaleY", 1f, 0f);
        }

        ObjectAnimator animator3 = ObjectAnimator.ofPropertyValuesHolder(view, animator, animator2);
        animator3.setDuration(500);
        animator3.setInterpolator(new LinearInterpolator());
        animator3.addUpdateListener(new ValueAnimator.AnimatorUpdateListener() {
            @Override
            public void onAnimationUpdate(ValueAnimator valueAnimator) {
                float value = (Float) valueAnimator.getAnimatedValue();
                mImgDeleteName.setAlpha(value);
                mImgDeletePsw.setAlpha(value);
                mImgLoginUser.setAlpha(value);
                mImgLoginPsw.setAlpha(value);
                mEtUserName.setAlpha(value);
                mEtUserPsw.setAlpha(value);
                if (value == 0 && !isStart) {
                    isStart = true;
                    mLlInput.setVisibility(View.INVISIBLE);
                    mLlprogress.setVisibility(View.VISIBLE);
                    LogUtils.e(TAG, "onAnimationUpdate: 加载 ");
                    progressAnimator(mLlprogress);

                }
            }
        });
        animator3.start();
    }

    /**
     * 出现进度动画
     *
     * @param view
     */
    private void progressAnimator(final View view) {
        PropertyValuesHolder animator = PropertyValuesHolder.ofFloat("scaleX",
                0f, 1f);
        PropertyValuesHolder animator2 = PropertyValuesHolder.ofFloat("scaleY", 0f, 1f);
        ObjectAnimator animator3 = ObjectAnimator.ofPropertyValuesHolder(view,
                animator, animator2);
        animator3.setDuration(500);
        animator3.setInterpolator(new LinearInterpolator());
        // animator3.setInterpolator(new JellyInterpolator());
        animator3.start();

        //注登录
        //mPrestenerMain.login(mContext, nick, psw);

    }

    class JellyInterpolator extends LinearInterpolator {
        private float factor;

        public JellyInterpolator() {
            this.factor = 0.15f;
        }

        @Override
        public float getInterpolation(float input) {
            return (float) (Math.pow(2, -10 * input)
                    * Math.sin((input - factor / 4) * (2 * Math.PI) / factor) + 1);
        }
    }
}
