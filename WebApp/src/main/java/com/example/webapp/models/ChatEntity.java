package com.example.webapp.models;

import com.example.webapp.Chat;
import org.bson.types.ObjectId;

import java.util.List;
import java.util.Objects;

public class ChatEntity {
    private String bezeichnung;
    private String receiver;
    private List<MessageEntity> messageEntities;

    public ChatEntity() {
    }

    public ChatEntity(String bezeichnung, List<MessageEntity> messageEntities, String receiver){
        this.bezeichnung = bezeichnung;
        this.messageEntities = messageEntities;
        this.receiver = receiver;
    }

    public String getBezeichnung() {
        return bezeichnung;
    }

    public ChatEntity setBezeichnung(String bezeichnung) {
        this.bezeichnung = bezeichnung;
        return this;
    }

    public List<MessageEntity> getMessages() {
        return messageEntities;
    }

    public ChatEntity setMessages(List<MessageEntity> messageEntities) {
        this.messageEntities = messageEntities;
        return this;
    }

    public String getReceiver() {
        return receiver;
    }

    public ChatEntity setReceiver(String receiver) {
        this.receiver = receiver;
        return this;
    }

    @Override
    public String toString() {
        return "Chat{" + "bezeichnung='" + bezeichnung + "messages='" + messageEntities + '\'' + '}';
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        ChatEntity chatEntity = (ChatEntity) o;
        return Objects.equals(bezeichnung, chatEntity.bezeichnung);
    }

    @Override
    public int hashCode() {
        return Objects.hash(bezeichnung, messageEntities);
    }


    public Chat toChat(){
        return new Chat(this);
    }
}
