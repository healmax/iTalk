package com.example.healmax.italk.API;

import android.content.ContentValues;
import android.content.Context;
import android.content.SharedPreferences;

import com.example.healmax.italk.Model.HttpResponseResult;
import com.example.healmax.italk.Model.ReturnMessage;
import com.example.healmax.italk.Util.HttpUtil;

import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;


/**
 * Created by healmax on 15/10/26.
 */
public class UserApi extends BaseApi{

    /*
     *  API String
     */
    private static String loginApi       = "token";
    private static String registerApi    = "account/register";


    //獲取login preference 字串
    private static final String LOGIN_PREFS       = "LoginPrefsFile";
    public  static final String USER_ID           = "UserID";
    public  static final String USER_PW           = "UserPW";
    public  static final String HAS_BEEN_LOGIN    = "hasBeenLogin";


    public ReturnMessage login(String id, String pw) {
        ContentValues values = new ContentValues();
        values.put("username", id);
        values.put("password", pw);

        HttpResponseResult result = HttpUtil.doPost(loginApi, values);

        if (result.getStatus() == -2) {
            return new ReturnMessage(-1, result.getContent());
        }

        ReturnMessage message = new ReturnMessage();
        
        try {
            JSONObject jsonObject = new JSONObject(result.getContent());
            message = prepareRespone(jsonObject);
        } catch (Exception ex) {
            ex.printStackTrace();
            return new ReturnMessage(new Integer(-1), ex.getMessage());
        }

        return message;
    }

    public ReturnMessage autoLogin(Context context) {

        Map<String, String> userInfo = UserApi.getUserInfo(context);
        String id;
        String pw;
        id = userInfo.get(UserApi.USER_ID);
        pw = userInfo.get(UserApi.USER_PW);

        ContentValues values = new ContentValues();
        values.put("username", id);
        values.put("password", pw);

        HttpResponseResult result = HttpUtil.doPost(loginApi, values);

        if (result.getStatus() == -2) {
            return new ReturnMessage(-1, result.getContent());
        }

        ReturnMessage message = new ReturnMessage();

        try {
            JSONObject jsonObject = new JSONObject(result.getContent());
            message = prepareRespone(jsonObject);
        } catch (Exception ex) {
            ex.printStackTrace();
            return new ReturnMessage(new Integer(-1), ex.getMessage());
        }

        return message;
    }

    public ReturnMessage register (Context context, String id, String pw){
        ContentValues values = new ContentValues();
        values.put("username", id);
        values.put("password", pw);
        HttpResponseResult result = HttpUtil.doPost(registerApi, values);

        if (result.getStatus() == -2) {
            return new ReturnMessage(-1, result.getContent());
        }

        ReturnMessage message = new ReturnMessage();


        try {
            JSONObject jsonObject = new JSONObject(result.getContent());

            message = prepareRespone(jsonObject);

            if (message.getStatus() == 200) {
                UserApi.saveUserInfo(context, id, pw);
            }
        } catch (Exception ex) {
            ex.printStackTrace();
            return new ReturnMessage(new Integer(-1), ex.getMessage());
        }

        return message;
    }

    private static Map<String, String> getUserInfo(Context context) {
        Map<String, String> userInfo = new HashMap<String, String>();
        SharedPreferences loginPrefs;
        loginPrefs = context.getSharedPreferences(UserApi.LOGIN_PREFS, 0);

        boolean hasBeenLoing = loginPrefs.getBoolean(UserApi.HAS_BEEN_LOGIN, false);
        if(hasBeenLoing) {
            userInfo.put(UserApi.USER_ID, loginPrefs.getString(UserApi.USER_ID, null));
            userInfo.put(UserApi.USER_PW, loginPrefs.getString(UserApi.USER_PW, null));
        }

        return userInfo;
    }

    private static void saveUserInfo(Context context, String id, String pw) {
        SharedPreferences loginPrefs;
        loginPrefs = context.getSharedPreferences(UserApi.LOGIN_PREFS, 0);
        SharedPreferences.Editor prefsWriter = loginPrefs.edit();
        prefsWriter.putBoolean(UserApi.HAS_BEEN_LOGIN, true);
        prefsWriter.putString(UserApi.USER_ID, id);
        prefsWriter.putString(UserApi.USER_PW, pw);
        prefsWriter.commit();
        prefsWriter.clear();
    }
}
