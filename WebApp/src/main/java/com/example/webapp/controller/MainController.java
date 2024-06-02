package com.example.webapp.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/app")
public class MainController {

    //Loginpage zurückgeben
    @GetMapping("/Login")
    public String login(){
        return "loginPage";
    }

    //Mainpage zurückgeben
    @GetMapping("/mainPage")
    public String mainPage(){
        return "mainPage";
    }
}
