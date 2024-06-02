package com.example.webapp.controller;

import com.example.webapp.WebSocketHandler;
import com.example.webapp.dtos.UserDto;
import com.example.webapp.models.ChatEntity;
import com.example.webapp.models.MessageEntity;
import com.example.webapp.models.UserEntity;
import com.example.webapp.services.UserService;
import lombok.extern.slf4j.Slf4j;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.autoconfigure.security.SecurityProperties;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

@Slf4j
@Controller
@RestController
@RequestMapping("/app")
public class UserController {
    private final static Logger LOGGER = LoggerFactory.getLogger(MainController.class);
    private final UserService userService;
    private final WebSocketHandler webSocketHandler;

    @Autowired
    public UserController(UserService userService, WebSocketHandler webSocketHandler) {

        this.userService = userService;
        this.webSocketHandler = webSocketHandler;
    }

    //Passwort überprüfen und User erstellen/zurückgeben
    @PostMapping("user")
    @ResponseStatus(HttpStatus.CREATED)
    public UserDto postUser(@RequestBody UserDto userDto) {
        UserEntity user = userDto.toUserEntity();
        List<UserDto> users = getUsers();
        boolean existingUser = false;

        if (users.size() == 0){
            return userService.save(new UserDto(user));
        }
        for (UserDto u:users) {
            existingUser = user.equalsUsername(u.toUserEntity());
            if (existingUser == true) {
                if (user.checkPassword(u.toUserEntity()) == true){
                    //System.out.println("Passwort richtig!");
                    return u;
                }else {
                    //System.out.println("Passwort falsch!");
                    break;
                }
            }
        }
        if (existingUser == false) {
            return userService.save(new UserDto(user));
        }
        throw new ResponseStatusException(HttpStatus.UNAUTHORIZED, "Anmeldeinformationen ungültig");
    }

    //alle User aus der DB holen
    @GetMapping("users")
    public List<UserDto> getUsers() {
        return userService.findAll();
    }

    //einen User aus der DB holen
    public UserDto getUser(@PathVariable String id) {
        UserDto userDTO = userService.findOne(id);
        if (userDTO == null) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "User nicht gefunden");;
        return userDTO;
    }

    //alle Chats eines Users aus der DB holen
    @GetMapping("/users/{userId}/chats")
    public List<ChatEntity> getChatsByUser(@PathVariable String userId) {
        //System.out.println(userService.getChatsByUser(userId));
        return userService.getChatsByUser(userId);
    }

    //Chat eines Users aus der DB holen
    @GetMapping("/users/{userId}/chat/{chatId}")
    public ChatEntity getChatByUser(@PathVariable String userId, @PathVariable String chatId) {
        //System.out.println(userService.getChatByUser(userId, chatId));
        return userService.getChatByUser(userId, chatId);
    }

    //Chat hinzufuegen und Client über WS benachrichtigen
    @PostMapping("addChat")
    @ResponseBody
    public void addChat(@RequestParam String userId, @RequestParam String chatName, @RequestParam String receiver) {
        userService.updateChats(getUser(userId), chatName, receiver);
    }

    //Nachricht hinzufügen und zum Empfänger senden
    @PostMapping("addMsg")
    @ResponseBody
    public void addMsg(@RequestParam String id, @RequestParam String chatname, @RequestParam String msg, @RequestParam String receiver) {
        sendMsg(receiver, msg, chatname);
        userService.updateMsg(getUser(id), chatname, msg, new SimpleDateFormat("dd-MM-yyyy HH:mm").format(new Date()), false);
    }

    //Nachricht beim Empfänger hinzufügen
    @ResponseBody
    public void sendMsg(String id, String msg, String chatname) {
        UserDto user = getUser(id);
        List<ChatEntity> chats = user.toUserEntity().getChats();
        for (ChatEntity c:chats) {
            if(c.getBezeichnung().equals(chatname)){
                c.setNewMsg(true);
                userService.updateMsg(user, chatname, msg, new SimpleDateFormat("dd-MM-yyyy HH:mm").format(new Date()), true);
            }
        }
    }

    //Chatstatus ändern
    @PostMapping("updateStaus")
    @ResponseBody
    public void updateStaus(@RequestParam String id, @RequestParam String chatname) {
        UserDto user = getUser(id);
        userService.updateStatus(user, chatname);

    }

    //Exception Handler
    @ExceptionHandler(RuntimeException.class)
    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    public final Exception handleAllExceptions(RuntimeException e) {
        LOGGER.error("Internal server error.", e);
        return e;
    }
}
