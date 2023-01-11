import PySimpleGUI as sg
import json
import ctypes
import platform

__version__ = "v0.1"

sg.theme("DarkBlue")  # Add a touch of color
# All the stuff inside your window.

import layout.upload_layout as upload
from app.ImageUploader import ImageUploader


def __get_metadata(meta_path: str) -> str:
    metadata = ""

    if meta_path:
        with open(meta_path) as fd:
            metadata = json.dumps(json.load(fd))

    return metadata


def handle_submit(event, values) -> tuple[bool, str]:

    if not values["-access_key-"]:
        return [False, "No Server Access Key!"]
    if not values["-secret_key-"]:
        return [False, "No Server Secret Key!"]
    if not values["-name-"]:
        return [False, "No Image Name!"]
    if not values["-width-"]:
        return [False, "No Image Width!"]

    if values["image_mode"]:
        if not values["-image_path-"]:
            return [False, "No Image-Path!"]

        uploader = ImageUploader(values["-access_key-"], values["-secret_key-"])
        metadata = __get_metadata(values["-meta_path-"])

        uploader.upload_image(
            values["-image_path-"], values["-name-"], values["-width-"], metadata
        )

    elif values["folder_mode"]:
        if not values["-folder_path-"]:
            return [False, "No Folder-Path!"]

        uploader = ImageUploader(values["-access_key-"], values["-secret_key-"])
        metadata = __get_metadata(values["-meta_path-"])

        uploader.upload_images_from_folder(
            values["-folder_path-"], values["-name-"], values["-width-"], metadata
        )

    elif values["video_mode"]:
        if not values["-video_path-"]:
            return [False, "No Video-Path!"]

        uploader = ImageUploader(values["-access_key-"], values["-secret_key-"])
        metadata = __get_metadata(values["-meta_path-"])

        number_of_images = 30

        uploader.upload_video(
            values["-video_path-"],
            values["-name-"],
            values["-width-"],
            number_of_images,
            metadata,
        )

    return [True, "\nTask finished!\n\n"]


update_layout = [
    [sg.T("Update", font="_ 16")],
    [sg.T("Not implemented")],
]

get_layout = [
    [sg.T("Get", font="_ 16")],
    [sg.T("Not implemented")],
]

delete_layout = [
    [sg.T("Delete", font="_ 16")],
    [sg.T("Not implemented")],
]


input_layout = [
    [
        sg.Text("Server Access Key: "),
        sg.InputText(k="-access_key-"),
    ],
    [
        sg.Text("Server Secret Key:  "),
        sg.InputText(k="-secret_key-"),
    ],
    [
        sg.Text("Mode:                    "),
        sg.R(
            "Upload",
            enable_events=True,
            group_id="mode",
            k="upload_mode",
            default=True,
            disabled=True,
        ),
        sg.R(
            "Update",
            enable_events=True,
            group_id="mode",
            k="update_mode",
            disabled=True,
        ),
        sg.R("Get", enable_events=True, group_id="mode", k="get_mode", disabled=True),
        sg.R(
            "Delete",
            enable_events=True,
            group_id="mode",
            k="delete_mode",
            disabled=True,
        ),
    ],
    [sg.HSeparator()],
    [sg.Col(upload.layout, k="-UPLOAD_LAYOUT-", element_justification="l")],
    [
        sg.Col(
            update_layout, k="-UPDATE_LAYOUT-", element_justification="l", visible=False
        )
    ],
    [sg.Col(get_layout, k="-GET_LAYOUT-", element_justification="l", visible=False)],
    [
        sg.Col(
            delete_layout, k="-DELETE_LAYOUT-", element_justification="l", visible=False
        )
    ],
]

output_layout = [
    [
        sg.Text("Output: ", font="_ 16"),
    ],
    [
        sg.Multiline(
            size=(70, 21),
            write_only=False,
            expand_x=True,
            expand_y=True,
            reroute_stdout=True,
            echo_stdout_stderr=True,
            reroute_cprint=True,
            autoscroll=True,
            background_color="grey",
            text_color="black",
        )
    ],
]

layout = [
    [sg.T(f"Plantgardener {__version__}", font="DEFAULT 25")],
    [
        sg.Pane(
            [
                sg.Column(
                    input_layout,
                    element_justification="l",  # elements are left
                    expand_x=True,
                    expand_y=True,
                ),
                # sg.VSeperator(), # TODO: somehow not working
                sg.Column(
                    output_layout,
                    element_justification="c",
                    expand_x=True,
                    expand_y=True,
                ),
            ],
            orientation="h",
            relief=sg.RELIEF_SUNKEN,
            expand_x=True,
            expand_y=True,
            k="-PANE-",
        )
    ],
]
window = sg.Window("Plantgardener", layout)

while True:
    event, values = window.read()

    window["-UPLOAD_IMAGE_LAYOUT-"].update(visible=True)
    window["-UPLOAD_FOLDER_LAYOUT-"].update(visible=True)
    window["-UPLOAD_VIDEO_LAYOUT-"].update(visible=True)

    if event == "Upload":
        print("HI")

    if event in ("Cancel", sg.WIN_CLOSED):
        break
    # window["-FOLDERNAME-"].hide_row()

    elif event == "-submit-":
        handle_submit(event, values)

    if values["image_mode"]:
        window["-UPLOAD_IMAGE_LAYOUT-"].unhide_row()
        window["-UPLOAD_FOLDER_LAYOUT-"].hide_row()
        window["-UPLOAD_VIDEO_LAYOUT-"].hide_row()

        window["-UPLOAD_INPUT_LAYOUT-"].hide_row()
        window["-UPLOAD_INPUT_LAYOUT-"].unhide_row()

    elif values["folder_mode"]:
        window["-UPLOAD_IMAGE_LAYOUT-"].hide_row()
        window["-UPLOAD_FOLDER_LAYOUT-"].unhide_row()
        window["-UPLOAD_VIDEO_LAYOUT-"].hide_row()

        window["-UPLOAD_INPUT_LAYOUT-"].hide_row()
        window["-UPLOAD_INPUT_LAYOUT-"].unhide_row()

    elif values["video_mode"]:
        window["-UPLOAD_IMAGE_LAYOUT-"].hide_row()
        window["-UPLOAD_FOLDER_LAYOUT-"].hide_row()
        window["-UPLOAD_VIDEO_LAYOUT-"].unhide_row()

        window["-UPLOAD_INPUT_LAYOUT-"].hide_row()
        window["-UPLOAD_INPUT_LAYOUT-"].unhide_row()


def make_dpi_aware():
    if int(platform.release()) >= 8:
        ctypes.windll.shcore.SetProcessDpiAwareness(True)


make_dpi_aware()


window.close()
