package com.example.webapp.controller;

import com.example.webapp.dtos.UserDto;
import com.example.webapp.services.UserService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/app")
public class UserController {
    private final static Logger LOGGER = LoggerFactory.getLogger(MainController.class);
    private final UserService userService;

    public UserController(UserService userService) {

        this.userService = userService;
    }
    @PostMapping("user")
    @ResponseStatus(HttpStatus.CREATED)
    public UserDto postUser(@RequestBody UserDto userDto) {
        return userService.save(userDto);
    }

    @PostMapping("users")
    @ResponseStatus(HttpStatus.CREATED)
    public List<UserDto> postUsers(@RequestBody List<UserDto> userEntities) {
        return userService.saveAll(userEntities);
    }

    @GetMapping("users")
    public List<UserDto> getUsers() {
        return userService.findAll();
    }

    @GetMapping("user/{id}")
    public ResponseEntity<UserDto> getUser(@PathVariable String id) {
        UserDto userDTO = userService.findOne(id);
        if (userDTO == null) return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        return ResponseEntity.ok(userDTO);
    }

    @GetMapping("users/count")
    public Long getCount() {
        return userService.count();
    }

    @DeleteMapping("user/{id}")
    public Long deleteUser(@PathVariable String id) {
        return userService.delete(id);
    }

    @PutMapping("user")
    public UserDto putUser(@RequestBody UserDto userDTO) {
        return userService.update(userDTO);
    }

    @ExceptionHandler(RuntimeException.class)
    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    public final Exception handleAllExceptions(RuntimeException e) {
        LOGGER.error("Internal server error.", e);
        return e;
    }
}