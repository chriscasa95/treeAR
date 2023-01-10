import PySimpleGUI as sg
import ctypes
import platform

sg.theme("DarkBlue")  # Add a touch of color
# All the stuff inside your window.

upload_image_layout = [
    [sg.T("Select Image:")],
    [
        sg.In(sg.user_settings_get_entry("-editor program-", ""), k="-EDITOR PROGRAM-"),
        sg.FileBrowse(file_types=(("img", ".jpg .JPG"),)),
    ],
]

upload_folder_layout = [
    [
        sg.Combo(
            sorted(sg.user_settings_get_entry("-folder names-", [])),
            default_value=sg.user_settings_get_entry("-demos folder-", "get_demo_path"),
            size=(50, 1),
            key="-FOLDERNAME-",
        ),
        sg.FolderBrowse("Folder Browse", target="-FOLDERNAME-"),
    ],
]

upload_video_layout = [
    [sg.T("Select Video:")],
    [
        sg.In(sg.user_settings_get_entry("-editor program-", ""), k="-EDITOR PROGRAM-"),
        sg.FileBrowse(),
    ],
]

upload_input_layout = [
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


upload_layout = [
    [sg.T("Upload", font="_ 16")],
    [
        sg.R("Image", group_id="upload_mode", default=True),
        sg.R("Folder", group_id="upload_mode"),
        sg.R("Video", group_id="upload_mode"),
    ],
    [sg.Col(upload_image_layout)],
    # [sg.Col(upload_folder_layout)],
    # [sg.Col(upload_video_layout)],
    [sg.Col(upload_input_layout)],
]


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
        sg.R("Upload", group_id="mode", default=True),
        sg.R("Update", group_id="mode"),
        sg.R("Get", group_id="mode"),
        sg.R("Delete", group_id="mode"),
    ],
    [sg.HSeparator()],
    [sg.Col(upload_layout)],
    # [sg.Col(update_layout)],
    # [sg.Col(get_layout)],
    # [sg.Col(delete_layout)],
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
        )
    ],
]

layout = [
    [sg.T("Plantgardener", font="DEFAULT 25")],
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
    # if event == "Ok":
    #     sg.user_settings_set_entry("-demos folder-", values["-FOLDERNAME-"])
    #     sg.user_settings_set_entry("-editor program-", values["-EDITOR PROGRAM-"])
    #     sg.user_settings_set_entry("-theme-", values["-THEME-"])
    #     sg.user_settings_set_entry(
    #         "-folder names-",
    #         list(
    #             set(
    #                 sg.user_settings_get_entry("-folder names-", [])
    #                 + [
    #                     values["-FOLDERNAME-"],
    #                 ]
    #             )
    #         ),
    #     )
    #     sg.user_settings_set_entry("-explorer program-", values["-EXPLORER PROGRAM-"])
    #     sg.user_settings_set_entry("-advanced mode-", values["-ADVANCED MODE-"])
    #     sg.user_settings_set_entry("-dclick runs-", values["-DCLICK RUNS-"])
    #     sg.user_settings_set_entry("-dclick edits-", values["-DCLICK EDITS-"])
    #     sg.user_settings_set_entry("-dclick nothing-", values["-DCLICK NONE-"])
    #     print("HI")
    #     break
    elif event == "Clear History":
        print("CLEAR")

        # sg.user_settings_set_entry("-folder names-", [])
        # sg.user_settings_set_entry("-last filename-", "")
        # window["-FOLDERNAME-"].update(values=[], value="")
        # window["-FOLDERNAME-"].hide_row()


def make_dpi_aware():
    if int(platform.release()) >= 8:
        ctypes.windll.shcore.SetProcessDpiAwareness(True)


make_dpi_aware()


window.close()
