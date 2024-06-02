package com.example.webapp.services;

import com.example.webapp.dtos.UserDto;
import java.util.List;

//Repository für die Funktionen des UserService
public interface IUserService {
    UserDto save(UserDto userDTO);

    List<UserDto> findAll();

    UserDto findOne(String id);
}
