package com.example.webapp.repositories;

import com.example.webapp.models.UserEntity;
import org.apache.catalina.User;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface IUserRepository {
    UserEntity save(UserEntity userEntity);

    List<UserEntity> saveAll(List<UserEntity> userEntities);

    List<UserEntity> findAll();

    List<UserEntity> findAll(List<String> ids);

    UserEntity findOne(String id);

    long count();

    long delete(String id);

    long delete(List<String> ids);

    long deleteAll();

    UserEntity update(UserEntity userEntity);

    long update(List<UserEntity> userEntities);
}
