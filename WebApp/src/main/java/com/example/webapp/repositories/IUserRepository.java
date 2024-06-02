package com.example.webapp.repositories;

import com.example.webapp.models.UserEntity;
import org.springframework.stereotype.Repository;
import java.util.List;

//Repository für die Funktionen des UserRepository
@Repository
public interface IUserRepository {
    UserEntity save(UserEntity userEntity);

    List<UserEntity> findAll();

    UserEntity findOne(String id);

    UserEntity update(UserEntity userEntity);
}
