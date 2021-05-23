#include "level/BackpackScript.hpp"
#include "main/MainMenuScript.hpp"

#include <Godot.hpp>

/** GDNative Initialize **/
extern "C" void GDN_EXPORT godot_linwoodworld_gdnative_init(godot_gdnative_init_options *o) {
    godot::Godot::gdnative_init(o);
}

/** GDNative Terminate **/
extern "C" void GDN_EXPORT godot_linwoodworld_gdnative_terminate(godot_gdnative_terminate_options *o) {
    godot::Godot::gdnative_terminate(o);
}

/** NativeScript Initialize **/
extern "C" void GDN_EXPORT godot_linwoodworld_nativescript_init(void *handle) {
    godot::Godot::nativescript_init(handle);

    godot::register_class<BackpackScript>();
    godot::register_class<MainMenuScript>();
}

