#include "MainMenuScript.hpp"

#include <OS.hpp>
#include <SceneTree.hpp>
#include <WindowDialog.hpp>

void MainMenuScript::_register_methods()
{
    register_method("_ready", &MainMenuScript::_ready);
    register_method("on_create_button_pressed", &MainMenuScript::on_create_button_pressed);
    register_method("on_news_button_pressed", &MainMenuScript::on_news_button_pressed);
    register_method("on_source_button_pressed", &MainMenuScript::on_source_button_pressed);
    register_method("on_website_button_pressed", &MainMenuScript::on_website_button_pressed);
    register_method("on_mods_button_pressed", &MainMenuScript::on_mods_button_pressed);
    register_method("on_quit_button_pressed", &MainMenuScript::on_quit_button_pressed);
}

void MainMenuScript::open(godot::String uri)
{
    godot::OS::get_singleton()->shell_open(uri);
}

void MainMenuScript::_ready()
{
    continue_button = get_node<godot::Button>("Panel/ScrollContainer/VBoxContainer/ContinueButton");
}

void MainMenuScript::on_create_button_pressed()
{
    // TODO: change to a direct reference, requires LoadingScreen to be C++
    auto loader = get_node/*<LoadingScreen>*/("/root/LoadingScreen");
    loader->call("Load", "res://level/Main_Scene.tscn");
}

void MainMenuScript::on_news_button_pressed()
{
    open("https://linwood.tk/blog");
}

void MainMenuScript::on_source_button_pressed()
{
    open("https://github.com/LinwoodCloud/world");
}

void MainMenuScript::on_website_button_pressed()
{
    open("https://linwood.tk/docs/world/overview");
}

void MainMenuScript::on_mods_button_pressed()
{
    get_node<godot::WindowDialog>("ModsDialog")->popup();
}

void MainMenuScript::on_quit_button_pressed()
{
    get_tree()->quit();
}








