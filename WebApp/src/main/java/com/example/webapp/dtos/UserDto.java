package com.example.webapp.dtos;

import com.example.webapp.models.ChatEntity;
import com.example.webapp.models.UserEntity;
import org.bson.types.ObjectId;

import java.util.List;

public record UserDto (String id, String username, String password, List<ChatEntity> chats){
    public UserDto(UserEntity c) {
        this(c.getId() == null ? new ObjectId().toHexString() : c.getId().toHexString(), c.getUsername(), c.getPassword(), c.getChats());
    }

    public UserEntity toUserEntity() {
        ObjectId _id = id == null ? new ObjectId() : new ObjectId(id);
        return new UserEntity(_id, username, password, chats);
    }
}
