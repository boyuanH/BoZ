package com.example.com.rcndemo_android.activity;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.net.Uri;
import android.os.Environment;
import android.os.StrictMode;
import android.os.Bundle;
import android.telephony.TelephonyManager;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.example.com.rcndemo_android.Obj.Msg;
import com.example.com.rcndemo_android.shared.MySharedPreference;
import com.example.com.rcndemo_android.view.ColorPickView;
import com.example.com.rcndemo_android.view.LongPressView;
import com.example.com.rcndemo_android.R;
import com.example.com.rcndemo_android.view.SignaturePad;

import org.apache.commons.net.ftp.FTPClient;
import org.apache.commons.net.ftp.FTPReply;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.UUID;

public class MainActivity extends Activity implements View.OnClickListener{
    private TextView titleText;
    private Button OkBtn,ResetBtn;
    private SignaturePad Signatureview;
    private ImageView SignatureBackground,PanelColor,PanelSize;
    //private ImageView BargroundColor;
    private RelativeLayout Contentlayout,SignatureLayout;
    private LongPressView longView;
    private MySharedPreference mySharedPreference;

    private FTPClient ftpClient = null;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        this.requestWindowFeature(Window.FEATURE_NO_TITLE);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        mySharedPreference = new MySharedPreference(this);
        titleText = (TextView) findViewById(R.id.titleText);
        OkBtn = (Button)findViewById(R.id.OkBtn);
        ResetBtn = (Button)findViewById(R.id.ResetBtn);
        SignatureBackground = (ImageView)findViewById(R.id.SignatureBackground);
        SignatureLayout = (RelativeLayout) findViewById(R.id.SignatureLayout);
        Contentlayout = (RelativeLayout)findViewById(R.id.Contentlayout);
        longView = (LongPressView)findViewById(R.id.longView);
        PanelColor = (ImageView) findViewById(R.id.PanelColor);
        PanelSize = (ImageView) findViewById(R.id.PanelSize);
        OkBtn.setOnClickListener(this);
        ResetBtn.setOnClickListener(this);
        SignatureBackground.setOnClickListener(this);
        Contentlayout.setOnClickListener(this);
        PanelColor.setOnClickListener(this);
        PanelSize.setOnClickListener(this);
        longView.setOnLongClickListener(new View.OnLongClickListener() {

            @Override
            public boolean onLongClick(View v) {
                Intent setIntent = new Intent(MainActivity.this, SettingActivity.class);
                startActivity(setIntent);
                return false;
            }
        });
    }

    @Override
    protected void onStart() {
        super.onStart();
        String Title = mySharedPreference.getStringShared(Msg.TITLE);
        if(Title.equals("")){
            titleText.setText("欢迎参加2017理光（中国）代理商战略大会");
        }else{
            titleText.setText(Title);
        }
    }
    public File getAlbumStorageDir(String albumName) {
        // Get the directory for the user's public pictures directory.
        File file = new File(Environment.getExternalStoragePublicDirectory(
                Environment.DIRECTORY_PICTURES), albumName);
        if (!file.mkdirs()) {
            Log.e("SignaturePad", "Directory not created");
        }
        return file;
    }

    public void saveBitmapToJPG(Bitmap bitmap, File photo) throws IOException {
        Bitmap newBitmap = Bitmap.createBitmap(bitmap.getWidth(), bitmap.getHeight(), Bitmap.Config.ARGB_8888);
        Canvas canvas = new Canvas(newBitmap);
        canvas.drawColor(Color.WHITE);
        canvas.drawBitmap(bitmap, 0, 0, null);
        OutputStream stream = new FileOutputStream(photo);
        newBitmap.compress(Bitmap.CompressFormat.JPEG, 80, stream);
        stream.close();
    }

    public boolean addJpgSignatureToGallery(Bitmap signature) {
        boolean result = false;
        try {
            //String deviceId = getDeviceId(this);
            File photo = new File(getAlbumStorageDir("SignaturePad"), String.format("Signature_%d.jpg", System.currentTimeMillis()));
            saveBitmapToJPG(signature, photo);
            scanMediaFile(photo);
            result = true;
            String fullFilePath=photo.getPath();
            upLoadFileToFTP(fullFilePath);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return result;
    }

    public static boolean dialogType;
    @Override
    public void onClick(View v) {
        switch (v.getId()){
            case R.id.OkBtn:
                if(Signatureview!=null){
                    if (Signatureview.getTouched()) {

                        Bitmap signatureBitmap = Signatureview.getSignatureBitmap();
                        if (addJpgSignatureToGallery(signatureBitmap)) {
                            Signatureview.clear();
                            SignatureBackground.setVisibility(View.VISIBLE);
                            SignatureLayout.setVisibility(View.GONE);
                            Signatureview.setVisibility(View.GONE);
                            Signatureview = null;
                        } else {
                            Toast.makeText(MainActivity.this, "Unable to store the signature", Toast.LENGTH_SHORT).show();
                        }

                    } else {

                        Toast.makeText(this, "您没有签名~", Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(this,"请赐签名",Toast.LENGTH_SHORT).show();
                }
                break;
            case R.id.ResetBtn:
                if(Signatureview!=null){
                    Signatureview.clear();
                }
                break;
            case R.id.SignatureBackground:
                SignatureBackground.setVisibility(View.GONE);
                SignatureLayout.setVisibility(View.VISIBLE);
                Signatureview = (SignaturePad) findViewById(R.id.Signatureview);
                Signatureview.setVisibility(View.VISIBLE);
                break;
            case R.id.Contentlayout:
                SignatureBackground.setVisibility(View.GONE);
                SignatureLayout.setVisibility(View.VISIBLE);
                Signatureview = (SignaturePad) findViewById(R.id.Signatureview);
                Signatureview.setVisibility(View.VISIBLE);
                break;
            case R.id.PanelColor:
                dialogType = true;
                showBackgroundColorDialog(PanelColor);
                break;
            case R.id.PanelSize:
                showPaintSizeDialog(PanelSize);
                break;
        }
    }
    private int select_paintcolor;
    private int select_size_index = 1;
    private int select_barcolor;
    public int selectHandWritetSize(int which){
        int size =Integer.parseInt(this.getResources().getStringArray(R.array.paintsize)[which]);
        return size;
    }

    //弹出画笔大小选项对话框
    public void showBackgroundColorDialog(View parent){
        //type = true 画笔 type = false 背景

        AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(this);
        final AlertDialog alertDialog = alertDialogBuilder.create();
        alertDialogBuilder.setTitle("选择背景颜色：");
        View view = View.inflate(this, R.layout.activity_color, null);
        alertDialog.setView(view, 0, 0, 0, 0);
        final View txtColor = (View) view.findViewById(R.id.txt_color);
        final ColorPickView myView= (ColorPickView) view.findViewById(R.id.color_picker_view);
        final Button colorReset_btn = (Button) view.findViewById(R.id.colorReset_btn);
        final Button colorOk_btn = (Button) view.findViewById(R.id.colorOk_btn);

        if(dialogType==true){
            int color = mySharedPreference.getIntShared(Msg.P_COLOR);
            if(color==0){
                select_paintcolor = Color.WHITE;
            }else{
                select_paintcolor = color;
            }
            txtColor.setBackgroundColor(select_paintcolor);
        }else{
            int color = mySharedPreference.getIntShared(Msg.B_COLOR);
            if(color==0){
                select_barcolor = Color.WHITE;
            }else{
                select_barcolor = color;
            }
            txtColor.setBackgroundColor(select_barcolor);
        }
        myView.setOnColorChangedListener(new ColorPickView.OnColorChangedListener() {

            @Override
            public void onColorChange(int color) {
                txtColor.setBackgroundColor(color);
                if (dialogType == true) {
                    select_paintcolor = color;
                } else {
                    select_barcolor = color;
                }
            }

        });
        colorReset_btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (dialogType == true) {
                    int x = mySharedPreference.getIntShared(Msg.P_POSITIONX);
                    int y = mySharedPreference.getIntShared(Msg.P_POSITIONY);
                    myView.setPosition(x, y);
                    int color = mySharedPreference.getIntShared(Msg.P_COLOR);
                    if (color == 0) {
                        select_paintcolor = Color.WHITE;
                    } else {
                        select_paintcolor = color;
                    }
                    txtColor.setBackgroundColor(select_paintcolor);
                } else {

                    int x = mySharedPreference.getIntShared(Msg.B_POSITIONX);
                    int y = mySharedPreference.getIntShared(Msg.B_POSITIONY);
                    myView.setPosition(x, y);
                    int color = mySharedPreference.getIntShared(Msg.B_COLOR);
                    if (color == 0) {
                        select_barcolor = Color.WHITE;
                    } else {
                        select_barcolor = color;
                    }
                    txtColor.setBackgroundColor(select_barcolor);
                }
            }
        });
        colorOk_btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (select_paintcolor == select_barcolor) {
                    if (dialogType) {
                        Toast.makeText(MainActivity.this, "画笔颜色和背景颜色不可设置为同一颜色", Toast.LENGTH_LONG).show();
                    } else {
                        Toast.makeText(MainActivity.this, "背景颜色和画笔颜色不可设置为同一颜色", Toast.LENGTH_LONG).show();
                    }
                } else {
                    if (dialogType) {
                        Signatureview.setPenColor(select_paintcolor);
                        myView.getPaintPosition();
                        mySharedPreference.setIntShared(Msg.P_COLOR, select_paintcolor);
                    } else {
                        //Signatureview.setBackColor(select_barcolor);
                        myView.getBargroundPosition();
                        mySharedPreference.setIntShared(Msg.B_COLOR, select_barcolor);
                    }
                    alertDialog.dismiss();
                }
            }
        });
        alertDialog.show();
    }
    //弹出画笔大小选项对话框
    public void showPaintSizeDialog(View parent){
        AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(this);
        alertDialogBuilder.setTitle("选择画笔大小：");

        alertDialogBuilder.setSingleChoiceItems(R.array.paintsize, select_size_index, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                select_size_index = which;
                Signatureview.setMinWidth(selectHandWritetSize(which)/3+3);
                Signatureview.setMaxWidth(selectHandWritetSize(which));
                Toast.makeText(MainActivity.this,selectHandWritetSize(which)+"",Toast.LENGTH_LONG).show();
                dialog.dismiss();
            }
        });

        alertDialogBuilder.setNegativeButton("取消", new DialogInterface.OnClickListener() {

            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
            }
        });
        alertDialogBuilder.create().show();
    }

    //获取设备的唯一标示
    public String getDeviceId(Context context){
        StringBuilder deviceId = new StringBuilder();
        deviceId.append("a");
        TelephonyManager tm = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);
        String imei = tm.getDeviceId();
        if(isEmpty(imei)){
            deviceId.append("imei");
            deviceId.append(imei);
            return deviceId.toString();
        }
        String sn = tm.getSimSerialNumber();
        if(isEmpty(sn)){
            deviceId.append("sn");
            deviceId.append(sn);
            return deviceId.toString();
        }
        String uuid = getUUid();
        if(isEmpty(uuid)){
            deviceId.append(uuid);
            return deviceId.toString();
        }
        return deviceId.toString();
    }

    public String getUUid(){
        String uuid = UUID.randomUUID().toString();
        return uuid;
    }
    public boolean isEmpty(String str){
        boolean isboo = false;
        if(str!=null&&!str.equals("")){
            isboo = true;
        }else{
            isboo = false;
        }
        return isboo;
    }


    private boolean upLoadFileToFTP(String fileFullPath){
        if(getFTPConnection() == false || ftpClient == null){
            return false;
        }
        File fileSrc = new File(fileFullPath);
        if(!fileSrc.exists()){
            return false;
        }
        InputStream is = null;
        try {
            is = new FileInputStream(fileSrc);
            if(is == null){
                return false;
            }
            String remoteFileName = fileFullPath.substring(fileFullPath.lastIndexOf("/") + 1);
            ftpClient.enterLocalPassiveMode();
            ftpClient.setFileType(FTPClient.BINARY_FILE_TYPE);
            ftpClient.storeFile(remoteFileName, is);
            is.close();

        } catch (Exception e) {
            e.printStackTrace();
            return  false;
        }
        finally {
            if(ftpClient!=null){
                try {
                    ftpClient.disconnect();
                } catch (IOException e) {
                    Toast.makeText(MainActivity.this,e.getMessage().toString(),Toast.LENGTH_LONG).show();
                    e.printStackTrace();
                }
            }
            ftpClient = null;
        }
        return  true;
    }

    private boolean getFTPConnection(){
        ftpClient = new FTPClient();
        //ftpClient.setConnectTimeout(1000);
        try {
            String hostName = mySharedPreference.getStringShared(Msg.IP);
            String portstr = mySharedPreference.getStringShared(Msg.PORT);
            Toast.makeText(MainActivity.this, "hostName:　"+hostName, Toast.LENGTH_SHORT).show();
            int port = Integer.parseInt(portstr);
            StrictMode.setThreadPolicy(new StrictMode.ThreadPolicy.Builder().detectDiskReads().detectDiskWrites().detectNetwork().penaltyLog().build());
            ftpClient.connect(hostName,port);
            int replyCode = ftpClient.getReplyCode();
            if(!FTPReply.isPositiveCompletion(replyCode)){
                ftpClient.disconnect();
                ftpClient = null;
                return false;
            }
            ftpClient.login(getResources().getString(R.string.ftpUserName),getResources().getString(R.string.ftpPassword));
            if (!FTPReply.isPositiveCompletion(ftpClient.getReplyCode())) {
                ftpClient.disconnect();
                ftpClient = null;
                return false;
            }
        } catch (Exception e) {
            e.printStackTrace();
            ftpClient = null;
        }
        return true;
    }

    private void scanMediaFile(File photo) {
        Intent mediaScanIntent = new Intent(Intent.ACTION_MEDIA_SCANNER_SCAN_FILE);
        Uri contentUri = Uri.fromFile(photo);
        mediaScanIntent.setData(contentUri);
        MainActivity.this.sendBroadcast(mediaScanIntent);
    }
}
