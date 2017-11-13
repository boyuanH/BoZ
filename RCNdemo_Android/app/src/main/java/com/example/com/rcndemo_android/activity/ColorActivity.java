package com.example.com.rcndemo_android.activity;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

import com.example.com.rcndemo_android.R;
import com.example.com.rcndemo_android.view.ColorPickView;

public class ColorActivity extends AppCompatActivity {

    private View txtColor;
    private ColorPickView myView;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_color);
        myView = (ColorPickView) findViewById(R.id.color_picker_view);
        txtColor = (View) findViewById(R.id.txt_color);
        myView.setOnColorChangedListener(new ColorPickView.OnColorChangedListener() {

            @Override
            public void onColorChange(int color) {
                txtColor.setBackgroundColor(color);
            }

        });
    }
}
