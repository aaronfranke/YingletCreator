import bpy
import os

def export():
    # export
    selectedObjects = bpy.context.selected_objects.copy()
    
    for selectedObject in selectedObjects:
        selectedObject.select_set(False)
    
    for selectedObject in selectedObjects:
        exportPath = os.path.dirname(bpy.data.filepath)
        exportPath += "/../Assets/Art/Models/"

        # The model's path should be based on the collections it is under
        collections = selectedObject.users_collection
        if collections is None:
            collections = []
        for collection in collections:
            exportPath += collection.name + "/"
            
        os.makedirs(exportPath, exist_ok=True)

        # exportPath += bpy.path.basename(bpy.context.blend_data.filepath).split(".")[0]
        # exportPath += "/"
        exportPath += selectedObject.name
        exportPath += ".fbx"
        exportPath = os.path.abspath(exportPath)
        
        
        selectedObject.select_set(True)
        bpy.ops.export_scene.fbx(filepath = exportPath, use_selection = True, axis_forward = 'Z', axis_up = 'Y', bake_space_transform=True, apply_scale_options='FBX_SCALE_ALL')
        selectedObject.select_set(False)
    
    for selectedObject in selectedObjects:
        selectedObject.select_set(True)
    
export()