package com.wyl.androidterminal;

/**
 * Created by wyl on 2019/12/13.
 */
public class Controls {
    public String control;
    public double Y;
    public double X;

    public double Width;

    public double Height;

    public String text;

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
