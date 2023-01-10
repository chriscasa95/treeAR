import cv2
import base64
import json
import os
import sys
import numpy as np

from cv2 import Mat
from TargetAPI import TargetAPI

MEGABYTE = 1024 * 1024


class ImageUploader(TargetAPI):
    def upload_image(self, img_path: str, img_name: str, width: str):
        img = cv2.imread(img_path)

        img_compressed = self.__compress_image(img)

        html_body = self.__generate_body(img_compressed, img_name, width)

        self._post(html_body)

        print(f"uploaded: {img_name}")

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

    def __get_jpg_byte_size(self, image: Mat) -> int:

        return sys.getsizeof(cv2.imencode(".jpg", image)[1])

    def __compress_image(self, image: Mat) -> Mat:

        img_byte_size = self.__get_jpg_byte_size(image)
        scale_percent = 80  # percent of original size

        while img_byte_size >= 1 * MEGABYTE:

            width = int(image.shape[1] * scale_percent / 100)
            height = int(image.shape[0] * scale_percent / 100)
            dim = (width, height)

            # resize image
            image = cv2.resize(image, dim, interpolation=cv2.INTER_AREA)
            img_byte_size = self.__get_jpg_byte_size(image)

        print(f"new size: {img_byte_size}")

        return image

    def __generate_body(self, image: Mat, name: str, width: float) -> str:
        jpg_as_b64 = base64.b64encode(cv2.imencode(".jpg", image)[1]).decode()

        dictonary = {"name": name, "width": width, "image": jpg_as_b64}

        return json.dumps(dictonary, ensure_ascii=False, indent=4)


if __name__ == "__main__":

    server_access_key = "eb861e363ecf1563a824b290dd2e32b633d9d7b3"
    server_secret_key = "0aba77815d86e9861597d6226b4c2f70493891db"
    img_path = "./testleaf.jpg"
    img_path = "./IMG_20221206_141113.jpg"

    uploader = ImageUploader(server_access_key, server_secret_key)

    uploader.upload_image(img_path, "Test", 1)
