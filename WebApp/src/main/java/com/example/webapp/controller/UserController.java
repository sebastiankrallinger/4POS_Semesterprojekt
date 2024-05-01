package com.example.webapp.controller;

import com.example.webapp.dtos.UserDto;
import com.example.webapp.models.ChatEntity;
import com.example.webapp.models.UserEntity;
import com.example.webapp.services.UserService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import java.text.SimpleDateFormat;
import java.util.Date;
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
        UserEntity user = userDto.toUserEntity();
        List<UserDto> users = getUsers();
        if (users.size() == 0){
            return userService.save(new UserDto(user));
        }
        for (UserDto u:users) {
            boolean existingUser = user.equalsUsername(u.toUserEntity());
            if (existingUser == true) {
                if (user.checkPassword(u.toUserEntity()) == true){
                    System.out.println("Passwort richtig!");
                    return userDto;
                }else {
                    System.out.println("Passwort falsch!");
                }
            } else {
                return userService.save(new UserDto(user));
            }
        }
        throw new ResponseStatusException(HttpStatus.UNAUTHORIZED, "Anmeldeinformationen ungültig");
    }

    @GetMapping("users")
    public List<UserDto> getUsers() {
        return userService.findAll();
    }

    @GetMapping("user/{id}")
    public UserDto getUser(@PathVariable String id) {
        UserDto userDTO = userService.findOne(id);
        if (userDTO == null) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "User nicht gefunden");;
        return userDTO;
    }
    @GetMapping("/users/{userId}/chats")
    public List<ChatEntity> getChatsByUser(@PathVariable String userId) {
        System.out.println(userService.getChatsByUser(userId));
        return userService.getChatsByUser(userId);
    }

    @GetMapping("/users/{userId}/chat/{chatId}")
    public ChatEntity getChatByUser(@PathVariable String userId, @PathVariable String chatId) {
        System.out.println(userService.getChatByUser(userId, chatId));
        return userService.getChatByUser(userId, chatId);
    }

    @DeleteMapping("user/{id}")
    public Long deleteUser(@PathVariable String id) {
        return userService.delete(id);
    }

    @PutMapping("user")
    public UserDto putUser(@RequestBody UserDto userDTO) {
        return userService.update(userDTO);
    }

    @PutMapping("addChat")
    @ResponseBody
    public void addChat(@RequestParam String userId, @RequestParam String chatName, @RequestParam String receiver) {
        userService.updateChats(getUser(userId), chatName, receiver);
    }

    @PutMapping("addMsg")
    @ResponseBody
    public void addMsg(@RequestParam String id, @RequestParam String chatname, @RequestParam String msg, @RequestParam String receiver) {
        sendMsg(receiver, msg, chatname);
        userService.updateMsg(getUser(id), chatname, msg, new SimpleDateFormat("dd-MM-yyyy HH:mm").format(new Date()), false);
    }
    @ResponseBody
    public void sendMsg(String id, String msg, String chatname) {
        UserDto user = getUser(id);
        List<ChatEntity> chats = user.toUserEntity().getChats();
        for (ChatEntity c:chats) {
            if(c.getBezeichnung().equals(chatname)){
                userService.updateMsg(user, chatname, msg, new SimpleDateFormat("dd-MM-yyyy HH:mm").format(new Date()), true);
            }
        }
    }


    @ExceptionHandler(RuntimeException.class)
    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    public final Exception handleAllExceptions(RuntimeException e) {
        LOGGER.error("Internal server error.", e);
        return e;
    }
}
