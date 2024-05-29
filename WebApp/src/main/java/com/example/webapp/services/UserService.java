package com.example.webapp.services;

import com.example.webapp.dtos.UserDto;
import com.example.webapp.models.ChatEntity;
import com.example.webapp.models.MessageEntity;
import com.example.webapp.models.UserEntity;
import com.example.webapp.repositories.UserRepository;
import org.springframework.stereotype.Service;
import java.util.ArrayList;
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
    public List<UserDto> findAll() {
        return userRepository.findAll().stream().map(UserDto::new).toList();
    }


    @Override
    public UserDto findOne(String id) {
        return new UserDto(userRepository.findOne(id));
    }

    public UserDto getUserId(String username) {
        return new UserDto(userRepository.getUserId(username));
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

    public MessageEntity getMsgsByChat(String chatId){
        return null;
    }

    public UserDto updateChats(UserDto sender, String chatName, String receiver){
        UserEntity entity_sender = sender.toUserEntity();
        List<ChatEntity> chats_sender = entity_sender.getChats();
        UserEntity entity_receiver = getUserId(receiver).toUserEntity();

        List<ChatEntity> chat_receiver = entity_receiver.getChats();

        if (chats_sender != null){
            chats_sender.add(new ChatEntity(chatName, null, entity_receiver.getId().toString(), false));
        }else {
            chats_sender = new ArrayList<>();
            chats_sender.add(new ChatEntity(chatName, null, entity_receiver.getId().toString(), false));
        }
        if (chat_receiver != null){
            chat_receiver.add(new ChatEntity(chatName, null, entity_sender.getId().toString(), false));
        }else {
            chat_receiver = new ArrayList<>();
            chat_receiver.add(new ChatEntity(chatName, null, entity_sender.getId().toString(), false));
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

    public UserDto updateStatus(UserDto userDto, String chat){
        UserEntity entity = userDto.toUserEntity();
        List<ChatEntity> chats = entity.getChats();
        for (ChatEntity c:chats) {
            if (c.getBezeichnung().equals(chat)){
                c.setNewMsg(false);
            }
        }
        return new UserDto(userRepository.update(entity));
    }
}
