package com.example.healmax.italk.Model;

/**
 * Created by healmax on 15/10/27.
 */
public class HttpResponseResult {

    private Integer status;
    private String content;

    public HttpResponseResult() {

    }

    public HttpResponseResult(Integer status, String message) {
        this.status = status;
        this.content = message;
    }

    public Integer getStatus() {
        return status;
    }

    public void setStatus(Integer status) {
        this.status = status;
    }


    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }
}
