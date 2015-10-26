package com.example.healmax.italk.API;

import com.example.healmax.italk.Model.ReturnMessage;

import org.json.JSONObject;

/**
 * Created by healmax on 15/10/26.
 */
public class BaseApi {

    protected <T> ReturnMessage<T> prepareRespone(JSONObject obj) {

        ReturnMessage<T> message = new ReturnMessage<>();
        try {
            message.setStatus(obj.getInt("status"));
            message.setMessage(obj.getString("Message"));
        } catch (Exception ex) {
            ex.printStackTrace();
            message.setStatus(new Integer(-1));
            message.setMessage(ex.getMessage());
        }
        return message;
    }
}
