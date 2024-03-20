package com.example.webapp.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;


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
