There are three distinct namespaces here: Creator, Compositor, and Specifications

The Creator namespace provides the mechanisms for a user to modify their Yinglet.
This includes setting sliders, colors, toggles, etc...

The Compositor namespace applies those modifications to visuals.
This includes updating the rig, meshes, materials, and textures

The Specifications namespace includes shared data entities that bridge the gap between creation and composition

Right now, a lot of this logic is exclusively in the CharacterCompositor namespace.
I will be working on breaking it apart
