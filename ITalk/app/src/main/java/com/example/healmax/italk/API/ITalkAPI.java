package com.example.healmax.italk.API;

import com.example.healmax.italk.Util.ITalkUtil;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.DataOutputStream;
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
import java.util.ArrayList;
import java.util.List;

/**
 * Created by healmax on 15/10/17.
 */

public class ITalkAPI {

    private static String url = "http://italk-api.elasticbeanstalk.com/";

    static public String signIn (String id, String pw) {
        String signInUrl = "http://italk-api.elasticbeanstalk.com/token";
        String result = null;

        HttpURLConnection conn = null;
        try {
            URL url = new URL(signInUrl);
            conn = (HttpURLConnection) url.openConnection();
            conn.setReadTimeout(ITalkUtil.NETWORK_TIMEOUT_SOCKET);
            conn.setConnectTimeout(ITalkUtil.NETWORK_TIMEOUT_CONNECTION);
            conn.setRequestMethod("POST");
            conn.setDoInput(true);
            conn.setDoOutput(true);

            List<NameValuePair> params = new ArrayList<NameValuePair>();
            params.add(new BasicNameValuePair("grant_type", "password"));
            params.add(new BasicNameValuePair("username", id));
            params.add(new BasicNameValuePair("password", pw));

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
            result = responseBuff.toString();
            int t = 0;
        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (Exception ex) {
            ex.printStackTrace();
        }finally {
            if (conn != null) {
                conn.disconnect();
            }
        }

        return result;
    }

    static public String register (String id, String pw) {
        String signInUrl = "http://italk-api.elasticbeanstalk.com/account/register";
        String result = null;

        HttpURLConnection conn = null;
        try {
            URL url = new URL(signInUrl);
            conn = (HttpURLConnection) url.openConnection();
            conn.setReadTimeout(ITalkUtil.NETWORK_TIMEOUT_SOCKET);
            conn.setConnectTimeout(ITalkUtil.NETWORK_TIMEOUT_CONNECTION);
            conn.setRequestMethod("POST");
            conn.setDoInput(true);
            conn.setDoOutput(true);

            conn.setRequestProperty("Content-Type","application/json");
            conn.setRequestProperty("Host", "android.schoolportal.gr");
            conn.connect();
            //Create JSONObject here
            JSONObject jsonParam = new JSONObject();
            jsonParam.put("username", id);
            jsonParam.put("password", pw);



            DataOutputStream os = new DataOutputStream(conn.getOutputStream());
            BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(os, "UTF-8"));
            writer.write(jsonParam.toString());
            writer.flush();
            writer.close();
            os.close();

            // get response
            conn.connect();
//            String response = "";
            InputStream is = conn.getInputStream();
            BufferedReader rd = new BufferedReader(new InputStreamReader(is, "UTF-8"));
            String line;
            StringBuffer responseBuff = new StringBuffer();
            while ((line = rd.readLine()) != null) {
                responseBuff.append(line);
                responseBuff.append('\r');
            }
            rd.close();
            result = responseBuff.toString();
            int t = 0;
        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (Exception ex) {
            ex.printStackTrace();
        }finally {
            if (conn != null) {
                conn.disconnect();
            }
        }

        return result;
    }

    public static String getQuery(List<NameValuePair> params) throws UnsupportedEncodingException {
        StringBuilder result = new StringBuilder();
        boolean first = true;

        for (NameValuePair pair : params) {
            if (first)
                first = false;
            else
                result.append("&");

            result.append(URLEncoder.encode(pair.getName(), "UTF-8"));
            result.append("=");
            result.append(URLEncoder.encode(pair.getValue(), "UTF-8"));
        }

        return result.toString();
    }

    static public String checkFriend(String userID) {
        String apiString = url + "account/" + userID;

        String result = null;

        HttpURLConnection conn = null;
        try {
            URL url = new URL(apiString);
            conn = (HttpURLConnection) url.openConnection();
            conn.setReadTimeout(ITalkUtil.NETWORK_TIMEOUT_SOCKET);
            conn.setConnectTimeout(ITalkUtil.NETWORK_TIMEOUT_CONNECTION);
            conn.setRequestMethod("GET");

            conn.setRequestProperty("Content-Type","application/json");
            conn.setRequestProperty("Host", "android.schoolportal.gr");
            conn.setRequestProperty("Authorization", "bearer "+ ITalkUtil.getToken());

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
            result = responseBuff.toString();
        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (Exception ex) {
            ex.printStackTrace();
        }finally {
            if (conn != null) {
                conn.disconnect();
            }
        }

        return result;
    }

    // 標頭格式  conn.setRequestProperty("Authorization", "bearer token($%%@#^GDRGdfgafg$%#Y)");
    static public List<String> getFriends () {
        List<String> friends = new ArrayList<String>();
        friends.add("healmax");
        friends.add("andy");

        return  friends;
    }

}
