import PySimpleGUI as sg

__image_layout = [
    [sg.T("Select Image:")],
    [
        sg.In(sg.user_settings_get_entry("-editor program-", ""), k="-EDITOR PROGRAM-"),
        sg.FileBrowse(file_types=(("img", ".jpg .JPG"),)),
    ],
]


__folder_layout = [
    [sg.T("Select Folder:")],
    [
        sg.Combo(
            sorted(sg.user_settings_get_entry("-folder names-", [])),
            # default_value=sg.user_settings_get_entry("-demos folder-", "get_demo_path"),
            size=(44, 1),
            key="-FOLDERNAME-",
        ),
        sg.FolderBrowse("Browse", target="-FOLDERNAME-"),
    ],
]


__video_layout = [
    [sg.T("Select Video:")],
    [
        sg.In(sg.user_settings_get_entry("-editor program-", ""), k="-EDITOR PROGRAM-"),
        sg.FileBrowse(file_types=(("video", ".mp4 .MP4"),)),
    ],
]


input_layout = [
    [sg.T("Select Metadata:")],
    [
        sg.In(sg.user_settings_get_entry("-explorer program-"), k="-EXPLORER PROGRAM-"),
        sg.FileBrowse(file_types=(("metadata", "*.json *.JSON"),)),
    ],
    [
        sg.Text("Name: "),
    ],
    [
        sg.InputText(),
    ],
    [
        sg.Text("Width: "),
    ],
    [
        sg.InputText(default_text=1),
    ],
]


layout = [
    [sg.T("Upload", font="_ 16")],
    [
        sg.R(
            "Image",
            enable_events=True,
            group_id="upload_mode",
            k="image_mode",
            default=True,
        ),
        sg.R("Folder", enable_events=True, group_id="upload_mode", k="folder_mode"),
        sg.R("Video", enable_events=True, group_id="upload_mode", k="video_mode"),
    ],
    [sg.Col(__image_layout, k="-UPLOAD_IMAGE_LAYOUT-")],
    [sg.Col(__folder_layout, k="-UPLOAD_FOLDER_LAYOUT-")],
    [sg.Col(__video_layout, k="-UPLOAD_VIDEO_LAYOUT-")],
    [sg.Col(input_layout, k="-UPLOAD_INPUT_LAYOUT-")],
]
