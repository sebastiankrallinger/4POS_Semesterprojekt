package com.example.webapp.controller;

import com.example.webapp.dtos.UserDto;
import com.example.webapp.models.UserEntity;
import com.example.webapp.services.UserService;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonDeserializer;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.annotation.JsonDeserialize;
import com.sun.tools.javac.Main;
import org.bson.types.ObjectId;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@Controller
@RequestMapping("/app")
public class MainController {
    @GetMapping("/Login")
    public String login(){
        return "loginPage";
    }

    @GetMapping("/mainPage")
    public String mainPage(){
        return "mainPage";
    }
}
