package com.wyl.plugindemo.bean;

/**
 * Created by wyl on 2020/1/17.
 */
public class ControlAttribute {
    private String text;//文本内容
    private String textColor;//#2232323
    private float textSize = 15;//文字大小
    private String backgroundImage;//背景图片 连接形式
    private String backgroundColor;//背景颜色  #12312312
    private int gravity;//文字方向 1 居中

    public float getTextSize() {
        return textSize;
    }

    public void setTextSize(float textSize) {
        this.textSize = textSize;
    }

    public int getGravity() {
        return gravity;
    }

    public void setGravity(int gravity) {
        this.gravity = gravity;
    }

    public String getText() {
        return text == null ? "" : text;
    }

    public void setText(String text) {
        this.text = text;
    }

    public String getTextColor() {
        return textColor == null ? "#000000" : textColor;
    }

    public void setTextColor(String textColor) {
        this.textColor = textColor;
    }


    public String getBackgroundImage() {
        return backgroundImage == null ? "" : backgroundImage;
    }

    public void setBackgroundImage(String backgroundImage) {
        this.backgroundImage = backgroundImage;
    }

    public String getBackgroundColor() {
        return backgroundColor == null ? "#ffffff" : backgroundColor;
    }

    public void setBackgroundColor(String backgroundColor) {
        this.backgroundColor = backgroundColor;
    }
}
