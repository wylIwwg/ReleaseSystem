package com.wyl.plugindemo.bean;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by wyl on 2020/1/19.
 */
public class BannerData {

    private List<String> urls;
    private int srollTime;
    private int scrollDelay;

    public List<String> getUrls() {
        if (urls == null) {
            return new ArrayList<>();
        }
        return urls;
    }

    public void setUrls(List<String> urls) {
        this.urls = urls;
    }

    public int getSrollTime() {
        return srollTime;
    }

    public void setSrollTime(int srollTime) {
        this.srollTime = srollTime;
    }

    public int getScrollDelay() {
        return scrollDelay;
    }

    public void setScrollDelay(int scrollDelay) {
        this.scrollDelay = scrollDelay;
    }
}
