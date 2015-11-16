package com.example.healmax.italk.Util;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.AsyncTask;

import com.example.healmax.italk.API.FriendApi;
import com.example.healmax.italk.DataBase.DBUtil;
import com.example.healmax.italk.Interface.IView;
import com.example.healmax.italk.Model.Friend;
import com.example.healmax.italk.Model.ReturnMessage;
import com.example.healmax.italk.Model.User;
import com.example.healmax.italk.Service.LoginService;
import com.example.healmax.italk.sync.SyncManager;
import com.example.healmax.italk.sync.SyncParameter;

/**
 * Created by healmax on 15/10/31.
 */
public class FriendUtil {

    public class SearchFriendAsyncTask extends AsyncTask<String, Void, Void> {

        ReturnMessage<User> result;
        IView viewInterface;


        public SearchFriendAsyncTask (IView viewInterface ) {
            this.viewInterface = viewInterface;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Void doInBackground(String... params) {

            String id = params[0];
            FriendApi api = new FriendApi();
            String tokenString = LoginService.getInstance().getLoginUser().getToken();
            result = api.search(id, tokenString);

            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);

            if(result.getStatus() == 0) {
                this.viewInterface.success(result);
            } else {
                this.viewInterface.fial(result);
            }
        }
    }

//    public class AddFriendAsyncTask extends AsyncTask<String, Void, Void> {
//
//        ReturnMessage<User> result;
//        IView viewInterface;
//
//
//        public AddFriendAsyncTask (IView viewInterface ) {
//            this.viewInterface = viewInterface;
//        }
//
//        @Override
//        protected void onPreExecute() {
//            super.onPreExecute();
//        }
//
//        @Override
//        protected Void doInBackground(String... params) {
//
//            String id = params[0];
//            FriendApi api = new FriendApi();
//            String tokenString = LoginService.getInstance().getLoginUser().getToken();
//            result = api.addFriend(id, tokenString);
//
//            return null;
//        }
//
//        @Override
//        protected void onPostExecute(Void aVoid) {
//            super.onPostExecute(aVoid);
//
//            if(result.getStatus() == 0) {
//                this.viewInterface.success(result);
//            } else {
//                this.viewInterface.fial(result);
//            }
//        }
//    }

    public class AddFriendAsyncTask extends AsyncTask<String, Void, Void> {

        Context context = null;
        Friend friendInfo;
        ProgressDialog mDialog = null;


        public AddFriendAsyncTask (Context context, Friend friendInfo) {
            this.context = context;
        }

        @Override
        protected Void doInBackground(String... params) {

            long id = DBUtil.insertFriend(context, friendInfo, true);

            if (id > 0){
                SyncManager.addSyncAction(context, String.valueOf(id), SyncParameter.SYNC_ACTION_ADD_FRIEND);
            }

            return null;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            mDialog.dismiss();
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            mDialog = new ProgressDialog(context);
            mDialog.setMessage("wating");
            mDialog.setCancelable(false);
            mDialog.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            mDialog.show();
        }
    }

}
