package com.example.healmax.italk.API;

import android.content.ContentValues;

import com.example.healmax.italk.Model.Friend;
import com.example.healmax.italk.Model.HttpResponseResult;
import com.example.healmax.italk.Model.ReturnMessage;
import com.example.healmax.italk.Model.User;
import com.example.healmax.italk.Service.LoginService;
import com.example.healmax.italk.Util.HttpUtil;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by healmax on 15/10/29.
 */
public class FriendApi extends BaseApi{

    public ReturnMessage<User> addFriend (Friend friendInfo, String token) {
        ContentValues values = new ContentValues();
        JSONObject jsonParam = new JSONObject();
        try {
            jsonParam.put("FriendName", friendInfo.getId());
        } catch (Exception ex) {
            ex.printStackTrace();
        }

        HttpResponseResult result = HttpUtil.doPostByJson("relationship", jsonParam, token);

        if (result.getStatus() == -2) {
            return new ReturnMessage(result.getStatus(), result.getContent());
        }

        ReturnMessage<User> message = new ReturnMessage<User>();
        try {
            Friend user = new Friend();
            JSONObject jsonObject = new JSONObject(result.getContent());
            message = prepareRespone(jsonObject);
        } catch (Exception ex) {
            ex.printStackTrace();
            return new ReturnMessage(new Integer(-1), ex.getMessage());
        }

        return message;
    }

    public ReturnMessage<List<Friend>> getList () {
        String token = LoginService.getInstance().getLoginUser().getToken();
        HttpResponseResult result = HttpUtil.doGet("relationship", null, token);

        if (result.getStatus() == -2) {
            return new ReturnMessage(result.getStatus(), result.getContent());
        }

        ReturnMessage<List<Friend>> message = new ReturnMessage<List<Friend>>();
        try {
            JSONObject jsonObject = new JSONObject(result.getContent());
            List<Friend> friendList = getFriendListFromJsonObject(jsonObject);
            message = prepareRespone(jsonObject);
            message.setDate(friendList);
        } catch (Exception ex) {
            ex.printStackTrace();
            return new ReturnMessage(new Integer(-1), ex.getMessage());
        }

        return message;
    }

    private List<Friend> getFriendListFromJsonObject (JSONObject object) {

        List<Friend> friendList = new ArrayList<Friend>();
        Friend friend = null;
        try {
            JSONArray array = object.getJSONArray("friends");
            for (int index=0; index < array.length(); index++) {
                friend = new Friend();
                friend.setId(array.getString(index));
                friend.setName(array.getString(index));
                friendList.add(friend);
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }

        return friendList;
    }

    public ReturnMessage<User> search (String id, String token) {
        ContentValues values = new ContentValues();
        values.put("username", id);
        HttpResponseResult result = HttpUtil.doGet("account", values, token);

        if (result.getStatus() == -2) {
            return new ReturnMessage(result.getStatus(), result.getContent());
        }

        ReturnMessage<User> message = new ReturnMessage<User>();
        try {
            User user = new User();
            JSONObject jsonObject = new JSONObject(result.getContent());
            user.setId(jsonObject.optString("UserName"));
            user.setIsFriend(jsonObject.optBoolean("IsFriend"));
            message = prepareRespone(jsonObject);
            message.setDate(user);
        } catch (Exception ex) {
            ex.printStackTrace();
            return new ReturnMessage(new Integer(-1), ex.getMessage());
        }

        return message;
    }

}
