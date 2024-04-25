package com.example.webapp.services;

import com.example.webapp.dtos.UserDto;
import com.example.webapp.models.ChatEntity;
import com.example.webapp.models.MessageEntity;
import com.example.webapp.models.UserEntity;
import com.example.webapp.repositories.UserRepository;
import org.apache.catalina.User;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.bind.annotation.PathVariable;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Optional;

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

    public List<ChatEntity> getChatsByUser(String userId){
        UserEntity user = userRepository.findOne(userId);
        return user.getChats();
    }

    public ChatEntity getChatByUser(String userId, String chatId){
        UserEntity user = userRepository.findOne(userId);
        List<ChatEntity> chats = user.getChats();
        for (ChatEntity c:chats) {
            if (chatId.equals(c.getBezeichnung())){
                return c;
            }
        }
        return null;
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

    public UserDto updateChats(UserDto sender, String chatName){
        UserEntity entity_sender = sender.toUserEntity();
        List<ChatEntity> chats_sender = entity_sender.getChats();

        String receiver = getChatByUser(entity_sender.getId().toString(), chatName).getReceiver();
        UserEntity entity_receiver = findOne(receiver).toUserEntity();

        List<ChatEntity> chat_receiver = entity_receiver.getChats();

        if (chats_sender != null){
            chats_sender.add(new ChatEntity(chatName, null, entity_receiver.getId().toString()));
        }else {
            chats_sender = new ArrayList<>();
            chats_sender.add(new ChatEntity(chatName, null, entity_receiver.getId().toString()));
        }
        if (chat_receiver != null){
            chat_receiver.add(new ChatEntity(chatName, null, entity_sender.getId().toString()));
        }else {
            chats_sender = new ArrayList<>();
            chats_sender.add(new ChatEntity(chatName, null, entity_sender.getId().toString()));
        }
        entity_sender.setChats(chats_sender);
        entity_receiver.setChats(chat_receiver);
        userRepository.update(entity_receiver);
        return new UserDto(userRepository.update(entity_sender));
    }

    public UserDto updateMsg(UserDto userDto, String chat, String msg, String date, boolean receiver){
        UserEntity entity = userDto.toUserEntity();
        List<ChatEntity> chats = entity.getChats();
        for (ChatEntity c:chats) {
            if (c.getBezeichnung().equals(chat)){
                List<MessageEntity> msgs = c.getMessages();
                if (msgs == null){
                    msgs = new ArrayList<>();
                }
                msgs.add(new MessageEntity(msg,  receiver, date));
                c.setMessages(msgs);
            }
        }
        return new UserDto(userRepository.update(entity));
    }
}
