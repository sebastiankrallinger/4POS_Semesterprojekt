package com.example.webapp.models;

import org.bson.types.ObjectId;
import java.util.List;
import java.util.Objects;

//UserEntity mit Eigenschaften, Gettern und Settern
public class UserEntity {
    private ObjectId id;
    private String username;
    private String password;
    private List<ChatEntity> chatEntities;

    public UserEntity() {
    }

    public UserEntity(ObjectId id, String username, String password, List<ChatEntity> chatEntities) {
        this.id = id;
        this.username = username;
        this.password = password;
        this.chatEntities = chatEntities;
    }

    public ObjectId getId() {
        return id;
    }

    public UserEntity setId(ObjectId id) {
        this.id = id;
        return this;
    }

    public String getUsername() {
        return username;
    }

    public UserEntity setUsername(String username) {
        this.username = username;
        return this;
    }

    public String getPassword() {
        return password;
    }

    public UserEntity setPassword(String password) {
        this.password = password;
        return this;
    }

    public List<ChatEntity> getChats() {
        return chatEntities;
    }

    public UserEntity setChats(List<ChatEntity> chatEntities) {
        this.chatEntities = chatEntities;
        return this;
    }

    //ToString zum Debugen
    @Override
    public String toString() {
        return "User{" + "username='" + username + '\'' + ", password='" + password + '\'' + ", chats='" + chatEntities + '\'' + '}';
    }

    //Usernamen vergleichen
    public boolean equalsUsername(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        UserEntity userEntity = (UserEntity) o;
        return Objects.equals(username, userEntity.username);
    }

    //Passwort vergeleichen
    public boolean equalsPassword(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        UserEntity userEntity = (UserEntity) o;
        return Objects.equals(password, userEntity.password);
    }

    //Methode die beide Vergleiche ausfuehrt
    public boolean checkPassword(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        UserEntity userEntity = (UserEntity) o;
        if (equalsUsername(userEntity)){
            if (equalsPassword(userEntity)){
                return true;
            }
        }
        return false;
    }
}
