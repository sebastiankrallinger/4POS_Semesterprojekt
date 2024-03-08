package com.example.webapp.services;

import com.example.webapp.dtos.UserDto;

import java.util.List;

public interface IUserService {
    UserDto save(UserDto userDTO);

    List<UserDto> saveAll(List<UserDto> userEntities);

    List<UserDto> findAll();

    List<UserDto> findAll(List<String> ids);

    UserDto findOne(String id);

    long count();

    long delete(String id);

    long delete(List<String> ids);

    long deleteAll();

    UserDto update(UserDto userDTO);

    long update(List<UserDto> userEntities);
}
