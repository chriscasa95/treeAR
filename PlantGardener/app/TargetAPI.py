import requests
import hmac
import base64

from hashlib import sha1, md5
from email.utils import formatdate
from requests import Response


class TargetAPI:
    def __init__(self, server_access_key: str, server_secret_key: str) -> None:
        self.__host = "vws.vuforia.com"
        self.__request_path = "/targets"
        self.__access_key = server_access_key
        self.__server_secret = server_secret_key

    def _get(self, target_id: str):
        header = self.__header("GET", target_id=target_id)

        response = requests.get(url=self.__url(target_id), headers=header)
        self.__check_respose(response)

    def _post(self, body: str):
        header = self.__header("POST", body=body)

        response = requests.post(url=self.__url(), headers=header, data=body)
        self.__check_respose(response)

    def _put(
        self,
        targrt_id: str,
        body: str,
    ):
        header = self.__header("PUT", target_id=targrt_id, body=body)

        response = requests.put(url=self.__url(targrt_id), headers=header, data=body)
        self.__check_respose(response)

    def _delete(self, target_id: str):
        header = self.__header("DELETE", target_id=target_id)

        response = requests.delete(url=self.__url(target_id), headers=header)
        self.__check_respose(response)

    #######################################################################

    def __url(self, target_id: str = "") -> str:
        if target_id:
            return f"https://{self.__host}{self.__request_path}/{target_id}"

        return f"https://{self.__host}{self.__request_path}"

    def __header(self, http_verb: str, target_id: str = "", body: str = "") -> dict:
        # RFC 1123 Date - example: Tue, 03 Jan 2023 13:43:38 GMT
        rfc_1123_date = formatdate(timeval=None, localtime=False, usegmt=True)

        content_type = ""
        request_path = self.__request_path

        if target_id:
            request_path = f"{request_path}/{target_id}"

        if body:
            content_type = "application/json"

        vws_auth = self.__vws_auth(
            http_verb, content_type, rfc_1123_date, request_path, body
        )

        return {
            "Host": self.__host,
            "Date": rfc_1123_date,
            "Authorization": vws_auth,
            "Content-Type": content_type,
        }

    def __vws_auth(
        self,
        http_verb: str,
        content_type: str,
        rfc_1123_date: str,
        request_path: str,
        body: str,
    ) -> str:
        # md5 body hash
        content_md5_hash = md5(body.encode()).hexdigest()

        # create siganture body: https://library.vuforia.com/web-api/vuforia-web-api-authentication#vws-authentication
        sign_body = f"{http_verb}\n{content_md5_hash}\n{content_type}\n{rfc_1123_date}\n{request_path}"

        # OAuth HMAC-SHA1
        hmac_hash = hmac.new(self.__server_secret.encode(), sign_body.encode(), sha1)

        # Encode to base64
        hash_base64 = base64.b64encode(hmac_hash.digest()).decode()

        return f"VWS {self.__access_key}:{hash_base64}"

    def __check_respose(self, r: Response):
        print("\n\n###################################################################")
        print(r.status_code)
        print(r.headers)
        print(r.content)

        print("\n\n###################################################################")
        print(r.request.url)
        print(r.request.headers)
        # print(r.request.body)


if __name__ == "__main__":

    import cv2
    import os
    import base64
    import json

    server_access_key = "eb861e363ecf1563a824b290dd2e32b633d9d7b3"
    server_secret_key = "0aba77815d86e9861597d6226b4c2f70493891db"

    api = TargetAPI(server_access_key, server_secret_key)

    img_path = "./testleaf.jpg"

    # Using cv2.imread() method
    img = cv2.imread(img_path)

    img_byte_size = os.path.getsize(img_path)

    print(img_byte_size)

    jpg_as_b64 = base64.b64encode(cv2.imencode(".jpg", img)[1]).decode()

    dictonary = {"name": "PlantGardener2", "width": 2, "image": jpg_as_b64}

    json_obj = json.dumps(dictonary, ensure_ascii=False, indent=4)

    target_id = "3d2af960e5e64b869228012571f11d76"

    # api.put(target_id, json_obj)

    api.get(target_id)

    # api.post(json_obj)

    # api.delete(target_id)

    # print(json_obj)

    # Displaying the image
    # cv2.imshow("image", img)
