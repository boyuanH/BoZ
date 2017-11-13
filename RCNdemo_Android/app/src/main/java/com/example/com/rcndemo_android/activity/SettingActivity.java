package com.example.com.rcndemo_android.activity;

import android.app.Activity;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.example.com.rcndemo_android.Obj.Msg;
import com.example.com.rcndemo_android.R;
import com.example.com.rcndemo_android.shared.MySharedPreference;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class SettingActivity extends Activity implements View.OnClickListener{

    private EditText IpEdit,PortEdit,TitleEdit;
    private Button reset_Btn,confirm_Btn;
    MySharedPreference mySharedPreference;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_setting);
        TitleEdit = (EditText) findViewById(R.id.TitleEdit);
        IpEdit = (EditText)findViewById(R.id.IpEdit);
        PortEdit = (EditText) findViewById(R.id.PortEdit);
        reset_Btn = (Button) findViewById(R.id.reset_Btn);
        confirm_Btn = (Button) findViewById(R.id.confirm_Btn);
        mySharedPreference = new MySharedPreference(this);
        String hostName = mySharedPreference.getStringShared(Msg.IP);
        if(hostName!=null){
            if(!hostName.equals("")){
                IpEdit.setText(hostName);
            }
        }
        String portstr = mySharedPreference.getStringShared(Msg.PORT);
        if(portstr!=null){
            if(!portstr.equals("")){
                PortEdit.setText(portstr);
            }
        }
        String titlestr = mySharedPreference.getStringShared(Msg.TITLE);
        if(titlestr!=null){
            if(!titlestr.equals("")){
                TitleEdit.setText(titlestr);
            }
        }
        confirm_Btn.setOnClickListener(this);
        reset_Btn.setOnClickListener(this);
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()){
            case R.id.reset_Btn:
                IpEdit.setText("");
                PortEdit.setText("");
                TitleEdit.setText("");
                break;
            case R.id.confirm_Btn:
                if(IsIp(IpEdit.getText().toString())){
                    mySharedPreference.setStringShared(Msg.IP, IpEdit.getText().toString());
                    mySharedPreference.setStringShared(Msg.PORT, PortEdit.getText().toString());
                    mySharedPreference.setStringShared(Msg.TITLE, TitleEdit.getText().toString());
                    this.finish();
                }else{
                    Toast.makeText(SettingActivity.this,"Ip格式不正确",Toast.LENGTH_LONG).show();
                }


                break;
        }
    }
    public boolean IsIp(String IP){
        Pattern pattern = Pattern.compile("\\b((?!\\d\\d\\d)\\d+|1\\d\\d|2[0-4]\\d|25[0-5])\\.((?!\\d\\d\\d)\\d+|1\\d\\d|2[0-4]\\d|25[0-5])\\.((?!\\d\\d\\d)\\d+|1\\d\\d|2[0-4]\\d|25[0-5])\\.((?!\\d\\d\\d)\\d+|1\\d\\d|2[0-4]\\d|25[0-5])\\b");
        Matcher matcher = pattern.matcher(IP); //以验证127.400.600.2为例
        return matcher.matches();
    }
}
