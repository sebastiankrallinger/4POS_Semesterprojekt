package com.example.webapp.models;

import java.util.Objects;

//MessageEntity mit Eigenschaften, Gettern und Settern
public class MessageEntity {
    private String message;
    private boolean receiver;
    private String date;

    public MessageEntity() {
    }

    public MessageEntity(String message, boolean receiver, String date) {
        this.message = message;
        this.receiver = receiver;
        this.date = date;

    }

    public String getMessage() {
        return message;
    }

    public MessageEntity setMessage(String message) {
        this.message = message;
        return this;
    }

    public String getDate(){
        return date;
    }

    public MessageEntity setDate(String date){
        this.date = date;
        return this;
    }

    public boolean getReceiver(){
        return receiver;
    }

    public MessageEntity setReceiver(boolean receiver){
        this.receiver = receiver;
        return this;
    }

    //ToString zum Debugen
    @Override
    public String toString() {
        return "Messsage{" + "Message='" + message + ": " + date + '\'' + '}';
    }
}
