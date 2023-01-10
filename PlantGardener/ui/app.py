import PySimpleGUI as sg
import ctypes
import platform

__version__ = "v0.1"

sg.theme("DarkBlue")  # Add a touch of color
# All the stuff inside your window.

import layout.upload_layout as upload


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
        sg.InputText(),
    ],
    [
        sg.Text("Server Secret Key:  "),
        sg.InputText(),
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

    if event == "Upload":
        print("HI")

    if event in ("Cancel", sg.WIN_CLOSED):
        break
    # window["-FOLDERNAME-"].hide_row()

    elif event == "Clear History":
        print("CLEAR")

    if values["upload_mode"]:
        print("upload_mode")
        window["-UPLOAD_LAYOUT-"].unhide_row()
        window["-UPDATE_LAYOUT-"].hide_row()
        window["-GET_LAYOUT-"].hide_row()
        window["-DELETE_LAYOUT-"].hide_row()

    elif values["update_mode"]:
        print("update_mode")
        window["-UPLOAD_LAYOUT-"].hide_row()
        window["-UPDATE_LAYOUT-"].unhide_row()
        window["-GET_LAYOUT-"].hide_row()
        window["-DELETE_LAYOUT-"].hide_row()

        window["-UPDATE_LAYOUT-"]._visible = True

    elif values["get_mode"]:
        print("get_mode")
        window["-UPLOAD_LAYOUT-"].hide_row()
        window["-UPDATE_LAYOUT-"].hide_row()
        window["-GET_LAYOUT-"].unhide_row()
        window["-DELETE_LAYOUT-"].hide_row()

    elif values["delete_mode"]:
        print("delete_mode")
        window["-UPLOAD_LAYOUT-"].hide_row()
        window["-UPDATE_LAYOUT-"].hide_row()
        window["-GET_LAYOUT-"].hide_row()
        window["-DELETE_LAYOUT-"].unhide_row()

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
