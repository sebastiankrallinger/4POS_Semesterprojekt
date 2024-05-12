package com.example.webapp;

import com.example.webapp.models.ChatEntity;
import com.example.webapp.models.UserEntity;

import java.util.ArrayList;
import java.util.List;

public class Chat {
    private List<UserEntity> chatSubscriber;
    private ChatEntity active_chat;

    public Chat(ChatEntity chat){
        active_chat = chat;
        chatSubscriber = new ArrayList<>();
    }

    public void setActive_chat(ChatEntity chat){
        active_chat = chat;
    }

    public ChatEntity getActive_chat(){
        return active_chat;
    }

    public void subscribe(UserEntity user){
        chatSubscriber.add(user);
    }

    public void remove(UserEntity user){
        chatSubscriber.remove(user);
    }

    public void updateChats(){
        for (UserEntity u:chatSubscriber) {
            System.out.println(u.getChats().toString());
        }
    }
}
