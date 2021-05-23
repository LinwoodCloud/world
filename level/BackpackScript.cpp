#include "BackpackScript.hpp"

void BackpackScript::_register_methods() {
    register_method("_gui_input", &BackpackScript::_gui_input);
}

void BackpackScript::_gui_input(godot::Ref<godot::InputEvent> event) {
    if (event->is_action_pressed("backpack")) {
        set_visible(!is_visible());
    }
}

