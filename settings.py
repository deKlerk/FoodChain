# settings.py

def get_settings(name):
    # retrieve specific data from settings.json
    import os.path as path
    import json
    
    path_settings = 'AppData\Roaming\Grasshopper\Libraries\Food_Chain\settings.json'
    path_user = path.expanduser('~')
    path_full = path.join(path_user, path_settings)
    
    if path.exists(path_full):
        with open(path_full, 'r') as settings:
            data = json.loads(settings.read())
            #version = data['version']
            #namespaces = data['namespaces']
            return data[str(name)]
    else: return None
