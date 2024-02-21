from pyzbar.pyzbar import decode
from PIL import Image
import requests, cv2

IP="http://localhost:5000"

CONST_VIDEO_PATH = 'video.mp4'

class Doors:
    
    @staticmethod
    def read_qr_code_enter_door(qr_content):
        response = requests.get(f"{IP}/enter/{qr_content}")

        if response.status_code == 200:
            data = response.json()
            
            if(data['success'] == True):
                #AQUI EJECUTARIAMOS PETICION DE APERTURA DE PUERTA
                pass
            return data
        else:
            return f"Error: {response.status_code}"

    @staticmethod
    def read_qr_code_exit_door(qr_content):
        response = requests.get(f"{IP}/exit/{qr_content}")

        if response.status_code == 200:
            data = response.json()
            
            if(data['success'] == True):
                #AQUI EJECUTARIAMOS PETICION DE APERTURA DE PUERTA
                pass
            return data
        else:
            return f"Error: {response.status_code}"


def draw_rectangle_qr(frame, points):
    if len(points) > 4:
        hull = cv2.convexHull([point for point in points])
        points = hull
            
    for j in range(len(points)):
        cv2.line(frame, tuple(points[j]), tuple(points[(j + 1) % len(points)]), (0, 255, 0), 3)

def calculate_scale(text, target_width, font=cv2.FONT_HERSHEY_SIMPLEX, thickness=1):
    text_size = cv2.getTextSize(text, font, 1, thickness)[0]
    text_width = text_size[0]

    scale = target_width / text_width

    return scale

def write_around_object(frame, label, colour, points, above):
    scale = calculate_scale(label, points[1][0] - points[0][0])

    text_size = cv2.getTextSize(label, cv2.FONT_HERSHEY_SIMPLEX, scale, 1)[0]

    if(above):
        x_label = int(((points[2][0] + points[3][0])/2) - (text_size[0] / 2))
        y_label = int(((points[2][1] + points[3][1])/2) - (text_size[1]))
    else:
        x_label = int(((points[1][0] + points[0][0])/2) - (text_size[0] / 2))
        y_label = int(((points[1][1] + points[0][1])/2) + (text_size[1]))

    cv2.putText(frame, label, (x_label, y_label), cv2.FONT_HERSHEY_SIMPLEX, scale, colour, 1)

def process_frame_enter(frame, frame_count, fps, limitacion_uso):
    decoded_data = decode(frame)

    if decoded_data:
        qr_content = decoded_data[0].data.decode('utf-8')
        label = qr_content
        points = decoded_data[0].polygon

        draw_rectangle_qr(frame, points)
        
        write_around_object(frame, label, (0, 0, 255), points, False)

        if(frame_count > limitacion_uso[1]):
            response = Doors.read_qr_code_enter_door(qr_content)

            if(response['success']):
                limitacion_uso = [True, frame_count + (6*fps)]
                write_around_object(frame, f"PUERTA ABIERTA: {round((limitacion_uso[1]/fps) - (frame_count/fps), 3)}s", (0, 0, 255), points, True)
            else:
                limitacion_uso = [False, frame_count + (6*fps)]
                write_around_object(frame, f"DENEGADO, REINTENTO EN: {round((limitacion_uso[1]/fps) - (frame_count/fps), 3)}s", (0, 0, 255), points, True)
            
        else:
            if(limitacion_uso[0] is True):
                write_around_object(frame, f"PUERTA ABIERTA: {round((limitacion_uso[1]/fps) - (frame_count/fps), 3)}s", (0, 0, 255), points, True)
            else:
                write_around_object(frame, f"DENEGADO, REINTENTO EN: {round((limitacion_uso[1]/fps) - (frame_count/fps), 3)}s", (0, 0, 255), points, True)

    cv2.imshow('Frame', frame)
    cv2.waitKey(1)

    return limitacion_uso

def puerta_entrada():
    cap = cv2.VideoCapture(CONST_VIDEO_PATH)
    fps = cap.get(cv2.CAP_PROP_FPS)
    frame_count = 0
    limitacion_uso = [0, False]

    if not cap.isOpened():
        print("Error: No se pudo abrir el video.")
        exit()

    while cap.isOpened():
        ret, frame = cap.read()

        if not ret:
            print("Fin del video")
            break

        frame_count += 1
        limitacion_uso = process_frame_enter(frame, frame_count, fps, limitacion_uso)

    cap.release()
    cv2.destroyAllWindows()

def process_frame_exit(frame, frame_count, fps, limitacion_uso):
    decoded_data = decode(frame)

    if decoded_data:
        qr_content = decoded_data[0].data.decode('utf-8')
        label = qr_content
        points = decoded_data[0].polygon

        draw_rectangle_qr(frame, points)
        
        write_around_object(frame, label, (0, 0, 255), points, False)

        if(frame_count > limitacion_uso[1]):
            response = Doors.read_qr_code_exit_door(qr_content)

            if(response['success']):
                limitacion_uso = [True, frame_count + (6*fps)]
                write_around_object(frame, f"PUERTA ABIERTA: {round((limitacion_uso[1]/fps) - (frame_count/fps), 3)}s", (0, 0, 255), points, True)
            else:
                limitacion_uso = [False, frame_count + (6*fps)]
                write_around_object(frame, f"DENEGADO, REINTENTO EN: {round((limitacion_uso[1]/fps) - (frame_count/fps), 3)}s", (0, 0, 255), points, True)
            
        else:
            if(limitacion_uso[0] is True):
                write_around_object(frame, f"PUERTA ABIERTA: {round((limitacion_uso[1]/fps) - (frame_count/fps), 3)}s", (0, 0, 255), points, True)
            else:
                write_around_object(frame, f"DENEGADO, REINTENTO EN: {round((limitacion_uso[1]/fps) - (frame_count/fps), 3)}s", (0, 0, 255), points, True)

    cv2.imshow('Frame', frame)
    cv2.waitKey(1)

    return limitacion_uso

def puerta_salida():
    cap = cv2.VideoCapture(CONST_VIDEO_PATH)
    fps = cap.get(cv2.CAP_PROP_FPS)
    frame_count = 0
    limitacion_uso = [0, False]

    if not cap.isOpened():
        print("Error: No se pudo abrir el video.")
        exit()

    while cap.isOpened():
        ret, frame = cap.read()

        if not ret:
            print("Fin del video")
            break

        frame_count += 1
        limitacion_uso = process_frame_exit(frame, frame_count, fps, limitacion_uso)

    cap.release()
    cv2.destroyAllWindows()