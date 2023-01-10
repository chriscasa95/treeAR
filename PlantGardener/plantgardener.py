from app.ImageUploader import ImageUploader

server_access_key = "eb861e363ecf1563a824b290dd2e32b633d9d7b3"
server_secret_key = "0aba77815d86e9861597d6226b4c2f70493891db"

video_path = "./data/VID_20230110_104750.mp4"
plant_name = "TestPlant"
width = 2
number_of_images = 30

uploader = ImageUploader(server_access_key, server_secret_key)

uploader.upload_video(video_path, plant_name, width, number_of_images)
