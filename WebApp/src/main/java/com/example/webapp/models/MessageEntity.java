package com.example.webapp.models;

import org.bson.types.ObjectId;

import java.util.Date;
import java.util.Objects;

public class MessageEntity {
    private String message;
    private Date date;

    public MessageEntity() {
    }

    public MessageEntity(String message) {
        this.message = message;
    }

    public String getMessage() {
        return message;
    }

    public MessageEntity setMessage(String message) {
        this.message = message;
        return this;
    }

    @Override
    public String toString() {
        return "Messsage{" + "Message='" + message + '\'' + '}';
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
        return Objects.hash(message);
    }
}
