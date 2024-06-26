package com.example.webapp.models;

import java.util.List;

//ChatEntity mit Eigenschaften, Gettern und Settern
public class ChatEntity {
    private String bezeichnung;
    private String receiver;
    private boolean newMsg;
    private List<MessageEntity> messageEntities;

    public ChatEntity() {
    }

    public ChatEntity(String bezeichnung, List<MessageEntity> messageEntities, String receiver, boolean newMsg){
        this.bezeichnung = bezeichnung;
        this.messageEntities = messageEntities;
        this.receiver = receiver;
        this.newMsg = newMsg;
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

    public boolean getNewMsg() {
        return newMsg;
    }

    public ChatEntity setNewMsg(boolean newMsg) {
        this.newMsg = newMsg;
        return this;
    }

    //ToString zum Debugen
    @Override
    public String toString() {
        return "Chat{" + "bezeichnung='" + bezeichnung + "messages='" + messageEntities + '\'' + '}';
    }
}
