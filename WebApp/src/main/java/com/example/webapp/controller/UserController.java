package com.example.webapp.controller;

import com.example.webapp.dtos.UserDto;
import com.example.webapp.models.ChatEntity;
import com.example.webapp.models.MessageEntity;
import com.example.webapp.models.UserEntity;
import com.example.webapp.services.UserService;
import lombok.extern.slf4j.Slf4j;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.simp.SimpMessagingTemplate;
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
    @Autowired
    private SimpMessagingTemplate messagingTemplate;

    //Konstruktor
    public UserController(UserService userService) {

        this.userService = userService;
    }

    //Passwort ueberpruefen und User erstellen/zurueckgeben
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
                    System.out.println("Passwort richtig!");
                    return u;
                }else {
                    System.out.println("Passwort falsch!");
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
    @GetMapping("user/{id}")
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
        System.out.println(userService.getChatByUser(userId, chatId));
        return userService.getChatByUser(userId, chatId);
    }

    //alle Nachrichten eines Chats aus der DB holen
    @GetMapping("/users/{userId}/messages/{chatId}")
    public MessageEntity getMsgsByChat(@PathVariable String userId, @PathVariable String chatId) {
        System.out.println(userService.getMsgsByChat(chatId));
        return userService.getMsgsByChat(chatId);
    }

    //Chat hinzufuegen und Client ueber WS benachrichtigen
    @PutMapping("addChat")
    @ResponseBody
    @MessageMapping("/chat")
    public void addChat(@RequestParam String userId, @RequestParam String chatName, @RequestParam String receiver) {
        userService.updateChats(getUser(userId), chatName, receiver);
        messagingTemplate.convertAndSend("/topic/loadChat", chatName);
    }


    //Nachricht hinzufuegen und zum Empfaenger senden
    @PutMapping("addMsg")
    @ResponseBody
    public void addMsg(@RequestParam String id, @RequestParam String chatname, @RequestParam String msg, @RequestParam String receiver) {
        sendMsg(receiver, msg, chatname);
        userService.updateMsg(getUser(id), chatname, msg, new SimpleDateFormat("dd-MM-yyyy HH:mm").format(new Date()), false);
    }

    //Nachricht beim Empfaenger hinzufuegen und Client ueber WS benachrichtigen
    @ResponseBody
    @MessageMapping("/loadChat")
    public void sendMsg(String id, String msg, String chatname) {
        messagingTemplate.convertAndSend("/topic/loadChat", msg);
        UserDto user = getUser(id);
        List<ChatEntity> chats = user.toUserEntity().getChats();
        for (ChatEntity c:chats) {
            if(c.getBezeichnung().equals(chatname)){
                userService.updateMsg(user, chatname, msg, new SimpleDateFormat("dd-MM-yyyy HH:mm").format(new Date()), true);
            }
        }
    }

    //Exception Handler
    @ExceptionHandler(RuntimeException.class)
    @ResponseStatus(HttpStatus.INTERNAL_SERVER_ERROR)
    public final Exception handleAllExceptions(RuntimeException e) {
        LOGGER.error("Internal server error.", e);
        return e;
    }
}
