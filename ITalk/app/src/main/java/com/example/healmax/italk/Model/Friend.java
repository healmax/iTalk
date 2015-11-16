package com.example.healmax.italk.Model;

/**
 * Created by healmax on 15/10/31.
 */
public class Friend {

    public int sysId;
    public String id;
    public String name;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public int getSysId() {
        return sysId;
    }

    public void setSysId(int sysId) {
        this.sysId = sysId;
    }
}
