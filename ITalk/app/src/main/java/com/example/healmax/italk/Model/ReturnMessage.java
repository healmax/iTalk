package com.example.healmax.italk.Model;

/**
 * Created by healmax on 15/10/26.
 */
public class ReturnMessage<T> {
    private Integer status;
    private String message;

    private T date;

    public ReturnMessage() {

    }

    public ReturnMessage(Integer status, String message) {
        this.status = status;
        this.message = message;
    }

    public Integer getStatus() {
        return status;
    }

    public void setStatus(Integer status) {
        this.status = status;
    }


    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public T getDate() {
        return date;
    }

    public void setDate(T date) {
        this.date = date;
    }
}
