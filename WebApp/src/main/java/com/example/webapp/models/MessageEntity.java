package com.example.webapp.models;

import org.bson.types.ObjectId;

import java.sql.Time;
import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.time.format.DateTimeFormatter;
import java.util.Date;
import java.util.Objects;

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

    @Override
    public String toString() {
        return "Messsage{" + "Message='" + message + ": " + date + '\'' + '}';
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        MessageEntity messageEntity = (MessageEntity) o;
        return Objects.equals(message, messageEntity.message);
    }

    @Override
    public int hashCode() {
        return Objects.hash(message, date);
    }
}
