package com.example.healmax.italk.Interface;

import com.example.healmax.italk.Model.Friend;
import com.example.healmax.italk.Model.ReturnMessage;

/**
 * Created by healmax on 15/10/31.
 */
public interface IView {
    void success (ReturnMessage<?> result);
    void fial(ReturnMessage<?> result);
}
