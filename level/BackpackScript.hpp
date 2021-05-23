#pragma once

#include <Godot.hpp>

#include <InputEvent.hpp>
#include <WindowDialog.hpp>

class BackpackScript : public godot::WindowDialog {
    GODOT_CLASS(BackpackScript, godot::WindowDialog);

public:
    static void _register_methods();

    void _init() {}

    void _gui_input(const godot::Ref<godot::InputEvent> event);
};

