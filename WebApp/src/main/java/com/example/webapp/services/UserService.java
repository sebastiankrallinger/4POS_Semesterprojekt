package com.example.webapp.services;

import com.example.webapp.dtos.UserDto;
import com.example.webapp.repositories.UserRepository;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class UserService implements IUserService{
    private final UserRepository userRepository;

    public UserService(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    @Override
    public UserDto save(UserDto userDTO) {
        return new UserDto(userRepository.save(userDTO.toUserEntity()));
    }

    @Override
    public List<UserDto> saveAll(List<UserDto> userEntities) {
        return userEntities.stream()
                .map(UserDto::toUserEntity)
                .peek(userRepository::save)
                .map(UserDto::new)
                .toList();
    }

    @Override
    public List<UserDto> findAll() {
        return userRepository.findAll().stream().map(UserDto::new).toList();
    }

    @Override
    public List<UserDto> findAll(List<String> ids) {
        return userRepository.findAll(ids).stream().map(UserDto::new).toList();
    }

    @Override
    public UserDto findOne(String id) {
        return new UserDto(userRepository.findOne(id));
    }

    @Override
    public long count() {
        return userRepository.count();
    }

    @Override
    public long delete(String id) {
        return userRepository.delete(id);
    }

    @Override
    public long delete(List<String> ids) {
        return userRepository.delete(ids);
    }

    @Override
    public long deleteAll() {
        return userRepository.deleteAll();
    }

    @Override
    public UserDto update(UserDto userDTO) {
        return new UserDto(userRepository.update(userDTO.toUserEntity()));
    }

    @Override
    public long update(List<UserDto> userEntities) {
        return userRepository.update(userEntities.stream().map(UserDto::toUserEntity).toList());
    }
}
