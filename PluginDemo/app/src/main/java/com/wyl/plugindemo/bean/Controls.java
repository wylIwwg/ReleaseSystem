package com.wyl.plugindemo.bean;

/**
 * Created by wyl on 2019/12/13.
 */
public class Controls {
    private String control;//控件类型
    private double Y;//y坐标
    private double X;//x坐标
    private double Width;//控件宽度
    private double Height;//控件高度
    private String text;//文本内容

    private ControlAttribute mAttribute;

    public ControlAttribute getAttribute() {
        return mAttribute;
    }

    public void setAttribute(ControlAttribute attribute) {
        mAttribute = attribute;
    }

    public String getControl() {
        return control == null ? "" : control;
    }

    public void setControl(String control) {
        this.control = control;
    }

    public double getY() {
        return Y;
    }

    public void setY(double y) {
        Y = y;
    }

    public double getX() {
        return X;
    }

    public void setX(double x) {
        X = x;
    }

    public double getWidth() {
        return Width;
    }

    public void setWidth(double width) {
        Width = width;
    }

    public double getHeight() {
        return Height;
    }

    public void setHeight(double height) {
        Height = height;
    }

    public String getText() {
        return text == null ? "" : text;
    }

    public void setText(String text) {
        this.text = text;
    }
}
