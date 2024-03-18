package com.example.webapp.models;

import org.bson.types.ObjectId;

import java.util.List;
import java.util.Objects;

public class ChatEntity {
    private String bezeichnung;
    private List<MessageEntity> messageEntities;

    public ChatEntity() {
    }

    public ChatEntity(String bezeichnung, List<MessageEntity> messageEntities){
        this.bezeichnung = bezeichnung;
        this.messageEntities = messageEntities;
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
}
