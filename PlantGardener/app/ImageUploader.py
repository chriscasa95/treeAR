import cv2
import base64
import json
import os
import sys

from cv2 import Mat
from TargetAPI import TargetAPI

MEGABYTE = 1024 * 1024


class ImageUploader(TargetAPI):
    def upload_image(self, img_path: str, img_name: str, width: str):
        img = cv2.imread(img_path)

        img_compressed = self.__compress_image(img)

        html_body = self.__generate_body(img_compressed, img_name, width)

        # self._post(html_body)

    def upload_multiple_images(self, img_paths: list[str]):
        pass

    def upload_video(self, video_path: str):
        pass

    def upload_images_from_folder(self, folder_path: str):
        pass

    def get_image_information(self, target_id: str):
        pass

    def update_image(self, target_id: str, image_path: str):
        pass

    def delete_image(self, target_id: str):
        pass

    def __compress_image(self, image, img_path) -> Mat:
        img_byte_size = os.path.getsize(img_path)

        buffer = cv2.imencode(".jpg", image)[1]

        print(f"imgsize path: {img_byte_size}")
        print(f"imgsize cv2: {sys.getsizeof(buffer)}")

        if img_byte_size >= MEGABYTE:
            pass

        pass

    def __generate_body(self, image: Mat, name: str, width: float) -> str:
        jpg_as_b64 = base64.b64encode(cv2.imencode(".jpg", image)[1]).decode()

        dictonary = {"name": name, "width": width, "image": jpg_as_b64}

        return json.dumps(dictonary, ensure_ascii=False, indent=4)


if __name__ == "__main__":

    server_access_key = "eb861e363ecf1563a824b290dd2e32b633d9d7b3"
    server_secret_key = "0aba77815d86e9861597d6226b4c2f70493891db"
    img_path = "./testleaf.jpg"

    uploader = ImageUploader(server_access_key, server_secret_key)

    uploader.upload_image(img_path, "Test", 1)
