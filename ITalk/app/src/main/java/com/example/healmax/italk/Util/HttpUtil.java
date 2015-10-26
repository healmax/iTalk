package com.example.healmax.italk.Util;

import android.content.ContentValues;

import com.example.healmax.italk.Model.HttpResponseResult;

import org.apache.http.NameValuePair;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLEncoder;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Created by healmax on 15/10/26.
 */
public class HttpUtil {

    private static String url = "http://italk-api.elasticbeanstalk.com/";
    public static final int NETWORK_TIMEOUT_CONNECTION = 5000;
    public static final int NETWORK_TIMEOUT_SOCKET = NETWORK_TIMEOUT_CONNECTION + 5000;

    public static HttpResponseResult doPost (String api, ContentValues params) {
//        String result= null;

        HttpURLConnection conn = null;
        HttpResponseResult result = new HttpResponseResult();

        try {
            String apiString = url + api;
            URL url = new URL(apiString);
            conn = (HttpURLConnection)url.openConnection();
            conn.setReadTimeout(HttpUtil.NETWORK_TIMEOUT_SOCKET);
            conn.setConnectTimeout(HttpUtil.NETWORK_TIMEOUT_CONNECTION);
            conn.setDoInput(true);
            conn.setDoOutput(true);
            conn.setRequestMethod("POST");

            OutputStream os = conn.getOutputStream();
            BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(os, "UTF-8"));
            writer.write(getQuery(params));
            writer.flush();
            writer.close();
            os.close();

            // get response
            conn.connect();
            String response = "";
            InputStream is = conn.getInputStream();
            BufferedReader rd = new BufferedReader(new InputStreamReader(is, "UTF-8"));
            String line;
            StringBuffer responseBuff = new StringBuffer();
            while ((line = rd.readLine()) != null) {
                responseBuff.append(line);
                responseBuff.append('\r');
            }
            rd.close();
            result.setStatus(conn.getResponseCode());
            result.setContent(responseBuff.toString());
            int t = 0;
        } catch (MalformedURLException e) {
            e.printStackTrace();
            result.setStatus(-2);
            result.setContent(e.getMessage());
        } catch (IOException e) {
            e.printStackTrace();
            result.setStatus(-2);
            result.setContent(e.getMessage());
        } catch (Exception ex) {
            ex.printStackTrace();
            result.setStatus(-2);
            result.setContent(ex.getMessage());
        }finally {
            if (conn != null) {
                conn.disconnect();
            }
        }

        return result;
    }

    public static String getQuery(ContentValues params) throws UnsupportedEncodingException {
        StringBuilder result = new StringBuilder();
        boolean first = true;

        Set<Map.Entry<String, Object>> s=params.valueSet();
        Iterator itr = s.iterator();


        while(itr.hasNext())
        {
            if (first) {
                first = false;
            }
            else {
                result.append("&");
            }
            Map.Entry me = (Map.Entry)itr.next();
            String key = me.getKey().toString();
            String value =  (String)me.getValue();

            result.append(URLEncoder.encode(key, "UTF-8"));
            result.append("=");
            result.append(URLEncoder.encode(value, "UTF-8"));
        }

        return result.toString();
    }
}
