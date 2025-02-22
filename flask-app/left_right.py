import cv2
import mediapipe as mp
import time
import numpy as np
from PIL import Image, ImageDraw, ImageFont

# Constants
IMAGE_SIZE = (300, 280)  # Širina 300px, visina 280px
CHECK_INTERVAL = 3  # sekundi
FONT_PATH = "DejaVuSans.ttf"
FONT_SIZE = 46
IMAGE_PATHS = {
    'level1': {'left': "Slike/Bike.jpg", 'right': "Slike/Car.jpg"},
    'level2': {'left': "Slike/Cutting.jpeg", 'right': "Slike/Seeding.jpg"},
    'level3': {'left': "Slike/Paper_bag.jpg", 'right': "Slike/Plastic_bag.jpg"}
}

# Initialize MediaPipe Face Mesh
mp_face_mesh = mp.solutions.face_mesh
face_mesh = mp_face_mesh.FaceMesh()

# Initialize camera
cap = cv2.VideoCapture(0)


def check_face_position(frame_width, landmarks):
    """Provjeri da li je lice potpuno na lijevoj ili desnoj strani"""
    frame_center = frame_width // 2
    all_left = all(lm.x * frame_width < frame_center for lm in landmarks)
    all_right = all(lm.x * frame_width >= frame_center for lm in landmarks)
    return all_left, all_right


def load_images():
    """Učitaj i promijeni veličinu svih slika za igru"""
    images = {}
    for level in IMAGE_PATHS:
        images[level] = {
            'left': cv2.resize(cv2.imread(IMAGE_PATHS[level]['left']), IMAGE_SIZE),
            'right': cv2.resize(cv2.imread(IMAGE_PATHS[level]['right']), IMAGE_SIZE)
        }
    return images


class GameState:
    def __init__(self):
        self.score = 0
        self.current_level = 1
        self.last_check_time = time.time()
        self.images = load_images()
        self.font = ImageFont.truetype(FONT_PATH, FONT_SIZE)

    def get_current_level_key(self):
        return f'level{self.current_level}'

    def draw_text(self, frame, text, position, color):
        """Napiši tekst na sliku koristeći PIL za podršku Unicode znakova"""
        pil_img = Image.fromarray(cv2.cvtColor(frame, cv2.COLOR_BGR2RGB))
        draw = ImageDraw.Draw(pil_img)
        draw.text(position, text, font=self.font, fill=color)
        return cv2.cvtColor(np.array(pil_img), cv2.COLOR_RGB2BGR)

    def handle_level_transition(self, frame, all_left, all_right):
        """Upravljaj logikom igre za trenutni nivo"""
        frame_center = frame.shape[1] // 2
        level_rules = {
            1: {'correct': 'left'},
            2: {'correct': 'right'},
            3: {'correct': 'left'}
        }

        current_rule = level_rules[self.current_level]
        response_text, response_color = ("Netačan odgovor!", (255, 0, 0))

        if all_left and current_rule['correct'] == 'left':
            self.score += 1
            response_text, response_color = ("Tačan odgovor!", (0, 255, 0))
        elif all_right and current_rule['correct'] == 'right':
            self.score += 1
            response_text, response_color = ("Tačan odgovor!", (0, 255, 0))
        elif not all_left and not all_right:
            response_text = "Moraš se odlučiti!"

        frame = self.draw_text(frame, response_text,
                             (frame_center - 200, 50), response_color)
        cv2.imshow('Interactive Game', frame)
        cv2.waitKey(3000)

        if self.current_level < 3:
            self.current_level += 1
        else:
            print(f"Final score: {self.score}")
            exit()


def main():
    game = GameState()

    while True:
        ret, frame = cap.read()
        if not ret:
            break

        # Zrcali sliku i pretvori je u RGB
        frame = cv2.flip(frame, 1)
        rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

        # Procesiraj sliku sa Face Mesh
        results = face_mesh.process(rgb_frame)

        if results.multi_face_landmarks:
            face_landmarks = results.multi_face_landmarks[0]
            landmarks = face_landmarks.landmark

            if time.time() - game.last_check_time >= CHECK_INTERVAL:
                all_left, all_right = check_face_position(frame.shape[1], landmarks)
                game.handle_level_transition(frame, all_left, all_right)
                game.last_check_time = time.time()

            # Nacrtaj landmarkove lica
            for lm in landmarks:
                x = int(lm.x * frame.shape[1])
                y = int(lm.y * frame.shape[0])
                cv2.circle(frame, (x, y), 1, (0, 255, 0), -1)

        # Prikaži slike za trenutni nivo
        level_key = game.get_current_level_key()
        y_offset = 73
        frame_center = frame.shape[1] // 2
        x_left = (frame_center // 2) - (IMAGE_SIZE[0] // 2)  # Centrirano za širinu 300px
        x_right = (frame_center + frame_center // 2) - (IMAGE_SIZE[0] // 2)  # Centrirano za širinu 300px

        frame[y_offset:y_offset+IMAGE_SIZE[1], x_left:x_left+IMAGE_SIZE[0]] = game.images[level_key]['left']
        frame[y_offset:y_offset+IMAGE_SIZE[1], x_right:x_right+IMAGE_SIZE[0]] = game.images[level_key]['right']

        # Nacrtaj podjelu ekrana i rezultat
        cv2.line(frame, (frame_center, 0), (frame_center, frame.shape[0]), (255, 0, 0), 2)
        frame = game.draw_text(frame, f'Score: {game.score}', (10, 30), (0, 255, 0))
        cv2.namedWindow('Interactive Game', cv2.WND_PROP_FULLSCREEN)
        cv2.setWindowProperty('Interactive Game', cv2.WND_PROP_FULLSCREEN, cv2.WINDOW_FULLSCREEN)
        cv2.imshow('Interactive Game', frame)

        if cv2.waitKey(5) & 0xFF == ord('q'):
            break

        api_url = f'http://localhost:5024/api/Score/increase-score/{sys.argv[1]}/{game.score}'

        # Pozivanje API-ja sa PUT metodom
        response = requests.put(api_url)

        # Provera odgovora od API-ja
        if response.status_code == 200:
         print(f"Uspešno upisan rezultat za korisnika {sys.argv[1]}")
        else:
         print(f"Došlo je do greške pri upisu rezultata. Status kod: {response.status_code}")
    cap.release()
    cv2.destroyAllWindows()
    print(f"Final score: {game.score}")


if __name__ == "__main__":
    main()