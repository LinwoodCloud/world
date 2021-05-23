#pragma once

#include <Godot.hpp>

#include <Control.hpp>
#include <OS.hpp>
#include <Button.hpp>
#include <Ref.hpp>

class MainMenuScript : public godot::Control {
    GODOT_CLASS(MainMenuScript, godot::Control);
    
    godot::Button *continue_button;

    void open(godot::String uri);
    
public:
    static void _register_methods();

    void _init() {}

    void _ready();
    void on_create_button_pressed();
    void on_news_button_pressed();
    void on_source_button_pressed();
    void on_website_button_pressed();
    void on_mods_button_pressed();
    void on_quit_button_pressed();
};
