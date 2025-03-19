import bpy
import os

def export():
    # export
    selectedObjects = bpy.context.selected_objects.copy()
    
    for selectedObject in selectedObjects:
        selectedObject.select_set(False)
        
    exportObjects = []
    for obj in selectedObjects:
        # Never export a mesh without its parent
        if obj.type == 'MESH' and obj.parent and obj.parent.type == 'ARMATURE':
            exportObjects.append(obj.parent)
        else:
            exportObjects.append(obj)
    # Make unique
    exportObjects = list(set(exportObjects)) 
    
    for selectedObject in exportObjects:
        exportPath = os.path.dirname(bpy.data.filepath)
        exportPath += "/../Assets/Art/Models"

        # The model's path should be based on the collections it is under
        exportPath += get_collection_path(selectedObject.name) + "/"
        
        print(exportPath)
            
        os.makedirs(exportPath, exist_ok=True)

        # exportPath += bpy.path.basename(bpy.context.blend_data.filepath).split(".")[0]
        # exportPath += "/"
        exportPath += selectedObject.name
        exportPath += ".fbx"
        exportPath = os.path.abspath(exportPath)
        
        if (selectedObject.type == 'ARMATURE'):
             obj.animation_data.action = bpy.data.actions.get("__T-Pose")
        
        select_children_recursive(selectedObject, True)
        bpy.ops.export_scene.fbx(
            filepath = exportPath,
            use_selection = True,
            axis_forward = 'Z',
            axis_up = 'Y',
            bake_space_transform=True,
            apply_scale_options='FBX_SCALE_ALL',
            use_mesh_modifiers=True,  # Ensure the final deformed shape is exported
            object_types={'MESH', 'ARMATURE'},  # Include only mesh and armature
            use_armature_deform_only=True,  # Export only bones used for deformation
            add_leaf_bones=False,  # Prevents extra bones from being added
        )
        select_children_recursive(selectedObject, False)
        
    
    for selectedObject in selectedObjects:
        selectedObject.select_set(True)
        
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
    
export()
