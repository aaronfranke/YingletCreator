import bpy
import os

default_ying_object_filter = [
    "Antenna",
    "Body",
    "BodyArms",
    "BodyLegs",
    "Boobs",
    "Ears",
    "Eye_Left",
    "Eye_Right",
    "Fluff_Chest_Boobs",
    "Fluff_Crotch",
    "Fluff_Elbow",
    "Fluff_Knee",
    "Fluff_Neck",
    "Fluff_Shoulder",
    "Head",
    "MouthInterior",
    "ShellTooth",
    "Tongue",
    "Whiskers",
]

def export():
    selectedObjects = bpy.context.selected_objects.copy()
    
    for selectedObject in selectedObjects:
        selectedObject.select_set(False)
        
    exportObjects = []
    for obj in selectedObjects:
        # Never export a mesh without its parent, and always make sure that parent is visible
        if obj.type == 'MESH' and obj.parent and obj.parent.type == 'ARMATURE':
            obj.parent.hide_set(False)
            obj.parent.hide_viewport = False
            exportObjects.append(obj.parent)
        else:
            exportObjects.append(obj)
    # Make unique test
    exportObjects = list(set(exportObjects)) 
    
    for selectedObject in exportObjects:
        exportPath = os.path.dirname(bpy.data.filepath)
        exportPath += "/../Assets/Art/Models"

        # The model's path should be based on the collections it is under
        exportPath += get_collection_path(selectedObject.name) + "/"
            
        os.makedirs(exportPath, exist_ok=True)
        
        selection_name = selectedObject.name
        file_name = bpy.path.basename(bpy.context.blend_data.filepath).split(".")[0]
        
        # If it's the yinglet rig, there's a bunch of files exporting this object
        # Instead, export by the filename
        if (selection_name == "YingletRig"):
            exportPath += file_name
        else:
            exportPath += selection_name
        
        
        if (selectedObject.type == 'ARMATURE'):
            selectedObject.animation_data.action = bpy.data.actions.get("_T-Pose")
            for child in selectedObject.children:
                 # Don't export objects that are just used for building other meshes
                hidden = ("(PreApply)" in child.name) or ("(Ignore)" in child.name) 
                 
                # If this is a yinglet file that isn't the base one, filter base assets
                if file_name.startswith("Yinglet") and file_name != "Yinglet-Base":
                    if child.name in default_ying_object_filter:
                        hidden = True
                
                # Apply hidden
                child.hide_set(hidden)  # Visible in viewport
                child.hide_viewport = hidden  # Visible in viewport
        
        select_children_recursive(selectedObject, True)
        export_fbx(exportPath, False)
        select_children_recursive(selectedObject, False)

def export_fbx(exportPath, animsOnly):
    exportPath += ".fbx"
    exportPath = os.path.abspath(exportPath)
    object_types = {'MESH', 'ARMATURE'}
    if (animsOnly):
        object_types = {'ARMATURE'}
    
    bpy.ops.export_scene.fbx(
        filepath = exportPath,
        use_selection = True,
        axis_forward = 'Z',
        axis_up = 'Y',
        bake_space_transform=True,
        apply_scale_options='FBX_SCALE_ALL',
        use_mesh_modifiers=True,  # Apply mirror; this prevents shape key usage. It might be possible to get this working with https://github.com/smokejohn/SKkeeper/tree/master
        object_types=object_types,
        bake_anim=animsOnly,
        use_armature_deform_only=True,  # Export only bones used for deformation
        add_leaf_bones=False,  # Prevents extra bones from being added
        bake_anim_step = 1, # (Default 1) - How often frames are sampled
        bake_anim_simplify_factor = .45 # (Default 1) - The lower the value is, the higher the filesize. Feet also shuffle with this though
    )

def select_children_recursive(obj, doSelect):
    obj.select_set(doSelect)
    for child in obj.children:
        select_children_recursive(child, doSelect)
        

def get_collection_path(obj_name):
    """Returns the relative collection path of an object based on its hierarchy."""
    # Check which collection the object belongs to
    for collection in bpy.data.collections:
        child_collection_names = [child_collection.name for child_collection in collection.children]
        if obj_name in collection.objects or obj_name in child_collection_names:
            return get_collection_path(collection.name) + "/" + collection.name
    
    return "" 
    

# These operations are somewhat destructive (changing the selection, unhiding content, changing the animation)
# Rather than trying to undo things ourselves, just revert the file back to what it was right before we applied this
bpy.ops.wm.save_mainfile()
export()

bpy.ops.wm.revert_mainfile()

