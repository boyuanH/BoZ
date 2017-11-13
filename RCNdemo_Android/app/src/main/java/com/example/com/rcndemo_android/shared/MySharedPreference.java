package com.example.com.rcndemo_android.shared;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;

/**
 * Created by srcb03147 on 2017/4/18.
 */
public class MySharedPreference {
    public Context context;
    public SharedPreferences share;
    public MySharedPreference(Context context){
        this.context = context;
        share= context.getSharedPreferences("test",
                Activity.MODE_PRIVATE);
    }

    public void setStringShared(String name,String Value){
        SharedPreferences.Editor editor = share.edit();
        editor.putString(name, Value);
        editor.commit();
    }

    public String getStringShared(String name){

        return share.getString(name,"");
    }

    public void setIntShared(String name,int Value){
        SharedPreferences.Editor editor = share.edit();
        editor.putInt(name, Value);
        editor.commit();
    }
    public int getIntShared(String name){
        return share.getInt(name,0);
    }
}
