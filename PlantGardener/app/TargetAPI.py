import requests
import hmac

from hashlib import sha1, md5
from email.utils import formatdate
from requests import Response


class TargetAPI:
    def __init__(self, server_access_key: str, server_secret_key: str) -> None:
        self.__url = "https://vws.vuforia.com/targets"
        self.__access_key = server_access_key
        self.__server_secret = server_secret_key

    def get(self, target_id: str):
        header = self.__header("GET", target_id=target_id)

        r = requests.get(url=self.__url, headers=header)

    def post(self, body: str):
        header = self.__header("POST", body=body)

        r = requests.post(url=self.__url, headers=header, json=body)

    def put(self, body: str):
        header = self.__header("PUT", body=body)

        r = requests.put(url=self.__url, headers=header, json=body)

    def delete(self, target_id: str):
        header = self.__header("DELETE", target_id=target_id)

        r = requests.delete(url=self.__url, headers=header)

    #######################################################################

    def __header(self, http_verb: str, target_id: str = "", body: str = "") -> dict:
        # RFC 1123 Date - example: Tue, 03 Jan 2023 13:43:38 GMT
        rfc_1123_date = formatdate(timeval=None, localtime=False, usegmt=True)

        content_type = "application/json"
        target_path = self.__url

        if http_verb == "GET" or "DELETE":  # == GET || POST
            content_type = ""
            target_path = f"{self.__url}/{target_id}"

        vws_auth = self.__vws_auth(
            http_verb, content_type, rfc_1123_date, target_path, body
        )

        return {
            "Host": target_path,
            "Date": rfc_1123_date,
            "Authorization": vws_auth,
            "Content-Type": content_type,
        }

    def __vws_auth(
        self,
        http_verb: str,
        content_type: str,
        rfc_1123_date: str,
        target_path: str,
        body: str,
    ):
        # md5 body hash
        content_md5_hash = md5(bytearray(body))

        sign_body = f'{http_verb} + "\n" + {content_md5_hash} + "\n" + {content_type} + "\n" + {rfc_1123_date} + "\n" + {target_path}'

        # OAuth HMAC-SHA1
        hash = hmac.new(bytearray(self.__server_secret), bytearray(sign_body), sha1)

        # Encode to base64
        hash_base64 = hash.digest().encode("base64")  # .rstrip('\n')

        return f"VWS {self.__access_key}:{hash_base64}"

    def __respose(r: Response):
        pass
