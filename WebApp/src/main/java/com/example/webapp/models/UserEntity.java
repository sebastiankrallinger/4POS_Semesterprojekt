package com.example.webapp.models;

import org.apache.catalina.User;
import org.bson.types.ObjectId;

import java.util.Objects;

public class UserEntity {
    private ObjectId id;
    private String username;
    private String password;

    public UserEntity() {
    }

    public UserEntity(ObjectId id, String username, String password) {
        this.id = id;
        this.username = username;
        this.password = password;
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

    @Override
    public String toString() {
        return "User{" + "username='" + username + '\'' + ", password='" + password + '\'' + '}';
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        UserEntity userEntity = (UserEntity) o;
        return Objects.equals(username, userEntity.username) && Objects.equals(password, userEntity.password);
    }

    @Override
    public int hashCode() {
        return Objects.hash(username, password);
    }
}
